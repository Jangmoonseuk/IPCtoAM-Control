using System;
using System.Collections.Generic;

using System.Text;
using System.Diagnostics;


namespace INFOGET_ZERO_HULL.Geometry
{
    /// <summary>
    /// Point3D Class 
    /// </summary>
    [DebuggerDisplay("X = {X}, Y = {Y}, Z = {Z}")]
    public class Point3D
    {
        public static Point3D Instance = new Point3D();

        public double X;
        public double Y;
        public double Z;

        public Point3D()
        {
            //if (License.CheckLicense() == "Fail")
            //{
            //    System.Threading.Thread.Sleep(10);

            //    Random oRandom = new Random(0);
            //    this.X = this.X * oRandom.Next(100);
            //    this.Y = this.Y * oRandom.Next(100);
            //    this.Z = this.Z * oRandom.Next(100);
            //}
        }
        public Point3D(string s,bool ischange = true)
        {
            if (ischange)
            {
                //X 32342mm Y 16231mm Z 12369mm
                string[] splitdata = s.Split(' ');
                if (splitdata.Length < 6)
                {
                    return;
                }
                X = double.Parse(splitdata[1].Replace("mm", ""));
                Y = double.Parse(splitdata[3].Replace("mm", ""));
                Z = double.Parse(splitdata[5].Replace("mm", ""));
            }
            else
            {
                //8400,2983,4500
                string[] splitdata = s.Split(',');
                if (splitdata.Length < 3)
                {
                    return;
                }
                X = double.Parse(splitdata[0]);
                Y = double.Parse(splitdata[1]);
                Z = double.Parse(splitdata[2]);
            }
        }

        public Point3D(double x, double y, double z)
        {
            //if (License.CheckLicense() == "Fail")
            //{
            //    Random oRandom = new Random();
            //    x = x * oRandom.Next(100);
            //    y = y * oRandom.Next(100);
            //    z = z * oRandom.Next(100);
            //}

            this.SetCoordinates(x, y, z);
        }


        /// <summary>
        /// calculate the distance between this and point ToPoint
        /// </summary>
        /// <param name="ToPoint"></param>
        /// <returns></returns>
        public double DistanceToPoint(Point3D ToPoint)
        {
            double xDiff = this.X - ToPoint.X;
            double yDiff = this.Y - ToPoint.Y;
            double zDiff = this.Z - ToPoint.Z;
            return Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2) + Math.Pow(zDiff, 2));
        }

        /// <summary>
        /// Set the point from given coordinates
        /// </summary>
        /// <param name="XValue"></param>
        /// <param name="YValue"></param>
        /// <param name="ZValue"></param>
        public void SetCoordinates(double XValue, double YValue, double ZValue)
        {
            this.X = XValue;
            this.Y = YValue;
            this.Z = ZValue;
        }

        /// <summary>
        /// Set the point to be the midpoint between two other points
        /// </summary>
        /// <param name="FromPoint"></param>
        /// <param name="ToPoint"></param>
        public void SetFromMidPoint(Point3D FromPoint, Point3D ToPoint)
        {
            this.X = (FromPoint.X + ToPoint.X) / 2;
            this.Y = (FromPoint.Y + ToPoint.Y) / 2;
            this.Z = (FromPoint.Z + ToPoint.Z) / 2;
        }

        /// <summary>
        /// Set the point with values from another point (OnPoint)
        /// </summary>
        /// <param name="OnPoint"></param>
        public void SetFromPoint(Point3D OnPoint)
        {
            this.X = OnPoint.X;
            this.Y = OnPoint.Y;
            this.Z = OnPoint.Z;

        }
        /// <summary>
        ///  Transform the point using a transformation matrix.
        ///  self will be updated with the result
        /// </summary>
        /// <param name="tra"></param>
        public void Transform(Transformation3D tra)
        {
            double proj = this.X*tra.matrix14 + this.Y*tra.matrix24 +this.Z*tra.matrix34 + tra.matrix44;
            if (proj <= 1.0E-15 && proj >= -1.0E-15) proj = 1.0;
            double x = (this.X * tra.matrix11 + this.Y * tra.matrix21 + this.Z * tra.matrix31 + tra.matrix41) / proj;
            double y = (this.X * tra.matrix12 + this.Y * tra.matrix22 + this.Z * tra.matrix32 + tra.matrix42) / proj;
            double z = (this.X * tra.matrix13 + this.Y * tra.matrix23 + this.Z * tra.matrix33 + tra.matrix43) / proj;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public void Print()
        {
            Console.WriteLine("Point3D X : {0}, Y : {1}, Z : {2}", this.X, this.Y, this.Z);
        }
        public void Print(string msg)
        {
            Console.WriteLine("###################################");
            Console.WriteLine("");
            Console.WriteLine("Point3D X : {0}, Y : {1}, Z : {2}", this.X, this.Y, this.Z);

            System.Diagnostics.StackTrace creationStackTrace = new System.Diagnostics.StackTrace(1, true);
            
            System.Diagnostics.StackFrame frame = creationStackTrace.GetFrame(0);
           
            Console.WriteLine(" Call Location : {0}", frame.GetMethod());
           
            Console.WriteLine("###################################");
            System.Windows.Forms.MessageBox.Show(msg);
        }

        public Point3D DownScale(Point3D CenterP, Point3D TargetP, double scale)
        {
            Point3D interval = new Point3D((TargetP.X - CenterP.X) * scale, (TargetP.Y - CenterP.Y) * scale, (TargetP.Z - CenterP.Z) * scale);

            Point3D returnP = new Point3D(CenterP.X + interval.X, CenterP.Y + interval.Y, CenterP.Z + interval.Z);

            return returnP;
        }
        /// <summary>
        /// 중심점 좌표를 구한다.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public Point3D getCenter(List<Point3D> points)
        {
            if (points.Count == 0)
                return null;
            if (points.Count == 1)
                return points[0];
            double X = 0; double Y = 0; double Z = 0;
            foreach (var a in points)
            {
                X += a.X;
                Y += a.Y;
                Z += a.Z;
            }
            X = X / points.Count;
            Y = Y / points.Count;
            Z = Z / points.Count;
            return new Point3D(X, Y, Z);
        }
        /// <summary>
        /// Min점을 구한다.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public Point3D getminP(List<Point3D> points)
        {
            if (points.Count == 0)
                return null;
            if (points.Count == 1)
                return points[0];
            double X = points[0].X; double Y = points[0].Y; double Z = points[0].Z;
            foreach (var a in points)
            {
                if (a.X <= X)
                {
                    X = a.X;
                }
                if (a.Y <= Y)
                {
                    Y = a.Y;
                }
                if (a.Z <= Z)
                {
                    Z = a.Z;
                }
            }
            return new Point3D(X, Y, Z);
        }
        /// <summary>
        /// Max점을 구한다.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public Point3D getmaxP(List<Point3D> points)
        {
            if (points.Count == 0)
                return null;
            if (points.Count == 1)
                return points[0];
            double X = points[0].X; double Y = points[0].Y; double Z = points[0].Z;
            foreach (var a in points)
            {
                if (a.X >= X)
                {
                    X = a.X;
                }
                if (a.Y >= Y)
                {
                    Y = a.Y;
                }
                if (a.Z >= Z)
                {
                    Z = a.Z;
                }
            }
            return new Point3D(X, Y, Z);
        }
    }
}
