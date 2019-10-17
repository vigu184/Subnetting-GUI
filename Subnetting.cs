using System;
using System.Collections.Generic;
using System.Text;

namespace Subnetting_GUI
{
    class Subnetting
    {
        string IpAddress;
        byte[] SubnetMaskOctet;
        byte[] DefaultMaskOctet;
        internal int TotalSubnets;
        public int TotalHosts { get; private set; }
        public int Host { get; private set; }// No. of hosts per subnet 


        //Function stores the network details fro the IP passed as arguments
        public void GetNetWorkDetails(IP ip)
        {
            TotalHosts = 1;
            DefaultMaskOctet = new byte[4];
            IpAddress = ip.IpAddress;
            SubnetMaskOctet = ip.SubnetOctet;
            SubnetMaskOctet.CopyTo(DefaultMaskOctet, 0);
            for (int i = 0; i < 4; i++)//Calculate total hosts in the network
            {
                if (DefaultMaskOctet[i] != 255)
                {
                    TotalHosts *= Convert.ToInt32((byte)~DefaultMaskOctet[i]) + 1;
                }
            }
        }

        public bool SetNumSubnets(string noS)
        {
            int TnoS;
            if (!int.TryParse(noS,out TnoS) || TnoS > TotalHosts)
                return false;
            TotalSubnets = TnoS;
            return true;
        }

        //Function asks for the Number of subnets and ,rounding of to the nearest power of 2, creates new subnet mask and subnets
        public string SetSubnetMask()
        {
            int i, j;
            Host = 1;
            
            int bit = (1 << Convert.ToString(TotalSubnets - 1, 2).Length) - 1;// Total bits used by subnet
            TotalSubnets = bit + 1;//Total subnets created
            string subnetbits = Convert.ToString(bit, 2);// Bits used in subnet converted to binary
            subnetbits = subnetbits.PadRight(((subnetbits.Length / 8) + 1) * 8, '0');// Adding padding to make size as byte/multiples of byte
            byte[] tempoctet = new byte[subnetbits.Length / 8];//Stores temporary Octet



            //Setting up of the new SubNet mask
            for (i = 0; i < subnetbits.Length / 8; i++)
            {
                tempoctet[i] = Convert.ToByte(subnetbits.Substring(i * 8, 8), 2);
            }

            for (i = 0, j = 0; i < 4 && j < tempoctet.Length; i++)
            {
                if (SubnetMaskOctet[i] != 255)
                {
                    SubnetMaskOctet[i] = tempoctet[j];
                    j++;
                }
            }
            int pt = 0;
            StringBuilder sb = new StringBuilder();
            foreach (byte Octet in SubnetMaskOctet)
            {
                sb.Append(Octet);
                if (pt < 3)
                {
                    sb.Append(".");
                }
                pt++;


            }
            //************************************************************************************
            //Console.WriteLine("Subnet Mask: {0}", sb.ToString());//Display the new Subnet Mask
            //Total No of hosts in the network/No of hosts per subnet 
            //************************************************************************************
            for (i = 0; i < 4; i++)
            {
                if (SubnetMaskOctet[i] != 255)
                {
                    Host *= Convert.ToInt32((byte)~SubnetMaskOctet[i]) + 1;
                }
            }

            Console.WriteLine("Hosts Per Subnet: {0}", Host);

            return sb.ToString();
        }

        public string GetSubnetFirstIP(int subnetno)
        {
            int i, j, subnetbytes = 0, subnetposition = 1;
            byte[] FipOctet = new byte[4];
            string[] IpOctetS;
            byte[] IpOctet = new byte[4];

            IpOctetS = IpAddress.Split('.');//Retrieving the octets from the ip address
            byte[] subnet = new byte[4];
            for (i = 0; i < 4; i++)
            {
                subnet[i] = (byte)(SubnetMaskOctet[i] ^ DefaultMaskOctet[i]);//Identifying the subnet value added to the default mask
            }
            for (i = 3; i >= 0; i--)//Identify the starting position of the first subnet bit and the span of the bits
            {

                if (subnet[i] != 0)
                {
                    subnetposition = i;
                    subnetbytes++;
                }
            }
            //Converting the subnet to binary
            string subnetbits = Convert.ToString(subnetno, 2).PadLeft(Convert.ToString(TotalSubnets, 2).Length - 1, '0').PadRight(subnetbytes * 8, '0');
            for (i = 0; i < 4; i++)
            {
                IpOctet[i] = Convert.ToByte(IpOctetS[i]);
                FipOctet[i] = Convert.ToByte((IpOctet[i] & DefaultMaskOctet[i]));
            }

            for (i = subnetposition, j = 0; i < subnetposition + subnetbytes && j < subnetbytes; i++, j++)
            {

                FipOctet[i] |= Convert.ToByte(subnetbits.Substring(j * 8, 8), 2);
            }
            StringBuilder sb = new StringBuilder();
            for (i = 0; i < 4; i++) 
            {
                sb.Append(FipOctet[i]);
                if (i != 3)
                    sb.Append('.');
            }
            
            return sb.ToString();


        }

        public string GetSubnetLastIP(int subnetno)
        {
            int i, j, subnetbytes = 0, subnetposition = 1;
            byte[] LipOctet = new byte[4];
            string[] IpOctetS = new string[4];
            byte[] IpOctet = new byte[4];
            byte[] subnet = new byte[4];
            for (i = 0; i < 4; i++)
            {
                subnet[i] = (byte)(SubnetMaskOctet[i] ^ DefaultMaskOctet[i]);
            }
            for (i = 3; i >= 0; i--)
            {

                if (subnet[i] != 0)
                {
                    subnetposition = i;
                    subnetbytes++;
                }
            }

            string subnetbits = Convert.ToString(subnetno, 2).PadLeft(Convert.ToString(TotalSubnets, 2).Length - 1, '0').PadRight(subnetbytes * 8, '0');
            IpOctetS = IpAddress.Split('.');

            for (i = 0; i < 4; i++)
            {
                IpOctet[i] = Convert.ToByte(IpOctetS[i]);
                IpOctet[i] = Convert.ToByte((IpOctet[i] & DefaultMaskOctet[i]));
                LipOctet[i] = 0;
            }

            int hostbitI = Host;
            i = 3;
            while (hostbitI > 0)
            {

                LipOctet[i] = (byte)(hostbitI % 256 - 1);
                hostbitI /= 256;

                i--;
            }

            for (i = 0; i < 4; i++)
            {
                LipOctet[i] = Convert.ToByte(IpOctet[i] | LipOctet[i]);
            }

            for (i = subnetposition, j = 0; j < subnetbytes; i++, j++)
            {

                LipOctet[i] |= Convert.ToByte(subnetbits.Substring(j * 8, 8), 2);
            }

            StringBuilder sb = new StringBuilder();
            for (i = 0; i < 4; i++)
            {
                sb.Append(LipOctet[i]);
                if (i!=3)
                    sb.Append('.');
            }

            return sb.ToString();


        }
    }
    }
