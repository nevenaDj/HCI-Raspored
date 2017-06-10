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

namespace Raspored.DDrop
{
    public partial class PrikaziPredmet : Window, INotifyPropertyChanged
    {
        public PrikaziPredmet()
        {
            InitializeComponent();
            this.DataContext = this;
            p = new Predmet();
        }

        public PrikaziPredmet(Predmet p)
        {
            InitializeComponent();
            this.DataContext = this;
            this.p = p;
           
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public Predmet p { get;  set; }
       

  

   

        private void HandleWindowActivated(object sender, EventArgs e)
        {

            this.Focus();
            Keyboard.Focus(this);
            FocusManager.SetFocusedElement(this, this);

        }
        

    }
        
}
