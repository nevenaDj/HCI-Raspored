using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

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
        private DateTime _datumUvodjenja;
        private string _opis;

        public Smer()
        {

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
    }
}
