using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;


namespace INFOGET_ZERO_HULL.Geometry
{
    /// <summary>
    /// Point2D Class 
    /// </summary>
    [DebuggerDisplay("X = {X}, Y = {Y}")]
    public class Point2D : IEquatable<Point2D>
    {
        public static Point2D Instance = new Point2D();

        public double X;
        public double Y;


        public Point2D()
        {
        }
        public Point2D(double x, double y)
        {
            this.SetCoordinates(x, y);
        }

        /// <summary>
        /// Deep Copy
        /// </summary>
        /// <param name="oPoint2D"></param>
        public Point2D(Point2D oPoint2D)
        {
            this.SetFromPoint(oPoint2D);
        }

        public bool Equals(Point2D other)
        {
            // Would still want to check for null etc. first.
            return this.X == other.X &&
                   this.Y == other.Y;
        }


        public double DistanceToPoint(Point2D ToPoint)
        {
            double xDiff = this.X - ToPoint.X;
            double yDiff = this.Y - ToPoint.Y;
            return Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2));
        }

        public void Move(double XOffset, double YOffset)
        {
            this.X = this.X + XOffset;
            this.Y = this.Y + YOffset;
        }

        public void SetCoordinates(double XValue, double YValue)
        {
            this.X = XValue;
            this.Y = YValue;
        }

        public void SetFromMidPoint(Point2D FromPoint, Point2D ToPoint)
        {
            this.X = (FromPoint.X + ToPoint.X) / 2;
            this.Y = (FromPoint.Y + ToPoint.Y) / 2;
        }

        public void SetFromPoint(Point2D OnPoint)
        {
            this.X = OnPoint.X;
            this.Y = OnPoint.Y;
        }

        public void Print()
        {
            Console.WriteLine("Point2D X : {0}, Y : {1}", this.X, this.Y);
        }
    }
}
