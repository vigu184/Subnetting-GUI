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

namespace Subnetting_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IP iP;
        Subnetting S;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Ip_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Foreground = Brushes.Black;
            textBox.Clear();
            textBox.GotFocus -= Ip_GotFocus;


        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            iP = new IP();
            iP.IpAddress = Ip.Text;
            iP.GetIP();
            Boolean p = iP.IpAddress.Equals("Error");
            if (p)
            {
                Ip.Text = "Error";
                Ip.Foreground = Brushes.Red;
            }
            else
            {
                P1.Visibility = Visibility.Hidden;
                P2.Visibility = Visibility.Visible;
                char ipClass = iP.GetClass();
                string subnet = iP.ReturnSubnet();
                IPDMask.Content = subnet;
                if (ipClass != '0' && ipClass != 'l')
                    IPC.Content = ipClass.ToString().ToUpper();
                    //Console.WriteLine($"IP Class: {ipClass.ToString().ToUpper()}");
                if (ipClass == 'l')
                {
                    IPC.Content = "LocalHost Entered";//Console.WriteLine("LocalHost Entered");
                    return;
                }
                if (ipClass == 'a' || ipClass == 'b' || ipClass == 'c')
                {
                    String[] SubnetOctet = iP.SubnetToBinary();
                    IPDMaskBIN.Content = string.Join('.',SubnetOctet);
                }
            }
        }

        private void Run2_Click(object sender, RoutedEventArgs e)
        {
            if (iP.GetClass() == 'a' || iP.GetClass() == 'b' || iP.GetClass() == 'c')
            {
                P2.Visibility = Visibility.Hidden;
                P3.Visibility = Visibility.Visible;
                S = new Subnetting();
                S.GetNetWorkDetails(iP);

            }
        }

        private void Run3_Click(object sender, RoutedEventArgs e)
        {
            if (S.SetNumSubnets(NumSubnet.Text))
            {
                S.SetSubnetMask();
                int sno;
                if (!int.TryParse(SubnetNo.Text, out sno) || S.TotalSubnets > sno)
                {
                    FirstIP.Content =S.GetSubnetFirstIP(sno);
                    LastIP.Content = S.GetSubnetLastIP(sno);
                }
            }
        }
    }
}
