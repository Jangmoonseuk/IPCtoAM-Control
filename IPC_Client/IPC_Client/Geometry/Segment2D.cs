using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INFOGET_ZERO_HULL.Geometry
{
    public class Segment2D
    {
        public string Type = "Line"; //Arc
        public Line2D linesegment = new Line2D { };
        public Arc2D arcsegment = new Arc2D { };

        //dmkim 170320
        /// <summary>
        /// 기타사용을 위한. 
        /// 1. 해칭라인에서 기분 라인은 그리는것 제외하기 위해서.
        /// </summary>
        public string sMsg = string.Empty;

        public Segment2D(string type, Line2D linesegment)
        {
            this.Type = type;
            this.linesegment = linesegment;
        }
        public Segment2D(string type, Arc2D arcsegment)
        {
            this.Type = type;
            this.arcsegment = arcsegment;
        }

        public void Print()
        {
            Console.WriteLine("##########   Segment2D   ##########");
            Console.WriteLine("Type    : {0}", this.Type);
            if (this.Type == "Line")
            {
                Console.WriteLine("Start_Point X : {0}, Y : {1}", this.linesegment.Start_Point.X, this.linesegment.Start_Point.Y);
                Console.WriteLine("End_Point   X : {0}, Y : {1}", this.linesegment.End_Point.X, this.linesegment.End_Point.Y);
                Console.WriteLine("Direction   X : {0}, Y : {1}", this.linesegment.Direction.X, this.linesegment.Direction.Y);
                Console.WriteLine("Length    : {0}", this.linesegment.Length);
            }
            else
            {
                Console.WriteLine("Center X : {0}, Y : {1}", this.arcsegment.Center.X, this.arcsegment.Center.Y);
                Console.WriteLine("Amplitude    : {0}", this.arcsegment.Amplitude);
                Console.WriteLine("Angle        : {0}", this.arcsegment.Angle);
                Console.WriteLine("Length       : {0}", this.arcsegment.Length);
            }


        }

    }
}
