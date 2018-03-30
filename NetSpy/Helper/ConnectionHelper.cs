using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetSpy.Helper
{
    public class ConnectionHelper
    {
        //INFO: NOW JUST IN 127.0.0.1 FOR TESTING
        //INFORMAZIONI DEL MRE
        public const int MrePort = 8080;
        public const string MreIp = "127.0.0.1";
        private const int Buffer = 2048;

        //INFORMAZIONI DEL CLIENT A CUI DEVE INVIARE I DATI
        private IPAddress victimIp;
        public IPAddress VictimIp
        {
            get { return victimIp; }
            set
            {
                if(value != null)
                    victimIp = value;
            }
        }

        private int victimPort;
        public int VictimPort
        {
            get { return victimPort; }
            set
            {
                if(!String.IsNullOrWhiteSpace(value.ToString()) || value >= 0 || value <= 65535)
                    victimPort = value;
            }
        }

        private UdpClient MREServer;
        private UdpClient MREClient;
        private Thread listen;

        //COSTRUTTORE
        public ConnectionHelper()
        {
            MREClient = new UdpClient(MrePort);
            MREClient = new UdpClient();
        }

        private void StartServer()
        {
            Console.WriteLine("Server creato.");
            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, MrePort);
                var data = MREServer.Receive(ref remoteEP);
                byte[] recivedData = data.ToArray<byte>();
            }
        }

        public void StartAsyncListening()
        {
            listen = new Thread(StartServer);
            listen.Start();
        }

        public void StopAsyncListening()
        {
            listen.Abort();
        }

        public void SendData(byte[] data)
        {
            MREClient.Connect(IPAddress.Parse(MreIp), MrePort);

            byte[][] packets = PreparePackets(data);
            SendAllPackets(packets);
        }



        private byte[][] PreparePackets(byte[] data)
        {
            float packetsFloat = data.Length / (float) Buffer;
            int packetsNumber = 0;

            if (NumbersHelper.IsFloat(packetsFloat))
            {
                packetsNumber = (int)packetsFloat + 1;
            }
            else
            {
                packetsNumber = (int)packetsFloat;
            }
            Console.WriteLine("Packets needed: " + packetsNumber);
            byte[][] packets = new byte[packetsNumber][];

            if (data.Length < Buffer)
            {
                packets[0] = data;
                return packets;
            }
            else
            {
                int dataPos = 0;
                for (int i = 0; i < packets.Length; i++)
                {
                    int remaningBytes = data.Length - dataPos;
                    byte[] packet;
                    if (remaningBytes < Buffer)
                    {
                        packet = new byte[remaningBytes];
                    }
                    else
                    {
                        packet = new byte[Buffer];
                    }

                    Console.WriteLine("Building packet n." + i + "------------------------------");
                    //BUILD PACKET
                    for (int j = 0; j < Buffer; j++)
                    {
                        if (dataPos < data.Length)
                        {
                            Console.WriteLine("Cell: " + j + " - " + dataPos);
                            packet[j] = data[dataPos];
                            dataPos++;
                        }
                        else
                        {
                            Console.WriteLine("FINISHED");
                            break;
                        }
                    }

                    packets[i] = packet;
                }

                return packets;
            }
        }

        private void SendAllPackets(byte[][] packets)
        {
            for (int i = 0; i < packets.Length; i++)
            {
                MREClient.Send(packets[i], packets[i].Length);
            }
        }









        private void PrintMatrix(byte[][] matrix)
        {
            Console.Write("[" + Environment.NewLine);

            for (int i = 0; i < matrix.Length; i++)
            {
                Console.Write(i + ") ");
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    Console.Write(matrix[i][j] + " ");
                }
                Console.WriteLine("");
            }


            Console.Write("]" + Environment.NewLine);
        }

        public byte[] BuildRandomPackets(int length)
        {
            byte[] data = new byte[length];
            FillRandomPacket(data);
            return data;
        }


        private void FillRandomPacket(byte[] packet)
        {
            for (int i = 0; i < packet.Length; i++)
            {
                packet[i] = (byte)NumbersHelper.GenerateNumber(0, 100);
            }
        }

        public void TryPreparePackets()
        {
            byte[][] matrix = PreparePackets(BuildRandomPackets(300 * 100));
            PrintMatrix(matrix);
        }
    }
}
