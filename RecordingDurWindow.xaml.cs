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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace HDD_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class RecordingDurWindow : Window
    {
        private CameraName _cam;
        private Resolution _resb;
        private EncodingType _encb;
        private BitRate _bitRate;

        private readonly int _cameraColumn = 0;
        private readonly int _resolutionColumn = 1;
        private readonly int _encodingtypeColumn = 2;
        private readonly int _bitrateColumn = 3;

        public RecordingDurWindow()
        {
            InitializeComponent();
            //_databaseSize = Load_Excel_File();
            //fill CameraName with all the different camera names from listcm

            Camerasbox.ItemsSource = StringToCameraNameList(GlobalVariables.getInstance()._databaseSize, _cameraColumn);
            Camerasbox.DisplayMemberPath = "Name";

            Resolutionbox.IsHitTestVisible = false;
            EncodingBox.IsHitTestVisible = false;

            BitRateBox.ItemsSource = StringToBitRateList(GlobalVariables.getInstance()._databaseSize, _bitrateColumn);
            BitRateBox.DisplayMemberPath = "Value";
            //  this.Closed += new EventHandler(MainWindow_Closed);


        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();

        }
        private List<BitRate> StringToBitRateList(int _databaseSize, int ColumnNumber)
        {
            List<BitRate> brate = new List<BitRate>();
            List<string> temp = new List<string>();
            for (var i = 0; i < _databaseSize; i++)
            {
                temp.Add(GlobalVariables.getInstance()._database[i, ColumnNumber]);
            }
            temp = temp.Distinct().ToList();

            for (var i = 0; i < temp.Count; i++)
            {
                BitRate br = new BitRate();
                br.Value = Int32.Parse(temp[i]);
                brate.Add(br);
            }
            brate = brate.OrderBy(x => x.Value).ToList();
            return brate;
        }
        private List<CameraName> StringToCameraNameList(int _databaseSize, int ColumnNumber)
        {
            List<CameraName> cam = new List<CameraName>();
            List<string> temp = new List<string>();
            for (var i = 0; i < _databaseSize; i++)
            {
                temp.Add(GlobalVariables.getInstance()._database[i, ColumnNumber]);
            }
            temp = temp.Distinct().ToList();
            for (var i = 0; i < temp.Count; i++)
            {
                CameraName cn = new CameraName();
                cn.Name = temp[i];
                cam.Add(cn);
            }
            cam = cam.OrderBy(x => x.Name).ToList();
            return cam;
        }





        private void Camerasbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EncodingBox.ItemsSource = "";
            EncodingBox.IsHitTestVisible = false;

            this.OptimalBitrateTextBlock.Text = String.Empty;

            List<Resolution> res;
            _cam = Camerasbox.SelectedItem as CameraName;
            if (_cam == null) return;
            res = new List<Resolution>();

            List<string> temp = new List<string>();
            for (var index = 0; index < GlobalVariables.getInstance()._databaseSize; index++)
            {
                if (_cam.Name == GlobalVariables.getInstance()._database[index, _cameraColumn])
                {
                    temp.Add(GlobalVariables.getInstance()._database[index, _resolutionColumn]);
                }
            }
            temp = temp.Distinct().ToList();
            for (var i = 0; i < temp.Count; i++)
            {
                //check all instances of cam.name and fill into a list of resolutions
                Resolution r = new Resolution();
                r.Name = temp[i];
                res.Add(r);
            }
            res = res.OrderBy(x => x.Name).ToList();

            Resolutionbox.ItemsSource = res;
            Resolutionbox.DisplayMemberPath = "Name";
            Resolutionbox.IsHitTestVisible = true;
            //need to save in another database the selected camera and resolution
        }

        private void Resolutionbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            _resb = Resolutionbox.SelectedItem as Resolution;
            List<EncodingType> enctype = new List<EncodingType>(); ;
            if (_resb == null)
                return;

            List<string> temp = new List<string>();
            for (var index = 0; index < GlobalVariables.getInstance()._databaseSize; index++)
            {
                if (_cam.Name == GlobalVariables.getInstance()._database[index, _cameraColumn] && _resb.Name == GlobalVariables.getInstance()._database[index, _resolutionColumn])
                {
                    temp.Add(GlobalVariables.getInstance()._database[index, _encodingtypeColumn]);
                }
            }
            temp = temp.Distinct().ToList();
            for (var i = 0; i < temp.Count; i++)
            {
                //check all instances of cam.name and fill into a list of resolutions
                EncodingType r = new EncodingType();
                r.Name = temp[i];
                enctype.Add(r);
            }

            enctype = enctype.OrderBy(x => x.Name).ToList();

            EncodingBox.ItemsSource = enctype;
            EncodingBox.DisplayMemberPath = "Name";



            EncodingBox.IsHitTestVisible = true;
            //need to save in another database the selected resolution
        }

        private void EncodingBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OptimalBitRateMessage.Visibility = Visibility.Visible;
            _encb = EncodingBox.SelectedItem as EncodingType;
            if (_encb == null) return;
            for (var index = 0; index < GlobalVariables.getInstance()._databaseSize; index++)
            {
                if (_cam.Name == GlobalVariables.getInstance()._database[index, _cameraColumn] && _resb.Name == GlobalVariables.getInstance()._database[index, _resolutionColumn] && _encb.Name == GlobalVariables.getInstance()._database[index, _encodingtypeColumn])
                {
                    OptimalBitrateTextBlock.Text = GlobalVariables.getInstance()._database[index, _bitrateColumn] + " Kbps";
                    break;
                }
            }
            //need to save in another database the selected encoding type
        }

        private void BitRate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*   _bitRate = Resolutionbox.SelectedItem as BitRate;
               List<BitRate> brate = new List<BitRate>(); ;
               if (_bitRate == null)
                   return;

               List<string> temp = new List<string>();
               for (var index = 0; index < _databaseSize; index++)
               {
                   if (_bitRate.Name == _database[index, _bitrateColumn])
                   {
                       temp.Add(_database[index, _encodingtypeColumn]);
                   }
               }
               temp = temp.Distinct().ToList();
               for (var i = 0; i < temp.Count; i++)
               {
                   //check all instances of cam.name and fill into a list of resolutions
                   EncodingType r = new EncodingType();
                   r.Name = temp[i];
                   enctype.Add(r);
               }


               EncodingBox.ItemsSource = enctype;
               EncodingBox.DisplayMemberPath = "Name";



               EncodingBox.IsHitTestVisible = true;*/
        }




     /*   private List<CameraName> SetCameraNames(List<Camera> cams)
        {
            var cam = new List<CameraName>();


            var tmpcameranames = new List<string>();
            foreach (var camera in cams)
            {
                tmpcameranames.Add(camera.CameraName);
            }
            tmpcameranames = tmpcameranames.Distinct().ToList();
            foreach (var camstring in tmpcameranames)
            {
                CameraName tmpcamname = new CameraName();
                tmpcamname.Name = camstring;
                cam.Add(tmpcamname);
            }

            return cam;
        }*/
    }
}
