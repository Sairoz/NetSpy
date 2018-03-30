using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetSpy.Model;
using NetSpy.Helper;

namespace NetSpy
{
    public partial class Connections : Form
    {
        public ConnectionHelper connectionHelper;
        public Random rnd;

        public Connections()
        {
            InitializeComponent();
            connectionHelper = new ConnectionHelper();
            connectionHelper.VictimIp = IPAddress.Parse("127.0.0.1");
            connectionHelper.VictimPort = 1236;

            rnd = new Random();
        }

        private void Connections_Load(object sender, EventArgs e)
        {
            connectionHelper.TryPreparePackets();
            connectionHelper.SendData(new byte[100]);
        }

        public void SendFakePacket(int length)
        {
            byte[] packet = new byte[length];
            foreach (var cell in packet)
            {
                packet[cell] = (byte) GenerateNumber(1, 100);
            }
        }

        private int GenerateNumber(int min, int max)
        {
            return rnd.Next(min, max);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connectionHelper.SendData(connectionHelper.BuildRandomPackets(100 * 100));
        }
    }
}
