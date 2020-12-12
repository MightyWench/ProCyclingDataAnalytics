using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cycling1._1.ViewModels
{
    class TeamsViewModel : Screen
    {
        private ObservableCollection<Teamdetails> _CyclingTeams = new ObservableCollection<Teamdetails>(Singleton_Class.InstancesofTeams);

        public ObservableCollection<Teamdetails> CyclingTeams
        {
            get { return _CyclingTeams; }
            set { _CyclingTeams = value; }
        }

        private Teamdetails _team;

        public Teamdetails Team
        {
            get { return _team; }
            set { _team = value;
                NotifyOfPropertyChange(() => Team);
            }
            
        }
    }
}
