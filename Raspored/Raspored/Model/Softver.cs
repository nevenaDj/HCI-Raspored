using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Raspored.Model
{
    public class Softver: INotifyPropertyChanged
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
        private string _proizvodjac;
        private string _sajt;
        private int _godinaIzdavanja;
        private double _cena;
        private string _opis;
        private string _sistem;
        private string _file;

        public Softver()
        {
            _oznaka = "";
            _naziv = "";
            _proizvodjac = "";
            _sajt = "";
            _godinaIzdavanja = 2017;
            Sistemi = new ObservableCollection<string>();
            Sistemi.Add("Windows");
            Sistemi.Add("Linux");
            Sistemi.Add("Cross-platform");
        }

        public Softver(string oznaka, string naziv, string proizvodjac,
            string sajt, int godinaIzdavanja, double cena, string opis, string sistem)
        {
            _oznaka = oznaka;
            _naziv = naziv;
            _proizvodjac = proizvodjac;
            _sajt = sajt;
            _godinaIzdavanja = godinaIzdavanja;
            _cena = cena;
            _opis = opis;
            _sistem = sistem;

            Sistemi = new ObservableCollection<string>();
            Sistemi.Add("Windows");
            Sistemi.Add("Linux");
            Sistemi.Add("Cross-platform");
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
       
       
        public string Proizvodjac
        {
            get
            {
                return _proizvodjac;
            }
            set
            {
                if (_proizvodjac != value)
                {
                    _proizvodjac = value;
                    OnPropertyChanged("Proizvodjac");
                }
            }
        }
        
        public string Sajt
        {
            get
            {
                return _sajt;
            }
            set
            {
                if (_sajt != value)
                {
                    _sajt = value;
                    OnPropertyChanged("Sajt");
                }
            }
        }
        
        public int GodinaIzdavanja
        {
            get
            {
                return _godinaIzdavanja;
            }
            set
            {
                if (_godinaIzdavanja != value)
                {
                    _godinaIzdavanja = value;
                    OnPropertyChanged("GodinaIzdavanja");
                }
            }
        }
        
        public double Cena
        {
            get
            {
                return _cena;
            }
            set
            {
                if (_cena != value)
                {
                    _cena = value;
                    OnPropertyChanged("Cena");
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

        public ObservableCollection<string> Sistemi
        {
            get;
            set;
        }

    }
}
