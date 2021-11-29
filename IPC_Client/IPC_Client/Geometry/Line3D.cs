using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace INFOGET_ZERO_HULL.Geometry
{
    public class Line3D : ICloneable
    {
        public static Line3D Instance = new Line3D();

        public Point3D Point = new Point3D();
        public Vector3D Direction = new Vector3D();

        public double Length = 1.0;

        public Point3D EndPoint = new Point3D();

        public string LineType = "Solid";
        public string Colour = "Black";

        public Point3D pointonself = new Point3D();
        public Point3D pointonlineseg3d = new Point3D();

        public double U1 = -1000;
        public double U2 = -1000;

        public Point3D StartPoint = new Point3D();

        public object Clone()
        {
            //Deep Copy

            Line3D line3D = new Line3D();
            line3D.Point = new Point3D(this.Point.X, this.Point.Y, this.Point.Z);
            //line3D.Point = this.Point;
            line3D.Direction = new Vector3D(this.Direction.X, this.Direction.Y,this.Direction.Z);
            //line3D.Direction = this.Direction;
            line3D.Length = this.Length;
            line3D.EndPoint = new Point3D(this.EndPoint.X, this.EndPoint.Y, this.EndPoint.Z);
            //line3D.EndPoint = this.EndPoint;
            line3D.LineType = this.LineType;
            line3D.Colour = this.Colour;
            line3D.pointonself = new Point3D(this.pointonself.X, this.pointonself.Y, this.pointonself.Z);
            //line3D.pointonself = this.pointonself;
            line3D.pointonlineseg3d = new Point3D(this.pointonlineseg3d.X, this.pointonlineseg3d.Y, this.pointonlineseg3d.Z);
            //line3D.pointonlineseg3d = this.pointonlineseg3d;
            line3D.U1 = this.U1;
            line3D.U2 = this.U2;

            return line3D;

            //return new LineSegment3D(this.Point, this.EndPoint, this.LineType);

            //Shallow Copy
            //return this.MemberwiseClone();  
        }

        public Line3D()
        {
            this.InitPara();
        }

        public Line3D(Point3D point, Vector3D direction)
        {

            this.Point.SetFromPoint(point);
            this.Direction.SetFromVector(direction);

            this.InitPara();
        }

        public Line3D(Point3D point, Vector3D direction, double length)
        {

            this.Point.SetFromPoint(point);
            this.Direction.SetFromVector(direction);
            this.Length = length;

            this.InitPara();
        }

        public Line3D(Point3D startpt3d, Point3D endpt3d, string linetype = "SolidWide")
        {
            StartPoint = startpt3d;
            this.SetFromPoints(startpt3d, endpt3d);
            this.LineType = linetype;
        }

        public void InitPara()
        {
            this.Direction.SetToUnitVector();
            this.Direction.BlankProduct(this.Length);

            this.EndPoint.SetCoordinates(this.Point.X + this.Direction.X, this.Point.Y + this.Direction.Y, this.Point.Z + this.Direction.Z);
            this.Direction.SetToUnitVector();
        }

        public void SetFromPoints(Point3D startpt3d, Point3D endpt3d)
        {
            Vector3D dir = new Vector3D();
            dir.SetFromPoints(startpt3d, endpt3d);
            double length = dir.Length();

            if (length < 0.00001)
            {
                length = 0.0001;
                dir.SetCoordinates(1, 1, 1);
            }

            string linetype = this.LineType;
            string colour = this.Colour;

            //this.Point.SetCoordinates(startpt3d.X, startpt3d.Y, startpt3d.Z);
            this.Point.SetFromPoint(startpt3d);
            this.Direction.SetFromVector(dir);

            this.Length = length;

            this.EndPoint = new Point3D();

            this.Direction.SetToUnitVector();
            this.Direction.BlankProduct(this.Length);

            this.EndPoint.SetCoordinates(this.Point.X + this.Direction.X, this.Point.Y + this.Direction.Y, this.Point.Z + this.Direction.Z);
            this.Direction.SetToUnitVector();

            this.pointonself = new Point3D();
            this.pointonlineseg3d = new Point3D();

            this.U1 = -1000;
            this.U2 = -1000;

            this.LineType = linetype;
            this.Colour = colour;

        }

        //OK
        public List<Point3D> GetCrossPointFromLine3D(Line3D lineseg3d, ref double U1, ref double U2)
        {

            Plane3D plane = new Plane3D(this.Point, this.Direction);

            Line3D lineprojected = lineseg3d.GetLineSegment3DProjectedOnPlane3D(plane);

            List<double> resList = lineprojected.PointProjectedOnLine(this.Point);

            Point3D pointonlineseg3d = lineseg3d.GetPointOnLineFromU(resList[3]);

            List<double> resList1 = this.PointProjectedOnLine(pointonlineseg3d);

            Point3D pointonself = new Point3D(resList1[0], resList1[1], resList1[2]);

            U1 = resList1[3];
            U2 = resList[3];

            List<Point3D> rtnlist = new List<Point3D> { pointonself, pointonlineseg3d };

            return rtnlist;

            //Console.WriteLine("{0},{1},{2}", this.pointonself.X, this.pointonself.Y, this.pointonself.Z);
            //Console.WriteLine("{0}", this.U1);
            //Console.WriteLine("{0},{1},{2}", this.pointonlineseg3d.X, this.pointonlineseg3d.Y, this.pointonlineseg3d.Z);
            //Console.WriteLine("{0}", this.U2);
        }

        //OK
        public Line3D GetLineSegment3DProjectedOnPlane3D(Plane3D plane3d)
        {
            Line3D sline = new Line3D(this.Point, plane3d.Normal, 1.0);
            Line3D eline = new Line3D(this.EndPoint, plane3d.Normal, 1.0);

            Point3D points = new Point3D();
            Point3D pointe = new Point3D();

            plane3d.IntersectLine(sline, ref points);
            plane3d.IntersectLine(eline, ref pointe);



            Vector3D dir = new Vector3D();
            dir.SetFromPoints(points, pointe);
            Length = dir.Length();

            Line3D rtnLine3D = new Line3D(points, dir, Length);
            return rtnLine3D;
        }
        //OK
        public List<double> PointProjectedOnLine(Point3D point3d)
        {
            if (this.Length < 0.000001)
            {
                List<object> rtnlist = new List<object> { this.Point, 0.0 };
            }

            Vector3D vec = new Vector3D();
            vec.SetFromPoints(this.Point, point3d);

            Vector3D linedir = new Vector3D();
            linedir.SetFromVector(this.Direction);

            double dot = vec.DotProduct(linedir);
            double veconselflength = dot / (linedir.Length());

            linedir.SetToUnitVector();
            linedir.BlankProduct(veconselflength);

            Point3D pointonline = new Point3D(this.Point.X + linedir.X, this.Point.Y + linedir.Y, this.Point.Z + linedir.Z);

            double U = this.GetUFromPointOnLine(pointonline)[1];

            List<double> rtnlist1 = new List<double> { pointonline.X, pointonline.Y, pointonline.Z, U };

            return rtnlist1;


        }
        //OK
        public Point3D PointProjectedOnLineSegment(Point3D point3d, ref int result)
        {
            Point3D linepoint = new Point3D();
            Vector3D linevector = new Vector3D();
            linevector.SetFromVector(this.Direction);
            Point3D projectedpoint = new Point3D();

            if (this.Point.DistanceToPoint(point3d) < this.EndPoint.DistanceToPoint(point3d))
            {
                result = 0;
                linepoint.SetFromPoint(this.Point);
                linevector.BlankProduct(1.0);
            }
            else
            {
                result = -1;
                linepoint.SetFromPoint(this.EndPoint);
                linevector.BlankProduct(-1.0);
            }

            linevector.SetToUnitVector();

            Vector3D vec = new Vector3D();
            vec.SetFromPoints(linepoint, point3d);
            double dot = vec.DotProduct(linevector);

            //Console.WriteLine("dot {0}", dot.ToString());

            //dmkim 120504 if (dot < 0) 수정
            if (dot < -0.000000001)
            {
                return linepoint;
            }
            else
            {
                linevector.BlankProduct(dot);
                projectedpoint.SetCoordinates(linepoint.X + linevector.X,
                                              linepoint.Y + linevector.Y,
                                              linepoint.Z + linevector.Z);
                result = 1;
                return projectedpoint;
            }


        }

        //OK ...
        public List<double> GetUFromPointOnLine(Point3D point3D)
        {
            Vector3D dirvec = new Vector3D();
            dirvec.SetFromPoints(this.Point, point3D);

            if (dirvec.Length() < 0.00001)
            {
                List<double> rtnlist = new List<double> { 1, 0, 0 };
                return rtnlist;
            }

            if (this.Direction.Length() < 0.00001)
            {
                List<double> rtnlist = new List<double> { 0, 0 };
                return rtnlist;
            }

            double dot = dirvec.DotProduct(this.Direction);
            double cosa = dot / (this.Direction.Length() * dirvec.Length());

            if ((Math.Abs(cosa) - 1.0) < 0.001)
            {
                double U;
                if (0.0 <= dot) U = dirvec.Length() / this.Length;
                else U = -dirvec.Length() / this.Length;

                List<double> rtnlist = new List<double> { 1, U };
                return rtnlist;
            }
            else
            {
                List<double> rtnlist = new List<double> { 0, 0 };
                return rtnlist;
            }


        }
        //OK
        public Point3D GetPointOnLineFromU(double U)
        {
            Point3D point = new Point3D();
            point.SetFromPoint(this.Point);
            Point3D epoint = new Point3D();
            epoint.SetFromPoint(this.EndPoint);

            point.X = point.X + U * (epoint.X - point.X);
            point.Y = point.Y + U * (epoint.Y - point.Y);
            point.Z = point.Z + U * (epoint.Z - point.Z);

            return point;

        }
        //OK
        public Point3D DistanceFromPoint3D(Point3D point3d, ref int result, ref double distance)
        {
            Point3D point = this.PointProjectedOnLineSegment(point3d, ref result);
            distance = point.DistanceToPoint(point3d);

            //dmkim 161031
            //return point3d;
            return point;
        }

        public void Transform(Transformation3D tra)
        {
            this.Point.Transform(tra);
            this.EndPoint.Transform(tra);
            this.SetFromPoints(this.Point, this.EndPoint);
        }

        /// <summary>
        /// 두선분의 최단거리.
        /// </summary>
        /// <param name="oOtherLine3D"></param>
        /// <returns></returns>
        public double DistanceLine3DSegment(Line3D oOtherLine3D)
        {
            Line3D l1 = this;
            Line3D l2 = oOtherLine3D; ;

            Vector3D uS = new Vector3D { X = l1.Point.X, Y = l1.Point.Y, Z = l1.Point.Z };
            Vector3D uE = new Vector3D { X = l1.EndPoint.X, Y = l1.EndPoint.Y, Z = l1.EndPoint.Z };
            Vector3D vS = new Vector3D { X = l2.Point.X, Y = l2.Point.Y, Z = l2.Point.Z };
            Vector3D vE = new Vector3D { X = l2.EndPoint.X, Y = l2.EndPoint.Y, Z = l2.EndPoint.Z };
            Vector3D w1 = new Vector3D { X = l1.Point.X, Y = l1.Point.Y, Z = l1.Point.Z };
            Vector3D w2 = new Vector3D { X = l2.Point.X, Y = l2.Point.Y, Z = l2.Point.Z };
            Vector3D u = new Vector3D(); u.SetFromVectorDifference(uE, uS);
            Vector3D v = new Vector3D(); v.SetFromVectorDifference(vE, vS);
            Vector3D w = new Vector3D(); w.SetFromVectorDifference(w1, w2);

            double a = u.DotProduct(u);
            double b = u.DotProduct(v); 
            double c = v.DotProduct(v);
            double d = u.DotProduct(w); 
            double e = v.DotProduct(w);
            double D = a * c - b * b;
            double sc, sN, sD = D;
            double tc, tN, tD = D;
            if (D < 0.01)
            {
                sN = 0;
                sD = 1;
                tN = e;
                tD = c;
            }
            else
            {
                sN = (b * e - c * d);
                tN = (a * e - b * d);
                if (sN < 0)
                {
                    sN = 0;
                    tN = e;
                    tD = c;
                }
                else if (sN > sD)
                {
                    sN = sD;
                    tN = e + b;
                    tD = c;
                }
            }
            if (tN < 0)
            {
                tN = 0;
                if (-d < 0)
                {
                    sN = 0;
                }
                else if (-d > a)
                {
                    sN = sD;
                }
                else
                {
                    sN = -d;
                    sD = a;
                }
            }
            else if (tN > tD)
            {
                tN = tD;
                if ((-d + b) < 0)
                {
                    sN = 0;
                }
                else if ((-d + b) > a)
                {
                    sN = sD;
                }
                else
                {
                    sN = (-d + b);
                    sD = a;
                }
            }
            if (Math.Abs(sN) < 0.01)
            {
                sc = 0;
            }
            else
            {
                sc = sN / sD;
            }
            if (Math.Abs(tN) < 0.01)
            {
                tc = 0;
            }
            else
            {
                tc = tN / tD;
            }
            u.BlankProduct(sc);
            v.BlankProduct(tc);
            Vector3D dP = new Vector3D();// w + (sc * u) - (tc * v);
            dP.SetFromVectorDifference(u,v);
            dP.SetFromVectorSum(dP, w);
            double distance1 = Math.Sqrt(dP.DotProduct(dP));
            return distance1;

        }

        public void Transform(Transformation3D tra, Boolean Print, string message)
        {
            Console.WriteLine("##########   PRE POINT   ########## {0}", message);
            Console.WriteLine("Start_Point X : {0}, Y : {1}, Z : {2}", this.Point.X, this.Point.Y, this.Point.Z);
            this.Point.Transform(tra);
            this.EndPoint.Transform(tra);
            this.SetFromPoints(this.Point, this.EndPoint);
            Console.WriteLine("##########   AFT POINT   ##########");
            Console.WriteLine("Start_Point X : {0}, Y : {1}, Z : {2}", this.Point.X, this.Point.Y, this.Point.Z);
        }


        public void Print()
        {
            Console.WriteLine("##########   LINESEGMENT 3D   ##########");
            Console.WriteLine("Start_Point X : {0}, Y : {1}, Z : {2}", this.Point.X, this.Point.Y, this.Point.Z);
            Console.WriteLine("End_Point   X : {0}, Y : {1}, Z : {2}", this.EndPoint.X, this.EndPoint.Y, EndPoint.Z);
            Console.WriteLine("Direction   X : {0}, Y : {1}, Z : {2}", this.Direction.X, this.Direction.Y, this.Direction.Z);
            Console.WriteLine("Length    : {0}", this.Length);
        }

        public void Print(string message)
        {
            Console.WriteLine("##########   LINESEGMENT 3D   ########## {0}", message);
            Console.WriteLine("Start_Point X : {0}, Y : {1}, Z : {2}", this.Point.X, this.Point.Y, this.Point.Z);
            Console.WriteLine("End_Point   X : {0}, Y : {1}, Z : {2}", this.EndPoint.X, this.EndPoint.Y, EndPoint.Z);
            Console.WriteLine("Direction   X : {0}, Y : {1}, Z : {2}", this.Direction.X, this.Direction.Y, this.Direction.Z);
            Console.WriteLine("Length    : {0}", this.Length);
        }
        public void PrintDetail()
        {
            Console.WriteLine("##########   LINESEGMENT 3D   ##########");
            Console.WriteLine("Start_Point X : {0}, Y : {1}, Z : {2}", this.Point.X, this.Point.Y, this.Point.Z);
            Console.WriteLine("End_Point   X : {0}, Y : {1}, Z : {2}", this.EndPoint.X, this.EndPoint.Y, EndPoint.Z);
            Console.WriteLine("Direction   X : {0}, Y : {1}, Z : {2}", this.Direction.X, this.Direction.Y, this.Direction.Z);
            Console.WriteLine("Length    : {0}", this.Length);
            Console.WriteLine("pointonself   X : {0}, Y : {1}, Z : {2}", this.pointonself.X, this.pointonself.Y, this.pointonself.Z);
            Console.WriteLine("pointonlineseg3d   X : {0}, Y : {1}, Z : {2}", this.pointonlineseg3d.X, this.pointonlineseg3d.Y, this.pointonlineseg3d.Z);
            Console.WriteLine("U1    : {0}", this.U1);
            Console.WriteLine("U2    : {0}", this.U2);
        }

    }
}
