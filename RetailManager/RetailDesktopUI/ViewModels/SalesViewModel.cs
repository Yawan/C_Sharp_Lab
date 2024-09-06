using AutoMapper;
using Caliburn.Micro;
using RetailDesktopUI.Library.Api;
using RetailDesktopUI.Library.Helpers;
using RetailDesktopUI.Library.Models;
using RetailDesktopUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IProductEndpoint _productEndpoint;
        private readonly ISaleEndpoint _saleEndpoint;
        private readonly IConfigHelper _configHelper;
        private readonly IMapper _mapper;

        private BindableCollection<CartItemDisplayModel> _cart = new BindableCollection<CartItemDisplayModel>();
        private BindingList<ProductDisplayModel> _products;

        private int _itemQuantity = 1;

        private ProductDisplayModel _selectedProduct;

        public SalesViewModel(IProductEndpoint productEndpoint, ISaleEndpoint saleEndpoint,
            IConfigHelper configHelper, IMapper mapper)
        {
            _productEndpoint = productEndpoint;
            _saleEndpoint = saleEndpoint;
            _configHelper = configHelper;
            _mapper = mapper;
        }

        public async Task LoadProducts()
        {
            var productsList = await _productEndpoint.GetAll();
            var products = _mapper.Map<List<ProductDisplayModel>>(productsList);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        public ProductDisplayModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;

                // Make sure something is selected
                // Make sure there is an item quantity
                if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }

                return output;
            }
        }

        public string SubTotal
        {
            get
            {
                return CalculateSubTotal().ToString("C");
            }
        }

        public string Tax
        {
            get
            {
                return CalculateTax().ToString("C");
            }
        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;

            foreach (var item in Cart)
            {
                subTotal += item.Product.RetailPrice * item.QuantityInCart;
            }
            return subTotal;
        }

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = _configHelper.GetTaxRate() / 100;

            taxAmount = Cart
                .Where(x => x.Product.IsTaxable)
                .Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

            return taxAmount;
        }

        public string Total
        {
            get
            {
                // TODO Replace with calculation
                return (CalculateSubTotal() + CalculateTax()).ToString("C");
            }
        }

        public BindableCollection<CartItemDisplayModel> Cart
        {
            get { return _cart; }
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public void AddToCart()
        {
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
            }
            else
            {
                CartItemDisplayModel item = new CartItemDisplayModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(item);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;

                // TODO Make sure something is selected

                return output;
            }
        }

        public void RemoveFromCart()
        {
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public bool CanCheckOut
        {
            get
            {
                // TODO Make sure there is something in the cart
                return Cart.Count > 0;
            }
        }

        public async Task CheckOut()
        {
            // Create a SaleModel and Post to the API
            SaleModel sale = new SaleModel();

            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            await _saleEndpoint.PostSale(sale);
        }
    }
}