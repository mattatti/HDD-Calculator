using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HDD_Calculator
{
    public class GlobalVariables : INotifyPropertyChanged
    {
        private GlobalVariables() { }
        private static GlobalVariables instance = null;
        public static GlobalVariables getInstance()
        {
            if (instance == null)
            {
                instance = new GlobalVariables();
            }
            return instance;
        }

        public int _databaseSize { get; set; }
        public string[,] _database { get; set; }
        public bool initflag = true;

        
        // public CameraContext Context { get; set; }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }

}
