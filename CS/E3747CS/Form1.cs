using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel;

namespace FilterMasterDetailGrid
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            BindingSource bs = new BindingSource();
            bs.DataSource = CreateObjectSource(3, 5);
            gridControl1.DataSource = bs;
            new FilterHelper(gridView1, gridView2);

        }

        BindingList<Record> CreateObjectSource(int master, int detail)
        {
            BindingList<Record> list = new BindingList<Record>();
            for (int i = 0; i < master; i++)
            {
                Record rec = new Record();
                rec.Value = "Master " + i;
                rec.Detail = new BindingList<Record>();
                for (int j = 0; j < detail; j++)
                {
                    Record childRec = new Record();
                    childRec.Value = "Child "+i +" "+ + j;
                    rec.Detail.Add(childRec);
                }
                list.Add(rec);
            }
            return list;
        }

        class Record
        {
            string _value;
            BindingList<Record> _detail;
            public string Value
            { 
                get {return _value; }
                set { _value = value; }
            }

            public BindingList<Record> Detail {
                get { return _detail; }
                set { _detail = value; }
            }
        }


        private DataTable CreateTable(int RowCount)
        {
            DataTable tbl = new DataTable("Parent");
            tbl.Columns.Add("Name", typeof(string));
            tbl.Columns.Add("Number", typeof(int));
            tbl.Columns.Add("Date", typeof(DateTime));
            tbl.Columns.Add("DETAILID", typeof(int));
            for (int i = 0; i < RowCount; i++)
                tbl.Rows.Add(new object[] { String.Format("Name{0}", i), 3 - i, DateTime.Now.AddDays(i), i });
            return tbl;
        }

        private DataTable CreateDet1Table(int RowCount)
        {
            DataTable tbl = new DataTable("Det1");
            tbl.Columns.Add("Name", typeof(string));
            tbl.Columns.Add("ID", typeof(int));
            for (int j = 0; j < 5; j++)
                for (int i = 0; i < RowCount; i++)
                    tbl.Rows.Add(new object[] { String.Format("Name{0}", j + (i % 2) * 5), i });
            return tbl;
        }

        private DataSet GetMasterDetail()
        {
            DataSet ds = new DataSet("TestDS");
            ds.Tables.Add(CreateTable(20));
            ds.Tables.Add(CreateDet1Table(20));
            DataColumn parentColumn = ds.Tables["Parent"].Columns["DETAILID"];
            DataColumn childColumn = ds.Tables["Det1"].Columns["ID"];
            ds.Relations.Add(new DataRelation("Detail", parentColumn, childColumn));
            return ds;
        }

        private void gridView2_ColumnFilterChanged(object sender, EventArgs e)
        {
            gridView1.DataController.RefreshData();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            gridView2.ActiveFilter.Clear();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            gridControl1.DataSource = GetMasterDetail().Tables["Parent"];
            gridView1.PopulateColumns();
            gridView2.PopulateColumns();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            gridControl1.DataSource = CreateObjectSource(3, 5);
            gridView1.PopulateColumns();
            gridView2.PopulateColumns();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = CreateObjectSource(3, 5);
            gridControl1.DataSource = bs;
            gridView1.PopulateColumns();
            gridView2.PopulateColumns();
        }
    }
}
