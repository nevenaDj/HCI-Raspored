﻿using System;
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

namespace Raspored.Tabele
{
    /// <summary>
    /// Interaction logic for IzborSmera.xaml
    /// </summary>
    public partial class IzborSmera : Window
    {
        public IzborSmera()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //MessageBox.Show("close");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("click");

        }
    }
}
