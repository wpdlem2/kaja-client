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
    public partial class Schedule : Form
    {
        DBClass dbc = new DBClass();
        DataRow selectRow, deleteRow;
        string selectId;

        public Schedule()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        public void ColumnHeader()
        {
            DBGrid.Columns[0].HeaderText = "번호";
            DBGrid.Columns[1].HeaderText = "제목";
            DBGrid.Columns[3].HeaderText = "날짜";
            DBGrid.Columns[2].Visible = false;
            DBGrid.Columns[4].Visible = false;
            DBGrid.Columns[0].Width = 60;
            DBGrid.Columns[1].Width = 203;
        }

        public void DBView() // + unSort
        {
            dbc.ScheduleTable = dbc.DS.Tables["schedule"];
            DBGrid.DataSource = dbc.DS.Tables["schedule"].DefaultView;
            DBGrid.CurrentCell = null;
            DBGrid.Sort(DBGrid.Columns[3], ListSortDirection.Ascending);
            DBGrid.RowHeadersVisible = false;
            foreach (DataGridViewColumn item in DBGrid.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddSchedule addSchedule = new AddSchedule();
            addSchedule.ShowDialog();

            button2_Click(sender, e);
        }

        private void Schedule_Load(object sender, EventArgs e)
        {
            dbc.SDB_Open();
            DBView();
            ColumnHeader();

            for(int i=0; i<DBGrid.Rows.Count; i++)  // 해당 날짜의 일정에 하이라이트
            {
                if(DBGrid.Rows[i].Cells[3].FormattedValue.ToString() == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    DBGrid.Rows[i].DefaultCellStyle.Font = new Font("Fixsys", 9, FontStyle.Bold);
                    DBGrid.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }

        private void DBGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            else if (e.RowIndex > dbc.ScheduleTable.Rows.Count - 1)
            {
                MessageBox.Show("해당하는 데이터가 존재하지 않습니다.", "알림");
                return;
            }
            selectRow = dbc.ScheduleTable.Rows[e.RowIndex];
            selectId =  DBGrid.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
            textBox2.Text = "제목 : " + selectRow["sc_title"].ToString() + "\r\n날짜 : " + selectRow["sc_date"].ToString() +"\r\n=======================" + selectRow["sc_info"].ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Clear(); // 일정상세 지우기
            dbc.SDB_Open();
            DBView();
            
            for (int i = 0; i < DBGrid.Rows.Count; i++)  // 해당 날짜의 일정에 하이라이트
            {
                if (DBGrid.Rows[i].Cells[3].FormattedValue.ToString() == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    DBGrid.Rows[i].DefaultCellStyle.Font = new Font("Fixsys", 9, FontStyle.Bold);
                    DBGrid.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(textBox2.Text == "")
            {
                MessageBox.Show("삭제할 정보가 선택되지 않았습니다.", "알림");
            }
            else
            {
                DialogResult ok = MessageBox.Show("일정을 삭제하시겠습니까?", "알림", MessageBoxButtons.YesNo);

                if (ok == DialogResult.Yes)
                {
                    try
                    {
                        dbc.ScheduleTable = dbc.DS.Tables["schedule"];
                        DataColumn[] PrimaryKey = new DataColumn[1];
                        PrimaryKey[0] = dbc.ScheduleTable.Columns["sc_id"];
                        dbc.ScheduleTable.PrimaryKey = PrimaryKey;
                        deleteRow = dbc.ScheduleTable.Rows.Find(selectId);
                        int rowCount = dbc.ScheduleTable.Rows.Count;  // 전체 행의 개수 (삭제전)
                        deleteRow.Delete();
                        int select = Convert.ToInt32(selectId);  //  selectId를 증감시킬경우 for문에 영향을 주므로 변수를 따로 지정해서 사용

                        for (int i = 0; i < (rowCount - Convert.ToInt32(selectId)); i++)  //  행 하나가 삭제될 경우 행의 인덱스가 상제 대상보다 높은경우 모두 -1 해줌
                        {
                            deleteRow = dbc.ScheduleTable.Rows[rowCount - (rowCount - select)];
                            deleteRow.BeginEdit();
                            deleteRow["sc_id"] = Convert.ToInt32(deleteRow["sc_id"]) - 1;
                            deleteRow.EndEdit();
                            select += 1;
                        }

                        dbc.DBAdapter.Update(dbc.DS, "schedule");
                        dbc.DS.AcceptChanges();

                        button2_Click(sender, e);
                        MessageBox.Show("삭제 완료", "알림");
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
