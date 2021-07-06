using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Login.cs
{
    public partial class Alarm : Form
    {
        SoundPlayer sound = new SoundPlayer(Properties.Resources.alarm);

        public Alarm()
        {
            InitializeComponent();
        }

        private void Alarm_Load(object sender, EventArgs e)
        {
            TimeLabel.Text = DateTime.Now.ToString("hh:mm tt" ,new System.Globalization.CultureInfo("en-US"));
            sound.PlayLooping();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sound.Stop();
            Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeLabel.Text = DateTime.Now.ToString("hh:mm tt" ,new System.Globalization.CultureInfo("en-US"));
        }
    }
}
