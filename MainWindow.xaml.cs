using System;
using System.Collections.Generic;
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

namespace HDD_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HDDUsageWindow _hddUsageWindow;
        private RecordingDurWindow _recordingDurWindow;
      
        public MainWindow()
        {
            InitializeComponent();
            if (GlobalVariables.getInstance().initflag == true)
            {
                Load_Excel_File();
                GlobalVariables.getInstance().initflag = false;
            }
        }

        private void Load_Excel_File()
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            //Static File From Base Path...........
            Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "HDDCalcDatabase.xlsx", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0).Worksheets.get_Item(1);

            Microsoft.Office.Interop.Excel.Range excelRange = excelSheet.UsedRange;
            GlobalVariables.getInstance()._database = new string[excelRange.Rows.Count - 1, excelRange.Columns.Count];
            GlobalVariables.getInstance()._databaseSize = excelRange.Rows.Count - 1;
            int rowCnt = 0;

            for (rowCnt = 2; rowCnt <= excelRange.Rows.Count; rowCnt++)
            {
                string strData = "";

                for (var colCnt = 1; colCnt <= excelRange.Columns.Count; colCnt++)
                {
                    try
                    {
                        GlobalVariables.getInstance()._database[rowCnt - 2, colCnt - 1] = (string)(excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range)?.Value2;
                    }
                    catch (Exception ex)
                    {
                        var value2 = (excelRange.Cells[rowCnt, colCnt] as Microsoft.Office.Interop.Excel.Range)?.Value2;
                        if (value2 != null)
                        {
                            var douCellData = (int)value2;
                            GlobalVariables.getInstance()._database[rowCnt - 2, colCnt - 1] = douCellData.ToString();
                        }
                    }
                }
            }
            //excelBook.Close(true, null, null);
            excelApp.Quit();
            
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }
        private void Recording_Duration_button_Click(object sender, RoutedEventArgs e)
        {
            _recordingDurWindow = new RecordingDurWindow();
            //_recordingDurWindow.SubmitClicked += adminloginwin_SubmitClicked;
            _recordingDurWindow.Show();
            //TODO:
            //close main window
            this.Close();
        }

        private void HDDUsage_button_Click(object sender, RoutedEventArgs e)
        {
            _hddUsageWindow = new HDDUsageWindow();
           // _hddUsageWindow.SubmitClicked += adminloginwin_SubmitClicked;
            _hddUsageWindow.Show();
            //TODO:
            //close main window
       //     App.Current.MainWindow.Close();
            this.Close();
        }
    }
}
