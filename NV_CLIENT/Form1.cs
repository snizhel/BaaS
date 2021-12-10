using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NV_CLIENT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChitietNV newnhanvien = new ChitietNV()
            {
                 Manv = int.Parse(textBox2.Text.Trim()),
                 Ten = textBox3.Text.Trim(),
                 Chucvu = textBox4.Text.Trim(),
                 Phongban = textBox5.Text.Trim(),
                 Chuthich =textBox6.Text.Trim()
            };
            bool result = new NV_BUS().Addnew(newnhanvien);
            if (result)
            {
                MessageBox.Show("OK BAE!");
            }
            else
            {
                MessageBox.Show("SORRY BAE!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            new NV_BUS().ListenFirebase(dataGridView1);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                int manv = int.Parse(dataGridView1.SelectedRows[0].Cells["Manv"].Value.ToString());
                ChitietNV nhanvien = new NV_BUS().Getdetails(manv);
                if (nhanvien != null)
                {
                    textBox2.Text = nhanvien.Manv.ToString();
                    textBox3.Text = nhanvien.Ten;
                    textBox4.Text = nhanvien.Chucvu;
                    textBox5.Text = nhanvien.Phongban;
                    textBox6.Text = nhanvien.Chuthich;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String keyword = textBox1.Text.Trim();
            List<ChitietNV> nhanviens = new NV_BUS().Search(keyword);
            dataGridView1.BeginInvoke(new MethodInvoker(delegate{dataGridView1.DataSource = nhanviens;
            }));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChitietNV newnhanvien = new ChitietNV()
            {
                Manv = int.Parse(textBox2.Text.Trim()),
                Ten = textBox3.Text.Trim(),
                Chucvu = textBox4.Text.Trim(),
                Phongban = textBox5.Text.Trim(),
                Chuthich = textBox6.Text.Trim()
            };
            bool result = new NV_BUS().Update(newnhanvien);
            if (!result) MessageBox.Show("SORRY BABY!!!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("ARE U SURE?", "CONFIRMATION", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                int manv = int.Parse(textBox2.Text.Trim());
                bool result = new NV_BUS().Delete(manv);
                if (!result) MessageBox.Show("SORRY BABY!!!");
            }

        }
    }
}
