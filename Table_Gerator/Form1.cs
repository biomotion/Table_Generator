﻿using System;
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
    public partial class clearButton : Form
    {
        UInt16 cols, rows;
        List<List<Color>> colors;
        public clearButton()
        {
            InitializeComponent();
            cols = UInt16.Parse(col_text.Text);
            rows = UInt16.Parse(row_text.Text);
            tableLayoutPanel1.Height = this.Height - 56;
            pictureBox1.Width = this.Width - tableLayoutPanel1.Width - 32;
            pictureBox1.Height = this.Height - 56;

            colors = new List<List<Color>>();
            InitColors();
            UpdatePicture();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            UpdatePicture();
        }

        void UpdateInfo()
        {
            if(colors == null) { return; }
            if(colors.Count != cols)
            {
                Console.WriteLine("update cols");
                InitColors();
            }
            if(colors[0].Count != rows)
            {
                Console.WriteLine("update rows");
                InitColors();
            }
        }

        void InitColors()
        {
            colors.Clear();
            for (UInt16 i = 0; i < cols; i++)
            {
                colors.Add(new List<Color>());
                for (UInt16 j = 0; j < rows; j++)
                {
                    colors[i].Add(Color.FromName("Black"));
                }
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            tableLayoutPanel1.Height = this.Height - 56;
            pictureBox1.Width = this.Width - tableLayoutPanel1.Width - 32;
            pictureBox1.Height = this.Height - 56;
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

        private void pen_color_Click(object sender, EventArgs e)
        {
            ColorDialog colorChooser = new ColorDialog();
            if (colorChooser.ShowDialog() == DialogResult.OK)
                ((Button)sender).BackColor = colorChooser.Color;
        }

        private void pictureBox1_Draw(object sender, MouseEventArgs e)
        {
            int ix = e.Location.X * (int)cols / pictureBox1.Width;
            int iy = e.Location.Y * (int)rows / pictureBox1.Height;
            if (e.Button == MouseButtons.Left)
            {
                Console.WriteLine("left x = {0}, y = {1}", ix, iy);
                if(ix >= 0 && ix < cols && iy >= 0 && iy < rows)
                    colors[ix][iy] = pen_color_left.BackColor;

            }
            else if (e.Button == MouseButtons.Right)
            {
                Console.WriteLine("right x = {0}, y = {1}", ix, iy);
                if (ix >= 0 && ix < cols && iy >= 0 && iy < rows)
                    colors[ix][iy] = pen_color_right.BackColor;
            }
            UpdatePicture();
        }

        private void color_btns_MouseClick(object sender, MouseEventArgs e)
        {
            //Console.WriteLine(e.Button.ToString());
            if (e.Button == MouseButtons.Left)
                pen_color_left.BackColor = ((Button)sender).BackColor;
            else if (e.Button == MouseButtons.Right)
                pen_color_right.BackColor = ((Button)sender).BackColor;
        }

        private void exportButton_MouseClick(object sender, MouseEventArgs e)
        {
            String result = "{\n";
            for (UInt16 i = 0; i < cols; i++)
            {
                result += "  {";
                for (UInt16 j = 0; j < rows; j++)
                {
                    Color currentColor = colors.ElementAt(i).ElementAt(j);
                    result += "{" + currentColor.R + "," + currentColor.G + "," + currentColor.B + "}";
                    if (j != rows - 1)
                        result += ",";
                }
                if (i != cols)
                    result += "},\n";
                else
                    result += "}\n";
            }
            result += "}";
            Console.WriteLine(result);
            Clipboard.SetText(result);
            MessageBox.Show(this, "The picture table is already in your clipboard.", "Generate Success");
        }

        private void fileButton_MouseClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Select an Image"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show(this, dialog.FileName);

            }
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            InitColors();
            UpdatePicture();
        }

        private void ImportButton_MouseClick(object sender, MouseEventArgs e)
        {
            String inputText = Clipboard.GetText();
            String[] allLine = inputText.Split('\n');
            Console.WriteLine(allLine.Length);


        }

        void UpdatePicture()
        {
            UpdateInfo();
            Image image = pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height); ;
            Graphics g = Graphics.FromImage(image);
            Pen linePen = new Pen(Brushes.Gray);
            float col_width = (float)pictureBox1.Width / cols,
                  row_height = (float)pictureBox1.Height / rows;

            for(UInt16 i = 0; i<cols && i < colors.Count; i++)
            {
                for(UInt16 j=0; j<rows && j<colors[i].Count; j++)
                {
                    g.FillRectangle(new SolidBrush( colors[i][j]), i * col_width, j * row_height, col_width, row_height);
                }
            }

            for (UInt16 i = 0; i < cols; i++)
                g.DrawLine(linePen, col_width * i, 0, col_width * i, pictureBox1.Height);
            for (UInt16 i = 0; i < rows; i++)
                g.DrawLine(linePen, 0, row_height * i, pictureBox1.Width, row_height * i);
            g.DrawLine(linePen, pictureBox1.Width-1, 0, pictureBox1.Width-1, pictureBox1.Height);
            g.DrawLine(linePen, 0, pictureBox1.Height-1, pictureBox1.Width, pictureBox1.Height-1);
        }
    }
}
