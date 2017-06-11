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
        private CitanjeIPisanje citanje_pisanje;

        public PravljenjeRasporeda()
        {
            InitializeComponent();
            this.DataContext = this;
            this.rasp = new Model.Raspored();
        }

        public PravljenjeRasporeda(Model.Raspored rasp, CitanjeIPisanje citanje_pisanje)
        {
            InitializeComponent();
            this.DataContext = this;
            this.rasp = rasp;
            this.citanje_pisanje = citanje_pisanje;

            Ucionice = new ObservableCollection<Ucionica>();
            List<Softver> sof = citanje_pisanje.otvoriSoftver();
            Softveri = new ObservableCollection<Softver>(sof);
            List<Smer> s = citanje_pisanje.otvoriSmer();
            Smerovi = new ObservableCollection<Smer>(s);
            List<Predmet> p = citanje_pisanje.otvoriPredmet();
            List<Predmet> brisanje = new List<Predmet>();
            foreach (Predmet termin in rasp.OstaliTermini)
            {
                foreach (Predmet predmet in p)
                {
                    if (termin.Oznaka == predmet.Oznaka)
                    {
                        if (termin.BrojTermina >= predmet.BrojTermina)
                            //p.Remove(predmet);
                            brisanje.Add(predmet);
                        //else
                        //  predmet.BrojTermina = predmet.BrojTermina - termin.BrojTermina;
                    }
                }
            }
            foreach (Predmet pred in brisanje)
            {
                p.Remove(pred);
            }

            foreach (Smer smer in s)
            {
                foreach (Predmet pred in p)
                {
                    if (pred.SmerPredmeta.Oznaka == smer.Oznaka)
                        smer.Predmeti.Add(pred);
                }

            }

            Smerovi = new ObservableCollection<Smer>(s);

            Korak1Enable = "False";
            Korak2Enable = "False";

            Temp = new ObservableCollection<Predmet>();
            Predmeti = new ObservableCollection<Predmet>();

            Studenti = new ObservableCollection<Predmet>();
            Predmet pauza = new Predmet { Naziv = "Pauza", Oznaka = "Pauza" };
            Studenti.Add(pauza);

            Termini = new List<List<ObservableCollection<Predmet>>>();
            for (int i = 0; i < 61; i++)
            {
                List<ObservableCollection<Predmet>> temp = new List<ObservableCollection<Predmet>>();
                for (int j = 0; j < 7; j++)
                    temp.Add(new ObservableCollection<Predmet>());
                Termini.Add(temp);
            }
        }

        public Model.Raspored rasp { get; set; }
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
        public ObservableCollection<Predmet> Delete { get; private set; }
        public ObservableCollection<Predmet> Temp { get; private set; }
        public ObservableCollection<Softver> Softveri { get; private set; }
        public List<List<ObservableCollection<Predmet>>> Termini { get; private set; }
        private bool fromList = false;
        private bool fromListTemp = false;
        private int from_r = 0;
        private int from_c = 0;
        private ListView lv;

        private void Korak1_Click(object sender, RoutedEventArgs e)
        {
            
            foreach (Ucionica u in citanje_pisanje.otvoriUcionicu())
            {
                if (SelectedPredmet != null)
                {
                    if (SelectedPredmet.TrebaPametnaTabla)
                        if (SelectedPredmet.TrebaPametnaTabla != u.ImaPametnaTabla)
                            continue;
                    if (SelectedPredmet.TrebaProjektor)
                        if (SelectedPredmet.TrebaProjektor != u.ImaProjektor)
                            continue;
                    if (SelectedPredmet.TrebaTabla)
                        if (SelectedPredmet.TrebaTabla != u.ImaTabla)
                            continue;
                    if (SelectedPredmet.VelicinaGrupe > u.BrojRadnihMesta)
                        continue;
                    if (SelectedPredmet.Sistem == "Windows")
                        if (u.Sistem != "Windows" && u.Sistem != "Oba")
                            continue;
                    if (SelectedPredmet.Sistem == "Linux")
                        if (u.Sistem != "Linux" && u.Sistem != "Oba")
                            continue;
                    if (SelectedPredmet.Sistem == "Oba")
                        if (u.Sistem != "Oba")
                            continue;
                    Ucionice.Add(u);
                }
                else
                {
                    Ucionice.Add(u);
                }
            }
            

            Korak2Enable = "False";
            Korak1Enable = "False";
            Raspored1.Visibility = Visibility.Collapsed;
            Raspored2.Visibility = Visibility.Visible;
            // Raspored3.Visibility = Visibility.Collapsed;


        }

        private void Korak1_Bez_Predmeta_Click(object sender, RoutedEventArgs e)
        {
            SelectedPredmet = null;
            foreach (Ucionica u in citanje_pisanje.otvoriUcionicu())
            {
                Ucionice.Add(u);
            }

            Korak2Enable = "False";
            Korak1Enable = "False";
            Raspored1.Visibility = Visibility.Collapsed;
            Raspored2.Visibility = Visibility.Visible;
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
            int broj_termina = 0;
            List<Predmet> termin = citanje_pisanje.otvoriPredmet();
            if (SelectedPredmet != null)
            {
                foreach (Predmet p in termin)
                    if (p.Oznaka == SelectedPredmet.Oznaka)
                    {
                        broj_termina = p.BrojTermina;
                        break;
                    }
                //Predme
                int y = 0;
                foreach (Predmet ur in rasp.OstaliTermini)
                    if (ur.Oznaka == SelectedPredmet.Oznaka)
                    {
                        y = 1;
                        if (broj_termina > ur.BrojTermina)
                        {
                            //if (!Studenti.Contains(SelectedPredmet))
                            Studenti.Add(SelectedPredmet);
                        }
                        break;
                    }
                if (y == 0)
                {
                    Studenti.Add(SelectedPredmet);
                }
            }
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
                            {
                                Termini[i][j].Add(ur.Rasporedi[i][j]);
                            }
                        }
                    }
                    x = 1;
                }
            if (x == 0)
            {
                rasp.Rasporedi.Add(new UcionicaRaspored(SelectedUcionica));
            }
            //


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
                                else
                                    if (ur.Rasporedi[i][j].Oznaka != "")
                                    ur.Rasporedi[i][j] = new Predmet();
                            }
                        }
                    }

            sacuvajRaspored();
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


        private void ListView_PreviewMouseLeftButtonDown3(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);

            fromListTemp = true;
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
                int broj_termina = 0;
                List<Predmet> termin = citanje_pisanje.otvoriPredmet();
                foreach (Predmet p in termin)
                    if (p.Oznaka == student.Oznaka)
                    {
                        broj_termina = p.BrojTermina;
                        break;
                    }


                if (Termini[c1][r1].Count == 0)
                {
                    if (fromList)
                    {
                        int cas = student.DuzinaTermina;
                        //MessageBox.Show("Cas: " + cas);
                        bool nasla = false;
                        bool empty = true;
                        if (student.Oznaka != "Pauza")
                        {
                            for (int i = 0; i < cas * 3; i++)
                                if (Termini[c1 + i][r1].Count != 0)
                                {
                                    empty = false;
                                    break;
                                }
                            if (empty)
                            {

                                foreach (Predmet p in rasp.OstaliTermini)
                                    if (p.Oznaka == student.Oznaka)
                                    {

                                        p.BrojTermina += cas;
                                        if (p.BrojTermina >= broj_termina)
                                        {
                                            Studenti.Remove(student);
                                        }
                                        nasla = true;
                                        break;
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

                                for (int i = 0; i < student.DuzinaTermina * 3; i++)
                                {
                                    Termini[c1 + i][r1].Add(student);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Nema dovoljno mesta za termin.");
                            }
                        }
                        else
                        {
                            Termini[c1][r1].Add(student);
                        }



                    }
                    else if (fromListTemp)
                    {
                        int cas = student.DuzinaTermina;
                        //MessageBox.Show("Cas: " + cas);
                        bool nasla = false;
                        bool empty = true;
                        if (student.Oznaka != "Pauza")
                        {
                            for (int i = 0; i < cas * 3; i++)
                                if (Termini[c1 + i][r1].Count != 0)
                                {
                                    empty = false;
                                    break;
                                }
                            if (empty)
                            {
                                foreach (Predmet p in rasp.OstaliTermini)
                                    if (p.Oznaka == student.Oznaka)
                                    {

                                        p.BrojTermina += cas;
                                        if (p.BrojTermina >= broj_termina)
                                        {
                                            Temp.Remove(student);
                                        }
                                        nasla = true;
                                        break;
                                    }
                                if (!nasla)
                                {
                                    //MessageBox.Show("Nije Nasla ");
                                    Predmet termini = student;
                                    termini.BrojTermina = cas;
                                    rasp.OstaliTermini.Add(termini);
                                    if (termini.BrojTermina >= broj_termina)
                                    {
                                        Temp.Remove(student);
                                    }

                                }
                                if (student.Oznaka != "Pauza")
                                {
                                    for (int i = 0; i < student.DuzinaTermina * 3; i++)
                                    {
                                        Termini[c1 + i][r1].Add(student);
                                    }
                                }

                            }
                            else
                            {
                                MessageBox.Show("Nema dovoljno mesta za termin.");
                            }
                        }
                        else
                        {
                            Temp.Remove(student);
                            Termini[c1][r1].Add(student);
                        }


                    }

                    else
                    {
                        {
                            int i = 0;
                            int x = 0;
                            if (student.Oznaka != "Pauza")
                            {
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

                                bool empty = true;
                                for (int k = 0; k < student.DuzinaTermina * 3; k++)
                                    if (Termini[c1 + k][r1].Count != 0)
                                    {
                                        if (Termini[c1 + k][r1][0].Oznaka != student.Oznaka)
                                        {
                                            empty = false;
                                            break;
                                        }
                                    }
                                if (empty)
                                {
                                    for (int j = 0; j < student.DuzinaTermina * 3; j++)
                                    {
                                        Termini[from_c - i + j][from_r].Remove(student);
                                        lv.Background = Brushes.White;
                                    }
                                    listView.Background = Brushes.LightGreen;
                                    for (int k = 0; k < student.DuzinaTermina * 3; k++)
                                    {
                                        Termini[c1 + k][r1].Add(student);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Nema dovoljno mesto za termin");
                                }
                            }
                            else
                            {
                                Termini[from_c][from_r].Remove(student);
                                Termini[c1][r1].Add(student);
                            }
                        }
                        // Termini[c1][r1].Add(student);

                    }


                }
                fromList = false;
                fromListTemp = false;
            }
        }

        private void ListView_Delete(object sender, DragEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Da li si siguran/sigurna?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {

                ListView listView = sender as ListView;

                //  CONFIRM

                if (e.Data.GetDataPresent("myFormat"))
                {
                    Predmet student = e.Data.GetData("myFormat") as Predmet;


                    if (fromList || fromListTemp)
                    {


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

                        for (int j = 0; j < student.DuzinaTermina * 3; j++)
                        {
                            Termini[from_c - i + j][from_r].Remove(student);
                            lv.Background = Brushes.White;

                        }
                        bool add = true;
                        if (SelectedPredmet.Oznaka == student.Oznaka)
                        {
                            foreach (Predmet p in Studenti)
                                if (p.Oznaka == student.Oznaka)
                                    add = false;
                            if (add)
                                Studenti.Add(student);
                        }
                        // MessageBox.Show("" + citanje_pisanje.nadjiPredmet(student.Oznaka).DuzinaTermina);
                        Predmet brisanje = null;
                        foreach (Predmet p in rasp.OstaliTermini)
                        {
                            if (p.Oznaka == student.Oznaka)
                            {
                                p.BrojTermina = p.BrojTermina - student.DuzinaTermina;
                                if (p.BrojTermina == 0)
                                    brisanje = p;
                            }
                        }
                        rasp.OstaliTermini.Remove(brisanje);
                    }

                    fromList = false;
                }
            }
        }

        private void ListView_Temp(object sender, DragEventArgs e)
        {
            ListView listView = sender as ListView;

            //  CONFIRM

            if (e.Data.GetDataPresent("myFormat"))
            {
                Predmet student = e.Data.GetData("myFormat") as Predmet;


                if (fromList || fromListTemp)
                {


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

                    for (int j = 0; j < student.DuzinaTermina * 3; j++)
                    {
                        Termini[from_c - i + j][from_r].Remove(student);
                        lv.Background = Brushes.White;

                    }
                    Predmet brisanje = null;
                    foreach (Predmet p in rasp.OstaliTermini)
                    {
                        if (p.Oznaka == student.Oznaka)
                        {
                            p.BrojTermina = p.BrojTermina - student.DuzinaTermina;
                            if (p.BrojTermina == 0)
                                brisanje = p;
                        }
                    }
                    rasp.OstaliTermini.Remove(brisanje);
                    Temp.Add(student);
                    //kivoni a 
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

        private void Korak_Nazad_Click(object sender, RoutedEventArgs e)
        {
            //!!!!!!!!!!!!
            Korak2Enable = "False";
            Korak1Enable = "True";
            
            SelectedUcionica = null;
            Raspored1.Visibility = Visibility.Visible;
            Raspored2.Visibility = Visibility.Collapsed;

            Ucionice.ToList().All(i => Ucionice.Remove(i));
        }

        private void Show_Predmet_Click(object sender, MouseButtonEventArgs e)
        {
            //  MessageBox.Show("blbabla");
            ListView listView = sender as ListView;
            String v = listView.Name.ToString();
            String r = v.Substring(4, 1);
            int r1 = Convert.ToInt32(r);
            String c = v.Substring(2, 2);
            int c1 = Convert.ToInt32(c);

            if (Termini[c1][r1].Count!=0) { 
                foreach (Predmet p in citanje_pisanje.otvoriPredmet())
                    if (Termini[c1][r1][0].Oznaka == p.Oznaka)
                    {
                        PrikaziPredmet pp = new PrikaziPredmet(p);
                        pp.Show();
                    }
            }



        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[1]);
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                str = "PravljenjeRasporeda";
                HelpProvider.ShowHelp(str, this);
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.Focus();
            Keyboard.Focus(this);
            FocusManager.SetFocusedElement(this, this);

        }
    }
}
