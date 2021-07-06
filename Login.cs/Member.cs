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
    public partial class Member : Form
    {
        DBClass dbc = new DBClass();
        DataRow selectedRow;
        string selectedId; // 셀클릭시 클릭한 셀의 id 정보를 저장하기 위한 변수 

        public Member()
        {
            InitializeComponent();
            dbc.MDB_Open();
            this.ControlBox = false;
        }

        public void groupAndButton()
        {
            button4.Text = "회원명 조회";
            button6.Text = "전화번호 조회";
            button9.Text = "전체 조회";

            groupBox2.Visible = false;
            groupBox3.Visible = false;
            groupBox5.Visible = false;
        }

        public void DBView()  // + unSort
        {
            DBGrid.DataSource = dbc.DS.Tables["member"].DefaultView;
            DBGrid.CurrentCell = null;
            DBGrid.RowHeadersVisible = false;  //  좌측 빈 컬럼 지우기

            for (int i = 1; i < DBGrid.Rows.Count; i++) // 홀수행 색 변경
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

            foreach (DataGridViewColumn item in DBGrid.Columns) // ColumnHeader 클릭 막기 (정렬막기)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public void ColumnHeader()
        {
            DBGrid.Columns[0].HeaderText = "회원 번호";
            DBGrid.Columns[1].HeaderText = "회원명";
            DBGrid.Columns[2].HeaderText = "전화 번호";
            DBGrid.Columns[0].Width = 85;
            DBGrid.Columns[2].Width = 150;
            DBGrid.Columns[3].Visible = false;
        }

        // 상세정보 텍스트박스 클리어
        public void InfoClear()
        {
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }

        // 회원명 조회메뉴
        private void button4_Click(object sender, EventArgs e)
        {
            groupAndButton();
            groupBox2.Visible = true;
            button4.Text = "▶ " + button4.Text + " ◀";
            textBox2.Focus();
        }

        // 전화번호 조회메뉴
        private void button6_Click(object sender, EventArgs e)
        {
            groupAndButton();
            groupBox3.Visible = true;
            button6.Text = "▶ " + button6.Text + " ◀";
            textBox1.Focus();
        }

        // 전체 조회
        private void button9_Click(object sender, EventArgs e)
        {
            groupAndButton();
            groupBox5.Visible = true;
            button9.Text = "▶ " + button9.Text + " ◀";
            label4.Text = "회원 [ 전체 ] 조회";

            dbc.MDB_Open();
            DBView();
            ColumnHeader();
            dbc.MemberTable = dbc.DS.Tables["member"];
            textBox7.Text = dbc.MemberTable.Rows.Count.ToString();
        }

        // 종료 버튼
        private void button8_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        // 회원명 검색 
        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("회원명을 입력하세요.", "알림");
            }
            else
            {
                InfoClear();
                dbc.MDB_Name(textBox2.Text);
                DBView();
                ColumnHeader();
                label4.Text = "회원 [ " + textBox2.Text + " ] 검색 결과";
                textBox2.Clear();
                InfoClear();
            }
        }
        // 회원명 검색 엔터이벤트
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button7_Click(sender, e);
            }
        }

        // 전화번호 검색
        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("전화번호를 입력하세요.", "알림");
            }
            else if (textBox1.Text.Length != 4)
            {
                MessageBox.Show("전화번호 뒷번호 네자리만 입력하세요.", "알림");
            }
            else
            {
                InfoClear();
                dbc.MDB_Phone(textBox1.Text);
                DBView();
                ColumnHeader();
                label4.Text = "전화번호 [ " + textBox1.Text + " ] 검색 결과";
                textBox1.Clear();
                InfoClear();
            }
        }
        // 전화번호 검색 엔터이벤트
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button5_Click(sender, e);
            }
        }

        // 회원 등록 버튼
        private void button1_Click(object sender, EventArgs e)
        {
            AddMember addMember = new AddMember();
            addMember.ShowDialog();

            // 등록 완료후 동작
            int addYN = addMember.AddYN;
            if (addYN == 1)
            {
                dbc.MDB_Open();
                dbc.MemberTable = dbc.DS.Tables["member"];
                int idCount = dbc.MemberTable.Rows.Count;   // 전체 DB를 담아서 rowCount를 변수지정
                dbc.MDB_Id(idCount);
                dbc.MemberTable = dbc.DS.Tables["member"];  // rowCount로 가장 마지막 등록된  mem_id를 조회
                DBView();
                ColumnHeader();
                groupAndButton();
                label4.Text = "신규 회원 등록 완료";
                groupBox5.Visible = true;
                textBox7.Text = (Convert.ToInt32(textBox7.Text) + 1).ToString();
            }
        }

        // 폼 로드
        private void Member_Load(object sender, EventArgs e)
        {
            button9_Click(sender, e); // 전화번호 조회 버튼
            this.ActiveControl = button9;
        }

        // 셀클릭
        private void DBGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dbc.MemberTable = dbc.DS.Tables["member"];
            if (e.RowIndex < 0)
            {
                return;
            }
            else if (e.RowIndex > dbc.MemberTable.Rows.Count - 1)
            {
                MessageBox.Show("해당하는 데이터가 존재하지 않습니다.", "알림");
                return;
            }
            selectedRow = dbc.MemberTable.Rows[e.RowIndex];
            textBox3.Text = selectedRow["mem_name"].ToString();
            textBox4.Text = selectedRow["mem_id"].ToString();
            textBox5.Text = selectedRow["mem_phone"].ToString();
            if (selectedRow["mem_address"].ToString() == "")
            {
                textBox6.Text = "주소 정보 없음";
            }
            else
            {
                textBox6.Text = selectedRow["mem_address"].ToString();
            }

            selectedId = selectedRow["mem_id"].ToString();
        }

        // 정보 수정 버튼
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("회원이 선택되지 않았습니다.", "알림");
            }
            else
            {
                UpdateMember updateMember = new UpdateMember();
                updateMember.Owner = this;
                updateMember.Mem_id = selectedRow["mem_id"].ToString();
                updateMember.Mem_name = selectedRow["mem_name"].ToString();
                updateMember.Mem_phone = selectedRow["mem_phone"].ToString();
                if (selectedRow["mem_address"].ToString() == "")
                {
                    updateMember.Mem_address = "주소 정보 없음";
                }
                else
                {
                    updateMember.Mem_address = selectedRow["mem_address"].ToString();
                }
                updateMember.ShowDialog();

                // 등록 완료후 동작
                int updateYN = updateMember.UpdateYN;
                if (updateYN == 1)
                {
                    dbc.MDB_Id(Convert.ToInt32(selectedRow["mem_id"]));
                    dbc.MemberTable = dbc.DS.Tables["member"];
                    label4.Text = "[ " + Convert.ToInt32(selectedRow["mem_id"]) + " ]번 회원 정보 수정 완료";
                    DBView();
                    groupAndButton();
                    groupBox5.Visible = true;
                }
            }
        }

        // 정보 삭제 버튼
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("삭제할 정보가 선택되지 않았습니다.", "알림");
            }
            else
            {
                DialogResult ok = MessageBox.Show("회원 번호\r\n[ " + selectedId + " ]번의 정보를 삭제하시겠습니까?.", "알림", MessageBoxButtons.YesNo);

                if (ok == DialogResult.Yes)
                {
                    try
                    {
                        dbc.MDB_Open();
                        dbc.MemberTable = dbc.DS.Tables["member"];
                        DataColumn[] PrimaryKey = new DataColumn[1];
                        PrimaryKey[0] = dbc.MemberTable.Columns["mem_id"];
                        dbc.MemberTable.PrimaryKey = PrimaryKey;
                        selectedRow = dbc.MemberTable.Rows.Find(Convert.ToInt32(selectedId));
                        int rowCount = dbc.MemberTable.Rows.Count;  // 전체 행의 개수 (삭제전)
                        string deletedName = selectedRow["mem_name"].ToString(); // 라벨 변경에 사용할 삭제할 회원명
                        selectedRow.Delete();
                        int select = Convert.ToInt32(selectedId);  //  selectedId 증감시킬경우 for문에 영향을 주므로 변수를 따로 지정해서 사용
                        
                        for (int i = 0; i < (rowCount - Convert.ToInt32(selectedId)); i++)  //  행 하나가 삭제될 경우 행의 인덱스가 상제 대상보다 높은경우 모두 -1 해줌
                        {
                            selectedRow = dbc.MemberTable.Rows[rowCount - (rowCount - select)];
                            selectedRow.BeginEdit();
                            selectedRow["mem_id"] = Convert.ToInt32(selectedRow["mem_id"]) - 1;
                            selectedRow.EndEdit();
                            select += 1;
                        }

                        dbc.DBAdapter.Update(dbc.DS, "member");
                        dbc.DS.AcceptChanges();

                        button9_Click(sender, e);
                        label4.Text = "회원 [ " + deletedName + " ] 정보 삭제 완료";
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
    }
}
