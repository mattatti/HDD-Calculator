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
        private List<EncodingType> _enc;
        private AdminLoginWindow _adminloginwin;
        private AddCameraWindow _addCameraWindow;
        private List<CameraName> _cameras;
        private CameraContext _cameracontext;

        public MainWindow()
        {
            InitializeComponent();
            //fill CameraName with all the different camera names from listcm
            _cameras = new List<CameraName>();
            _cameracontext = new CameraContext();
            _cameras = GetCameraNames(_cameracontext.Cameras.ToList());
       
            Camerasbox.ItemsSource = _cameras;
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
            foreach (var camera in _cameras)
            {
                if (camera.Name == _cam.Name)
                {
                    res = new List<Resolution>();
                    res = findResolutions(_cameracontext.Cameras.ToList() , _cam.Name);
                     List<Resolution> resnodup = new List<Resolution>();
                   
                    resnodup = GetResNames(res);
                    Resolutionbox.ItemsSource = resnodup;
                       
                    Resolutionbox.DisplayMemberPath = "Name";
                }
            }
            Resolutionbox.IsHitTestVisible = true;
        }

        private void Resolutionbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _resb = Resolutionbox.SelectedItem as Resolution;

            if (_resb == null)
                return;
            _enc = new List<EncodingType>();
            _enc = findEncodingTypes(_cameracontext.Cameras.ToList(), _cam.Name, _resb.Name);
            EncodingBox.ItemsSource = _enc.Distinct().ToList();
            EncodingBox.DisplayMemberPath = "Name";
            EncodingBox.IsHitTestVisible = true;
        }

        private void EncodingBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OptimalBitRateMessage.Visibility = Visibility.Visible;
            _encb = EncodingBox.SelectedItem as EncodingType;
            if (_encb == null) return;
            BitrateTextBlock.Text = findbitrate(_cameracontext.Cameras.ToList(), _cam.Name, _resb.Name, _encb.Name);
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

        private List<CameraName> GetCameraNames(List<Camera> cams)
        {
            var cam = new List<CameraName>();
            
              
            var tmpcameranames = new List<string>();
            foreach (var camera in cams)
            {
                tmpcameranames.Add(camera.CameraName);
            }
            tmpcameranames=tmpcameranames.Distinct().ToList();
            foreach (var camstring in tmpcameranames)
            {
                CameraName tmpcamname = new CameraName();
                tmpcamname.Name = camstring;
                cam.Add(tmpcamname); 
            }

            return cam;
        }

        private List<Resolution> findResolutions(List<Camera> cams, string cameraName)
        {
            List<Resolution> tempres = new List<Resolution>();
            foreach (var camera in cams)
            {
                if (camera.CameraName == cameraName)
                {
                    Resolution res = new Resolution();
                    res.Name = camera.Resolution;
                    tempres.Add(res);
                }
            }
            return tempres;
        }

        private List<EncodingType> findEncodingTypes(List<Camera> cams, string cameraName,string resolutionName)
        {
            List<EncodingType> tempenctype = new List<EncodingType>();
            foreach (var camera in cams)
            {
                if (camera.CameraName == cameraName && camera.Resolution == resolutionName)
                {
                    EncodingType enctype = new EncodingType();
                    enctype.Name = camera.EncodingType;
                    tempenctype.Add(enctype);
                }
            }
            return tempenctype;
        }

        private string findbitrate(List<Camera> cams, string cameraName, string resolutionName,string encodingtypeName)
        {
            foreach (var camera in cams)
            {
                if (camera.CameraName == cameraName && camera.Resolution == resolutionName && camera.EncodingType==encodingtypeName)
                {             
                    return camera.OptimalBitRate;
                }
            }

            return null;
        }

        private List<Resolution> GetResNames(List<Resolution> res)
        {
            var tempres = new List<Resolution>();
            
            var tmpresnames = new List<string>();
            foreach (var resolution in res)
            {
                tmpresnames.Add(resolution.Name);
            }
            tmpresnames = tmpresnames.Distinct().ToList();
            foreach (var camstring in tmpresnames)
            {
                Resolution tmpresname = new Resolution();
                tmpresname.Name = camstring;
                tempres.Add(tmpresname);
            }
            return tempres;
        }
    }
}
