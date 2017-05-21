using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Raspored.Model
{
    public class Predmet: INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private string _oznaka;
        private string _naziv;
        private Smer _smer;
        private string _opis;
        private int _velicinaGrupe;
        private int _duzinaTermina;
        private bool _trebaProjektor;
        private bool _trebaTabla;
        private bool _trebaPametnaTabla;
        private OS _neophodanOS;

        public ObservableCollection<Softver> Softveri
        {
            get;
            set;
        }

        public Predmet()
        {
            Softveri = new ObservableCollection<Softver>();

        }

        public string Oznaka
        {
            get
            {
                return _oznaka;
            }
            set
            {
                if (_oznaka != value)
                {
                    _oznaka = value;
                    OnPropertyChanged("Oznaka");
                }
            }
        }
        
        public string Naziv
        {
            get
            {
                return _naziv;
            }
            set
            {
                if (_naziv != value)
                {
                    _naziv = value;
                    OnPropertyChanged("Naziv");
                }
            }
        }
        
        public Smer SmerPredmeta
        {
            get
            {
                return _smer;
            }
            set
            {
                if (_smer != value)
                {
                    _smer = value;
                    OnPropertyChanged("SmerPredmeta");
                }
            }
        }
        
        public string Opis
        {
            get
            {
                return _opis;
            }
            set
            {
                if (_opis != value)
                {
                    _opis = value;
                    OnPropertyChanged("Opis");
                }
            }
        }
       
        public int VelicinaGrupe
        {
            get
            {
                return _velicinaGrupe;
            }
            set
            {
                if (_velicinaGrupe != value)
                {
                    _velicinaGrupe = value;
                    OnPropertyChanged("VelicinaGrupe");
                }
            }
        }
        
        public int DuzinaTermina
        {
            get
            {
                return _duzinaTermina;
            }
            set
            {
                if (_duzinaTermina != value)
                {
                    _duzinaTermina = value;
                    OnPropertyChanged("DuzinaTermina");
                }
            }
        }
        
        public bool TrebaProjektor
        {
            get
            {
                return _trebaProjektor;
            }
            set
            {
                if (_trebaProjektor != value)
                {
                    _trebaProjektor = value;
                    OnPropertyChanged("TrebaProjektor");
                }
            }
        }
        
        public bool TrebaTabla
        {
            get
            {
                return _trebaTabla;
            }
            set
            {
                if (_trebaTabla != value)
                {
                    _trebaTabla = value;
                    OnPropertyChanged("TrebaTabla");
                }
            }
        }
        
        public bool TrebaPametnaTabla
        {
            get
            {
                return _trebaPametnaTabla;
            }
            set
            {
                if (_trebaPametnaTabla != value)
                {
                    _trebaPametnaTabla = value;
                    OnPropertyChanged("TrebaPametnaTabla");
                }
            }
        }
        
        public OS NeophodanOS
        {
            get
            {
                return _neophodanOS;
            }
            set
            {
                if (_neophodanOS != value)
                {
                    _neophodanOS = value;
                    OnPropertyChanged("NeophodanOS");
                }
            }
        }
    }
}
