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
    public partial class AlarmTime : Form
    {
        DBClass dbc = new DBClass();
        string time;
        public AlarmTime()
        {
            InitializeComponent();
        }

        private void AlarmTime_Load(object sender, EventArgs e)
        {
            dbc.Alarm_Open();
            dbc.AlarmTable = dbc.DS.Tables["alarm"];
            time = dbc.AlarmTable.Rows[0]["time"].ToString();

            comboBox1.Text = time.Substring(0, 2);
            comboBox2.Text = time.Substring(3, 2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult ok = MessageBox.Show("시간 수정을 완료 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ok == DialogResult.Yes)
                {
                    dbc.Alarm_Open();
                    dbc.DS.Clear();
                    dbc.DBAdapter.Fill(dbc.DS, "alarm");
                    dbc.AlarmTable = dbc.DS.Tables["alarm"];

                    DataRow currRow = dbc.AlarmTable.Rows[0];
                    currRow.BeginEdit();
                    currRow["time"] = comboBox1.Text + ":" + comboBox2.Text;
                    currRow.EndEdit();

                    dbc.DBAdapter.Update(dbc.DS, "alarm");
                    dbc.DS.AcceptChanges();

                    Dispose();
                }

            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
            catch (Exception DE)
            {
                MessageBox.Show(DE.Message);
            }
        }
    }
}
