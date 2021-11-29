using INFOGET_ZERO_HULL.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INFOGET_ZERO_HULL.Geometry
{
    public class PlaneName
    {
        Point3D MainPoint = new Point3D();
        Vector3D normalvector = new Vector3D();
        string PlaneNames = string.Empty;

        public Vector3D getNormal()
        {
            return normalvector;
        }
        public string getName()
        {
            return PlaneNames;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <param name="E"></param>
        /// <param name="F"></param>
        public PlaneName(double A, double B, double C, double D, double E, double F, string name)
        {
            Vector3D normal = new Vector3D(A, B, C);
            normal.SetToUnitVector();
            normalvector = normal;
            PlaneNames = name;

            MainPoint = new Point3D(D, E, F);
        }

        public PlaneName()
        {

        }
    }
}
