using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INFOGET_ZERO_HULL.Geometry
{
    public class Transformation3D
    {
        public static Transformation3D Instance = new Transformation3D();

        public int type;
        public double matrix11;
        public double matrix12;
        public double matrix13;
        public double matrix14;
        public double matrix21;
        public double matrix22;
        public double matrix23;
        public double matrix24;
        public double matrix31;
        public double matrix32;
        public double matrix33;
        public double matrix34;
        public double matrix41;
        public double matrix42;
        public double matrix43;
        public double matrix44;

        public Transformation3D()
        {
            type = 1;
            matrix11 = 1.0;
            matrix12 = 0.0;
            matrix13 = 0.0;
            matrix14 = 0.0;
            matrix21 = 0.0;
            matrix22 = 1.0;
            matrix23 = 0.0;
            matrix24 = 0.0;
            matrix31 = 0.0;
            matrix32 = 0.0;
            matrix33 = 1.0;
            matrix34 = 0.0;
            matrix41 = 0.0;
            matrix42 = 0.0;
            matrix43 = 0.0;
            matrix44 = 1.0;
        }
        //OK
        public void SetFromTransformation(Transformation3D tra)
        {
            this.type = tra.type;
            this.matrix11 = tra.matrix11;
            this.matrix12 = tra.matrix12;
            this.matrix13 = tra.matrix13;
            this.matrix14 = tra.matrix14;
            this.matrix21 = tra.matrix21;
            this.matrix22 = tra.matrix22;
            this.matrix23 = tra.matrix23;
            this.matrix24 = tra.matrix24;
            this.matrix31 = tra.matrix31;
            this.matrix32 = tra.matrix32;
            this.matrix33 = tra.matrix33;
            this.matrix34 = tra.matrix34;
            this.matrix41 = tra.matrix41;
            this.matrix42 = tra.matrix42;
            this.matrix43 = tra.matrix43;
            this.matrix44 = tra.matrix44;
        }
        //OK
        public void Combine(Transformation3D tra)
        {
            double[][] M1 = new double[4][];
            M1[0] = new double[] { 0.0, 0.0, 0.0, 0.0 };
            M1[1] = new double[] { 0.0, 0.0, 0.0, 0.0 };
            M1[2] = new double[] { 0.0, 0.0, 0.0, 0.0 };
            M1[3] = new double[] { 0.0, 0.0, 0.0, 0.0 };

            double[][] M2 = new double[4][];
            M2[0] = new double[] { 0.0, 0.0, 0.0, 0.0 };
            M2[1] = new double[] { 0.0, 0.0, 0.0, 0.0 };
            M2[2] = new double[] { 0.0, 0.0, 0.0, 0.0 };
            M2[3] = new double[] { 0.0, 0.0, 0.0, 0.0 };

            double[][] MOUT = new double[4][];
            MOUT[0] = new double[] { 0.0, 0.0, 0.0, 0.0 };
            MOUT[1] = new double[] { 0.0, 0.0, 0.0, 0.0 };
            MOUT[2] = new double[] { 0.0, 0.0, 0.0, 0.0 };
            MOUT[3] = new double[] { 0.0, 0.0, 0.0, 0.0 };
            
            int t1 = 0;
            int t2 = 0;

            this.GetByRow(ref t1, ref M1);
            tra.GetByRow(ref t2,ref M2);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    double sum = 0.0;
                    for (int k = 0; k < 4; k++)
                    {
                        sum = sum + M1[i][k] * M2[k][j];
                    }
                    MOUT[i][j] = sum;
                }
            }
            int tout;
            if (this.type > tra.type)
            {
                tout = this.type;
            }
            else
            {
                tout = tra.type;
            }
            this.SetByRow(tout, MOUT);
            




        }

        public void GetByRow(ref int t,ref double[][] M)
        {
            t = this.type;
            M[0][0] = this.matrix11;
            M[0][1] = this.matrix12;
            M[0][2] = this.matrix13;
            M[0][3] = this.matrix14;
            M[1][0] = this.matrix21;
            M[1][1] = this.matrix22;
            M[1][2] = this.matrix23;
            M[1][3] = this.matrix24;
            M[2][0] = this.matrix31;
            M[2][1] = this.matrix32;
            M[2][2] = this.matrix33;
            M[2][3] = this.matrix34;
            M[3][0] = this.matrix41;
            M[3][1] = this.matrix42;
            M[3][2] = this.matrix43;
            M[3][3] = this.matrix44;

        }

        public void SetByRow(int t, double[][] M)
        {
            this.type = t;
            this.matrix11 = M[0][0] + 0.0;
            this.matrix12 = M[0][1] + 0.0;
            this.matrix13 = M[0][2] + 0.0;
            this.matrix14 = M[0][3] + 0.0;
            this.matrix21 = M[1][0] + 0.0;
            this.matrix22 = M[1][1] + 0.0;
            this.matrix23 = M[1][2] + 0.0;
            this.matrix24 = M[1][3] + 0.0;
            this.matrix31 = M[2][0] + 0.0;
            this.matrix32 = M[2][1] + 0.0;
            this.matrix33 = M[2][2] + 0.0;
            this.matrix34 = M[2][3] + 0.0;
            this.matrix41 = M[3][0] + 0.0;
            this.matrix42 = M[3][1] + 0.0;
            this.matrix43 = M[3][2] + 0.0;
            this.matrix44 = M[3][3] + 0.0;

        }

        //OK
        public void SetFromPointAndTwoVectors(Point3D p0, Vector3D U, Vector3D V)
        {
            this.type = 1;

            //update U Vector
            double length = U.Length();
            this.matrix11 = U.X / length;
            this.matrix12 = U.Y / length;
            this.matrix13 = U.Z / length;
            this.matrix14 = 0.0;

            //update V Vector
            length = V.Length();
            this.matrix21 = V.X / length;
            this.matrix22 = V.Y / length;
            this.matrix23 = V.Z / length;
            this.matrix24 = 0.0;

            //update W - vector by calculation cross product of U and V
            Vector3D W = new Vector3D();
            W.SetFromCrossProduct(U, V);
            length = W.Length();
            this.matrix31 = W.X / length;
            this.matrix32 = W.Y / length;
            this.matrix33 = W.Z / length;
            this.matrix34 = 0.0;

            //update the origo
            this.matrix41 = p0.X;
            this.matrix42 = p0.Y;
            this.matrix43 = p0.Z;
            this.matrix44 = 1.0;

        }
        //Ok
        public void SetFromPointAndThreeVectors(Point3D p0, Vector3D uvec, Vector3D vvec, Vector3D wvec)
        {
            this.type = 1;
            this.matrix11= uvec.X;
            this.matrix12= uvec.Y;
            this.matrix13= uvec.Z;
            this.matrix14= 0.0;
            this.matrix21= vvec.X;
            this.matrix22= vvec.Y;
            this.matrix23= vvec.Z;
            this.matrix24= 0.0;
            this.matrix31= wvec.X;
            this.matrix32= wvec.Y;
            this.matrix33= wvec.Z;
            this.matrix34= 0.0;
            this.matrix41= p0.X;
            this.matrix42= p0.Y;
            this.matrix43= p0.Z;
            this.matrix44 = 1.0;
        }
        //OK
        public void Invert()
        {
            double[][] M = new double[4][];
            M[0] = new double[] { 0.0, 0.0, 0.0, 0.0 };
            M[1] = new double[] { 0.0, 0.0, 0.0, 0.0 };
            M[2] = new double[] { 0.0, 0.0, 0.0, 0.0 };
            M[3] = new double[] { 0.0, 0.0, 0.0, 0.0 };

            int t = 0;
            this.GetByRow(ref t, ref M);

            this.matrix11 = M[0][0];
            this.matrix21 = M[0][1];
            this.matrix31 = M[0][2];
            this.matrix12 = M[1][0];
            this.matrix22 = M[1][1];
            this.matrix32 = M[1][2];
            this.matrix13 = M[2][0];
            this.matrix23 = M[2][1];
            this.matrix33 = M[2][2];
            this.matrix14 = 0.0;
            this.matrix24 = 0.0;
            this.matrix34 = 0.0;
            this.matrix41 = (-M[3][0]*M[0][0]-M[3][1]*M[0][1]-M[3][2]*M[0][2])/M[3][3];
            this.matrix42 = (-M[3][0]*M[1][0]-M[3][1]*M[1][1]-M[3][2]*M[1][2])/M[3][3];
            this.matrix43 = (-M[3][0]*M[2][0]-M[3][1]*M[2][1]-M[3][2]*M[2][2])/M[3][3];
            this.matrix44 = 1.0 / M[3][3];
        }

        public void ReflectX()
        {
            Transformation3D T = new Transformation3D();
            T.matrix11 = -1.0;
            this.Combine(T);
        }
        public void ReflectY()
        {
            Transformation3D T = new Transformation3D();
            T.matrix22 = -1.0;
            this.Combine(T);
        }
        public void ReflectZ()
        {
            Transformation3D T = new Transformation3D();
            T.matrix33 = -1.0;
            this.Combine(T);
        }
        //OK
        public void Rotate(Point3D point, Vector3D axis, double angle)
        {
            Vector3D w_vec = new Vector3D(axis.X, axis.Y, axis.Z);
            w_vec.SetToUnitVector();

            //calculate vector perpendicular to rotation axis
            Vector3D u_vec = new Vector3D(0.0, 0.0, 0.0);
            if (Math.Abs(w_vec.X) + Math.Abs(w_vec.Y) > 1.0E-15)
            {
               u_vec.X = w_vec.Y;
               u_vec.Y = w_vec.X;
            }
            else
            {
               u_vec.Y = -w_vec.Z;
               u_vec.Z = w_vec.Y;
            }
    
           //calculate the third vector
            Vector3D v_vec = new Vector3D(0.0, 0.0, 0.0);
            v_vec.SetFromCrossProduct(w_vec, u_vec);
    
           //create a transformation matrix from uvw-xyz
            Transformation3D T2 = new Transformation3D();
            T2.SetFromPointAndTwoVectors(point, u_vec, v_vec);
    
           //create a transformation matrix for rotation
            Transformation3D T3 = new Transformation3D();
            T3.matrix11 = Math.Cos(angle);
            T3.matrix12 = Math.Sin(angle);
            T3.matrix21 = -1*Math.Sin(angle);
            T3.matrix22 = Math.Cos(angle);
    
           //Transformation xyz-uvw
           //Rotation
           //Transformation uvw-xyz
            T3.Combine(T2);
            T2.Invert();
            T2.Combine(T3);
    
           //combine self with the result of rotation
            this.Combine(T2);
            
        }
        //OK
        public void Translate(Vector3D transvec)
        {
            Transformation3D T = new Transformation3D();
            T.matrix41 = transvec.X;
            T.matrix42 = transvec.Y;
            T.matrix43 = transvec.Z;
            this.Combine(T);
        }
        //OK
        public Transformation3D SetToTranformLG(Point3D originpt, Vector3D zdir, Vector3D xdir)
        {
            zdir.SetToUnitVector();
            xdir.SetToUnitVector();

            Vector3D vvec = new Vector3D();
            vvec.SetFromCrossProduct(zdir, xdir);

            this.SetFromPointAndThreeVectors(originpt, xdir, vvec, zdir);

            Transformation3D TransformMatrixLG = new Transformation3D();
            TransformMatrixLG.SetFromTransformation(this);

            return TransformMatrixLG;
        }
        //OK
        public Transformation3D SetToTranformGL(Point3D originpt, Vector3D zdir, Vector3D xdir)
        {
            this.SetFromTransformation(this.SetToTranformLG(originpt,zdir,xdir));
            this.Invert();

            Transformation3D TransformMatrixGL = new Transformation3D();
            TransformMatrixGL.SetFromTransformation(this);

            return TransformMatrixGL;
        }
        //OK
        public void Scale(double scale)
        {
            Transformation3D T = new Transformation3D();
            T.matrix44 = 1 / scale;
            this.Combine(T);
        }
        public List<Vector3D> GetUVWOrigin(ref Point3D Origin)
        {
            Vector3D U = new Vector3D(this.matrix11, this.matrix12, this.matrix13);
            Vector3D V = new Vector3D(this.matrix21, this.matrix22, this.matrix23);
            Vector3D W = new Vector3D(this.matrix31, this.matrix32, this.matrix33);

            U.SetToUnitVector();
            V.SetToUnitVector();
            W.SetToUnitVector();

            Origin.SetCoordinates(this.matrix41, this.matrix42, this.matrix43);

            List<Vector3D> rtnlist = new List<Vector3D> { U, V, W };

            return rtnlist;

        }

        public void Print()
        {
            Console.WriteLine("\n Transformation3D ");
            Console.WriteLine("\t Type : {0}",this.type.ToString());
            Console.WriteLine("\t [{0},{1},{2},{3}]",
                this.matrix11.ToString(), this.matrix12.ToString(), this.matrix13.ToString(), this.matrix14.ToString());
            Console.WriteLine("\t [{0},{1},{2},{3}]",
                this.matrix21.ToString(), this.matrix22.ToString(), this.matrix23.ToString(), this.matrix24.ToString());
            Console.WriteLine("\t [{0},{1},{2},{3}]",
                this.matrix31.ToString(), this.matrix32.ToString(), this.matrix33.ToString(), this.matrix34.ToString());
            Console.WriteLine("\t [{0},{1},{2},{3}] \n",
                this.matrix41.ToString(), this.matrix42.ToString(), this.matrix43.ToString(), this.matrix44.ToString());
        }
    }
}
