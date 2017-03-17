using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigAndLogCollector.Commands;

namespace ConfigAndLogCollector.ViewModel
{
    class ConfigAndLogCollectorViewModel :ViewModelBase
    {

        public RelayCommand GetDataCommand { get; set; }

        private int myVar = 10;
        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }




    }
}
