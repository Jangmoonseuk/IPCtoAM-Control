using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INFOGET_ZERO_HULL.Geometry
{
    public class Circle2D
    {
        public static Circle2D Instance = new Circle2D();

        public Point2D Centre = new Point2D();
        public double radius = 0.0;

        public string linetype = "SolidXWide";
        public string colour = "Black";

        public Circle2D()
        {
        }

        public Circle2D(Point2D inpoint, double rad)
        {
            this.Centre.SetCoordinates(inpoint.X, inpoint.Y);
            this.radius = rad;
        }

        public void Assign(Point2D that)
        {
        }

    }
}
