using Caliburn.Micro;
using RetailDesktopUI.Library.Api;
using RetailDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEndpoint _productEndpoint;
        private BindingList<ProductModel> _cart;
        private BindingList<ProductModel> _products;
        private int _itemQuantity;

        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
        }

        public async Task LoadProducts()
        {
            var productsList = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(productsList);
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        public string SubTotal
        {
            get
            {
                // TODO Replace with calculation
                return "$0.00";
            }
        }

        public string Tax
        {
            get
            {
                // TODO Replace with calculation
                return "$0.00";
            }
        }

        public string Total
        {
            get
            {
                // TODO Replace with calculation
                return "$0.00";
            }
        }

        public BindingList<ProductModel> Cart
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
            }
        }

        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;

                // TODO Make sure something is selected
                // TODO Make sure there is an item quantity

                return output;
            }
        }

        public void AddToCart()
        {
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
        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;

                // TODO Make sure there is something in the cart

                return output;
            }
        }

        public void CheckOut()
        {
        }
    }
}