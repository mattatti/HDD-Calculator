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
        private SubSteamBitRate _subSteamBitRate;
       // private RecordingTimePerDay _recordingTimePerDay;
        private ObservableCollection<DatagridData> datagridData = new ObservableCollection<DatagridData>();
        private readonly int _cameraColumn = 0;
        private readonly int _resolutionColumn = 1;
        private readonly int _encodingtypeColumn = 2;
        private readonly int _bitrateColumn = 3;
        private readonly int _maxChannelNumber = 128;
        private readonly int _maxHoursInADay = 24;
        private int _numValue = 0;
        private int _datagridDataIndex = 1;
        private double _capacityInTB;
        private double _totalcapacity = 0;
        private List<int> _h264list = new List<int>()
        {
          64,128,256,384,512,768,1024,1536,2048,2304,2560,3072,4096,5120,6144,7168,8192,10240,12536
        };
        private List<int> _h265list = new List<int>()
        {
          64,128,256,384,512,768,1024,1536,2048,2304,2560,3072,4096,5120,6144,7168
        };
       
       private  List<OssiaOS> _oss_OS = new List<OssiaOS>();



        public HDDUsageWindow()
        {
            InitializeComponent();
          
            Camerasbox.ItemsSource = StringToCameraNameList(GlobalVariables.getInstance()._databaseSize, _cameraColumn);
            Camerasbox.DisplayMemberPath = "Name";
            
            Resolutionbox.IsHitTestVisible = false;
            EncodingBox.IsHitTestVisible = false;
            BitRateBox.IsHitTestVisible = false;

            ChannelNumberBox.ItemsSource= InitChannelNumbers();    
            ChannelNumberBox.DisplayMemberPath = "Value";
            OssiaOSBox.ItemsSource = InitOssiaOS();
            OssiaOSBox.DisplayMemberPath = "Name";
            OssiaOSBox.Text = "Yes";
            //List<BitRate> tempbrate = new List<BitRate>();
            //tempbrate = StringToBitRateList(GlobalVariables.getInstance()._databaseSize, _bitrateColumn);
            SubStreamBitRateBox.ItemsSource = SubStreamBitRateList();
            SubStreamBitRateBox.DisplayMemberPath = "Value";
            //BitRateBox.ItemsSource = tempbrate;
            //BitRateBox.DisplayMemberPath = "Value";
            RecordingTimePerDayBox.ItemsSource = InitRecordingTimePerDay();
            RecordingTimePerDayBox.DisplayMemberPath = "Value";

            RequiredRecordingTimeBox.Text = _numValue.ToString();
            // SubStreamBitRateBox.IsHitTestVisible = false;
            //  this.DataContext = datagridData;
            
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
        
        private List<RecordingTimePerDay> InitRecordingTimePerDay()
        {
            List<RecordingTimePerDay> recordingtimeperday = new List<RecordingTimePerDay>();
            for (var i = 1; i <= _maxHoursInADay; i++)
            {
                RecordingTimePerDay rtpd = new RecordingTimePerDay();
                rtpd.Value = i;
                recordingtimeperday.Add(rtpd);
            }
            return recordingtimeperday;
        }
        private List<SubSteamBitRate> SubStreamBitRateList()
        {
            List<SubSteamBitRate> substeambitrate = new List<SubSteamBitRate>();

            substeambitrate.Add(new SubSteamBitRate() { Value = 64 });
            substeambitrate.Add(new SubSteamBitRate() { Value = 128 });
            substeambitrate.Add(new SubSteamBitRate() { Value = 256 });
            substeambitrate.Add(new SubSteamBitRate() { Value = 384 });
            substeambitrate.Add(new SubSteamBitRate() { Value = 512 });
            substeambitrate.Add(new SubSteamBitRate() { Value = 1024 });
            substeambitrate.Add(new SubSteamBitRate() { Value = 1536 });
            substeambitrate.Add(new SubSteamBitRate() { Value = 2048 });
            substeambitrate.Add(new SubSteamBitRate() { Value = 2304 });
            substeambitrate.Add(new SubSteamBitRate() { Value = 2560 });
            substeambitrate.Add(new SubSteamBitRate() { Value = 3072 });
            substeambitrate.Add(new SubSteamBitRate() { Value = 4096 });


       
        
            return substeambitrate;
        }
        private List<OssiaOS> InitOssiaOS()
        {
           
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
            Resolutionbox.IsHitTestVisible = false;
            Resolutionbox.ItemsSource = "";
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
            BitRateBox.IsHitTestVisible = false;
            BitRateBox.Text = "";
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
            BitRateBox.Text = "";

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

            if (_encb.Name == "H.264")
            {
               // BitRateBox.ItemsSource = ;
                BitRateBox.ItemsSource = _h264list;
            }
            if (_encb.Name == "H.264+")
            {
               // BitRateBox.ItemsSource = _h264list;
                BitRateBox.ItemsSource = _h264list;

            }
            if (_encb.Name == "H.265")
            {
               // BitRateBox.ItemsSource = _h265list;
                BitRateBox.ItemsSource = _h265list;

            }
            BitRateBox.IsHitTestVisible = true;

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

            _ossiaOS = OssiaOSBox.SelectedItem as OssiaOS;
            if (_ossiaOS.Name == "No")
            {
                SubStreamBitRateBox.IsHitTestVisible = false;
                SubStreamBitRateBox.Text = "";
                SubStreamKbps.Visibility = Visibility.Hidden;
            }
            else if (_ossiaOS.Name == "Yes")
            {
                SubStreamBitRateBox.IsHitTestVisible = true;
                SubStreamKbps.Visibility = Visibility.Visible;
               
            }
        }

        private void SubStremBitRateBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _subSteamBitRate= SubStreamBitRateBox.SelectedItem as SubSteamBitRate;
        }

        private void Calculate_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Check that Bit-Rate & Channel & Ossia OS & Sub-Stream Bit-Rate & Recording Time Per Day & Required recording time fields aren't empty
                if (_ossiaOS.Name == "Yes" && SubStreamBitRateBox.Text != "" && RecordingTimePerDayBox.Text != "" && ChannelNumberBox.Text != "" && BitRateBox.Text != "")//bitrate channel# 
                {
                  
                    // ((G49 + K49) * I49 * 3600 / 8 / 1024) * (Q49*T49/1024) if answer in gigabyte
                    double x = (double)((double)(int.Parse(BitRateBox.Text) + (double)int.Parse(SubStreamBitRateBox.Text)) * (double)int.Parse(ChannelNumberBox.Text)
                          * 3600 / 8 / 1024) * (double)int.Parse(RecordingTimePerDayBox.Text) * (double)int.Parse(RequiredRecordingTimeBox.Text) / 1024 / 1024;
                    //SubStreamBitRateBox might be null
                    _capacityInTB = Math.Round(x, 4);// answer in terabyte
                    help_calculation_and_AddtoDatagrid();
                    

                }
                else if (_ossiaOS.Name == "No" && RecordingTimePerDayBox.Text != "" && ChannelNumberBox.Text != "" && BitRateBox.Text != "")
                {
                   
                    // (G49 * I49 * 3600 / 8 / 1024))* (Q49*T49/1024) if answer in gigabyte
                    // /1024 if answer in terabyte
                    double x = (double)((double)int.Parse(BitRateBox.Text) * (double)int.Parse(ChannelNumberBox.Text)
                                                                           * 3600 / 8 / 1024) * (double)int.Parse(RecordingTimePerDayBox.Text) * (double)int.Parse(RequiredRecordingTimeBox.Text) / 1024 / 1024;

                    _capacityInTB = Math.Round(x, 4);// answer in terabyte
                    help_calculation_and_AddtoDatagrid();
                   

                }
               
            }   
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }
            OssiaOSBox.Text = "Yes";
        }
       
        private void help_calculation_and_AddtoDatagrid()
        {
            try
            {
                datagridData.Add(new DatagridData()
                {
                    Id = _datagridDataIndex,
                    BitRate = int.Parse(BitRateBox.Text),
                    Channels = int.Parse(ChannelNumberBox.Text),
                    substream = int.Parse(SubStreamBitRateBox.Text), //might be null
                    hours = int.Parse(RecordingTimePerDayBox.Text),
                    Days = int.Parse(RequiredRecordingTimeBox.Text),
                    Capacity = _capacityInTB
                });
            }
            catch(FormatException ex)
            {
                datagridData.Add(new DatagridData()
                {
                    Id = _datagridDataIndex,
                    BitRate = int.Parse(BitRateBox.Text),
                    Channels = int.Parse(ChannelNumberBox.Text),
                    substream = 0, 
                    hours = int.Parse(RecordingTimePerDayBox.Text),
                    Days = int.Parse(RequiredRecordingTimeBox.Text),
                    Capacity = _capacityInTB
                });
                
            }

            
            _datagridDataIndex++;
            dataGridCapacityTB.ItemsSource=datagridData;   
             _totalcapacity += _capacityInTB;
            TotalCapacity_textbox.Text = _totalcapacity.ToString();

            if (dataGridCapacityTB.Items.Count > 0)
            {
                var border = VisualTreeHelper.GetChild(dataGridCapacityTB, 0) as Decorator;
                if (border != null)
                {
                    var scroll = border.Child as ScrollViewer;
                    if (scroll != null) scroll.ScrollToEnd();
                }
            }




            Camerasbox.Text = "";
            Resolutionbox.Text = "";
            EncodingBox.Text = "";
            BitRateBox.Text = "";
            ChannelNumberBox.Text = "";
            SubStreamBitRateBox.Text = "";
            RecordingTimePerDayBox.Text = "";
            RequiredRecordingTimeBox.Text = "0";



        }
        public int NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                RequiredRecordingTimeBox.Text = value.ToString();
            }
        }

       

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
            NumValue++;
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
            if (NumValue == 0)
                return;
            NumValue--;
        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            RequiredRecordingTimeBox.TextChanged += (s, ee) =>
            {
                var textbox = s as TextBox;
                int value;
                if (int.TryParse(textbox.Text, out value))
                {
                    if (value > 2000)
                        textbox.Text = "2000";
                    else if (value < 0)
                        textbox.Text = "0";
                }
            };
            if (RequiredRecordingTimeBox == null)
            {
                return;
            }

            if (!int.TryParse(RequiredRecordingTimeBox.Text, out _numValue))
                RequiredRecordingTimeBox.Text = _numValue.ToString();
        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //thinking of some sort of delay timer sleep
                DatagridData row = (DatagridData)dataGridCapacityTB.SelectedItems[0];
                ObservableCollection<DatagridData> data = (ObservableCollection<DatagridData>)dataGridCapacityTB.ItemsSource;
                data.Remove(row);
               
                    double tmp = Math.Round(row.Capacity, 4);
                _totalcapacity -= tmp;
                _totalcapacity= Math.Round(_totalcapacity, 4);
                TotalCapacity_textbox.Text = _totalcapacity.ToString();
             //   dataGridCapacityTB.Items.RemoveAt(dataGridCapacityTB.SelectedIndex);
              
                
            }
            catch (InvalidCastException ex)
            {
                ;
            }
        }

        private void Clear_button_Click(object sender, RoutedEventArgs e)
        {
            HDDUsageWindow mw = new HDDUsageWindow(); //Think of changing to real clear instead of new window
            mw.Show();
            this.Close();
        }
    }
}
