using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Raspored.Model;

namespace Raspored.Tabele
{
    /// <summary>
    /// Interaction logic for Tabele.xaml
    /// </summary>
    public partial class Tabele : Window, INotifyPropertyChanged
    {
        public Tabele()
        {
            InitializeComponent();
            this.DataContext = this;

            List<Smer> s = new List<Smer>();
            List<Predmet> p = new List<Predmet>();
            List<Softver> sf = new List<Softver>();
            List<Ucionica> u = new List<Ucionica>();

            s.Add(new Smer { Naziv = "Softversko inzenjerstvo i informacione tehnologije", Skracenica ="SIIT", Oznaka="SW", DatumUvodjenja=new DateTime(), Opis = "Departman za racunarstvo"});
            s.Add(new Smer { Naziv = "Softversko inzenjerstvo i informacione tehnologije", Skracenica = "SIIT", Oznaka = "SW", DatumUvodjenja = new DateTime(), Opis = "Departman za racunarstvo" });
            s.Add(new Smer { Naziv = "Softversko inzenjerstvo i informacione tehnologije", Skracenica = "E2", Oznaka = "SW", DatumUvodjenja = new DateTime(), Opis = "Departman za racunarstvo" });
            Smerovi = new ObservableCollection<Smer>(s);

            p.Add(new Predmet { Naziv = "Interakcija covek racunar", Oznaka = "HCI", Skracenica = "HCI (SIIT)", DuzinaTermina = 2, BrojTermina = 6, VelicinaGrupe = 16, SmerPredmeta = s[0], TrebaTabla = true, TrebaPametnaTabla = false, TrebaProjektor = true });
            p.Add(new Predmet { Naziv = "Internet softverske arhitekture", Oznaka = "ISA", Skracenica = "ISA (SIIT)", DuzinaTermina = 2, BrojTermina = 5, VelicinaGrupe = 16, SmerPredmeta = s[0], TrebaTabla = false, TrebaPametnaTabla = false, TrebaProjektor = true });
            Predmeti = new ObservableCollection<Predmet>(p);

            u.Add(new Ucionica{ Oznaka = "L1", BrojRadnihMesta=16, ImaPametnaTabla=false, ImaTabla=true, ImaProjektor = true});
            u.Add(new Ucionica { Oznaka = "L2", BrojRadnihMesta = 16, ImaPametnaTabla = false, ImaTabla = true, ImaProjektor = true });
            u.Add(new Ucionica { Oznaka = "L3", BrojRadnihMesta = 16, ImaPametnaTabla = false, ImaTabla = true, ImaProjektor = true });
            Ucionice = new ObservableCollection<Ucionica>(u);

            sf.Add(new Softver { Naziv = "Visual Studio", GodinaIzdavanja = 2000, Cena = 50000, Opis = "Mircosoft", Proizvodjac = "Mircosoft", Oznaka = "VS" });
            Softveri = new ObservableCollection<Softver>(sf);

            SacuvajUcionicu.Visibility = Visibility.Hidden;
            SacuvajPredmet.Visibility = Visibility.Hidden;
            SacuvajSmer.Visibility = Visibility.Hidden;
            SacuvajSoftver.Visibility = Visibility.Hidden;

            OdusatniUcionica.Visibility = Visibility.Hidden;
            OdustaniPredmet.Visibility = Visibility.Hidden;
            OdustaniSmer.Visibility = Visibility.Hidden;
            OdustaniSoftver.Visibility = Visibility.Hidden;

            GridUcionice.IsEnabled = false;
            GridSmer.IsEnabled = false;
            GridPredmeti.IsEnabled = false;
            GridSoftver.IsEnabled = false;

        }

        public ObservableCollection<Predmet> Predmeti
        {
            get;
            set;
        }

        public ObservableCollection<Smer> Smerovi
        {
            get;
            set;
        }

        public ObservableCollection<Ucionica> Ucionice
        {
            get;
            set;
        }

        public ObservableCollection<Softver> Softveri
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


        private bool _rezimDodavanje;
        private string _rezimPrikaz;
        private bool _rezimIzmena;

        public bool RezimDodavanje
        {
            get
            {
                return _rezimDodavanje;
            }
            set
            {
                if (_rezimDodavanje != value)
                {
                    _rezimDodavanje = value;
                    OnPropertyChanged("RezimDodavanje");
                }
            }
        }

        public string RezimPrikaz
        {
            get
            {
                return _rezimPrikaz;
            }
            set
            {
                if (_rezimPrikaz != value)
                {
                    _rezimPrikaz = value;
         
                    
                    OnPropertyChanged("RezimPrikaz");
                }
            }
        }

        public bool RezimIzmena
        {
            get
            {
                return _rezimIzmena;
            }
            set
            {
                if (_rezimIzmena != value)
                {
                    _rezimIzmena = value;
                    OnPropertyChanged("RezimIzmena");
                }
            }
        }

        private void DodajUcionicu_Click(object sender, RoutedEventArgs e)
        {
            SelectedUcionica = new Ucionica();
            EnablePromeni = "False";
            EnableIzbrisi = "False";
            EnableDodaj = "False";
            TabPredmeti = "False";
            TabSmer = "False";
            TabSoftver = "False";
            Podaci = "False";
            SacuvajUcionicu.Visibility = Visibility.Visible;
            OdusatniUcionica.Visibility = Visibility.Visible;
            GridUcionice.IsEnabled = true;


        }

        private Ucionica _selectedUcionica;

        public Ucionica SelectedUcionica
        {
            get
            {
                return _selectedUcionica;
            }
            set
            {
                if (_selectedUcionica != value)
                {
                    _selectedUcionica = value;
                    OnPropertyChanged("SelectedUcionica");
                }
            }
        }

        private string _enablePromeni;
        public string EnablePromeni
        {
            get
            {
                return _enablePromeni;
            }
            set
            {
                if (_enablePromeni != value)
                {
                    _enablePromeni = value;
                    OnPropertyChanged("EnablePromeni");
                }
            }
        }

        private string _enableIzbrisi;
        public string EnableIzbrisi
        {
            get
            {
                return _enableIzbrisi;
            }
            set
            {
                if (_enableIzbrisi != value)
                {
                    _enableIzbrisi = value;
                    OnPropertyChanged("EnableIzbrisi");
                }
            }
        }

        private string _enableDodaj;
        public string EnableDodaj
        {
            get
            {
                return _enableDodaj;
            }
            set
            {
                if (_enableDodaj != value)
                {
                    _enableDodaj = value;
                    OnPropertyChanged("EnableDodaj");
                }
            }
        }


        private string _tabUcionice;
        public string TabUcionice
        {
            get
            {
                return _tabUcionice;
            }
            set
            {
                if (_tabUcionice != value)
                {
                    _tabUcionice = value;
                    OnPropertyChanged("TabUcionice");
                }
            }
        }

        private string _tabPredmeti;
        public string TabPredmeti
        {
            get
            {
                return _tabPredmeti;
            }
            set
            {
                if (_tabPredmeti != value)
                {
                    _tabPredmeti = value;
                    OnPropertyChanged("TabPredmeti");
                }
            }
        }

        private string _tabSmer;
        public string TabSmer
        {
            get
            {
                return _tabSmer;
            }
            set
            {
                if (_tabSmer != value)
                {
                    _tabSmer = value;
                    OnPropertyChanged("TabSmer");
                }
            }
        }
        private string _tabSoftver;
        public string TabSoftver
        {
            get
            {
                return _tabSoftver;
            }
            set
            {
                if (_tabSoftver != value)
                {
                    _tabSoftver = value;
                    OnPropertyChanged("TabSoftver");
                }
            }
        }

        private string _podaci;
        public string Podaci
        {
            get
            {
                return _podaci;
            }
            set
            {
                if (_podaci != value)
                {
                    _podaci = value;
                    OnPropertyChanged("Podaci");
                }
            }
        }

        private void OdustaniUcionica_Click(object sender, RoutedEventArgs e)
        {
            if (Ucionice.Count > 0)
            {
                SelectedUcionica = Ucionice[0];
            }
            EnablePromeni = "True";
            EnableIzbrisi = "True";
            EnableDodaj = "True";
            TabPredmeti = "True";
            TabSmer = "True";
            TabSoftver = "True";
            Podaci = "True";

            SacuvajUcionicu.Visibility = Visibility.Hidden;
            OdusatniUcionica.Visibility = Visibility.Hidden;
            GridUcionice.IsEnabled = false;

        }

        private void SacuvajUcionicu_Click(object sender, RoutedEventArgs e)
        {
            Ucionice.Add(SelectedUcionica);
            EnablePromeni = "True";
            EnableIzbrisi = "True";
            EnableDodaj = "True";
            TabPredmeti = "True";
            TabSmer = "True";
            TabSoftver = "True";
            Podaci = "True";

            SacuvajUcionicu.Visibility = Visibility.Hidden;
            OdusatniUcionica.Visibility = Visibility.Hidden;
            GridUcionice.IsEnabled = false;
        }

        private void IzbrisiUcionicu_Click(object sender, RoutedEventArgs e)
        {
            Ucionice.Remove(SelectedUcionica);

        }

        private Predmet _selectedPredmet;
        public Predmet SelectedPredmet
        {
            get
            {
                return _selectedPredmet;
            }
            set
            {
                if (_selectedPredmet != value)
                {
                    _selectedPredmet = value;
                    OnPropertyChanged("SelectedPredmet");
                }
            }
        }

        private void DodajPredmet_Click(object sender, RoutedEventArgs e)
        {
            SelectedPredmet = new Predmet();
            EnablePromeni = "False";
            EnableIzbrisi = "False";
            EnableDodaj = "False";
            TabUcionice = "False";
            TabSmer = "False";
            TabSoftver = "False";
            Podaci = "False";
            SacuvajPredmet.Visibility = Visibility.Visible;
            OdustaniPredmet.Visibility = Visibility.Visible;

            GridPredmeti.IsEnabled = true;
        }

        private void OdustaniPredmet_Click(object sender, RoutedEventArgs e)
        {
            if (Predmeti.Count > 0)
            {
                SelectedPredmet = Predmeti[0];
            }
            EnablePromeni = "True";
            EnableIzbrisi = "True";
            EnableDodaj = "True";
            TabUcionice = "True";
            TabSmer = "True";
            TabSoftver = "True";
            Podaci = "True";

            SacuvajPredmet.Visibility = Visibility.Hidden;
            OdustaniPredmet.Visibility = Visibility.Hidden;

            GridPredmeti.IsEnabled = false;

        }

        private void SacuvajPredmet_Click(object sender, RoutedEventArgs e)
        {
            Predmeti.Add(SelectedPredmet);
            EnablePromeni = "True";
            EnableIzbrisi = "True";
            EnableDodaj = "True";
            TabUcionice = "True";
            TabSmer = "True";
            TabSoftver = "True";
            Podaci = "True";

            SacuvajPredmet.Visibility = Visibility.Hidden;
            OdustaniPredmet.Visibility = Visibility.Hidden;

            GridPredmeti.IsEnabled = false;

        }

        private Smer _selectedSmer;
        public Smer SelectedSmer
        {
            get
            {
                return _selectedSmer;
            }
            set
            {
                if (_selectedSmer != value)
                {
                    _selectedSmer = value;
                    OnPropertyChanged("SelectedSmer");
                }
            }
        }

        private void DodajSmer_Click(object sender, RoutedEventArgs e)
        {
            SelectedSmer = new Smer();
            EnablePromeni = "False";
            EnableIzbrisi = "False";
            EnableDodaj = "False";
            TabUcionice = "False";
            TabPredmeti = "False";
            TabSoftver = "False";
            Podaci = "False";
            SacuvajSmer.Visibility = Visibility.Visible;
            OdustaniSmer.Visibility = Visibility.Visible;

            GridSmer.IsEnabled = true;

        }

        private void OdustaniSmer_Click(object sender, RoutedEventArgs e)
        {
            if (Smerovi.Count > 0)
            {
                SelectedSmer = Smerovi[0];
            }

            EnablePromeni = "True";
            EnableIzbrisi = "True";
            EnableDodaj = "True";
            TabUcionice = "True";
            TabPredmeti = "True";
            TabSoftver = "True";
            Podaci = "True";

            SacuvajSmer.Visibility = Visibility.Hidden;
            OdustaniSmer.Visibility = Visibility.Hidden;

            GridSmer.IsEnabled = false;
        }

        private void SacuvajSmer_Click(object sender, RoutedEventArgs e)
        {
            Smerovi.Add(SelectedSmer);
            EnablePromeni = "True";
            EnableIzbrisi = "True";
            EnableDodaj = "True";
            TabUcionice = "True";
            TabPredmeti = "True";
            TabSoftver = "True";
            Podaci = "True";

            SacuvajSmer.Visibility = Visibility.Hidden;
            OdustaniSmer.Visibility = Visibility.Hidden;
            GridSmer.IsEnabled = false;

        }

        private Softver _selectedSoftver;
        public Softver SelectedSoftver
        {
            get
            {
                return _selectedSoftver;
            }
            set
            {
                if (_selectedSoftver != value)
                {
                    _selectedSoftver = value;
                    OnPropertyChanged("SelectedSoftver");
                }
            }
        }

        private void DodajSoftver_Click(object sender, RoutedEventArgs e)
        {
            SelectedSoftver = new Softver();
            EnablePromeni = "False";
            EnableIzbrisi = "False";
            EnableDodaj = "False";
            TabUcionice = "False";
            TabPredmeti = "False";
            TabSmer = "False";
            Podaci = "False";
            SacuvajSoftver.Visibility = Visibility.Visible;
            OdustaniSoftver.Visibility = Visibility.Visible;

            GridSoftver.IsEnabled = true;

        }

        private void OdustaniSoftver_Click(object sender, RoutedEventArgs e)
        {
            if (Softveri.Count > 0)
            {
                SelectedSoftver = Softveri[0];
            }
            EnablePromeni = "True";
            EnableIzbrisi = "True";
            EnableDodaj = "True";
            TabUcionice = "True";
            TabPredmeti = "True";
            TabSmer = "True";
            Podaci = "True";

            SacuvajSoftver.Visibility = Visibility.Hidden;
            OdustaniSoftver.Visibility = Visibility.Hidden;

            GridSoftver.IsEnabled = false;

        }

        private void SacuvajSoftver_Click(object sender, RoutedEventArgs e)
        {
            Softveri.Add(SelectedSoftver);
            EnablePromeni = "True";
            EnableIzbrisi = "True";
            EnableDodaj = "True";
            TabUcionice = "True";
            TabPredmeti = "True";
            TabSoftver = "True";
            Podaci = "True";

            SacuvajSoftver.Visibility = Visibility.Hidden;
            OdustaniSoftver.Visibility = Visibility.Hidden;

            GridSoftver.IsEnabled = false;

        }

        private void IzbrisiPredmet_Click(object sender, RoutedEventArgs e)
        {
            Predmeti.Remove(SelectedPredmet);
        }

        private void IzbrisiSmer_Click(object sender, RoutedEventArgs e)
        {
            Smerovi.Remove(SelectedSmer);

        }

        private void IzbrisiSoftver_Click(object sender, RoutedEventArgs e)
        {
            Softveri.Remove(SelectedSoftver);

        }

        private Visibility _visibleSacuvaj;
        public Visibility VisibleSacuvaj
        {
            get
            {
                return _visibleSacuvaj;
            }
            set
            {
                if (_visibleSacuvaj != value)
                {
                    _visibleSacuvaj = value;
                    OnPropertyChanged("VisibleSacuvaj");
                }
            }
        }



    }
}
