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
            l.Add(new Predmet { Naziv = "1", VelicinaGrupe=3, DuzinaTermina=45 });
            l.Add(new Predmet { Naziv = "2", VelicinaGrupe = 3, DuzinaTermina = 90 });
            l.Add(new Predmet { Naziv = "3", VelicinaGrupe = 3, DuzinaTermina = 45 });
            l.Add(new Predmet { Naziv = "4", VelicinaGrupe = 3, DuzinaTermina = 45 });
            l.Add(new Predmet { Naziv = "5", VelicinaGrupe = 3, DuzinaTermina = 45 });

            Studenti = new ObservableCollection<Predmet>(l);


            StudentiTo= new List<List<ObservableCollection<Predmet>>>();
            for (int i = 0; i < 60; i++) {
                List<ObservableCollection<Predmet>> temp = new List<ObservableCollection<Predmet>>();
                for (int j = 0; j < 7; j++)
                    temp.Add(new ObservableCollection<Predmet>());
                StudentiTo.Add(temp);
            }
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
        public List<List<ObservableCollection<Predmet>>> StudentiTo { get; private set; }
        private bool fromList = true;
        private int from_r = 0;
        private int from_c = 0;
        private ListView lv;

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
            lv = sender as ListView;
            String v = lv.Name.ToString();
            String r = v.Substring(4, 1);
            from_r = Convert.ToInt32(r) ;
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
            String r= v.Substring(4, 1);
            int r1= Convert.ToInt32(r) ;
            String c = v.Substring(2, 2);
            int c1 = Convert.ToInt32(c);
            //MessageBox.Show(r1.ToString());

            if (e.Data.GetDataPresent("myFormat"))
            {
                Predmet student = e.Data.GetData("myFormat") as Predmet;
                if (StudentiTo[c1][r1].Count == 0)
                    {
                    if (fromList)
                    {
                        Studenti.Remove(student);
                    }
                    else
                    {
                        int i = 0;
                        int x = 0;
                        while (true)
                        {
                           
                            foreach (Predmet p in StudentiTo[from_c - i - 1][from_r]) { 
                                if (p.Naziv != student.Naziv)
                                    x = 1;
                                }
                           // MessageBox.Show("" + x);
                            if (x == 1 || StudentiTo[from_c - i - 1][from_r].Count==0)
                                break;
                            i++;
                        }
                        
                        for (int j = 0; j < student.DuzinaTermina / 15; j++)
                        {
                            StudentiTo[from_c - i + j][from_r].Remove(student);
                            lv.Background = Brushes.White;

                        }
                    }
                   // StudentiTo[c1][r1].Add(student);
                    listView.Background = Brushes.LightGreen;
                    for (int i=0;i<student.DuzinaTermina/15; i++) {
                        StudentiTo[c1+i][r1].Add(student);
                        //listView.Background = Brushes.LightGreen;
                        //String ime = "lw" + (c1 + i) + (r1 + 1);
                        
                     }

                }
                fromList = false;


            }
        }

    }
}
