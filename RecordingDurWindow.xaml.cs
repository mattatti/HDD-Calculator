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
            private ChannelNumber _channelNumber;
            private OssiaOS _ossiaOS;
            private SubSteamBitRate _subSteamBitRate;
            //private RecordingTimePerDay _recordingTimePerDay;
            private ObservableCollection<DatagridData> datagridData = new ObservableCollection<DatagridData>();
            private AvailableHDD _availableHDD;
            private readonly int _cameraColumn = 0;
            private readonly int _resolutionColumn = 1;
            private readonly int _encodingtypeColumn = 2;
            private readonly int _bitrateColumn = 3;
            private readonly int _maxChannelNumber = 128;
            private readonly int _maxHoursInADay = 24;
           // private int _numValue = 0;
            private int _datagridDataIndex = 1;
            private double _capacityInTB;
            private double _totalcapacity = 0;
            private double _recordDuration = 0;
            private List<int> _h264list = new List<int>()
             {
               64,128,256,384,512,768,1024,1536,2048,2304,2560,3072,4096,5120,6144,7168,8192,10240,12536
             };
            private List<int> _h265list = new List<int>()
             {
               64,128,256,384,512,768,1024,1536,2048,2304,2560,3072,4096,5120,6144,7168
             };
            private List<int> _substreamlist = new List<int>()
             {
               64,128,256,384,512,768,1024,1536,2048,2304,2560,3072,4096
             };
            private List<OssiaOS> _oss_OS = new List<OssiaOS>();
            private List<AvailableHDD> _hddStorageList = new List<AvailableHDD>();
            private bool _calcflag=false;
      


            public RecordingDurWindow()
            {
                InitializeComponent();

                Camerasbox.ItemsSource = StringToCameraNameList(GlobalVariables.getInstance()._databaseSize, _cameraColumn);
                Camerasbox.DisplayMemberPath = "Name";

                Resolutionbox.IsHitTestVisible = false;
                EncodingBox.IsHitTestVisible = false;
                BitRateBox.IsHitTestVisible = false;

                ChannelNumberBox.ItemsSource = InitChannelNumbers();
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

            //AvailableHDDBlock.Text = _numValue.ToString();
            // SubStreamBitRateBox.IsHitTestVisible = false;
            //  this.DataContext = datagridData;
                AvailableHDDBox.ItemsSource= InitAvailableHDD();
            AvailableHDDBox.DisplayMemberPath = "Value";
            _availableHDD  = new AvailableHDD() { Value = 0 };
        }

            protected override void OnSourceInitialized(EventArgs e)
            {
                IconHelper.RemoveIcon(this);
            }


        private List<AvailableHDD> InitAvailableHDD()
        {
            _hddStorageList.Add(new AvailableHDD() { Value = 0.5 });
            int amountofStorage=1;
            for (int i = 0; i < 6; i++)
            {
                _hddStorageList.Add(new AvailableHDD() { Value = amountofStorage });
                amountofStorage++;
            }
          
            _hddStorageList.Add(new AvailableHDD() { Value = 8 });
            _hddStorageList.Add(new AvailableHDD() { Value = 16 });
            _hddStorageList.Add(new AvailableHDD() { Value = 18 });
            amountofStorage = 24;
            for (int i = 0; i < 13; i++)
            {
                _hddStorageList.Add(new AvailableHDD() { Value = amountofStorage });
                amountofStorage = amountofStorage +2;
            }
            _hddStorageList.Add(new AvailableHDD() { Value = 64 });
            _hddStorageList.Add(new AvailableHDD() { Value = 128 });
            //  {
            //     ,1,2,3,4,5,6,8,16,18,24,26,28,30,32,34,36,38,40,42,44,46,48,64,128
            // };
            return _hddStorageList;
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

                OssiaOS os1 = new OssiaOS();
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
                _subSteamBitRate = SubStreamBitRateBox.SelectedItem as SubSteamBitRate;
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
                              * 3600 / 8 / 1024) * (double)int.Parse(RecordingTimePerDayBox.Text)  / 1024 / 1024;
                        //SubStreamBitRateBox might be null
                        _capacityInTB = Math.Round(x, 4);// answer in terabyte
                        help_calculation_and_AddtoDatagrid();


                    }
                    else if (_ossiaOS.Name == "No" && RecordingTimePerDayBox.Text != "" && ChannelNumberBox.Text != "" && BitRateBox.Text != "")
                    {

                        // (G49 * I49 * 3600 / 8 / 1024))* (Q49*T49/1024) if answer in gigabyte
                        // /1024 if answer in terabyte
                        double x = (double)((double)int.Parse(BitRateBox.Text) * (double)int.Parse(ChannelNumberBox.Text)
                                                                               * 3600 / 8 / 1024) *  (double)int.Parse(RecordingTimePerDayBox.Text)  / 1024 /1024 ;

                    _capacityInTB = x;
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
                        Capacity = Math.Round(_capacityInTB, 4)
               
                    });
                }
                catch (FormatException ex)
                {
                    datagridData.Add(new DatagridData()
                    {
                        Id = _datagridDataIndex,
                        BitRate = int.Parse(BitRateBox.Text),
                        Channels = int.Parse(ChannelNumberBox.Text),
                        substream = 0,
                        hours = int.Parse(RecordingTimePerDayBox.Text),

                        Capacity = Math.Round(_capacityInTB, 4)
                    });
                }


                _datagridDataIndex++;
                dataGridCapacityTB.ItemsSource = datagridData;
                _totalcapacity += _capacityInTB;
            if (_availableHDD.Value != 0)
            {
                _recordDuration = _availableHDD.Value / _totalcapacity;              // all that needs to also be in calc

                _recordDuration = Math.Round(_recordDuration, 4);// answer in terabyte

                RecordingDays_textbox.Text = _recordDuration.ToString();
            }
            _calcflag = true;


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
                RecordingTimePerDayBox.Text = "";
            

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
                    _totalcapacity = Math.Round(_totalcapacity, 4);
                    //TotalCapacity_textbox.Text = _totalcapacity.ToString();
                    //   dataGridCapacityTB.Items.RemoveAt(dataGridCapacityTB.SelectedIndex);


                }
                catch (InvalidCastException ex)
                {
                    ;
                }
            }

            private void Clear_button_Click(object sender, RoutedEventArgs e)
            {
            RecordingDurWindow mw = new RecordingDurWindow(); //Think of changing to real clear instead of new window
            mw.Show();
            this.Close();
            }

        private void AvailableHDDBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _availableHDD = AvailableHDDBox.SelectedItem as AvailableHDD;
            if(_calcflag == true)
            {
            _recordDuration = _availableHDD.Value / _totalcapacity;              // all that needs to also be in calc

            _recordDuration = Math.Round(_recordDuration, 4);// answer in terabyte

            RecordingDays_textbox.Text = _recordDuration.ToString();

            }
        }
    }
    }


