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
                //ocitanje rasporeda iz fajla
                raspored = new Model.Raspored();

            } catch (Exception e)
            {
                Prozor2.Visibility = Visibility.Visible;
                Prozor1.Visibility = Visibility.Hidden;
                Raspored_Button.IsEnabled = false;
                //MessageBox.Show("error");
            }
        }

        


        private void Ucionice_Click(object sender, RoutedEventArgs e)
        {
           var w = new Tabele.Tabele();
           w.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var r = new DDrop.PravljenjeRasporeda(raspored);
            r.ShowDialog();

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
                saveRecent(filename);
                Prozor1.Visibility = Visibility.Visible;
                Prozor2.Visibility = Visibility.Hidden;
                Raspored_Button.IsEnabled = true;
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
        
    }
}
