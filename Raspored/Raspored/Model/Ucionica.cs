﻿using System;
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
        private bool _imaProjektor;
        private bool _imaTabla;
        private bool _imaPametnaTabla;
        private string _sistem;
        private string _projektor;
        private string _tabla;
        private string _pametnaTabla;
        private string _file;

        public List<Softver> Softveri
        {
            get;
            set;
        }

        public Ucionica()
        {
            Softveri = new List<Softver>();

            _oznaka = "";
            _opis = "";
            _imaPametnaTabla = false;
            _imaProjektor = true;
            _imaTabla = true;
            _brojRadnihMesta = 16;

            Sistemi = new ObservableCollection<string>();
            Sistemi.Add("Windows");
            Sistemi.Add("Linux");
            Sistemi.Add("Oba");

            if (_imaProjektor)
            {
                Projektor = "Ima";
            }
            else
            {
                Projektor = "Nema";
            }

            if (_imaTabla)
            {
                Tabla = "Ima";
            }
            else
            {
                Tabla = "Nema";
            }

            if (_imaPametnaTabla)
            {
                PametnaTabla = "Ima";
            }
            else
            {
                PametnaTabla = "Nema";
            }

        }

        public Ucionica(string oznaka, string opis, int brojRadnihMesta, 
            bool imaProjektor, bool imaTabla, bool imaPametnaTabla,List<Softver> softver, string sistem)
        {
            _oznaka = oznaka;
            _opis = opis;
            _brojRadnihMesta = brojRadnihMesta;
            _imaProjektor = imaProjektor;
            _imaTabla = imaTabla;
            _imaPametnaTabla = imaPametnaTabla;
            Softveri = softver;
            _sistem = sistem;
            if (_imaProjektor)
            {
                Projektor = "Ima";
            }else
            {
                Projektor = "Nema";
            }

            if (_imaTabla)
            {
                Tabla = "Ima";
            }else
            {
                Tabla = "Nema";
            }

            if (_imaPametnaTabla)
            {
                PametnaTabla = "Ima";
            }else
            {
                PametnaTabla = "Nema";
            }


            Sistemi = new ObservableCollection<string>();
            Sistemi.Add("Windows");
            Sistemi.Add("Linux");
            Sistemi.Add("Oba");
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

        public bool ImaProjektor
        {
            get
            {
                return _imaProjektor;
            }
            set
            {
                if (_imaProjektor != value)
                {
                    _imaProjektor = value;
                    if (_imaProjektor)
                    {
                        Projektor = "Ima";
                    }else
                    {
                        Projektor = "Nema";
                    }
                    OnPropertyChanged("ImaProjektor");
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
                    if (_imaTabla)
                    {
                        Tabla = "Ima";
                    }else
                    {
                        Tabla = "Nema";
                    }
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
                    if (_imaPametnaTabla)
                    {
                        PametnaTabla = "Ima";
                    }else
                    {
                        PametnaTabla = "Nema";
                    }
                    OnPropertyChanged("ImaPametnaTabla");
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
        public object OperativniSistem { get; internal set; }

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
