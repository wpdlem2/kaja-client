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
    public partial class AddMember : Form
    {
        DBClass dbc = new DBClass();
        int addYN = 0; // // 등록완료시 1로 변경하여 Member로 넘겨 뷰 리프레쉬 ( 0일 경우 리프레쉬 안함 )

        public AddMember()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        // Member로 addYN 변수를 넘겨주기 위한 프로퍼티
        public int AddYN
        {
            get { return addYN; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("회원명과 전화번호는 공백일 수 없습니다.", "알림");
            }
            else if (textBox2.Text.Length == 11 || textBox2.Text.Length == 13)
            {
                DialogResult ok = MessageBox.Show("회원 등록을 완료 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (ok == DialogResult.Yes)
                {
                    try
                    {
                        dbc.MDB_Open();
                        dbc.DS.Clear();
                        dbc.DBAdapter.Fill(dbc.DS, "member");
                        dbc.MemberTable = dbc.DS.Tables["member"];
                        DataRow newRow = dbc.MemberTable.NewRow();
                        newRow["mem_id"] = dbc.MemberTable.Rows.Count + 1;
                        newRow["mem_name"] = textBox1.Text;

                        // 전화번호 형식에 " - " 를 빼고 입력했을 경우 추가해서 DB등록
                        if (textBox2.Text.Length == 13)
                        {
                            if (textBox2.Text.Substring(3, 1) == "-" && textBox2.Text.Substring(8, 1) == "-")
                            {
                                newRow["mem_phone"] = textBox2.Text;
                            }
                            else
                            {
                                MessageBox.Show("전화번호의 형식이 잘못되었습니다.", "알림");
                                return;
                            }
                        }
                        else if (textBox2.Text.Length == 11)
                        {
                            string phone = textBox2.Text.Substring(0, 3) + "-" + textBox2.Text.Substring(3, 4) + "-" + textBox2.Text.Substring(7, 4);
                            newRow["mem_phone"] = phone;
                        }
                        newRow["mem_address"] = textBox3.Text;
                        dbc.MemberTable.Rows.Add(newRow);
                        dbc.DBAdapter.Update(dbc.DS, "member");
                        dbc.DS.AcceptChanges();

                        addYN = 1;  // 등록 완료 신호 (Member로 넘겨짐)

                        Dispose();
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
            else
            {
                MessageBox.Show("전화번호의 형식이 잘못되었습니다.", "알림");
            }
        }
    }
}
