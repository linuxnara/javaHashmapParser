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
        private String m1_str = String.Empty;
        private String m2_str = String.Empty;
        private String m3_str = String.Empty;
        private String m4_str = String.Empty;

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

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyDataGridViewToText("clipboard");
        }

        private void copyDataGridViewToText(String target)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                String copyText = "";
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    copyText += row.Cells["KEY"].Value + "=";
                    if (row.Cells["VALUE"].Value.ToString() == "")
                    {
                        copyText += ",\r\n";
                    }
                    else
                    {
                        copyText += row.Cells["VALUE"].Value + ",\r\n";
                    }

                }

                switch (target)
                {
                    case "clipboard":
                        Clipboard.SetText(copyText);
                        break;
                    case "m1":
                        m1_str = copyText;
                        break;
                    case "m2":
                        m2_str = copyText;
                        break;
                    case "m3":
                        m3_str = copyText;
                        break;
                    case "m4":
                        m4_str = copyText;
                        break;
                    default:
                        Clipboard.SetText(copyText);
                        break;
                }
                
            }
        }


        private void btnM1_Click(object sender, EventArgs e)
        {            
            if (m1_str == "")
            {
                copyDataGridViewToText("m1");                
            } else
            {
                textBox1.Text = m1_str.Replace("\r\n", "");
                button1_Click(sender, e);
            }
        }

        private void btnM2_Click(object sender, EventArgs e)
        {
            if (m2_str == "")
            {
                copyDataGridViewToText("m2");

            } else
            {
                textBox1.Text = m2_str.Replace("\r\n", "");
                button1_Click(sender, e);
            }
        }

        private void btnM3_Click(object sender, EventArgs e)
        {            
            if (m3_str == "")
            {
                copyDataGridViewToText("m3");
                
            } else
            {
                textBox1.Text = m3_str.Replace("\r\n", "");
                button1_Click(sender, e);
            }
        }

        private void btnM4_Click(object sender, EventArgs e)
        {            
            if (m4_str == "")
            {
                copyDataGridViewToText("m4");
                
            } else
            {
                textBox1.Text = m4_str.Replace("\r\n", "");
                button1_Click(sender, e);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (cbCacheName.SelectedIndex == -1) return;

            switch (cbCacheName.SelectedIndex)
            {
                case 0:
                    m1_str = string.Empty;
                    m2_str = string.Empty;
                    m3_str = string.Empty;
                    m4_str = string.Empty;
                    break;
                case 1:
                    m1_str = string.Empty;
                    break;
                case 2:
                    m2_str = string.Empty;
                    break;
                case 3:
                    m3_str = string.Empty;
                    break;
                case 4:
                    m4_str = string.Empty;
                    break;
                default:
                    break;
            }

        }
    }
}
