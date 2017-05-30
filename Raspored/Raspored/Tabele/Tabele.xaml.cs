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

            IzmeniSoftver.Visibility = Visibility.Hidden;
            IzmenaOdustaniSoftver.Visibility = Visibility.Hidden;

            SacuvajIzmenuSmera.Visibility = Visibility.Hidden;
            IzmenaOdustaniSmer.Visibility = Visibility.Hidden;

            SacuvajIzmenuPredmet.Visibility = Visibility.Hidden;
            IzmenaOdustaniPredmet.Visibility = Visibility.Hidden;

            SacuvajIzmenuUcionice.Visibility = Visibility.Hidden;
            IzmenaOdustaniUcionica.Visibility = Visibility.Hidden;

            if (Ucionice.Count > 0)
            {
                SelectedUcionica = Ucionice[0]; 
            }

            if (Predmeti.Count > 0)
            {
                SelectedPredmet = Predmeti[0];
            }

            if (Softveri.Count > 0)
            {
                SelectedSoftver = Softveri[0];
            }

            if (Smerovi.Count > 0)
            {
                SelectedSmer = Smerovi[0];
            }
        }

        /** KOLEKCIJE **/
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

        /*** ENABLE ZA DUGME DODAJ ***/
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
        /**** ENABLE ZA TABOVE ****/
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

        /********************************** MANIPULACIJA UCIONICAMA *****************************/
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

        private string _enablePromeniUcionicu;
        public string EnablePromeniUcionicu
        {
            get
            {
                return _enablePromeniUcionicu;
            }
            set
            {
                if (_enablePromeniUcionicu != value)
                {
                    _enablePromeniUcionicu = value;
                    OnPropertyChanged("EnablePromeniUcionicu");
                }
            }
        }

        private string _enableIzbrisiUcionicu;
        public string EnableIzbrisiUcionicu
        {
            get
            {
                return _enableIzbrisiUcionicu;
            }
            set
            {
                if (_enableIzbrisiUcionicu != value)
                {
                    _enableIzbrisiUcionicu = value;
                    OnPropertyChanged("EnableIzbrisiUcionicu");
                }
            }
        }

        /***  REZIM ZA DODAVANJE NOVE UCIONICE ***/
        private void DodajUcionicu_Click(object sender, RoutedEventArgs e)
        {
            SelectedUcionica = new Ucionica();
            EnablePromeniUcionicu = "False";
            EnableIzbrisiUcionicu = "False";
            EnableDodaj = "False";
            TabPredmeti = "False";
            TabSmer = "False";
            TabSoftver = "False";
            Podaci = "False";
            SacuvajUcionicu.Visibility = Visibility.Visible;
            OdusatniUcionica.Visibility = Visibility.Visible;
            GridUcionice.IsEnabled = true;
        }

        /**** KLIK NA DUGME ODUSTANI OD DODAVANJA UCIONICE ****/
        private void OdustaniUcionica_Click(object sender, RoutedEventArgs e)
        {
            if (Ucionice.Count > 0)
            {
                SelectedUcionica = Ucionice[0];
            }


            EnablePromeniUcionicu = "True";
            EnableIzbrisiUcionicu = "True";
            EnableDodaj = "True";
            TabPredmeti = "True";
            TabSmer = "True";
            TabSoftver = "True";
            Podaci = "True";

            SacuvajUcionicu.Visibility = Visibility.Hidden;
            OdusatniUcionica.Visibility = Visibility.Hidden;
            GridUcionice.IsEnabled = false;

        }
        /**** KLINK NA DUGME SACUVAJ UCIONICU ****/
        private void SacuvajUcionicu_Click(object sender, RoutedEventArgs e)
        {

            Ucionice.Add(SelectedUcionica);
            EnablePromeniUcionicu = "True";
            EnableIzbrisiUcionicu = "True";
            EnableDodaj = "True";
            TabPredmeti = "True";
            TabSmer = "True";
            TabSoftver = "True";
            Podaci = "True";

            SacuvajUcionicu.Visibility = Visibility.Hidden;
            OdusatniUcionica.Visibility = Visibility.Hidden;
            GridUcionice.IsEnabled = false;

            if (Ucionice.Count > 0)
            {
                EnableIzbrisiUcionicu = "True";
                EnablePromeniUcionicu = "True";
            }
        }

        /**** KLIK NA DUGME IZBRISI UCIONICU ***/
        private void IzbrisiUcionicu_Click(object sender, RoutedEventArgs e)
        {
            Ucionice.Remove(SelectedUcionica);

            if (Ucionice.Count <= 0)
            {
                EnableIzbrisiUcionicu = "False";
                EnablePromeniUcionicu = "False";
            }
        }

        /***** REZIM ZA IZMENU UCIONICE ****/
        private void RezimIzmeniUcionicu_Click(object sender, RoutedEventArgs e)
        {

            _index = Ucionice.IndexOf(SelectedUcionica);
            SelectedUcionica = new Ucionica(SelectedUcionica.Oznaka, SelectedUcionica.Opis,
                SelectedUcionica.BrojRadnihMesta, SelectedUcionica.ImaProjektor,
                SelectedUcionica.ImaTabla, SelectedUcionica.ImaPametnaTabla, SelectedUcionica.OperativniSistem);

            GridUcionice.IsEnabled = true;
            SacuvajIzmenuUcionice.Visibility = Visibility.Visible;
            IzmenaOdustaniUcionica.Visibility = Visibility.Visible;

            EnablePromeniUcionicu = "False";
            EnableIzbrisiUcionicu = "False";
            EnableDodaj = "False";
            TabPredmeti = "False";
            TabSmer = "False";
            TabSoftver = "False";
            Podaci = "False";
        }

        /***** KLINK NA DUGME SACUVAJ IZMENU UCIONICE ****/
        private void SacuvajIzmenuUcionice_Click(object sender, RoutedEventArgs e)
        {
            Ucionice[_index] = SelectedUcionica;

            SacuvajIzmenuUcionice.Visibility = Visibility.Hidden;
            IzmenaOdustaniUcionica.Visibility = Visibility.Hidden;

            EnablePromeniUcionicu = "True";
            EnableIzbrisiUcionicu = "True";
            EnableDodaj = "True";
            TabPredmeti = "True";
            TabSmer = "True";
            TabSoftver = "True";
            Podaci = "True";

            GridUcionice.IsEnabled = false;
        }

        /**** KLINK NA DUGME PONISTI IZMENU UCIONICE ****/
        private void IzmenaOdustaniUcionica_Click(object sender, RoutedEventArgs e)
        {
            SelectedUcionica = Ucionice[_index];

            SacuvajIzmenuUcionice.Visibility = Visibility.Hidden;
            IzmenaOdustaniUcionica.Visibility = Visibility.Hidden;

            EnablePromeniUcionicu = "True";
            EnableIzbrisiUcionicu = "True";
            EnableDodaj = "True";
            TabPredmeti = "True";
            TabSmer = "True";
            TabSoftver = "True";
            Podaci = "True";

            GridUcionice.IsEnabled = false;

        }

        /**************************************** MANIPULACIJA PREDMETIMA ************************************************/

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

        private string _enablePromeniPredmet;
        public string EnablePromeniPredmet
        {
            get
            {
                return _enablePromeniPredmet;
            }
            set
            {
                if (_enablePromeniPredmet != value)
                {
                    _enablePromeniPredmet = value;
                    OnPropertyChanged("EnablePromeniPredmet");
                }
            }
        }

        private string _enableIzbrisiPredmet;
        public string EnableIzbrisiPredmet
        {
            get
            {
                return _enableIzbrisiPredmet;
            }
            set
            {
                if (_enableIzbrisiPredmet != value)
                {
                    _enableIzbrisiPredmet = value;
                    OnPropertyChanged("EnableIzbrisiPredmet");
                }
            }
        }

        private void DodajPredmet_Click(object sender, RoutedEventArgs e)
        {
            SelectedPredmet = new Predmet();
            EnablePromeniPredmet = "False";
            EnableIzbrisiPredmet = "False";
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
            EnablePromeniPredmet = "True";
            EnableIzbrisiPredmet = "True";
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
            EnablePromeniPredmet = "True";
            EnableIzbrisiPredmet = "True";
            EnableDodaj = "True";
            TabUcionice = "True";
            TabSmer = "True";
            TabSoftver = "True";
            Podaci = "True";

            SacuvajPredmet.Visibility = Visibility.Hidden;
            OdustaniPredmet.Visibility = Visibility.Hidden;

            GridPredmeti.IsEnabled = false;

            if (Predmeti.Count > 0)
            {
                EnableIzbrisiPredmet = "True";
                EnablePromeniPredmet = "True";
            }
        }

        private void IzbrisiPredmet_Click(object sender, RoutedEventArgs e)
        {
            Predmeti.Remove(SelectedPredmet);
            if (Predmeti.Count <= 0)
            {
                EnableIzbrisiPredmet = "False";
                EnablePromeniPredmet = "False";
            }
        }

        private void RezimIzmeniPredmet_Click(object sender, RoutedEventArgs e)
        {
            _index = Predmeti.IndexOf(SelectedPredmet);
            SelectedPredmet = new Predmet(SelectedPredmet.Oznaka, SelectedPredmet.Naziv, SelectedPredmet.Skracenica,
                SelectedPredmet.SmerPredmeta, SelectedPredmet.Opis, SelectedPredmet.VelicinaGrupe,
                SelectedPredmet.DuzinaTermina, SelectedPredmet.BrojTermina, SelectedPredmet.TrebaProjektor,
                SelectedPredmet.TrebaTabla, SelectedPredmet.TrebaPametnaTabla,
                SelectedPredmet.NeophodanOS, SelectedPredmet.Softveri);

            GridPredmeti.IsEnabled = true;
            SacuvajIzmenuPredmet.Visibility = Visibility.Visible;
            IzmenaOdustaniPredmet.Visibility = Visibility.Visible;

            EnablePromeniPredmet = "False";
            EnableIzbrisiPredmet = "False";
            EnableDodaj = "False";
            TabUcionice = "False";
            TabSmer = "False";
            TabSoftver = "False";
            Podaci = "False";

        }

        private void SacuvajIzmenuPredmet_Click(object sender, RoutedEventArgs e)
        {
            Predmeti[_index] = SelectedPredmet;

            SacuvajIzmenuPredmet.Visibility = Visibility.Hidden;
            IzmenaOdustaniPredmet.Visibility = Visibility.Hidden;

            EnablePromeniPredmet = "True";
            EnableIzbrisiPredmet = "True";
            EnableDodaj = "True";
            TabUcionice = "True";
            TabSmer = "True";
            TabSoftver = "True";
            Podaci = "True";

            GridPredmeti.IsEnabled = false;

        }

        private void IzmenaOdustaniPredmet_Click(object sender, RoutedEventArgs e)
        {
            SelectedPredmet = Predmeti[_index];

            SacuvajIzmenuPredmet.Visibility = Visibility.Hidden;
            IzmenaOdustaniPredmet.Visibility = Visibility.Hidden;

            EnablePromeniPredmet = "True";
            EnableIzbrisiPredmet = "True";
            EnableDodaj = "True";
            TabUcionice = "True";
            TabSmer = "True";
            TabSoftver = "True";
            Podaci = "True";

            GridPredmeti.IsEnabled = false;

        }

        /******************************************   MANIPULACIJA SMEROVIMA  **************************************/
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

        private string _enablePromeniSmer;
        public string EnablePromeniSmer
        {
            get
            {
                return _enablePromeniSmer;
            }
            set
            {
                if (_enablePromeniSmer != value)
                {
                    _enablePromeniSmer = value;
                    OnPropertyChanged("EnablePromeniSmer");
                }
            }
        }

        private string _enableIzbrisiSmer;
        public string EnableIzbrisiSmer
        {
            get
            {
                return _enableIzbrisiSmer;
            }
            set
            {
                if (_enableIzbrisiSmer != value)
                {
                    _enableIzbrisiSmer = value;
                    OnPropertyChanged("EnableIzbrisiSmer");
                }
            }
        }

        private void DodajSmer_Click(object sender, RoutedEventArgs e)
        {
            SelectedSmer = new Smer();
            EnablePromeniSmer = "False";
            EnableIzbrisiSmer = "False";
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

            EnablePromeniSmer = "True";
            EnableIzbrisiSmer = "True";
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
            EnablePromeniSmer = "True";
            EnableIzbrisiSmer = "True";
            EnableDodaj = "True";
            TabUcionice = "True";
            TabPredmeti = "True";
            TabSoftver = "True";
            Podaci = "True";

            SacuvajSmer.Visibility = Visibility.Hidden;
            OdustaniSmer.Visibility = Visibility.Hidden;
            GridSmer.IsEnabled = false;

            if (Smerovi.Count > 0)
            {
                EnableIzbrisiSmer = "True";
                EnablePromeniSmer = "True";
            }

        }

        private void IzbrisiSmer_Click(object sender, RoutedEventArgs e)
        {
            Smerovi.Remove(SelectedSmer);
            if (Smerovi.Count <= 0)
            {
                EnableIzbrisiSmer = "False";
                EnablePromeniSmer = "False";
            }

        }

        private void RezimIzmeniSmer_Click(object sender, RoutedEventArgs e)
        {
            _index = Smerovi.IndexOf(SelectedSmer);
            SelectedSmer = new Smer(SelectedSmer.Oznaka, SelectedSmer.Naziv,
                SelectedSmer.Skracenica, SelectedSmer.DatumUvodjenja, SelectedSmer.Opis);

            GridSmer.IsEnabled = true;
            SacuvajIzmenuSmera.Visibility = Visibility.Visible;
            IzmenaOdustaniSmer.Visibility = Visibility.Visible;

            EnablePromeniSmer = "False";
            EnableIzbrisiSmer = "False";
            EnableDodaj = "False";
            TabUcionice = "False";
            TabPredmeti = "False";
            TabSoftver = "False";
            Podaci = "False";

        }

        private void SacuvajIzmenuSmera_Click(object sender, RoutedEventArgs e)
        {
            Smerovi[_index] = SelectedSmer;

            SacuvajIzmenuSmera.Visibility = Visibility.Hidden;
            IzmenaOdustaniSmer.Visibility = Visibility.Hidden;

            EnablePromeniSmer = "True";
            EnableIzbrisiSmer = "True";
            EnableDodaj = "True";
            TabUcionice = "True";
            TabPredmeti = "True";
            TabSoftver = "True";
            Podaci = "True";

            GridSmer.IsEnabled = false;

        }

        private void IzmenaOdustaniSmer_Click(object sender, RoutedEventArgs e)
        {
            SelectedSmer = Smerovi[_index];

            SacuvajIzmenuSmera.Visibility = Visibility.Hidden;
            IzmenaOdustaniSmer.Visibility = Visibility.Hidden;

            EnablePromeniSmer = "True";
            EnableIzbrisiSmer = "True";
            EnableDodaj = "True";
            TabUcionice = "True";
            TabPredmeti = "True";
            TabSoftver = "True";
            Podaci = "True";

            GridSmer.IsEnabled = false;

        }


        /***********************************************    MANIPULACIJA SOFTVERIMA   ************************************/

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

        private string _enablePromeniSoftver;
        public string EnablePromeniSoftver
        {
            get
            {
                return _enablePromeniSoftver;
            }
            set
            {
                if (_enablePromeniSoftver != value)
                {
                    _enablePromeniSoftver = value;
                    OnPropertyChanged("EnablePromeniSoftver");
                }
            }
        }

        private string _enableIzbrisiSoftver;
        public string EnableIzbrisiSoftver
        {
            get
            {
                return _enableIzbrisiSoftver;
            }
            set
            {
                if (_enableIzbrisiSoftver != value)
                {
                    _enableIzbrisiSoftver = value;
                    OnPropertyChanged("EnableIzbrisiSoftver");
                }
            }
        }        

        private void DodajSoftver_Click(object sender, RoutedEventArgs e)
        {
            SelectedSoftver = new Softver();
            EnablePromeniSoftver = "False";
            EnableIzbrisiSoftver = "False";
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
            EnablePromeniSoftver = "True";
            EnableIzbrisiSoftver = "True";
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
            EnablePromeniSoftver = "True";
            EnableIzbrisiSoftver = "True";
            EnableDodaj = "True";
            TabUcionice = "True";
            TabPredmeti = "True";
            TabSmer = "True";
            Podaci = "True";

            SacuvajSoftver.Visibility = Visibility.Hidden;
            OdustaniSoftver.Visibility = Visibility.Hidden;

            GridSoftver.IsEnabled = false;

            if (Softveri.Count > 0)
            {
                EnableIzbrisiSoftver = "True";
                EnablePromeniSoftver = "True";
            }

        }

        private void IzbrisiSoftver_Click(object sender, RoutedEventArgs e)
        {
            Softveri.Remove(SelectedSoftver);
            if (Softveri.Count <= 0)
            {
                EnableIzbrisiSoftver = "False";
                EnablePromeniSoftver = "False";
            }

        }

        private void RezimIzmeniSoftver_Click(object sender, RoutedEventArgs e)
        {
            _index = Softveri.IndexOf(SelectedSoftver);

            SelectedSoftver = new Softver(SelectedSoftver.Oznaka, SelectedSoftver.Naziv, 
                SelectedSoftver.OperativniSistem,SelectedSoftver.Proizvodjac, 
                SelectedSoftver.Sajt, SelectedSoftver.GodinaIzdavanja,
                SelectedSoftver.Cena, SelectedSoftver.Opis);

            GridSoftver.IsEnabled = true;
            IzmeniSoftver.Visibility = Visibility.Visible;
            IzmenaOdustaniSoftver.Visibility = Visibility.Visible;

            EnablePromeniSoftver = "False";
            EnableIzbrisiSoftver = "False";
            EnableDodaj = "False";
            TabUcionice = "False";
            TabPredmeti = "False";
            TabSmer = "False";
            Podaci = "False";
        }

        private void IzmeniSoftver_Click(object sender, RoutedEventArgs e)
        {
            Softveri[_index] = SelectedSoftver;

            IzmeniSoftver.Visibility = Visibility.Hidden;
            IzmenaOdustaniSoftver.Visibility = Visibility.Hidden;

            EnablePromeniSoftver = "True";
            EnableIzbrisiSoftver = "True";
            EnableDodaj = "True";
            TabUcionice = "True";
            TabPredmeti = "True";
            TabSmer = "True";
            Podaci = "True";

            GridSoftver.IsEnabled = false;
        }

        private void IzmenaOdustaniSoftver_Click(object sender, RoutedEventArgs e)
        {
            SelectedSoftver = Softveri[_index];

            IzmeniSoftver.Visibility = Visibility.Hidden;
            IzmenaOdustaniSoftver.Visibility = Visibility.Hidden;

            EnablePromeniSoftver = "True";
            EnableIzbrisiSoftver = "True";
            EnableDodaj = "True";
            TabUcionice = "True";
            TabPredmeti = "True";
            TabSmer = "True";
            Podaci = "True";
            GridSoftver.IsEnabled = false;

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

        private int _index;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
