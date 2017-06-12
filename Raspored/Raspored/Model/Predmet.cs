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
        private string _skracenica;
        private Smer _smer;
        private string _opis;
        private int _velicinaGrupe;
        private int _duzinaTermina;
        private int _brojTermina;
        private bool _trebaProjektor;
        private bool _trebaTabla;
        private bool _trebaPametnaTabla;
        private Softver _softver;
        private string _sistem;
        private string _tabla;
        private string _projektor;
        private string _pametnaTabla;
        private string _file;

        public List<Softver> Softveri
        {
            get;
            set;
        }

        public Predmet()
        {
            Softveri = new List<Softver>();
            _oznaka = "";
            _opis = "";
            _naziv = "";
            _duzinaTermina = 2;
            _brojTermina = 4;
            _velicinaGrupe = 16;
            _trebaPametnaTabla = false;
            _trebaTabla = true;
            _trebaProjektor = true;
            _smer = null;
            _sistem = "";

            Sistemi = new ObservableCollection<string>();
            Sistemi.Add("Windows");
            Sistemi.Add("Linux");
            Sistemi.Add("Svejedno");
            if (_trebaProjektor)
            {
                Projektor = "Treba";
            }
            else
            {
                Projektor = "Ne treba";


            }
            if (_trebaTabla)
            {
                Tabla = "Treba";

            }
            else
            {
                Tabla = "Ne treba";
            }
            if (_trebaPametnaTabla)
            {
                PametnaTabla = "Treba";

            }
            else
            {
                PametnaTabla = "Ne treba";

            }

        }
        public ObservableCollection<string> Sistemi
        {
            get;
            set;
        }

        public Predmet(string oznaka, string naziv,string skracenica, Smer smer, 
            string opis,int velicinaGrupe,int duzinaTermina, int brojTermina,
            bool trebaProjektor, bool trebaTabla, bool trebaPametnaTabla,
            List<Softver> softveri, string oznakaSmera, string sistem)
        {
            _oznaka = oznaka;
            _naziv = naziv;
            _skracenica = skracenica;
            _smer = smer;
            _opis = opis;
            _velicinaGrupe = velicinaGrupe;
            _duzinaTermina = duzinaTermina;
            _brojTermina = brojTermina;
            _trebaProjektor = trebaProjektor;
            _trebaTabla = trebaTabla;
            _trebaPametnaTabla = trebaPametnaTabla;
           
            Softveri = softveri;
            _oznakaSmera = oznakaSmera;
            _sistem = sistem;

            Sistemi = new ObservableCollection<string>();
            Sistemi.Add("Windows");
            Sistemi.Add("Linux");
            Sistemi.Add("Svejedno");

            if (_trebaProjektor)
            {
                Projektor = "Treba";
            }else
            {
                Projektor = "Ne treba";


            }
            if (_trebaTabla)
            {
                Tabla = "Treba";

            }else
            {
                Tabla = "Ne treba";
            }
            if (_trebaPametnaTabla)
            {
                PametnaTabla = "Treba";

            }else
            {
                PametnaTabla = "Ne treba";

            }
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

        public string File
        {
            get
            {
                return _file;
            }
            set
            {
                if (_file != value)
                {
                    _file = value;
                    OnPropertyChanged("File");
                }
            }
        }

        public string Skracenica
        {
            get
            {
                return _skracenica;
            }
            set
            {
                if (_skracenica != value)
                {
                    _skracenica = value;
                    OnPropertyChanged("Skracenica");
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
                    if (_smer != null)
                    {
                        OznakaSmera = _smer.Oznaka;
                    }
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

        public int BrojTermina
        {
            get
            {
                return _brojTermina;
            }
            set
            {
                if (_brojTermina != value)
                {
                    _brojTermina = value;
                    OnPropertyChanged("BrojTermina");
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
                    if (_trebaProjektor)
                    {
                        Projektor = "Treba";
                    }else
                    {
                        Projektor = "Ne treba";

                    }
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
                    if (_trebaTabla)
                    {
                        Tabla = "Treba";
                    }else
                    {
                        Tabla = "Ne treba";
                    }
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
                    if (_trebaPametnaTabla)
                    {
                        PametnaTabla = "Treba";
                    }else
                    {
                        PametnaTabla = "Ne treba";
                    }
                    OnPropertyChanged("TrebaPametnaTabla");
                }
            }
        }
        
     

        public Softver Softver
        {
            get
            {
                return _softver;
            }
            set
            {
                if (_softver != value)
                {
                    _softver = value;
                    OnPropertyChanged("Softver");
                }
            }
        }

        public static implicit operator ObservableCollection<object>(Predmet v)
        {
            throw new NotImplementedException();
        }
        private string _oznakaSmera; 
        public string OznakaSmera
        {
            get
            {
                return _oznakaSmera;
            }
            set
            {
                if (_oznakaSmera != value)
                {
                    _oznakaSmera = value;
                    OnPropertyChanged("OznakaSmera");
                }
            }
        }

        public string Sistem
        {
            get
            {
                return _sistem;
            }
            set
            {
                if (_sistem != value)
                {
                    _sistem = value;
                    OnPropertyChanged("Sistem");
                }
            }
        }
        public string Tabla
        {
            get
            {
                return _tabla;
            }
            set
            {
                if (_tabla != value)
                {
                    _tabla = value;
                    OnPropertyChanged("Tabla");
                }
            }
        }
        public string Projektor
        {
            get
            {
                return _projektor;
            }
            set
            {
                if (_projektor != value)
                {
                    _projektor = value;
                    OnPropertyChanged("Projektor");
                }
            }
        }
        public string PametnaTabla
        {
            get
            {
                return _pametnaTabla;
            }
            set
            {
                if (_pametnaTabla != value)
                {
                    _pametnaTabla = value;
                    OnPropertyChanged("PametnaTabla");
                }
            }
        }
    }
}
