using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Data.Entity;


namespace HDD_Calculator
{
   
    /// <summary>
    /// Interaction logic for AddCameraWindow.xaml
    /// </summary>
    public partial class AddCameraWindow : Window
    {
        private CameraContext _context = new CameraContext();
        private DbContext db;
        private  List<Camera> _camera = new List<Camera>();
        
        public AddCameraWindow()
        {
            InitializeComponent();
        }

        private void AddCameraButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var product in _context.Cameras.Local.ToList())
            {
                ;
            }
            _context.SaveChanges();
            this.cameraDataGrid.Items.Refresh();

            /* Camera cam = new Camera
              {
                 
                  CameraName = Cameranamebox.Text ,
                  Resolution = Resolutionbox.Text ,
                  EncodingType =EncodingTypebox.Text ,
                  OptimalBitRate =OptimalBitRatebox.Text
  
              };
              
            
             _context.Cameras.Add(cam);
              _context.Cameras.Local.ToList();
          
              int res= _context.SaveChanges();
            // int id =  _dbContext.Camera.;
              _camera.Add(cam);
              Cameranamebox.Text = "";
               Resolutionbox.Text = "";
              EncodingTypebox.Text = "";
              OptimalBitRatebox.Text = "";
              
             TheEnclosingMethod();
            
            
          }
          private async void TheEnclosingMethod()
          {
              CameraInfoMessage.Visibility = Visibility.Visible;
              await Task.Delay(1000);
              CameraInfoMessage.Visibility = Visibility.Hidden;
          }
          */
            this._context.Dispose();
            Close();
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource cameraViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("cameraViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // cameraViewSource.Source = [generic data source]

            _context.Cameras.Load();

            cameraViewSource.Source = _context.Cameras.Local;

        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            this._context.Dispose();
        }

    }
}
