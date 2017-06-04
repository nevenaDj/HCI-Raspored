using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Raspored.Model
{
    public class Smer: INotifyPropertyChanged
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
        private DateTime _datumUvodjenja;
        private string _opis;

        public Smer()
        {
            _naziv = "";
            _opis = "";
            _skracenica = "";
            _oznaka = "";
            _datumUvodjenja = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            Predmeti = new ObservableCollection<Predmet>();

        }

        public Smer(string oznaka, string naziv, string skracenica, DateTime datumUvodjenja, string opis)
        {
            _oznaka = oznaka;
            _naziv = naziv;
            _skracenica = skracenica;
            _datumUvodjenja = datumUvodjenja;
            _opis = opis;
            Predmeti = new ObservableCollection<Predmet>();

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

        public DateTime DatumUvodjenja
        {
            get
            {
                return _datumUvodjenja;
            }
            set
            {
                if (_datumUvodjenja != value)
                {
                    _datumUvodjenja = value;
                    OnPropertyChanged("DatumUvodjenja");
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

        public ObservableCollection<Predmet> Predmeti
        {
            get;
            set;
        }
    }
}
