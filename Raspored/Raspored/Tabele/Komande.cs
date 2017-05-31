﻿using System;
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
              //  new KeyGesture(Key.S, ModifierKeys.Control),
            }
            );

        public static readonly RoutedUICommand Dodavanje = new RoutedUICommand(
            "Dodavanje",
            "Dodavanje",
            typeof(Komande),
            new InputGestureCollection()
            {
             //   new KeyGesture(Key.D, ModifierKeys.Control),
            }
            );
    }
}