using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using System.Data;
using System.Windows.Forms;

namespace Login.cs
{
    class DBClass
    {
        string connectionString = " User Id =" + id + "; Password =" + pass + "; Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = "+host+")(PORT =" + port + "))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = xe )));";
        string commandString;
        private int selectedRowInedex;
        private int selectedKeyValue;
        OracleDataAdapter dBAdapter;
        DataSet dS;
        OracleCommandBuilder myCommandBuilder;
        DataTable productTable, countUpdateTable, scheduleTable, memberTable, alarmTable;
        int result = 1;

        private static string id = "wpdlem2";
        private static string pass = "wpdlem2";
        private static int port = 1522;
        private static string host = "localhost";

        private string excelForder = @"C:\excelfile\";  // 엑셀파일 저장 경로
        public string ExcelForder
        {
            get { return excelForder; }
        }

        // 프로퍼티
        public OracleDataAdapter DBAdapter
        {
            get { return dBAdapter; }
            set { dBAdapter = value; }
        }
        public DataSet DS
        {
            get { return dS; }
            set { dS = value; }
        }
        public OracleCommandBuilder MyCommandBuilder
        {
            get { return myCommandBuilder; }
            set { myCommandBuilder = value; }
        }
        public DataTable ProductTable
        {
            get { return productTable; }
            set { productTable = value; }
        }
        public DataTable CountUpdateTable
        {
            get { return countUpdateTable; }
            set { countUpdateTable = value; }
        }
        public DataTable ScheduleTable
        {
            get { return scheduleTable; }
            set { scheduleTable = value; }
        }
        public DataTable MemberTable
        {
            get { return memberTable; }
            set { memberTable = value; }
        }
        public DataTable AlarmTable
        {
            get { return alarmTable; }
            set { alarmTable = value; }
        }
        public int SelectedRowIndex
        {
            get { return selectedRowInedex; }
            set { selectedRowInedex = value; }
        }
        public int Result
        {
            get { return result; }
        }

        // 상품 전체
        public void PDB_Open()
        {
            try
            {
                commandString = "select * from product1 order by id asc";
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "product1");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }
        // 타입별 검색(입고,출고)
        public void PDB_Open(int i)
        {
            try
            {
                commandString = "select id, pro_name, price, count, type from product1 where type = " + i + "order by id asc";
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "product1");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }
        // 재고수량 검색
        public void Search_Count(int num)
        {
            try
            {
                commandString = "select id, pro_name, price, count, type from product1 where count <="+num +"order by id asc";
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "product1");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }
        // 상품명 검색
        public void Search_Produt(string name)
        {
            try
            {
                commandString = "select id, pro_name, price, count, type from product1 where pro_name like  '%"+name+"%'  order by id asc";
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "product1");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }
        // 상품 가격순 오름,내림차순 정렬
        public void PDB_Price()
        {
            string sc = "";
            
             if (result == 1)
            {
                sc = "asc";
                result = 0;
            }
            else if (result == 0)
            {
                sc = "desc";
                result = 1;
            }
             try
             {
                commandString = "select * from product1 order by price " + sc;
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "product1");
             }
             catch (DataException DE)
             {
                 MessageBox.Show(DE.Message);
             }
        }

        // 등록번호 오름차순
        public void CDB_Open()
        {
            try
            {
                commandString = "select * from countupdate order by up_id asc";
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "countupdate");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }
        // 날짜별 조회 등록순 
        public void CDB_Open1(string date, int type)
        {
            try
            {
                commandString = "select u.up_date, p.pro_name ,sum(u.count), sum(p.price * u.count), p.id, u.type from product1 p, countupdate u where u.pro_id = p.id and u.type ="+type+" and (u.up_date like '%"+date+"%') group by u.up_date, p.pro_name, p.id, u.type";
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "countupdate");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }
         // 날짜별 조회 결산
        public void CDB_Open2(string date, int type)
        {
            try
            {
                commandString = "select up_id, p.pro_name, u.count, (p.price*u.count) ,u.up_date, u.type from countupdate u, product1 p where u.pro_id = p.id and u.type ="+type+" and (u.up_date like '%"+date+"%') order by u.up_id desc";
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "countupdate");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }
        // 상품명 등록순
        public void CDB_Name1(string name, int type)
        {
            try
            {
                commandString = "select up_id, p.pro_name, u.count, (p.price*u.count) ,u.up_date, u.type from countupdate u, product1 p where u.pro_id = p.id and u.type ="+type+" and p.pro_name like '%"+name+"%' order by u.up_id desc";
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "countupdate");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }
        // 상품명 결산
        public void CDB_Name2(string name, int type)
        {
            try
            {
                commandString = "select u.up_date, p.pro_name ,sum(u.count), sum(p.price * u.count), p.id, u.type from product1 p, countupdate u where u.pro_id = p.id and u.type ="+type+" and p.pro_name like '%"+name+"%' group by u.up_date, p.pro_name, p.id, u.type";
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "countupdate");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }
        // 상품번호 등록순
        public void CDB_ProNum1(int proNum, int type)
        {
            try
            {
                commandString = "select up_id, p.pro_name, u.count, (p.price*u.count) ,u.up_date, u.type from countupdate u, product1 p where u.pro_id = p.id and u.type ="+type+" and p.id = "+proNum+" order by u.up_id desc";
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "countupdate");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }
        // 상품번호 결산
        public void CDB_ProNum2(int proNum, int type)
        {
            try
            {
                commandString = "select u.up_date, p.pro_name ,sum(u.count), sum(p.price * u.count), p.id, u.type from product1 p, countupdate u where u.pro_id = p.id and u.type ="+type+" and p.id = "+proNum+" group by u.up_date, p.pro_name, p.id, u.type";
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "countupdate");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }

        // 최근 등록 건별
        public void CDB_Lately(int rowNum, int type)
        {
            try
            {
                commandString = "select * from ( select up_id, p.pro_name, u.count, (p.price*u.count) ,u.up_date, u.type  from countupdate u, product1 p  where u.pro_id = p.id and u.type = "+type+" order by u.up_id desc ) where rownum <= "+rowNum;
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "countupdate");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }

        // 스케쥴 검색 오름차순
        public void SDB_Open()
        {
            try
            {
                commandString = "select * from schedule order by sc_id asc";
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "schedule");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }

        // 회원정보 전체 
        public void MDB_Open()
        {
            try
            {
                commandString = "select * from member order by mem_id asc";
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "member");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }

        // 회원명으로 검색
        public void MDB_Name(string name)
        {
            try
            {
                commandString = "select * from member where  mem_name like '%"+name+"%' order by mem_id asc";
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "member");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }

        // 회원 전화번호로 검색
        public void MDB_Phone(string phone)
        {
            try
            {
                commandString = "select * from member where  mem_phone like '%"+phone+"%' order by mem_id asc";
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "member");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }

        // 회원 번호로 검색 ( 정보수정 이후 뷰 띄우기 )
        public void MDB_Id(int num)
        {
            try
            {
                commandString = "select * from member where  mem_id like '%"+num+"%' order by mem_id asc";
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "member");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }

        // 알람시간
        public void Alarm_Open()
        {
            try
            {
                commandString = "select time from Alarm";
                DBAdapter = new OracleDataAdapter(commandString, connectionString);
                MyCommandBuilder = new OracleCommandBuilder(DBAdapter);
                DS = new DataSet();
                DBAdapter.Fill(DS, "Alarm");
            }
            catch (DataException DE)
            {
                MessageBox.Show(DE.Message);
            }
        }
    }
}
