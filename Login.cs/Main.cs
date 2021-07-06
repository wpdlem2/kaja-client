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
    public partial class Main : Form
    {
        
        DBClass dbc = new DBClass();
        DBClass dbc2 = new DBClass();
        DataRow currRow;
        int count, result;
        DataTable productTable;
        UpdateProduct updateProduct;
        int type;
        int highLiteRowIndex; // 셀클릭시 인덱스를 받아 keyDown 이벤트로 하이라이트 설정

        string date = DateTime.Now.ToString("yyyy-MM-dd");

        public Main()
        {
            InitializeComponent();
            this.ControlBox = false;
            dbc.PDB_Open();
            dbc2.CDB_Open();
        }

        public void DBView()
        {
            DBGrid.DataSource = dbc.DS.Tables["product1"].DefaultView;
            DBGrid.CurrentCell = null;
            DBGrid.RowHeadersVisible = false; //  좌측 빈 컬럼 지우기

            for (int i = 1; i < DBGrid.Rows.Count; i++)
            {
                if (i % 2 != 0)
                {
                    DBGrid.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(240, 255, 240);
                }
                else
                {
                    DBGrid.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        // DBGrid 컬럼헤더 변경
        public void ColumnHeader()
        {
            DBGrid.Columns[0].HeaderText = "상품 번호";
            DBGrid.Columns[1].HeaderText = "상품명";
            DBGrid.Columns[2].HeaderText = "가격";
            DBGrid.Columns[3].HeaderText = "재고수량";
            DBGrid.Columns[4].HeaderText = "상품 타입";
            DBGrid.Columns[0].Width = 70;
            DBGrid.Columns[1].Width = 230;
            DBGrid.Columns[4].Visible = false;
        }

        public void ProNameDefault()
        {
            textBox8.Text = "상품을 선택해 주세요.";
        }
        
        // 입출고로 수량변경시 변경수량 커밋
        public void CountUp() 
        {
            currRow.BeginEdit();
            currRow["count"] = Convert.ToInt32(currRow["count"]) + Convert.ToInt32(textBox6.Text);
            currRow.EndEdit();
            dbc.DBAdapter.Update(dbc.DS, "product1");
            dbc.DS.AcceptChanges();
        }

        public void CountDown() 
        {
            currRow.BeginEdit();
            currRow["count"] = Convert.ToInt32(currRow["count"]) - Convert.ToInt32(textBox6.Text);
            currRow.EndEdit();
            dbc.DBAdapter.Update(dbc.DS, "product1");
            dbc.DS.AcceptChanges();
        }

        // 전체 품목 뷰 버튼
        public Button getButton
        {
            get { return button7; }
        }
        // 자식폼으로 라벨 넘김
        public Label getLabel
        {
            get { return label4; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Parent = (Login) Owner;
            DataLabel.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TimeLabel.Text = DateTime.Now.ToString("hh:mm tt" ,new System.Globalization.CultureInfo("en-US"));

            dbc.ProductTable = dbc.DS.Tables["product1"];
            DBView();
            ColumnHeader();
            foreach (DataGridViewColumn item in DBGrid.Columns)
            {
                item.SortMode=DataGridViewColumnSortMode.NotSortable;
            }

            this.ActiveControl = textBox2; // 폼 로드시 포커스될 컨트롤 설정
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AddProduct addProduct = new AddProduct();
            addProduct.ShowDialog();

            int type = addProduct.Type;
            string typeName = addProduct.TypeName;
            if(type != 0)
            {
                dbc.PDB_Open(type);
                DBView();
                label4.Text = typeName + " 신규 상품 등록";
            }
        }

        //카테고리 버튼
        private void button7_Click(object sender, EventArgs e)
        {
            label4.Text = "전체 재고 현황";
            ProNameDefault();

            dbc.PDB_Open();
            DBView();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label4.Text = "와인 재고 현황";
            ProNameDefault();

            dbc.PDB_Open(1);
            DBView();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label4.Text = "위스키 재고 현황";
            ProNameDefault();

            dbc.PDB_Open(2);
            DBView();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label4.Text = "꼬냑 재고 현황";
            ProNameDefault();

            dbc.PDB_Open(3);
            DBView();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            label4.Text = "데낄라 재고 현황";
            ProNameDefault();

            dbc.PDB_Open(4);
            DBView();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            label4.Text = "중국 재고 현황";
            ProNameDefault();

            dbc.PDB_Open(8);
            DBView();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            label4.Text = "기타 재고 현황";
            ProNameDefault();

            dbc.PDB_Open(9);
            DBView();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            label4.Text = "보드카 재고 현황";
            ProNameDefault();

            dbc.PDB_Open(7);
            DBView();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            label4.Text = "리큐르 재고 현황";
            ProNameDefault();

            dbc.PDB_Open(5);
            DBView();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            label4.Text = "진 재고 현황";
            ProNameDefault();

            dbc.PDB_Open(6);
            DBView();
        }

        private void button23_Click(object sender, EventArgs e)
        {
            label4.Text = "맥주 재고 현황";
            ProNameDefault();

            dbc.PDB_Open(10);
            DBView();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            int num = Convert.ToInt32(comboBox2.Text);
            dbc.Search_Count(num);
            DBView();

            textBox2.Clear();
            label4.Text = num.ToString() + "개 이하 재고 검색";
        }

        // 상품명 검색 버튼
        private void button12_Click(object sender, EventArgs e)
        {
            textBox8.Text = "상품을 선택해 주세요.";
            string name = textBox2.Text;
            dbc.Search_Produt(name);
            DBView();

            textBox2.Clear();
            label4.Text = "["+name +"] 검색 결과";
        }

        // 상품명 검색 엔터이벤트
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button12_Click(sender, e);
            }
        }

        // 상품 수정 버튼
        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox8.Text == "상품을 선택해 주세요.") 
            {
                MessageBox.Show("상품이 선택되지 않았습니다.", "알림");
            }
            else
            {
                 try
                {
                    updateProduct = new UpdateProduct();
                    updateProduct.Owner = this;

                    switch (currRow["type"].ToString())
                    {
                        case "1": 
                            updateProduct.Type = "와인";
                            break;
                        case "2": 
                            updateProduct.Type = "위스키";
                            break;
                        case "3": 
                            updateProduct.Type = "꼬냑";
                            break;
                        case "4": 
                            updateProduct.Type = "데낄라";
                            break;
                        case "5": 
                            updateProduct.Type = "리큐르";
                            break;
                        case "6": 
                            updateProduct.Type = "진";
                            break;
                        case "7": 
                            updateProduct.Type = "보드카";
                            break;
                        case "8": 
                            updateProduct.Type = "중국";
                            break;
                        case "9": 
                            updateProduct.Type = "기타";
                            break;
                    }
                    updateProduct.Id = currRow["id"].ToString();
                    updateProduct.Name = currRow["pro_name"].ToString();
                    updateProduct.Price = currRow["price"].ToString();
                    updateProduct.Count = currRow["count"].ToString();
                    updateProduct.ShowDialog();
                    int updateYN = updateProduct.UpdateYN;
                    if(updateYN == 1)
                    {
                        string name = updateProduct.NameChanged;
                        dbc.Search_Produt(name);
                        DBView();
                        label4.Text ="[ "+ name + " ] 수정 완료";
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

        // 수정결과 버튼
        private void button13_Click(object sender, EventArgs e)
        {
            string name = updateProduct.ChangedName;
            
            dbc.Search_Produt(name);
            DBView();

            label4.Text = "["+name+"]"+" 검색 결과";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(textBox8.Text == "상품을 선택해 주세요.")
            {
                MessageBox.Show("상품이 선택되지 않았습니다.", "알림");
            }
            else
            {
                textBox4.Text = date;
                textBox5.Text = textBox8.Text;
                textBox6.Text = comboBox1.Text;
                textBox7.Text = "입고";
                type = 1;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(textBox8.Text == "상품을 선택해 주세요.")
            {
                MessageBox.Show("상품이 선택되지 않았습니다.", "알림");
            }
            else
            {
                textBox4.Text = date;
                textBox5.Text = textBox8.Text;
                textBox6.Text = comboBox1.Text;
                textBox7.Text = "출고";
                type = 2;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
        }

        private void button26_Click(object sender, EventArgs e)
        {
            if(textBox4.Text =="" || textBox5.Text == "상품을 선택해 주세요." || textBox6.Text == "" || textBox7.Text == "")
            {
                MessageBox.Show("입출고 내용에 공백이 있습니다.", "알림");
            }
            else
            {
                DialogResult ok = MessageBox.Show(textBox7.Text + " 등록 하시겠습니까?" , "알림", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (ok == DialogResult.Yes)
                {
                    dbc2.DS.Clear();
                    dbc2.DBAdapter.Fill(dbc2.DS, "countupdate");
                    dbc2.CountUpdateTable = dbc2.DS.Tables["countupdate"];
                    DataRow newRow = dbc2.CountUpdateTable.NewRow();
                    newRow["up_id"] = dbc2.CountUpdateTable.Rows.Count + 1;
                    newRow["pro_id"] = currRow["id"].ToString();
                    newRow["count"] = textBox6.Text;
                    newRow["up_date"] = textBox4.Text;
                    if(textBox7.Text == "입고")
                    {
                        newRow["type"] = 1;
                        CountUp();
                    }else if(textBox7.Text == "출고")
                    {
                        newRow["type"] = 2;
                        CountDown();
                    }
                    dbc2.CountUpdateTable.Rows.Add(newRow);
                    dbc2.DBAdapter.Update(dbc2.DS, "countupdate");
                    dbc2.DS.AcceptChanges();

                    DBView();

                    MessageBox.Show(textBox7.Text + " 등록 완료.");

                    // 확인후 텍스트박스 비워줌
                    comboBox1.Text = "1";
                    textBox8.Text = "상품을 선택해 주세요.";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    textBox7.Text = "";
                    
                }
            }
        }

        // 가격순 정렬
        private void button24_Click(object sender, EventArgs e)
        {
            dbc.PDB_Price();
            DBView();
            ProNameDefault();

            if(dbc.Result == 1)
            {
                label4.Text = "높은 가격순으로 정렬";
            }
            else if(dbc.Result == 0)
            {
                 label4.Text = "낮은 가격순으로 정렬";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeLabel.Text = DateTime.Now.ToString("hh:mm tt" ,new System.Globalization.CultureInfo("en-US"));
        }

        // 셀 클릭 이벤트
        private void DBGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                productTable = dbc.DS.Tables["product1"];
                if (e.RowIndex < 0)
                {
                    return;
                }
                else if (e.RowIndex > productTable.Rows.Count - 1)
                {
                    MessageBox.Show("해당하는 데이터가 존재하지 않습니다.", "알림");
                    return;
                }
                string id =  DBGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
                currRow = productTable.Rows[e.RowIndex];
                // *******
                highLiteRowIndex = e.RowIndex;

                textBox8.Text = currRow["pro_name"].ToString();

                dbc.SelectedRowIndex = Convert.ToInt32(currRow["id"]);
                if(result == 1)
                {
                    currRow = productTable.Rows[1];
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

//상품타입 안들어가는것 수정
