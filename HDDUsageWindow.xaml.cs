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
    public partial class HDDUsageWindow : Window
    {
        private CameraName _cam;
        private Resolution _resb;
        private EncodingType _encb;
        private BitRate _bitRate;
        private ChannelNumber _channelNumber;
        private OssiaOS _ossiaOS;

        private readonly int _cameraColumn = 0;
        private readonly int _resolutionColumn = 1;
        private readonly int _encodingtypeColumn = 2;
        private readonly int _bitrateColumn = 3;
        private readonly int _maxChannelNumber = 128;

        public HDDUsageWindow()
        {
            InitializeComponent();
          
            Camerasbox.ItemsSource = StringToCameraNameList(GlobalVariables.getInstance()._databaseSize, _cameraColumn);
            Camerasbox.DisplayMemberPath = "Name";

            Resolutionbox.IsHitTestVisible = false;
            EncodingBox.IsHitTestVisible = false;
            ChannelNumberBox.ItemsSource= InitChannelNumbers();    
            ChannelNumberBox.DisplayMemberPath = "Value";
            OssiaOSBox.ItemsSource = InitOssiaOS();
            OssiaOSBox.DisplayMemberPath = "Name";
            BitRateBox.ItemsSource = StringToBitRateList(GlobalVariables.getInstance()._databaseSize, _bitrateColumn);
            BitRateBox.DisplayMemberPath = "Value";
        
        }
       
        protected override void OnSourceInitialized(EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }
        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();

        }

        private List<OssiaOS> InitOssiaOS()
        {
            List<OssiaOS> _oss_OS = new List<OssiaOS>();
            OssiaOS os1= new OssiaOS();
            os1.Name = "Yes";
            _oss_OS.Add(os1);
            OssiaOS os2 = new OssiaOS();
            os2.Name = "No";
            _oss_OS.Add(os2);
            return _oss_OS;
        }

        private List<ChannelNumber> InitChannelNumbers()
        {
            List<ChannelNumber> channel_num = new List<ChannelNumber>();
            for (var i = 1; i <= _maxChannelNumber; i++)
            {
                ChannelNumber cn = new ChannelNumber();
                cn.Value = i;
                channel_num.Add(cn);
            }
            return channel_num;
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

        private void BitRateBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
               _bitRate = BitRateBox.SelectedItem as BitRate;
               List<BitRate> brate = new List<BitRate>(); 
               if (_bitRate == null)
                   return;
               // add bitrate to the table (datagrid) and calculation class

            
        }

        private void ChannelNumberBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _channelNumber = ChannelNumberBox.SelectedItem as ChannelNumber;

            // add bitrate to the table (datagrid) and calculation class
        }

        private void OssiaOSBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SubStremBitRateBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Calculate_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RecordingTimePerDayBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
