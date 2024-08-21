using Caliburn.Micro;
using RetailDesktopUI.EventModels;
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
        private LoginViewModel _loginVM;
        private SalesViewModel _salesVM;
        private IEventAggregator _events;

        public ShellViewModel(IEventAggregator events, LoginViewModel loginVM, SalesViewModel salesVM)
        {
            _events = events;
            _loginVM = loginVM;
            _salesVM = salesVM;

            _events.SubscribeOnPublishedThread(this);
            ActivateItemAsync(_loginVM);

            //var context = SynchronizationContext.Current;
            //context.Send(async _ =>
            //{
            //    await ActivateItemAsync(_loginVM);
            //}, null);
        }

        async Task IHandle<LogOnEvent>.HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesVM);
        }
    }
}