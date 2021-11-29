using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aveva.Pdms.Database;

using A = INFOGET_ZERO_HULL.AM.AvevaUtil;
using AM_ATT = Aveva.Pdms.Database.DbAttributeInstance;
using ATT = INFOGET_ZERO_HULL.AM.AvevaAtt;
using ATT_UDA = INFOGET_ZERO_HULL.AM.AvevaUDA;
using ATT_MIX = INFOGET_ZERO_HULL.AM.AvaveMixAtt;
//using G = INFOGET.Default.DefaultGlobal;
using INFOGET_ZERO_HULL.Geometry;

using Aveva.Pdms.Geometry;

namespace INFOGET_ZERO_HULL.AM
{
    /// <summary>
    /// AM Attribute General
    /// </summary>
    public static class AvevaAtt
    {
        //////////////////////////////////////////////////////////////////////////
        /// Attr정보가져오는 부분에서 에러가나면 
        /// 해당타입에 호출한 Atrribute 가 있는지 확인.
        /// 
        //////////////////////////////////////////////////////////////////////////


        public static string UNSET = "unset";
        public static string NULL = "";


        public static class IsValid
        {
            public static bool NCOF(DbElement rPart)
            {
                Aveva.Pdms.Geometry.Position oPos = Aveva.Pdms.Geometry.Position.Create();
                return rPart.GetValidPosition(AM_ATT.NCOF, ref oPos);
            }
            public static bool BRCOG(DbElement rPart)
            {
                Aveva.Pdms.Geometry.Position oPos = Aveva.Pdms.Geometry.Position.Create();
                return rPart.GetValidPosition(AM_ATT.BRCOG, ref oPos);
            }
        }

        /// <summary>
        /// 해당 Attribute 속성이 있는 지 체크 후 값 리턴.
        /// </summary>
        public static class Valid
        {
            public static string PARNAM(DbElement rPart) { return rPart.IsAttributeValid(AM_ATT.PARNAM) ? ATT.PARNAM(rPart) : string.Empty; }
            public static string SPLN(DbElement rPart) { return rPart.IsAttributeValid(AM_ATT.SPLN) ? ATT.SPLN(rPart) : string.Empty; }
            public static string SPLT(DbElement rPart) { return rPart.IsAttributeValid(AM_ATT.SPLT) ? ATT.SPLT(rPart) : string.Empty; }
            public static string PRTIDL(DbElement rPart) { return rPart.IsAttributeValid(AM_ATT.PRTIDL) ? ATT.PRTIDL(rPart) : string.Empty; }
            public static string PURP(DbElement rPart) { return rPart.IsAttributeValid(AM_ATT.PURP) ? ATT.PURP(rPart) : string.Empty; }
        }

        /// <summary>
        /// Arrive excess for leave tube (AM12.SP4)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double AEXCES(DbElement rPart)
        {
            double dRtn = 0.0;

            try
            {
                dRtn = rPart.GetDouble(AM_ATT.AREAEX);//AEXCES
            }
            catch
            {
            }
            return dRtn;
        }

        /// <summary>
        /// Allowance
        /// FLAN,WELD
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double ALLO(DbElement rPart)
        {
            double dRtn = 0.0;

            try
            {
                dRtn = rPart.GetDouble(AM_ATT.ALLO);
            }
            catch
            {
            }
            return dRtn;
        }



        /// <summary>
        /// ANGLE
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double ANGL(DbElement rPart)
        {
            double dRtn = 0.0;

            try
            {
                dRtn = rPart.GetDouble(AM_ATT.ANGL);
            }
            catch
            {
            }
            return dRtn;
        }


        /// <summary>
        /// Answer (Real)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double ANSW(DbElement rPart)
        {
            return rPart.GetDouble(AM_ATT.ANSW);
        }

        /// <summary>
        /// Get Arrive Ppoint
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static int ARRI(DbElement rPart)
        {
            return rPart.GetInteger(AM_ATT.ARRI);
        }

        /// <summary>
        /// Get Attachment Type
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string ATTY(DbElement rPart)
        {
            string sRtn = string.Empty;

            if (AvevaAtt.TYPE(rPart) == "ATTA")
            {
                sRtn = rPart.GetAsString(AM_ATT.ATTY);
            }
            return sRtn;
        }


        /// <summary>
        /// Bolt Diameter (BLTP)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string BDIA(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetString(AM_ATT.BDIA);

                sRtn = sRtn.Replace("(", "").Replace(")", "").Trim();

                //dmkim 171101 Data 보정.
                //( 0.625 in ) 이런값이 들어가 있는 경우도 있음.
                sRtn = sRtn.Replace("in", "").Replace("IN", "").Trim();
            }
            catch
            {
            }
            return sRtn;
        }

        //dmkim 190502 
        /// <summary>
        /// Bolt Diameter (BLTP)
        /// </summary>
        /// <param name="rPart"></param>
        /// <param name="bGetDiameterInch"></param>
        /// <returns></returns>
        public static string BDIA_Bolt(DbElement rPart, ref bool bGetDiameterInch)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetString(AM_ATT.BDIA);

                sRtn = sRtn.Replace("(", "").Replace(")", "").Trim();

                //dmkim 171101 Data 보정.
                //( 0.625 in ) 이런값이 들어가 있는 경우도 있음.
                //dmkim 190502
                if (sRtn.Contains("in") || sRtn.Contains("IN")) { bGetDiameterInch = true; }
                sRtn = sRtn.Replace("in", "").Replace("IN", "").Trim();
            }
            catch
            {
            }
            return sRtn;
        }


        /// <summary>
        /// PSPOOL 일 경우만.
        /// 포함하는 아이템 가져오기.
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static List<DbElement> BELRFA(DbElement rPart)
        {
            List<DbElement> olrRtn = new List<DbElement> { };

            DbElement[] olrPart = rPart.GetElementArray(AM_ATT.BELRFA);

            foreach (DbElement rTmpPart in olrPart) olrRtn.Add(rTmpPart);

            return olrRtn;
        }

        /// <summary>
        /// Bolt Item Lengths
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double[] BITL(DbElement rPart)
        {
            double[] olRtn = new double[] { };
            try
            {
                olRtn = rPart.GetDoubleArray(AM_ATT.BITL);
            }
            catch { }
            return olRtn;
        }

        /// <summary>
        /// Bolt Length
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double[] BLEN(DbElement rPart)
        {
            double[] olRtn = new double[] { };
            try
            {
                olRtn = rPart.GetDoubleArray(AM_ATT.BLEN);
            }
            catch { }
            return olRtn;
        }


        /// <summary>
        /// Bolt Reference
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement BLTR(DbElement rPart)
        {
            DbElement rRtn = DbElement.GetElement();
            try
            {
                rRtn = rPart.GetElement(AM_ATT.BLTR);
            }
            catch
            { }
            return rRtn;
        }

        /// <summary>
        /// Branch COG
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double[] BRCOG(DbElement rPart)
        {
            return rPart.GetDoubleArray(AM_ATT.BRCOG);
        }

        /// <summary>
        /// Bolt selector
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string BSEL(DbElement rPart)
        {
            return rPart.GetString(AM_ATT.BSEL);
        }


        /// <summary>
        /// Bolt specification
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement BSPE(DbElement rPart)
        {
            return rPart.GetElement(AM_ATT.BSPE);
        }

        /// <summary>
        /// TEXP의 Annotation text string
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string BTEX(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetString(AM_ATT.BTEX);
            }
            catch
            {
            }
            return sRtn;
        }

        /// <summary>
        /// Bolt Thickness (BLTP)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string BTHK(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetString(AM_ATT.BTHK);
            }
            catch
            {
            }
            return sRtn;
        }
        /// <summary>
        /// Bolt Thickness (BLTP)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double THICKN(DbElement rPart)
        {
            double sRtn = 0;

            try
            {
                sRtn = rPart.GetDouble(AM_ATT.THICKN);
            }
            catch(Exception ex)
            {

            }
            return sRtn;
        }

        /// <summary>
        /// Bolt Type (BLTP)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string BTYP(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetString(AM_ATT.BTYP);
            }
            catch
            {
            }
            return sRtn;
        }

        //dmkim 200101
        /// <summary>
        /// Bulge Factor
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double BULG(DbElement rPart)
        {
            return rPart.GetDouble(AM_ATT.BULG);
        }

        /// <summary>
        /// Component reference
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement CMPR(DbElement rPart)
        {
            DbElement rRtn = DbElement.GetElement();
            try
            {
                rRtn = rPart.GetElement(AM_ATT.CMPR);
            }
            catch
            { }
            return rRtn;
        }


        /// <summary>
        /// Connection Reference
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement CREF(DbElement rPart)
        {
            DbElement rRtn = DbElement.GetElement();
            try
            {
                rRtn = rPart.GetElement(AM_ATT.CREF);
            }
            catch
            { }
            return rRtn;
        }

        //dmkim 190211
        /// <summary>
        /// Connection Reference Array
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static List<DbElement> CRFA(DbElement rPart)
        {
            List<DbElement> olrRtn = new List<DbElement> { };
            try
            {
                DbElement[] olrPart = rPart.GetElementArray(AM_ATT.CRFA);

                foreach (DbElement rTmpPart in olrPart)
                {
                    if (A.IsValidElement(rTmpPart))
                    {
                        olrRtn.Add(rTmpPart);
                    }

                }
            }
            catch { }
            return olrRtn;
        }

        //dmkim 190722
        /// <summary>
        /// Connection Reference Array, with Index
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static Dictionary<int, DbElement> CRFA_INDEX(DbElement rPart)
        {
            Dictionary<int, DbElement> dicRtn = new Dictionary<int, DbElement> { };
            try
            {
                DbElement[] olrPart = rPart.GetElementArray(AM_ATT.CRFA);

                for (int i = 0; i < olrPart.Length; i++)
                {
                    DbElement rTmpPart = olrPart[i];
                    if (A.IsValidElement(rTmpPart))
                    {
                        dicRtn.Add(i + 1, rTmpPart);
                    }

                }
            }
            catch { }
            return dicRtn;
        }


        /// <summary>
        /// Total component weight (Pipe 제외한 모든 피팅)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double CWEI(DbElement rPart)
        {
            double dRtn = 0.0;

            try
            {
                //dmkim 190810
                string sValue = rPart.GetString(AM_ATT.CWEI);

                DbDouble oValue = DbDouble.Create(sValue.Replace("(", "").Replace(")", ""));
                DbDoubleUnits oUnit = DbDoubleUnits.GetUnits("KG");
                oValue = oValue.ConvertUnits(oUnit);

                dRtn = oValue.Value;
            }
            catch
            {
            }
            return dRtn;
        }

        public static string DESC(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetAsString(AM_ATT.DESC);
            }
            catch
            {
            }
            return sRtn;
        }

        /// <summary>
        /// Diameter
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double DIAM(DbElement rPart)
        {
            double dRtn = 0.0;

            try
            {
                dRtn = rPart.GetDouble(AM_ATT.DIAM);
            }
            catch
            {
            }
            return dRtn;
        }


        public static string DOFIL(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetAsString(AM_ATT.DOFIL);
            }
            catch
            {
            }
            return sRtn;
        }

        /// <summary>
        /// Data Title
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string DTIT(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetString(AM_ATT.DTIT);
            }
            catch
            {
            }
            return sRtn;
        }

        /// <summary>
        /// Data set Reference
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement DTRE(DbElement rCatref)
        {
            //////////////////////////////////////////////////////////////////////////
            /// MATR 이 없을 경우 에러남.

            DbElement rRtn = DbElement.GetElement();
            try
            {
                rRtn = rCatref.GetElement(AM_ATT.DTRE);
            }
            catch
            {
            }
            return rRtn;
        }


        /// <summary>
        /// DTXR (RText에서 가져오면 Parameter값이 표현되어서 사용할 수 없음)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string DTXR(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetString(AM_ATT.DTXR);
            }
            catch
            {
            }
            return sRtn;
        }
        public static string DTXS(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetString(AM_ATT.DTXS);
            }
            catch
            {
            }
            return sRtn;
        }
        public static string DTXT(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetString(AM_ATT.DTXT);
            }
            catch
            {
            }
            return sRtn;
        }

        /// <summary>
        /// DKEY (DTSE의(DTREF of CATREF)의 MEMBER인 DATA의 속성값.)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string DKEY(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetString(AM_ATT.DKEY);
            }
            catch
            {
            }
            return sRtn;
        }


        /// <summary>
        /// TEXP의 Example Text
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string ETEX(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetString(AM_ATT.ETEX);
            }
            catch
            {
            }
            return sRtn;
        }

        /// <summary>
        /// Fitting 길이.
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double FITLEN(DbElement rPart)
        {
            double dRtn = 0.0;

            try
            {
                dRtn = rPart.GetDouble(AM_ATT.FITLEN);
            }
            catch
            {
            }
            return dRtn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rPart"></param>
        /// <param name="bContainSlash">"/" 포함여부.</param>
        /// <returns></returns>
        public static string FLNM(DbElement rPart, bool bContainSlash = true)
        {
            string sRtn = string.Empty;
            if (bContainSlash) { sRtn = rPart.GetString(AM_ATT.FLNM); }
            else { sRtn = rPart.GetString(AM_ATT.FLNN); }
            return sRtn;
        }

        public static string OWNER(DbElement rPart, bool bContainSlash = true)
        {
            string sRtn = string.Empty;
            if (bContainSlash) { sRtn = rPart.GetString(AM_ATT.OWNER); }
            else { sRtn = rPart.GetString(AM_ATT.OWNER); }
            return sRtn;
        }

        /// <summary>
        /// DB foreign/local
        /// DB의 DBELEMENT의 값에서 가져온다. EX)*PIPE/1DK_AFT
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns>FOREIGN, LOCAL</returns>
        public static string FOREDB(DbElement rPart)
        {
            string sRtn = string.Empty;
            try
            {
                sRtn = rPart.GetString(AM_ATT.FOREDB);
            }
            catch { }
            return sRtn;
        }

        /// <summary>
        /// Fillet Radius
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double FRAD(DbElement rPart)
        {
            double dRtn = 0.0;

            try
            {
                dRtn = rPart.GetDouble(AM_ATT.FRAD);
            }
            catch
            {
            }
            return dRtn;
        }

        /// <summary>
        /// FUNCTION
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string FUNC(DbElement rPart)
        {
            return rPart.GetString(AM_ATT.FUNC);
        }

        /// <summary>
        /// Generic Type
        /// </summary>
        /// <param name="rPart">Catref</param>
        /// <returns></returns>
        public static string GTYP(DbElement rPart)
        {
            return rPart.GetString(AM_ATT.GTYP);
        }

        public static double GWEI(DbElement rPart)
        {
            double dRtn = 0.0;
            try
            {
                dRtn = rPart.GetDouble(AM_ATT.GWEI);
            }
            catch
            {
            }
            return dRtn;
        }


        public static double HEIG(DbElement rPart)
        {
            double dRtn = 0.0;
            try
            {
                dRtn = rPart.GetDouble(AM_ATT.HEIG);
            }
            catch
            {
            }
            return dRtn;
        }

        /// <summary>
        /// BRANCH의 Head Reference
        /// Null Element Return 시 값 체크 주의할 것.
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement HREF(DbElement rPart)
        {
            return rPart.GetElement(AM_ATT.HREF);
        }

        /// <summary>
        /// DRAWLIST ELEMENT
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement IDLN(DbElement rPart)
        {
            return rPart.GetElement(AM_ATT.IDLN);
        }

        /// <summary>
        /// True if element is named
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static bool ISNAME(DbElement rPart)
        {
            bool bRtn = false;

            try
            {
                bRtn = rPart.GetBool(AM_ATT.ISNAME);
            }
            catch
            {
            }
            return bRtn;
        }

        /// <summary>
        /// Table linking AITEMS elements to piping components
        /// </summary>
        /// <param name="rPart">AITEMS</param>
        /// <returns></returns>
        public static List<int> ITMTBL(DbElement rPart)
        {
            List<int> olRtn = new List<int> { };

            try
            {
                int[] olTempRtn = rPart.GetIntegerArray(AM_ATT.ITMTBL);
                foreach (int iNo in olTempRtn)
                {
                    olRtn.Add(iNo);
                }
            }
            catch
            {
            }
            return olRtn;
        }


        /// <summary>
        /// TUBE 길이.
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double ITLE(DbElement rPart)
        {
            double dRtn = 0.0;

            try
            {
                dRtn = rPart.GetDouble(AM_ATT.ITLE);
            }
            catch
            {
            }
            return dRtn;
        }

        /// <summary>
        /// 마지막 수정날짜. LASTMOD
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string LASTM(DbElement rPart)
        {
            string sRtn = string.Empty;
            try
            {
                sRtn = rPart.GetString(AM_ATT.LASTM);
            }
            catch
            {

            }

            return sRtn;
        }

        /// <summary>
        /// Get Leave Ppoint
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static int LEAV(DbElement rPart)
        {
            return rPart.GetInteger(AM_ATT.LEAV);
        }

        /// <summary>
        /// Length
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double LENG(DbElement rPart)
        {
            double dRtn = 0.0;

            try
            {
                dRtn = rPart.GetDouble(AM_ATT.LENG);
            }
            catch
            {
            }
            return dRtn;
        }

        /// <summary>
        /// Leave excess for leave tube (AM12.SP4)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double LEXCES(DbElement rPart)
        {
            double dRtn = 0.0;

            try
            {
                dRtn = rPart.GetDouble(AM_ATT.LEXTNS);//LEXCES
            }
            catch
            {
            }
            return dRtn;
        }

        /// <summary>
        /// True if element is locked
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static bool LOCK(DbElement rPart)
        {
            return rPart.GetBool(DbAttributeInstance.LOCK);
        }


        /// <summary>
        /// LOOSE
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static bool LOOSE(DbElement rPart)
        {
            bool bRtn = false;

            try
            {
                bRtn = rPart.GetBool(AM_ATT.LOOS);
            }
            catch
            {
            }
            return bRtn;
        }


        /// <summary>
        /// Enclosing Box Volume
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static LimitsBox LVOL(DbElement rPart)
        {
            double[] olData = rPart.GetDoubleArray(AM_ATT.LVOL);
            Position oOtherMinP3 = Position.Create(olData[0], olData[1], olData[2]);
            Position oOtherMaxP3 = Position.Create(olData[3], olData[4], olData[5]);
            return LimitsBox.Create(oOtherMinP3, oOtherMaxP3);
        }

        /// <summary>
        /// Enclosing Box Volume
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static LimitsBox WVOLS(DbElement rPart)
        {
            double[] olData = rPart.GetDoubleArray(AM_ATT.WVOL);
            Position oOtherMinP3 = Position.Create(olData[0], olData[1], olData[2]);
            Position oOtherMaxP3 = Position.Create(olData[3], olData[4], olData[5]);
            return LimitsBox.Create(oOtherMinP3, oOtherMaxP3);
        }
        /// <summary>
        /// WVOL에서 일정길이 확대
        /// </summary>
        /// <param name="rPart"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static LimitsBox WVOLSadd(DbElement rPart,double length)
        {
            double[] olData = rPart.GetDoubleArray(AM_ATT.WVOL);
            Position oOtherMinP3 = Position.Create(olData[0]- length, olData[1]- length, olData[2]- length);
            Position oOtherMaxP3 = Position.Create(olData[3]+ length, olData[4]+ length, olData[5]+ length);
            return LimitsBox.Create(oOtherMinP3, oOtherMaxP3);
        }

        /// <summary>
        /// Insulation Spec Reference
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement ISPE(DbElement rPart)
        {
            return rPart.GetElement(AM_ATT.ISPE);
        }

        /// <summary>
        /// Material Reference
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement MATR(DbElement rPart)
        {
            //////////////////////////////////////////////////////////////////////////
            /// MATR 이 없을 경우 에러남.

            DbElement rRtn = DbElement.GetElement();
            try
            {
                rRtn = rPart.GetElement(AM_ATT.MATR);
            }
            catch
            {
            }
            return rRtn;
        }

        /// <summary>
        ///  Material list control for components (ON,OFF,DOTD,DOTU)
        ///  ON  :Include on the material list and draw normally.
        ///  OFF :Exclude from the material list but draw normally.
        ///  DOTD:Exclude from the material list and draw dotted and dimensioned.
        ///  DOTU:Exclude from the material list and draw dotted but undimensioned
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string MTOC(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetAsString(AM_ATT.MTOC);
            }
            catch
            {
            }
            return sRtn;
        }

        //dmkim 190503
        public static string MTOT(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetAsString(AM_ATT.MTOT);
            }
            catch
            {
            }
            return sRtn;
        }

        public static string HCOFG(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetAsString(AM_ATT.HCOFG);
            }
            catch
            {
            }
            return sRtn;
        }

        public static string QUATXT(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetAsString(AM_ATT.QUATXT);
            }
            catch
            {
            }
            return sRtn;
        }
        //dmkim 190503
        public static string MTOH(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetAsString(AM_ATT.MTOH);
            }
            catch
            {
            }
            return sRtn;
        }

        //dmkim 190618
        /// <summary>
        /// MTOREF
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static List<DbElement> MTOR(DbElement rPart)
        {
            List<DbElement> olrRtn = new List<DbElement> { };

            DbElement[] olrPart = rPart.GetElementArray(AM_ATT.MTOR);

            foreach (DbElement rTmpPart in olrPart)
            {
                olrRtn.Add(rTmpPart);
            }

            return olrRtn;
        }


        /// <summary>
        /// XTEXT of material text
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string MTXX(DbElement rPart)
        {
            //////////////////////////////////////////////////////////////////////////
            /// MATR 이 없을 경우 에러남.

            string sRtn = string.Empty;
            try
            {
                sRtn = rPart.GetString(AM_ATT.MTXX);
            }
            catch
            {
            }
            return sRtn;
        }

        /// <summary>
        /// Number Off (BTSE)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static int NOFF(DbElement rPart)
        {
            return rPart.GetInteger(AM_ATT.NOFF);
        }

        /// <summary>
        /// Non-Standard Bolt Length
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement NSTD(DbElement rPart)
        {
            //////////////////////////////////////////////////////////////////////////
            /// MATR 이 없을 경우 에러남.

            DbElement rRtn = DbElement.GetElement();
            try
            {
                rRtn = rPart.GetElement(AM_ATT.NSTD);
            }
            catch
            {
            }
            return rRtn;
        }


        /// <summary>
        /// Number
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static int NUMB(DbElement rPart)
        {
            int iRtn = -1;
            try
            {
                iRtn = rPart.GetInteger(AM_ATT.NUMB);
            }
            catch { }

            return iRtn;
        }

        /// <summary>
        /// Nett weight
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double NWEI(DbElement rPart)
        {
            double dRtn = 0.0;
            try
            {
                dRtn = rPart.GetDouble(AM_ATT.NWEI);
            }
            catch { }

            return dRtn;
        }

        /// <summary>
        /// PART의 World Orientation 을 가져옴.
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static Aveva.Pdms.Geometry.Orientation ORI(DbElement rPart)
        {
            return rPart.GetOrientation(AM_ATT.ORI, 1, MDB.CurrentMDB.GetFirstWorld(DbType.Design));
        }

        public static double[] PARA(DbElement rPart)
        {
            double[] olRtn = null;

            try
            {
                olRtn = rPart.GetDoubleArray(AM_ATT.PARA);
            }
            catch
            {

            }
            return olRtn;

        }
        public static double PARA(DbElement rPart, int iIndex)
        {
            double dRtn = 0;
            try
            {
                double[] olPara = rPart.GetDoubleArray(AM_ATT.PARA);
                dRtn = olPara[iIndex];
            }
            catch
            {

            }
            return dRtn;
        }
        /// <summary>
        /// Part Name
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string PARNAM(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetAsString(AM_ATT.PARNAM);
            }
            catch
            {
            }
            return sRtn;
        }

        /// <summary>
        /// Part의 PSPOOL Element
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement PCRFA(DbElement rPart)
        {
            DbElement rRtn = DbElement.GetElement();
            try
            {
                DbElement[] olrRtn = rPart.GetElementArray(AM_ATT.PCRFA);
                if (olrRtn.Length > 0)
                {
                    rRtn = olrRtn[0];
                }
            }
            catch
            {
            }
            return rRtn;
        }

        public static string PICF(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetAsString(AM_ATT.PICF);
            }
            catch
            {
            }
            return sRtn;
        }


        /// <summary>
        /// Pipe specification
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement PSPE(DbElement rPart)
        {
            return rPart.GetElement(AM_ATT.PSPE);
        }

        //dmkim 200101
        /// <summary>
        /// Point Reference
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement PTRF(DbElement rPart)
        {
            return rPart.GetElement(AM_ATT.PTRF);
        }

        public static string PURP(DbElement rPart)
        {
            return rPart.GetString(AM_ATT.PURP);
        }


        /// <summary>
        /// PPOINT 갯수
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static int PPCOU(DbElement rPart)
        {
            return rPart.GetInteger(AM_ATT.PPCOU);
        }

        /// <summary>
        /// PPOINT INDEX LIST
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static List<int> PPVI(DbElement rPart)
        {
            return rPart.GetIntegerArray(AM_ATT.PPVI).ToList();
        }

        /// <summary>
        /// Primary Owning Element
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement PRMOWN(DbElement rPart)
        {
            return rPart.GetElement(AM_ATT.PRMOWN);
        }

        /// <summary>
        /// Long Part ID
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string PRTIDL(DbElement rPart)
        {
            return rPart.GetString(AM_ATT.PRTIDL);
        }

        /// <summary>
        /// Ppoint wall thickness
        /// </summary>
        /// <param name="rPart"></param>
        /// <param name="iIndex"></param>
        /// <returns></returns>
        public static double PWALLT(DbElement rPart, int iIndex)
        {
            double dRtn = 0.0;
            try
            {
                double[] olPara = rPart.GetDoubleArray(AM_ATT.PWALLT);
                dRtn = olPara[iIndex];
            }
            catch
            {

            }
            return dRtn;
        }

        /// <summary>
        /// Type of question
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string QTYPE(DbElement rPart)
        {
            return rPart.GetString(AM_ATT.QTYPE);
        }

        /// <summary>
        /// Question
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string QUES(DbElement rPart)
        {
            return rPart.GetString(AM_ATT.QUES);
        }

        /// <summary>
        /// RADIUS (BEND/ELBO)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double RADI(DbElement rPart)
        {
            double dRtn = 0.0;
            try
            {
                dRtn = rPart.GetDouble(AM_ATT.RADI);
            }
            catch
            {
            }
            return dRtn;
        }

        public static string REFNO(DbElement rPart)
        {
            try
            {
                return rPart.GetAsString(AM_ATT.REF);
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// SBOLT REFERENCEs
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static List<DbElement> SBRA(DbElement rPart)
        {
            List<DbElement> olrRtn = new List<DbElement> { };

            DbElement[] olrPart = rPart.GetElementArray(AM_ATT.SBRA);

            foreach (DbElement rTmpPart in olrPart)
            {
                olrRtn.Add(rTmpPart);
            }

            return olrRtn;
        }

        /// <summary>
        /// 도면 SHEE, BACK등 사이즈 가져오기
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double[] SIZE(DbElement rPart)
        {
            return rPart.GetDoubleArray(AM_ATT.SIZE);
        }


        /// <summary>
        /// ISO DB에 속한 SPOOl Reference
        /// Return reference from piping component or leave tube to spool or field element
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement SFREF(DbElement rPart)
        {
            //////////////////////////////////////////////////////////////////////////
            /// GASK 같이 SFREF없을 경우 에러남...

            DbElement rRtn = DbElement.GetElement();
            try
            {
                rRtn = rPart.GetElement(AM_ATT.SFREF);
            }
            catch
            {
            }
            return rRtn;
        }

        /// <summary>
        /// SHOP True/False
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static bool SHOP(DbElement rPart)
        {
            bool bRtn = false;
            try
            {
                bRtn = rPart.GetBool(AM_ATT.SHOP);
            }
            catch
            {

            }
            return bRtn;
        }

        /// <summary>
        /// ATTA의 SPKBRK
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static bool SPKBRK(DbElement rPart)
        {
            bool bRtn = false;
            try
            {
                bRtn = rPart.GetBool(AM_ATT.SPKBRK);
            }
            catch
            {

            }
            return bRtn;
        }

        public static string SPLH(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetString(AM_ATT.SPLH);
            }
            catch
            {
            }
            return sRtn;
        }
        public static string SPLN(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetString(AM_ATT.SPLN);
            }
            catch
            {
            }
            return sRtn;
        }
        public static string SPLT(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetString(AM_ATT.SPLT);
            }
            catch
            {
            }
            return sRtn;
        }

        /// <summary>
        /// Table linking SPOOL + FIELD elements to piping components
        /// </summary>
        /// <param name="rPart">Spool/Field Element</param>
        /// <returns></returns>
        public static List<int> SPLTBL(DbElement rPart)
        {
            List<int> olRtn = new List<int> { };

            try
            {
                int[] olTempRtn = rPart.GetIntegerArray(AM_ATT.SPLTBL);
                foreach (int iNo in olTempRtn)
                {
                    olRtn.Add(iNo);
                }
            }
            catch
            {
            }
            return olRtn;
        }

        /// <summary>
        /// Spool Number
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static int SPLNUM(DbElement rPart)
        {
            return rPart.GetInteger(AM_ATT.SPLNUM);
        }

        /// <summary>
        /// Spoo lNumber Prefix
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string SPLP(DbElement rPart)
        {
            return rPart.GetString(AM_ATT.SPLP);
        }

        /// <summary>
        /// Component spec reference
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement SPREF(DbElement rPart)
        {
            DbElement rRtn = DbElement.GetElement();

            try
            {
                rRtn = rPart.GetElement(AM_ATT.SPRE);
            }
            catch
            {
            }
            return rRtn;
        }

        public static string SKEY(DbElement rPart)
        {
            string sRtn = string.Empty;
            try
            {
                sRtn = rPart.GetString(AM_ATT.SKEY);
            }
            catch
            { }
            return sRtn;
        }

        //dmkim 161102 Length없는 Tube 제거, invalid DbElement 제거
        /// <summary>
        /// SPOOL 일 경우만.
        /// 포함하는 아이템 가져오기.
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static List<DbElement> SPLMEM(DbElement rPart)
        {
            List<DbElement> olrRtn = new List<DbElement> { };

            DbElement[] olrPart = rPart.GetElementArray(AM_ATT.SPLMEM);

            foreach (DbElement rTmpPart in olrPart)
            {
                if (A.IsValidElement(rTmpPart) == false) { continue; }
                if (rTmpPart.GetElementType().Equals(DbElementTypeInstance.TUBING))
                {
                    try
                    {
                        Point3D oApos = A.Element.GetPos(rTmpPart, DbAttributeInstance.APOS);
                        Point3D oLpos = A.Element.GetPos(rTmpPart, DbAttributeInstance.LPOS);
                        if (oApos.DistanceToPoint(oLpos) < 1) { continue; }
                    }
                    catch { continue; }
                }
                olrRtn.Add(rTmpPart);
            }

            return olrRtn;
        }

        /// <summary>
        /// Get SText
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string STEXT(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetAsString(AM_ATT.STEX);
            }
            catch
            {
            }
            return sRtn;
        }

        /// <summary>
        /// STYPE (SPECON type question)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string STYP(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                sRtn = rPart.GetAsString(AM_ATT.STYP);
            }
            catch
            {
            }
            return sRtn;
        }


        /// <summary>
        ///  Text answer for text styp in spec
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string TANS(DbElement rPart)
        {
            return rPart.GetString(AM_ATT.TANS);
        }

        /// <summary>
        /// Text default for text styp in spec
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string TDEF(DbElement rPart)
        {
            return rPart.GetString(AM_ATT.TDEF);
        }


        /// <summary>
        /// Task Parameter Name
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string TPVAL(DbElement rPart)
        {
            return rPart.GetString(AM_ATT.TPVAL);
        }


        /// <summary>
        /// BRANCH의 Tail Reference
        /// Null Element Return 시 값 체크 주의할 것.
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement TREF(DbElement rPart)
        {
            return rPart.GetElement(AM_ATT.TREF);
        }

        /// <summary>
        /// Tracing Spec Reference
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement TSPE(DbElement rPart)
        {
            return rPart.GetElement(AM_ATT.TSPE);
        }


        public static string TYPE(DbElement rPart)
        {
            return rPart.GetString(AM_ATT.TYPE);
        }

        /// <summary>
        /// Unit pipe weight
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double UWEI(DbElement rPart)
        {
            double dRtn = 0.0;

            try
            {
                //dmkim 190810
                string sValue = rPart.GetString(AM_ATT.UWEI);

                DbDouble oValue = DbDouble.Create(sValue.Replace("(", "").Replace(")", ""));
                DbDoubleUnits oUnit = DbDoubleUnits.GetUnits("KG");
                oValue = oValue.ConvertUnits(oUnit);

                dRtn = oValue.Value;
            }
            catch
            {
            }
            return dRtn;
        }

        /// <summary>
        /// WORLD Position
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double[] WORPOS(DbElement rPart)
        {
            return rPart.GetDoubleArray(AM_ATT.WORPOS); ;
        }

        /// <summary>
        /// VOLUMN
        /// MIN X, Y, Z,  MAX X ,Y ,Z
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double[] WVOL(DbElement rPart)
        {
            return rPart.GetDoubleArray(AM_ATT.WVOL); ;
        }

        /// <summary>
        /// Extra length requirement for BOLTS
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static double XTRA(DbElement rPart)
        {
            double dRtn = 0.0;
            try
            {
                dRtn = rPart.GetDouble(AM_ATT.XTRA);
            }
            catch
            {
            }
            return dRtn;
        }

        public static double[] XYPS(DbElement rPart)
        {
            return rPart.GetDoubleArray(AM_ATT.XYPS);
        }

        /// <summary>
        /// User Name of user claiming Element
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string USERC(DbElement rPart)
        {
            return rPart.GetString(AM_ATT.USERC);
        }


        /// <summary>
        /// Get Parent List
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string[] AHLIST(DbElement rPart)
        {
            return rPart.GetAsStringArray(DbAttributeInstance.AHLIST);
        }
    }

    /// <summary>
    /// AM User Attribute
    /// </summary>
    public static class AvevaUDA
    {
        public static class Valid
        {
            public static string mldbMDwgNO(DbElement rPart)
            {
                string sUDA = ":mldbMDwgNO";
                if (DbAttribute.GetDbAttribute(sUDA) == null) return string.Empty;
                return rPart.IsAttributeValid(DbAttribute.GetDbAttribute(sUDA)) ? ATT_UDA.mldbMDwgNO(rPart) : string.Empty; //복사시 ATT.UDA 함수확인
            }
            public static string valid(DbElement rPart)
            {
                string sUDA = ":valid";
                if (DbAttribute.GetDbAttribute(sUDA) == null) return string.Empty;
                return rPart.IsAttributeValid(DbAttribute.GetDbAttribute(sUDA)) ? ATT_UDA.valid(rPart) : string.Empty; //복사시 ATT.UDA 함수확인
            }
        }

        public static string mldbMDwgNO(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":mldbMDwgNO");
                sRtn = rPart.GetString(uda);
            }
            catch
            {
            }
            return sRtn;
        }


        /// <summary>
        /// 자재코드
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string ERP_CODE(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_ERP_CODE");
                sRtn = rPart.GetString(uda);
            }
            catch
            {
            }
            return sRtn;
        }

        //dmkim 190221
        /// <summary>
        /// 자재코드 (별로 ENG회사것을 사용하기위해)
        /// </summary>
        /// <param name="rPart">SPREF</param>
        /// <returns></returns>
        public static string ENI_CODE(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":ENI_CODE");
                sRtn = rPart.GetString(uda);
            }
            catch
            {
            }
            return sRtn;
        }


        /// <summary>
        /// BRACKET 연결모델 정보 .(임시로 사용.)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string NOTE(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":NOTE");
                sRtn = rPart.GetString(uda);
            }
            catch
            {
            }
            return sRtn;
        }


        /// <summary>
        /// Get FLAN_ANGLE
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string SHI_FLAN_ANGLE(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_FLAN_ANGLE");
                sRtn = rPart.GetString(uda);

                if (sRtn == ATT.UNSET) { sRtn = string.Empty; }
            }
            catch
            {
            }
            return sRtn;
        }

        //dmkim 190215
        /// <summary>
        /// Get FLAN_JOINT_NO (ex, "0","1",...)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string SHI_FLAN_JOINT_NO(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_FLAN_JOINT_NO");
                sRtn = rPart.GetString(uda);

                if (sRtn == ATT.UNSET) { sRtn = string.Empty; }
            }
            catch
            {
            }
            return sRtn;
        }

        //dmkim 180329
        /// <summary>
        /// SPCO의 SHI_ERP_DESC
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string SHI_ERP_DESC(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_ERP_DESC");
                sRtn = rPart.GetString(uda);

                if (sRtn == ATT.UNSET) { sRtn = string.Empty; }
            }
            catch
            {
            }
            return sRtn;
        }

        /// <summary>
        /// Get S_WN => SHI_WN
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string SHI_WN(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_WN");
                sRtn = rPart.GetString(uda);
            }
            catch
            {
            }
            return sRtn;
        }

        /// <summary>
        /// WELD 속성. (PE:PLAN END, BE:BEVEL END, DE:DOUBLE BEVEL END) Trim적용.
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string SHI_PIPE_END(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_PIPE_END");
                sRtn = rPart.GetString(uda).Trim();

                ////dmkim 170412 TEST
                //if (sRtn.Length == 0 && G.bDevMode)
                //{
                //    uda = DbAttribute.GetDbAttribute(":S_PIPE_END");
                //    sRtn = rPart.GetString(uda).Trim();
                //}
            }
            catch
            {
            }
            return sRtn;
        }


        /// <summary>
        /// 자재코드
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string SHI_CODE(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_CODE");
                sRtn = rPart.GetString(uda);
            }
            catch
            {
            }
            return sRtn;
        }

        /// <summary>
        /// ATTA<->STRU (ShipSideBracket 용도로 사용.)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static List<DbElement> SHI_CONNECTEDREF(DbElement rPart)
        {
            List<DbElement> olrRtn = new List<DbElement> { };

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_CONNECTEDREF");
                DbElement[] olrPart = rPart.GetElementArray(uda);

                foreach (DbElement rTmpPart in olrPart)
                {
                    if (A.IsValidElement(rTmpPart))
                    {
                        olrRtn.Add(rTmpPart);
                    }
                }
            }
            catch
            {

            }

            return olrRtn;
        }

        /// <summary>
        /// STAGE 정보.
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string SHI_STAGE(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_STAGE");
                sRtn = rPart.GetString(uda);
            }
            catch
            {
            }
            return sRtn;
        }

        //dmkim 190410
        /// <summary>
        /// Note 정보.
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string SHI_STEXT(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_STEXT");
                sRtn = rPart.GetString(uda);
            }
            catch
            {
            }
            return sRtn;
        }

        /// <summary>
        /// UNIT 정보.
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string SHI_UNIT_NO(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_UNIT_NO");
                sRtn = rPart.GetString(uda);
            }
            catch
            {
            }
            return sRtn;
        }

        /// <summary>
        /// ADJUST PC’S
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string SHI_PRE_ADJ(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_PRE_ADJ");
                sRtn = rPart.GetString(uda);
            }
            catch
            {
            }
            return sRtn;
        }

        /// <summary>
        /// ADJ. PC’S PRE-PAINTED
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string SHI_PRE_ADJ_PNT(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_PRE_ADJ_PNT");
                sRtn = rPart.GetString(uda);
            }
            catch
            {
            }
            return sRtn;
        }

        /// <summary>
        /// REGI
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string SHI_DSN_DP(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_DSN_DP");
                sRtn = rPart.GetString(uda);
            }
            catch
            {
            }
            return sRtn;
        }

        //dmkim 191002
        /// <summary>
        /// :SHI_DWG_REV00 ~ :SHI_DWG_REV10
        /// </summary>
        /// <param name="rPart"></param>
        /// <param name="sIndex"></param>
        /// <returns></returns>
        public static string SHI_DWG_REV(DbElement rPart, string sIndex)
        {
            string sRtn = string.Empty;

            try
            {
                string sUda = ":SHI_DWG_REV" + sIndex;
                DbAttribute uda = DbAttribute.GetDbAttribute(sUda);
                sRtn = rPart.GetString(uda);
            }
            catch
            {
            }
            return sRtn;
        }

        //dmkim 161031 Valve Tag
        /// <summary>
        /// Get Valve Tag (VALV, INST, PCOM)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string SHI_VALV_TAG(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_VALV_TAG");
                sRtn = rPart.GetString(uda);
            }
            catch
            {
            }
            return sRtn;
        }

        //dmkim 161101 SUPPORT의 :SHI_SUPP_NO
        /// <summary>
        /// SUPPORT(ATTA)의 :SHI_SUPP_NO
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string SHI_SUPP_NO(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_SUPP_NO");
                sRtn = rPart.GetString(uda);
            }
            catch
            {
            }
            return sRtn;
        }

        //dmkim 180718 사용.
        //dmkim 180710 사용안함.
        //dmkim 161101 ATTA NOTE
        /// <summary>
        /// ATTA의 :SHI_NOTE
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string SHI_NOTE(DbElement rPart)
        {
            string sRtn = string.Empty;

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_NOTE");
                sRtn = rPart.GetString(uda);
            }
            catch
            {
            }
            return sRtn;
        }


        /// <summary>
        /// ATTA에 연결된 STRU (MultiPiece 용도로 사용.)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static List<DbElement> SHI_O_PENESTRUREF(DbElement rPart)
        {
            List<DbElement> olrRtn = new List<DbElement> { };

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_O_PENESTRUREF");
                DbElement[] olrPart = rPart.GetElementArray(uda);

                foreach (DbElement rTmpPart in olrPart)
                {
                    if (A.IsValidElement(rTmpPart))
                    {
                        olrRtn.Add(rTmpPart);
                    }
                }
            }
            catch
            {

            }

            return olrRtn;
        }

        /// <summary>
        /// STRU 연결된 ATTA (MultiPiece 용도로 사용.)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static List<DbElement> SHI_O_PENEATTAREF(DbElement rPart)
        {
            List<DbElement> olrRtn = new List<DbElement> { };

            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":SHI_O_PENEATTAREF");
                DbElement[] olrPart = rPart.GetElementArray(uda);

                foreach (DbElement rTmpPart in olrPart)
                {
                    if (A.IsValidElement(rTmpPart))
                    {
                        olrRtn.Add(rTmpPart);
                    }
                }
            }
            catch
            {

            }

            return olrRtn;
        }

        /// <summary>
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string valid(DbElement rPart)
        {
            string sRtn = string.Empty;
            try
            {
                DbAttribute uda = DbAttribute.GetDbAttribute(":valid");
                sRtn = rPart.GetString(uda);
            }
            catch
            {
            }
            return sRtn;
        }
    }

    /// <summary>
    /// AM Attribute 조합.
    /// </summary>
    public static class AvaveMixAtt
    {
        /// <summary>
        /// Bolt reference array 중에서 첫번째.
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement BLRF_CATR_SPRE(DbElement rPart)
        {
            DbElement rElement = DbElement.GetElement();
            try
            {
                DbElement rSpref = rPart.GetElement(AM_ATT.SPRE);
                DbElement rCatref = rSpref.GetElement(AM_ATT.CATR);
                DbElement[] olrBlrf = rCatref.GetElementArray(AM_ATT.BLRF);
                if (olrBlrf.Length > 0)
                {
                    rElement = olrBlrf[0];
                }
            }
            catch
            {

            }
            return rElement;
        }

        public static DbElement CATR_SPRE(DbElement rPart)
        {
            DbElement rElement = DbElement.GetElement();
            try
            {
                DbElement rSpref = rPart.GetElement(AM_ATT.SPRE);
                rElement = rSpref.GetElement(AM_ATT.CATR);
            }
            catch
            {

            }
            return rElement;
        }

        //dmkim 170302
        /// <summary>
        /// Point Set Reference (PTREF of CATREF of SPREF)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement PTREF_CATR_SPRE(DbElement rPart)
        {
            DbElement rElement = DbElement.GetElement();
            try
            {
                DbElement rCatref = ATT_MIX.CATR_SPRE(rPart);
                if (A.IsValidElement(rCatref))
                {
                    rElement = rCatref.GetElement(AM_ATT.PTRE);
                }
            }
            catch
            {

            }
            return rElement;
        }

        //dmkim 170302
        /// <summary>
        /// Geometry Set Reference (GMREF of CATREF of SPREF)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static DbElement GMRE_CATR_SPRE(DbElement rPart)
        {
            DbElement rElement = DbElement.GetElement();
            try
            {
                DbElement rCatref = ATT_MIX.CATR_SPRE(rPart);
                if (A.IsValidElement(rCatref))
                {
                    rElement = rCatref.GetElement(AM_ATT.GMRE);
                }
            }
            catch
            {

            }
            return rElement;
        }

        public static string DESCOfCATREF(DbElement rPart)
        {
            string sRtn = string.Empty;
            try
            {
                DbElement rCatref = rPart.GetElement(DbAttributeInstance.CATR);
                sRtn = rCatref.GetString(DbAttributeInstance.DESC);
            }
            catch
            {
            }
            return sRtn;
        }

        public static double DESP(DbElement rPart, int iIndex)
        {
            double dRtn = 0;
            try
            {
                double[] olDesp = rPart.GetDoubleArray(DbAttributeInstance.DESP);
                dRtn = olDesp[iIndex];
            }
            catch
            {
            }

            return dRtn;
        }

        public static string MATL(DbElement rPart)
        {
            string sRtn = "";
            try
            {
                sRtn = rPart.GetElement(DbAttributeInstance.SPRE).GetElement(DbAttributeInstance.MATX).GetString(DbAttributeInstance.XTEX);
            }
            catch
            {
            }
            return sRtn;
        }

        public static double[] PARA(DbElement rPart)
        {
            double[] olRtn = null;

            try
            {
                olRtn = rPart.GetDoubleArray(DbAttributeInstance.PARA);
            }
            catch
            {
            }
            return olRtn;

        }
        public static double PARA(DbElement rPart, int iIndex)
        {
            double dRtn = 0;
            try
            {
                double[] olPara = rPart.GetDoubleArray(DbAttributeInstance.PARA);
                dRtn = olPara[iIndex];
            }
            catch
            {
            }
            return dRtn;
        }


        public static double PARA_CATR_SPRE(DbElement rPart, int iIndex)
        {
            double dRtn = 0;
            try
            {
                DbElement rSpref = rPart.GetElement(AM_ATT.SPRE);
                DbElement rCatref = rSpref.GetElement(AM_ATT.CATR);

                double[] olPara = rCatref.GetDoubleArray(AM_ATT.PARA);
                dRtn = olPara[iIndex];
            }
            catch
            {

            }
            return dRtn;
        }

        public static string PARA_CATR_SPRE(DbElement rPart)
        {
            string sRtn = null;
            try
            {
                DbElement rSpref = rPart.GetElement(AM_ATT.SPRE);
                DbElement rCatref = rSpref.GetElement(AM_ATT.CATR);

                sRtn = rCatref.GetAsString(AM_ATT.PARA);
            }
            catch
            {

            }
            return sRtn;
        }


        /// <summary>
        /// RTEXT (자재정보)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string RTEXT(DbElement rPart)
        {
            string sRtn = "";
            try
            {
                sRtn = rPart.GetElement(AM_ATT.SPRE).GetElement(AM_ATT.DETR).GetString(AM_ATT.RTEX);
            }
            catch
            {

            }
            return sRtn;
        }

        /// <summary>
        /// SPEC RTEXT (자재정보)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string RTEXT_DETR(DbElement rPart)
        {
            string sRtn = "";
            try
            {
                //dmkim 190811
                string sDefault = rPart.GetElement(AM_ATT.DETR).GetString(AM_ATT.RTEX); ;
                DbElement rDetr = rPart.GetElement(AM_ATT.DETR);
                sRtn = A.Element.GetStringProperty(rPart, rDetr, AM_ATT.RTEX, sDefault);
            }
            catch
            {

            }
            return sRtn;
        }

        /// <summary>
        /// LSTUBE의 RTEXT (BEND일경우..)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string RTEXT_LSTUBE(DbElement rPart)
        {
            string sRtn = "";
            try
            {
                sRtn = rPart.GetElement(AM_ATT.LSTU).GetElement(AM_ATT.DETR).GetString(AM_ATT.RTEX);
            }
            catch
            {

            }
            return sRtn;
        }
        // <summary>
        /// LSTUBE의 STEXT (BEND일경우..)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string STEXT_LSTUBE(DbElement rPart)
        {
            string sRtn = "";
            try
            {
                sRtn = rPart.GetElement(AM_ATT.LSTU).GetElement(AM_ATT.DETR).GetString(AM_ATT.STEX);
            }
            catch
            {

            }
            return sRtn;
        }
        // <summary>
        /// LSTUBE의 TTEXT (BEND일경우..)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string TTEXT_LSTUBE(DbElement rPart)
        {
            string sRtn = "";
            try
            {
                sRtn = rPart.GetElement(AM_ATT.LSTU).GetElement(AM_ATT.DETR).GetString(AM_ATT.TTEX);
            }
            catch
            {

            }
            return sRtn;
        }

        /// <summary>
        /// XTEXT (재질정보)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string XTEXT_MATX(DbElement rPart)
        {
            string sRtn = "";
            try
            {
                sRtn = rPart.GetElement(AM_ATT.MATX).GetString(AM_ATT.XTEX);
            }
            catch
            {

            }
            return sRtn;
        }

        /// <summary>
        /// XTEXT (재질정보)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string XTEXT_MATX_SPRE(DbElement rPart)
        {
            string sRtn = "";
            try
            {
                sRtn = rPart.GetElement(AM_ATT.SPRE).GetElement(AM_ATT.MATX).GetString(AM_ATT.XTEX);
            }
            catch
            {

            }
            return sRtn;
        }



        /// <summary>
        /// LSTUBE의 XTEXT (재질정보) (BEND일경우..)
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static string XTEXT_LSTUBE(DbElement rPart)
        {
            string sRtn = "";
            try
            {
                sRtn = rPart.GetElement(AM_ATT.LSTU).GetElement(AM_ATT.MATX).GetString(AM_ATT.XTEX);
            }
            catch
            {

            }
            return sRtn;
        }
    }
}
