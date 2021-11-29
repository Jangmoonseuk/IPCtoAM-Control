using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace INFOGET_ZERO_HULL.Geometry
{
    public class Vector3D
    {
        public static Vector3D Instance = new Vector3D();

        public double X;
        public double Y;
        public double Z;

        public Vector3D()
        {
            this.X = -32000.0 + 0.0;
            this.Y = -32000.0 + 0.0;
            this.Z = -32000.0 + 0.0;

        }

        public Vector3D(double x, double y, double z)
        {
            this.SetCoordinates(x, y, z);
        }


        public double AngleToVector(Vector3D vec)
        {
            double len1 = this.Length();
            double len2 = vec.Length();

            if (len1 < 1.0E-15) return Math.Asin(1);
            else if (len2 < 1.0E-15) return Math.Asin(1);
            else
            {
                double xx = this.DotProduct(vec);
                if ((xx / (len1 * len2)) > 1.0) return Math.Acos(1);
                else if ((xx / (len1 * len2)) < -1.0) return Math.Acos(-1);
                else return (Math.Acos(xx / (len1 * len2)));
            }
        }

        public double AngleToVectorWithSign(Vector3D vec, Vector3D nvec)
        {
            if (this.Length() < 1.0E-15) return Math.Asin(1);
            else if (vec.Length() < 1.0E-15) return Math.Asin(1);
            else if (nvec.Length() < 1.0E-15) return Math.Asin(1);
            else
            {
                double angle = this.AngleToVector(vec);
                if (nvec.BoxProduct(this, vec) < 0.0) return (-1.0 * angle);
                else return angle;
            }

        }

        public double Length()
        {
            return Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
        }

        public double DotProduct(Vector3D vec)
        {
            return (this.X * vec.X + this.Y * vec.Y + this.Z * vec.Z);
        }

        public double BoxProduct(Vector3D vec1, Vector3D vec2)
        {
            Vector3D vect = new Vector3D();
            vect.SetFromCrossProduct(vec1, vec2);
            return this.DotProduct(vect);
        }

        public void BlankProduct(double sc)
        {
            this.X = this.X * sc;
            this.Y = this.Y * sc;
            this.Z = this.Z * sc;
        }

        public int CompareVector(Vector3D vec, double tolerance)
        {
            double xx = Math.Sqrt((this.X - vec.X) * (this.X - vec.X) + (this.Y - vec.Y) * (this.Y - vec.Y) + (this.Z - vec.Z) * (this.Z - vec.Z));
            if (xx < tolerance) return 1;
            else return 0;
        }

        public int AbsoluteLargestComponentAxis()
        {
            double MaxValue = Math.Abs(this.X);
            int index = 0;
            if (MaxValue < Math.Abs(this.Y))
            {
                MaxValue = Math.Abs(this.Y);
                index = 1;
            }
            if (MaxValue < Math.Abs(this.Z)) index = 2;
            return index;
        }

        public void ProjectOnLine(Line3D projline)
        {
            this.ProjectOnVector(projline.Direction);
        }

        public void ProjectOnVector(Vector3D projvec)
        {
            double LL = projvec.X * projvec.X + projvec.Y * projvec.Y + projvec.Z * projvec.Z;
            if (LL > 1.0E-15)
            {
                LL = (this.X * projvec.X + this.Y * projvec.Y + this.Z * projvec.Z) / LL;
                this.X = projvec.X * LL;
                this.Y = projvec.Y * LL;
                this.Z = projvec.Z * LL;
            }
        }

        public void ProjectOnPlane(Plane3D projplane)
        {
            Vector3D nvec = new Vector3D();
            nvec.SetFromVector(projplane.Normal);
            double len = nvec.Length();
            if (len >= 1.0E-15)
            {
                double sc = this.DotProduct(nvec) / (len * len);
                this.X = this.X - sc * nvec.X;
                this.Y = this.Y - sc * nvec.Y;
                this.Z = this.Z - sc * nvec.Z;
            }

        }

        public void Rotate(double angle, Vector3D rvec)
        {
            double c1 = Math.Cos(angle);
            if (rvec.Length() >= 1.0E-15)
            {
                Vector3D urvec = new Vector3D(rvec.X, rvec.Y, rvec.Z);
                urvec.SetToUnitVector();
                double c2 = (1.0 - c1) * this.DotProduct(urvec);
                double c3 = Math.Sin(angle);
                double x = c1 * this.X + c2 * urvec.X + c3 * (urvec.Y * this.Z - urvec.Z * this.Y);
                double y = c1 * this.Y + c2 * urvec.Y + c3 * (urvec.Z * this.X - urvec.X * this.Z);
                double z = c1 * this.Z + c2 * urvec.Z + c3 * (urvec.X * this.Y - urvec.Y * this.X);
                this.X = x;
                this.Y = y;
                this.Z = z;
            }
            else this.BlankProduct(c1);
        }

        public void Round(int decimals)
        {
            this.X = (this.X * Math.Pow(10, decimals) + 0.5) / Math.Pow(10, decimals);
            this.Y = (this.Y * Math.Pow(10, decimals) + 0.5) / Math.Pow(10, decimals);
            this.Z = (this.Z * Math.Pow(10, decimals) + 0.5) / Math.Pow(10, decimals);
        }

        public double ScalarComponentOnLine(Line3D projline)
        {
            double len = projline.Direction.Length();
            if (len < 1.0E-15) return 0;
            else return ((this.DotProduct(projline.Direction)) / len);
        }

        public void SetComponents(double x, double y, double z)
        {
            this.X = x + 0.0;
            this.Y = y + 0.0;
            this.Z = z + 0.0;
        }

        public void SetFromCrossProduct(Vector3D vec1, Vector3D vec2)
        {
            this.X = vec1.Y * vec2.Z - vec1.Z * vec2.Y;
            this.Y = vec1.Z * vec2.X - vec1.X * vec2.Z;
            this.Z = vec1.X * vec2.Y - vec1.Y * vec2.X;
        }

        public void SetFromPoints(Point3D p1, Point3D p2)
        {
            this.X = p2.X - p1.X;
            this.Y = p2.Y - p1.Y;
            this.Z = p2.Z - p1.Z;
        }

        public void SetFromVector(Vector3D vec)
        {
            this.X = vec.X;
            this.Y = vec.Y;
            this.Z = vec.Z;
        }

        public void SetFromVectorDifference(Vector3D vec1, Vector3D vec2)
        {
            this.X = vec1.X - vec2.X;
            this.Y = vec1.Y - vec2.Y;
            this.Z = vec1.Z - vec2.Z;
        }

        public void SetFromVectorSum(Vector3D vec1, Vector3D vec2)
        {
            this.X = vec1.X + vec2.X;
            this.Y = vec1.Y + vec2.Y;
            this.Z = vec1.Z + vec2.Z;
        }

        public void SetLength(double length)
        {
            this.SetToUnitVector();
            this.BlankProduct(length);
        }

        public void SetToUnitVector()
        {
            double len = this.Length();
            if (len >= 1.0E-15)
            {
                this.X = this.X / len;
                this.Y = this.Y / len;
                this.Z = this.Z / len;
            }
        }

        public void Transform(Transformation3D tra)
        {
            double X = this.X * tra.matrix11 + this.Y * tra.matrix21 + this.Z * tra.matrix31;
            double Y = this.X * tra.matrix12 + this.Y * tra.matrix22 + this.Z * tra.matrix32;
            double Z = this.X * tra.matrix13 + this.Y * tra.matrix23 + this.Z * tra.matrix33;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public void SetCoordinates(double x, double y, double z)
        {
            this.X = x + 0.0;
            this.Y = y + 0.0;
            this.Z = z + 0.0;
        }
        
        public void Print()
        {
            Console.WriteLine("Vector3D X : {0}, Y : {1}, Z : {2}", this.X, this.Y, this.Z);
        }

        public void Print(string msg)
        {
            Console.WriteLine("###################################");
            Console.WriteLine("");
            Console.WriteLine("Vector3D X : {0}, Y : {1}, Z : {2}", this.X, this.Y, this.Z);

            System.Diagnostics.StackTrace creationStackTrace = new System.Diagnostics.StackTrace(1, true);
            //for (int i = 0; i < creationStackTrace.FrameCount; ++i)
            //{
            System.Diagnostics.StackFrame frame = creationStackTrace.GetFrame(0);
            //Console.WriteLine(" ............. : {0}", frame.ToString());
            Console.WriteLine(" Call Location : {0}", frame.GetMethod());
            //Console.WriteLine(" Line Number   : {0}", frame.GetFileLineNumber());
            //}
            Console.WriteLine("###################################");
            MessageBox.Show(msg);
        }

    }
}
