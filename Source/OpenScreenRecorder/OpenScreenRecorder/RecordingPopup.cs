using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenScreenRecorder
{
    public partial class RecordingPopup : Form
    {
        public RecordingPopup()
        {
            InitializeComponent();
        }

        private void RecordingPopup_Load(object sender, EventArgs e)
        {
            TransparencyKey = Color.Lime;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TopMost = true;
            Location = new Point(0, 0);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
