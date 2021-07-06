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
    public partial class AddSchedule : Form
    {
        DBClass dbc = new DBClass();

        public AddSchedule()
        {
            InitializeComponent();
            this.ControlBox = false;
            dbc.SDB_Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                MessageBox.Show("제목을 입력해 주세요.");
            }
            else if(textBox2.Text == "")
            {
                MessageBox.Show("내용을 입력해 주세요.");
            }
            else
            {
                DialogResult ok = MessageBox.Show("상품 등록을 완료 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(ok == DialogResult.Yes)
                {
                    try
                    {
                        dbc.DS.Clear();
                        dbc.DBAdapter.Fill(dbc.DS, "schedule");
                        dbc.ScheduleTable = dbc.DS.Tables["schedule"];
                        DataRow newRow = dbc.ScheduleTable.NewRow();
                        newRow["sc_id"] = dbc.ScheduleTable.Rows.Count + 1;
                        newRow["sc_title"] = textBox1.Text;
                        newRow["sc_info"] = textBox2.Text;
                        newRow["sc_date"] = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                        if(checkBox1.Checked == true)
                        {
                            newRow["sc_check"] = 1;
                        }
                        else if(checkBox1.Checked == false)
                        {
                            newRow["sc_check"] = 2;
                        }
                        dbc.ScheduleTable.Rows.Add(newRow);
                        dbc.DBAdapter.Update(dbc.DS, "schedule");
                        dbc.DS.AcceptChanges();
                    }
                    catch (DataException DE)
                    {
                        MessageBox.Show(DE.Message);
                    }
                    catch (Exception DE)
                    {
                        MessageBox.Show(DE.Message);
                    }
                    Dispose();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
