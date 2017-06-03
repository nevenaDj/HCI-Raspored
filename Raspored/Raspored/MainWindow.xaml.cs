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
                raspored = new Model.Raspored();
                raspored.File = recentFile;

            } catch (Exception e)
            {
                Prozor2.Visibility = Visibility.Visible;
                Prozor1.Visibility = Visibility.Hidden;
                Raspored_Button.IsEnabled = false;
                //MessageBox.Show("error");
            }
        }

        Tabele.Tabele w = new Tabele.Tabele();


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

        Model.Raspored OtvoriRaspored(String fileName)
        {
            Model.Raspored rasp = new Model.Raspored();

           /* List<Predmet> predmeti = new List<Predmet>();
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

            }*/
            return rasp;
        }



    }
}
