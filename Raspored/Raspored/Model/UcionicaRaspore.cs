using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspored.Model
{
    public class UcionicaRaspored : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private Ucionica _ucionica;

        public List<List<Predmet>> Rasporedi
        {
            get;
            set;
        }

        public UcionicaRaspored()
        {
            Rasporedi = new List<List<Predmet>>();
           // Rasporedi = new List<List<Predmet>>();
            for (int i = 0; i < 61; i++)
            {
                List<Predmet> novi_red = new List<Predmet>();
                for (int j = 0; j < 7; j++)
                {
                    Predmet p = new Predmet();
                    novi_red.Add(p);
                }
                Rasporedi.Add(novi_red);
            }
            _ucionica = new Ucionica();
        }

        public UcionicaRaspored(Ucionica u)
        {
            Rasporedi = new List<List<Predmet>>();
            for (int i = 0; i < 61; i++)
            {
                List<Predmet> novi_red = new List<Predmet>();
                for (int j = 0; j < 7; j++)
                {
                    Predmet p = new Predmet();
                    novi_red.Add(p);
                }
                Rasporedi.Add(novi_red);
            }
            _ucionica = u;
        }

        public UcionicaRaspored( Ucionica ucionica, List<List<Predmet>> raspored)
        {
            _ucionica=ucionica;
            Rasporedi = raspored;
        }

        public Ucionica Ucionica
        {
            get
            {
                return _ucionica;
            }
            set
            {
                if (_ucionica != value)
                {
                    _ucionica = value;
                    OnPropertyChanged("Ucionica");
                }
            }
        }

    }
}
