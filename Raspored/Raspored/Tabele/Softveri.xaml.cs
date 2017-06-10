using Raspored.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Raspored.Tabele
{
    public partial class SoftveriOtvori : Window
    {
        public SoftveriOtvori()
        {
            InitializeComponent();
            this.DataContext = this;
            List<Softver> sf = otvoriSoftver();
            List2 = new ObservableCollection<Softver>(sf);
            List1 = new ObservableCollection<Softver>();
            Search = "";
        }

        public SoftveriOtvori(Ucionica u)
        {
            InitializeComponent();
            this.DataContext = this;
            Search = "";
            if (u.Softveri == null)
            {
                List<Softver> sf = otvoriSoftver();
                List2 = new ObservableCollection<Softver>(sf);
                List1 = new ObservableCollection<Softver>();
                SaveList2= new List<Softver>();
            }
            else
            {
                List<Softver> l2 = otvoriSoftver();
                List<Softver> l1 = new List<Softver>();
                foreach (Softver s in u.Softveri)
                {
                    //MessageBox.Show(s.Naziv);
                    l1.Add(s);
                    Softver remove = null;
                    foreach (Softver s_r in l2)
                        if (s_r.Oznaka == s.Oznaka)
                            remove = s_r;
                    l2.Remove(remove);
                }
                List2 = new ObservableCollection<Softver>(l2);
                List1 = new ObservableCollection<Softver>(l1);
            }

        }

        public SoftveriOtvori(Predmet u)
        {
            InitializeComponent();
            this.DataContext = this;
            Search = "";
            SaveList2 = new List<Softver>();
            if (u.Softveri == null)
            {
                List<Softver> sf = otvoriSoftver();
                List2 = new ObservableCollection<Softver>(sf);
                List1 = new ObservableCollection<Softver>();
            }
            else
            {
                List<Softver> l2 = otvoriSoftver();
                List<Softver> l1 = new List<Softver>();
                foreach (Softver s in u.Softveri)
                {
                    //MessageBox.Show(s.Naziv);
                    l1.Add(s);
                    Softver remove = null;
                    foreach (Softver s_r in l2)
                        if (s_r.Oznaka == s.Oznaka)
                            remove = s_r;
                    l2.Remove(remove);
                }
                List2 = new ObservableCollection<Softver>(l2);
                List1 = new ObservableCollection<Softver>(l1);
            }

        }

        public ObservableCollection<Model.Softver> List1 { get;  set; }
        public ObservableCollection<Model.Softver> List2 { get;  set; }
        public ObservableCollection<Model.Softver> SaveList1 { get;  set; }
        public List<Model.Softver> SaveList2 { get;  set; }
        public String Search { get; set; }
        private Point startPoint;
        private int list = 0;

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
            list = 1;

        }

        private void ListView_PreviewMouseLeftButtonDown2(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
            list = 2;
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
                Model.Softver student = (Model.Softver)listView.ItemContainerGenerator.
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
            if (list == 1)
                if (e.Data.GetDataPresent("myFormat"))
                {
                    Model.Softver soft = e.Data.GetData("myFormat") as Model.Softver;
                 
                    List1.Remove(soft);
                    List2.Add(soft);
                    //SaveList2.Add(soft);
                   
                }
        }

        private void ListView_Drop2(object sender, DragEventArgs e)
        {
            if (list==2)
                if (e.Data.GetDataPresent("myFormat"))
                {
                    Model.Softver soft = e.Data.GetData("myFormat") as Model.Softver;
                    if (SaveList2 == null)
                    {
                        SaveList2 = new List<Softver>();
                        foreach (Softver s in List2)
                            if (s != null)
                                SaveList2.Add(s);
                    }
                    List2.Remove(soft);
                    List1.Add(soft);
                    SaveList2.Remove(soft);
                    /* SaveList2 = new List<Softver>();
                     foreach (Softver s in List2)
                         if (s != null)
                             SaveList2.Add(s);
                     Search = "";
                     just_do_it();*/
                }
        }

        private void HandleWindowActivated(object sender, EventArgs e)
        {

            this.Focus();

        }

        List<Softver> otvoriSoftver()
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
                Model.Softver s = new Model.Softver();
                if (softver == "")
                    return softveri;
                string[] sf = softver.Split('|');

                s.Oznaka = sf[0];
                s.Naziv = sf[1];
                s.Cena = Convert.ToDouble(sf[2]);
                s.GodinaIzdavanja = Convert.ToInt32(sf[3]);
                s.Sistem = sf[4];

                s.Opis = sf[5];
                s.Proizvodjac = sf[6];
                s.Sajt = sf[7].Trim();

                softveri.Add(s);
            }

            return softveri;
        }

        public List<Softver> getList1()
        {
            List<Softver> retVal = new List<Softver>();
            foreach (Softver s in List1)
                retVal.Add(s);
            return retVal;
        }

     

      


        private void Text_Changed(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
           // MessageBox.Show("Search "+ textBox.Text);
            if (SaveList2 != null)
                foreach (Softver s in SaveList2)
                    if (!List2.Contains(s))
                        List2.Add(s);
            if (textBox.Text != "")

            {
                SaveList2 = new List<Softver>();
                foreach (Softver s in List2)
                    if (s != null)
                        SaveList2.Add(s);

                //MessageBox.Show("Savelist: "+ SaveList2.Count);

                List<Softver> filtered = new List<Softver>();
                foreach (Softver s in List2)
                {
                    if (!s.Naziv.Contains(textBox.Text) && !s.Oznaka.Contains(textBox.Text))
                    {
                        filtered.Add(s);
                    }

                }
                if (filtered.Count != 0)
                {
                    foreach (Softver f in filtered)
                        List2.Remove(f);
                    //List2.Remove(s => s);
                    // List2 = new ObservableCollection<Softver>(filtered);
                    // MessageBox.Show("jepp: "+filtered.Count);
                }

            }

        }

    }
        
}
