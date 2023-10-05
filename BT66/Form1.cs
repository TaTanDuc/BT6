using BT66.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT66
{
    public partial class Form1 : Form
    {   
        NhanVienEntities context = new NhanVienEntities();
        public Form1()
        {
            InitializeComponent();
        }

        public void update()
        {   
            listView1.Items.Clear();
            foreach (NV v in context.NVs.ToList())
            {
                ListViewItem item = new ListViewItem();
                item.Text = v.MaNV;
                item.SubItems.Add(v.TenNV);
                item.SubItems.Add(v.NgaySinh.ToString());
                item.SubItems.Add(v.PB.TenPB);
                listView1.Items.Add(item);
            }
        }

        public bool duyetMSNV(string duyet)
        {
            foreach(NV nv in context.NVs)
            {
                if(nv.MaNV.Equals(duyet))
                    return true;
            }
            return false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            update();

            List<PB> listPB = context.PBs.ToList();
            comboBox2.DataSource = listPB;
            comboBox2.DisplayMember = "TenPB";
            comboBox2.ValueMember = "MaPB";
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            textBox1.Text = listView1.SelectedItems[0].Text;
            textBox2.Text = listView1.SelectedItems[0].SubItems[1].Text;
            dateTimePicker1.Text = listView1.SelectedItems[0].SubItems[2].Text;
            comboBox2.Text = listView1.SelectedItems[0].SubItems[3].Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi!", MessageBoxButtons.OK);
                return;
            }
            if (duyetMSNV(textBox1.Text) == true)
            {
                MessageBox.Show("Đã tồn tại MSNV!", "Lỗi!", MessageBoxButtons.OK);
                return;
            }
            
            NV nv = new NV();
            nv.MaNV = textBox1.Text;
            nv.TenNV = textBox2.Text;
            nv.NgaySinh = dateTimePicker1.Value;
            nv.MaPB = comboBox2.SelectedValue.ToString();

            context.NVs.Add(nv);
            context.SaveChanges();
            update();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NV nv = context.NVs.FirstOrDefault(p => p.MaNV == textBox1.Text);
            DialogResult dr = MessageBox.Show("Bạn có muốn xóa!","Cảnh Báo!",MessageBoxButtons.OKCancel);
            if (nv != null && dr == DialogResult.OK)
            {
                context.NVs.Remove(nv);
                context.SaveChanges();
                update();
            }
            else
                return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NV nv = context.NVs.FirstOrDefault(p => p.MaNV == textBox1.Text);
            if (nv == null)
            {
                MessageBox.Show("Không tồn tại MSNV!", "Lỗi!", MessageBoxButtons.OK);
                return;
                
            }
            else if(MessageBox.Show("Bạn có muốn sửa!", "Cảnh Báo!", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                nv.TenNV = textBox2.Text;
                nv.NgaySinh = dateTimePicker1.Value;
                nv.MaPB = comboBox2.SelectedValue.ToString();

                context.SaveChanges();
                update();
            }
        }
    }
}
