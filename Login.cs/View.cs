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
    public partial class View : Form
    {
        DBClass dbc = new DBClass();
        DBClass dbc2 = new DBClass();

        string date = DateTime.Now.ToString("yyyy-MM-dd");
        int type= 2; //  입고1 출고2 변수
        string typeName = "출고";  //  '입고' '출고' 문자열 변수
        int pro_id;   //  DBGrid sellClick 에서 얻은 상품번호
        int cancelCount; // DBGrid sellClick 에서 얻은 입 출고 수량
        int checkType;  //  조회시 날짜별 0 / 상품명 1 / 기간별 2 변수지정하여 삭제후 view에 띄울 db 결정
        string name;  // textBox에서 변수를 지정해 dbc.CDB_Name1(name, type); 메서드를 호출
        string selectId;  //  DBGrid 셀 클릭시 열의 0번인덱스(등록번호/countudpate기본키) 지정할 변수
        int totalPrice;  //  DBGrid 에 표시된 금액의 합산을 저장
        string priceText;  //  totalPrice를 자릿수(,) 표시한 문자열로 저장
        string dateString;  //  기간별 조회를 위한 변수
        int rowNum; // 최근 등록순 검색의 건수 저장을 위한 변수

        DataRow currRow, dRow, xRow;
        // x = countUp, Down
        // d = sellClick


        public View()
        {
            InitializeComponent();
            this.ControlBox = false;
            this.Width = 902;
        }

        public void DBView1()
        {
            DBGrid1.DataSource = dbc.DS.Tables["countupdate"].DefaultView;
            DBGrid1.CurrentCell = null; // 뷰 로드될때 첫번째열 선택안되게
            DBGrid1.RowHeadersVisible = false;  //  좌측 빈 컬럼 지우기
            for (int i = 1; i < DBGrid1.Rows.Count; i++)
            {
                if (i % 2 != 0)
                {
                    DBGrid1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(240, 255, 240);
                }
                else
                {
                    DBGrid1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        public void DBView2()
        {
            DBGrid2.DataSource = dbc.DS.Tables["countupdate"].DefaultView;
            DBGrid2.CurrentCell = null;
            DBGrid2.Sort(DBGrid2.Columns[0], ListSortDirection.Descending);
            DBGrid2.RowHeadersVisible = false;
        }

        public void textBoxClear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
        }

        public void GroupAndButton()
        {
            groupBox7.Visible = false;
            groupBox5.Visible = false;
            groupBox4.Visible = false;
            groupBox2.Visible = false;

            button12.Text = "날 짜 별";
            button13.Text = "기 간 별";
            button14.Text = "상 품 명";
            button15.Text = "상 품 번 호";
        }

        // 결산
        public void ColumnHeader1()
        {
            if(type == 1)
            {
                DBGrid2.Columns[0].HeaderText = "입고일";
                DBGrid2.Columns[1].HeaderText = "입고 상품명";
                DBGrid2.Columns[2].HeaderText = "입고 수량";
                DBGrid2.Columns[3].HeaderText = "금액";
                DBGrid2.Columns[4].HeaderText = "상품 번호";
                DBGrid2.Columns[5].Visible = false;
                DBGrid2.Columns[1].Width = 210;
                DBGrid2.Columns[2].Width = 85;
                DBGrid2.Columns[4].Width = 85;
            }else if(type == 2)
            {
                DBGrid2.Columns[0].HeaderText = "출고일";
                DBGrid2.Columns[1].HeaderText = "출고 상품명";
                DBGrid2.Columns[2].HeaderText = "출고 수량";
                DBGrid2.Columns[3].HeaderText = "금액";
                DBGrid2.Columns[4].HeaderText = "상품 번호";
                DBGrid2.Columns[5].Visible = false;
                DBGrid2.Columns[1].Width = 210;
                DBGrid2.Columns[2].Width = 85;
                DBGrid2.Columns[4].Width = 85;
            }
        }

        // 등록별
        public void ColumnHeader2()
        {
            if(type == 1)
            {
                DBGrid1.Columns[0].HeaderText = "등록 번호";
                DBGrid1.Columns[1].HeaderText = "입고 상품명";
                DBGrid1.Columns[2].HeaderText = "입고 수량";
                DBGrid1.Columns[3].HeaderText = "금액";
                DBGrid1.Columns[4].HeaderText = "입고일";
                DBGrid1.Columns[5].Visible = false;
                DBGrid1.Columns[0].Width = 85;
                DBGrid1.Columns[1].Width = 210;
                DBGrid1.Columns[2].Width = 85;
            }
            else if(type == 2)
            {
                DBGrid1.Columns[0].HeaderText = "등록 번호";
                DBGrid1.Columns[1].HeaderText = "출고 상품명";
                DBGrid1.Columns[2].HeaderText = "출고 수량";
                DBGrid1.Columns[3].HeaderText = "금액";
                DBGrid1.Columns[4].HeaderText = "출고일";
                DBGrid1.Columns[5].Visible = false;
                DBGrid1.Columns[0].Width = 85;
                DBGrid1.Columns[1].Width = 210;
                DBGrid1.Columns[2].Width = 85;
            }
        }

       public void CountUp() 
        {
            dbc2.PDB_Open();
            dbc2.ProductTable = dbc2.DS.Tables["product1"];
            xRow = dbc2.ProductTable.Rows[pro_id-1];
            xRow.BeginEdit();
            xRow["count"] = Convert.ToInt32(xRow["count"]) + cancelCount;
            xRow.EndEdit();
            dbc2.DBAdapter.Update(dbc2.DS, "product1");
            dbc2.DS.AcceptChanges();
        }

        public void CountDown() 
        {
            dbc2.PDB_Open();
            dbc2.ProductTable = dbc2.DS.Tables["product1"];
            xRow = dbc2.ProductTable.Rows[pro_id-1];
            xRow.BeginEdit();
            xRow["count"] = Convert.ToInt32(xRow["count"]) - cancelCount;
            xRow.EndEdit();
            dbc2.DBAdapter.Update(dbc2.DS, "product1");
            dbc2.DS.AcceptChanges();
        }

        public void unSort()
        {
            foreach (DataGridViewColumn item in DBGrid2.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn item in DBGrid1.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        // 판매금액의 합산을 구하여 자릿수를 지정한 문자열로 변환하여 라벨에 대입
        public void getTotalPrice()
        {
            totalPrice = 0;
           
            for(int i = 0; i<DBGrid2.Rows.Count; i++)
            {
                totalPrice += Convert.ToInt32(DBGrid2.Rows[i].Cells[3].FormattedValue);
            }
            string a = totalPrice.ToString();
            string[] aa;
            int priceLength = a.Length;
            if (priceLength == 6)
            {
                aa = new string[2];
                aa[0] = a.Substring(0, 3);
                aa[1] = a.Substring(3, 3);
                priceText = aa[0] + "," + aa[1];
            }
            else if (priceLength == 5)
            {
                aa = new string[2];
                aa[0] = a.Substring(0, 2);
                aa[1] = a.Substring(2, 3);
                priceText = aa[0] + "," + aa[1];
            }
            else if (priceLength == 4)
            {
                aa = new string[2];
                aa[0] = a.Substring(0, 1);
                aa[1] = a.Substring(1, 3);
                priceText = aa[0] + "," + aa[1];
            }
            else if (priceLength == 7)
            {
                aa = new string[3];
                aa[0] = a.Substring(0, 1);
                aa[1] = a.Substring(1, 3);
                aa[2] = a.Substring(3, 3);
                priceText = aa[0] + "," + aa[1] + "," + aa[2];
            }
            else if(totalPrice == 0)
            {
                priceText = "0";
            }
            label6.Text = "총계 : " + priceText + "원";
            label6.Visible = true;
        }

        // 날짜별 검색
        private void button7_Click(object sender, EventArgs e)
        {
            textBoxClear();
            date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            dbc.CDB_Open1(date, type);
            DBView2();
            ColumnHeader1();
           
            dbc.CDB_Open2(date, type);
            DBView1();
            ColumnHeader2();

            unSort(); // 정렬 막기
            
            // 라벨 변경
            label4.Text = "[" + date + "] "+typeName+" 결산";
            label5.Text = "[" + date + "] "+typeName+" 등록 내역";

            getTotalPrice();
            checkType = 0; // 삭제버튼으로
        }

        private void View_Load(object sender, EventArgs e)
        {
            button14_Click(sender, e);
            this.ActiveControl = textBox2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            type = 1;
            typeName = "입고";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            type = 2;
            typeName = "출고";
        }

        // 삭제 버튼
        private void button2_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("삭제할 정보가 선택되지 않았습니다.", "알림");
            }
            else
            {
                DialogResult ok = MessageBox.Show("입출고 등록번호\r\n[ " +selectId+" ]번의 정보를 삭제하시겠습니까?.", "알림", MessageBoxButtons.YesNo);

                if (ok == DialogResult.Yes)
                {
                    try
                    {
                        dbc.CDB_Open();
                        dbc.CountUpdateTable = dbc.DS.Tables["countupdate"];
                        DataColumn[] PrimaryKey = new DataColumn[1];
                        PrimaryKey[0] = dbc.CountUpdateTable.Columns["up_id"];
                        dbc.CountUpdateTable.PrimaryKey = PrimaryKey;
                        currRow = dbc.CountUpdateTable.Rows.Find(selectId);
                        int rowCount = dbc.CountUpdateTable.Rows.Count;  // 전체 행의 개수 (삭제전)
                        currRow.Delete();
                        int select = Convert.ToInt32(selectId);  //  dbc.SelectedRowIndex를 증감시킬경우 for문에 영향을 주므로 변수를 따로 지정해서 사용

                        for (int i = 0; i < (rowCount - Convert.ToInt32(selectId)); i++)  //  행 하나가 삭제될 경우 행의 인덱스가 상제 대상보다 높은경우 모두 -1 해줌
                        {
                            currRow = dbc.CountUpdateTable.Rows[rowCount - (rowCount - select)];
                            currRow.BeginEdit();
                            currRow["up_id"] = Convert.ToInt32(currRow["up_id"]) - 1;
                            currRow.EndEdit();
                            select += 1;
                        }

                        dbc.DBAdapter.Update(dbc.DS, "countupdate");
                        dbc.DS.AcceptChanges();

                        if (checkType == 0) // 날짜로 검색시
                        {
                            dbc.CDB_Open2(date, type);
                            DBView1();
                            dbc.CDB_Open1(date, type);
                            DBView2();
                        }
                        else if (checkType == 1) //  상품명 검색시
                        {
                            dbc.CDB_Name1(name, type);
                            DBView1();
                            dbc.CDB_Name2(name, type);
                            DBView2();
                        }
                        else if (checkType == 2)  // 기간별 검색시
                        {
                            dbc.CDB_Open2(dateString, type);
                            DBView1();
                            dbc.CDB_Open1(dateString, type);
                            DBView2();
                        }
                        else if(checkType == 3) // 최근 n건 검색시
                        {
                            rowNum = rowNum - 1;
                            dbc.CDB_Lately(rowNum, type);
                            DBView1();
                        }

                        // 입출고 삭제시 재고 복구 코드
                        if (type == 1)
                        {
                            CountDown();
                        }
                        else if (type == 2)
                        {
                            CountUp();
                        }
                        MessageBox.Show("삭제 완료", "알림");
                        textBoxClear();
                        if(DBGrid2.Columns.Count >= 1)
                        {
                            getTotalPrice();  //  삭제후 총계 업데이트
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

        // 상품명 검색
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("상품명이 입력되지 않았습니다.", "알림");
            }
            else
            {
                name = textBox2.Text;
                dbc.CDB_Name1(name, type);
                DBView1();
                dbc.CDB_Name2(name, type);
                DBView2();
                ColumnHeader1();
                ColumnHeader2();
                getTotalPrice();  //  총계 업데이트
                textBoxClear();
                checkType = 1; // 삭제버튼으로

                unSort(); // 정렬 막기

                // 라벨 변경
                label5.Text = "[" + name + "] 등록별 " + typeName + " 내역";
                label4.Text = "[" + name + "] 날짜별 " + typeName + " 결산";
            }
        }

        // 상품명 검색 엔터이벤트
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button4_Click(sender, e);
            }
        }

        // 기간별 조회
        private void button6_Click(object sender, EventArgs e)
        {
            textBoxClear();
            dateString = "";
            string setDate1 = dateTimePicker2.Value.ToString("yyyyMMdd");
            string setDate2 = dateTimePicker3.Value.ToString("yyyyMMdd");
            if (Convert.ToInt32(setDate1) > Convert.ToInt32(setDate2))
            {
                MessageBox.Show("종료일은 시작일보다 빠를 수 없습니다.", "알림");
            }
            else
            {
                string setDate1_Y = setDate1.Substring(0, 4);
                string setDate1_M = setDate1.Substring(4, 2);
                string setDate1_D = setDate1.Substring(6, 2);

                string setDate2_Y = setDate2.Substring(0, 4);
                string setDate2_M = setDate2.Substring(4, 2);
                string setDate2_D = setDate2.Substring(6, 2);

                string dateString_YM;

                int gapY = Convert.ToInt32(setDate2_Y) - Convert.ToInt32(setDate1_Y);
                int gapM = 13 - Convert.ToInt32(setDate1_M); // 11월~1월 이라면 13-11=2 (0,1,2 = 11,12,1월) 
                int gapD; 
                if(setDate1 == setDate2)
                {
                    dateString = setDate1_Y + "-" + setDate1_M + "-" + setDate1_D;
                }
                else if(setDate1_M == setDate2_M)
                {
                    dateString_YM = setDate1_Y + "-" + setDate1_M;
                    gapD = Convert.ToInt32(setDate2_D) - Convert.ToInt32(setDate1_D);
                    for(int i =0; i<=gapD; i++)
                    {
                        if (i == gapD)
                        {
                            dateString = dateString + dateString_YM + "-" + setDate1_D;
                        }
                        else
                        {
                            dateString = dateString + dateString_YM + "-" + setDate1_D + "%' or u.up_date like '%";
                            setDate1_D = (Convert.ToInt32(setDate1_D) + 1).ToString("00");
                        }
                    }
                }
                else
                {
                    gapD = 32 - Convert.ToInt32(setDate1_D);  // 시작일의 일에서 31일까지 남은기간
                    for (int i = 0; i <= gapY; i++)
                    {
                        if(i == 0)
                        {
                            gapM = 13 - Convert.ToInt32(setDate1_M);
                        }
                        else if(i == gapY)
                        {
                            gapM = Convert.ToInt32(setDate2_M);
                        }
                        else
                        {
                            gapM = 12;
                        }
                        dateString_YM = setDate1_Y + "-" + setDate1_M;
                        for (int j = 1; j <= gapM; j++)
                        {
                            dateString_YM = setDate1_Y + "-" + setDate1_M;
                            if (i == gapY && j == gapM)
                            {
                                gapD = Convert.ToInt32(setDate2_D);
                            }
                            for (int k = 1; k <= gapD; k++)
                            {
                                if (i == gapY && j == gapM && k == gapD)
                                {
                                    dateString = dateString + dateString_YM + "-" + setDate1_D;
                                }
                                else
                                {
                                    dateString = dateString + dateString_YM + "-" + setDate1_D + "%' or u.up_date like '%";
                                    setDate1_D = (Convert.ToInt32(setDate1_D) + 1).ToString("00");
                                }
                            }
                            setDate1_D = "01"; // 시작월(setDate1_M)이 끝나서 01로 초기화
                            gapD = 31; // for문 반복횟수를 31회로 재설정
                            setDate1_M = (Convert.ToInt32(setDate1_M) + 1).ToString("00");
                            if (setDate1_M == "13")
                            {
                                setDate1_M = "01";  //  13월까지 증가시 1월로 변환
                            }
                        }
                        setDate1_Y = (Convert.ToInt32(setDate1_Y) + 1).ToString(); // 연도 +1
                        setDate1_M = "01"; // 1월부터 재시작
                    }
                }
                
                checkType = 2; // 삭제버튼으로
                dbc.CDB_Open2(dateString, type);
                DBView1();
                ColumnHeader2();
                dbc.CDB_Open1(dateString, type);
                DBView2();
                ColumnHeader1();

                getTotalPrice(); // 총계 업데이트
                unSort(); // 정렬 막기

                // 라벨 변경
                label5.Text = "[" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "] ~ ["
                    + dateTimePicker3.Value.ToString("yyyy-MM-dd") + "] " + typeName + " 등록 내역";
                label4.Text = "[" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "] ~ ["
                    + dateTimePicker3.Value.ToString("yyyy-MM-dd") + "] " + typeName + " 결산";

                string a = dateString.Substring(0, 10);
                string b = dateString.Substring(dateString.Length - 10, 10);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            rowNum = Convert.ToInt32(comboBox1.Text);
            dbc.CDB_Lately(rowNum ,type);
            DBView1();
            ColumnHeader2();
            DBGrid2.Columns.Clear();

            unSort(); // 정렬 막기

            // 라벨 변경
            label5.Text = "최근 "+typeName+" 등록 내역 ["+comboBox1.Text.ToString()+"]건";
            label4.Text = "조회 결산";
            label6.Visible = false;
            
            checkType = 3; // 삭제 버튼으로
            textBoxClear();
        }

        // 상품 번호로 검색
        private void button9_Click(object sender, EventArgs e)
        {
           if(textBox3.Text == "")
            {
                MessageBox.Show("상품번호가 입력되지 않았습니다.", "알림");
            }
            else
            {
                int proNum = Convert.ToInt32(textBox3.Text);
                dbc.CDB_ProNum1(proNum, type);
                DBView1();
                ColumnHeader2();

                dbc.CDB_ProNum2(proNum, type);
                DBView2();
                ColumnHeader1();

                label5.Text = "상품번호 [" + proNum + "]번 " + typeName + " 등록 내역";
                label4.Text = "상품번호 [" + proNum + "]번 " + typeName + " 결산";
                getTotalPrice();
                textBoxClear();
            }
        }

        // 상품번호 엔터이벤트
        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
             if(e.KeyCode == Keys.Enter)
            {
                button9_Click(sender, e);
            }
        }

        // 상품번호 숫자만 입력받기
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
           if(!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void DBGrid2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DBGrid2.ClearSelection();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            date = dateTimePicker1.Value.ToString("yyy-MM-dd");
        }

        private void DBGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dbc.CDB_Open();                                                                                                                                 // 1. dbc객체에 countupdate 테이블 전체를 담음  > 등록번호(up_id)순으로 정렬되어있음
            dbc.CountUpdateTable = dbc.DS.Tables["countUpdate"];
            if (e.RowIndex < 0)
            {
                return;
            }
            else if (e.RowIndex > dbc.CountUpdateTable.Rows.Count - 1)
            {
                MessageBox.Show("해당하는 데이터가 존재하지 않습니다.", "알림");
                return;
            }

            // selectId = 셀클릭시 해당 열의 등록번호
            selectId =  DBGrid1.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();                                        // 2. 클릭한 셀의 0번 인덱스 (등록번호) 지정

            // cancelCount = 셀클릭시 해당 열의 등록수량
            cancelCount = Convert.ToInt32(DBGrid1.Rows[e.RowIndex].Cells[2].FormattedValue);                      // 3. 클릭한 셀의 2번 인덱스 (등록수량) 지정

            textBox1.Text = selectId.ToString();

            dRow = dbc.CountUpdateTable.Rows[Convert.ToInt32(selectId)-1];                                                   // 4. 전체 테이블에서 위에서 지정한 등록번호를 가진 열을 dRow로 지정
            // pro_id = dRow의 상품번호
            pro_id = Convert.ToInt32(dRow[1]);                                                                                                       // 5. dRow에 등록된 상품번호(product1의 id) 를 지정

            // 삭제 메뉴의 텍스트박스 지정
            textBox4.Text =  DBGrid1.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
            textBox5.Text =  DBGrid1.Rows[e.RowIndex].Cells[2].FormattedValue.ToString();
            textBox6.Text =  DBGrid1.Rows[e.RowIndex].Cells[3].FormattedValue.ToString();
            textBox7.Text =  DBGrid1.Rows[e.RowIndex].Cells[4].FormattedValue.ToString();
            if(type == 1)
            {
                textBox8.Text = "입고";
            }
            else if(type == 2)
            {
                textBox8.Text = "출고";
            }
        }

        // 상세 조회 메뉴 
        // 상품명 검색
        private void button14_Click(object sender, EventArgs e)
        {
            GroupAndButton();
            groupBox4.Visible = true;
            button14.Text = "▶  " + button14.Text + "  ◀";
            textBox2.Focus();
        }
        // 날짜별 검색
        private void button12_Click(object sender, EventArgs e)
        {
            GroupAndButton();
            groupBox2.Visible = true;
            button12.Text = "▶  " + button12.Text + "  ◀";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(this.Width != 1117)
            {
                this.Width = 1117;
                button3.Text = "삭제메뉴 닫기";
            }else if(this.Width == 1117)
            {
                this.Width = 902;
                button3.Text = "삭제메뉴 열기";
            }
        }

        // 기간별 검색
        private void button13_Click(object sender, EventArgs e)
        {
            GroupAndButton();
            groupBox5.Visible = true;
            button13.Text = "▶  " + button13.Text + "  ◀";
        }
        // 상품번호 검색
        private void button15_Click(object sender, EventArgs e)
        {
            GroupAndButton();
            groupBox7.Visible = true;
            button15.Text = "▶  " + button15.Text + "  ◀";
            textBox3.Focus();
        }
    }
}
