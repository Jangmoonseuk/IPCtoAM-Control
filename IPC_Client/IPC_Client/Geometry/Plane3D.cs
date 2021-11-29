using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace INFOGET_ZERO_HULL.Geometry
{
    public class Plane3D
    {
        public static Plane3D Instance = new Plane3D();

        public Point3D Point = new Point3D();
        public Vector3D Normal = new Vector3D();

        public Plane3D()
        {
        }

        public Plane3D(Point3D pnt, Vector3D norm)
        {
            this.Point = pnt;
            this.Normal = norm;

        }

        public void Assign(Plane3D that)
        {
        }

        //OK
        public double DistanceToPoint(Point3D point)
        {
            Line3D line = new Line3D(point, this.Normal);
            Point3D inpoint = new Point3D();
            int res = this.IntersectLine(line, ref inpoint);
            if (res == 0)
            {
                double dist = point.DistanceToPoint(inpoint);
                return dist;
            }
            else return -1;

        }

        //OK
        //If an intersection is found the function resturns 0, otherwise -1
        public int IntersectLine(Line3D sline, ref Point3D point)
        {
            int res = -1;
            Line3D line = new Line3D();
            line.Point = sline.Point;
            line.Direction = sline.Direction;

            Vector3D V = new Vector3D();
            V.SetFromPoints(this.Point, line.Point);
            if (0 < line.Direction.DotProduct(V)) line.Direction.BlankProduct(-1.0);
            Vector3D U = new Vector3D(line.Direction.X, line.Direction.Y, line.Direction.Z);
            U.SetToUnitVector();

            V.SetCoordinates(this.Normal.X, this.Normal.Y, this.Normal.Z);
            V.SetToUnitVector();
            double S = U.DotProduct(V);

            if (S >= 1.0E-6 || S <= -1.0E-6)
            {
                V.SetFromPoints(line.Point, this.Point);
                S = Math.Abs(S);
                if (0 < this.Normal.DotProduct(V)) S = -1.0 * S;
                Line3D line_normal = new Line3D(this.Point, this.Normal);
                S = V.ScalarComponentOnLine(line_normal) / S;
                point.X = line.Point.X - S * U.X;
                point.Y = line.Point.Y - S * U.Y;
                point.Z = line.Point.Z - S * U.Z;
                res = 0;
            }

            return res;
        }

        public List<double> IntersectLineSegment(Line3D linesegment3d, ref Point3D inpoint)
        {

            int ires = this.IntersectLine(linesegment3d, ref inpoint);

            if (ires == 0)
            {
                List<double> resu = linesegment3d.GetUFromPointOnLine(inpoint);

                if (resu[0] == 1)
                {
                    double u = resu[1];
                    if (u < 0.0)
                    {
                        double dist = inpoint.DistanceToPoint(linesegment3d.Point);
                        return new List<double> { 0, u, dist };
                    }
                    else if (1.0 < u)
                    {
                        double dist = inpoint.DistanceToPoint(linesegment3d.EndPoint);
                        return new List<double> { 0, u, dist };
                    }
                    //on line
                    else
                    {
                        return new List<double> { 1, u, 0 };
                    }
                }
                //fail getu
                else
                {
                    return new List<double> { -1, 0, 0 };
                }
            }
            //intersectline fail
            else
            {
                return new List<double> { -1, 0, 0 };
            }
        }

        public void Transform(Transformation3D tra)
        {
            this.Point.Transform(tra);
            this.Normal.Transform(tra);
        }

        public void Print()
        {
            Console.WriteLine("##########   PLANE 3D   ##########");
            Console.WriteLine("Point    X : {0}, Y : {1}, Z : {2}", this.Point.X, this.Point.Y, this.Point.Z);
            Console.WriteLine("Vector   X : {0}, Y : {1}, Z : {2}", this.Normal.X, this.Normal.Y, this.Normal.Z);
        }
    }
}
