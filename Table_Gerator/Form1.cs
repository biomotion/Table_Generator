using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Table_Gerator
{
    public partial class MainForm : Form
    {
        UInt16 cols, rows;
        public MainForm()
        {
            InitializeComponent();
            cols = UInt16.Parse(col_text.Text);
            rows = UInt16.Parse(row_text.Text);
            tableLayoutPanel1.Height = this.Height - 56;
            pictureBox1.Width = this.Width - tableLayoutPanel1.Width - 32;
            pictureBox1.Height = this.Height - 56;
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            UpdatePicture();
        }

        void UpdateInfo()
        {

        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            tableLayoutPanel1.Height = this.Height - 56;
            pictureBox1.Width = this.Width - tableLayoutPanel1.Width - 32;
            pictureBox1.Height = this.Height - 56;
            UpdatePicture();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdatePicture();
        }


        private void col_text_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (UInt16.TryParse(col_text.Text, out cols))
                    UpdatePicture();
            }
        }

        private void row_text_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (UInt16.TryParse(row_text.Text, out rows))
                    UpdatePicture();
            }
        }

        private void col_inc_Click(object sender, EventArgs e)
        {
            cols++;
            col_text.Text = cols.ToString();
            UpdatePicture();
            
        }

        private void col_dec_Click(object sender, EventArgs e)
        {
            if (cols > 1)
            {
                cols--;
                col_text.Text = cols.ToString();
                UpdatePicture();
                
            }
        }

        private void row_inc_Click(object sender, EventArgs e)
        {
            rows++;
            row_text.Text = rows.ToString();
            UpdatePicture();
        }

        private void row_dec_Click(object sender, EventArgs e)
        {
            if (rows > 1)
            {
                rows--;
                row_text.Text = rows.ToString();
                UpdatePicture();
            }
        }


        void UpdatePicture()
        {
            UpdateInfo();
            Image image = pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height); ;
            Graphics g = Graphics.FromImage(image);
            Pen linePen = new Pen(Brushes.Gray);
            float col_width = (float)pictureBox1.Width / cols,
                  row_height = (float)pictureBox1.Height / rows;
            for (UInt16 i = 0; i < cols; i++)
                g.DrawLine(linePen, col_width * i, 0, col_width * i, pictureBox1.Height);
            for (UInt16 i = 0; i < rows; i++)
                g.DrawLine(linePen, 0, row_height * i, pictureBox1.Width, row_height * i);
            g.DrawLine(linePen, pictureBox1.Width-1, 0, pictureBox1.Width-1, pictureBox1.Height);
            g.DrawLine(linePen, 0, pictureBox1.Height-1, pictureBox1.Width, pictureBox1.Height-1);
        }
    }
}
