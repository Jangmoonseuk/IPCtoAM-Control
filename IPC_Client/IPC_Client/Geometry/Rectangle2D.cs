using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INFOGET_ZERO_HULL.Geometry
{
    public class Rectangle2D
    {
        public Point2D Corner1 = new Point2D();
        public Point2D Corner2 = new Point2D();

        public Rectangle2D()
        {
        }

        public Rectangle2D(Point2D pnt1, Point2D pnt2)
        {
            this.Corner1.SetCoordinates(pnt1.X, pnt1.Y);
            this.Corner2.SetCoordinates(pnt2.X, pnt2.Y);
        }
        public Rectangle2D(double pnt1x, double pnt1y, double pnt2x, double pnt2y)
        {
            this.Corner1.SetCoordinates(pnt1x, pnt1y);
            this.Corner2.SetCoordinates(pnt2x, pnt2y);
        }
    }
}
