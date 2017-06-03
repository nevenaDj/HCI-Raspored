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

namespace Raspored
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model.Raspored raspored;

        public MainWindow()
        {
            InitializeComponent();
            FileStream f = new FileStream("../../Save/recent.txt", FileMode.OpenOrCreate);
            f.Close();
            w = new Tabele.Tabele(); 
            RecentFileList.MenuClick += (s, e) => FileOpenCore(e.Filepath);

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[\r\n]{3,}", options);
            string recentText = File.ReadAllText("../../Save/recent.txt");

            string[] tekst = recentText.Split('\n');
            string recentFile = tekst[0];
            try
            {
                
                string fileText = File.ReadAllText(recentFile);
                //MessageBox.Show(recentFile);
                Prozor1.Visibility = Visibility.Visible;
                Prozor2.Visibility = Visibility.Hidden;
                Raspored_Button.IsEnabled = true;
                //TO_DO: ocitanje rasporeda iz fajla
                //raspored = new Model.Raspored();
                raspored = otvoriRaspored(recentFile);
                raspored.File = recentFile;

            } catch (Exception e)
            {
                Prozor2.Visibility = Visibility.Visible;
                Prozor1.Visibility = Visibility.Hidden;
                Raspored_Button.IsEnabled = false;
                //MessageBox.Show("error");
            }
        }

        Tabele.Tabele w; 


        private void Ucionice_Click(object sender, RoutedEventArgs e)
        {
           
           w.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var r = new DDrop.PravljenjeRasporeda(raspored);
            r.ShowDialog();
            r.sacuvajRaspored();

        }

        bool FileOpenCore(string filepath)
        {
            MessageBox.Show(filepath);
            if (!File.Exists(filepath))
            {
                MessageBox.Show("File is removed or moved.");
                RecentFileList.RemoveFile(filepath);

            }
            // if deleted -> MessageBox and remove
            return true;
        }

        private void HandleWindowActivated(object sender, EventArgs e)
        {
            this.Focus();
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
                raspored = new Model.Raspored();
            }
        }

        void saveRecent(String filename)
        {
            String file_tekst = filename+"\n";
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

        Model.Raspored otvoriRaspored(String fileName)
        {
            Model.Raspored rasp = new Model.Raspored();
            rasp.File = fileName;
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[\r\n]{3,}", options);
            string open_text = File.ReadAllText(fileName);

            string[] tekst = open_text.Split('\n');
            rasp.Naziv = tekst[0];

            foreach (string pr in tekst[1].Split('|'))
            {
                string[] pr_termin = pr.Split(',');
               // pr_termin.ToList().Count
                if (pr_termin.ToList().Count == 2)
                {
                    if (pr_termin[0] != "" && pr_termin[1] != "")
                    {
                        Predmet p = w.nadjiPredmet(pr_termin[0]);
                        if (p != null)
                        {
                            p.BrojTermina = Convert.ToInt32(pr_termin[1]);
                            rasp.OstaliTermini.Add(p);
                        }
                       
                    }
                }
            }
            int broj = 2;
            while (true)
            {
                if (tekst.ToList().Count == broj || tekst[broj] == "" || tekst[broj] == "\r")
                    return rasp;
                string[] uc_term = tekst[broj].Split(':');               
                Ucionica u = w.nadjiUcionicu(uc_term[0]);
                UcionicaRaspored ur = new UcionicaRaspored(u);
                string[] pr = uc_term[1].Split('|');
                for (int i=0; i <61; i++ )
                {
                    if (pr[i] != "" || pr[i] != "\r")
                    {
                        string[] predmeti = pr[i].Split(',');
                        for (int j = 0; j < 7; j++)
                        {
                            Predmet p = w.nadjiPredmet(predmeti[j]);
                            if (p != null)
                                ur.Rasporedi[i][j] = p;
                        }
                    
                    }
                }
                rasp.Rasporedi.Add(ur);
                broj++;
                
            }

            return rasp;
        }



    }
}
