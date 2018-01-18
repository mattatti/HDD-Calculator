using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace HDD_Calculator
{
    /// <summary>
    /// Interaction logic for AdminLoginWindow.xaml
    /// </summary>
    public partial class AdminLoginWindow : Window
    {
        public event EventHandler SubmitClicked;
        public string UserName { get; private set; }

        public AdminLoginWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;

            //MoveFocus gives focus to the next textbox or whatever comes up next
            Loaded += (sender, e) =>
                MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));//
        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
       
        private void Login_button_Click(object sender, RoutedEventArgs e)
        {
            if (Usernamebox.Text.Trim() == "admin" && Passwordbox.Password == "123456")
            {
                BadCredentials.Visibility = Visibility.Hidden;
                if (SubmitClicked != null)
                {
                    UserName = Usernamebox.Text;
                    TheEnclosingMethod();
                    SubmitClicked?.Invoke(this, new EventArgs());
                }
            }
            else
            {
                BadCredentials.Text = "Bad Credentials, try again.";
            }
        }
        private async void TheEnclosingMethod()
        {
            await Task.Delay(200);
            this.Close();
        }


        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Login_button_Click(sender,e);
            }
        }

    }
   
}
