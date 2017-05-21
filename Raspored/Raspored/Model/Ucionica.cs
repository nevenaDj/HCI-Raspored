using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Raspored.Model
{
    public class Ucionica: INotifyPropertyChanged
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
        private string _opis;
        private int _brojRadnihMesta;
        private bool _imaTabla;
        private bool _imaPametnaTabla;
        private OS _operativniSistem;

        public ObservableCollection<Softver> Softveri
        {
            get;
            set;
        }

        public Ucionica()
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
        
        public int BrojRadnihMesta
        {
            get
            {
                return _brojRadnihMesta;
            }
            set
            {
                if (_brojRadnihMesta != value)
                {
                    _brojRadnihMesta = value;
                    OnPropertyChanged("BrojRadnihMesta");
                }
            }
        }
        
        public bool ImaTabla
        {
            get
            {
                return _imaTabla;
            }
            set
            {
                if (_imaTabla != value)
                {
                    _imaTabla = value;
                    OnPropertyChanged("ImaTabla");
                }
            }
        }
        
        public bool ImaPametnaTabla
        {
            get
            {
                return _imaPametnaTabla;
            }
            set
            {
                if (_imaPametnaTabla != value)
                {
                    _imaPametnaTabla = value;
                    OnPropertyChanged("ImaPametnaTabla");
                }
            }
        }
        
        public OS OperativniSistem
        {
            get
            {
                return _operativniSistem;
            }
            set
            {
                if (_operativniSistem != value)
                {
                    _operativniSistem = value;
                    OnPropertyChanged("OperativniSistem");
                }
            }
        } 
    }
}
