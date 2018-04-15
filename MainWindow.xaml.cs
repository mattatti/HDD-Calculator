﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;



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
        private BitRate _bitRate;
        private ObservableCollection<EncodingType> _enc;
        private AdminLoginWindow _adminloginwin;
        private AddCameraWindow _addCameraWindow;
        private string[,] _database;
        private int _databaseSize;
        private readonly int _cameraColumn = 0;
        private readonly int _resolutionColumn = 1;
        private readonly int _encodingtypeColumn = 2;
        private readonly int _bitrateColumn = 3;

        public MainWindow()
        {
            InitializeComponent();
            _databaseSize =  Load_Excel_File();
            
            //fill CameraName with all the different camera names from listcm
              
              int i = 0;
            
            Camerasbox.ItemsSource = StringToCameraNameList(_databaseSize, _cameraColumn);
            Camerasbox.DisplayMemberPath = "Name";
          
            Resolutionbox.IsHitTestVisible = false;
            EncodingBox.IsHitTestVisible = false;

            BitRateBox.ItemsSource = StringToBitRateList(_databaseSize, _bitrateColumn);
            BitRateBox.DisplayMemberPath = "Value";
            this.Closed += new EventHandler(MainWindow_Closed);
           

        }
        private List<BitRate> StringToBitRateList(int _databaseSize, int ColumnNumber)
        {
            List<BitRate> brate = new List<BitRate>();
            List<string> temp = new List<string>();
            for (var i = 0; i < _databaseSize; i++)
            {
                temp.Add(_database[i, ColumnNumber]);
            }
            temp = temp.Distinct().ToList();
            
            for (var i = 0; i < temp.Count; i++)
            {
                BitRate br = new BitRate();
                br.Value =Int32.Parse( temp[i]);
                brate.Add(br);
            }
            brate = brate.OrderBy(x => x.Value).ToList();
            return brate;
        }
        private List<CameraName> StringToCameraNameList(int _databaseSize, int ColumnNumber)
        {
            List < CameraName > cam= new List<CameraName>();
            List<string> temp = new List<string>();
            for (var i = 0; i < _databaseSize; i++)
            { 
                temp.Add(_database[i, ColumnNumber]);
            }
            temp =temp.Distinct().ToList();
            for (var i = 0; i < temp.Count; i++)
            {
                CameraName cn = new CameraName();
                cn.Name =  temp[i];
                cam.Add(cn);
            }
            cam = cam.OrderBy(x => x.Name).ToList();
            return cam;
        }

        private int Load_Excel_File()
        {
            string filepath = System.Environment.CurrentDirectory + "\\HDDCalcDatabase.xlsx";

            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            //Static File From Base Path...........
            //Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "TestExcel.xlsx", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            //Dynamic File Using Uploader...........
            Microsoft.Office.Interop.Excel.Workbook excelBook = excelApp.Workbooks.Open(filepath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel.Worksheet excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets.get_Item(1); ;
            Microsoft.Office.Interop.Excel.Range excelRange = excelSheet.UsedRange;
            _database = new string[excelRange.Rows.Count - 1, excelRange.Columns.Count];
            _databaseSize = excelRange.Rows.Count - 1;
            int rowCnt = 0;

            for (rowCnt = 2; rowCnt <= excelRange.Rows.Count; rowCnt++)
            {
                string strData = "";
               
                for (var colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                {
                    try
                    {
                        _database[rowCnt - 2, colCnt - 1] = (string)(excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range)?.Value2;
                    }
                    catch (Exception ex)
                    {
                        var value2 = (excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range)?.Value2;
                        if (value2 != null)
                        {
                            var douCellData = (int)value2;
                            _database[rowCnt - 2, colCnt - 1] = douCellData.ToString();
                        }
                    }
                }
            }
            excelBook.Close(true, null, null);
            excelApp.Quit();

            return _databaseSize;
        }

        private void MainWindow_Closed(object sender, EventArgs args)
        {
            _adminloginwin.Close();
            _addCameraWindow.Close();
            this.Close();
        }

        private void Camerasbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EncodingBox.ItemsSource = "";
            EncodingBox.IsHitTestVisible = false;
            _enc?.Clear();
            this.OptimalBitrateTextBlock.Text = String.Empty;

            List<Resolution> res;
            _cam = Camerasbox.SelectedItem as CameraName;
            if (_cam == null) return;
            res = new List<Resolution>();
       
            List<string> temp = new List<string>();
            for (var index = 0; index < _databaseSize; index++)
            {
                if (_cam.Name == _database[index, _cameraColumn])
                {
                    temp.Add(_database[index, _resolutionColumn]);
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
            for (var index = 0; index < _databaseSize; index++)
            {
                if (_cam.Name == _database[index, _cameraColumn]  && _resb.Name == _database[index, _resolutionColumn])
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
            for (var index = 0; index < _databaseSize; index++)
            {
                if (_cam.Name == _database[index, _cameraColumn] && _resb.Name == _database[index, _resolutionColumn] && _encb.Name == _database[index, _encodingtypeColumn])
                {
                    OptimalBitrateTextBlock.Text = _database[index, _bitrateColumn] + " Kbps";
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

        private List<CameraName> SetCameraNames(List<Camera> cams)
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
    }
}
