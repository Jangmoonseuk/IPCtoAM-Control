using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INFOGET_ZERO_HULL.Geometry
{
    public class Arc2D
    {
        public Point2D Start_Point = new Point2D();
        public Point2D End_Point = new Point2D();

        public double Amplitude = 0.1;//should not be zero

        public double ToleHighRes = 0.00001;
        public double ToleMidRes = 0.001;
        public double ToleLowRes = 0.1;
        public double Max = 10e15;
        public double Min = -10e15;

        public double ParameterA = 0.0;
        public double ParameterB = 0.0;
        public double ParameterC = 0.0;

        public double Radius = 0.0;

        public Point2D Center = new Point2D();
        public double Angle = 0.0;
        //public Center = public GetCenter()  # get center point and radius
        //public Angle = public GetAngleFromStart(public End_Point)

        public double Area = 0.0;
        public double Length = 0.0;
        //public Area = public GetArea() # area of contour which consist of start point-arc-end point        
        //public Length = public Angle * public Radius

        public List<Line2D> ParaInfo = new List<Line2D> { };
        public List<double> TangentInfo = new List<double> { };
        //public ParaInfo = [] # used for segment data, parameter, as a contour member
        //public TangentInfo = [] # [st,ed,dir]

        public string linetype = "Solid";
        public string colour = "Black";

        public Arc2D()
        {
        }

        public Arc2D(Point2D spoint, Point2D epoint, double amp, string colour = "Black", string linetype = "SolidWide")
        {
            this.Start_Point.SetFromPoint(spoint);
            this.End_Point.SetFromPoint(epoint);

            this.Amplitude = amp;

            this.Center = this.GetCenter();
            this.Angle = this.GetAngleFromStart(this.End_Point);

            this.Area = this.GetArea(); //area of contour which consist of start point-arc-end point
            this.Length = this.Angle * this.Radius;

            this.linetype = linetype;
            this.colour = colour;

            this.GetTangentInfo();
        }

        public Point2D GetCenter()
        {
            Vector3D sevec = new Vector3D(this.End_Point.X - this.Start_Point.X, this.End_Point.Y - this.Start_Point.Y, 0);
            Vector3D tvec = new Vector3D(0, 0, 1);

            Vector3D vect = new Vector3D();
            vect.SetFromCrossProduct(sevec, tvec);

            vect.SetToUnitVector();

            vect.BlankProduct(this.Amplitude);


            Point2D arcmidpt = new Point2D((this.End_Point.X + this.Start_Point.X) / 2.0 + vect.X, (this.End_Point.Y + this.Start_Point.Y) / 2.0 + vect.Y);
            Point2D center = this.GetCenterFromPoints(this.Start_Point, this.End_Point, arcmidpt);

            return center;
        }

        public Point2D GetCenterFromPoints(Point2D Start_Point, Point2D End_Point, Point2D OnArc_Point)
        {
            List<double> m0 = new List<double> { Start_Point.X, End_Point.X, OnArc_Point.X };
            List<double> m1 = new List<double> { Start_Point.Y, End_Point.Y, OnArc_Point.Y };

            List<double> m2 = new List<double> { 1.0, 1.0, 1.0 };

            List<double> r = new List<double> { -(m0[0] * m0[0] + m1[0] * m1[0]), -(m0[1] * m0[1] + m1[1] * m1[1]), -(m0[2] * m0[2] + m1[2] * m1[2]) };

            List<List<double>> m = new List<List<double>> { m0, m1, m2 };

            List<List<double>> minv = this.invmat(m);

            double a = minv[0][0] * r[0] + minv[1][0] * r[1] + minv[2][0] * r[2];
            double b = minv[0][1] * r[0] + minv[1][1] * r[1] + minv[2][1] * r[2];
            double c = minv[0][2] * r[0] + minv[1][2] * r[1] + minv[2][2] * r[2];

            this.ParameterA = a;
            this.ParameterB = b;
            this.ParameterC = c;

            this.Radius = Math.Sqrt(Math.Abs((a * a + b * b - 4 * c) / 4.0));


            Point2D Center = new Point2D(-a / 2.0, -b / 2.0);

            return Center;
        }

        public List<List<double>> invmat(List<List<double>> m)
        {
            List<double> wk = new List<double> { 0.0, 0.0, 0.0 };
            double t = m[1][1] * m[2][2] - m[1][2] * m[2][1];

            wk[0] = t;
            double det = t * m[0][0];

            t = m[2][1] * m[0][2] - m[0][1] * m[2][2];
            wk[1] = t;
            det = det + t * m[1][0];

            t = m[0][1] * m[1][2] - m[1][1] * m[0][2];
            wk[2] = t;
            det = det + t * m[2][0];

            List<double> v0 = new List<double> { wk[0], wk[1], wk[2] };

            t = m[2][0] * m[1][2] - m[1][0] * m[2][2];
            wk[0] = t;
            det = det + t * m[0][1];

            t = m[0][0] * m[2][2] - m[0][2] * m[2][0];
            wk[1] = t;
            det = det + t * m[1][1];

            t = m[1][0] * m[0][2] - m[0][0] * m[1][2];
            wk[2] = t;
            det = det + t * m[2][1];

            List<double> v1 = new List<double> { wk[0], wk[1], wk[2] };

            t = m[1][0] * m[2][1] - m[1][1] * m[2][0];
            wk[0] = t;
            det = det + t * m[0][2];

            t = m[2][0] * m[0][1] - m[0][0] * m[2][1];
            wk[1] = t;
            det = det + t * m[1][2];

            t = m[0][0] * m[1][1] - m[1][0] * m[0][1];
            wk[2] = t;
            det = det + t * m[2][2];

            List<double> v2 = new List<double> { wk[0], wk[1], wk[2] };

            v0[0] = 3.0 * v0[0] / det;
            v0[1] = 3.0 * v0[1] / det;
            v0[2] = 3.0 * v0[2] / det;

            v1[0] = 3.0 * v1[0] / det;
            v1[1] = 3.0 * v1[1] / det;
            v1[2] = 3.0 * v1[2] / det;

            v2[0] = 3.0 * v2[0] / det;
            v2[1] = 3.0 * v2[1] / det;
            v2[2] = 3.0 * v2[2] / det;

            return new List<List<double>> { v0, v1, v2 };
        }

        public double GetAngleFromStart(Point2D pnt)
        {
            Vector2D vecs = new Vector2D();
            Vector2D vecp = new Vector2D();

            vecs.SetFromPoints(this.Center, this.Start_Point);
            vecp.SetFromPoints(this.Center, pnt);


            if (vecp.Length() == 0 || vecs.Length() == 0)
                return 0;

            double cross = vecs.X * vecp.Y - vecs.Y * vecp.X;

            double co = vecs.DotProduct(vecp) / (vecp.Length() * vecs.Length());

            //for remove numerical unstablity
            if (1.0 < co)
                co = 1.0;
            if (co < -1.0)
                co = -1.0;

            double angle = 0.0;

            if (0 <= cross)
            {

                if (0 <= this.Amplitude)
                {

                    angle = Math.Acos(co);

                    return angle;
                }
                else
                {
                    angle = 2.0 * Math.PI - Math.Acos(co);

                    return angle;
                }
            }
            else
            {

                if (0 <= this.Amplitude)
                {
                    angle = 2.0 * Math.PI - Math.Acos(co);

                    return angle;
                }
                else
                {
                    angle = Math.Acos(co);

                    return angle;
                }
            }
        }

        public double GetArea()
        {
            double arcarea = 0.5 * this.Radius * this.Radius * this.Angle; // self.Radius * self.Radius * PI *(self.Angle/2PI)

            double trianglearea = 0.5 * this.Radius * this.Radius * Math.Sin(this.Angle);

            double area = arcarea - trianglearea;

            return Math.Abs(area);
        }

        public void GetTangentInfo()
        {
            this.TangentInfo.Add(this.GetTangentFromU2(0.0));
            this.TangentInfo.Add(this.GetTangentFromU2(1.0));

            Point2D pt1 = new Point2D(this.Center.X + this.Radius, this.Center.Y);
            Point2D pt2 = new Point2D(this.Center.X - this.Radius, this.Center.Y);


            double paratan1 = this.GetUFromPoint(pt1);
            double paratan2 = this.GetUFromPoint(pt2);

            if (paratan1 < paratan2)
            {
                if (paratan1 != -1)
                    this.TangentInfo.Add(paratan1);
                if (paratan2 != -1)
                    this.TangentInfo.Add(paratan2);
            }
            else
            {
                if (paratan2 != -1)
                    this.TangentInfo.Add(paratan2);
                if (paratan1 != -1)
                    this.TangentInfo.Add(paratan1);
            }
        }

        public double GetTangentFromU2(double para)
        {
            Point2D pt = this.GetPointOnSegment(para);
            double delx = this.Center.X - pt.X;
            double dely = this.Center.Y - pt.Y;
            if (Math.Abs(dely) < this.ToleHighRes)
                return this.Max;

            double val = Math.Tan(Math.Atan2(dely, delx) + 0.5 * Math.PI);
            if (Math.Abs(val) < this.ToleHighRes * this.ToleHighRes)
                val = 0.0;
            return val;
        }

        public Point2D GetPointOnSegment(double para)
        {
            double angle = this.Angle * para;

            Vector3D sevec = new Vector3D(this.Start_Point.X - this.Center.X, this.Start_Point.Y - this.Center.Y, 0);
            Vector3D rvec = new Vector3D(0, 0, 1);

            if (this.Amplitude < 0)
                angle = -angle;

            sevec.Rotate(angle, rvec);

            Point2D point = new Point2D(sevec.X + this.Center.X, sevec.Y + this.Center.Y);

            return point;
        }

        public double GetUFromPoint(Point2D point)
        {
            if (this.CheckOnSegment(point, 0) == 0)
                return -1;
            double angle = this.GetAngleFromStart(point);

            if (this.CheckOnSegment(point, 0.0) == 0)
            {
                double dists = this.Start_Point.DistanceToPoint(point);
                double diste = this.End_Point.DistanceToPoint(point);
                if (dists < diste)
                    angle = angle - 2.0 * Math.PI;
            }
            if (Math.Abs(this.Angle) < this.ToleHighRes)
                return 0.0;
            return angle / this.Angle;
        }

        public int CheckOnSegment(Point2D pnt, double Tol)
        {
            if (Tol == 0)
                Tol = this.ToleMidRes * 10.0;
            double dist = Math.Sqrt((pnt.X - this.Center.X) * (pnt.X - this.Center.X) + (pnt.Y - this.Center.Y) * (pnt.Y - this.Center.Y));
            if (this.ToleMidRes * 10.0 < Math.Abs(dist - this.Radius)) // check point on circle or not
                return 0;
            else // check angle
            {
                double ang = this.GetAngleFromStart(pnt);
                if (0.0 <= ang && ang <= this.Angle)
                    return 1;
                if (Math.Abs(ang - 2 * Math.PI) < Tol)
                    ang = 0.0;
                if (ang - this.Angle <= Tol)
                {
                    double dists = pnt.DistanceToPoint(this.Start_Point);
                    double diste = pnt.DistanceToPoint(this.End_Point);
                    if ((dists <= Tol) || (diste <= Tol))
                        return 1;
                }
                return 0;
            }
        }

        public List<Point2D> GetOnArcPoints(int num=2)
        {
            List<Point2D> pointlist = new List<Point2D> { };

            double dGap = 1.0 / Convert.ToDouble(num + 1);
            pointlist.Add(this.Start_Point);

            for (int i = 0; i < num; i++)
            {
                double dPara = dGap * (1.0 + Convert.ToDouble(i));
                Point2D oP2D = this.GetPointOnSegment(dPara);
                pointlist.Add(oP2D);
            }

            pointlist.Add(this.End_Point);
            return pointlist;
        }

    }
}
