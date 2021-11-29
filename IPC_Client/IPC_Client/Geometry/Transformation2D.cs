using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INFOGET_ZERO_HULL.Geometry
{
    public class Transformation2D
    {
        public static Transformation2D Instance = new Transformation2D();

        public double[][] matrix = new double[3][];
        public int type;


        public Transformation2D()
        {

            this.type = 1;
            this.matrix[0] = new double[] { 0.0, 0.0, 0.0 };
            this.matrix[1] = new double[] { 0.0, 0.0, 0.0 };
            this.matrix[2] = new double[] { 0.0, 0.0, 0.0 };

            this.IdentityTransf();


        }
        public void Assign(Transformation2D that)
        {
        }

        public void IdentityTransf()
        {
            for (int Row = 0; Row < 3; Row++)
            {
                for (int Col = 0; Col < 3; Col++)
                {
                    if (Row == Col) this.Set(Row, Col, 1.0);
                    else this.Set(Row, Col, 0.0);
                }
            }
        }

        public void Set(int Row, int Col, double Value)
        {
            double newValue = 0.0 + Value;
            this.matrix[Row][Col] = newValue;
        }
        public double Get(int Row, int Col)
        {
            List<int> range = new List<int> { 0, 1, 2 };

            if (range.Contains(Row) && range.Contains(Col)) return this.matrix[Row][Col];
            else return 0.0;
        }

        //OK
        public void Combine(Transformation2D tra)
        {
            double[][] M1 = new double[3][];
            M1[0] = new double[] { 0.0, 0.0, 0.0 };
            M1[1] = new double[] { 0.0, 0.0, 0.0 };
            M1[2] = new double[] { 0.0, 0.0, 0.0 };

            double[][] M2 = new double[3][];
            M2[0] = new double[] { 0.0, 0.0, 0.0 };
            M2[1] = new double[] { 0.0, 0.0, 0.0 };
            M2[2] = new double[] { 0.0, 0.0, 0.0 };

            double[][] MOUT = new double[3][];
            MOUT[0] = new double[] { 0.0, 0.0, 0.0 };
            MOUT[1] = new double[] { 0.0, 0.0, 0.0 };
            MOUT[2] = new double[] { 0.0, 0.0, 0.0 };

            int t1 = 0;
            int t2 = 0;

            this.GetByRow(t1, ref M1);
            tra.GetByRow(t2, ref M2);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    double sum = 0.0;
                    for (int k = 0; k < 3; k++)
                    {
                        sum = sum + M1[i][k] * M2[k][j];
                    }
                    MOUT[i][j] = sum;
                }
            }
            int tout;
            if (this.type > tra.type)
            {
                tout = this.type;
            }
            else
            {
                tout = tra.type;
            }
            this.SetByRow(tout, MOUT);
        }

        public void GetByRow(int t, ref double[][] M)
        {
            t = this.type;
            M[0][0] = this.Get(0, 0);
            M[0][1] = this.Get(0, 1);
            M[0][2] = this.Get(0, 2);

            M[1][0] = this.Get(1, 0);
            M[1][1] = this.Get(1, 1);
            M[1][2] = this.Get(1, 2);

            M[2][0] = this.Get(2, 0);

            // ??????????????????????????
            M[2][1] = this.Get(1, 1);
            M[2][2] = this.Get(2, 2);

        }

        public void SetByRow(int t, double[][] M)
        {
            this.type = t;
            this.Set(0, 0, M[0][0]);
            this.Set(0, 1, M[0][1]);
            this.Set(0, 2, M[0][2]);

            this.Set(1, 0, M[1][0]);
            this.Set(1, 1, M[1][1]);
            this.Set(1, 2, M[1][2]);

            this.Set(2, 0, M[2][0]);
            this.Set(2, 1, M[2][1]);
            this.Set(2, 2, M[2][2]);
        }

        //OK
        public void Rotate(Point2D center, double angle)
        {
            this.Set(0, 0, Math.Cos(angle));
            this.Set(0, 1, Math.Sin(angle));
            this.Set(1, 0, -Math.Sin(angle));
            this.Set(1, 1, Math.Cos(angle));
            this.Set(2, 0, (1.0 - Math.Cos(angle)) * center.X + Math.Sin(angle) * center.Y);
            this.Set(2, 1, (1.0 - Math.Cos(angle)) * center.Y - Math.Sin(angle) * center.X);

        }
        //OK
        public void Scale(double scale)
        {
            Transformation2D T = new Transformation2D();
            T.Set(2, 2, 1 / scale);
            this.Combine(T);
        }
        //OK
        public Point2D Transform(Point2D point)
        {
            double proj = point.X * this.Get(0, 2) + point.Y * this.Get(1, 2) + this.Get(2, 2);

            if (proj <= 1.0E-15 && proj >= -1.0E-15)
            {
                proj = 1.0;
            }
            double X = (point.X * this.Get(0, 0) + point.Y * this.Get(1, 0) + this.Get(2, 0)) / proj;
            double Y = (point.X * this.Get(0, 1) + point.Y * this.Get(1, 1) + this.Get(2, 1)) / proj;

            Point2D rtnPoint = new Point2D(X, Y);

            return rtnPoint;
        }
        //OK
        public Vector2D Transform(Vector2D point)
        {

            Point2D pt2ds = new Point2D(0, 0);
            Point2D pt2de = new Point2D(point.X, point.Y);

            point.SetFromPoints(this.Transform(pt2ds), this.Transform(pt2de));

            return point;
        }
        //OK
        public void Translate(Vector2D vector)
        {
            if (vector.X != 0.0 || vector.Y != 0.0)
            {
                this.Set(2, 0, this.Get(2, 0) + vector.X);
                this.Set(2, 1, this.Get(2, 1) + vector.Y);
            }
        }

        //public void Reflect(Point2D point, Vector2D vector)
        //{
        //}

        //public void Invert()
        //{
        //}

        //public void GetScale()
        //{
        //}

        //public void GetXYShear()
        //{
        //}

        //public void GetTranslation()
        //{
        //}

        //public void GetRotation()
        //{
        //}
        //public void GetReflection()
        //{
        //}

        public void Print()
        {
            Console.WriteLine("\n Transformation2D ");
            Console.WriteLine("\t Type : {0}", this.type.ToString());
            Console.WriteLine("\t [{0},{1},{2}]",
                this.matrix[0][0].ToString(), this.matrix[0][1].ToString(), this.matrix[0][2].ToString());
            Console.WriteLine("\t [{0},{1},{2}]",
                this.matrix[1][0].ToString(), this.matrix[1][1].ToString(), this.matrix[1][2].ToString());
            Console.WriteLine("\t [{0},{1},{2}]",
                this.matrix[2][0].ToString(), this.matrix[2][1].ToString(), this.matrix[2][2].ToString());
        }
    }
}
