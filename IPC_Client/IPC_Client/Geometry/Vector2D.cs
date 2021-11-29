using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace INFOGET_ZERO_HULL.Geometry
{
    [DebuggerDisplay("X = {X}, Y = {Y}")]
    public class Vector2D
    {
        public static Vector2D Instance = new Vector2D();

        public double X = -32000;
        public double Y = -32000;
        public double Tolerence;

        public Vector2D()
        {
        }

        public Vector2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
            this.Tolerence = 0.00001;
        }

        /// <summary>
        /// 주어진 두 포인트로 생성. From->To
        /// </summary>
        /// <param name="FromPoint"></param>
        /// <param name="ToPoint"></param>
        public Vector2D(Point2D FromPoint, Point2D ToPoint)
        {
            this.X = ToPoint.X - FromPoint.X;
            this.Y = ToPoint.Y - FromPoint.Y;
            //dmkim 170318
            this.Tolerence = 0.00001;
        }


        //Scale the vector length
        public void BlankProduct(double sc)
        {
            this.X = this.X * sc;
            this.Y = this.Y * sc;
        }

        //The two vectors self and vec are compared
        // If only the direction is supposed to be compared use unit_vector first.
        // true = The vectors are equal.
        // false = The vectors are not equal
        //dmkim 170519 dTol 추가.
        public Boolean CompareVector(Vector2D vec, double dTol = 0.0)
        {
            //dmkim 170519
            if (dTol == 0.0) { dTol = this.Tolerence; }
            

            double xx = Math.Sqrt(Math.Pow((this.X - vec.X), 2) + Math.Pow((this.Y - vec.Y), 2));
            if (xx < dTol)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public double DotProduct(Vector2D vec)
        {
            double rtnvalue = (this.X * vec.X) + (this.Y * vec.Y);
            return rtnvalue;
        }

        public string LargestComponentAxis()
        {
            if (Math.Abs(this.X) < Math.Abs(this.Y))
            {
                return "Y";
            }
            else
            {
                return "X";
            }
        }

        public double Length()
        {
            return Math.Sqrt(Math.Pow(this.X, 2) + Math.Pow(this.Y, 2));
        }

        public void SetFromPoints(Point2D FromPoint, Point2D ToPoint)
        {
            this.X = ToPoint.X - FromPoint.X;
            this.Y = ToPoint.Y - FromPoint.Y;
        }

        public void SetToUnitVector()
        {
            double VectorLength = Math.Sqrt(Math.Pow(this.X, 2) + Math.Pow(this.Y, 2));
            this.X = this.X / VectorLength;
            this.Y = this.Y / VectorLength;
        }

        public void SetLength(double newLength)
        {
            this.SetToUnitVector();
            this.X = this.X * newLength;
            this.Y = this.Y * newLength;
        }

        public void SetComponents(double XValue, double YValue)
        {
            this.X = XValue;
            this.Y = YValue;
        }

        /// <summary>
        /// 값 제대로 못 가져오 는것 같음 . dmkim 161228
        /// </summary>
        /// <param name="CompareVector"></param>
        /// <returns></returns>
        public double AngleToVector(Vector2D CompareVector)
        {
            double existdeg = 0;
            if (Math.Abs(this.X) > this.Tolerence)
            {
                if (this.Y > 0)
                {
                    existdeg = 90;
                }
                else
                {
                    existdeg = -90;
                }
            }
            else
            {
                double vecdeg = this.Y / this.X;
                existdeg = Math.Atan(vecdeg);
                if (this.X < 0)
                {
                    if (this.Y > 0)
                    {
                        existdeg = existdeg + 180;
                    }
                    else
                    {
                        existdeg = existdeg - 180;
                    }
                }
            }

            double diffdeg = 0;
            if (Math.Abs(CompareVector.X) < this.Tolerence)
            {
                if (CompareVector.Y > 0)
                {
                    diffdeg = 90;
                }
                else
                {
                    diffdeg = -90;
                }
            }
            else
            {
                double vecdeg = CompareVector.Y / CompareVector.X;
                diffdeg = Math.Atan(vecdeg);
                if (CompareVector.X < 0)
                {
                    if (CompareVector.Y > 0)
                    {
                        diffdeg = diffdeg + 180;
                    }
                    else
                    {
                        diffdeg = diffdeg - 180;
                    }
                }
            }

            double Angle = Math.Abs(existdeg - diffdeg);
            if (Angle > 180)
            {
                Angle = 360 - Angle;
            }
            return Angle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Radian..."></param>
        public void Rotate(double Degrees)
        {
            double x = Math.Cos(Degrees) * this.X - Math.Sin(Degrees) * this.Y;
            double y = Math.Sin(Degrees) * this.X + Math.Cos(Degrees) * this.Y;
            this.X = x;
            this.Y = y;
        }

        public double ScalarComponentOnVector(Vector2D vec)
        {
            double L1 = vec.Length();
            if (L1 < this.Tolerence)
            {
                return 0;
            }
            else
            {
                return ((this.X * vec.X + this.Y * vec.Y) / L1);
            }
        }

        public void SetFromVector(Vector2D vec)
        {
            this.X = vec.X;
            this.Y = vec.Y;
        }

        public void SetFromVectorDifference(Vector2D vec1, Vector2D vec2)
        {
            this.X = vec1.X - vec2.X;
            this.Y = vec1.Y - vec2.Y;
        }

        public void SetFromVectorSum(Vector2D vec1, Vector2D vec2)
        {
            this.X = vec1.X + vec2.X;
            this.Y = vec1.Y + vec2.Y;
        }

        public void SetCoordinates(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        
        public void Print()
        {
            Console.WriteLine("Vector2D X : {0}, Y : {1}", this.X, this.Y);
        }
    }
}
