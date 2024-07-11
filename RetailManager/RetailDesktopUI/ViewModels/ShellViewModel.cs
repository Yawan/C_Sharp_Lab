using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using System.Threading;

namespace RetailDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        private LoginViewModel _loginVM;

        public ShellViewModel(LoginViewModel loginVM)
        {
            _loginVM = loginVM;
            ActivateItemAsync(_loginVM);

            //var context = SynchronizationContext.Current;
            //context.Send(async _ =>
            //{
            //    await ActivateItemAsync(_loginVM);
            //}, null);
        }
    }
}