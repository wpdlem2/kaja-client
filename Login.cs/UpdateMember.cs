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
    public partial class UpdateMember : Form
    {
        DBClass dbc = new DBClass();
        int updateYN = 0; // 수정완료시 1로 변경하여 Member로 넘겨 뷰 리프레쉬 ( 0일 경우 리프레쉬 안함 )

        public UpdateMember()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        // Member에서 회원정보를 받아오기 위한 프로퍼티
        public string Mem_id
        {
            set { textBox11.Text = value; }
        }
        public string Mem_name
        {
            set { textBox12.Text = value; }
        }
        public string Mem_phone
        {
            set { textBox13.Text = value; }
        }
        public string Mem_address
        {
            set { textBox14.Text = value; }
        }
        // Member로 updateYN 변수를 넘겨주기 위한 프로퍼티
        public int UpdateYN
        {
            get { return updateYN; }
        }

        // 종료 버튼
        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        // 수정 완료 버튼
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult ok = MessageBox.Show("정보 수정을 완료 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ok == DialogResult.Yes)
                {
                    dbc.MDB_Open();
                    dbc.DS.Clear();
                    dbc.DBAdapter.Fill(dbc.DS, "member");
                    dbc.MemberTable = dbc.DS.Tables["member"];

                    DataRow currRow = dbc.MemberTable.Rows[Convert.ToInt32(textBox11.Text) - 1];
                    currRow.BeginEdit();

                    currRow["mem_name"] = textBox21.Text;
                    currRow["mem_phone"] = textBox22.Text;
                    if(textBox23.Text == "주소 정보 없음")
                    {
                        currRow["mem_address"] = "";
                    }
                    else
                    {
                        currRow["mem_address"] = textBox23.Text;
                    }
                    currRow.EndEdit();

                    dbc.DBAdapter.Update(dbc.DS, "member");
                    dbc.DS.AcceptChanges();
                    updateYN = 1;  // 수정 완료 신호 (Member로 넘겨짐)

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

        // 폼로드 (수정전의 텍스트를 수정사항에 대입)
        private void UpdateMember_Load(object sender, EventArgs e)
        {
            textBox21.Text = textBox12.Text;
            textBox22.Text = textBox13.Text;
            textBox23.Text = textBox14.Text;
        }
    }
}
