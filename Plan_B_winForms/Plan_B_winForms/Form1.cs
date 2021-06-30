using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Cuda;


namespace Plan_B_winForms
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
           if (openFileDialog1.ShowDialog() == DialogResult.OK)
           {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
                
                /*var test = new Bitmap(openFileDialog1.FileName);
                var targetBitmap = new Bitmap(test.Width, test.Height, test.PixelFormat);
                for (int i = 0; i < test.Width; i++)
                {
                    for (int j = 0; j < test.Height; j++)
                    {
                        Color c = test.GetPixel(i, j);
                        
                        targetBitmap.SetPixel(i, j, Color.FromArgb(c.A, 255, 11, 11));
                    }
                }
                pictureBox1.Image = targetBitmap;*/

            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            var inputImage = new Bitmap(pictureBox1.Image);
           
            var outputImage = new Bitmap(inputImage.Width,inputImage.Height,inputImage.PixelFormat);
            for (int i = 0; i < inputImage.Width; i++)
            {
                for (int j = 0; j < inputImage.Height; j++)
                {
                    Color c = inputImage.GetPixel(i, j);
                    if (c.R > c.G && c.R > c.B)
                    {
                        double r = c.R;
                        int nr = (int)((r / 100) * 95);
                        double g = c.G;
                        int ng = (int)((g / 100) * 85);
                        double b = c.B;
                        int nb = (int)((b / 100) * 85);
                        if (c.R - 20 > c.G || c.R - 20 > c.B)
                        {
                            
                            outputImage.SetPixel(i, j, Color.FromArgb(c.A, nr, ng, nb));
                        }
                        else if (c.R - 20 > c.G && c.R - 20 > c.B)
                        {
                            
                            nr = ((c.R / 100) * 20);
                            outputImage.SetPixel(i, j, Color.FromArgb(c.A, nr, ng, nb));
                        }
                        else
                        {
                            nr = c.R;
                            outputImage.SetPixel(i, j, Color.FromArgb(c.A, nr, ng, nb));
                        }                    
                    }
                    else if(c.G > c.R && c.G > c.B)
                    {
                        double r = c.R;
                        int nr = (int)((r / 100) * 85);
                        double g = c.G;
                        int ng = (int)((g / 100) * 95);
                        double b = c.B;
                        int nb = (int)((b / 100) * 85);
                        if (c.G - 20 > c.R || c.G - 20 > c.B)
                        {
                            outputImage.SetPixel(i, j, Color.FromArgb(c.A, nr, ng, nb));
                        }
                        else
                        {
                            ng = c.G;
                            outputImage.SetPixel(i, j, Color.FromArgb(c.A, nr, ng, nb));
                        }
                    }
                    else if(c.B > c.R && c.B > c.G)
                    {
                        double r = c.R;
                        int nr = (int)((r / 100) * 85);
                        double g = c.G;
                        int ng = (int)((g / 100) * 85);
                        double b = c.B;
                        int nb = (int)((b / 100) * 95);
                        if (c.B - 20 > c.R || c.B - 20 > c.G)
                        {
                            if (c.B > 30 && c.R > 30 && c.G > 30)
                                outputImage.SetPixel(i, j, Color.FromArgb(c.A, nr, ng, nb));
                        }
                        else
                        {
                            //nb = c.B;
                            outputImage.SetPixel(i, j, Color.FromArgb(c.A, nr, ng, c.B));
                        }
                    }
                    else
                    {
                        double r = c.R;
                        int nr = (int)((r / 100) * 85);
                        double g = c.G;
                        int ng = (int)((g / 100) * 85);
                        double b = c.B;
                        int nb = (int)((b / 100) * 90);
                        outputImage.SetPixel(i, j, Color.FromArgb(c.A, nr, ng, nb));
                    }
                }
            }
            pictureBox1.Image = outputImage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Image<Bgr, byte> inptImg ;
            
            var mainImg = new Bitmap(pictureBox1.Image);
            Color middleGrey = mainImg.GetPixel(0,0);
            //поиск среднесерого
            for (int i = 0; i < mainImg.Width; i++)
            {

                for (int j = 0; j < mainImg.Height; j++)
                {
                    Color c = mainImg.GetPixel(i, j);
                    if ((c.R >= 90 && c.R <= 120) && (c.G >= 55 && c.G <= 65) && (c.B >= 55 && c.B <= 65))
                    {
                        //меняем местами красный и синий
                        middleGrey = Color.FromArgb(c.A / 2,c.B,c.G,c.R);
                    }
                    else //если средний не найден 
                    {
                        middleGrey = Color.FromArgb(90, 65, 65, 120);
                    }
                }
            }
            //создание фильтра
            var filter = new Bitmap(mainImg.Width, mainImg.Height);
            for (int i = 0; i < filter.Width; i++)
            {
                for (int j = 0; j < filter.Height; j++)
                {
                    filter.SetPixel(i, j, middleGrey);
                }
            }
            //наложение
            using (var graph = Graphics.FromImage(mainImg))
            {
                /*for (int i = 0; i < filter.Width; i++)
                {
                    for (int j = 0; j < filter.Height; j++)
                    {
                        var pixel = filter.GetPixel(i, j);
                        filter.SetPixel(i, j, Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B));
                    }
                }*/
                graph.DrawImage(filter, new Rectangle(0, 0, filter.Width, filter.Height));
            }
            pictureBox1.Image = mainImg;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
        }
    }
}
