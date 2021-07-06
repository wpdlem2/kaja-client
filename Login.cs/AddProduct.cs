using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace Login.cs
{
    public partial class AddProduct : Form
    {
        DBClass dbc = new DBClass();

        string typeName;
        private int type = 0;
        public int Type  // ProductMain으로 추가한 상품의 타입을 넘겨서 DBGrid 리프레쉬
        {
            get { return type; }
        }
        public string TypeName  //  ProductMain으로 추가한 상품의 타입 이름을 넘김
        {
            get { return typeName; }
        }

        public AddProduct()
        {
            InitializeComponent();
            this.ControlBox = false;
            dbc.PDB_Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" )
            {
                MessageBox.Show("상품 정보에 공백이 있습니다.", "알림");
            }
            else if (textBox4.Text == "")  //  라디오버튼이 클릭되면 textBox4 텍스트로 설정되며 type null 방지
            {
                MessageBox.Show("좌측 상품 분류가 선택되지 않았습니다", "알림");
            }
            else
            {
                DialogResult ok = MessageBox.Show("상품 등록을 완료 하시겠습니까?", "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (ok == DialogResult.Yes)
                {
                    try
                    {
                        dbc.DS.Clear();
                        dbc.DBAdapter.Fill(dbc.DS, "product1");
                        dbc.ProductTable = dbc.DS.Tables["product1"];
                        DataRow newRow = dbc.ProductTable.NewRow();
                        newRow["id"] = dbc.ProductTable.Rows.Count + 1;
                        newRow["pro_name"] = textBox1.Text;
                        newRow["price"] = textBox2.Text;
                        newRow["count"] = 0;
                        if (radioButton1.Checked)
                        {
                            newRow["type"] = 1;
                            type = 1;
                            typeName = "와인";
                        }
                        else if (radioButton2.Checked)
                        {
                            newRow["type"] = 2;
                            type = 2;
                            typeName = "위스키";
                        }
                        else if (radioButton3.Checked)
                        {
                            newRow["type"] = 3;
                            type = 3;
                            typeName = "꼬냑";
                        }
                        else if (radioButton4.Checked)
                        {
                            newRow["type"] = 4;
                            type = 4;
                            typeName = "데낄라";
                        }
                        else if (radioButton5.Checked)
                        {
                            newRow["type"] = 5;
                            type = 5;
                            typeName = "리큐르";
                        }
                        else if (radioButton6.Checked)
                        {
                            newRow["type"] = 6;
                            type = 6;
                            typeName = "진";
                        }
                        else if (radioButton7.Checked)
                        {
                            newRow["type"] = 7;
                            type = 7;
                            typeName = "보드카";
                        }
                        else if (radioButton8.Checked)
                        {
                            newRow["type"] = 8;
                            type = 8;
                            typeName = "중국";
                        }
                        else if (radioButton9.Checked)
                        {
                            newRow["type"] = 9;
                            type = 9;
                            typeName = "기타";
                        }
                        else if (radioButton10.Checked)
                        {
                            newRow["type"] = 10;
                            type = 10;
                            typeName = "맥주";
                        }

                        dbc.ProductTable.Rows.Add(newRow);
                        dbc.DBAdapter.Update(dbc.DS, "product1");
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

        // 이하 분류 텍스트박스 textBox4.Text 변경
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Text = "와인";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Text = "위스키";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Text = "꼬냑";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Text = "데낄라";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Text = "리큐르";
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Text = "진";
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Text = "보드카";
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Text = "중국";
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Text = "기타";
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Text = "맥주";
        }
    }
}
