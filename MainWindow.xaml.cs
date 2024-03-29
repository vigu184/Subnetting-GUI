﻿using System;
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
        private void GotFocusReset(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            FirstIP.ClearValue(Label.ContentProperty);
            LastIP.ClearValue(Label.ContentProperty);
            SubnetMask.ClearValue(Label.ContentProperty);
            nHosts.ClearValue(Label.ContentProperty);

            textBox.GotFocus -= GotFocusReset;


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
                SubnetMask.Content=S.SetSubnetMask();
                int sno;
                if (!int.TryParse(SubnetNo.Text, out sno) || S.TotalSubnets > sno)
                {
                    FirstIP.Content =S.GetSubnetFirstIP(sno-1);
                    LastIP.Content = S.GetSubnetLastIP(sno-1);
                    //SubnetMask.Content=S.
                    nHosts.Content = S.Host;
                }
            }
        }

        private void NextP_Click3(object sender, RoutedEventArgs e)
        {
            if (FirstIP.Content != null)
            {
                P3.Visibility = Visibility.Hidden;
                P4.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Forgot Go","Warning",MessageBoxButton.OK,MessageBoxImage.Warning);
                Run3_Click(sender, e);
            }
        }

        private void BackP_Click3(object sender, RoutedEventArgs e)
        {
            P3.Visibility = Visibility.Hidden;
            P2.Visibility = Visibility.Visible;
        }

        private void BackP_Click4(object sender, RoutedEventArgs e)
        {
            P4.Visibility = Visibility.Hidden;
            P3.Visibility = Visibility.Visible;
            SubnetNo.GotFocus += GotFocusReset;
        }
    }
}
