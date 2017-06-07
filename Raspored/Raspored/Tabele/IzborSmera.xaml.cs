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
using Raspored.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

namespace Raspored.Tabele
{
    /// <summary>
    /// Interaction logic for IzborSmera.xaml
    /// </summary>
    public partial class IzborSmera : Window, INotifyPropertyChanged
    {
        public IzborSmera()
        {
            InitializeComponent();
        }

        public IzborSmera(Predmet predmet)
        {
            InitializeComponent();
            this.DataContext = this;
            IzabraniPredmet = predmet;

            IzabraniSmer = IzabraniPredmet.SmerPredmeta;
            if (IzabraniSmer != null)
            {
              //  SelectRowByIndex(dgrMainSmer, 0);
            }

            

            List<Smer> s = otvoriSmer(IzabraniSmer);
            Smerovi = new ObservableCollection<Smer>(s);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //MessageBox.Show("close");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          //  MessageBox.Show("click" + IzabraniSmer.Oznaka);

            IzabraniPredmet.SmerPredmeta = IzabraniSmer;
           
            


        }

        private Smer _izabraniSmer;
        public Smer IzabraniSmer
        {
            get
            {
                return _izabraniSmer;
            }
            set
            {
                if (_izabraniSmer != value)
                {
                    _izabraniSmer = value;
                    OnPropertyChanged("IzabraniSmer");
                }
            }
        }

        private Predmet _izabraniPredmet;
        public Predmet IzabraniPredmet
        {
            get
            {
                return _izabraniPredmet;
            }
            set
            {
                if (_izabraniPredmet != value)
                {
                    _izabraniPredmet = value;
                    OnPropertyChanged("IzabraniPredmet");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public ObservableCollection<Smer> Smerovi
        {
            get;
            set;
        }

        

        public List<Smer> otvoriSmer(Smer izabraniSmer)
        {

            List<Smer> smerovi = new List<Smer>();
            FileStream f = new FileStream("../../Save/smer.txt", FileMode.OpenOrCreate);
            f.Close();

            if (izabraniSmer != null)
            {
                smerovi.Add(izabraniSmer);
            }

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
                if (izabraniSmer != null)
                {
                    if (izabraniSmer.Oznaka != s.Oznaka)
                    {
                        smerovi.Add(s);
                    }
                }
                else
                {
                    smerovi.Add(s);

                }
                
            }

            return smerovi;
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

       
    }
}
