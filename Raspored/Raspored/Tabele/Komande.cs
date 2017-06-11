using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Raspored.Tabele
{
    public static class Komande
    {
        public static readonly RoutedUICommand Izmena = new RoutedUICommand(
            "Izmena",
            "Izmena",
            typeof(Komande),
            new InputGestureCollection()
            {
                new KeyGesture(Key.S, ModifierKeys.Control),
            }
            );

        public static readonly RoutedUICommand Dodavanje = new RoutedUICommand(
            "Dodavanje",
            "Dodavanje",
            typeof(Komande),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D, ModifierKeys.Control),
            }
            );

        public static readonly RoutedUICommand Novi = new RoutedUICommand(
          "Novi",
          "Novi",
          typeof(Komande),
          new InputGestureCollection()
          {
                new KeyGesture(Key.N, ModifierKeys.Control),
            }
          );
        public static readonly RoutedUICommand Izmeni = new RoutedUICommand(
          "Izmeni",
          "Izmeni",
          typeof(Komande),
          new InputGestureCollection()
          {
                new KeyGesture(Key.U, ModifierKeys.Control),
            }
          );

        public static readonly RoutedUICommand Brisanje = new RoutedUICommand(
           "Brisanje",
           "Brisanje",
           typeof(Komande),
           new InputGestureCollection()
           {
                new KeyGesture(Key.Delete, ModifierKeys.Control),
            }
           );

        public static readonly RoutedUICommand Pretraga = new RoutedUICommand(
          "Pretraga",
          "Pretraga",
          typeof(Komande),
          new InputGestureCollection()
          {
                new KeyGesture(Key.F, ModifierKeys.Control),
           }
          );
    }
}
