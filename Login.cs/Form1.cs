using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Login.cs
{
    public partial class Login : Form
    {
        DBClass dbc = new DBClass();
        string alarmTime;  //  알람을 울리기위해 현재 시간 변수
        string settingTime; //  알람이 울린는시간
        
        int show = 0;  //  알람 폼을 한번만 띄워주기 위한 카운트변수
        int scheduleCount;  // 금일자 스케줄의 갯수를 저장하기위한 변수


        public Login()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                ExcelCopy excelCopy = new ExcelCopy();
                excelCopy.button3_Click(sender, e);
                excelCopy.button2_Click(sender, e);
                Dispose();
            }
            else
            {
                Dispose();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (label6.Visible == true)
            {
                MessageBox.Show("확인하지 않은 일정이 존재합니다.");
            }
            View view = new View();
            this.Visible = false;
            view.ShowDialog();
            if (view.IsDisposed)
            {
                this.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (label6.Visible == true)
            {
                MessageBox.Show("확인하지 않은 일정이 존재합니다.");
            }
            Main main = new Main();
            this.Visible = false;
            main.ShowDialog();
            if (main.IsDisposed)
            {
                this.Visible = true;
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            dbc.Alarm_Open();
            dbc.AlarmTable = dbc.DS.Tables["alarm"];
            settingTime = dbc.AlarmTable.Rows[0]["time"].ToString();

            DataLabel.Text = DateTime.Now.ToString("yyyy-MM-dd");
            timer1.Start();

            dbc.SDB_Open();
            dbc.ScheduleTable = dbc.DS.Tables["schedule"];
            for (int i = 0; i < dbc.ScheduleTable.Rows.Count; i++)
            {
                DataRow currRow = dbc.ScheduleTable.Rows[i];
                if (currRow["sc_date"].ToString() == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    label6.Visible = true;
                    scheduleCount += 1;
                }
            }
            if(scheduleCount != 0)
            {
                label7.Text = "오늘의 일정\r\n[ " + scheduleCount.ToString() +" ]";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            DataLabel.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TimeLabel.Text = DateTime.Now.ToString("hh:mm:ss tt" ,new System.Globalization.CultureInfo("en-US"));
            alarmTime = DateTime.Now.ToString("HH:mm");
            if(alarmTime == settingTime)
            {
                if (show == 0)
                {
                    Alarm alarm = new Alarm();
                    alarm.Show();
                    show = 1;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label6.Visible = false;
            Schedule schedule = new Schedule();
            this.Visible = false;
            schedule.ShowDialog();
            if (schedule.IsDisposed)
            {
                this.Visible = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (label6.Visible == true)
            {
                MessageBox.Show("확인하지 않은 일정이 존재합니다.");
            }
            Member member = new Member();
            this.Visible = false;
            member.ShowDialog();
            if (member.IsDisposed)
            {
                this.Visible = true;
            }
        }

        // 엑셀파일 관리
        private void button6_Click(object sender, EventArgs e)
        {
            if (label6.Visible == true)
            {
                MessageBox.Show("확인하지 않은 일정이 존재합니다.");
            }
            ExcelCopy excelCopy = new ExcelCopy();
            this.Visible = false;
            excelCopy.ShowDialog();
            if (excelCopy.IsDisposed)
            {
                this.Visible = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AlarmTime alarmTime = new AlarmTime();
            alarmTime.ShowDialog();

            dbc.Alarm_Open();
            dbc.AlarmTable = dbc.DS.Tables["alarm"];
            settingTime = dbc.AlarmTable.Rows[0]["time"].ToString();
        }
    }
}
