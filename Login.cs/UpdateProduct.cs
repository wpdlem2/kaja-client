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
    public partial class UpdateProduct : Form
    {
        DataRow currRow;
        int updateYN;
        string nameChanged;

        new Main Parent;
        public UpdateProduct(Main Parent)
        {
            this.Parent = Parent;
        }
        private void P_ButtonClick()
        {
            Parent.getButton.PerformClick();  // 부모폼 버튼 클릭이벤트 발생
        }

        //  Main에서 선택항목을 가져오기 위한 프로퍼티
        public string Type
        {
            set { textBox1.Text = value; }
        }
        public string Name
        {
            set { textBox2.Text = value; }
        }
        public string Price
        {
            set { textBox3.Text = value; }
        }
        public string Id
        {
            set { textBox4.Text = value; }
        }
        public string Count
        {
            set { textBox9.Text  = value; }
        }
        public string NameChanged
        {
            get { return nameChanged ; }
        }

        // 변경된 상품정보 이름
        public string ChangedName
        {
            get { return (currRow["pro_name"]).ToString(); }
        }
        public int UpdateYN
        {
            get { return updateYN; }
        }

        DBClass dbc = new DBClass();

        public UpdateProduct()
        {
            InitializeComponent();
            this.ControlBox = false;
            dbc.PDB_Open();
        }

        // 취소
        private void button1_Click(object sender, EventArgs e)
        {
            updateYN = 0;
            Dispose();
        }


        // 항목설정 라디오버튼
        // 체크시 분류 텍스트박스 변경
        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                textBox5.Text = "와인";
            }
        }

        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                textBox5.Text = "위스키";
            }
        }

        private void radioButton3_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                textBox5.Text = "꼬냑";
            }
        }

        private void radioButton4_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                textBox5.Text = "데낄라";
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked == true)
            {
                textBox5.Text = "리큐르";
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked == true)
            {
                textBox5.Text = "진";
            }
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked == true)
            {
                textBox5.Text = "보드카";
            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked == true)
            {
                textBox5.Text = "중국";
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked == true)
            {
                textBox5.Text = "기타";
            }
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton10.Checked == true)
            {
                textBox5.Text = "맥주";
            }
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            Dispose();
        }

         // ProductMain에서 받아온 분류명의 라디오버튼 체크설정
        private void UpdateProduct_Load_1(object sender, EventArgs e)
        {
            Parent = (Main)Owner;

            switch (textBox1.Text)
            {
                case "와인":
                    radioButton1.Checked = true;
                    break;
                case "위스키":
                    radioButton2.Checked = true;
                    break;
                case "꼬냑":
                    radioButton3.Checked = true;
                    break;
                case "데낄라":
                    radioButton4.Checked = true;
                    break;
                case "리큐르":
                    radioButton5.Checked = true;
                    break;
                case "진":
                    radioButton6.Checked = true;
                    break;
                case "보드카":
                    radioButton7.Checked = true;
                    break;
                case "중국":
                    radioButton8.Checked = true;
                    break;
                case "기타":
                    radioButton9.Checked = true;
                    break;
                case "맥주":
                    radioButton10.Checked = true;
                    break;
            }
            // 수정 전의 텍스트를 수정사항에 대입
            textBox5.Text = textBox1.Text;
            textBox6.Text = textBox2.Text;
            textBox7.Text = textBox3.Text;
            comboBox1.Text = textBox9.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult ok = MessageBox.Show("정보 수정을 완료 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ok == DialogResult.Yes)
                {
                    dbc.DS.Clear();
                    dbc.DBAdapter.Fill(dbc.DS, "product1");
                    dbc.ProductTable = dbc.DS.Tables["product1"];
                
                    currRow = dbc.ProductTable.Rows[Convert.ToInt32(textBox4.Text)-1];

                    currRow.BeginEdit();
                    switch (textBox5.Text)
                    {
                        case "와인":
                            currRow["type"] = "1";
                            break;
                        case "위스키":
                            currRow["type"] = "2";
                            break;
                        case "꼬냑":
                            currRow["type"] = "3";
                            break;
                        case "데낄라":
                            currRow["type"] = "4";
                            break;
                        case "리큐르":
                            currRow["type"] = "5";
                            break;
                        case "진":
                            currRow["type"] = "6";
                            break;
                        case "보드카":
                            currRow["type"] = "7";
                            break;
                        case "중국":
                            currRow["type"] = "8";
                            break;
                        case "기타":
                            currRow["type"] = "9";
                            break;
                        case "맥주":
                            currRow["type"] = "10";
                            break;
                    }
                    currRow["pro_name"] = textBox6.Text;
                    nameChanged = textBox6.Text;
                    currRow["price"] = textBox7.Text;
                    currRow["count"] = comboBox1.Text;
                    currRow.EndEdit();

                    dbc.DBAdapter.Update(dbc.DS, "product1");
                    dbc.DS.AcceptChanges();
                    updateYN = 1;
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
            P_ButtonClick();
            Label pLabel = Parent.getLabel;
            pLabel.Text = "["+textBox6.Text+"]" + " 정보 수정 완료";
            Dispose();
        }
    }
}
