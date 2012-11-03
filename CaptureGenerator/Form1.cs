using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace CaptureGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<string> strings = new List<string>();
        private void btn_st_Click(object sender, EventArgs e)
        {
            try
            {
                Image[] images = GenerateCaptchares(Convert.ToInt32(textBox1.Text));
                int g = 0;
                foreach (Image image in images)
                {
                    image.Save(label1.Text + "\\" + strings[g] + ".png");
                    ++g;
                }
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex);
            }

        }
        Image[] GenerateCaptchares(int amount)
        {
            Random ren = new Random();
            Image[] images = new Image[amount];
            for (int z = 0; z < amount; z++)
            {
                Bitmap bitmap = new Bitmap(panel1.Width, panel1.Height);
                Graphics g = Graphics.FromImage(bitmap);
                g.Clear(panel1.BackColor);

                SolidBrush b = new SolidBrush(Color.FromArgb(0xFF, ren.Next(0, 255), ren.Next(0, 255), ren.Next(0, 255)));
                Pen p = new Pen(Color.FromArgb(0xFF, ren.Next(0, 255), ren.Next(0, 255), ren.Next(0, 255)));
                char[] chars = "qwertyuiopasdfghjklzxcvbnm0123456789".ToCharArray();
                string randomString = "";
                for (int i = 0; i < 6; i++)
                {
                    randomString += chars[ren.Next(0, 35)];
                }
                byte[] buffer = new byte[randomString.Length];
                int y = 0;
                foreach (char c in randomString.ToCharArray())
                {
                    buffer[y] = (byte)c;
                    ++y;
                }
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                string md5String = BitConverter.ToString(md5.ComputeHash(buffer)).Replace("-", "");
                strings.Add(md5String);
                FontFamily ff = new FontFamily("Arial");
                Font f = new Font(ff, 14);
                g.DrawString(randomString, f, b, 20, 20);
                for (int i = 0; i < 6; i++)
                {
                    int j = ren.Next(0, 2);
                    if (j == 0) g.DrawRectangle(p, ren.Next(0, 181), ren.Next(0, 100), ren.Next(0, 111), ren.Next(0, 60));
                    else
                    {
                        if (j == 1) g.DrawEllipse(p, ren.Next(0, 181), ren.Next(0, 100), ren.Next(0, 111), ren.Next(0, 60));
                    }
                    p.Color = Color.FromArgb(0xFF, ren.Next(0, 255), ren.Next(0, 255), ren.Next(0, 255));
                }
                panel1.BackgroundImage = bitmap;
                images[z] = bitmap;
            }
            return images;
        }

        private void btn_ch_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                label1.Text = fbd.SelectedPath;
            }
        }

        private string md5HashName = "";
        private void btn_open_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = ofd.FileName;
                md5HashName = Path.GetFileNameWithoutExtension(ofd.FileName);
            }
        }

        private void btn_sbm_Click(object sender, EventArgs e)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            int y = 0;
            byte[] buffer = new byte[textBox2.Text.Length];

            foreach (char c in textBox2.Text.ToCharArray())
            {
                buffer[y] = (byte)c;
                ++y;
            }
            string blach = BitConverter.ToString(md5.ComputeHash(buffer)).Replace("-", "");
            if (blach != md5HashName)
            {
                MessageBox.Show("You got it wrong!");
            }
            else
            {
                MessageBox.Show("You got it right!");
            }
        }
    }
}
