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
using System.Windows.Automation;

namespace Raspored.Tabele
{
    /// <summary>
    /// Interaction logic for Tabele.xaml
    /// </summary>
    public partial class Tabele : Window, INotifyPropertyChanged
    {
        public Tabele(Model.Raspored raspored)
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

            SacuvajUcionicuDemo.Visibility = Visibility.Collapsed;
            OdusatniUcionicaDemo.Visibility = Visibility.Collapsed;

            DodajDemo.Visibility = Visibility.Collapsed;
            IzmeniDemo.Visibility = Visibility.Collapsed;
            ObrisiDemo.Visibility = Visibility.Collapsed;
            SoftveriOtvoriDemo.Visibility = Visibility.Collapsed;
            ButtonFilterDemo.Visibility = Visibility.Collapsed;
            ButtonPretragaDemo.Visibility = Visibility.Collapsed;

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
            OS.Add("Oba");
            OSSoftver = new ObservableCollection<string>();
            OSSoftver.Add("");
            OSSoftver.Add("Windows");
            OSSoftver.Add("Linux");
            OSSoftver.Add("Cross-platform");
            ProjektorPredmet = new ObservableCollection<string>();
            ProjektorPredmet.Add("");
            ProjektorPredmet.Add("Ima");
            ProjektorPredmet.Add("Nema");
            TablaPredmet = new ObservableCollection<string>();
            TablaPredmet.Add("");
            TablaPredmet.Add("Ima");
            TablaPredmet.Add("Nema");
            PametnaTablaPredmet = new ObservableCollection<string>();
            PametnaTablaPredmet.Add("");
            PametnaTablaPredmet.Add("Ima");
            PametnaTablaPredmet.Add("Nema");
            OSPredmet = new ObservableCollection<string>();
            OSPredmet.Add("");
            OSPredmet.Add("Windows");
            OSPredmet.Add("Linux");
            OSPredmet.Add("Svejedno");

            demoThread = null;
            _greskaOznaka = false;
            _index = -1;

            _raspored = raspored;


        }

        private Model.Raspored _raspored;

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
            SacuvajUcionicu.Visibility = Visibility.Collapsed;
            OdusatniUcionica.Visibility = Visibility.Collapsed;
            DodajButton.Visibility = Visibility.Collapsed;
            IzmeniButton.Visibility = Visibility.Collapsed;
            ObrisiButton.Visibility = Visibility.Collapsed;
            SoftveriOtvori.Visibility = Visibility.Collapsed;
            ButtonPretraga.Visibility = Visibility.Collapsed;
            ButtonFilter.Visibility = Visibility.Collapsed;

            Box.IsReadOnly = true;
            OpisBox.IsReadOnly = true;
            BrMestaBox.IsReadOnly = true;
            Box.Background = new SolidColorBrush(Colors.White);
            OpisBox.Background = new SolidColorBrush(Colors.White);
            BrMestaBox.Background = new SolidColorBrush(Colors.White);
           /* Check1.IsEnabled = false;
            Check2.IsEnabled = false;
            Check3.IsEnabled = false;
            Check1.Background = new SolidColorBrush(Colors.White);
            Check2.Background = new SolidColorBrush(Colors.White);
            Check3.Background = new SolidColorBrush(Colors.White);
            */
        }

        Thread demoThread;

        private void popuni()
        {
            while (true)
            {
                Thread.Sleep(1000);
                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    if (Ucionice.Count == 4)
                    {
                        Ucionice.Remove(Ucionice[Ucionice.Count - 1]);
                        SelectedUcionica = Ucionice[Ucionice.Count - 1];
                        
                    }
                });
                Thread.Sleep(200);

                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    DodajDemo.Background = new SolidColorBrush(Colors.LightSkyBlue);
                });
                Thread.Sleep(600);
                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    DodajDemo.Background = new SolidColorBrush(Colors.LightGray);


                });

                Thread.Sleep(200);
                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    SacuvajUcionicuDemo.Visibility = Visibility.Visible;
                    OdusatniUcionicaDemo.Visibility = Visibility.Visible;
                    
                    GridUcionice.IsEnabled = true;
                    SelectedUcionica = new Ucionica();

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
                SelectedUcionica.BrojRadnihMesta = 1;
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
                    SacuvajUcionicuDemo.Background = new SolidColorBrush(Colors.LightSkyBlue);
                   

                });
                Thread.Sleep(600);
                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    SacuvajUcionicuDemo.Background = new SolidColorBrush(Colors.LightGray);


                });
                Thread.Sleep(200);

                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    
                    Ucionice.Add(SelectedUcionica);
                    GridUcionice.IsEnabled = false;
                    SacuvajUcionicuDemo.Visibility = Visibility.Hidden;
                    OdusatniUcionicaDemo.Visibility = Visibility.Hidden;
                });
               
                EnablePromeniUcionicu = "True";
                EnableIzbrisiUcionicu = "True";
                EnableDodaj = "True";
                TabPredmeti = "True";
                TabSmer = "True";
                TabSoftver = "True";
                Podaci = "True";
                GridUcioniceEnable = "False";
                Thread.Sleep(2000);
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
            _index = -1;
            GridPretraga.Visibility = Visibility.Collapsed;
            ButtonPretraga.Visibility = Visibility.Visible;
            ButtonFilter.Visibility = Visibility.Visible;
            GridFilter.Visibility = Visibility.Collapsed;
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

            EUcionice.Visibility = Visibility.Collapsed;
            _index = -1;
            Box.BorderBrush = new SolidColorBrush(Colors.Silver);

        }
        /**** KLINK NA DUGME SACUVAJ UCIONICU ****/
        private void SacuvajUcionicu_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            EUcionice.Visibility = Visibility.Collapsed;

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
            _index = -1;
            e.Handled = true;

        }

        /**** KLIK NA DUGME IZBRISI UCIONICU ***/
        private void IzbrisiUcionicu_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            MessageBoxResult res = MessageBox.Show(
                "Da li ste sigurni da želite da obišete učionicu koja ima oznaku  " + 
                SelectedUcionica.Oznaka + "?", "Brisanje učionice", MessageBoxButton.YesNo, 
                MessageBoxImage.Warning, MessageBoxResult.No);
            if (res == MessageBoxResult.Yes)
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
        }

        /***** REZIM ZA IZMENU UCIONICE ****/
        private void RezimIzmeniUcionicu_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            GridPretraga.Visibility = Visibility.Collapsed;
            ButtonPretraga.Visibility = Visibility.Visible;
            ButtonFilter.Visibility = Visibility.Visible;
            GridFilter.Visibility = Visibility.Collapsed;
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
                SelectedUcionica.ImaTabla, SelectedUcionica.ImaPametnaTabla, 
                SelectedUcionica.Softveri, SelectedUcionica.Sistem);

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
            _index = -1;
            e.Handled = true;
        }

        /**** KLINK NA DUGME PONISTI IZMENU UCIONICE ****/
        private void IzmenaOdustaniUcionica_Click(object sender, RoutedEventArgs e)
        {
            SelectedUcionica = Ucionice[_index];
            _index = -1;

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
            EUcionice.Visibility = Visibility.Collapsed;
            Box.BorderBrush = new SolidColorBrush(Colors.Silver);

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
            _index = -1;
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

            Naziv1.BorderBrush = new SolidColorBrush(Colors.Silver);
            Skracenica1.BorderBrush = new SolidColorBrush(Colors.Silver);
            Box2.BorderBrush = new SolidColorBrush(Colors.Silver);
            EPredmeti.Visibility = Visibility.Collapsed;
            _index = -1;

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
            _index = -1;


        }

        private void IzbrisiPredmet_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show(
               "Da li ste sigurni da želite da obišete predmet " +
               SelectedPredmet.Naziv + "?", "Brisanje predmeta", MessageBoxButton.YesNo,
               MessageBoxImage.Warning, MessageBoxResult.No);
            if (res == MessageBoxResult.Yes)
            {
                Predmeti.Remove(SelectedPredmet);
                if (Predmeti.Count <= 0)
                {
                    EnableIzbrisiPredmet = "False";
                    EnablePromeniPredmet = "False";
                }
                sacuvajPredmet();
            }
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
            _index = -1;
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
            _index = -1;
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

            Naziv1.BorderBrush = new SolidColorBrush(Colors.Silver);
            Skracenica1.BorderBrush = new SolidColorBrush(Colors.Silver);
            Box2.BorderBrush = new SolidColorBrush(Colors.Silver);
            EPredmeti.Visibility = Visibility.Collapsed;

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
            _index = -1;
            ESmerovi.Visibility = Visibility.Collapsed;
            Box3.BorderBrush = new SolidColorBrush(Colors.Silver);
            Naziv2.BorderBrush = new SolidColorBrush(Colors.Silver);
            Skracenica2.BorderBrush = new SolidColorBrush(Colors.Silver);
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
            _index = -1;

        }

        private void IzbrisiSmer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string poruka = "";
            bool i = false;
            foreach(Predmet p in Predmeti)
            {
                if (p.SmerPredmeta.Oznaka == SelectedSmer.Oznaka)
                {
                    if (i)
                    {
                        poruka += "\n    - " + p.Naziv;
                    }
                    else
                    {
                        poruka = "Ukoliko izbrišete smer " + SelectedSmer.Naziv + "\n" +
                            "Obrisaćete smer predmetima:" + "\n    - " +
                            p.Naziv;
                        i = true;
                    }
                    

                }
            }
            MessageBoxResult res;
            if (i)
            {
                poruka += "\n\nDa li ste sigurni da želite da nastavite? ";
                res = MessageBox.Show(
                    poruka, "Brisanje smera", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning, MessageBoxResult.No);
            }else
            {
                res = MessageBox.Show(
                    "Da li ste sigurni da želite da obišete smer " +
                    SelectedSmer.Naziv + "?",
                    "Brisanje smera", MessageBoxButton.YesNo,
                   MessageBoxImage.Warning, MessageBoxResult.No);

            }
            if (res == MessageBoxResult.Yes)
            {
                foreach(Predmet p in Predmeti)
                {
                    if (p.SmerPredmeta.Oznaka == SelectedSmer.Oznaka)
                    {
                        p.OznakaSmera = "";
                        p.SmerPredmeta = null;
                    }
                }
                Smerovi.Remove(SelectedSmer);
                if (Smerovi.Count <= 0)
                {
                    EnableIzbrisiSmer = "False";
                    EnablePromeniSmer = "False";
                }
                sacuvajPredmet();
                sacuvajSmer();
            }
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
            _index = -1;

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
            _index = -1;
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
            ESmerovi.Visibility = Visibility.Collapsed;
            Box3.BorderBrush = new SolidColorBrush(Colors.Silver);
            Naziv2.BorderBrush = new SolidColorBrush(Colors.Silver);
           Skracenica2.BorderBrush = new SolidColorBrush(Colors.Silver);

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
            _index = -1;

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
            _index = -1;

            Naziv3.BorderBrush = new SolidColorBrush(Colors.Silver);
            Box4.BorderBrush = new SolidColorBrush(Colors.Silver);
            ESoftveri.Visibility = Visibility.Collapsed;

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
            _index = -1;

        }

        private void IzbrisiSoftver_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string poruka = "";
            bool i = false;
            bool j = false;
            foreach (Predmet p in Predmeti)
            {
                foreach(Softver s in p.Softveri) {
                    if (s.Oznaka == SelectedSoftver.Oznaka)
                    {
                        if (i)
                        {
                            poruka += "\n    - " + s.Naziv;
                        }
                        else
                        {
                            poruka = "Ukoliko izbrišete softver " + SelectedSoftver.Naziv + "\n" +
                                "Uklonićete ga iz liste softvera kod predmeta: " + "\n    - " +
                                p.Naziv;
                            i = true;
                        }

                    }
                }
            }

            foreach (Ucionica u in Ucionice)
            {
                foreach (Softver s in u.Softveri)
                {
                    if (s.Oznaka == SelectedSoftver.Oznaka)
                    {
                        if (j)
                        {
                            poruka += "\n    - " + s.Naziv;
                        }
                        else
                        {
                            if (i)
                            {
                                poruka += "\n\n" +
                                    "Uklonićete ga iz liste softvera u učionicama: " + "\n    - " +
                                    u.Oznaka;
                            }else
                            {
                                poruka = "Ukoliko izbrišete softver " + SelectedSoftver.Naziv + "\n" +
                                   "Uklonićete ga iz liste softvera u učionicama: " + "\n    - " +
                                   u.Oznaka;

                            }
                            j = true;
                        }

                    }
                }
            }
            MessageBoxResult res;
            if (i || j)
            {

                poruka += "\n\nDa li ste sigurni da želite da nastavite? ";
                res = MessageBox.Show(
                  poruka, "Brisanje softvera", MessageBoxButton.YesNo,
                  MessageBoxImage.Warning, MessageBoxResult.No);
            }else
            {

                res = MessageBox.Show(
                 "Da li ste sigurni da želite da obišete softver koja ima oznaku  " +
                SelectedSoftver.Oznaka + "?", "Brisanje softvera", MessageBoxButton.YesNo,
                  MessageBoxImage.Warning, MessageBoxResult.No);

            }
            if (res == MessageBoxResult.Yes)
            {
             /*   foreach (Predmet p in Predmeti)
                {
                    foreach (Softver s in p.Softveri)
                    {
                        if (s.Oznaka == SelectedSoftver.Oznaka)
                        {
                            p.Softveri.Remove(s);

                        }
                    }
                }
                foreach (Ucionica u in Ucionice)
                {
                    foreach (Softver s in u.Softveri)
                    {
                        if (s.Oznaka == SelectedSoftver.Oznaka)
                        {
                            u.Softveri.Remove(s);

                        }
                    }
                }*/
                Softveri.Remove(SelectedSoftver);
                if (Softveri.Count <= 0)
                {
                    EnableIzbrisiSoftver = "False";
                    EnablePromeniSoftver = "False";
                }
                sacuvajSoftver();

            }

        }

        private void RezimIzmeniSoftver_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate ()
                {
                    Box4.Focus();
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
            _index = -1;
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
            _index = -1;
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

            Naziv3.BorderBrush = new SolidColorBrush(Colors.Silver);
            Box4.BorderBrush = new SolidColorBrush(Colors.Silver);
            ESoftveri.Visibility = Visibility.Collapsed;

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
                s.GodinaIzdavanja = Convert.ToInt32(sf[3]);
                s.Sistem = sf[4];

                s.Opis = sf[5];
                s.Proizvodjac = sf[6];
                s.Sajt = sf[7].Trim();

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
                if (!_greskaOznaka)
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
        }


        private void SacuvajUcionicu_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_greskeDodavanje == 0)
            {
                if (!_greskaOznaka)
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

            if (_greskeIzmena == 0)
            {
                if (!_greskaOznaka)
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
            }else if (oznaka.Trim().ToUpper() == "PROJEKTOR")
            {

                foreach (Ucionica ucionica in ucionice)
                {
                    if (tokens[1].Trim().ToUpper() == "IMA")
                    {
                        if (ucionica.ImaProjektor)
                        {
                            u.Add(ucionica.Oznaka, ucionica);
                        }

                    }
                    if (tokens[1].Trim().ToUpper() == "NEMA")
                    {
                        if (!ucionica.ImaProjektor)
                        {
                            u.Add(ucionica.Oznaka, ucionica);
                        }
                    }
                }
            }
            else if (oznaka.Trim().ToUpper() == "TABLA")
            {
                foreach (Ucionica ucionica in ucionice)
                {
                    if (tokens[1].Trim().ToUpper() == "IMA")
                    {
                        if (ucionica.ImaTabla)
                        {
                            u.Add(ucionica.Oznaka, ucionica);
                        }

                    }
                    if (tokens[1].Trim().ToUpper() == "NEMA")
                    {
                        if (!ucionica.ImaTabla)
                        {
                            u.Add(ucionica.Oznaka, ucionica);
                        }
                    }
                }

            }
            else if (oznaka.Trim().ToUpper() == "PAMETNA TABLA")
            {
                foreach (Ucionica ucionica in ucionice)
                {
                    if (tokens[1].Trim().ToUpper() == "IMA")
                    {
                        if (ucionica.ImaPametnaTabla)
                        {
                            u.Add(ucionica.Oznaka, ucionica);
                        }

                    }
                    if (tokens[1].Trim().ToUpper() == "NEMA")
                    {
                        if (!ucionica.ImaPametnaTabla)
                        {
                            u.Add(ucionica.Oznaka, ucionica);
                        }
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
                    if ((pretraga.IndexOf(" AND ") != -1) && (pretraga.IndexOf(" OR ") != -1))
                    {
                        Ucionice.Clear();
                    }
                    else if (pretraga.IndexOf(" AND ") != -1)
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
                    else if (pretraga.IndexOf(" OR ") != -1)
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
        public ObservableCollection<string> ProjektorPredmet
        {
            get;
            set;
        }
        public ObservableCollection<string> Tabla
        {
            get;
            set;
        }
        public ObservableCollection<string> TablaPredmet
        {
            get;
            set;
        }
        public ObservableCollection<string> PametnaTabla
        {
            get;
            set;
        }
        public ObservableCollection<string> PametnaTablaPredmet
        {
            get;
            set;
        }
        public ObservableCollection<string> OS
        {
            get;
            set;
        }
        public ObservableCollection<string> OSPredmet
        {
            get;
            set;
        }

        public ObservableCollection<string> OSSoftver
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
        private string _izabraniProjektorPredmet;
        public string IzabraniProjektorPredmet
        {
            get
            {
                return _izabraniProjektorPredmet;
            }
            set
            {
                if (_izabraniProjektorPredmet != value)
                {
                    _izabraniProjektorPredmet = value;
                    OnPropertyChanged("IzabraniProjektorPredmet");
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
        private string _izabranaTablaPredmet;
        public string IzabranaTablaPredmet
        {
            get
            {
                return _izabranaTablaPredmet;
            }
            set
            {
                if (_izabranaTablaPredmet != value)
                {
                    _izabranaTablaPredmet = value;
                    OnPropertyChanged("IzabranaTablaPredmet");
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
                    OnPropertyChanged("IzabranaPametnaTabla");
                }
            }
        }
        private string _izabranaPametnaTablaPredmet;
        public string IzabranaPametnaTablaPredmet
        {
            get
            {
                return _izabranaPametnaTablaPredmet;
            }
            set
            {
                if (_izabranaPametnaTablaPredmet != value)
                {
                    _izabranaPametnaTablaPredmet = value;
                    OnPropertyChanged("IzabranaPametnaTablaPredmet");
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
        private string _izabraniOSPredmet;
        public string IzabraniOSPredmet
        {
            get
            {
                return _izabraniOSPredmet;
            }
            set
            {
                if (_izabraniOSPredmet != value)
                {
                    _izabraniOSPredmet = value;
                    OnPropertyChanged("IzabraniOSPredmet");
                }
            }
        }
        private string _izabraniOSSoftver;
        public string IzabraniOSSoftver
        {
            get
            {
                return _izabraniOSSoftver;
            }
            set
            {
                if (_izabraniOSSoftver != value)
                {
                    _izabraniOSSoftver = value;
                    OnPropertyChanged("IzabraniOSSoftver");
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
                FocusManager.SetFocusedElement(this,dgrMainPredmet);

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
                FocusManager.SetFocusedElement(this,dgrMainUcionica);

            }

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (demoThread != null)
            {
                demoThread.Abort();
            }

        }

        private bool _greskaOznaka;

        private void Box_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Box.Text == "")
            {
                Box.BorderBrush = new SolidColorBrush(Colors.Red);

            }
            else
            {
                Box.BorderBrush = new SolidColorBrush(Colors.Silver);

                bool ima = false;
                for (int i = 0; i < Ucionice.Count; i++)
                {
                    if (_index == i)
                    {
                        continue;
                    }
                    if (Ucionice[i].Oznaka == Box.Text)
                    {
                        ima = true;
                        break;
                    }
                    
                }


                if (!ima)
                {
                    EUcionice.Visibility = Visibility.Collapsed;
                    _greskaOznaka = false;

                }
                else
                {
                    EUcionice.Text = " Oznaka ucionice mora biti jedinstvena.";
                    EUcionice.Visibility = Visibility.Visible;
                    _greskaOznaka = true;
                }
            }

        }

        private void ButtonPretragaPredmet_Click(object sender, RoutedEventArgs e)
        {
            GridPretragaPredmet.Visibility = Visibility.Visible;
            ButtonPretragaPredmet.Visibility = Visibility.Collapsed;
            ButtonFilterPredmet.Visibility = Visibility.Collapsed;
        }

        private void ButtonFilterPredmet_Click(object sender, RoutedEventArgs e)
        {
            GridFilterPredmet.Visibility = Visibility.Visible;
            ButtonFilterPredmet.Visibility = Visibility.Collapsed;
            ButtonPretragaPredmet.Visibility = Visibility.Collapsed;

        }

        private void ButtonFilterSmer_Click(object sender, RoutedEventArgs e)
        {
            GridFilterSmer.Visibility = Visibility.Visible;
            ButtonFilterSmer.Visibility = Visibility.Collapsed;
            ButtonPretragaSmer.Visibility = Visibility.Collapsed;

        }

        private void ButtonPretragaSmer_Click(object sender, RoutedEventArgs e)
        {
            GridPretragaSmer.Visibility = Visibility.Visible;
            ButtonPretragaSmer.Visibility = Visibility.Collapsed;
            ButtonFilterSmer.Visibility = Visibility.Collapsed;

        }

        private void ButtonFilterSoftver_Click(object sender, RoutedEventArgs e)
        {
            GridFilterSoftver.Visibility = Visibility.Visible;
            ButtonFilterSoftver.Visibility = Visibility.Collapsed;
            ButtonPretragaSoftver.Visibility = Visibility.Collapsed;
        }

        private void ButtonPretragaSoftver_Click(object sender, RoutedEventArgs e)
        {
            GridPretragaSoftver.Visibility = Visibility.Visible;
            ButtonPretragaSoftver.Visibility = Visibility.Collapsed;
            ButtonFilterSoftver.Visibility = Visibility.Collapsed;

        }


        private void ButtonOtkaziPredmet_Click(object sender, RoutedEventArgs e)
        {
            List<Predmet> u = otvoriPredmet();
            Predmeti.Clear();
            foreach (Predmet predmet in u)
            {
                Predmeti.Add(predmet);
            }

            GridPretragaPredmet.Visibility = Visibility.Collapsed;
            ButtonPretragaPredmet.Visibility = Visibility.Visible;
            ButtonFilterPredmet.Visibility = Visibility.Visible;
            GridFilterPredmet.Visibility = Visibility.Collapsed;

        }

       

        private void ButtonOtkaziSmer_Click(object sender, RoutedEventArgs e)
        {
            List<Smer> u = otvoriSmer();
            Smerovi.Clear();
            foreach (Smer smer in u)
            {
                Smerovi.Add(smer);
            }

            GridPretragaSmer.Visibility = Visibility.Collapsed;
            ButtonPretragaSmer.Visibility = Visibility.Visible;
            ButtonFilterSmer.Visibility = Visibility.Visible;
            GridFilterSmer.Visibility = Visibility.Collapsed;

        }

       

        private void ButtonOtkaziSoftver_Click(object sender, RoutedEventArgs e)
        {
            List<Softver> u = otvoriSoftver();
            Softveri.Clear();
            foreach (Softver softver in u)
            {
                Softveri.Add(softver);
            }

            GridPretragaSoftver.Visibility = Visibility.Collapsed;
            ButtonPretragaSoftver.Visibility = Visibility.Visible;
            ButtonFilterSoftver.Visibility = Visibility.Visible;
            GridFilterSoftver.Visibility = Visibility.Collapsed;

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (Pretraga == "")
            {
                List<Predmet> u = otvoriPredmet();
                Predmeti.Clear();
                foreach (Predmet predmet in u)
                {
                    Predmeti.Add(predmet);
                }
            }
            else
            {
                if (Pretraga != null)
                {
                    List<Predmet> u = otvoriPredmet();
                    string pretraga = Pretraga.ToUpper();
                    if ((pretraga.IndexOf(" AND ") != -1) && (pretraga.IndexOf(" OR ") != -1))
                    {
                        Predmeti.Clear();
                    }
                    else if (pretraga.IndexOf(" AND ") != -1)
                    {
                        List<Dictionary<string, Predmet>> dicts = new List<Dictionary<string,Predmet>>();
                        string[] andTokens = Regex.Split(pretraga, "AND");
                        for (int i = 0; i < andTokens.Count(); i++)
                        {
                            dicts.Add(pretragaPredmeta(u, andTokens[i]));
                        }


                        Predmeti.Clear();
                        foreach (Predmet predmet in dicts[0].Values)
                        {
                            bool da = true;
                            foreach (Dictionary<string, Predmet> d in dicts)
                            {
                                if (d.ContainsKey(predmet.Oznaka))
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
                                Predmeti.Add(predmet);

                            }

                        }
                    }
                    else if (pretraga.IndexOf(" OR ") != -1)
                    {
                        List<Dictionary<string, Predmet>> dicts = new List<Dictionary<string, Predmet>>();
                        string[] andTokens = Regex.Split(pretraga, "OR");

                        for (int i = 0; i < andTokens.Count(); i++)
                        {
                            dicts.Add(pretragaPredmeta(u, andTokens[i]));
                        }

                        Predmeti.Clear();
                        Dictionary<string, Predmet> hash = new Dictionary<string, Predmet>();
                        foreach (Dictionary<string, Predmet> d in dicts)
                        {
                            foreach (Predmet predmet in d.Values)
                            {
                                if (!hash.ContainsKey(predmet.Oznaka))
                                {
                                    Predmeti.Add(predmet);
                                    hash.Add(predmet.Oznaka, predmet);
                                }
                            }
                        }
                    }

                    else
                    {
                        Dictionary<string, Predmet> pretragaUc = pretragaPredmeta(u, Pretraga);

                        Predmeti.Clear();
                        foreach (Predmet predmet in pretragaUc.Values)
                        {
                            Predmeti.Add(predmet);

                        }

                    }
                }
            }

        }

        private Dictionary<string, Predmet> pretragaPredmeta(List<Predmet> predmeti, string kriterijum)
        {
            string[] tokens = kriterijum.Split(':');
            string oznaka = tokens[0];

            Dictionary<string, Predmet> u = new Dictionary<string, Predmet>();

            //List<Ucionica> u = new List<Ucionica>(); 
            if (oznaka.Trim().ToUpper() == "OZNAKA")
            {
                foreach (Predmet predmet in predmeti)
                {
                    if (predmet.Oznaka.ToUpper() == tokens[1].Trim().ToUpper())
                    {
                        u.Add(predmet.Oznaka, predmet);
                    }
                }

            }
            else if (oznaka.Trim().ToUpper() == "OPIS")
            {
                foreach (Predmet predmet in predmeti)
                {
                    if (predmet.Opis.ToUpper() == tokens[1].Trim().ToUpper())
                    {
                        u.Add(predmet.Oznaka, predmet);
                    }
                }
            }
            else if (oznaka.Trim().ToUpper() == "BROJ TERMINA")
            {
                int broj = -1;
                bool res = Int32.TryParse(tokens[1].Trim(), out broj);
                if (res)
                {
                    //   int brojRadnihMesta = Int32.Parse(tokens[1].Trim());
                    foreach (Predmet predmet in predmeti)
                    {
                        if (predmet.BrojTermina == broj)
                        {
                            u.Add(predmet.Oznaka, predmet);
                        }
                    }
                }
            }
            else if (oznaka.Trim().ToUpper() == "DUZINA TERMINA")
            {
                int broj = -1;
                bool res = Int32.TryParse(tokens[1].Trim(), out broj);
                if (res)
                {
                    
                    foreach (Predmet predmet in predmeti)
                    {
                        if (predmet.DuzinaTermina == broj)
                        {
                            u.Add(predmet.Oznaka, predmet);
                        }
                    }
                }
            }
            else if (oznaka.Trim().ToUpper() == "VELICINA GRUPE")
            {
                int broj = -1;
                bool res = Int32.TryParse(tokens[1].Trim(), out broj);
                if (res)
                {
                    
                    foreach (Predmet predmet in predmeti)
                    {
                        if (predmet.VelicinaGrupe == broj)
                        {
                            u.Add(predmet.Oznaka, predmet);
                        }
                    }
                }
            }
            else if (oznaka.Trim().ToUpper() == "OS")
            {

                foreach (Predmet predmet in predmeti)
                {
                    if (predmet.Sistem.ToUpper() == tokens[1].Trim().ToUpper())
                    {
                        u.Add(predmet.Oznaka, predmet);
                    }
                }
            }
            else if (oznaka.Trim().ToUpper() == "PROJEKTOR")
            {

                foreach (Predmet predmet in predmeti)
                {
                    if (tokens[1].Trim().ToUpper() == "IMA")
                    {
                        if (predmet.TrebaProjektor)
                        {
                            u.Add(predmet.Oznaka, predmet);
                        }

                    }
                    if (tokens[1].Trim().ToUpper() == "NEMA")
                    {
                        if (!predmet.TrebaProjektor)
                        {
                            u.Add(predmet.Oznaka, predmet);
                        }
                    }
                }
            }
            else if (oznaka.Trim().ToUpper() == "TABLA")
            {
                foreach (Predmet predmet in predmeti)
                {
                    if (tokens[1].Trim().ToUpper() == "IMA")
                    {
                        if (predmet.TrebaTabla)
                        {
                            u.Add(predmet.Oznaka, predmet);
                        }

                    }
                    if (tokens[1].Trim().ToUpper() == "NEMA")
                    {
                        if (!predmet.TrebaTabla)
                        {
                            u.Add(predmet.Oznaka, predmet);
                        }
                    }
                }

            }
            else if (oznaka.Trim().ToUpper() == "PAMETNA TABLA")
            {
                foreach (Predmet predmet in predmeti)
                {
                    if (tokens[1].Trim().ToUpper() == "IMA")
                    {
                        if (predmet.TrebaPametnaTabla)
                        {
                            u.Add(predmet.Oznaka, predmet);
                        }

                    }
                    if (tokens[1].Trim().ToUpper() == "NEMA")
                    {
                        if (!predmet.TrebaPametnaTabla)
                        {
                            u.Add(predmet.Oznaka, predmet);
                        }
                    }
                }

            }
            return u;

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (Pretraga == "")
            {
                List<Smer> u = otvoriSmer();
                Smerovi.Clear();
                foreach (Smer smer in u)
                {
                    Smerovi.Add(smer);
                }
            }
            else
            {
                if (Pretraga != null)
                {
                    List<Smer> u = otvoriSmer();
                    string pretraga = Pretraga.ToUpper();
                    if ((pretraga.IndexOf(" AND ") != -1) && (pretraga.IndexOf(" OR ") != -1))
                    {
                        Predmeti.Clear();
                    }
                    else if (pretraga.IndexOf(" AND ") != -1)
                    {
                        List<Dictionary<string, Smer>> dicts = new List<Dictionary<string, Smer>>();
                        string[] andTokens = Regex.Split(pretraga, "AND");
                        for (int i = 0; i < andTokens.Count(); i++)
                        {
                            dicts.Add(pretragaSmera(u, andTokens[i]));
                        }


                        Smerovi.Clear();
                        foreach (Smer smer in dicts[0].Values)
                        {
                            bool da = true;
                            foreach (Dictionary<string, Smer> d in dicts)
                            {
                                if (d.ContainsKey(smer.Oznaka))
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
                                Smerovi.Add(smer);

                            }

                        }
                    }
                    else if (pretraga.IndexOf(" OR ") != -1)
                    {
                        List<Dictionary<string, Smer>> dicts = new List<Dictionary<string, Smer>>();
                        string[] andTokens = Regex.Split(pretraga, "OR");

                        for (int i = 0; i < andTokens.Count(); i++)
                        {
                            dicts.Add(pretragaSmera(u, andTokens[i]));
                        }

                        Smerovi.Clear();
                        Dictionary<string, Smer> hash = new Dictionary<string, Smer>();
                        foreach (Dictionary<string, Smer> d in dicts)
                        {
                            foreach (Smer smer in d.Values)
                            {
                                if (!hash.ContainsKey(smer.Oznaka))
                                {
                                    Smerovi.Add(smer);
                                    hash.Add(smer.Oznaka, smer);
                                }
                            }
                        }
                    }

                    else
                    {
                        Dictionary<string, Smer> pretragaUc = pretragaSmera(u, Pretraga);

                        Smerovi.Clear();
                        foreach (Smer smer in pretragaUc.Values)
                        {
                            Smerovi.Add(smer);

                        }

                    }
                }
            }

        }

        private Dictionary<string, Smer> pretragaSmera(List<Smer> smerovi, string kriterijum)
        {
            string[] tokens = kriterijum.Split(':');
            string oznaka = tokens[0];

            Dictionary<string, Smer> u = new Dictionary<string, Smer>();

            //List<Ucionica> u = new List<Ucionica>(); 
            if (oznaka.Trim().ToUpper() == "OZNAKA")
            {
                foreach (Smer smer  in smerovi)
                {
                    if (smer.Oznaka.ToUpper() == tokens[1].Trim().ToUpper())
                    {
                        u.Add(smer.Oznaka, smer);
                    }
                }

            }
            else if (oznaka.Trim().ToUpper() == "OPIS")
            {
                foreach (Smer smer in smerovi)
                {
                    if (smer.Opis.ToUpper() == tokens[1].Trim().ToUpper())
                    {
                        u.Add(smer.Oznaka, smer);
                    }
                }
            }
            else if (oznaka.Trim().ToUpper() == "NAZIV")
            {
                foreach (Smer smer in smerovi)
                {
                    if (smer.Naziv.ToUpper() == tokens[1].Trim().ToUpper())
                    {
                        u.Add(smer.Oznaka, smer);
                    }
                }
            }
            else if (oznaka.Trim().ToUpper() == "SKRACENICA")
            {
                foreach (Smer smer in smerovi)
                {
                    if (smer.Skracenica.ToUpper() == tokens[1].Trim().ToUpper())
                    {
                        u.Add(smer.Oznaka, smer);
                    }
                }
            }
           
            return u;

        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (Pretraga == "")
            {
                List<Softver> u = otvoriSoftver();
                Softveri.Clear();
                foreach (Softver softver in u)
                {
                    Softveri.Add(softver);
                }
            }
            else
            {
                if (Pretraga != null)
                {
                    List<Softver> u = otvoriSoftver();
                    string pretraga = Pretraga.ToUpper();
                    if ((pretraga.IndexOf(" AND ") != -1) && (pretraga.IndexOf(" OR ") != -1))
                    {
                        Softveri.Clear();
                    }
                    else if (pretraga.IndexOf(" AND ") != -1)
                    {
                        List<Dictionary<string, Softver>> dicts = new List<Dictionary<string, Softver>>();
                        string[] andTokens = Regex.Split(pretraga, "AND");
                        for (int i = 0; i < andTokens.Count(); i++)
                        {
                            dicts.Add(pretragaSoftvera(u, andTokens[i]));
                        }


                        Softveri.Clear();
                        foreach (Softver softver in dicts[0].Values)
                        {
                            bool da = true;
                            foreach (Dictionary<string, Softver> d in dicts)
                            {
                                if (d.ContainsKey(softver.Oznaka))
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
                                Softveri.Add(softver);

                            }

                        }
                    }
                    else if (pretraga.IndexOf(" OR ") != -1)
                    {
                        List<Dictionary<string, Softver>> dicts = new List<Dictionary<string, Softver>>();
                        string[] andTokens = Regex.Split(pretraga, "OR");

                        for (int i = 0; i < andTokens.Count(); i++)
                        {
                            dicts.Add(pretragaSoftvera(u, andTokens[i]));
                        }

                        Softveri.Clear();
                        Dictionary<string, Softver> hash = new Dictionary<string, Softver>();
                        foreach (Dictionary<string, Softver> d in dicts)
                        {
                            foreach (Softver softver in d.Values)
                            {
                                if (!hash.ContainsKey(softver.Oznaka))
                                {
                                    Softveri.Add(softver);
                                    hash.Add(softver.Oznaka, softver);
                                }
                            }
                        }
                    }

                    else
                    {
                        Dictionary<string, Softver> pretragaUc = pretragaSoftvera(u, Pretraga);

                        Softveri.Clear();
                        foreach (Softver softver in pretragaUc.Values)
                        {
                            Softveri.Add(softver);

                        }

                    }
                }
            }

        }

        private Dictionary<string, Softver> pretragaSoftvera(List<Softver> softveri, string kriterijum)
        {
            string[] tokens = kriterijum.Split(':');
            string oznaka = tokens[0];

            Dictionary<string, Softver> u = new Dictionary<string, Softver>();

            //List<Ucionica> u = new List<Ucionica>(); 
            if (oznaka.Trim().ToUpper() == "OZNAKA")
            {
                foreach (Softver softver in softveri)
                {
                    if (softver.Oznaka.ToUpper() == tokens[1].Trim().ToUpper())
                    {
                        u.Add(softver.Oznaka, softver);
                    }
                }

            }
            else if (oznaka.Trim().ToUpper() == "OPIS")
            {
                foreach (Softver softver in softveri)
                {
                    if (softver.Opis.ToUpper() == tokens[1].Trim().ToUpper())
                    {
                        u.Add(softver.Oznaka, softver);
                    }
                }

            }
            else if (oznaka.Trim().ToUpper() == "SAJT")
            {
                foreach (Softver softver in softveri)
                {
                    if (softver.Sajt.ToUpper() == tokens[1].Trim().ToUpper())
                    {
                        u.Add(softver.Oznaka, softver);
                    }
                }

            }
            else if (oznaka.Trim().ToUpper() == "PROIZVODJAC")
            {
                foreach (Softver softver in softveri)
                {
                    if (softver.Proizvodjac.ToUpper() == tokens[1].Trim().ToUpper())
                    {
                        u.Add(softver.Oznaka, softver);
                    }
                }

            }
           
            else if (oznaka.Trim().ToUpper() == "GODINA")
            {
                int broj = -1;
                bool res = Int32.TryParse(tokens[1].Trim(), out broj);
                if (res)
                {
                    
                    foreach (Softver softver in softveri)
                    {
                        if (softver.GodinaIzdavanja == broj)
                        {
                            u.Add(softver.Oznaka, softver);
                        }
                    }
                }
            }
            else if (oznaka.Trim().ToUpper() == "CENA")
            {
                double broj = -1;
                bool res = Double.TryParse(tokens[1].Trim(), out broj);
                if (res)
                {
                  
                    foreach (Softver softver in softveri)
                    {
                        if (softver.Cena == broj)
                        {
                            u.Add(softver.Oznaka, softver);
                        }
                    }
                }
            }
            
            else if (oznaka.Trim().ToUpper() == "OS")
            {

                foreach (Softver softver in softveri)
                {
                    if (softver.Sistem.ToUpper() == tokens[1].Trim().ToUpper())
                    {
                        u.Add(softver.Oznaka, softver);
                    }
                }
            }
           
            return u;

        }

        private void Box2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Box2.Text == "")
            {
                Box2.BorderBrush = new SolidColorBrush(Colors.Red);

            }
            else
            {
                Box2.BorderBrush = new SolidColorBrush(Colors.Silver);
                bool ima = false;
                for (int i = 0; i < Predmeti.Count; i++)
                {
                    if (_index == i)
                    {
                        continue;
                    }
                    if (Predmeti[i].Oznaka == Box2.Text)
                    {
                        ima = true;
                        break;
                    }

                }


                if (!ima)
                {
                    EPredmeti.Visibility = Visibility.Collapsed;
                    _greskaOznaka = false;

                }
                else
                {
                    EPredmeti.Text = " Oznaka predmeta mora biti jedinstvena.";
                    EPredmeti.Visibility = Visibility.Visible;
                    _greskaOznaka = true;
                }
            }

        }

        private void Skracenica1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Skracenica1.Text == "")
            {
                Skracenica1.BorderBrush = new SolidColorBrush(Colors.Red);

            }
            else
            {
                Skracenica1.BorderBrush = new SolidColorBrush(Colors.Silver);

            }
        }

        private void Naziv1_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Naziv1.Text == "")
            {
                Naziv1.BorderBrush = new SolidColorBrush(Colors.Red);

            }
            else
            {
                Naziv1.BorderBrush = new SolidColorBrush(Colors.Silver);

            }

        }

        private void Box3_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Box3.Text == "")
            {
                Box3.BorderBrush = new SolidColorBrush(Colors.Red);

            }
            else
            {
                Box3.BorderBrush = new SolidColorBrush(Colors.Silver);
                bool ima = false;
                for (int i = 0; i < Smerovi.Count; i++)
                {
                    if (_index == i)
                    {
                        continue;
                    }
                    if (Smerovi[i].Oznaka == Box3.Text)
                    {
                        ima = true;
                        break;
                    }

                }


                if (!ima)
                {
                    ESmerovi.Visibility = Visibility.Collapsed;
                    _greskaOznaka = false;

                }
                else
                {
                    ESmerovi.Text = " Oznaka smera mora biti jedinstvena.";
                    ESmerovi.Visibility = Visibility.Visible;
                    _greskaOznaka = true;
                }
            }

        }

        private void Skracenica2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Skracenica2.Text == "")
            {
                Skracenica2.BorderBrush = new SolidColorBrush(Colors.Red);

            }
            else
            {
                Skracenica2.BorderBrush = new SolidColorBrush(Colors.Silver);
            }

        }

        private void Naziv2_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Naziv2.Text == "")
            {
                Naziv2.BorderBrush = new SolidColorBrush(Colors.Red);

            }
            else
            {
                Naziv2.BorderBrush = new SolidColorBrush(Colors.Silver);

            }
        }

        private void Box4_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Box4.Text == "")
            {
                Box4.BorderBrush = new SolidColorBrush(Colors.Red);

            }
            else
            {
                Box4.BorderBrush = new SolidColorBrush(Colors.Silver);
                bool ima = false;
                for (int i = 0; i < Softveri.Count; i++)
                {
                    if (_index == i)
                    {
                        continue;
                    }
                    if (Softveri[i].Oznaka == Box4.Text)
                    {
                        ima = true;
                        break;
                    }

                }


                if (!ima)
                {
                    ESoftveri.Visibility = Visibility.Collapsed;
                    _greskaOznaka = false;

                }
                else
                {
                    ESoftveri.Text = " Oznaka softvera mora biti jedinstvena.";
                    ESoftveri.Visibility = Visibility.Visible;
                    _greskaOznaka = true;
                }
            }

        }

        private void Naziv3_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Naziv3.Text == "")
            {
                Naziv3.BorderBrush = new SolidColorBrush(Colors.Red);

            }
            else
            {
                Naziv3.BorderBrush = new SolidColorBrush(Colors.Silver);

            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("" + DataPic.SelectedDate);
            Smerovi.Clear();
            List<Smer> u = otvoriSmer();

            foreach (Smer smer in u)
            {
                if (smer.DatumUvodjenja == DataPic.SelectedDate)
                {
                    Smerovi.Add(smer);
                }

            }
        }

        private void ComboBox_SelectionChanged_4(object sender, SelectionChangedEventArgs e)
        {
            if (IzabraniOSSoftver == "")
            {
                List<Softver> u = otvoriSoftver();
                Softveri.Clear();
                foreach (Softver softver in u)
                {
                    Softveri.Add(softver);
                }
            }
            else
            {
                Softveri.Clear();
                List<Softver> u = otvoriSoftver();

                foreach (Softver softver in u)
                {
                    if (softver.Sistem == IzabraniOSSoftver)
                    {
                        Softveri.Add(softver);
                    }
                }
            }
        }

        private void ComboBox_SelectionChanged_5(object sender, SelectionChangedEventArgs e)
        {
            Predmeti.Clear();
            List<Predmet> u = otvoriPredmet();
            if (IzabranaTablaPredmet == "")
            {
                foreach (Predmet predmet in u)
                {

                    Predmeti.Add(predmet);


                }
            }
            else
            {
                foreach (Predmet predmet in u)
                {
                    if (IzabranaTablaPredmet == "Ima")
                    {
                        if (predmet.TrebaTabla)
                        {
                            Predmeti.Add(predmet);
                        }
                    }
                    if (IzabranaTablaPredmet == "Nema")
                    {
                        if (!predmet.TrebaTabla)
                        {
                            Predmeti.Add(predmet);

                        }
                    }

                }
            }

        }

        private void ComboBox_SelectionChanged_6(object sender, SelectionChangedEventArgs e)
        {
            Predmeti.Clear();
            List<Predmet> u = otvoriPredmet();
            if (IzabranaPametnaTablaPredmet == "")
            {
                foreach (Predmet predmet in u)
                {

                    Predmeti.Add(predmet);


                }
            }
            else
            {
                foreach (Predmet predmet in u)
                {
                    if (IzabranaPametnaTablaPredmet == "Ima")
                    {
                        if (predmet.TrebaPametnaTabla)
                        {
                            Predmeti.Add(predmet);
                        }
                    }
                    if (IzabranaPametnaTablaPredmet == "Nema")
                    {
                        if (!predmet.TrebaPametnaTabla)
                        {
                            Predmeti.Add(predmet);

                        }
                    }

                }
            }

        }

        private void ComboBox_SelectionChanged_7(object sender, SelectionChangedEventArgs e)
        {
            Predmeti.Clear();
            List<Predmet> u = otvoriPredmet();
            if (IzabraniProjektorPredmet == "")
            {
                foreach (Predmet predmet in u)
                {

                    Predmeti.Add(predmet);


                }
            }
            else
            {
                foreach (Predmet predmet in u)
                {
                    if (IzabraniProjektorPredmet == "Ima")
                    {
                        if (predmet.TrebaProjektor)
                        {
                            Predmeti.Add(predmet);
                        }
                    }
                    if (IzabraniProjektorPredmet == "Nema")
                    {
                        if (!predmet.TrebaProjektor)
                        {
                            Predmeti.Add(predmet);

                        }
                    }

                }
            }

        }

        private void ComboBox_SelectionChanged_8(object sender, SelectionChangedEventArgs e)
        {
            Predmeti.Clear();
            List<Predmet> u = otvoriPredmet();
            if (IzabraniOSPredmet == "")
            {
                foreach (Predmet predmet in u)
                {

                    Predmeti.Add(predmet);


                }
            }
            else
            {
                foreach (Predmet predmet in u)
                {
                    if (predmet.Sistem.ToUpper() == IzabraniOSPredmet.ToUpper())
                    {
                        Predmeti.Add(predmet);
                    }

                }
            }
        }
    }

}
