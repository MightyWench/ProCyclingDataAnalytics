using Caliburn.Micro;
using Cycling1._1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cycling1._1
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            databaseimport.Teamslistdownloader();
            Initialize();
            
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
