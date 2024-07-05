using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailDesktopUI.ViewModels
{
    public class ShellViewModel
    {
        public ICalculations _calculations;

        public ShellViewModel(ICalculations calculations)
        {
            _calculations = calculations;
            var result = calculations.Add(0.1, 0.2);
            Console.WriteLine(result);
        }
    }
}