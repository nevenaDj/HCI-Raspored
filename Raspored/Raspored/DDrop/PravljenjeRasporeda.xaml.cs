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
        private Point startPoint;

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

            List<Predmet> l = new List<Predmet>();
            l.Add(new Predmet { Naziv = "1" });
            l.Add(new Predmet { Naziv = "2" });
            l.Add(new Predmet { Naziv = "3" });
            l.Add(new Predmet { Naziv = "4" });
            l.Add(new Predmet { Naziv = "5" });

            Studenti = new ObservableCollection<Predmet>(l);
            Studenti2 = new ObservableCollection<Predmet>();
            Studenti3 = new ObservableCollection<Predmet>();
            Studenti4 = new ObservableCollection<Predmet>();
            Studenti5 = new ObservableCollection<Predmet>();

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
        public ObservableCollection<Predmet> Studenti { get; private set; }
        public ObservableCollection<Predmet> Studenti2 { get; private set; }
        public ObservableCollection<Predmet> Studenti3 { get; private set; }
        public ObservableCollection<Predmet> Studenti4 { get; private set; }
        public ObservableCollection<Predmet> Studenti5 { get; private set; }

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


        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
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

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Predmet student = e.Data.GetData("myFormat") as Predmet;
                if (Studenti2.Count == 0)
                {
                    Studenti.Remove(student);
                    Studenti2.Add(student);
                    Studenti3.Add(student);
                }
                
            }
        }
        private void ListView_Drop2(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Predmet student = e.Data.GetData("myFormat") as Predmet;
                if (Studenti3.Count == 0)
                {
                    Studenti.Remove(student);
                    Studenti3.Add(student);
                }

            }
        }

        private void ListView_Drop4(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Predmet student = e.Data.GetData("myFormat") as Predmet;
                if (Studenti4.Count == 0)
                {
                    Studenti2.Remove(student);
                    Studenti4.Add(student);
                }

            }
        }

        private void ListView_Drop5(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                Predmet student = e.Data.GetData("myFormat") as Predmet;
                if (Studenti5.Count == 0)
                {
                    Studenti2.Remove(student);
                    Studenti5.Add(student);
                }

            }
        }

    }
}
