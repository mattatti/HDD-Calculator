using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;

namespace HDD_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CameraName _cam;
        private Resolution _resb;
        private EncodingType _encb;
        private ObservableCollection<EncodingType> _enc;
        private AdminLoginWindow _adminloginwin;
        private AddCameraWindow _addCameraWindow;


        public MainWindow()
        {
            InitializeComponent();
            var cam = new List<CameraName>
            {
                new CameraName() {Name = "Dark_Sight"},
                new CameraName() {Name = "S_Sight"},
                new CameraName() {Name = "X_Sight"}
            };
            Camerasbox.ItemsSource = cam;
            Camerasbox.DisplayMemberPath = "Name";
          
            Resolutionbox.IsHitTestVisible = false;
            EncodingBox.IsHitTestVisible = false;
            this.Closed += new EventHandler(MainWindow_Closed);
           

        }

        private void MainWindow_Closed(object sender, EventArgs args)
        {
            _adminloginwin.Close();
            _addCameraWindow.Close();
            this.Close();
           
        }

        private void Camerasbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            EncodingBox.IsHitTestVisible = false;
            _enc?.Clear();
            this.BitrateTextBlock.Text = String.Empty;

            List<Resolution> res;
            _cam = Camerasbox.SelectedItem as CameraName;
            if (_cam == null) return;
            switch (_cam.Name)
            {
                case "Dark_Sight":
                    res = new List<Resolution>();
                    res.Add(new Resolution() { Name = "3MP" });
                    res.Add(new Resolution() { Name = "5MP" });
                    Resolutionbox.ItemsSource = res;
                    Resolutionbox.DisplayMemberPath = "Name";
                    break;

                case "S_Sight":
                    res = new List<Resolution>();
                    res.Add(new Resolution() { Name = "1MP" });
                    res.Add(new Resolution() { Name = "2MP" });
                    Resolutionbox.ItemsSource = res;
                    Resolutionbox.DisplayMemberPath = "Name";
                    break;
                case "X_Sight":
                    res = new List<Resolution>();
                    res.Add(new Resolution() { Name = "3MP" });
                    res.Add(new Resolution() { Name = "5MP" });
                    Resolutionbox.ItemsSource = res;
                    Resolutionbox.DisplayMemberPath = "Name";
                    break;

                default:
                    return;
            }

            Resolutionbox.IsHitTestVisible = true;

        }


        private void Resolutionbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            _resb = Resolutionbox.SelectedItem as Resolution;

            if (_resb == null)
                return;
            if (_cam.Name == "Dark_Sight" && (_resb.Name == "3MP" || _resb.Name == "5MP"))
            {


                _enc = new ObservableCollection<EncodingType>
                {
                    new EncodingType() {Name = "H264"},
                    new EncodingType() {Name = "H265"}
                };
                EncodingBox.ItemsSource = _enc;
                EncodingBox.DisplayMemberPath = "Name";
            }
            if (_cam.Name == "X_Sight" && (_resb.Name == "3MP" || _resb.Name == "5MP"))
            {


                _enc = new ObservableCollection<EncodingType>
                {
                    new EncodingType() {Name = "H264"},
                    new EncodingType() {Name = "H265"}
                };
                EncodingBox.ItemsSource = _enc;
                EncodingBox.DisplayMemberPath = "Name";
            }

            if (_cam.Name == "S_Sight" && (_resb.Name == "1MP" || _resb.Name == "2MP"))
            {


                _enc = new ObservableCollection<EncodingType>
                {

                    new EncodingType() {Name = "H265"}
                };
                EncodingBox.ItemsSource = _enc;
                EncodingBox.DisplayMemberPath = "Name";
            }
            EncodingBox.IsHitTestVisible = true;
        }

        private void EncodingBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OptimalBitRateMessage.Visibility = Visibility.Visible;
            _encb = EncodingBox.SelectedItem as EncodingType;
            if (_encb == null) return;
            if (_cam.Name == "Dark_Sight" && _encb.Name == "H264" && _resb.Name == "3MP")
            {

                BitrateTextBlock.Text = "4096";

            }

        }

        private void AdminLogin_button_Click(object sender, RoutedEventArgs e)
        {
           
             _adminloginwin = new AdminLoginWindow();
            _adminloginwin.SubmitClicked += adminloginwin_SubmitClicked;
            _adminloginwin.ShowDialog();


        }
        private void adminloginwin_SubmitClicked(object sender, EventArgs e)
        {
            WelcomeMessage.Text = "Welcome" + " " + _adminloginwin.UserName;
            AddCameraType.Visibility = Visibility.Visible;
        }

        private void AddCameraType_Click(object sender, RoutedEventArgs e)
        {
            _addCameraWindow = new AddCameraWindow();
            _addCameraWindow.Show();
        }
    }
}
