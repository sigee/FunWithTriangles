using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FunWithTriangles
{
    public partial class Triangle : UserControl
    {
        private new const int Padding = 20;

        public Triangle()
        {
            InitializeComponent();
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

            return !(EdgeA + EdgeB < EdgeC) && !(EdgeA + EdgeC < EdgeB) && !(EdgeB + EdgeC < EdgeA);
        }

        public double GetPerimeter()
        {
            if (!IsConstructable()) return 0.0;
            return EdgeA + EdgeB + EdgeC;
        }

        public double GetArea()
        {
            if (!IsConstructable()) return 0.0;
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

        public bool IsEqual(Triangle otherTriangle)
        {
            const double tolerance = 0.001;
            return Math.Abs(GetLargestEdge() - otherTriangle.GetLargestEdge()) < tolerance &&
                   Math.Abs(GetSmallestEdge() - otherTriangle.GetSmallestEdge()) < tolerance &&
                   Math.Abs(GetTriangleHeight() - otherTriangle.GetTriangleHeight()) < tolerance;
        }

        private double CalculateAngle(double edge1, double edge2, double edge3)
        {
            var cos = (Math.Pow(edge1, 2) + Math.Pow(edge2, 2) - Math.Pow(edge3, 2)) / (2 * edge1 * edge2);
            return RadianToDegree(Math.Acos(cos));
        }

        private Angle GetSmallestAngle()
        {
            if (GetAlpha() < GetBeta() && GetAlpha() < GetGamma())
            {
                return Angle.Alpha;
            }

            return GetBeta() < GetGamma() ? Angle.Beta : Angle.Gamma;
        }

        private Angle GetMediumAngle()
        {
            var angles = new List<Angle> {Angle.Alpha, Angle.Beta, Angle.Gamma};
            angles.Remove(GetSmallestAngle());
            angles.Remove(GetLargestAngle());
            return angles[0];
        }

        private Angle GetLargestAngle()
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

        private double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            var g = paintEventArgs.Graphics;
            var brush = new SolidBrush(Color.Black);
            var font = new Font("Arial", 12);
            var format = new StringFormat();
            if (!IsConstructable())
            {
                g.DrawString("A háromszög nem szerkeszthető meg!", font, brush, 0, 0, format);
                return;
            }

            var pen = new Pen(Color.Black);

            var corner1 = CalculateFirstCorner();
            var corner2 = CalculateSecondCorner();
            var corner3 = CalculateThirdCorner();
            g.DrawLine(pen, corner1, corner2);
            g.DrawLine(pen, corner1, corner3);
            g.DrawLine(pen, corner2, corner3);

            g.DrawString(GetCornerAt(GetMediumAngle()), font, brush, corner1.X - 20, corner1.Y, format);
            g.DrawString(GetCornerAt(GetSmallestAngle()), font, brush, corner2, format);
            g.DrawString(GetCornerAt(GetLargestAngle()), font, brush, corner3.X - 10, corner3.Y - 20, format);

            var lineCenter1 = CalculateFirstLineCenter();
            var lineCenter2 = CalculateSecondLineCenter();
            var lineCenter3 = CalculateThirdLineCenter();
            g.DrawString(GetCornerAt(GetSmallestAngle()).ToLower(), font, brush, lineCenter1, format);
            g.DrawString(GetCornerAt(GetMediumAngle()).ToLower(), font, brush, lineCenter2, format);
            g.DrawString(GetCornerAt(GetLargestAngle()).ToLower(), font, brush, lineCenter3, format);
        }

        private Point CalculateFirstCorner()
        {
            return new Point(Padding, Height - Padding);
        }

        private Point CalculateSecondCorner()
        {
            return new Point(Width - Padding, Height - Padding);
        }

        private Point CalculateThirdCorner()
        {
            var percentageBase = GetLargestEdge();
            var largestWidth = Width - 2 * Padding;
            var heightLineLengthInPercentage = GetTriangleHeight() / percentageBase;
            var y = Height - 20 - (int) (largestWidth * heightLineLengthInPercentage);
            var calculatedX = Math.Round(Math.Sqrt(Math.Pow(GetSmallestEdge(), 2) - Math.Pow(GetTriangleHeight(), 2)));
            var calculatedXInPercentage = calculatedX / percentageBase;
            return new Point(20 + (int) (largestWidth * calculatedXInPercentage), y);
        }

        private Point CalculateFirstLineCenter()
        {
            var percentageBase = GetLargestEdge();
            var largestWidth = Width - 2 * Padding;
            var heightLineLengthInPercentage = GetTriangleHeight() / percentageBase;
            var calculatedX = Math.Round(Math.Sqrt(Math.Pow(GetSmallestEdge(), 2) - Math.Pow(GetTriangleHeight(), 2)));
            var calculatedXInPercentage = calculatedX / percentageBase;
            var labelX = Padding + (int) (largestWidth * (calculatedXInPercentage / 2));
            var labelY = Height - Padding - (int) (largestWidth * (heightLineLengthInPercentage / 2));
            return new Point(labelX - 20, labelY - 20);
        }

        private Point CalculateSecondLineCenter()
        {
            var percentageBase = GetLargestEdge();
            var largestWidth = Width - 2 * Padding;
            var heightLineLengthInPercentage = GetTriangleHeight() / percentageBase;
            var labelY = Height - Padding - (int) (largestWidth * (heightLineLengthInPercentage / 2));
            var calculatedX = Math.Round(Math.Sqrt(Math.Pow(GetMediumEdge(), 2) - Math.Pow(GetTriangleHeight(), 2)));
            var calculatedXInPercentage = calculatedX / percentageBase;
            var labelX = Width - Padding - (int) (largestWidth * (calculatedXInPercentage / 2));
            return new Point(labelX, labelY - 20);
        }

        private Point CalculateThirdLineCenter()
        {
            return new Point(Width / 2, Height - 20);
        }

        private static string GetCornerAt(Angle angle)
        {
            if (angle.Equals(Angle.Alpha))
            {
                return "A";
            }

            if (angle.Equals(Angle.Beta))
            {
                return "B";
            }

            return "C";
        }

        private double GetSmallestEdge()
        {
            if (EdgeA < EdgeB && EdgeA < EdgeC)
            {
                return EdgeA;
            }

            return EdgeB < EdgeC ? EdgeB : EdgeC;
        }

        private double GetMediumEdge()
        {
            var edges = new List<Double> {EdgeA, EdgeB, EdgeC};
            edges.Remove(GetSmallestEdge());
            edges.Remove(GetLargestEdge());
            return edges[0];
        }

        private double GetLargestEdge()
        {
            if (EdgeA >= EdgeB && EdgeA >= EdgeC)
            {
                return EdgeA;
            }

            return EdgeB >= EdgeC ? EdgeB : EdgeC;
        }

        private double GetTriangleHeight()
        {
            var a = Math.Sqrt(EdgeA + EdgeB + EdgeC);
            var b = Math.Sqrt(-EdgeA + EdgeB + EdgeC);
            var c = Math.Sqrt(EdgeA - EdgeB + EdgeC);
            var d = Math.Sqrt(EdgeA + EdgeB - EdgeC);
            return (a * b * c * d) / (2 * GetLargestEdge());
        }
    }
}