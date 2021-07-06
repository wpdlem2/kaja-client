using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using System.IO;

namespace Login.cs
{
    public partial class ExcelCopy : Form
    {
        int rowCount; // 조회버튼에서 행의 갯수 지정 
        int rowCount2;
        DBClass dbc = new DBClass();
        string path;
        int saveType = 1; // 저장버튼클릭시 재고 저정 메소드, 입출고 저장 메소드 선별 ( 1=재고저장, 2=입출고저장 ) 

        public ExcelCopy()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        public void ButtonClear()
        {
            button3.Text = "재고 현황";
            button7.Text = "입출고 내역";
        }

        // 입출고 엑셀 저장
        public void SaveAccount()
        {
            path = dbc.ExcelForder + "입출고\\" + textBox4.Text + ".xlsx";
            try
            {
                var excelApp = new Excel.Application();
                excelApp.Workbooks.Add();
                Excel._Worksheet worksheet = (Excel._Worksheet)excelApp.ActiveSheet;

                // 기본 폰트 사이즈
                excelApp.ActiveSheet.Rows.Font.Size = 11;
                // 출고 컬럼명 속성
                excelApp.ActiveSheet.Rows("2").Font.Size = 12;
                excelApp.ActiveSheet.Rows("2").Font.Bold = true;
                excelApp.ActiveSheet.Rows("2").RowHeight = 22;
                // 입고 컬럼명 속성 (출고rowCount +6 번째 행)
                excelApp.ActiveSheet.Rows(rowCount + 6).Font.Size = 12;
                excelApp.ActiveSheet.Rows(rowCount + 6).Font.Bold = true;
                excelApp.ActiveSheet.Rows(rowCount + 6).RowHeight = 22;
                // 등록일, 기간 셀 속성
                excelApp.ActiveSheet.Range("G2", "G3").Font.Size = 12;
                excelApp.ActiveSheet.Range("G2", "G3").Font.Bold = true;
                // 컬럼 열 너비
                excelApp.ActiveSheet.Columns("B").ColumnWidth = 20;
                excelApp.ActiveSheet.Columns("C").ColumnWidth = 50;
                excelApp.ActiveSheet.Columns("D").ColumnWidth = 13;
                excelApp.ActiveSheet.Columns("E").ColumnWidth = 15;
                excelApp.ActiveSheet.Columns("G").ColumnWidth = 40;
                // 금액 컬럼 포맷
                excelApp.ActiveSheet.Columns("E").NumberFormat = "\\" + "#,##0";
                // 각 행열별 텍스트 정렬
                excelApp.ActiveSheet.Columns("B").HorizontalAlignment = 3; // 3:중앙 , 2:오른쪽
                excelApp.ActiveSheet.Columns("C").HorizontalAlignment = 2;
                excelApp.ActiveSheet.Columns("D").HorizontalAlignment = 3;
                excelApp.ActiveSheet.Columns("E").HorizontalAlignment = 2;
                excelApp.ActiveSheet.Columns("G").HorizontalAlignment = 3;
                excelApp.ActiveSheet.Rows("2").HorizontalAlignment = 3;
                excelApp.ActiveSheet.Rows(rowCount + 6).HorizontalAlignment = 3;
                // 테두리 설정(출고)
                excelApp.ActiveSheet.Range("B2", "E" + (rowCount + 2)).Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlDouble;
                excelApp.ActiveSheet.Range("B2", "E" + (rowCount + 2)).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
                excelApp.ActiveSheet.Range("B2", "E" + (rowCount + 2)).Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDouble;
                excelApp.ActiveSheet.Range("B2", "E" + (rowCount + 2)).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlDouble;
                excelApp.ActiveSheet.Range("B2", "E2").Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
                // 테두리 설정 (입고)
                string sell = "E" + (rowCount + rowCount2 + 6);
                excelApp.ActiveSheet.Range("B" + (rowCount + 6), sell).Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlDouble;
                excelApp.ActiveSheet.Range("B" + (rowCount + 6), sell).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
                excelApp.ActiveSheet.Range("B" + (rowCount + 6), sell).Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDouble;
                excelApp.ActiveSheet.Range("B" + (rowCount + 6), sell).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlDouble;
                excelApp.ActiveSheet.Range("B" + (rowCount + 6), "E" + (rowCount + 5)).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
                // 테두리 설정 (등록일, 기간)
                excelApp.ActiveSheet.Range("G2:G3").Borders.LineStyle = Excel.XlLineStyle.xlDouble;
                // 합계 표시 셀 폰트
                excelApp.ActiveSheet.Rows(rowCount+3).Font.Bold = true;
                excelApp.ActiveSheet.Rows(rowCount+3).RowHeight = 22;
                excelApp.ActiveSheet.Rows(rowCount+rowCount2+7).Font.Bold = true;
                excelApp.ActiveSheet.Rows(rowCount+rowCount2+7).RowHeight = 22;
                // 합계 표시 테두리
                excelApp.ActiveSheet.Range("D" + (rowCount + 3), "E" + (rowCount + 3)).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
                excelApp.ActiveSheet.Range("D" + (rowCount + 3), "E" + (rowCount + 3)).Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDouble;
                excelApp.ActiveSheet.Range("D" + (rowCount + 3), "E" + (rowCount + 3)).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlDouble;
                excelApp.ActiveSheet.Range("D" + (rowCount + rowCount2 + 7), "E" + (rowCount + rowCount2 + 7)).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
                excelApp.ActiveSheet.Range("D" + (rowCount + rowCount2 + 7), "E" + (rowCount + rowCount2 + 7)).Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDouble;
                excelApp.ActiveSheet.Range("D" + (rowCount + rowCount2 + 7), "E" + (rowCount + rowCount2 + 7)).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlDouble;

                for (var i = 0; i < DBGrid2.Columns.Count - 2; i++)
                {
                    worksheet.Cells[2, i + 2] = DBGrid2.Columns[i].HeaderText;
                }
                for (var i = 0; i < DBGrid2.Rows.Count; i++)
                {
                    for (var j = 0; j <= DBGrid2.Columns.Count - 3; j++)
                    {
                        worksheet.Cells[i + 3, j + 2] = DBGrid2.Rows[i].Cells[j].Value;
                    }
                }

                for (var i = 0; i < DBGrid3.Columns.Count - 2; i++)
                {
                    worksheet.Cells[rowCount + 6, i + 2] = DBGrid3.Columns[i].HeaderText;
                }
                for (var i = 0; i < DBGrid3.Rows.Count; i++)
                {
                    for (var j = 0; j <= DBGrid3.Columns.Count - 3; j++)
                    {
                        worksheet.Cells[rowCount + i + 7, j + 2] = DBGrid3.Rows[i].Cells[j].Value;
                    }
                }

                worksheet.Cells[2, 7] = "작성일 : " + DateTime.Now.ToString("yyyy-MM-dd");
                worksheet.Cells[3, 7] = dateTimePicker1.Value.ToString("yyyy-MM-dd") + " ~ " + dateTimePicker2.Value.ToString("yyyy-MM-dd");
                int total1 = 0;
                int total2 = 0;
                for (int i = 0; i < rowCount; i++)
                {
                    total1 += Convert.ToInt32(DBGrid2.Rows[i].Cells[3].FormattedValue);
                }
                for (int i = 0; i < rowCount2; i++)
                {
                    total2 += Convert.ToInt32(DBGrid3.Rows[i].Cells[3].FormattedValue);
                }
                worksheet.Cells[rowCount + 3, 4] = "출고 합계 : ";
                worksheet.Cells[rowCount + 3, 5] = total1;
                worksheet.Cells[(rowCount + rowCount2 + 7), 4] = "입고 합계 : ";
                worksheet.Cells[(rowCount + rowCount2 + 7), 5] = total2;

                if (File.Exists(path))
                {
                    DialogResult ok = MessageBox.Show("해당 파일명이 이미 존재합니다. \r\n기존 파일을 삭제하고 새로 저장하시겠습니까?", "알림", MessageBoxButtons.YesNo);

                    if (ok == DialogResult.Yes)
                    {
                        File.Delete(path);
                    }
                    else
                    {
                        return;
                    }
                }

                worksheet.SaveAs(path);
                excelApp.Quit();

                MessageBox.Show("저장이 완료되었습니다.", "알림");
                if (radioButton1.Checked)
                {
                    System.Diagnostics.Process.Start(path);  // 저장 완료후 파일 열기
                }
                else if (radioButton2.Checked)
                {
                    System.Diagnostics.Process.Start(textBox5.Text);
                }
                Dispose();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void TextBoxClear()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox6.Clear();
            label11.Text = "데이터 없음";
        }

        // 재고현황 엑셀 저장
        public void SaveProduct()
        {
            path = dbc.ExcelForder + "재고\\" + textBox4.Text + ".xlsx";
            try
            {
                var excelApp = new Excel.Application();
                excelApp.Workbooks.Add();
                Excel._Worksheet worksheet = (Excel._Worksheet)excelApp.ActiveSheet;

                // 기본 폰트 사이즈
                excelApp.ActiveSheet.Rows.Font.Size = 11;
                // 출고 컬럼명 속성
                excelApp.ActiveSheet.Rows("2").Font.Size = 12;
                excelApp.ActiveSheet.Rows("2").Font.Bold = true;
                excelApp.ActiveSheet.Rows("2").RowHeight = 22;
                // 컬럼별 열 너비
                excelApp.ActiveSheet.Columns("B").ColumnWidth = 10;
                excelApp.ActiveSheet.Columns("C").ColumnWidth = 50;
                excelApp.ActiveSheet.Columns("D").ColumnWidth = 15;
                excelApp.ActiveSheet.Columns("E").ColumnWidth = 13;
                excelApp.ActiveSheet.Columns("F").ColumnWidth = 20;
                excelApp.ActiveSheet.Columns("H").ColumnWidth = 30;
                // 금액 열 포맷
                excelApp.ActiveSheet.Columns("D").NumberFormat = "\\" + "#,##0";
                excelApp.ActiveSheet.Columns("F").NumberFormat = "\\" + "#,##0";
                // 텍스트 정렬
                excelApp.ActiveSheet.Columns("A:F").HorizontalAlignment = 3;
                excelApp.ActiveSheet.Columns("C:D").HorizontalAlignment = 2;
                excelApp.ActiveSheet.Columns("F").HorizontalAlignment = 2;
                excelApp.ActiveSheet.Rows("2").HorizontalAlignment = 3;
                // 테두리 설정
                string cell = "F" + (rowCount + 2);
                excelApp.ActiveSheet.Range("B2", cell).Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlDouble;
                excelApp.ActiveSheet.Range("B2", cell).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
                excelApp.ActiveSheet.Range("B2", cell).Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDouble;
                excelApp.ActiveSheet.Range("B2", cell).Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlDouble;
                excelApp.ActiveSheet.Range("B2", "F2").Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlDouble;
                excelApp.ActiveSheet.Range("H2").Borders.LineStyle = Excel.XlLineStyle.xlDouble;
                // 총계 테두리
                excelApp.ActiveSheet.Cells[rowCount+3, 6].Borders.LineStyle = Excel.XlLineStyle.xlDouble;
                // 합계 표시 셀 폰트
                excelApp.ActiveSheet.Rows(rowCount + 3).Font.Bold = true;
                excelApp.ActiveSheet.Rows(rowCount+3).RowHeight = 22;
                for (var i = 0; i < DBGrid.Columns.Count - 1; i++)
                {
                    worksheet.Cells[2, i + 2] = DBGrid.Columns[i].HeaderText;
                }

               for (var i = 0; i < DBGrid.Rows.Count; i++)
                {
                    for (var j = 0; j < DBGrid.Columns.Count - 1; j++)
                    {
                        worksheet.Cells[i + 3, j + 2] = DBGrid.Rows[i].Cells[j].Value;
                    }
                }

                // 작성일
                worksheet.Cells[2, 8] = "작성일 : " + DateTime.Now.ToString("yyyy-MM-dd");
                // 총계
                worksheet.Cells[2, 6] = "총 계";
                int total = 0;
                for (var j = 0; j < DBGrid.Rows.Count; j++)
                {
                    worksheet.Cells[j + 3, 6] = Convert.ToInt32(DBGrid.Rows[j].Cells[2].FormattedValue) * Convert.ToInt32(DBGrid.Rows[j].Cells[3].FormattedValue);
                }
                for(int i=0; i<rowCount; i++)
                {
                    total += worksheet.Cells[i + 3, 6].value;
                }
                worksheet.Cells[rowCount + 3, 6] = total;


                if (File.Exists(path))
                {
                    DialogResult ok = MessageBox.Show("해당 파일명이 이미 존재합니다. \r\n기존 파일을 삭제하고 새로 저장하시겠습니까?", "알림", MessageBoxButtons.YesNo);

                    if (ok == DialogResult.Yes)
                    {
                        File.Delete(path);
                    }
                    else
                    {
                        return;
                    }
                }

                worksheet.SaveAs(path);
                excelApp.Quit();

                MessageBox.Show("저장이 완료되었습니다.", "알림");
                if(this.Visible == true)
                {
                    if (radioButton1.Checked)
                    {
                        System.Diagnostics.Process.Start(path);  // 저장 완료후 파일 열기
                    }
                    else if (radioButton2.Checked)
                    {
                        System.Diagnostics.Process.Start(textBox5.Text);
                    }
                }
                Dispose();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // 재고현황 헤더
        public void ColumnHeaderP()
        {
            DBGrid.Columns[0].HeaderText = "상품 번호";
            DBGrid.Columns[1].HeaderText = "상품명";
            DBGrid.Columns[2].HeaderText = "가격";
            DBGrid.Columns[3].HeaderText = "재고수량";
            DBGrid.Columns[4].HeaderText = "상품 타입";
            DBGrid.Columns[0].Width = 80;
            DBGrid.Columns[1].Width = 230;
            DBGrid.Columns[4].Visible = false;
        }

        // 출고 헤더
        public void ColumnHeaderC2()
        {
            DBGrid2.Columns[0].HeaderText = "출고일";
            DBGrid2.Columns[1].HeaderText = "출고 상품명";
            DBGrid2.Columns[2].HeaderText = "출고 수량";
            DBGrid2.Columns[3].HeaderText = "금액";
            DBGrid2.Columns[5].Visible = false;
            DBGrid2.Columns[4].Visible = false;
            DBGrid2.Columns[1].Width = 225;
            DBGrid2.Columns[2].Width = 85;
        }

        // 입고 헤더
        public void ColumnHeaderC1()
        {
            DBGrid3.Columns[0].HeaderText = "입고일";
            DBGrid3.Columns[1].HeaderText = "입고 상품명";
            DBGrid3.Columns[2].HeaderText = "입고 수량";
            DBGrid3.Columns[3].HeaderText = "금액";
            DBGrid3.Columns[5].Visible = false;
            DBGrid3.Columns[4].Visible = false;
            DBGrid3.Columns[1].Width = 225;
            DBGrid3.Columns[2].Width = 85;
        }

        public void PDBView()
        {
            dbc.ProductTable = dbc.DS.Tables["product1"];
            DBGrid.DataSource = dbc.ProductTable.DefaultView;
            DBGrid.CurrentCell = null;
            DBGrid.RowHeadersVisible = false; //  좌측 빈 컬럼 지우기
            foreach (DataGridViewColumn item in DBGrid.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
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

        public void CDBView2()
        {
            dbc.CountUpdateTable = dbc.DS.Tables["countupdate"];
            DBGrid2.DataSource = dbc.CountUpdateTable.DefaultView;
            DBGrid2.CurrentCell = null;
            DBGrid2.RowHeadersVisible = false; //  좌측 빈 컬럼 지우기
            DBGrid2.Sort(DBGrid2.Columns[0], ListSortDirection.Ascending);
            foreach (DataGridViewColumn item in DBGrid2.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            for (int i = 1; i < DBGrid2.Rows.Count; i++)
            {
                if (i % 2 != 0)
                {
                    DBGrid2.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(240, 255, 240);
                }
                else
                {
                    DBGrid2.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        public void CDBView1()
        {
            dbc.CountUpdateTable = dbc.DS.Tables["countupdate"];
            DBGrid3.DataSource = dbc.CountUpdateTable.DefaultView;
            DBGrid3.CurrentCell = null;
            DBGrid3.RowHeadersVisible = false; //  좌측 빈 컬럼 지우기
            DBGrid2.Sort(DBGrid2.Columns[0], ListSortDirection.Ascending);
            foreach (DataGridViewColumn item in DBGrid3.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            for (int i = 1; i < DBGrid3.Rows.Count; i++)
            {
                if (i % 2 != 0)
                {
                    DBGrid3.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(240, 255, 240);
                }
                else
                {
                    DBGrid3.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        // 저장
        public void button2_Click(object sender, EventArgs e)
        {
            if (label11.Text == "데이터 없음") 
            {
                MessageBox.Show("저장할 정보가 선택되지 않았습니다.", "알림");
            }
            else
            {
                if (saveType == 1)
                {
                    if(DBGrid.Rows.Count != 0)
                    {
                        MessageBox.Show("현재 재고를 엑셀로 저장합니다.\r\n저장 작업은 최대 3분이 소요됩니다.\r\n자동으로 종료되기 전 임의로 프로그램을 종료하지 마세요.","알림");
                        SaveProduct();
                    }
                    else if(DBGrid.Rows.Count == 0)
                    {
                        MessageBox.Show("저장할 정보가 선택되지 않았습니다.", "알림");
                    }
                }
                else if (saveType == 2)
                {
                    if(DBGrid2.Rows.Count !=0 || DBGrid3.Rows.Count != 0)
                    {
                        MessageBox.Show("입출고 내역을 엑셀로 등록합니다.\r\n저장 작업은 최대 3분이 소요됩니다.\r\n자동으로 종료되기 전 임의로 프로그램을 종료하지 마세요.", "알림");
                        SaveAccount();
                    }
                    else if(DBGrid2.Rows.Count ==0 && DBGrid3.Rows.Count == 0)
                    {
                         MessageBox.Show("지정한 날짜의 입출고 내역이 존재하지 않습니다.", "알림");
                    }
                }
            }
        }

        // 저장 폴더 열기
        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(textBox5.Text);
        }

        // 파일명 수정
        private void button5_Click(object sender, EventArgs e)
        {
            textBox4.ReadOnly = false;
            textBox4.Focus();
        }
        // 파일명 수정 엔터 이벤트
        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox4.ReadOnly = true;
                button2.Focus();
            }
        }

        // 저장경로 텍스트박스 클릭시
        private void textBox5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("저장 경로는 수정할 수 없습니다.", "알림");
            button2.Focus();
        }

        private void ExcelCopy_Load(object sender, EventArgs e)
        {
            this.ActiveControl = label1; // 폼 로드시 포커스될 컨트롤 설정
        }

        // 재고 현황 버튼
        public void button3_Click(object sender, EventArgs e)
        {
            // 버튼 텍스트
            ButtonClear();
            button3.Text = "▶ " + button3.Text + " ◀";
            label6.Visible = true;

            // 뷰 띄우기
            dbc.PDB_Open();
            PDBView();
            ColumnHeaderP();
            DBGrid2.DataSource = null;
            DBGrid3.DataSource = null;

            // 데이터타입
            TextBoxClear();
            textBox1.Text = "현재 재고 수량";
            textBox2.Text = DateTime.Now.ToString("yyyy-MM-dd");
            label11.Text = "현재 기준\r\n전체 " + DBGrid.Rows.Count + "개의 상품정보\r\n(상품번호, 상품명, 가격, 수량)\r\n불러오기 완료.";
            textBox3.Text = "해당 없음";
            textBox6.Text = "해당 없음";

            // 저장 정보 텍스트박스 ( 파일명, 경로 지정 )
            textBox4.Text = DateTime.Now.ToString("yyyy-MM-dd") + " 재고";
            textBox5.Text = dbc.ExcelForder + "재고";

            // 기준일 정보
            label6.Text = DateTime.Now.ToString("yyyy-MM-dd") + "\r\n" + DateTime.Now.ToString("hh:mm tt", new System.Globalization.CultureInfo("en-US"));

            // row수를 담아서 엑셀로 넘겨줌 ( 테두리 설정을 위함 )
            rowCount = DBGrid.Rows.Count;

            // 저장 타입을 재고현황으로 설정
            saveType = 1;
        }

        // 입출고 현황 버튼
        private void button7_Click(object sender, EventArgs e)
        {
            // 버튼 텍스트
            ButtonClear();
            button7.Text = "▶ " + button7.Text + " ◀";
            label6.Visible = false;

            // 데이터 타입
            TextBoxClear();
            textBox1.Text = "입출고 현황";
            textBox2.Text = DateTime.Now.ToString("yyyy-MM-dd");

            // 저장 정보 텍스트박스 ( 파일명, 경로 지정 )
            textBox4.Text = DateTime.Now.ToString("yyyy-MM-dd") + " 결산";
            textBox5.Text = dbc.ExcelForder + "입출고";

            // 저장타입을 입출고로 설정
            saveType = 2;

            // 재고 뷰 지움
            DBGrid.DataSource = null;
        }

        // 입출고 내역 > 입출고 조회
        private void button6_Click(object sender, EventArgs e)
        {
            string dateString = "";
            string setDate1 = dateTimePicker1.Value.ToString("yyyyMMdd");
            string setDate2 = dateTimePicker2.Value.ToString("yyyyMMdd");
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
                if (setDate1 == setDate2)
                {
                    dateString = setDate1_Y + "-" + setDate1_M + "-" + setDate1_D;
                }
                else if (setDate1_M == setDate2_M)
                {
                    dateString_YM = setDate1_Y + "-" + setDate1_M;
                    gapD = Convert.ToInt32(setDate2_D) - Convert.ToInt32(setDate1_D);
                    for (int i = 0; i <= gapD; i++)
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
                        if (i == 0)
                        {
                            gapM = 13 - Convert.ToInt32(setDate1_M);
                        }
                        else if (i == gapY)
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
                dbc.CDB_Open1(dateString, 2);
                CDBView2();
                ColumnHeaderC2();

                dbc.CDB_Open1(dateString, 1);
                CDBView1();
                ColumnHeaderC1();

                // row수를 담아서 엑셀로 넘겨줌 ( 테두리 설정을 위함 )
                rowCount = DBGrid2.Rows.Count;
                rowCount2 = DBGrid3.Rows.Count;

                // 데이터 타입 그룹박스
                textBox3.Text = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                textBox6.Text = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                if(DBGrid2.Rows.Count ==0 && DBGrid3.Rows.Count==0)
                {
                    label11.Text = textBox3.Text + " ~ " + textBox6.Text + "\r\n출고 내역 총" + DBGrid2.Rows.Count + "개\r\n입고 내역 총" + DBGrid3.Rows.Count + "개\r\n불러올 데이터 없음.";
                }
                else
                {
                    label11.Text = textBox3.Text + " ~ " + textBox6.Text + "\r\n출고 내역 총" + DBGrid2.Rows.Count + "개\r\n입고 내역 총" + DBGrid3.Rows.Count + "개\r\n정보 불러오기 완료.";
                }
            }
        }
    }
}
