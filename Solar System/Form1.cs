using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Solar_System
{
    public partial class Form1 : Form
    {

        class Satellite
        {
            // encapsulated data
            int or;
            int r;
            Color c;
            double a;
            double s;
            // constructor(s)
            public Satellite(int OrbitRadius, int Radius, Color Color, double Speed)
            {
                or = OrbitRadius;
                r = Radius;
                c = Color;
                a = 0;
                s = Speed;
            }
            // interface(s)
            public int OrbitRadius
            {
                get { return or; }
            }
            // private methods
            private Color Dim(Color Color, double Ratio)
            {
                int Red = Convert.ToInt32(Color.R * Ratio);
                int Green = Convert.ToInt32(Color.G * Ratio);
                int Blue = Convert.ToInt32(Color.B * Ratio);
                return Color.FromArgb(Red, Green, Blue);
            }
            private void DrawSphere(Bitmap View, float x, float y, float radius, Color Color, double Exponent)
            {
                int LeftX = Convert.ToInt32(x - radius - 1);
                int RightX = Convert.ToInt32(x + radius + 1);
                int TopY = Convert.ToInt32(y - radius - 1);
                int BottomY = Convert.ToInt32(y + radius + 1);
                for (int x1 = LeftX; x1 <= RightX; x1++)
                    for (int y1 = TopY; y1 <= BottomY; y1++)
                    {
                        double distance = Math.Sqrt((x - x1) * (x - x1) + (y - y1) * (y - y1));
                        if (distance <= radius)
                        {
                            double p = distance / radius;
                            double intensity = 1 - Math.Pow(p, Exponent);
                            View.SetPixel(x1, y1, Dim(Color, intensity));
                        }
                    }
            }
            // public methods
            public void Move()
            {
                a += s;
            }
            public void Draw(Bitmap b, int x, int y)
            {
                int SatelliteX = x + Convert.ToInt32(Math.Cos(a) * or);
                int SatelliteY = y + Convert.ToInt32(Math.Sin(a) * or);
                DrawSphere(b, SatelliteX, SatelliteY, r, c, 3.5);
            }
        }


        System.Collections.ArrayList Satellites;



        public Form1()
        {
            InitializeComponent();

            Satellites = new System.Collections.ArrayList();

            // Size picturebox to fill interior of form
            pictureBox1.Width = this.Width - 16;
            pictureBox1.Height = this.Height - 38;

            Satellite Earth = new Satellite(200,8,Color.Blue,0.005);
            Satellites.Add(Earth);

            //Satellite Mars = new Satellite(250, 5, Color.Red, 0.003);
            Satellites.Add(new Satellite(250, 5, Color.Red, 0.003)); // Mars
            Satellites.Add(new Satellite(80, 3, Color.Gold, 0.01)); // Mercury

            // Turn on timer
            timer1.Enabled = true;
        }



        Color Dim(Color Color, double Ratio)
        {
            int Red = Convert.ToInt32(Color.R * Ratio);
            int Green = Convert.ToInt32(Color.G * Ratio);
            int Blue = Convert.ToInt32(Color.B * Ratio);
            return Color.FromArgb(Red, Green, Blue);
        }



        void DrawSphere(Bitmap View, float x, float y, float radius, Color Color, double Exponent)
        {
            int LeftX = Convert.ToInt32(x - radius - 1);
            int RightX = Convert.ToInt32(x + radius + 1);
            int TopY = Convert.ToInt32(y - radius - 1);
            int BottomY = Convert.ToInt32(y + radius + 1);
            for (int x1 = LeftX; x1<=RightX; x1++)
                for (int y1 = TopY; y1 <= BottomY; y1++)
                {
                    double distance = Math.Sqrt((x - x1) * (x - x1) + (y - y1) * (y - y1));
                    if (distance <= radius)
                    {
                        double p = distance / radius;
                        double intensity = 1 - Math.Pow(p, Exponent);
                        View.SetPixel(x1, y1, Dim(Color, intensity));
                    }
                }
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            // Move satellites
            foreach (Satellite s in Satellites)
            {
                s.Move();
            }

            // Create bitmap and draw background
            Bitmap View = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(View);
            SolidBrush brush = new SolidBrush(Color.Black);
            g.FillRectangle(brush, 0, 0, View.Width, View.Height);

            // Draw sun
            int x = View.Width / 2;
            int y = View.Height / 2;
            //g.FillEllipse(new SolidBrush(Color.Yellow), x - 35, y - 35, 70, 70);
            DrawSphere(View, x, y, 35, Color.Yellow, 4.0);

            foreach (Satellite s in Satellites)
            {
                g.DrawEllipse(new Pen(Color.FromArgb(64,64,64)), x - s.OrbitRadius, y - s.OrbitRadius, 2 * s.OrbitRadius, 2 * s.OrbitRadius);
            }

            // Draw satellites
            foreach (Satellite s in Satellites)
            {
                s.Draw(View, x, y);
            }

            // Display new "frame" in picturebox
            pictureBox1.Image = View;
        }

        

    }
}
