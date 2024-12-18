using Caliburn.Micro;
using RetailDesktopUI.EventModels;
using RetailDesktopUI.Library.Api;
using RetailDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//using System.Threading;

namespace RetailDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private SalesViewModel _salesVM;
        private IEventAggregator _events;

        private ILoggedInUserModel _user;
        private IAPIHelper _apiHelper;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM, ILoggedInUserModel user, IAPIHelper apiHelper)
        {
            _events = events;
            _salesVM = salesVM;
            _user = user;
            _apiHelper = apiHelper;

            _events.SubscribeOnPublishedThread(this);

            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }

        public bool IsLoggedIn
        {
            get
            {
                // if we are logged in, token is shouldn't be null or empty string
                return string.IsNullOrWhiteSpace(_user.Token) == false;
            }
        }

        public async Task LogOut()
        {
            _user.ResetUserModel();
            _apiHelper.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
            await _salesVM.ResetSalesViewModel();
        }

        public async Task UserManagement()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>());
        }

        public async Task SalesManagement()
        {
            await ActivateItemAsync(_salesVM);
        }

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesVM);
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void ExitApplication()
        {
            TryCloseAsync();
        }
    }
}