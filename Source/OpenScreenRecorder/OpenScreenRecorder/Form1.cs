using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SharpAvi;
using SharpAvi.Codecs;
using SharpAvi.Output;

namespace OpenScreenRecorder
{
    public partial class Form1 : Form
    {


        const string ver = "0.1";
#if DEBUG
        const string version = "v" + ver + "-DEBUG";
#else
        const string version = "v" + ver;
#endif

        RecordingPopup recordingPopup;
        Recorder rec;
        bool infocus;
        bool sidePanelOpen;
        bool recording = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sidePanelOpen = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;
            this.Text = String.Empty;
            label3.Text = version;
            recordingPopup = new RecordingPopup();
            
            recordingPopup.ShowInTaskbar = false;
        }

        private void updateTick_Tick(object sender, EventArgs e)
        {
            bool temp_mof = ClientRectangle.Contains(
      Form.MousePosition.X - Location.X,
      Form.MousePosition.Y - Location.Y) && Form.MousePosition.Y - Location.Y > 35;
            if (Cursor.Position.X > (this.Width - 30) + this.Location.X && temp_mof)
                sidePanelOpen = true;
            if (Cursor.Position.X < (this.Width - sidePanel.Width - 20) + this.Location.X)
                sidePanelOpen = false;
            if (!temp_mof)
                sidePanelOpen = false;


            if (sidePanelOpen)
            {
                sidePanel.Size = new Size((int)Lerp.lerp(sidePanel.Size.Width, 512, 0.3f), sidePanel.Height); 
            }
            else
            {
                sidePanel.Size = new Size((int)Lerp.lerp(sidePanel.Size.Width, 15, 0.3f), sidePanel.Height);
            }
            sidebarTab.Location = new Point(this.Width - sidePanel.Width - sidebarTab.Width, 50);
        }

        private void sidePanelBorder_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            infocus = false;
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            infocus = true;
        }



        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void metroToggle1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void minButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void sidePanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void RecordButton_Click(object sender, EventArgs e)
        {
            if (!recording)
            {
                if (textBox1.Text == String.Empty)
                {
                    MessageBox.Show("You must enter a path!", "Text box blank!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (textBox2.Text == String.Empty)
                {
                    MessageBox.Show("You must enter a name!", "Text box blank!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    rec = new Recorder(new RecorderParams(textBox1.Text + textBox2.Text + ".avi", trackBar1.Value, SharpAvi.KnownFourCCs.Codecs.MotionJpeg, trackBar2.Value));
                    recording = true;
                    recordingPopup.Show();
                }
                
            }

            else
            {
                recording = false;
                rec.Dispose();
                recordingPopup.Hide();
            }
                
        }

        private void SnapButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == String.Empty)
            {
                MessageBox.Show("You must enter a path!", "Text box blank!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox2.Text == String.Empty)
            {
                MessageBox.Show("You must enter a name!", "Text box blank!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Bitmap captureBitmap = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);

                Rectangle captureRectangle = Screen.AllScreens[0].Bounds;
                Graphics captureGraphics = Graphics.FromImage(captureBitmap);
                captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
                captureBitmap.Save(textBox1.Text + textBox2.Text + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private void ScreencapTimer_Tick(object sender, EventArgs e)
        {
            Bitmap captureBitmap = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
            
            Rectangle captureRectangle = Screen.AllScreens[0].Bounds;
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);
            captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
            if(!(PreviewBox.Image == null))
                PreviewBox.Image.Dispose();
            PreviewBox.Image = captureBitmap;
            GC.Collect();

            GC.WaitForPendingFinalizers();


        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            ScreencapTimer.Interval = 1000 / trackBar3.Value;
        }
    }
}
