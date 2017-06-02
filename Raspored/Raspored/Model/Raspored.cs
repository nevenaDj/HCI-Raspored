using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspored.Model
{
    public class Raspored: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private string _file;
        private string _naziv;
        private List<Predmet> _ostali_termini;

        public List<UcionicaRaspored> Rasporedi
        {
            get;
            set;
        }

        public Raspored()
        {
            Rasporedi = new List<UcionicaRaspored>();
            _file = "";
            _naziv = "";
            _ostali_termini = new List<Predmet>();
        }

        public Raspored(string file, string naziv, List<Predmet> ostali, List<UcionicaRaspored> raspored)
        {
            _file = file;
            _naziv = naziv;
            Rasporedi = raspored;
            _ostali_termini = ostali;
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

        public List<Predmet> OstaliTermini
        {
            get
            {
                return _ostali_termini;
            }
            set
            {
                if (_ostali_termini != value)
                {
                    _ostali_termini = value;
                    OnPropertyChanged("Ostali Termini");
                }
            }
        }


    }
}
