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
using System.IO;
using System.Text.RegularExpressions;

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
            /*
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

            */

            //List<Smer> s = new List<Smer>(); 
            List<Smer> s = otvoriSmer();
            Smerovi = new ObservableCollection<Smer>(s);

            //List<Softver> sf = new List<Softver>();
            List<Softver> sf = otvoriSoftver();
            Softveri = new ObservableCollection<Softver>(sf);

            // List<Predmet> p = new List<Predmet>();
            List<Predmet> p = otvoriPredmet();
            //p.Add(new Predmet { Naziv = "Interakcija covek racunar", Oznaka = "HCI", Skracenica = "HCI (SIIT)", DuzinaTermina = 2, BrojTermina = 6, VelicinaGrupe = 16, SmerPredmeta = s[0], TrebaTabla = true, TrebaPametnaTabla = false, TrebaProjektor = true });
            //p.Add(new Predmet { Naziv = "Internet softverske arhitekture", Oznaka = "ISA", Skracenica = "ISA (SIIT)", DuzinaTermina = 2, BrojTermina = 5, VelicinaGrupe = 16, SmerPredmeta = s[0], TrebaTabla = false, TrebaPametnaTabla = false, TrebaProjektor = true });
            Predmeti = new ObservableCollection<Predmet>(p);

            // List<Ucionica> u = new List<Ucionica>();
            List<Ucionica> u = otvoriUcionicu();
            //u.Add(new Ucionica{ Oznaka = "L1", BrojRadnihMesta=16, ImaPametnaTabla=false, ImaTabla=true, ImaProjektor = true});
            //u.Add(new Ucionica { Oznaka = "L2", BrojRadnihMesta = 16, ImaPametnaTabla = false, ImaTabla = true, ImaProjektor = true });
            //u.Add(new Ucionica { Oznaka = "L3", BrojRadnihMesta = 16, ImaPametnaTabla = false, ImaTabla = true, ImaProjektor = true });

            Ucionice = new ObservableCollection<Ucionica>(u);
            

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
            }else
            {

                EnableIzbrisiUcionicu = "False";
                EnablePromeniUcionicu = "False";
            }

            if (Predmeti.Count > 0)
            {
                SelectedPredmet = Predmeti[0];
            }else
            {
                EnablePromeniPredmet = "False";
                EnableIzbrisiPredmet = "False";
            }

            if (Softveri.Count > 0)
            {
                SelectedSoftver = Softveri[0];
            }else
            {
                EnablePromeniSoftver = "False";
                EnableIzbrisiSoftver = "False";
            }

            if (Smerovi.Count > 0)
            {
                SelectedSmer = Smerovi[0];
            }else
            {
                EnableIzbrisiSmer = "False";
                EnablePromeniSmer = "False";
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

        /************************************************* MANIPULACIJA UCIONICAMA ******************************************/
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
                EnablePromeniUcionicu = "True";
                EnableIzbrisiUcionicu = "True";
            }
            else
            {
                SelectedUcionica = null;
                EnablePromeniUcionicu = "False";
                EnableIzbrisiUcionicu = "False";

            }
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
        private void SacuvajUcionicu_Executed(object sender, ExecutedRoutedEventArgs e)
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

            sacuvajUcionicu();

            e.Handled = true;


            SelectRowByIndex(dgrMainUcionica, Ucionice.Count - 1);
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
            sacuvajUcionicu();
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
            sacuvajUcionicu();
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

        /********************************************* MANIPULACIJA PREDMETIMA ************************************************/

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
                EnablePromeniPredmet = "True";
                EnableIzbrisiPredmet = "True";
            }else
            {
                SelectedPredmet = null;
                EnablePromeniPredmet = "False";
                EnableIzbrisiPredmet = "False";
            }
            
            EnableDodaj = "True";
            TabUcionice = "True";
            TabSmer = "True";
            TabSoftver = "True";
            Podaci = "True";

            SacuvajPredmet.Visibility = Visibility.Hidden;
            OdustaniPredmet.Visibility = Visibility.Hidden;

            GridPredmeti.IsEnabled = false;

        }

        private void SacuvajPredmet_Executed(object sender, ExecutedRoutedEventArgs e)
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
            sacuvajPredmet();

            e.Handled = true;


            SelectRowByIndex(dgrMainPredmet, Predmeti.Count-1);
            
            
        }

        private void IzbrisiPredmet_Click(object sender, RoutedEventArgs e)
        {
            Predmeti.Remove(SelectedPredmet);
            if (Predmeti.Count <= 0)
            {
                EnableIzbrisiPredmet = "False";
                EnablePromeniPredmet = "False";
            }
            sacuvajPredmet();
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
            sacuvajPredmet();

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

        /***************************************************   MANIPULACIJA SMEROVIMA  **************************************/
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
                EnablePromeniSmer = "True";
                EnableIzbrisiSmer = "True";
            }else
            {
                SelectedSmer = null;
                EnablePromeniSmer = "False";
                EnableIzbrisiSmer = "False";
            }

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
            sacuvajSmer();

            SelectRowByIndex(dgrMainSmer, Smerovi.Count - 1);

        }

        private void IzbrisiSmer_Click(object sender, RoutedEventArgs e)
        {
            Smerovi.Remove(SelectedSmer);
            if (Smerovi.Count <= 0)
            {
                EnableIzbrisiSmer = "False";
                EnablePromeniSmer = "False";
            }
            sacuvajSmer();

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
            sacuvajSmer();

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


        /****************************************************    MANIPULACIJA SOFTVERIMA   ************************************/

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
                EnablePromeniSoftver = "True";
                EnableIzbrisiSoftver = "True";
            }
            else
            {
                SelectedSoftver = null;
                EnablePromeniSoftver = "False";
                EnableIzbrisiSoftver = "False";
            }
            
            EnableDodaj = "True";
            TabUcionice = "True";
            TabPredmeti = "True";
            TabSmer = "True";
            Podaci = "True";

            SacuvajSoftver.Visibility = Visibility.Hidden;
            OdustaniSoftver.Visibility = Visibility.Hidden;

            GridSoftver.IsEnabled = false;

        }

        private void SacuvajSoftver_Executed(object sender, ExecutedRoutedEventArgs e)
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
            sacuvajSoftver();

            e.Handled = true;


            SelectRowByIndex(dgrMainSoftver, Softveri.Count - 1);

        }

        private void IzbrisiSoftver_Click(object sender, RoutedEventArgs e)
        {
            Softveri.Remove(SelectedSoftver);
            if (Softveri.Count <= 0)
            {
                EnableIzbrisiSoftver = "False";
                EnablePromeniSoftver = "False";
            }
            sacuvajSoftver();

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
            sacuvajSoftver();
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

        /***********************************************    MANIPULACIJA FAJLOVIMA   ************************************/

        void sacuvajUcionicu()
        {
            FileStream f1 = new FileStream("../../Save/ucionica.txt", FileMode.Create);
            f1.Close();

            StreamWriter f = new StreamWriter("../../Save/ucionica.txt");
            // MessageBox.Show("123");
            foreach (Ucionica u in Ucionice)
            {
                f.Write(u.BrojRadnihMesta + "|" + u.ImaPametnaTabla + "|" + u.ImaProjektor + "|" + u.ImaTabla + "|");
                if (u.OperativniSistem == OS.widows)
                    f.Write(0);
                else if (u.OperativniSistem == OS.linux)
                    f.Write(1);
                else
                    f.Write(2);

                f.Write("|" + u.Opis + "|" + u.Oznaka + "|");
                if (u.Softveri != null)
                    if (u.Softveri.Count > 0)
                    {
                        foreach (Softver s in u.Softveri)
                        {
                            if (s != null)
                            {
                                f.Write(s.Oznaka + ",");
                            }
                        }
                    }
                f.WriteLine();
            }
            f.Close();
        }

        List<Ucionica> otvoriUcionicu()
        {
            List<Ucionica> ucionice = new List<Ucionica>();
            FileStream f = new FileStream("../../Save/ucionica.txt", FileMode.OpenOrCreate);
            f.Close();

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[\r\n]{3,}", options);
            string recentText = File.ReadAllText("../../Save/ucionica.txt");

            string[] tekst = recentText.Split('\n');
            foreach (string ucionica in tekst)
            {
                Ucionica u = new Ucionica();
                if (ucionica == "")
                    return ucionice;
                string[] uc = ucionica.Split('|');
                u.BrojRadnihMesta = Convert.ToInt32(uc[0]);
                u.ImaTabla = Convert.ToBoolean(uc[3]);
                u.ImaPametnaTabla = Convert.ToBoolean(uc[1]);
                u.ImaProjektor = Convert.ToBoolean(uc[2]);
                if (Convert.ToInt32(uc[4]) == 0)
                    u.OperativniSistem = OS.widows;
                else if (Convert.ToInt32(uc[4]) == 1)
                    u.OperativniSistem = OS.linux;
                else
                    u.OperativniSistem = OS.ostalo;
                u.Opis = uc[5];
                u.Oznaka = uc[6];
                ObservableCollection<Softver> softveri = new ObservableCollection<Softver>();
                foreach (string sof in uc[7].Split(','))
                {
                    Softver s = nadjiSoftver(sof);
                    softveri.Add(s);
                }
                u.Softveri = softveri;

                ucionice.Add(u);
            }

            return ucionice;
        }

        void sacuvajSmer()
        {
            StreamWriter f = new StreamWriter("../../Save/smer.txt");
            foreach (Smer s in Smerovi)
            {
                f.WriteLine(s.Oznaka + "|" + s.Skracenica + "|" + s.Opis + "|" + s.Naziv + "|" + s.DatumUvodjenja);
            }
            f.Close();
        }

        List<Smer> otvoriSmer()
        {

            List<Smer> smerovi = new List<Smer>();
            FileStream f = new FileStream("../../Save/smer.txt", FileMode.OpenOrCreate);
            f.Close();

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[\r\n]{3,}", options);
            string recentText = File.ReadAllText("../../Save/smer.txt");

            string[] tekst = recentText.Split('\n');
            foreach (string smer in tekst)
            {
                Smer s = new Smer();
                if (smer == "")
                {
                    return smerovi;
                }

                string[] sm = smer.Split('|');

                s.Oznaka = sm[0];
                s.Skracenica = sm[1];
                s.Opis = sm[2];
                s.Naziv = sm[3];
                s.DatumUvodjenja = Convert.ToDateTime(sm[4]);

                smerovi.Add(s);
            }

            return smerovi;
        }

        void sacuvajSoftver()
        {
            StreamWriter f = new StreamWriter("../../Save/softver.txt");
            foreach (Softver s in Softveri)
            {
                f.Write(s.Oznaka + "|" + s.Naziv + "|" + s.Cena + "|" + s.GodinaIzdavanja + "|");
                if (s.OperativniSistem == OS.widows)
                    f.Write(0);
                else if (s.OperativniSistem == OS.linux)
                    f.Write(1);
                else
                    f.Write(2);
                f.Write("|" + s.Opis + "|" + s.Proizvodjac + "|" + s.Sajt);
                f.Write("\r\n");
            }
            f.Close();
        }

        List<Softver> otvoriSoftver()
        {
            List<Softver> softveri = new List<Softver>();
            FileStream f = new FileStream("../../Save/softver.txt", FileMode.OpenOrCreate);
            f.Close();

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[\r\n]{3,}", options);
            string recentText = File.ReadAllText("../../Save/softver.txt");

            string[] tekst = recentText.Split('\n');
            foreach (string softver in tekst)
            {
                Softver s = new Softver();
                if (softver == "")
                    return softveri;
                string[] sf = softver.Split('|');

                s.Oznaka = sf[0];
                s.Naziv = sf[1];
                s.Cena = Convert.ToDouble(sf[2]);
                if (Convert.ToInt32(sf[3]) == 0)
                    s.OperativniSistem = OS.widows;
                else if (Convert.ToInt32(sf[3]) == 1)
                    s.OperativniSistem = OS.linux;
                else
                    s.OperativniSistem = OS.ostalo;
                s.Opis = sf[4];
                s.Proizvodjac = sf[5];
                s.Sajt = sf[6];

                softveri.Add(s);
            }

            return softveri;
        }

        void sacuvajPredmet()
        {
            StreamWriter f = new StreamWriter("../../Save/predmet.txt");
            foreach (Predmet p in Predmeti)
            {
                f.Write(p.Naziv + "|" + p.BrojTermina + "|" + p.DuzinaTermina + "|");

                if (p.NeophodanOS == OS.widows)
                    f.Write(0);
                else if (p.NeophodanOS == OS.linux)
                    f.Write(1);
                else
                    f.Write(2);
                f.Write("|" + p.Opis + "|" + p.Oznaka + "|" + p.Skracenica + "|");
                if (p.SmerPredmeta != null)
                    f.Write(p.SmerPredmeta.Oznaka);
                f.Write("|"+p.TrebaPametnaTabla + "|" + p.TrebaProjektor + "|" + p.TrebaTabla + "|" + p.VelicinaGrupe + "|");
                if (p.Softveri == null || p.Softveri.Count==0)
                    foreach (Softver s in p.Softveri)
                    {
                        f.Write(s.Oznaka + ",");
                    }
                f.WriteLine();
            }
            f.Close();
        }



        List<Predmet> otvoriPredmet()
        {
            List<Predmet> predmeti = new List<Predmet>();
            FileStream f = new FileStream("../../Save/predmet.txt", FileMode.OpenOrCreate);
            f.Close();

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[\r\n]{3,}", options);
            string recentText = File.ReadAllText("../../Save/predmet.txt");

            string[] tekst = recentText.Split('\n');
            foreach (string predmet in tekst)
            {
                Predmet p = new Predmet();
                if (predmet == "")
                    return predmeti;
                string[] pr = predmet.Split('|');

                p.Naziv = pr[0];
                 
                p.BrojTermina = Convert.ToInt32(pr[1]);
                p.DuzinaTermina = Convert.ToInt32(pr[2]);
                if (Convert.ToInt32(pr[3]) == 0)
                    p.NeophodanOS = OS.widows;
                else if (Convert.ToInt32(pr[3]) == 1)
                    p.NeophodanOS = OS.linux;
                else
                    p.NeophodanOS = OS.ostalo;
                p.Opis = pr[4];
                p.Oznaka = pr[5];
                p.Skracenica = pr[6];
                p.SmerPredmeta = nadjiSmer(pr[7]);
                //MessageBox.Show("TrebaPametnaTabla: " + pr[8]);
                p.TrebaPametnaTabla = Convert.ToBoolean(pr[8]);
                //MessageBox.Show("TrebaProjektor: " + pr[9]);
                p.TrebaProjektor = Convert.ToBoolean(pr[9]);
                //MessageBox.Show("TrebaTabla: " + pr[10]);
                p.TrebaTabla = Convert.ToBoolean(pr[10]);
                p.VelicinaGrupe = Convert.ToInt32(pr[11]);
                ObservableCollection<Softver> softveri = new ObservableCollection<Softver>();
                foreach (string sof in pr[12].Split(','))
                {
                    Softver s = nadjiSoftver(sof);
                    softveri.Add(s);
                }
                p.Softveri = softveri;
                predmeti.Add(p);

            }

            return predmeti;
        }

        Smer nadjiSmer(string oznaka)
        {
            //MessageBox.Show(""+Smerovi.Count);
            foreach (Smer s in Smerovi)
            {
                if (s.Oznaka == oznaka)
                    return s;
            }
            return null;
        }

        Softver nadjiSoftver(string oznaka)
        {
            //MessageBox.Show("" + Softveri.Count);
            foreach (Softver s in Softveri)
            {
                if (s.Oznaka == oznaka)
                    return s;
            }
            return null;
        }

        /****************************************        VALIDATION          **********************************************/
        private int _noOfErrorsOnScreen = 0;

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }

        private void SacuvajPredmet_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        private void SacuvajUcionicu_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        private void SacuvajSoftver_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        public static void SelectRowByIndex(DataGrid dataGrid, int rowIndex)
        {
            if (!dataGrid.SelectionUnit.Equals(DataGridSelectionUnit.FullRow))
                throw new ArgumentException("The SelectionUnit of the DataGrid must be set to FullRow.");

            if (rowIndex < 0 || rowIndex > (dataGrid.Items.Count - 1))
                throw new ArgumentException(string.Format("{0} is an invalid row index.", rowIndex));

            dataGrid.SelectedItems.Clear();
            /* set the SelectedItem property */
            object item = dataGrid.Items[rowIndex]; // = Product X
            dataGrid.SelectedItem = item;

            DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            if (row == null)
            {
                /* bring the data item (Product object) into view
                 * in case it has been virtualized away */
                dataGrid.ScrollIntoView(item);
                row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            }
            //TODO: Retrieve and focus a DataGridCell object
        }

        
    }

}
