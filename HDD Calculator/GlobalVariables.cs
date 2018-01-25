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
        private static readonly GlobalVariables _instance = new GlobalVariables();
        public static GlobalVariables getInstance()
        { 
            return _instance;
        }

        private List<Camera> _cameras;
        public List<Camera> Cameras
        {
            get { return _cameras;}
            set
            {
                _cameras = value;
                OnPropertyChanged("Cameras");
            }
        }

        private CameraContext _context;
        public CameraContext Context
        {
            get { return _context; }
            set
            {
                _context = value;
                OnPropertyChanged("Context");
            }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }

}
