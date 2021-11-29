using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INFOGET_ZERO_HULL.Geometry
{
    public  class Polyline2D
    {
        public class Vertex
        {
            public Point2D oPoint = new Point2D();
            public double dBulgeFactor = 0.0;

            public Vertex(Point2D oPoint)
            {
                this.oPoint.SetFromPoint(oPoint);
            }
            public Vertex(Point2D oPoint, double dBulgeFactor)
            {
                this.oPoint.SetFromPoint(oPoint);
                this.dBulgeFactor = dBulgeFactor;
            }
        }

        public int iLineStype = LineType.GetNoOfLineType(LineType.SOLID);
        public int iColour = Colour.GetNoOfColour(Colour.BLACK);

        public List<Vertex> olVertex = new List<Vertex> { };

        public Polyline2D()
        {

        }
        public Polyline2D(Point2D oStartPoint)
        {
            this.Add(oStartPoint);
        }
        public Polyline2D(Point2D oStartPoint, string sLineType, string sColour)
        {
            this.Add(oStartPoint);
            this.iLineStype = LineType.GetNoOfLineType(sLineType);
            this.iColour = Colour.GetNoOfColour(sColour);

        }
        public void Add(Point2D oPoint)
        {
            this.olVertex.Add(new Vertex(oPoint));
        }
        public void Add(Point2D oPoint, double dBulgeFactor)
        {
            this.olVertex.Add(new Vertex(oPoint, dBulgeFactor));
        }
        public void Add(Point2D oPoint, string sLineType, string sColour)
        {
            this.olVertex.Add(new Vertex(oPoint));
            this.iLineStype = LineType.GetNoOfLineType(sLineType);
            this.iColour = Colour.GetNoOfColour(sColour);
        }
        public void Add(Point2D oPoint, double dBulgeFactor, string sLineType, string sColour)
        {
            this.olVertex.Add(new Vertex(oPoint, dBulgeFactor));
            this.iLineStype = LineType.GetNoOfLineType(sLineType);
            this.iColour = Colour.GetNoOfColour(sColour);
        }
    }
}
