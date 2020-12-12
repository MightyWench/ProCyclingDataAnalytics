using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cycling1._1.ViewModels
{
    public class ShellViewModel : Screen
    {
        IWindowManager manager = new WindowManager();

        private ObservableCollection<Stageraces> _WTStageRacesWPF = new ObservableCollection<Stageraces>(Singleton_Class.ListofStageRaces);
        public ObservableCollection<Stageraces> WTStageRacesWPF
        {
            get { return _WTStageRacesWPF; }
            set { _WTStageRacesWPF = value; }
        }

        private Stageraces _StageRace; 

        public Stageraces StageRace
        {
            get { return _StageRace; }
            set { _StageRace = value; }
        }

        public void ViewTeams()
        {
            manager.ShowWindow(new TeamsViewModel(), null, null);
        }
    }
}
