using Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Raspored.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;

namespace Raspored
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            FileStream f = new FileStream("../../Save/recent.txt", FileMode.OpenOrCreate);
            f.Close();

            citanje_pisanje = new CitanjeIPisanje();

            RecentFileList.MenuClick += (s, e) => FileOpenCore(e.Filepath);
            //w = new Tabele.Tabele();
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[\r\n]{3,}", options);
            string recentText = File.ReadAllText("../../Save/recent.txt");

            string[] tekst = recentText.Split('\n');
            string recentFile = tekst[0];
            string[] dani = { "Ponedeljak", "Utorak", "Sreda", "Cetvtak", "Petak", "Subota" };
            Dani = new ObservableCollection<string>(dani);

            try
            {
                List<Ucionica> u = citanje_pisanje.otvoriUcionicu();
                Ucionice = new ObservableCollection<Ucionica>(u);
                //this.dgrMainUcionica.ItemsSource = u;
                //this.dgrMainUcionica.SelectedItem = SelectedUcionica;
                this.lsUcionice.ItemsSource = Ucionice;
                string fileText = File.ReadAllText(recentFile);
                Prozor1.Visibility = Visibility.Visible;
                Prozor2.Visibility = Visibility.Hidden;
                Raspored_Button.IsEnabled = true;
                raspored = citanje_pisanje.otvoriRaspored(recentFile);
                raspored.File = recentFile;
                Termini = new List<List<ObservableCollection<Predmet>>>();
                TerminiDan= new List<List<ObservableCollection<Predmet>>>();
                //Termini = new List<List<Predmet>>();
                for (int i = 0; i < 61; i++)
                {
                    List<ObservableCollection<Predmet>> temp = new List<ObservableCollection<Predmet>>();
                    List<ObservableCollection<Predmet>> temp1 = new List<ObservableCollection<Predmet>>();
                    for (int j = 0; j < 7; j++)
                        temp.Add(new ObservableCollection<Predmet>());
                    Termini.Add(temp);
                    for (int k = 0; k < 11; k++)
                        temp1.Add(new ObservableCollection<Predmet>());
 
                    TerminiDan.Add(temp1);
                }
                
                uc = new List<string>();
                for (int i = 0; i < u.Count; i++)
                    uc.Add(u[i].Oznaka);
                uc1 = uc[0];

                if (u.Count < 10)
                    c10.Width = new GridLength(0);
                if (u.Count < 9)
                    c9.Width = new GridLength(0);
                if (u.Count < 8)
                    c8.Width = new GridLength(0);
                if (u.Count < 7)
                    c7.Width = new GridLength(0);
                if (u.Count < 6)
                    c6.Width = new GridLength(0);
                if (u.Count < 5)
                    c5.Width = new GridLength(0);
                if (u.Count < 4)
                    c4.Width = new GridLength(0);
                if (u.Count < 3)
                    c3.Width = new GridLength(0);
                if (u.Count < 2)
                    c2.Width = new GridLength(0);
                if (u.Count < 1)
                    c1.Width = new GridLength(0);
                    

            }
            catch (Exception e)
            {
                Prozor2.Visibility = Visibility.Visible;
                Prozor1.Visibility = Visibility.Hidden;
                Raspored_Button.IsEnabled = false;
                //MessageBox.Show("error");
            }
        }
        
        //public List<List<Predmet>> Termini { get; private set; }
        public List<List<ObservableCollection<Predmet>>> Termini { get; private set; }
        public List<List<ObservableCollection<Predmet>>> TerminiDan { get; private set; }
        private Model.Raspored raspored;
        // Tabele.Tabele w;
        CitanjeIPisanje citanje_pisanje;

        public ObservableCollection<Ucionica> Ucionice
        {
            get;
            set;
        }

        public ObservableCollection<string> Dani
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


        private List<string> _uc;
        public List<string> uc
        {
            get
            {
                return _uc;
            }
            set
            {
                if (_uc != value)
                {
                    _uc = value;
                    OnPropertyChanged("_uc");
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

        private string _selectedDan;
        public string SelectedDan
        {
            get
            {
                return _selectedDan;
            }
            set
            {
                if (_selectedDan != value)
                {
                    _selectedDan = value;
                    OnPropertyChanged("SelectedDan");
                }
            }
        }

        private void Ucionice_Click(object sender, RoutedEventArgs e)
        {
            Tabele.Tabele w = new Tabele.Tabele(raspored);
            w.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var r = new DDrop.PravljenjeRasporeda(raspored, citanje_pisanje);
            r.ShowDialog();
            raspored = r.rasp;
            r.sacuvajRaspored();
            for (int i = 1; i < 61; i++)
            {
                for (int j = 1; j < 7; j++)
                {
                    if (Termini[i][j].Count != 0)
                    {
                        Termini[i][j].RemoveAt(0);
                    }
                }
            }


            foreach (UcionicaRaspored ur in raspored.Rasporedi)
                if (ur.Ucionica != null && SelectedUcionica!=null)
                {
                    if (ur.Ucionica.Oznaka == SelectedUcionica.Oznaka)
                    {
                        for (int i = 0; i < 61; i++)
                        {
                            for (int j = 0; j < 7; j++)
                            {
                                if (ur.Rasporedi[i][j].Oznaka != "")
                                {
                                    Termini[i][j].Add(ur.Rasporedi[i][j]);
                                    //Termini[i][j]=ur.Rasporedi[i][j];
                                }
                            }
                        }
                    }
                }



        }

        bool FileOpenCore(string filepath)
        {
            // MessageBox.Show(filepath);
            if (!File.Exists(filepath))
            {
                MessageBox.Show("File is removed or moved.");
                RecentFileList.RemoveFile(filepath);
            }
            else
            {
                RecentFileList.InsertFile(filepath);
                saveRecent(filepath);
                Prozor1.Visibility = Visibility.Visible;
                Prozor2.Visibility = Visibility.Hidden;
                Raspored_Button.IsEnabled = true;
                //TO_DO: raspored -> citanjeIz fajla
                raspored = citanje_pisanje.otvoriRaspored(filepath);
            }
            return true;
        }

        private void HandleWindowActivated(object sender, EventArgs e)
        {
            this.Focus();
            Keyboard.Focus(this);
            FocusManager.SetFocusedElement(this, this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Raspored dokument (.rsp)|*.rsp";

            if (saveFileDialog.ShowDialog() == true)
            {
                String filename = saveFileDialog.FileName;
                FileStream f = new FileStream(filename, FileMode.Create);
                f.Close();
                saveRecent(filename);
                RecentFileList.InsertFile(filename);
                Prozor1.Visibility = Visibility.Visible;
                Prozor2.Visibility = Visibility.Hidden;
                Raspored_Button.IsEnabled = true;
                raspored = new Model.Raspored();
                raspored.File = filename;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".rsp";
            openFileDialog.Filter = "Raspored dokument (.rsp)|*.rsp";

            if (openFileDialog.ShowDialog() == true)
            {
                String filename = openFileDialog.FileName;
                RecentFileList.InsertFile(filename);
                saveRecent(filename);
                Prozor1.Visibility = Visibility.Visible;
                Prozor2.Visibility = Visibility.Hidden;
                Raspored_Button.IsEnabled = true;
                //TO_DO: raspored -> citanjeIz fajla
                raspored = citanje_pisanje.otvoriRaspored(filename);
                //raspored = new Model.Raspored();
            }
        }

        void saveRecent(String filename)
        {
            String file_tekst = filename + "\n";
            FileStream f = new FileStream("../../Save/recent.txt", FileMode.OpenOrCreate);
            f.Close();

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[\r\n]{3,}", options);
            string recentText = File.ReadAllText("../../Save/recent.txt");

            foreach (string tekst in recentText.Split('\n'))
            {
                if (tekst != filename)
                    file_tekst += tekst + "\n";
            }

            StreamWriter f_save = new StreamWriter("../../Save/recent.txt");
            f_save.Write(file_tekst);
            f_save.Close();
        }

        private void RecentFileList_MenuClick(object sender, RecentFileList.MenuClickEventArgs e)
        {
            String listView = sender as String;
            MessageBox.Show(listView);
        }


        private void Novi_Raspored_Click(object sender, RoutedEventArgs e)
        {
            var r = new Raspored.DDrop.PravljenjeRasporeda(raspored, citanje_pisanje);
            r.ShowDialog();
            raspored = r.rasp;
            r.sacuvajRaspored();
            for (int i = 1; i < 61; i++)
            {
                for (int j = 1; j < 7; j++)
                {
                    if (Termini[i][j].Count != 0)
                    {
                        Termini[i][j].RemoveAt(0);
                    }
                }
            }

         
            foreach (UcionicaRaspored ur in raspored.Rasporedi)
                if (ur.Ucionica.Oznaka == SelectedUcionica.Oznaka)
                {
                    for (int i = 0; i < 61; i++)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            if (ur.Rasporedi[i][j].Oznaka != "")
                            {
                                Termini[i][j].Add(ur.Rasporedi[i][j]);
                                //Termini[i][j]=ur.Rasporedi[i][j];
                            }
                        }
                    }
                }
            
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            IInputElement focusedControl = FocusManager.GetFocusedElement(Application.Current.Windows[0]);
            if (focusedControl is DependencyObject)
            {
                string str = HelpProvider.GetHelpKey((DependencyObject)focusedControl);
                str = "Forma";
                HelpProvider.ShowHelp(str, this);
            }
        }

        public void doThings(string param)
        {
            Title = param;
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Tabele.Tabele t = new Tabele.Tabele("demo");
            t.ShowDialog();

        }

        private void Ucionice_Radio(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("ucionice");
            scoll.Visibility = Visibility.Visible;
            lsUcionice.Visibility = Visibility.Visible;
            header.Visibility = Visibility.Visible;
            scoll_2.Visibility = Visibility.Collapsed;
            lsDani.Visibility = Visibility.Collapsed;
            header_2.Visibility = Visibility.Collapsed;

        }
        public string uc1;
        private void Dani_Radio(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Dani");
            scoll.Visibility = Visibility.Collapsed;
            lsUcionice.Visibility = Visibility.Collapsed;
            header.Visibility = Visibility.Collapsed;
            scoll_2.Visibility = Visibility.Visible;
            lsDani.Visibility = Visibility.Visible;
            header_2.Visibility = Visibility.Visible;
            

        }

        private void lsUcionice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 1; i < 61; i++)
            {
                for (int j = 1; j < 7; j++)
                {
                    if (Termini != null)
                    {
                        if (Termini[i][j].Count != 0)
                        {
                            Termini[i][j].RemoveAt(0);
                        }
                    }
                }
            }

            SelectedUcionica = (Ucionica)lsUcionice.SelectedItem;
            Oznaka_ucionica.Text = SelectedUcionica.Oznaka;
            //MessageBox.Show(SelectedUcionica.Oznaka);
            foreach (UcionicaRaspored ur in raspored.Rasporedi)
                if (ur.Ucionica != null)
                {
                    if (ur.Ucionica.Oznaka == SelectedUcionica.Oznaka)
                    {
                        for (int i = 0; i < 61; i++)
                        {
                            for (int j = 0; j < 7; j++)
                            {
                                if (ur.Rasporedi[i][j].Oznaka != "")
                                {
                                    Termini[i][j].Add(ur.Rasporedi[i][j]);
                                    //Termini[i][j]=ur.Rasporedi[i][j];
                                }
                            }
                        }
                    }
                }


        }

        private void lsDani_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedDan = (string)lsDani.SelectedItem;
            int row = 1;
            switch (SelectedDan)
            {
                case "Ponedeljak":
                    row = 1;
                    break;
                case "Utorak":
                    row = 2;
                    break;
                case "Sreda":
                    row = 3;
                    break;
                
                case "Petak":
                    row = 5;
                    break;
                case "Subota":
                    row = 6;
                    break;
                default:
                    row = 4;
                    break;
            }

            List<Ucionica> u = citanje_pisanje.otvoriUcionicu();
            if (raspored.Rasporedi.Count != 0)
            {
                for (int i = 1; i <= u.Count; i++)
                {
                    foreach (UcionicaRaspored ucR in raspored.Rasporedi)
                        if (ucR.Ucionica != null)
                        {
                            if (ucR.Ucionica.Oznaka == u[i - 1].Oznaka)
                                for (int j = 1; j < 61; j++)
                                {
                                    if (TerminiDan[j][i].Count != 0)
                                        TerminiDan[j][i].RemoveAt(0);
                                }
                        }

                }
                for (int i = 1; i <= u.Count; i++)
                {
                    foreach (UcionicaRaspored ucR in raspored.Rasporedi)
                        if (ucR.Ucionica != null)
                        {
                            if (ucR.Ucionica.Oznaka == u[i - 1].Oznaka)
                                for (int j = 1; j < 61; j++)
                                {
                                    if (ucR.Rasporedi[j][row].Oznaka != "")
                                        TerminiDan[j][i].Add(ucR.Rasporedi[j][row]);
                                }
                        }

                }
            }


        }
    }
}
