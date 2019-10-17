using System;
using System.Collections.Generic;
using System.Text;

namespace Subnetting_GUI
{
    class IP
    {
        public string IpAddress { get; set; }//Property returns iPAddress to other classes

        string[] IPOctet;//Array of all IP Octet
        int IPOctet0;//First IP Octet

        public byte[] SubnetOctet { get; private set; }//Auto property
        char IPClass;
        public string GetIP()//Gets the IP address from the user. Returns error if incorrect IP
        {
            Console.WriteLine("Entered IP Address");
            

            IPOctet = IpAddress.Split('.');
            if (IPOctet.Length != 4)
            {
                IpAddress = "Error";
                return "Error"; 
            }
            IPOctet0 = Convert.ToInt32(IPOctet[0]);
            if (IPOctet0.CompareTo(254) > 0 ||
                    Convert.ToInt32(IPOctet[1]).CompareTo(254) > 0 ||
                        Convert.ToInt32(IPOctet[2]).CompareTo(254) > 0 ||
                            Convert.ToInt32(IPOctet[3]).CompareTo(254) > 0)
            {
                IpAddress = "Error";
                return "Error";
            }
            return IpAddress;
        }
        public char GetClass()// Returns IP Class
        {
            if (IPOctet0.CompareTo(126) <= 0)
            {
                IPClass = 'A';
            }
            else if (IPOctet0.CompareTo(128) >= 0
                      && IPOctet0.CompareTo(191) <= 0)
            {
                IPClass = 'B';
            }
            else if (IPOctet0.CompareTo(192) >= 0
                      && IPOctet0.CompareTo(223) <= 0)
            {
                IPClass = 'C';
            }
            else if (IPOctet0.CompareTo(224) >= 0
                      && IPOctet0.CompareTo(239) <= 0)
            {
                IPClass = 'D';
            }
            else if (IPOctet0.CompareTo(240) >= 0
                      && IPOctet0.CompareTo(254) <= 0)
            {
                IPClass = 'E';
            }
            else if (IPOctet0.Equals(127))
            {
                IPClass = 'l';
            }
            else
                IPClass = '0';

            return IPClass;
        }
        public string ReturnSubnet()
        {
            SubnetOctet = new Byte[4];
            SubnetOctet[0] = 255;
            switch (IPClass)
            {
                case 'A':
                    SubnetOctet[1] = 0;
                    SubnetOctet[2] = 0;
                    SubnetOctet[3] = 0;
                    break;
                case 'B':
                    SubnetOctet[1] = 255;
                    SubnetOctet[2] = 0;
                    SubnetOctet[3] = 0;
                    break;
                case 'C':
                    SubnetOctet[1] = 255;
                    SubnetOctet[2] = 255;
                    SubnetOctet[3] = 0;
                    break;
                default:
                    return "Error";

            }

            StringBuilder sb = new StringBuilder();
            int pt = 0;
            foreach (byte i in SubnetOctet)
            {

                pt++;
                sb.Append(i);
                if (pt <= 3)
                {
                    sb.Append('.');
                }
            }
            return sb.ToString();
        }


        public string[] SubnetToBinary()
        {

            string[] SOctet = new string[4];
            for (int i = 0; i < 4; i++)
            {
                SOctet[i] = Convert.ToString(SubnetOctet[i], 2).PadLeft(8, '0');
            }
            return SOctet;
        }
    }
}
