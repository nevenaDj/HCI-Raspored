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

namespace Raspored.DDrop
{
    /// <summary>
    /// Interaction logic for PravljenjeRasporeda.xaml
    /// </summary>
    public partial class PravljenjeRasporeda : Window
    {
        public PravljenjeRasporeda()
        {
            InitializeComponent();
            this.DataContext = this;
            Smerovi = new ObservableCollection<Smer>();
            Smer s = new Smer() { Naziv = "E2" };
            Smerovi.Add(s);
            s = new Smer() { Naziv = "SIIT" };
            Smerovi.Add(s);

            Predmeti = new ObservableCollection<Predmet>();

        }
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

        private void Korak1_Click(object sender, RoutedEventArgs e)
        {
            Raspored1.Visibility = Visibility.Collapsed;
            Raspored2.Visibility = Visibility.Visible;

        }

        private void Korak2_Click(object sender, RoutedEventArgs e)
        {
            Raspored2.Visibility = Visibility.Collapsed;
            Raspored3.Visibility = Visibility.Visible;

        }
    }
}
