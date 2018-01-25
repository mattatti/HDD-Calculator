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
        public delegate void DataChangedEventHandler(object sender, EventArgs e);
        public event DataChangedEventHandler DataChanged;
        public AddCameraWindow()
        {
            InitializeComponent();
            GlobalVariables.getInstance().Context = new CameraContext();
          
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalVariables.getInstance().Context.SaveChanges();
            this.cameraDataGrid.Items.Refresh();
            DataChangedEventHandler handler = DataChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource cameraViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("cameraViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // cameraViewSource.Source = [generic data source]

            GlobalVariables.getInstance().Context.Cameras.Load();

            cameraViewSource.Source = GlobalVariables.getInstance().Context.Cameras.Local;

        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
           // GlobalVariables.getInstance().Context.Dispose();
        }

    }
}
