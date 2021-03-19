using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace javaHashmapParser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            String parseString = @"{a=0, b=1000, c=가나다라마바사}";
            //DataTable ret = toDictionary(parseString);

            dataGridView1.DefaultCellStyle.Font = new Font("Tahoma", 8);
            dataGridView1.DefaultCellStyle.ForeColor = Color.Blue;
            dataGridView1.DefaultCellStyle.BackColor = Color.Beige;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Yellow;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Black;
            textBox1.Text = parseString;
        }

        public DataTable toDictionary(String parseString)
        {
            DataTable dict = new DataTable();
            dict.Columns.Add("KEY", typeof(String));
            dict.Columns.Add("VALUE", typeof(String));

            if (parseString == "") return null;

            parseString = parseString.Trim();
            //if (parseString.Substring(0, 1) != "{" &&
            //        parseString.Substring(parseString.Length - 1, 1) != "}") {
            //    return null;
            //}
            //parseString = parseString.Substring(1, parseString.Length - 2);
            
            String[] splitData = parseString.Split(new String[] { tbSplit.Text }, StringSplitOptions.RemoveEmptyEntries);
            for(int i = 0; i < splitData.Length; i++)
            {                
                String[] splitItem = splitData[i].Split(new String[] { tbDivide.Text }, StringSplitOptions.None);
                if(splitItem.Length == 2)
                {
                    DataRow dr = dict.NewRow();
                    dr[0] = splitItem[0].Trim();
                    dr[1] = splitItem[1].Trim();                    
                    dict.Rows.Add(dr);
                } else
                {
                    DataRow dr = dict.NewRow();
                    dr[0] = splitItem[0].Trim();
                    dr[1] = "";                    
                    dict.Rows.Add(dr);
                }
            }

            return dict;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();

            DataTable dtRet = toDictionary(textBox1.Text);
            dataGridView1.DataSource = dtRet;
            
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.BorderStyle = BorderStyle.Fixed3D;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;


            DataGridViewButtonColumn btnEncoding = new DataGridViewButtonColumn();
            btnEncoding.HeaderText = "Encoding";
            btnEncoding.Name = "ENC";
            dataGridView1.Columns.Insert(1, btnEncoding);

            DataGridViewButtonColumn btnDecoding = new DataGridViewButtonColumn();
            btnDecoding.HeaderText = "Decoding";
            btnDecoding.Name = "DES";
            dataGridView1.Columns.Insert(2, btnDecoding);

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[1].Value = "ENC";
                row.Cells[2].Value = "DES";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable ret = (DataTable)dataGridView1.DataSource;

            IEnumerable<DataRow> queryKey = from p in ret.AsEnumerable()
                                            where p.Field<String>("KEY").StartsWith(textBox2.Text.Trim())
                                            select p;
            try
            {
                //dataGridView1.Columns.Clear();
                dataGridView1.DataSource = queryKey.CopyToDataTable<DataRow>();

                if (dataGridView1.Columns["ENC"] != null)
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        row.Cells["ENC"].Value = "ENC";
                        row.Cells["DES"].Value = "DES";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataTable ret = (DataTable)dataGridView1.DataSource;

            IEnumerable<DataRow> queryKey = from p in ret.AsEnumerable()
                                            where p.Field<String>("VALUE").StartsWith(textBox3.Text.Trim())
                                            select p;
            try
            {
                //dataGridView1.Columns.Clear();
                dataGridView1.DataSource = queryKey.CopyToDataTable<DataRow>();

                if (dataGridView1.Columns["ENC"] != null)
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        row.Cells["ENC"].Value = "ENC";
                        row.Cells["DES"].Value = "DES";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == -1 || e.RowIndex == -1) return;

            if(dataGridView1.Columns[e.ColumnIndex].Name == "ENC")
            {
                String value = dataGridView1["VALUE", e.RowIndex].Value.ToString();
                dataGridView1["VALUE", e.RowIndex].Value = System.Web.HttpUtility.UrlEncode(value);
            }
            else if(dataGridView1.Columns[e.ColumnIndex].Name == "DES")
            {
                String value = dataGridView1["VALUE", e.RowIndex].Value.ToString();
                dataGridView1["VALUE", e.RowIndex].Value = System.Web.HttpUtility.UrlDecode(value);
            }
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["ENC"].Value = "ENC";
                row.Cells["DES"].Value = "DES";
            }
        }
    }
}
