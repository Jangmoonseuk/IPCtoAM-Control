using INFOGET_ZERO_HULL.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INFOGET_ZERO_HULL.Geometry
{
    public class IGES
    {
        public PlaneName Plane = new PlaneName();
        public List<Line3D> Lines = new List<Line3D>();

        public IGES()
        {

        }
        public void SetPlane(List<string> a, string name)
        {
            List<double> setDo = new List<double>() { double.Parse(a[1]), double.Parse(a[2]), double.Parse(a[3]), double.Parse(a[6]), double.Parse(a[7]), double.Parse(a[8].Replace(';', ' ').Trim()) };

            Plane = new PlaneName(setDo[0], setDo[1], setDo[2], setDo[3], setDo[4], setDo[5], name);
        }
        public void SetLine(List<string> a)
        {
            List<double> setDO = new List<double>() { double.Parse(a[1]), double.Parse(a[2]), double.Parse(a[3]), double.Parse(a[4]), double.Parse(a[5]), double.Parse(a[6].Replace(';', ' ').Trim()) };

            Point3D start = new Point3D(setDO[0], setDO[1], setDO[2]);
            Point3D end = new Point3D(setDO[3], setDO[4], setDO[5]);
            Line3D line3D = new Line3D(start, end);

            Lines.Add(line3D);
        }
    }
}
