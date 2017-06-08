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
using System.Collections.ObjectModel;
using Raspored.Model;
using System.Data;
using System.IO;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Raspored.DDrop
{
    /// <summary>
    /// Interaction logic for PravljenjeRasporeda.xaml
    /// </summary>
    public partial class PravljenjeRasporeda : Window, INotifyPropertyChanged
    {
        private Point startPoint;


        public PravljenjeRasporeda()
        {
            InitializeComponent();
            this.DataContext = this;
            this.rasp = new Model.Raspored();
        }

        public PravljenjeRasporeda(Model.Raspored rasp)
        {
            InitializeComponent();
            this.DataContext = this;
            this.rasp = rasp;

            List<Softver> sof = otvoriSoftver();
            Softveri = new ObservableCollection<Softver>(sof);
            List<Smer> s = otvoriSmer();
            Smerovi = new ObservableCollection<Smer>(s);
            List<Predmet> p = otvoriPredmet();
            foreach (Smer smer in s)
            {
                foreach (Predmet pred in p)
                {
                    if (pred.SmerPredmeta.Oznaka == smer.Oznaka)
                        smer.Predmeti.Add(pred);
                }

            }


            Smerovi = new ObservableCollection<Smer>(s);

            List<Ucionica> u = otvoriUcionicu();
            Ucionice = new ObservableCollection<Ucionica>(u);


            Korak1Enable = "False";
            Korak2Enable = "False";


            Predmeti = new ObservableCollection<Predmet>();

            /* List<Predmet> l = new List<Predmet>();
             l.Add(new Predmet { Naziv = "1", Oznaka = "1", VelicinaGrupe = 3, DuzinaTermina = 45, BrojTermina = 2 });
             l.Add(new Predmet { Naziv = "2", Oznaka = "2", VelicinaGrupe = 3, DuzinaTermina = 90, BrojTermina = 2 });
             l.Add(new Predmet { Naziv = "3", Oznaka = "3", VelicinaGrupe = 3, DuzinaTermina = 90, BrojTermina = 4 });
             l.Add(new Predmet { Naziv = "4", Oznaka = "4", VelicinaGrupe = 3, DuzinaTermina = 45, BrojTermina = 2 });
             l.Add(new Predmet { Naziv = "5", Oznaka = "5", VelicinaGrupe = 3, DuzinaTermina = 45, BrojTermina = 2 });
             Studenti = new ObservableCollection<Predmet>(l);*/
            Studenti = new ObservableCollection<Predmet>();

            Termini = new List<List<ObservableCollection<Predmet>>>();
            for (int i = 0; i < 61; i++)
            {
                List<ObservableCollection<Predmet>> temp = new List<ObservableCollection<Predmet>>();
                for (int j = 0; j < 7; j++)
                    temp.Add(new ObservableCollection<Predmet>());
                Termini.Add(temp);
            }
        }

        private Model.Raspored rasp;
        public ObservableCollection<Smer> Smerovi
        {
            get;
            set;
        }

        public ObservableCollection<Predmet> Predmeti
        {
            get;
            set;
        }


        public ObservableCollection<Ucionica> Ucionice
        {
            get;
            set;
        }

        public Ucionica Selected
        {
            get;
            set;
        }
        public ObservableCollection<Predmet> Studenti { get; private set; }
        public ObservableCollection<Softver> Softveri { get; private set; }
        public List<List<ObservableCollection<Predmet>>> Termini { get; private set; }
        private bool fromList = true;
        private int from_r = 0;
        private int from_c = 0;
        private ListView lv;

        private void Korak1_Click(object sender, RoutedEventArgs e)
        {
            Korak1Enable = "False";
            Raspored1.Visibility = Visibility.Collapsed;
            Raspored2.Visibility = Visibility.Visible;
            // Raspored3.Visibility = Visibility.Collapsed;

        }

        private void Korak1_Nazad_Click(object sender, RoutedEventArgs e)
        {
            Korak2Enable = "True";
            Studenti.Remove(SelectedPredmet);
            Raspored2.Visibility = Visibility.Visible;
            Raspored3.Visibility = Visibility.Collapsed;
            //TO_DO : ments el a Termini-t
            foreach (UcionicaRaspored ur in rasp.Rasporedi)
                if (ur.Ucionica.Oznaka == SelectedUcionica.Oznaka)
                {
                    //TO_DO: studentiTo -ba atkonvertalni 
                    for (int i = 1; i < 61; i++)
                    {
                        for (int j = 1; j < 7; j++)
                        {
                            if (Termini[i][j].Count != 0)
                            {
                                ur.Rasporedi[i][j] = Termini[i][j][0];
                                Termini[i][j].RemoveAt(0);
                            }
                        }
                    }
                }
            // sacuvajRaspored();

        }



        private void Korak2_Click(object sender, RoutedEventArgs e)
        {
            Korak2Enable = "False";
            Studenti.Add(SelectedPredmet);
            Raspored2.Visibility = Visibility.Collapsed;
            Raspored3.Visibility = Visibility.Visible;
            int x = 0;
            foreach (UcionicaRaspored ur in rasp.Rasporedi)
                if (ur.Ucionica.Oznaka == SelectedUcionica.Oznaka)
                {
                    for (int i = 0; i < 61; i++)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            if (ur.Rasporedi[i][j].Oznaka != "")
                                Termini[i][j].Add(ur.Rasporedi[i][j]);
                        }
                    }
                    x = 1;
                }
            if (x == 0)
                rasp.Rasporedi.Add(new UcionicaRaspored(SelectedUcionica));

        }

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            //TO_DO : ments el a Termini-t

            if (SelectedUcionica != null)
                foreach (UcionicaRaspored ur in rasp.Rasporedi)
                    if (ur.Ucionica.Oznaka == SelectedUcionica.Oznaka)
                    {
                        //TO_DO: studentiTo -ba atkonvertalni 
                        for (int i = 1; i < 61; i++)
                        {
                            for (int j = 1; j < 7; j++)
                            {
                                if (Termini[i][j].Count != 0)
                                    ur.Rasporedi[i][j] = Termini[i][j][0];
                            }
                        }
                    }

            // sacuvajRaspored();
        }



        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
            lv = sender as ListView;
            String v = lv.Name.ToString();
            String r = v.Substring(4, 1);
            from_r = Convert.ToInt32(r);
            String c = v.Substring(2, 2);
            from_c = Convert.ToInt32(c);
        }

        private void ListView_PreviewMouseLeftButtonDown2(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);

            fromList = true;
        }


        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;


                ListViewItem listViewItem =
                    FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem == null)
                    return;

                // Find the data behind the ListViewItem
                Predmet student = (Predmet)listView.ItemContainerGenerator.
                    ItemFromContainer(listViewItem);


                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("myFormat", student);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void ListView_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            var parent = element;
            while (parent != null)
            {
                var correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }
            return null;
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            ListView listView = sender as ListView;

            String v = listView.Name.ToString();
            String r = v.Substring(4, 1);
            int r1 = Convert.ToInt32(r);
            String c = v.Substring(2, 2);
            int c1 = Convert.ToInt32(c);
            //MessageBox.Show(r1.ToString());

            if (e.Data.GetDataPresent("myFormat"))
            {
                Predmet student = e.Data.GetData("myFormat") as Predmet;
                int broj_termina = student.BrojTermina;
                if (Termini[c1][r1].Count == 0)
                {
                    if (fromList)
                    {
                        int cas = student.DuzinaTermina / 45;
                        //MessageBox.Show("Cas: " + cas);
                        bool nasla = false;
                        foreach (Predmet p in rasp.OstaliTermini)
                            if (p.Oznaka == student.Oznaka)
                            {

                                p.BrojTermina += cas;
                                if (p.BrojTermina >= broj_termina)
                                {
                                    Studenti.Remove(student);
                                }
                                nasla = true;
                            }
                        if (!nasla)
                        {
                            //MessageBox.Show("Nije Nasla ");
                            Predmet termini = student;
                            termini.BrojTermina = cas;
                            rasp.OstaliTermini.Add(termini);
                            if (termini.BrojTermina >= broj_termina)
                            {
                                Studenti.Remove(student);
                            }

                        }

                    }
                    else
                    {
                        int i = 0;
                        int x = 0;
                        while (true)
                        {

                            foreach (Predmet p in Termini[from_c - i - 1][from_r])
                            {
                                if (p.Naziv != student.Naziv)
                                    x = 1;
                            }
                            // MessageBox.Show("" + x);
                            if (x == 1 || Termini[from_c - i - 1][from_r].Count == 0)
                                break;
                            i++;
                        }

                        for (int j = 0; j < student.DuzinaTermina / 15; j++)
                        {
                            Termini[from_c - i + j][from_r].Remove(student);
                            lv.Background = Brushes.White;

                        }
                    }
                    // Termini[c1][r1].Add(student);
                    listView.Background = Brushes.LightGreen;
                    for (int i = 0; i < student.DuzinaTermina / 15; i++)
                    {
                        Termini[c1 + i][r1].Add(student);
                    }



                }
                fromList = false;
            }
        }
        public void sacuvajRaspored()
        {
            StreamWriter f = new StreamWriter(rasp.File);
            f.WriteLine(rasp.Naziv);
            foreach (Predmet p in rasp.OstaliTermini)
            {
                f.Write(p.Oznaka + "," + p.BrojTermina + "|");
            }
            f.WriteLine();
            foreach (UcionicaRaspored r in rasp.Rasporedi)
            {
                f.Write(r.Ucionica.Oznaka + ":");
                for (int i = 0; i < 61; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        f.Write(r.Rasporedi[i][j].Oznaka + ",");
                    }
                    f.Write("|");
                }
                f.WriteLine();
            }

            f.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
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

        private string _korak1Enable;
        public string Korak1Enable
        {
            get
            {
                return _korak1Enable;
            }
            set
            {
                if (_korak1Enable != value)
                {
                    _korak1Enable = value;
                    OnPropertyChanged("Korak1Enable");
                }
            }
        }

        private string _korak2Enable;
        public string Korak2Enable
        {
            get
            {
                return _korak2Enable;
            }
            set
            {
                if (_korak2Enable != value)
                {
                    _korak2Enable = value;
                    OnPropertyChanged("Korak2Enable");
                }
            }
        }

        private void trvPredmeti_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

            if (trvPredmeti.SelectedItem.GetType().Equals(typeof(Model.Predmet)))
            {
                SelectedPredmet = (Model.Predmet)trvPredmeti.SelectedItem;
                Korak1Enable = "True";
            }
            if (trvPredmeti.SelectedItem.GetType().Equals(typeof(Model.Smer)))
            {
                Korak1Enable = "False";

            }
        }


        private void lsUcionice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Korak2Enable = "True";
            SelectedUcionica = (Ucionica)lsUcionice.SelectedItem;
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
    }
}
