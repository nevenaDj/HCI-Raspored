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
using Raspored.Tabele;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Threading;

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
            SveUcionice = new ObservableCollection<Ucionica>(u);



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
            else
            {

                EnableIzbrisiUcionicu = "False";
                EnablePromeniUcionicu = "False";
            }

            if (Predmeti.Count > 0)
            {
                SelectedPredmet = Predmeti[0];
            }
            else
            {
                EnablePromeniPredmet = "False";
                EnableIzbrisiPredmet = "False";
            }

            if (Softveri.Count > 0)
            {
                SelectedSoftver = Softveri[0];
            }
            else
            {
                EnablePromeniSoftver = "False";
                EnableIzbrisiSoftver = "False";
            }

            if (Smerovi.Count > 0)
            {
                SelectedSmer = Smerovi[0];
            }
            else
            {
                EnableIzbrisiSmer = "False";
                EnablePromeniSmer = "False";
            }
            SelectedTabUcionice = true;

            Projektor = new ObservableCollection<string>();
            Projektor.Add("");
            Projektor.Add("Ima");
            Projektor.Add("Nema");
            Tabla = new ObservableCollection<string>();
            Tabla.Add("");
            Tabla.Add("Ima");
            Tabla.Add("Nema");
            PametnaTabla = new ObservableCollection<string>();
            PametnaTabla.Add("");
            PametnaTabla.Add("Ima");
            PametnaTabla.Add("Nema");
            OS = new ObservableCollection<string>();
            OS.Add("");
            OS.Add("Windows");
            OS.Add("Linux");
            OS.Add("Ostalo");

            demoThread = null;


        }

        public Tabele(string demo)
        {
            InitializeComponent();
            this.DataContext = this;


            //List<Smer> s = new List<Smer>(); 
            List<Smer> s = otvoriSmer();
            Smerovi = new ObservableCollection<Smer>(s);

            List<Softver> sf = new List<Softver>();
            //List<Softver> sf = otvoriSoftver();
            Softveri = new ObservableCollection<Softver>(sf);

            // List<Predmet> p = new List<Predmet>();
            List<Predmet> p = new List<Predmet>();
            p.Add(new Predmet { Naziv = "Interakcija covek racunar", Oznaka = "HCI", Skracenica = "HCI (SIIT)", DuzinaTermina = 2, BrojTermina = 6, VelicinaGrupe = 16, SmerPredmeta = s[0], TrebaTabla = true, TrebaPametnaTabla = false, TrebaProjektor = true });
            p.Add(new Predmet { Naziv = "Internet softverske arhitekture", Oznaka = "ISA", Skracenica = "ISA (SIIT)", DuzinaTermina = 2, BrojTermina = 5, VelicinaGrupe = 16, SmerPredmeta = s[0], TrebaTabla = false, TrebaPametnaTabla = false, TrebaProjektor = true });
            Predmeti = new ObservableCollection<Predmet>(p);

            List<Ucionica> u = new List<Ucionica>();
            //List<Ucionica> u = otvoriUcionicu();
            u.Add(new Ucionica{ Oznaka = "L1", BrojRadnihMesta=16, ImaPametnaTabla=false, ImaTabla=true, ImaProjektor = true});
            u.Add(new Ucionica { Oznaka = "L2", BrojRadnihMesta = 16, ImaPametnaTabla = false, ImaTabla = true, ImaProjektor = true });
            u.Add(new Ucionica { Oznaka = "L3", BrojRadnihMesta = 16, ImaPametnaTabla = false, ImaTabla = true, ImaProjektor = true });

            Ucionice = new ObservableCollection<Ucionica>(u);
            SveUcionice = new ObservableCollection<Ucionica>(u);



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
            else
            {

                EnableIzbrisiUcionicu = "False";
                EnablePromeniUcionicu = "False";
            }

            if (Predmeti.Count > 0)
            {
                SelectedPredmet = Predmeti[0];
            }
            else
            {
                EnablePromeniPredmet = "False";
                EnableIzbrisiPredmet = "False";
            }

            if (Softveri.Count > 0)
            {
                SelectedSoftver = Softveri[0];
            }
            else
            {
                EnablePromeniSoftver = "False";
                EnableIzbrisiSoftver = "False";
            }

            if (Smerovi.Count > 0)
            {
                SelectedSmer = Smerovi[0];
            }
            else
            {
                EnableIzbrisiSmer = "False";
                EnablePromeniSmer = "False";
            }
            SelectedTabUcionice = true;

            Projektor = new ObservableCollection<string>();
            Projektor.Add("");
            Projektor.Add("Ima");
            Projektor.Add("Nema");
            Tabla = new ObservableCollection<string>();
            Tabla.Add("");
            Tabla.Add("Ima");
            Tabla.Add("Nema");
            PametnaTabla = new ObservableCollection<string>();
            PametnaTabla.Add("");
            PametnaTabla.Add("Ima");
            PametnaTabla.Add("Nema");
            OS = new ObservableCollection<string>();
            OS.Add("");
            OS.Add("Windows");
            OS.Add("Linux");
            OS.Add("Ostalo");

          
            _greskeDodavanje = 0;
            _greskeIzmena = -100;
            SelectedUcionica = new Ucionica();
           
           
           
            demoThread = new Thread(new ThreadStart(popuni));
            demoThread.Start();
            lockO = new Object();


        }

        Thread demoThread;
        Object lockO;

        private void popuni()
        {
            while (true)
            {
                Thread.Sleep(1000);
                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    SacuvajUcionicu.Visibility = Visibility.Visible;
                    OdusatniUcionica.Visibility = Visibility.Visible;
                    
                    GridUcionice.IsEnabled = true;
                    if (Ucionice.Count == 4)
                    {
                        Ucionice.Remove(Ucionice[Ucionice.Count-1]);
                        SelectedUcionica = new Ucionica();
                    }
                    
                });
                GridUcioniceEnable = "True";
                EnablePromeniUcionicu = "False";
                EnableIzbrisiUcionicu = "False";
                EnableDodaj = "False";
                TabPredmeti = "False";
                TabSmer = "False";
                TabSoftver = "False";
                Podaci = "False";
                Thread.Sleep(100);
                SelectedUcionica.Oznaka = "";
                SelectedUcionica.Opis = "";
                SelectedUcionica.BrojRadnihMesta = 16;
                SelectedUcionica.Sistem = "";
                SelectedUcionica.ImaPametnaTabla = false;
                Thread.Sleep(1000);
                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    Box.BorderBrush = new SolidColorBrush(Colors.Blue);
                });
                SelectedUcionica.Oznaka = "L";
                Thread.Sleep(300);
                SelectedUcionica.Oznaka = "L4";
                Thread.Sleep(1000);
                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    Box.BorderBrush = new SolidColorBrush(Colors.Silver);
                });
                Thread.Sleep(200);
                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    OpisBox.BorderBrush = new SolidColorBrush(Colors.Blue);
                });
                SelectedUcionica.Opis = "n";
                Thread.Sleep(300);
                SelectedUcionica.Opis = "no";
                Thread.Sleep(300);
                SelectedUcionica.Opis = "nov";
                Thread.Sleep(300);
                SelectedUcionica.Opis = "nova";
                Thread.Sleep(300);
                SelectedUcionica.Opis = "nova u";
                Thread.Sleep(300);
                SelectedUcionica.Opis = "nova uc";
                Thread.Sleep(300);
                SelectedUcionica.Opis = "nova uci";
                Thread.Sleep(300);
                SelectedUcionica.Opis = "nova ucio";
                Thread.Sleep(300);
                SelectedUcionica.Opis = "nova ucion";
                Thread.Sleep(300);
                SelectedUcionica.Opis = "nova ucioni";
                Thread.Sleep(300);
                SelectedUcionica.Opis = "nova ucionic";
                Thread.Sleep(300);
                SelectedUcionica.Opis = "nova ucionica";
                Thread.Sleep(1000);
                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    OpisBox.BorderBrush = new SolidColorBrush(Colors.Silver);
                });
                Thread.Sleep(200);
                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    BrMestaBox.BorderBrush = new SolidColorBrush(Colors.Blue);
                });
                Thread.Sleep(300);
                SelectedUcionica.BrojRadnihMesta = 3;
                Thread.Sleep(300);
                SelectedUcionica.BrojRadnihMesta = 32;
                Thread.Sleep(600);
                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    BrMestaBox.BorderBrush = new SolidColorBrush(Colors.Silver);
                });
                Thread.Sleep(600);
                Thread.Sleep(600);
                SelectedUcionica.Sistem = "Windows";
                Thread.Sleep(600);
                SelectedUcionica.ImaPametnaTabla = true;
                Thread.Sleep(600);
                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    Ucionice.Add(SelectedUcionica);
                    GridUcionice.IsEnabled = false;
                    SacuvajUcionicu.Visibility = Visibility.Hidden;
                    OdusatniUcionica.Visibility = Visibility.Hidden;
                   
                });
               
                
                EnablePromeniUcionicu = "True";
                EnableIzbrisiUcionicu = "True";
                EnableDodaj = "True";
                TabPredmeti = "True";
                TabSmer = "True";
                TabSoftver = "True";
                Podaci = "True";
                GridUcioniceEnable = "False";
                Thread.Sleep(1000);
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

        public ObservableCollection<Ucionica> SveUcionice
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

        private string _gridUcioniceEnable;
        public string GridUcioniceEnable
        {
            get
            {
                return _gridUcioniceEnable;
            }
            set
            {
                if (_gridUcioniceEnable != value)
                {
                    _gridUcioniceEnable = value;
                    OnPropertyChanged("GridUcioniceEnable");
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
        private void DodajUcionicu_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, GridUcionice);
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate ()
                {
                    Box.Focus();         // Set Logical Focus
                    Keyboard.Focus(Box); // Set Keyboard Focus
                }));
            _greskeDodavanje = 0;
            _greskeIzmena = -100;
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
                SelectRowByIndex(dgrMainUcionica, 0);
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
            FocusManager.SetFocusedElement(this, dgrMainUcionica);

        }
        /**** KLINK NA DUGME SACUVAJ UCIONICU ****/
        private void SacuvajUcionicu_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            Ucionice.Add(SelectedUcionica);
            SveUcionice.Add(SelectedUcionica);
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
            SelectRowByIndex(dgrMainUcionica, Ucionice.Count - 1);

            e.Handled = true;
        }

        /**** KLIK NA DUGME IZBRISI UCIONICU ***/
        private void IzbrisiUcionicu_Click(object sender, RoutedEventArgs e)
        {
            Ucionice.Remove(SelectedUcionica);
            SveUcionice.Remove(SelectedUcionica);

            if (Ucionice.Count <= 0)
            {
                EnableIzbrisiUcionicu = "False";
                EnablePromeniUcionicu = "False";
            }
            sacuvajUcionicu();
        }

        /***** REZIM ZA IZMENU UCIONICE ****/
        private void RezimIzmeniUcionicu_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate ()
                {
                    Box.Focus();
                    Keyboard.Focus(Box);
                }));

            _greskeIzmena = 0;
            _greskeDodavanje = -100;

            _index = Ucionice.IndexOf(SelectedUcionica);
            SelectedUcionica = new Ucionica(SelectedUcionica.Oznaka, SelectedUcionica.Opis,
                SelectedUcionica.BrojRadnihMesta, SelectedUcionica.ImaProjektor,
                SelectedUcionica.ImaTabla, SelectedUcionica.ImaPametnaTabla, SelectedUcionica.Softveri, SelectedUcionica.Sistem);

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
        private void SacuvajIzmenuUcionice_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            MessageBox.Show(SelectedUcionica.Oznaka + " " + SelectedUcionica.Opis + " " + SelectedUcionica.ImaProjektor);

            Ucionice[_index] = SelectedUcionica;
            SveUcionice[_index] = SelectedUcionica;

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



            e.Handled = true;
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

        private void DodajPredmet_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate ()
                {
                    Box.Focus();
                    Keyboard.Focus(Box2);
                }));

            _greskeDodavanje = 0;
            _greskeIzmena = -100;
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
            }
            else
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




            SelectRowByIndex(dgrMainPredmet, Predmeti.Count - 1);

            e.Handled = true;


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

        private void RezimIzmeniPredmet_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate ()
                {
                    Box.Focus();
                    Keyboard.Focus(Box2);
                }));

            _greskeIzmena = 0;
            _greskeDodavanje = -100;

            _index = Predmeti.IndexOf(SelectedPredmet);
            SelectedPredmet = new Predmet(SelectedPredmet.Oznaka, SelectedPredmet.Naziv, SelectedPredmet.Skracenica,
                SelectedPredmet.SmerPredmeta, SelectedPredmet.Opis, SelectedPredmet.VelicinaGrupe,
                SelectedPredmet.DuzinaTermina, SelectedPredmet.BrojTermina, SelectedPredmet.TrebaProjektor,
                SelectedPredmet.TrebaTabla, SelectedPredmet.TrebaPametnaTabla,
                 SelectedPredmet.Softveri, SelectedPredmet.OznakaSmera, SelectedPredmet.Sistem);

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

        private void SacuvajIzmenuPredmet_Executed(object sender, ExecutedRoutedEventArgs e)
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

            e.Handled = true;

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

        private void DodajSmer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate ()
                {
                    Box.Focus();
                    Keyboard.Focus(Box3);
                }));

            _greskeDodavanje = 0;
            _greskeIzmena = -100;

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
            }
            else
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

        private void RezimIzmeniSmer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate ()
                {
                    Box.Focus();
                    Keyboard.Focus(Box3);
                }));

            _greskeIzmena = 0;
            _greskeDodavanje = -100;

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

        private void DodajSoftver_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate ()
                {
                    Box.Focus();
                    Keyboard.Focus(Box4);
                }));

            _greskeDodavanje = 0;
            _greskeIzmena = -100;

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




            SelectRowByIndex(dgrMainSoftver, Softveri.Count - 1);

            e.Handled = true;

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

        private void RezimIzmeniSoftver_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate ()
                {
                    Box.Focus();
                    Keyboard.Focus(Box4);
                }));
            _greskeIzmena = 0;
            _greskeDodavanje = -100;

            _index = Softveri.IndexOf(SelectedSoftver);

            SelectedSoftver = new Softver(SelectedSoftver.Oznaka, SelectedSoftver.Naziv,
                SelectedSoftver.Proizvodjac,
                SelectedSoftver.Sajt, SelectedSoftver.GodinaIzdavanja,
                SelectedSoftver.Cena, SelectedSoftver.Opis, SelectedSoftver.Sistem);

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

        private void IzmeniSoftver_Executed(object sender, ExecutedRoutedEventArgs e)
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

            e.Handled = true;
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

        public void sacuvajUcionicu()
        {
            FileStream f1 = new FileStream("../../Save/ucionica.txt", FileMode.Create);
            f1.Close();

            StreamWriter f = new StreamWriter("../../Save/ucionica.txt");
            // MessageBox.Show("123");
            foreach (Ucionica u in Ucionice)
            {
                f.Write(u.BrojRadnihMesta + "|" + u.ImaPametnaTabla + "|" + u.ImaProjektor + "|" + u.ImaTabla + "|");
                f.Write(u.Sistem);
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

        public List<Ucionica> otvoriUcionicu()
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
                u.Sistem = uc[4];
                u.Opis = uc[5];
                u.Oznaka = uc[6];
                List<Softver> softveri = new List<Softver>();
                foreach (string sof in uc[7].Split(','))
                {
                    Softver s = nadjiSoftver(sof);
                    if (s != null)
                        softveri.Add(s);

                }
                // 
                u.Softveri = softveri;
                // u.Softveri = new ObservableCollection<Softver>( softveri);
                // MessageBox.Show("" + u.Softveri.Count);
                ucionice.Add(u);
            }

            return ucionice;
        }

        public void sacuvajSmer()
        {
            StreamWriter f = new StreamWriter("../../Save/smer.txt");
            foreach (Smer s in Smerovi)
            {
                f.WriteLine(s.Oznaka + "|" + s.Skracenica + "|" + s.Opis + "|" + s.Naziv + "|" + s.DatumUvodjenja);
            }
            f.Close();
        }

        public List<Smer> otvoriSmer()
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

        public void sacuvajSoftver()
        {
            StreamWriter f = new StreamWriter("../../Save/softver.txt");
            foreach (Softver s in Softveri)
            {
                f.Write(s.Oznaka + "|" + s.Naziv + "|" + s.Cena + "|" + s.GodinaIzdavanja + "|");
                f.Write(s.Sistem);
                f.Write("|" + s.Opis + "|" + s.Proizvodjac + "|" + s.Sajt);
                f.Write("\r\n");
            }
            f.Close();
        }

        public List<Softver> otvoriSoftver()
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
                s.Sistem = sf[3];

                s.Opis = sf[4];
                s.Proizvodjac = sf[5];
                s.Sajt = sf[6];

                softveri.Add(s);
            }

            return softveri;
        }

        public void sacuvajPredmet()
        {
            StreamWriter f = new StreamWriter("../../Save/predmet.txt");
            foreach (Predmet p in Predmeti)
            {
                f.Write(p.Naziv + "|" + p.BrojTermina + "|" + p.DuzinaTermina + "|");
                f.Write(p.Sistem);

                f.Write("|" + p.Opis + "|" + p.Oznaka + "|" + p.Skracenica + "|");
                if (p.SmerPredmeta != null)
                    f.Write(p.SmerPredmeta.Oznaka);
                f.Write("|" + p.TrebaPametnaTabla + "|" + p.TrebaProjektor + "|" + p.TrebaTabla + "|" + p.VelicinaGrupe + "|");
                //if (p.Softveri == null || p.Softveri.Count==0)
                foreach (Softver s in p.Softveri)
                {
                    f.Write(s.Oznaka + ",");
                }
                f.WriteLine();
            }
            f.Close();
        }



        public List<Predmet> otvoriPredmet()
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
                p.Sistem = pr[3];

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
                List<Softver> softveri = new List<Softver>();
                foreach (string sof in pr[12].Split(','))
                {
                    Softver s = nadjiSoftver(sof);
                    if (s != null)
                        softveri.Add(s);
                }
                p.Softveri = softveri;
                // MessageBox.Show(""+p.Softveri.Count);
                predmeti.Add(p);

            }

            return predmeti;
        }

        public Smer nadjiSmer(string oznaka)
        {
            //MessageBox.Show(""+Smerovi.Count);
            foreach (Smer s in Smerovi)
            {
                if (s.Oznaka == oznaka)
                    return s;
            }
            return null;
        }

        public Softver nadjiSoftver(string oznaka)
        {
            //MessageBox.Show("" + Softveri.Count);
            foreach (Softver s in Softveri)
            {
                if (s.Oznaka == oznaka)
                    return s;
            }
            return null;
        }

        public Ucionica nadjiUcionicu(string oznaka)
        {
            //MessageBox.Show("" + Softveri.Count);
            foreach (Ucionica u in Ucionice)
            {
                if (u.Oznaka == oznaka)
                    return u;
            }
            return null;
        }

        public Predmet nadjiPredmet(string oznaka)
        {
            //MessageBox.Show("" + Softveri.Count);
            foreach (Predmet p in Predmeti)
            {
                if (p.Oznaka == oznaka)
                    return p;
            }
            return null;
        }

        /****************************************        VALIDATION          **********************************************/
        private int _greskeDodavanje;
        private int _greskeIzmena;

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                _greskeDodavanje++;
                _greskeIzmena++;
            }

            else
            {
                _greskeDodavanje--;
                _greskeIzmena--;
            }
        }


        private void SacuvajPredmet_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_greskeDodavanje == 0)
            {
                if (SelectedPredmet != null)
                {
                    if (SelectedPredmet.Oznaka != "" && SelectedPredmet.Naziv != ""
                        && SelectedPredmet.Skracenica != "")
                    {
                        e.CanExecute = true;
                        e.Handled = true;
                    }
                }

            }
        }


        private void SacuvajUcionicu_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_greskeDodavanje == 0)
            {
                if (SelectedUcionica != null)
                {
                    if (SelectedUcionica.Oznaka != "")
                    {
                        e.CanExecute = true;
                        e.Handled = true;

                    }
                }
            }
        }

        private void SacuvajSoftver_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_greskeDodavanje == 0)
            {
                if (SelectedSoftver != null)
                {
                    if (SelectedSoftver.Oznaka != "")
                    {
                        e.CanExecute = true;
                        e.Handled = true;

                    }
                }
            }

        }

        private void IzmeniPredmet_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _greskeIzmena == 0;
            e.Handled = true;
        }

        private void IzmeniUcionicu_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _greskeIzmena == 0;
            e.Handled = true;
        }

        private void IzmeniSoftver_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _greskeIzmena == 0;
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
            Keyboard.Focus(dataGrid);

        }

        private void DodajUcionicu_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

            if (SelectedTabUcionice == true)
            {
                e.CanExecute = true;
                e.Handled = true;
            }

        }

        private void HandleWindowActivated(object sender, EventArgs e)
        {

            this.Focus();
            FocusManager.SetFocusedElement(this,dgrMainUcionica);

        }

        private void SoftveriOtvori_Click(object sender, RoutedEventArgs e)
        {

            var w = new SoftveriOtvori(SelectedUcionica);

            w.ShowDialog();
            List<Softver> s = w.getList1();
            SelectedUcionica.Softveri = s;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var w = new SoftveriOtvori(SelectedPredmet);

            w.ShowDialog();
            List<Softver> s = w.getList1();
            SelectedPredmet.Softveri = s;
        }

        private void CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // MessageBox.Show("daa" + CB.SelectedItem);

        }

        private bool _selectedTabUcionice;
        public bool SelectedTabUcionice
        {
            get
            {
                return _selectedTabUcionice;
            }
            set
            {
                if (_selectedTabUcionice != value)
                {
                    _selectedTabUcionice = value;
                    OnPropertyChanged("SelectedTabUcionice");
                }
            }
        }

        private bool _selectedTabPredmeti;
        public bool SelectedTabPredmeti
        {
            get
            {
                return _selectedTabPredmeti;
            }
            set
            {
                if (_selectedTabPredmeti != value)
                {
                    _selectedTabPredmeti = value;
                    OnPropertyChanged("SelectedTabPredmeti");
                }
            }
        }

        private bool _selectedTabSmer;
        public bool SelectedTabSmer
        {
            get
            {
                return _selectedTabSmer;
            }
            set
            {
                if (_selectedTabSmer != value)
                {
                    _selectedTabSmer = value;
                    OnPropertyChanged("SelectedTabSmer");
                }
            }
        }

        private bool _selectedTabSoftver;
        public bool SelectedTabSoftver
        {
            get
            {
                return _selectedTabSoftver;
            }
            set
            {
                if (_selectedTabSoftver != value)
                {
                    _selectedTabSoftver = value;
                    OnPropertyChanged("SelectedTabSoftver");
                }
            }
        }

        private void DodajPredmet_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedTabPredmeti == true)
            {
                e.CanExecute = true;
                e.Handled = true;
            }
        }

        private void DodajSmer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedTabSmer == true)
            {
                e.CanExecute = true;
                e.Handled = true;
            }

        }

        private void DodajSoftver_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedTabSoftver == true)
            {
                e.CanExecute = true;
                e.Handled = true;
            }

        }

        private void RezimIzmeniUcionicu_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedUcionica != null)
            {
                e.CanExecute = true;
                e.Handled = true;

            }

        }

        private void RezimIzmeniPredmet_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedPredmet != null)
            {
                e.CanExecute = true;
                e.Handled = true;

            }

        }

        private void RezimIzmeniSmer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedSmer != null)
            {
                e.CanExecute = true;
                e.Handled = true;

            }

        }

        private void RezimIzmeniSoftver_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (SelectedSoftver != null)
            {
                e.CanExecute = true;
                e.Handled = true;

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var p = new IzborSmera(SelectedPredmet);
            p.ShowDialog();
            SelectedPredmet.SmerPredmeta = p.IzabraniSmer;
            //    MessageBox.Show(SelectedPredmet.SmerPredmeta.Naziv);
        }



        public ObservableCollection<Smer> SmeroviPredmeta
        {
            get;
            set;
        }



        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SelectedPredmet != null)
            {
                SelectedPredmet.SmerPredmeta = null;
                //  MessageBox.Show(SelectedPredmet.OznakaSmera);
                foreach (Smer s in Smerovi)
                {
                    if (UnosSmera.Text == s.Oznaka)
                    {
                        SelectedPredmet.SmerPredmeta = s;
                    }
                }
            }

        }

        private string _pretraga;
        public string Pretraga
        {
            get
            {
                return _pretraga;
            }
            set
            {
                if (_pretraga != value)
                {
                    _pretraga = value;
                    OnPropertyChanged("Pretraga");
                }
            }
        }

        private Dictionary<string, Ucionica> pretragaUcionica(List<Ucionica> ucionice, string kriterijum)
        {
            string[] tokens = kriterijum.Split(':');
            string oznaka = tokens[0];

            Dictionary<string, Ucionica> u = new Dictionary<string, Ucionica>();

            //List<Ucionica> u = new List<Ucionica>(); 
            if (oznaka.Trim().ToUpper() == "OZNAKA")
            {
                foreach (Ucionica ucionica in ucionice)
                {
                    if (ucionica.Oznaka.ToUpper() == tokens[1].Trim().ToUpper())
                    {
                        u.Add(ucionica.Oznaka, ucionica);
                    }
                }

            }
            else if (oznaka.Trim().ToUpper() == "OPIS")
            {
                foreach (Ucionica ucionica in ucionice)
                {
                    if (ucionica.Opis.ToUpper() == tokens[1].Trim().ToUpper())
                    {
                        u.Add(ucionica.Oznaka, ucionica);
                    }
                }
            }
            else if (oznaka.Trim().ToUpper() == "BROJ RADNIH MESTA")
            {
                int brojRadnihMesta = -1;
                bool res = Int32.TryParse(tokens[1].Trim(), out brojRadnihMesta);
                if (res)
                {
                    //   int brojRadnihMesta = Int32.Parse(tokens[1].Trim());
                    foreach (Ucionica ucionica in ucionice)
                    {
                        if (ucionica.BrojRadnihMesta == brojRadnihMesta)
                        {
                            u.Add(ucionica.Oznaka, ucionica);
                        }
                    }
                }
            }
            else if (oznaka.Trim().ToUpper() == "OS")
            {

                foreach (Ucionica ucionica in ucionice)
                {
                    if (ucionica.Sistem.ToUpper() == tokens[1].Trim().ToUpper())
                    {
                        u.Add(ucionica.Oznaka, ucionica);
                    }
                }
            }
            return u;

        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //PretragaWindow v = new PretragaWindow();
            // v.Show();
            //  Pretraga = v.Pretraga;

            //   Ucionice = new ObservableCollection<Ucionica>( otvoriUcionicu());
            // MessageBox.Show(Pretraga);
            /*   if (Pretraga != null)
               {
                   Pretraga = Pretraga.ToUpper();
                   if (Pretraga.IndexOf("AND") != -1)
                   {
                       string[] andTokens = Regex.Split(Pretraga,"AND");
                       string[] andTokens1 = andTokens[0].Split(':');
                       string[] andTokens2 = andTokens[1].Split(':');



                   }

               }
               */
            if (Pretraga == "")
            {
                List<Ucionica> u = otvoriUcionicu();
                Ucionice.Clear();
                foreach (Ucionica ucionica in u)
                {
                    Ucionice.Add(ucionica);
                }
            }
            else
            {
                if (Pretraga != null)
                {
                    List<Ucionica> u = otvoriUcionicu();
                    string pretraga = Pretraga.ToUpper();
                    if ((pretraga.IndexOf("AND") != -1) && (pretraga.IndexOf("OR") != -1))
                    {
                        Ucionice.Clear();
                    }
                    else if (pretraga.IndexOf("AND") != -1)
                    {
                        List<Dictionary<string, Ucionica>> dicts = new List<Dictionary<string, Ucionica>>();
                        string[] andTokens = Regex.Split(pretraga, "AND");
                        for (int i = 0; i < andTokens.Count(); i++)
                        {
                            dicts.Add(pretragaUcionica(u, andTokens[i]));
                        }


                        Ucionice.Clear();
                        foreach (Ucionica ucionica in dicts[0].Values)
                        {
                            bool da = true;
                            foreach (Dictionary<string, Ucionica> d in dicts)
                            {
                                if (d.ContainsKey(ucionica.Oznaka))
                                {
                                    da = true;
                                }
                                else
                                {
                                    da = false;
                                    break;
                                }
                            }
                            if (da)
                            {
                                Ucionice.Add(ucionica);

                            }

                        }
                    }
                    else if (pretraga.IndexOf("OR") != -1)
                    {
                        List<Dictionary<string, Ucionica>> dicts = new List<Dictionary<string, Ucionica>>();
                        string[] andTokens = Regex.Split(pretraga, "OR");

                        for (int i = 0; i < andTokens.Count(); i++)
                        {
                            dicts.Add(pretragaUcionica(u, andTokens[i]));
                        }

                        Ucionice.Clear();
                        Dictionary<string, Ucionica> hash = new Dictionary<string, Ucionica>();
                        foreach (Dictionary<string, Ucionica> d in dicts)
                        {
                            foreach (Ucionica ucionica in d.Values)
                            {
                                if (!hash.ContainsKey(ucionica.Oznaka))
                                {
                                    Ucionice.Add(ucionica);
                                    hash.Add(ucionica.Oznaka, ucionica);
                                }
                            }
                        }
                    }

                    else
                    {
                        Dictionary<string, Ucionica> pretragaUc = pretragaUcionica(u, Pretraga);

                        Ucionice.Clear();
                        foreach (Ucionica ucionica in pretragaUc.Values)
                        {
                            Ucionice.Add(ucionica);

                        }

                    }
                }
            }

           
            
            


        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            GridPretraga.Visibility = Visibility.Visible;
            ButtonPretraga.Visibility = Visibility.Collapsed;
            ButtonFilter.Visibility = Visibility.Collapsed;
        }

        private void ButtonOtkazi_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            List<Ucionica> u = otvoriUcionicu();
            Ucionice.Clear();
            foreach(Ucionica ucionica in u)
            {
                Ucionice.Add(ucionica);
            }

            GridPretraga.Visibility = Visibility.Collapsed;
            ButtonPretraga.Visibility = Visibility.Visible;
            ButtonFilter.Visibility = Visibility.Visible;
            GridFilter.Visibility = Visibility.Collapsed;


        }

        private void ButtonFilter_Click(object sender, RoutedEventArgs e)
        {
            GridFilter.Visibility = Visibility.Visible;
            ButtonFilter.Visibility = Visibility.Collapsed;
            ButtonPretraga.Visibility = Visibility.Collapsed;
        }

        public ObservableCollection<string> Projektor
        {
            get;
            set;
        }
        public ObservableCollection<string> Tabla
        {
            get;
            set;
        }
        public ObservableCollection<string> PametnaTabla
        {
            get;
            set;
        }
        public ObservableCollection<string> OS
        {
            get;
            set;
        }

        private string _izabraniProjektor;
        public string IzabraniProjektor
        {
            get
            {
                return _izabraniProjektor;
            }
            set
            {
                if (_izabraniProjektor != value)
                {
                    _izabraniProjektor = value;
                    OnPropertyChanged("IzabraniProjektor");
                }
            }
        }

        private string _izabranaTabla;
        public string IzabranaTabla
        {
            get
            {
                return _izabranaTabla;
            }
            set
            {
                if (_izabranaTabla != value)
                {
                    _izabranaTabla = value;
                    OnPropertyChanged("IzabranaTabla");
                }
            }
        }

        private string _izabranaPametnaTabla;
        public string IzabranaPametnaTabla
        {
            get
            {
                return _izabranaPametnaTabla;
            }
            set
            {
                if (_izabranaPametnaTabla != value)
                {
                    _izabranaPametnaTabla = value;
                    OnPropertyChanged("IzabranaTabla");
                }
            }
        }

        private string _izabraniOS;
        public string IzabraniOS
        {
            get
            {
                return _izabraniOS;
            }
            set
            {
                if (_izabraniOS != value)
                {
                    _izabraniOS= value;
                    OnPropertyChanged("IzabraniOS");
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // MessageBox.Show(IzabraniOS);
            Ucionice.Clear();
            List<Ucionica> u = otvoriUcionicu();
            if (IzabraniOS == "")
            {
                foreach (Ucionica ucionica in u)
                {

                    Ucionice.Add(ucionica);


                }
            }
            else
            {
                foreach (Ucionica ucionica in u)
                {
                    if (ucionica.Sistem.ToUpper() == IzabraniOS.ToUpper())
                    {
                        Ucionice.Add(ucionica);
                    }

                }
            }


        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            Ucionice.Clear();
            List<Ucionica> u = otvoriUcionicu();
            if (IzabraniProjektor == "")
            {
                foreach (Ucionica ucionica in u)
                {

                    Ucionice.Add(ucionica);


                }
            }
            else
            {
                foreach (Ucionica ucionica in u)
                {
                    if (IzabraniProjektor == "Ima")
                    {
                        if (ucionica.ImaProjektor)
                        {
                            Ucionice.Add(ucionica);
                        }
                    }
                    if (IzabraniProjektor == "Nema")
                    {
                        if (!ucionica.ImaProjektor)
                        {
                            Ucionice.Add(ucionica); 

                        }
                    }

                }
            }


        }

        private void ComboBox_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {
            Ucionice.Clear();
            List<Ucionica> u = otvoriUcionicu();
            if (IzabranaTabla == "")
            {
                foreach (Ucionica ucionica in u)
                {

                    Ucionice.Add(ucionica);


                }
            }
            else
            {
                foreach (Ucionica ucionica in u)
                {
                    if (IzabranaTabla == "Ima")
                    {
                        if (ucionica.ImaTabla)
                        {
                            Ucionice.Add(ucionica);
                        }
                    }
                    if (IzabranaTabla == "Nema")
                    {
                        if (!ucionica.ImaTabla)
                        {
                            Ucionice.Add(ucionica);

                        }
                    }

                }
            }

        }

        private void ComboBox_SelectionChanged_3(object sender, SelectionChangedEventArgs e)
        {
            Ucionice.Clear();
            List<Ucionica> u = otvoriUcionicu();
            if (IzabranaPametnaTabla == "")
            {
                foreach (Ucionica ucionica in u)
                {

                    Ucionice.Add(ucionica);


                }
            }
            else
            {
                foreach (Ucionica ucionica in u)
                {
                    if (IzabranaPametnaTabla == "Ima")
                    {
                        if (ucionica.ImaPametnaTabla)
                        {
                            Ucionice.Add(ucionica);
                        }
                    }
                    if (IzabranaPametnaTabla == "Nema")
                    {
                        if (!ucionica.ImaPametnaTabla)
                        {
                            Ucionice.Add(ucionica);

                        }
                    }

                }
            }

        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[1]);
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                if (SelectedTabPredmeti && str == "Tabele")
                {
                    str = "PrikazPredmeta";
                }
                if (SelectedTabSmer && str == "Tabele")
                {
                    str = "PrikazSmera";
                }

                if (SelectedTabSoftver && str == "Tabele")
                {
                    str = "PrikazSoftvera";
                }

                if (str == "index")
                {
                    if (SelectedTabUcionice)
                    {
                        str = "Tabele";
                    }else if (SelectedTabPredmeti)
                    {
                        str = "PrikazPredmeta";
                    }else if (SelectedTabSmer)
                    {
                        str = "PrikazSmera";
                    }else if (SelectedTabSoftver)
                    {
                        str = "PrikazSoftvera";
                    }
                }

                if (str == "Tab")
                {
                    if (SelectedTabUcionice)
                    {
                        str = "Tabele";
                    }else if (SelectedTabPredmeti)
                    {
                        str = "PrikazPredmeta";

                    }else if (SelectedTabSmer)
                    {
                        str = "PrikazSmera";
                    }else if (SelectedTabSoftver)
                    {
                        str = "PrikazSoftvera";
                    }
                }
                HelpProvider.ShowHelp(str, this);
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedTabPredmeti)
            {
                FocusManager.SetFocusedElement(this,TabPr);

            }
            else if (SelectedTabSmer)
            {
                FocusManager.SetFocusedElement(this, dgrMainSmer);

            }
            else if (SelectedTabSoftver)
            {
                FocusManager.SetFocusedElement(this, dgrMainSoftver);

            }
            else if (SelectedTabUcionice)
            {
                FocusManager.SetFocusedElement(this,TabUc);

            }

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (demoThread != null)
            {
                demoThread.Abort();
            }

        }
    }

}
