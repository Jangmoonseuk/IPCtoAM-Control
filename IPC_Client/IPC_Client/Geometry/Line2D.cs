using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace INFOGET_ZERO_HULL.Geometry
{
    [DebuggerDisplay("START X = {Start_Point.X}, Y = {Start_Point.Y}, END X = {End_Point.X}, Y = {End_Point.Y}")]
    public class Line2D : IEquatable<Line2D>
    {

        public Point2D Start_Point = new Point2D(0, 0);
        public Point2D End_Point = new Point2D(1, 0);
        public Vector2D Direction = new Vector2D { };
        public double Length = 0.0; //Length  업데이트 안됨.

        public double Max = 10e15;
        public double Min = -10e15;

        public double ToleHighRes = 0.00001;
        public double ToleMidRes = 0.001;
        public double ToleLowRes = 0.1;

        public string Type = "Line";

        public double ParameterA = 0.0;
        public double ParameterB = 0.0;
        public double ParameterC = 0.0;

        public string linetype = "Solid";
        public string colour = "Black";

        //public object Clone()
        //{
        //    return new LineSegment2D(this.Start_Point, this.End_Point);
        //}

        public Line2D()
        {
            this.InitPara();
        }
        public Line2D(Point2D spnt, Point2D epnt, string colour = "Black", string linetype = "Solid")
        {
            this.Start_Point.SetCoordinates(spnt.X, spnt.Y);
            this.End_Point.SetCoordinates(epnt.X, epnt.Y);

            this.linetype = linetype;
            this.colour = colour;

            //this.Start_Point.SetFromPoint(spnt);
            //this.End_Point.SetFromPoint(epnt);
            this.InitPara();
        }
        //dmkim 170331
        public Line2D(double dStartX, double dStartY, double dEndX, double dEndY, string colour = "Black", string linetype = "Solid")
        {
            this.Start_Point.SetCoordinates(dStartX, dStartY);
            this.End_Point.SetCoordinates(dEndX, dEndY);

            this.linetype = linetype;
            this.colour = colour;

            //this.Start_Point.SetFromPoint(spnt);
            //this.End_Point.SetFromPoint(epnt);
            this.InitPara();
        }

        public bool Equals(Line2D other)
        {
            double dDist1 = this.Start_Point.DistanceToPoint(other.Start_Point);
            double dDist2 = this.End_Point.DistanceToPoint(other.End_Point);

            // Would still want to check for null etc. first.
            return dDist1 == 0.0 &&
                   dDist2 == 0.0;
        }


        public void InitPara()
        {

            this.Direction.SetCoordinates(this.End_Point.X - this.Start_Point.X, this.End_Point.Y - this.Start_Point.Y);
            this.Length = this.Direction.Length();

            this.SetParameter();
        }

        public void SetFromStartEnd(Point2D spoint, Point2D endp)
        {
            this.Start_Point.SetFromPoint(new Point2D(spoint.X, spoint.Y));
            this.End_Point.SetFromPoint(new Point2D(endp.X, endp.Y));
            this.InitPara();
        }

        //Ax + By + c = 0
        public void SetParameter()
        {
            if (Math.Abs(this.Start_Point.X - this.End_Point.X) < this.ToleHighRes)
            {
                this.ParameterA = 1.0;
                this.ParameterB = 0.0;
                this.ParameterC = -(this.Start_Point.X);
            }

            else if (Math.Abs(this.Start_Point.Y - this.End_Point.Y) < this.ToleHighRes)
            {
                this.ParameterA = 0.0;
                this.ParameterB = 1.0;
                this.ParameterC = -(this.Start_Point.Y);
            }

            else
            {
                this.ParameterA = (this.Start_Point.Y - this.End_Point.Y) / (this.Start_Point.X - this.End_Point.X);
                this.ParameterB = -1.0;
                this.ParameterC = -this.Start_Point.X * (this.Start_Point.Y - this.End_Point.Y) / (this.Start_Point.X - this.End_Point.X) + this.Start_Point.Y;
            }

        }

        public void Transform(Transformation2D transform)
        {
            Point2D spt = new Point2D();
            Point2D ept = new Point2D();

            spt.SetFromPoint(this.Start_Point);
            ept.SetFromPoint(this.End_Point);

            spt = transform.Transform(spt);
            ept = transform.Transform(ept);

            this.Start_Point = spt;
            this.End_Point = ept;
            this.InitPara();

            //좀 이상함......

            //LineSegment2D newLineSegment2D = new LineSegment2D(spt, ept);

        }

        public void SetFromStartDirectionLength(Point2D point, Vector2D direction, double length)
        {
            Point2D sp = new Point2D();
            Point2D ep = new Point2D();

            sp.SetFromPoint(point);
            Vector2D dir = new Vector2D();
            dir.SetFromVector(direction);
            dir.SetLength(length);

            ep.X = sp.X + dir.X;
            ep.Y = sp.Y + dir.Y;

            this.Start_Point.SetFromPoint(sp);
            this.End_Point.SetFromPoint(ep);
            this.InitPara();
        }


        /// <summary>
        /// Cross체크.
        /// </summary>
        /// <param name="pt0"></param>
        /// <param name="pt1"></param>
        /// <param name="Segment"></param>
        /// <param name="bEndPoint">끝단과의 Cross면 제외.  맞붙은 라인.</param>
        /// <returns></returns>
        public int IntersectSegment2D(ref Point2D pt0, ref Point2D pt1, Line2D Segment, bool bEndPoint = true)
        {
            int res = this.IntersectLineSegment2D(ref pt0, ref pt1, Segment, 1);

            double dMinDist = 0.001;
            if (res == 1 && bEndPoint == false)
            {
                if (pt0.DistanceToPoint(Segment.Start_Point) < dMinDist || pt0.DistanceToPoint(Segment.End_Point) < dMinDist)
                {
                    res = 0;
                }
            }

            return res;
        }

        //dmkim 170602
        /// <summary>
        /// Cross체크. (교점이 끝단인것은 제외)
        /// </summary>
        /// <param name="pt0"></param>
        /// <param name="pt1"></param>
        /// <param name="Segment"></param>
        /// <returns></returns>
        public int IntersectSegment2DForDimHatch(ref Point2D pt0, ref Point2D pt1, Line2D Segment)
        {
            int res = this.IntersectLineSegment2D(ref pt0, ref pt1, Segment, 1);

            double dMinDist = 0.001;
            if (res == 1)
            {
                if (pt0.DistanceToPoint(Segment.Start_Point) < dMinDist || pt0.DistanceToPoint(Segment.End_Point) < dMinDist ||
                    pt0.DistanceToPoint(this.Start_Point) < dMinDist || pt0.DistanceToPoint(this.End_Point) < dMinDist)
                {
                    res = 0;
                }
            }

            return res;
        }

        public int IntersectLineSegment2D(ref Point2D pt0, ref Point2D pt1, Line2D linesegment2d, int opt)
        {
            if (opt == 1)
            {
                double para = this.ToleHighRes;
                Line2D temp1 = new Line2D(this.GetPointOnSegment(-para), this.GetPointOnSegment(1.0 + para));
                Line2D temp2 = new Line2D(linesegment2d.GetPointOnSegment(-para), linesegment2d.GetPointOnSegment(1.0 + para));
                int res = temp1.IntersectLineSegment2D(ref pt0, ref pt1, temp2, -1);
                return res;
            }
            else
            {
                //Point2D pt0 = new Point2D(0, 0);
                //Point2D pt1 = new Point2D(0, 0);
                Vector2D vecseg1 = new Vector2D();
                Vector2D vecseg2 = new Vector2D();
                Vector2D vecseg2Sseg1S = new Vector2D();
                Vector2D vecseg2Sseg1E = new Vector2D();

                vecseg1.SetFromPoints(this.Start_Point, this.End_Point);
                vecseg2.SetFromPoints(linesegment2d.Start_Point, linesegment2d.End_Point);

                vecseg2Sseg1S.SetFromPoints(linesegment2d.Start_Point, this.Start_Point);
                vecseg2Sseg1E.SetFromPoints(linesegment2d.Start_Point, this.End_Point);

                double cross12 = this.CrossProduct(vecseg1, vecseg2);
                double cross13 = this.CrossProduct(vecseg1, vecseg2Sseg1S);
                double cross23 = this.CrossProduct(vecseg2, vecseg2Sseg1S);

                //dmkim TEST
                //Console.WriteLine("TEST");
                //Console.WriteLine("cross12 {0}", cross12.ToString());
                //Console.WriteLine("cross13 {0}", cross13.ToString());
                //Console.WriteLine("cross23 {0}", cross23.ToString());

                if (Math.Abs(cross12) < this.ToleHighRes) // seg1 and seg2 is parallel
                {
                    if (cross13 != 0.0 || cross23 != 0.0) // not collinear
                        return 0;
                    //they are collinear or degenerate
                    //heck if they are degenerate points
                    double dot1 = vecseg1.DotProduct(vecseg1);
                    double dot2 = vecseg2.DotProduct(vecseg2);

                    //Console.WriteLine("dot1 {0}", dot1.ToString());
                    //Console.WriteLine("dot2 {0}", dot2.ToString());

                    if (dot1 == 0 && dot2 == 0)
                    {
                        if (this.Start_Point != linesegment2d.Start_Point)
                            return 0;

                        pt0.SetFromPoint(this.Start_Point);
                        return 1;
                    }

                    if (dot1 == 0) // seg1 is a single point
                    {
                        if (linesegment2d.CheckOnSegment(this.Start_Point) == 0) // not in seg2
                            return 0;

                        pt0.SetFromPoint(this.Start_Point);
                        return 1;
                    }
                    if (dot2 == 0) // seg2 is a single point
                    {
                        if (this.CheckOnSegment(linesegment2d.Start_Point) == 0) // not in seg1
                            return 0;

                        pt0.SetFromPoint(linesegment2d.Start_Point);
                        return 1;
                    }

                    //they are collinear segments - get overlap (or not)
                    double t0 = 0.0;
                    double t1 = 0.0;
                    if (vecseg2.X != 0)
                    {
                        t0 = vecseg2Sseg1S.X / vecseg2.X;
                        t1 = vecseg2Sseg1E.X / vecseg2.X;
                    }
                    else
                    {
                        t0 = vecseg2Sseg1S.Y / vecseg2.Y;
                        t1 = vecseg2Sseg1E.Y / vecseg2.Y;
                    }

                    //Console.WriteLine("t0 {0}", t0.ToString());
                    //Console.WriteLine("t1 {0}", t1.ToString());

                    if (t0 > t1) //swap in case t1 is smaller than t0
                    {
                        double tt = t0;
                        t0 = t1;
                        t1 = tt;
                    }

                    if (t0 > 1 || t1 < 0)
                        return 0;

                    if (t0 < 0) t0 = 0;
                    if (t1 > 1) t1 = 1;

                    if (t0 == t1)
                    {
                        pt0.X = linesegment2d.Start_Point.X + vecseg2.X * t0;
                        pt0.Y = linesegment2d.Start_Point.Y + vecseg2.Y * t0;

                        return 1;
                    }

                    pt0.X = linesegment2d.Start_Point.X + vecseg2.X * t0;
                    pt0.Y = linesegment2d.Start_Point.Y + vecseg2.Y * t0;

                    pt1.X = linesegment2d.Start_Point.X + vecseg2.X * t1;
                    pt1.Y = linesegment2d.Start_Point.Y + vecseg2.Y * t1;

                    return 2;
                }

                double sI = cross23 / cross12;

                //Console.WriteLine("sI {0}", sI.ToString());

                //dmkim 160630 변경. sI < -0.0001 -> sI < -0.01 

                if (sI < -0.05 || sI > 1)
                //if (sI < -0.0001 || sI > 1)
                //if (sI < 0 || sI > 1)
                {

                    return 0;
                }

                double tI = cross13 / cross12;

                //Console.WriteLine("tI {0}", tI.ToString());

                if (tI < -0.01 || tI > 1)
                //if (tI < -0.0001 || tI > 1)
                //if (tI < 0 || tI > 1)
                {
                    return 0;
                }

                pt0.X = this.Start_Point.X + vecseg1.X * sI;
                pt0.Y = this.Start_Point.Y + vecseg1.Y * sI;

                return 1;
            }
        }
        public void IntersectArcSegment2D()
        {
        }

        public Point2D GetPointOnSegment(double para)
        {
            double length = this.Length * para;
            Vector2D vec = new Vector2D();
            vec.SetFromVector(this.Direction);
            //??
            vec.SetToUnitVector();
            vec.BlankProduct(length);
            Point2D point = new Point2D(this.Start_Point.X + vec.X, this.Start_Point.Y + vec.Y);
            return point;
        }

        public double CrossProduct(Vector2D vec1, Vector2D vec2)
        {
            return vec1.X * vec2.Y - vec1.Y * vec2.X;
        }

        public int CheckOnSegment(Point2D pnt)
        {
            Vector2D vec1 = new Vector2D();
            Vector2D vec2 = new Vector2D();

            vec1.SetFromPoints(pnt, this.Start_Point);
            vec2.SetFromPoints(pnt, this.End_Point);

            if (vec1.Length() < this.ToleLowRes || vec2.Length() < this.ToleLowRes)
                return 1;

            double dot = vec1.DotProduct(vec2);

            if (dot < 0.0)
            {
                if (Math.Abs(Math.Abs(dot / (vec1.Length() * vec2.Length())) - 1.0) < this.ToleMidRes)
                    return 1;
            }
            else return 0;

            //??
            return 0;
        }

        public Point2D GetPointFromU(double u)
        {
            return this.GetPointOnSegment(u);
        }
        public void Print()
        {
            Console.WriteLine("##########   LINE 2D   ##########");
            Console.WriteLine("Start_Point X : {0}, Y : {1}", this.Start_Point.X, this.Start_Point.Y);
            Console.WriteLine("End_Point   X : {0}, Y : {1}", this.End_Point.X, this.End_Point.Y);
            Console.WriteLine("Direction   X : {0}, Y : {1}", this.Direction.X, this.Direction.Y);
            Console.WriteLine("Length    : {0}", this.Length);
        }

        public void Print(string message)
        {
            Console.WriteLine("##########   {0}   ##########", message);
            Console.WriteLine("Start_Point X : {0}, Y : {1}", this.Start_Point.X, this.Start_Point.Y);
            Console.WriteLine("End_Point   X : {0}, Y : {1}", this.End_Point.X, this.End_Point.Y);
            Console.WriteLine("Direction   X : {0}, Y : {1}", this.Direction.X, this.Direction.Y);
            Console.WriteLine("Length    : {0}", this.Length);
        }
    }
}
