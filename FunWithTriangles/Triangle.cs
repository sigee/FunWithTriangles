using System;
using System.Drawing;
using System.Windows.Forms;

namespace FunWithTriangles
{
    public partial class Triangle : UserControl
    {
        public Triangle()
        {
            InitializeComponent();
            Width = 100;
            Height = 100;
        }

        public double EdgeA { get; set; }
        public double EdgeB { get; set; }
        public double EdgeC { get; set; }

        public bool IsConstructable()
        {
            if (EdgeA == 0 || EdgeB == 0 || EdgeC == 0)
            {
                return false;
            }

            if (EdgeA + EdgeB < EdgeC || EdgeA + EdgeC < EdgeB || EdgeB + EdgeC < EdgeA)
            {
                return false;
            }

            return true;
        }

        public double GetArea()
        {
            var s = (EdgeA + EdgeB + EdgeC) / 2;
            return Math.Sqrt(s * (s - EdgeA) * (s - EdgeB) * (s - EdgeC));
        }

        public double GetAlpha()
        {
            return CalculateAngle(EdgeB, EdgeC, EdgeA);
        }

        public double GetBeta()
        {
            return CalculateAngle(EdgeA, EdgeC, EdgeB);
        }

        public double GetGamma()
        {
            return CalculateAngle(EdgeA, EdgeB, EdgeC);
        }

        public Angle GetLargestAngle()
        {
            if (GetAlpha() >= GetBeta() && GetAlpha() >= GetGamma())
            {
                return Angle.Alpha;
            }

            if (GetBeta() >= GetAlpha() && GetBeta() >= GetGamma())
            {
                return Angle.Beta;
            }

            return Angle.Gamma;
        }

        private double CalculateAngle(double edge1, double edge2, double edge3)
        {
            var cos = (Math.Pow(edge1, 2) + Math.Pow(edge2, 2) - Math.Pow(edge3, 2)) / (2 * edge1 * edge2);
            return RadianToDegree(Math.Acos(cos));
        }

        private double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            var g = paintEventArgs.Graphics;
            g.FillRectangle(new SolidBrush(Color.DodgerBlue), 0, 0, Width, Height);
            g.DrawString(Text, new Font("Arial", 13), new SolidBrush(Color.Black), 10, 10, new StringFormat());
            // @TODO: Draw the triangle
            // @Note: The largest angle goes to the top
//            g.DrawLine();
        }

        public bool IsEqual(Triangle otherTriangle)
        {
            double tolerance = 0.001;
            return Math.Abs(GetLargestEdge() - otherTriangle.GetLargestEdge()) < tolerance &&
                   Math.Abs(GetSmallestEdge() - otherTriangle.GetSmallestEdge()) < tolerance &&
                   Math.Abs(GetTriangleHeight() - otherTriangle.GetTriangleHeight()) < tolerance;
        }

        private double GetLargestEdge()
        {
            if (EdgeA >= EdgeB && EdgeA >= EdgeC)
            {
                return EdgeA;
            }

            return EdgeB >= EdgeC ? EdgeB : EdgeC;
        }

        private double GetSmallestEdge()
        {
            if (EdgeA <= EdgeB && EdgeA <= EdgeC)
            {
                return EdgeA;
            }

            return EdgeB <= EdgeC ? EdgeB : EdgeC;
        }

        private double GetTriangleHeight()
        {
            var a = Math.Sqrt(EdgeA + EdgeB + EdgeC);
            var b = Math.Sqrt(-EdgeA + EdgeB + EdgeC);
            var c = Math.Sqrt(EdgeA - EdgeB + EdgeC);
            var d = Math.Sqrt(EdgeA + EdgeB - EdgeC);
            return (a * b * c * d) / 2 * GetLargestEdge();
        }
    }
}