using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Collections;

using Aveva.Pdms.Database;
using Aveva.Pdms.Shared;
using Aveva.PDMS.Database.Filters;
using Aveva.Pdms.Geometry;
using Aveva.ApplicationFramework.Presentation;
using Aveva.Pdms.Shared.Search;
using Aveva.Pdms.Graphics;

using INFOGET_ZERO_HULL.Geometry;
using INFOGET_ZERO_HULL;

using A = INFOGET_ZERO_HULL.AM.AvevaUtil;
using ATT = INFOGET_ZERO_HULL.AM.AvevaAtt;
using ATT_UDA = INFOGET_ZERO_HULL.AM.AvevaUDA;
using ATT_MIX = INFOGET_ZERO_HULL.AM.AvaveMixAtt;
using D = INFOGET_ZERO_HULL.Default.DefaultDefine;
using E = INFOGET_ZERO_HULL.Default.DefaultEnum;

namespace INFOGET_ZERO_HULL.AM
{
    public static class AvevaUtil
    {
        //너무 자주 사용되어서 

        /// <summary>
        /// Element의 유효성 체크.
        /// rPart != null, rPart.IsNull==false,  rPart.IsValid==true
        /// </summary>
        /// <param name="rPart"></param>
        /// <returns></returns>
        public static bool IsValidElement(DbElement rPart)
        {
            return (rPart != null && !rPart.IsNull && rPart.IsValid);
        }
        /// <summary>
        /// Element의 유효성 체크.
        /// </summary>
        /// <param name="sPart"></param>
        /// <param name="bAddSlash">'/'를 앞에 추가</param>
        /// <returns></returns>
        public static bool IsValidElement(string sPart, bool bAddSlash = false)
        {
            if (bAddSlash) { sPart = "/" + sPart; }
            DbElement rPart = DbElement.GetElement(sPart);
            return A.IsValidElement(rPart);
        }

        /// <summary>
        /// COLLECT ALL "oElementType" for "rElement" (모든하위 다 찾음)
        /// </summary>
        /// <param name="rElement"></param>
        /// <param name="oElementType"></param>
        /// <param name="AllType">True이면 모든 하위 Elements</param>
        /// <returns></returns>
        public static List<DbElement> GetCollect(DbElement rElement, DbElementType oElementType, bool AllType = false)
        {
            List<DbElement> olRtn = new List<DbElement> { };

            TypeFilter oFilter = new TypeFilter(oElementType);

            DBElementCollection collection = new DBElementCollection(rElement, oFilter);

            if (AllType) { collection = new DBElementCollection(rElement); }
            foreach (DbElement rMember in collection)
            {
                olRtn.Add(rMember);
            }
            return olRtn;
        }
        /// <summary>
        /// COLLECT ALL "정해진 타입만"
        /// </summary>
        /// <param name="rElement"></param>
        /// <param name="olElementType"></param>
        /// <returns></returns>
        public static List<DbElement> GetCollect(DbElement rElement, List<DbElementType> olElementType)
        {
            List<DbElement> olRtn = new List<DbElement> { };

            TypeFilter oFilter = new TypeFilter(olElementType.ToArray());

            DBElementCollection collection = new DBElementCollection(rElement, oFilter);


            foreach (DbElement rMember in collection)
            {
                olRtn.Add(rMember);
            }
            return olRtn;
        }


        public static List<DbElement> GetWithInCollect(List<DbElementType> olElementType, LimitsBox oBox, bool bCompletelyWithin = false  )
        {
            List<DbElement> olRtn = new List<DbElement> { };

            string sElementType = string.Empty;
            if (olElementType.Count > 0)
            {
                sElementType = "(";

                foreach (DbElementType oElementType in olElementType)
                {
                    sElementType += " " + oElementType.ToString(); 
                }
                sElementType += ")";
            }

            string sWithIn = bCompletelyWithin ? "EXCLUSIVE" : "";


            string sCmd = String.Format("VAR !olCol COLL ALL {0} {1} WITHIN E{2} N{3} U{4} to E{5} N{6} U{7}",
                sElementType, sWithIn, oBox.Minimum.X, oBox.Minimum.Y, oBox.Minimum.Z, oBox.Maximum.X, oBox.Maximum.Y, oBox.Maximum.Z);
            A.Command.CommandForPDMS(sCmd);
            //A.Command.PrintInCommandWindow(sCmd);

            //A.Command.PrintInCommandWindow("START");
            //205개 리스트 약 6초정도 걸림.
            A.Command.CommandForPDMS("!!ZEROHULLCOLLVOL.delete()");
            A.Command.CommandForPDMS("!!ZEROHULLCOLLSIZE.delete()");

            A.Command.CommandForPDMS("!!ZEROHULLCOLLSIZE = !olCol.Size()");
            double dSize = A.Command.GetPMLValueDouble("ZEROHULLCOLLSIZE");

            for (int i = 1; i <= dSize; i++)
            {
                A.Command.CommandForPDMS(string.Format("!!ZEROHULLCOLLVOL = !olCol[{0}]", i));
                
                string sName = A.Command.GetPMLValueString("ZEROHULLCOLLVOL");
                //A.Command.PrintInCommandWindow(sName);
                olRtn.Add(DbElement.GetElement(sName));
            }

            return olRtn;
        }

       

        /// <summary>
        /// AM 관련 정보.
        /// </summary>
        public static class Info
        {
            /// <summary>
            /// EX)"2204" rSystem.GetString(DbAttributeInstance.PRJN)
            /// </summary>
            public static string NUMBER = A.GetProject("NUMBER");
            /// <summary>
            /// EX)"CAF" Project.CurrentProject.Name
            /// </summary>
            public static string CODE = A.GetProject("CODE");
            /// <summary>
            /// EX)"Marine Sample Project Version 1213.v.1.2" rSystem.GetString(DbAttributeInstance.PRJD)
            /// </summary>
            public static string DESC = A.GetProject("DESC");
            /// <summary>
            /// EX) "12.1"
            /// </summary>
            public static string VERSION = A.GetVersion();
            /// <summary>
            /// EX) Outfitting
            /// </summary>
            public static E.MODULE MODULE = A.GetModule();
            /// <summary>
            /// EX)"2204" rSystem.GetString(DbAttributeInstance.PRJID)
            /// </summary>
            public static string ID = A.GetProject("ID");
            /// <summary>
            /// EX)"ALL_NO_MDS" MDB.CurrentMDB.Name;
            /// </summary>
            public static string MDB = A.GetCurrentMDB().Name;
            /// <summary>
            /// EX)"SYSTEM" Project.CurrentProject.UserName;
            /// </summary>
            public static string USER = A.GetLoginUesrName();

            /// <summary>
            /// Design DB World '/*' , Collect 시 Root로 가져올 시 사용.
            /// </summary>
            public static DbElement DESI_WORLD = A.GetCurrentMDB().GetFirstWorld(Aveva.Pdms.Database.DbType.Design);
        }
        #region Project Info
        /// <summary>
        /// Project "NUMBER","DESC","ID","CODE"
        /// </summary>
        /// <param name="sGetInfoType"></param>
        /// <returns></returns>
        private static string GetProject(string sGetInfoType)
        {
            string sRtn = string.Empty;

            Project oProj = Project.CurrentProject;
            Db oDB = oProj.SystemDB;
            DbElement rWorld = oDB.World;
            DbElement[] olMember = rWorld.Members();

            DbElement rSystem = DbElement.GetElement();
            foreach (DbElement rMember in olMember)
            {
                if (AvevaAtt.FLNM(rMember) == "/*S")
                {
                    rSystem = rMember;
                }

            }

            if (AvevaUtil.IsValidElement(rSystem))
            {
                if (sGetInfoType == "NUMBER")
                {
                    sRtn = rSystem.GetString(DbAttributeInstance.PRJN).Trim();
                }

                else if (sGetInfoType == "DESC") sRtn = rSystem.GetString(DbAttributeInstance.PRJD).Trim();

                // 20170421 YJS
                else if (sGetInfoType == "ID")
                {
                    sRtn = rSystem.GetString(DbAttributeInstance.PRJID).Trim();

                    ////dmkim 170829
                    ////sRtn = "-RN1011";
                    //if (D.SHIPYARD == E.SHIPYARD.SHI)
                    //{
                    //    sRtn = sRtn.Substring(0, 1) == "-" ? sRtn.Substring(1) : sRtn; // -RN1011
                    //    sRtn = sRtn.Contains("(") ? sRtn.Substring(0, sRtn.IndexOf('(')) : sRtn; // SN2217(M
                    //    //dmkim 170912
                    //    sRtn = sRtn.Trim();
                    //}
                }
            }

            if (sGetInfoType == "CODE")
            {
                sRtn = oProj.Name.Trim();
            }

            return sRtn;
        }

        private static string GetVersion()
        {
            string sRtn = string.Empty;
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Directory.GetParent(Application.ExecutablePath) + @"\pdms.dll");

            //12.0, 12.1
            sRtn = versionInfo.FileMajorPart + "." + versionInfo.FileMinorPart;
            return sRtn;
        }

        private static E.MODULE GetModule()
        {
            string sInfo = Aveva.Pdms.PdmsVersion.GetShortBanner();
            E.MODULE module = (E.MODULE)Enum.Parse(typeof(E.MODULE), sInfo.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2]);

            return module;
        }
        /// <summary>
        /// 현재 MDB를 가져온다. 
        /// "/" 빠져서 나옴.
        /// </summary>
        /// <returns></returns>
        private static MDB GetCurrentMDB()
        {
            return MDB.CurrentMDB;
        }
        /// <summary>
        /// Login User Name을 가져옴
        /// </summary>
        /// <returns></returns>
        private static string GetLoginUesrName()
        {
            return Project.CurrentProject.UserName;
        }
        #endregion

        public static class Command
        {
            /// <summary>
            /// Command Window에 명령을 날림.
            /// </summary>
            /// <param name="sCmd"></param>
            public static void CommandForPDMS(string sCmd, bool bUseRunInPDMS = false)
            {
                try
                {
                    Aveva.Pdms.Utilities.CommandLine.Command commandForPDMS = Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(sCmd);
                    if (bUseRunInPDMS)
                        commandForPDMS.RunInPdms();
                    else
                        commandForPDMS.Run();
                }
                catch { }
            }

            public static bool CommandRunOK(string sCmd)
            {
                bool bOK = false;

                try
                {
                    Aveva.Pdms.Utilities.CommandLine.Command comm = Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(sCmd);
                    bOK = comm.Run();
                }
                catch
                {
                }

                return bOK;
            }

            /// <summary>
            /// Command Window에 메시지 표시하기 위해서.
            /// </summary>
            /// <param name="Msg"></param>
            public static void PrintInCommandWindow(string Msg)
            {
                if (Msg.Contains("\n") == false)
                    Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("$p " + DateTime.Now.ToString("HH:mm:ss") + " : " + Msg).RunInPdms();
                else
                {
                    string[] tt = Msg.Split(new char[] { '\r', '\n' });
                    foreach (var t in tt)
                        Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("$p " + t).RunInPdms();
                }
            }

            /// dmkim 200105 메시지만.
            /// <summary>
            /// Command Line 명령
            /// </summary>
            /// <param name="Msg"></param>
            public static void PrintInCommandWindowOnlyMsg(string Msg)
            {
                if (Msg.Contains("\n") == false)
                    Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("$p " + Msg).RunInPdms();
                else
                {
                    string[] tt = Msg.Split(new char[] { '\r', '\n' });
                    foreach (var t in tt)
                        Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("$p " + t).RunInPdms();
                }
            }

            /// <summary>
            /// 주어진 Syntax로 Command Line에서 정보 가져옴 (pml 글로벌 변수를 이용해서)
            /// </summary>
            /// <param name="sSyntax">PML 문장</param>
            /// <param name="sGlobalVarName">PML 글로벌 변수 ex(!!aaa, 느낌표는 제외) </param>
            /// <returns></returns>
            public static string GetPMLValue(string sSyntax, string sGlobalVarName = "INFOGETVAR")
            {
                string sPmlCode = string.Format("VAR !!{0} {1}", sGlobalVarName, sSyntax);
                Aveva.Pdms.Utilities.CommandLine.Command oComm = Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(sPmlCode);
                oComm.RunInPdms();

                var sRtn = oComm.GetPMLVariableString(sGlobalVarName).Trim();

                return sRtn;
            }

            /// <summary>
            /// Command Line에서 정보 가져옴.
            /// </summary>
            /// <param name="strVariable"></param>
            /// <returns></returns>
            public static string GetPMLValueString(string strVariable)
            {
                string str = "$p get pml value string";
                Aveva.Pdms.Utilities.CommandLine.Command comm = Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand(str);

                string result = comm.GetPMLVariableString(strVariable);
                //NISO.Display.WriteLine(result);
                string retString = result.ToString();

                return retString;
            }
            /// <summary>
            /// Command Line에서 정보 가져옴.
            /// </summary>
            /// <param name="strVariable"></param>
            /// <returns></returns>
            public static bool GetPMLValueBool(string strVariable)
            {
                Aveva.Pdms.Utilities.CommandLine.Command comm = Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("");
                bool result = comm.GetPMLVariableBoolean(strVariable);
                return result;
            }

            //dmkim 170428
            /// <summary>
            /// Command Line에서 정보 가져옴.
            /// </summary>
            /// <param name="strVariable"></param>
            /// <param name="bDeleteVar">글로벌변수삭제여부.</param>
            /// <returns></returns>
            public static double GetPMLValueDouble(string strVariable, bool bDeleteVar = false)
            {
                Aveva.Pdms.Utilities.CommandLine.Command comm = Aveva.Pdms.Utilities.CommandLine.Command.CreateCommand("");
                double result = comm.GetPMLVariableReal(strVariable);

                if (bDeleteVar)
                {
                    A.Command.CommandForPDMS(string.Format("!!{0}.Delete()", strVariable));
                }
                return result;
            }
        }

        public static class Sys
        {
            #region System
            /// <summary>
            /// SAVEWORK
            /// </summary>
            /// <param name="bUnclaimAll"></param>
            /// <param name="sMsg"></param>
            public static void SaveWork(bool bUnclaimAll = false, string sMsg = "")
            {
                MDB.CurrentMDB.SaveWork(sMsg);

                if (bUnclaimAll) { A.Sys.UnclaimAll(); }
            }

            public static void UnclaimAll()
            {
                A.Command.CommandForPDMS("UNCLAIM ALL");
            }

            /// <summary>
            /// Getwork for all MDB.
            /// </summary>
            public static void GetWork()
            {
                MDB.CurrentMDB.GetWork();
            }
            /// <summary>
            /// Getwork for given DBs.
            /// </summary>
            /// <param name="dbs"></param>
            public static void GetWork(Db[] dbs)
            {
                MDB.CurrentMDB.GetWork(dbs);
            }
            public static void GetWork(Db db)
            {
                MDB.CurrentMDB.GetWork(new Db[] { db });
            }
            /// <summary>
            /// 입력된 Password가 유효한지 체크.
            /// !sess = CURRENT SESSION
            /// !confirmed = !sess.confirmID( |/| + !this.oldPasswd.val)
            /// </summary>
            /// <param name="sPass"></param>
            /// <returns></returns>
            public static bool CheckValidPassword(string sPass)
            {
                A.Command.CommandForPDMS("!sess = CURRENT SESSION");
                A.Command.CommandForPDMS(string.Format("!!NISOPASSWORD = !sess.confirmID( |/| + '" + sPass + "')"));

                string sGlobal = "NISOPASSWORD";
                bool bRtn = A.Command.GetPMLValueBool(sGlobal);

                return bRtn;
            }
            //dmkim 190717
            public static E.COORDINATE GetCoordinate()
            {
                E.COORDINATE oRtn = E.COORDINATE.ENU;

                A.Command.CommandForPDMS("var !!NISOCOORD coord");

                string sGlobal = "NISOCOORD";

                string sRtn = A.Command.GetPMLValueString(sGlobal);

                if (sRtn == "XYZ")
                {
                    oRtn = E.COORDINATE.XYZ;
                }

                return oRtn;
            }

            /// <summary>
            /// NT로 Authentication User 인지 확인(Admin=Free 은 False)
            /// TTY 모드일 경우 Password 묻기 위해서.
            /// </summary>
            /// <returns></returns>
            public static bool isNTAuthenticationUser()
            {
                bool bRtn = false;

                if (Project.CurrentProject.IsAuthenticatedUserProject())
                {
                    bRtn = true;

                    string[] olFree = Project.CurrentProject.GetFreeUserData();
                    //[0] = "/SYSTEM (System User) [free]"
                    //[1] = "/PIPE (Outfit Piping User) [free]"

                    string sUser = A.Info.USER;

                    foreach (string sFree in olFree)
                    {
                        string sName = sFree.Split(' ')[0].Substring(1);
                        if (sName == sUser)
                        {
                            bRtn = false;
                            break;
                        }
                    }

                }

                return bRtn;
            }



            /// <summary>
            /// Get Teams 
            /// </summary>
            /// <returns></returns>
            public static List<string> GetTeamsName()
            {
                List<string> olRtn = new List<string> { };
                //olRtn.Add("MPIPE");
                //olRtn.Add("HPIPE");
                //olRtn.Add("ACCO");

                DbElement rUser = DbElement.GetElement(A.Info.USER);

                DbElement[] olrTeam = rUser.GetElementArray(DbAttributeInstance.TEAMLS);

                foreach (DbElement rTeam in olrTeam)
                {
                    olRtn.Add(ATT.FLNM(rTeam, false).Substring(1));
                }


                return olRtn;
            }
            /// <summary>
            /// Get Dbs 
            /// </summary>
            /// <param name="dbType"></param>
            /// <returns></returns>
            private static List<Db> GetDb(DbType dbType)
            {
                Db[] olDb = MDB.CurrentMDB.GetDBArray(dbType);
                return olDb.ToList();
            }
            /// <summary>
            /// 현재 MDB에서 주어진 DbType, DbName으로 해당되는 World Element를 가져온다.
            /// </summary>
            /// <param name="dbType"></param>
            /// <param name="sDbName"></param>
            /// <param name="rWorld"></param>
            /// <returns></returns>
            public static bool GetWorldElementFromDbName(DbType dbType, string sDbName, ref DbElement rWorld)
            {
                bool bRtn = false;

                List<string> olDBName = new List<string> { };
                foreach (Db oDb in A.Sys.GetDb(dbType))
                {
                    olDBName.Add(oDb.Name);
                    if (oDb.Name == sDbName) { rWorld = oDb.World; bRtn = true; break; }
                }

                if (bRtn == false)
                {
                    foreach (string sDBName in olDBName)
                    {
                        INFOGET_ZERO_HULL.Display.WriteLine("DB :" + sDbName);
                    }
                }

                return bRtn;
            }

            #endregion
        }

        public static class Element
        {
            #region Element
            /// <summary>
            /// 너무 자주사용해서 배치.
            /// </summary>
            /// <param name="rPart"></param>
            /// <returns></returns>
            public static string Flnm(DbElement rPart)
            {
                return ATT.FLNM(rPart);
            }
            /// <summary>
            /// 너무 자주사용해서 배치.
            /// </summary>
            /// <param name="rPart"></param>
            /// <returns></returns>
            public static string Parnam(DbElement rPart)
            {
                return ATT.PARNAM(rPart);
            }

            /// <summary>
            /// Element Type 정보가져오기.
            /// </summary>
            /// <param name="sPart"></param>
            /// <returns></returns>
            public static string Type(string sPart)
            {
                return ATT.TYPE(DbElement.GetElement(sPart));
            }

            /// <summary>
            /// CE SET
            /// </summary>
            /// <param name="sPart"></param>
            public static void SetCE(string sPart)
            {
                A.Element.CE_Set(DbElement.GetElement(sPart));
            }
            /// <summary>
            /// CE SET
            /// </summary>
            /// <param name="rPart"></param>
            public static void CE_Set(DbElement rPart)
            {
                if (IsValidElement(rPart))
                {
                    CurrentElement.Element = rPart;
                }
            }
            /// <summary>
            /// CE GET
            /// </summary>
            /// <returns></returns>
            public static DbElement CE_Get()
            {
                return CurrentElement.Element;
            }

            public static string GetCE()
            {
                return A.Element.Flnm(A.Element.CE_Get());
            }
            /// <summary>
            /// 삭제가능한지 여부.
            /// </summary>
            /// <param name="sPart"></param>
            /// <returns></returns>
            public static bool IsDeleteable(string sPart)
            {
                return DbElement.GetElement(sPart).IsDeleteable;
            }

            /// <summary>
            /// 하위 DbElement를 Lock 또는 Unlock.
            /// </summary>
            /// <param name="rRoot"></param>
            /// <param name="bLock">Lock True/False</param>
            public static void Lock(DbElement rRoot, bool bLock = true)
            {
                //System.Diagnostics.Debugger.Launch();
                List<DbElement> olrAllMember = A.GetCollect(rRoot, DbElementTypeInstance.TEXT, true);
                foreach (DbElement rElement in olrAllMember)
                {
                    try
                    {
                        rElement.SetAttribute(DbAttributeInstance.LOCK, bLock);
                    }
                    catch { }
                }

            }

            #endregion

            #region Attribute
            /// <summary>
            /// POSTION 정보 가져옴
            /// </summary>
            /// <param name="rPart">가져올 ELEMENT, Zone Ori정보확인.</param>
            /// <param name="oAttribute">PPOS,POS,APOS,LPOS,HPOS,TPOS...</param>
            /// <param name="index">Optional</param>
            /// <returns>Point3D</returns>
            public static Point3D GetPos(DbElement rPart, DbAttribute oAttribute, int iIndex = -1)
            {
                Point3D oRtn = new Point3D();

                //////////////////////////////////////////////////////////////////////////
                /// 에러체크, POS, APOS,LPOS,HPOS,TPOS iIndex의 값이 존재하면 안됨
                /// 실수로 인자값 넣었다면 -1로 변경.
                if (oAttribute == DbAttributeInstance.POS ||
                    oAttribute == DbAttributeInstance.APOS || oAttribute == DbAttributeInstance.LPOS ||
                    oAttribute == DbAttributeInstance.HPOS || oAttribute == DbAttributeInstance.TPOS)
                {
                    if (iIndex != -1) iIndex = -1;
                }

                double[] olData = new double[] { };

                if (iIndex == -1) olData = rPart.GetDoubleArray(oAttribute);
                else olData = rPart.GetDoubleArray(oAttribute, iIndex);

                oRtn = new Point3D(olData[0], olData[1], olData[2]);



                //////////////////////////////////////////////////////////////////////////
                /// PART의 OWNER(BRAN).OWNER(PIPE).OWNER(ZONE)

                //dmkim 180108 PIPCA일경우. 한단계 더 하위이기때문.
                //dmkim 180309 오류수정
                DbElement rZone = rPart.Owner.Owner.Owner;
                if (rZone.GetElementType() == DbElementTypeInstance.PIPE)
                {
                    rZone = rZone.Owner;
                }
                A.Element.GetPosByZoneOri(rZone, ref oRtn);

                return oRtn;
            }

            /// <summary>
            /// DIRECTION 정보 가져옴
            /// </summary>
            /// <param name="rPart">가져올 ELEMENT, Zone Ori정보확인.</param>
            /// <param name="oAttribute">PDIR,DIR(VIEW),ADIR,LDIR,HDIR,TDIR..</param>
            /// <param name="index">Optional</param>
            /// <returns></returns>
            public static Vector3D GetDir(DbElement rPart, DbAttribute oAttribute, int iIndex = -1)
            {
                Vector3D oRtn = new Vector3D();

                double[] olData = new double[] { };

                try
                {
                    //dmkim 180108 DDIR 추가.
                    if (oAttribute == DbAttributeInstance.DIR ||
                        oAttribute == DbAttributeInstance.ADIR || oAttribute == DbAttributeInstance.LDIR ||
                        oAttribute == DbAttributeInstance.HDIR || oAttribute == DbAttributeInstance.TDIR ||
                        oAttribute == DbAttributeInstance.DDIR)
                    {
                        Direction oViewDir = rPart.GetDirection(oAttribute);
                        olData = new double[] { oViewDir.East, oViewDir.North, oViewDir.Up };
                    }
                    else olData = rPart.GetDoubleArray(oAttribute, iIndex);

                    oRtn = new Vector3D(olData[0], olData[1], olData[2]);

                    //////////////////////////////////////////////////////////////////////////
                    /// PART의 OWNER(BRAN).OWNER(PIPE).OWNER(ZONE)
                    A.Element.GetDirByZoneOri(rPart.Owner.Owner.Owner, ref oRtn);
                }
                catch // TUBI의 경우 못가져올때가 많음. 따로 계산.
                {
                }

                return oRtn;
            }

            /// <summary>
            /// Zone의 Orientaion을 반영한 POS값
            /// </summary>
            /// <param name="rZonePart"></param>
            /// <param name="oPoint3D"></param>
            public static void GetPosByZoneOri(DbElement rZonePart, ref Point3D oPoint3D)
            {
                //dmkim 170124
                if (rZonePart.GetElementType() == DbElementTypeInstance.NODISPLACEMENT)//UNKNOW
                {
                    rZonePart = rZonePart.Owner;
                }

                Aveva.Pdms.Geometry.Orientation oOri = ATT.ORI(rZonePart);

                oPoint3D.X = oPoint3D.X * oOri.XDir().East + oPoint3D.Y * oOri.YDir().East + oPoint3D.Z * oOri.ZDir().East;
                oPoint3D.Y = oPoint3D.X * oOri.XDir().North + oPoint3D.Y * oOri.YDir().North + oPoint3D.Z * oOri.ZDir().North;
                oPoint3D.Z = oPoint3D.X * oOri.XDir().Up + oPoint3D.Y * oOri.YDir().Up + oPoint3D.Z * oOri.ZDir().Up;
            }
            private static void GetDirByZoneOri(DbElement rZonePart, ref Vector3D oVector3D)
            {
                Aveva.Pdms.Geometry.Orientation oOri = ATT.ORI(rZonePart);

                oVector3D.X = oVector3D.X * oOri.XDir().East + oVector3D.Y * oOri.YDir().East + oVector3D.Z * oOri.ZDir().East;
                oVector3D.Y = oVector3D.X * oOri.XDir().North + oVector3D.Y * oOri.YDir().North + oVector3D.Z * oOri.ZDir().North;
                oVector3D.Z = oVector3D.X * oOri.XDir().Up + oVector3D.Y * oOri.YDir().Up + oVector3D.Z * oOri.ZDir().Up;
            }

            //dmkim 171024
            /// <summary>
            /// Element의 Dbwrite 여부.
            /// </summary>
            /// <param name="sPart"></param>
            /// <returns></returns>
            public static bool GetDbwrite(string sPart)
            {
                DbElement rPart = DbElement.GetElement(sPart);
                A.Command.CommandForPDMS("!igrGetDbElementPart = !igGetDbElementPart.dbref()");
                A.Command.CommandForPDMS("!NISODBWRITE = igrGetDbElementPart.dbwrite");
                A.Command.CommandForPDMS("!igGetDbElementPart = '" + ATT.FLNM(rPart) + "'");
                string sGlobal = "NISODBWRITE";
                bool bRtn = A.Command.GetPMLValueBool(sGlobal);


                return bRtn;
            }


            /// <summary>
            /// Connection 정보 가져오기
            /// ACONN LCONN PCONN
            /// </summary>
            /// <param name="rPart"></param>
            /// <param name="oAttribute"></param>
            /// <param name="iIndex"></param>
            /// <returns></returns>
            public static string GetConnType(DbElement rPart, DbAttribute oAttribute, int iIndex = -1)
            {
                string sRtn = string.Empty;

                if (oAttribute == DbAttributeInstance.ACON || oAttribute == DbAttributeInstance.LCON)
                {
                    sRtn = rPart.GetString(oAttribute);
                }
                else if (oAttribute == DbAttributeInstance.PPCO)
                {
                    if (iIndex == -1) iIndex = 0;
                    sRtn = rPart.GetString(oAttribute, iIndex);
                }
                //dmkim 180108
                else if (oAttribute == DbAttributeInstance.DCON)
                {
                    sRtn = rPart.GetString(oAttribute, 0);
                }
                else Log.AddDev("A.GetConnType FAIL : " + ATT.FLNM(rPart));

                return sRtn;
            }

            /// <summary>
            /// BORE(Diameter값 가져오기)
            /// ABORE,LBORE,PHOD,PTOD,P1BORE,P1OD
            /// TUBE는 PHOD,PTOD등 OUTDIAMETER 없음.
            /// </summary>
            /// <param name="rPart"></param>
            /// <param name="oAttribute"></param>
            /// <param name="iIndex"></param>
            /// <returns></returns>
            public static double GetBore(DbElement rPart, DbAttribute oAttribute, int iIndex = -1)
            {
                double dRtn = 0.0;

                //dmkim 171102 GET PML.
                //dmkim 180108 SHI만. 적용.
                bool bGetPML = false;
                if (D.SHIPYARD == E.SHIPYARD.SHI)
                {
                    bGetPML = true;
                    ////dmkim 180309 TEE, REDU등 값 가져올 때 2번째 값 동일하게 가져와서..
                    //bGetPML = false;

                    //UNIT가 MM가 아니면.bGetPML = false
                    //dmkim 180807
                    string sGlobal = "NISOUNIT";
                    try
                    {
                        A.Command.CommandForPDMS("var !!NISOUNIT UNIT");
                        string sRtn = A.Command.GetPMLValueString(sGlobal);
                        if (sRtn.Contains("MM Bore MM Distance") == false)
                        {
                            bGetPML = false;
                        }
                    }
                    catch
                    {

                    }

                }

                if (bGetPML)
                {
                    dRtn = A.Element.GetPMLBore(rPart, oAttribute, iIndex);
                }
                else
                {
                    try
                    {
                        if (oAttribute == DbAttributeInstance.ABOR || oAttribute == DbAttributeInstance.LBOR)
                        {
                            dRtn = rPart.GetDouble(oAttribute);
                        }
                        else if ((ATT.TYPE(rPart) != "TUBI") &&
                            oAttribute == DbAttributeInstance.PHOD || oAttribute == DbAttributeInstance.PTOD)
                        {
                            dRtn = rPart.GetDouble(oAttribute);
                        }
                        else if (oAttribute == DbAttributeInstance.PPBO)
                        {
                            if (iIndex == -1) iIndex = 0;
                            dRtn = rPart.GetDouble(oAttribute, iIndex);
                        }
                        else if (oAttribute == DbAttributeInstance.POD)
                        {
                            if (iIndex == -1) iIndex = 0;
                            dRtn = rPart.GetDouble(oAttribute, iIndex);
                        }
                        //dmkim 180108
                        else if (oAttribute == DbAttributeInstance.BORE)
                        {
                            dRtn = rPart.GetDouble(oAttribute);
                        }
                        else
                        {
                            Log.AddDev("A.GetBore CHECK : " + ATT.FLNM(rPart));
                        }
                    }
                    catch
                    {
                        Log.AddDev("A.GetBore FAIL : " + ATT.FLNM(rPart));
                    }
                }


                return dRtn;
            }

            /// <summary>
            /// 특정 CATA에서 Nominal Bore를 C#에서가져오지 못한다. ( PML Return 형식으로 가져옴.)
            /// PARA1의 값이 ND값이 아닐 경우.. SHI SN2217 Project
            /// </summary>
            /// <param name="rPart"></param>
            /// <param name="oAttribute"></param>
            /// <param name="iIndex"></param>
            /// <returns></returns>
            public static double GetPMLBore(DbElement rPart, DbAttribute oAttribute, int iIndex = -1)
            {
                double dRtn = 0.0;


                A.Command.CommandForPDMS("!igGetBoreDbElementPart = '" + ATT.FLNM(rPart) + "'");
                A.Command.CommandForPDMS("!igrGetBoreDbElementPart = !igGetBoreDbElementPart.dbref()");

                string sGlobal = "NISOBORE";

                bool bCheck = true;
                try
                {
                    if (oAttribute == DbAttributeInstance.ABOR)
                    {
                        A.Command.CommandForPDMS("!!NISOBORE = !igrGetBoreDbElementPart.ABORE.String().Real()");
                    }
                    else if (oAttribute == DbAttributeInstance.LBOR)
                    {
                        A.Command.CommandForPDMS("!!NISOBORE = !igrGetBoreDbElementPart.LBORE.String().Real()");
                    }

                    else if (oAttribute == DbAttributeInstance.PHOD)
                    {
                        if ((ATT.TYPE(rPart) == "TUBI")) { bCheck = false; }
                        A.Command.CommandForPDMS("!!NISOBORE = !igrGetBoreDbElementPart.PHOD");
                    }
                    else if (oAttribute == DbAttributeInstance.PTOD)
                    {
                        if ((ATT.TYPE(rPart) == "TUBI")) { bCheck = false; }
                        A.Command.CommandForPDMS("!!NISOBORE = !igrGetBoreDbElementPart.PTOD");
                    }
                    else if (oAttribute == DbAttributeInstance.PPBO)
                    {
                        if (iIndex == -1) iIndex = 0;

                        A.Command.CommandForPDMS("!!NISOBORE = !igrGetBoreDbElementPart.PPBORE[" + iIndex.ToString() + "].String().Real()");

                    }
                    else if (oAttribute == DbAttributeInstance.POD)
                    {
                        if (iIndex == -1) iIndex = 0;

                        A.Command.CommandForPDMS("!!NISOBORE = !igrGetBoreDbElementPart.POD[" + iIndex.ToString() + "]");
                    }
                    //dmkim 180108
                    else if (oAttribute == DbAttributeInstance.BORE)
                    {
                        A.Command.CommandForPDMS("!!NISOBORE = !igrGetBoreDbElementPart.BORE.String().Real()");
                    }
                    else
                    {
                        bCheck = false;
                    }

                    if (bCheck)
                    {
                        dRtn = A.Command.GetPMLValueDouble(sGlobal, true);

                        //A.Command.CommandForPDMS("!!NISOBORE.Delete()");
                    }
                    else
                    {
                        Log.AddDev("A.GetBore CHECK : " + ATT.FLNM(rPart));
                    }
                }
                catch
                {
                    Log.AddDev("A.GetBore FAIL : " + ATT.FLNM(rPart));
                }



                return dRtn;
            }

            //dmkim 170329
            /// <summary>
            /// get PREV ( ileave Tubi 포함)
            /// </summary>
            /// <param name="rPart"></param>
            /// <param name="rPrev"></param>
            /// <returns></returns>
            public static bool GetPrev(DbElement rPart, ref DbElement rPrev)
            {
                if (ATT.TYPE(rPart) == "TUBI")
                {
                    rPrev = A.PipeCollect.getFittfromileaveTube(rPart);
                }
                else
                {
                    if (A.IsValidElement(rPart.Previous))
                    {
                        bool bSuccess = true;
                        DbElement rTubi = A.PipeCollect.getileaveTube(rPart.Previous, ref bSuccess);
                        if (bSuccess) rPrev = rTubi;
                        else rPrev = rPart.Previous;
                    }
                    else
                    {
                        //BRAN의 ileave Tubi
                        bool bSuccess = true;
                        DbElement rTubi = A.PipeCollect.getileaveTube(rPart.Owner, ref bSuccess);
                        if (bSuccess) rPrev = rTubi;
                        else { rPrev = DbElement.GetElement(); }
                        //시작 Fitting 일 경우. rPrev is Null..

                    }
                }
                return A.IsValidElement(rPrev);
            }

            /// <summary>
            /// get Next ( ileave Tubi 포함)
            /// </summary>
            /// <param name="rPart"></param>
            /// <param name="rNext"></param>
            /// <returns></returns>
            public static bool GetNext(DbElement rPart, ref DbElement rNext)
            {
                bool bSuccess = true;

                if (ATT.TYPE(rPart) == "TUBI")
                {
                    rNext = rPart.Next();
                }
                else
                {
                    DbElement rTubi = A.PipeCollect.getileaveTube(rPart, ref bSuccess);
                    if (bSuccess) rNext = rTubi;
                    else rNext = rPart.Next();
                }

                return A.IsValidElement(rNext);
            }


            /// <summary>
            /// Element의 특정 PPoint의 Direction을 String으로 가져옴. ==>DIR W45N45D
            /// </summary>
            /// <param name="rPart"></param>
            /// <param name="iIndex"></param>
            /// <param name="bSpaceTrim"></param> //dmkim 190725
            /// <returns></returns>
            public static string GetDirStringFromPPoint(DbElement rPart, int iIndex, bool bSpaceTrim = true)
            {
                string sRtn = string.Empty;
                try
                {
                    Vector3D oDir = A.Element.GetDir(rPart, DbAttributeInstance.PDIR, iIndex);
                    Direction oXXX = Direction.Create();
                    oXXX.East = oDir.X;
                    oXXX.North = oDir.Y;
                    oXXX.Up = oDir.Z;
                    sRtn = oXXX.ToString();

                    sRtn = "DIR " + sRtn.Replace("-X", "W").Replace("X", "E").Replace("-Y", "S").Replace("Y", "N").Replace("-Z", "D").Replace("Z", "U").Trim();
                    if (bSpaceTrim) { sRtn = sRtn.Replace(" ", ""); }

                }
                catch { }


                return sRtn;
            }

            //dmkim 170428
            public static double GetBThkFromBltp(DbElement rPart, DbElement rBltp)
            {
                double dRtn = 0.0;

                string sBoltThk = ATT.BTHK(rBltp);

                //dmkim 170908 Thk가 값으로 들어올때도 있음.
                if (double.TryParse(sBoltThk, out dRtn)) { return dRtn; }

                string sFlnm = ATT.FLNM(rPart);

                //PLM방식으로 바로 가져옴.
                string sGlobal = "NISOBOLTTHICKNESS";
                try
                {
                    string sCmd = "!!" + sGlobal + " = " + sBoltThk + " of " + sFlnm;
                    sCmd = sCmd.Replace("(", "").Replace(")", "").Trim();
                    A.Command.CommandForPDMS(sCmd);
                    dRtn = A.Command.GetPMLValueDouble(sGlobal);
                }
                catch { }

                return dRtn;
            }




            public static string GetStringProperty(DbElement rPart, DbElement rElement, DbAttribute oAttribute, string sDefaultVale)
            {
                if (A.IsValidElement(rElement) == false) { return sDefaultVale; }

                //if (DbAttributeExtensions.IsExpression(oAttribute)) { return sDefaultVale; }

                string sRtn = sDefaultVale;

                try
                {
                    string value = rElement.GetString(oAttribute);

                    DbExpression exp = (DbExpression)null;
                    Aveva.Pdms.Utilities.Messaging.PdmsMessage error = (Aveva.Pdms.Utilities.Messaging.PdmsMessage)null; ;

                    if (DbExpression.Parse(value, out exp, out error))
                    {
                        sRtn = rPart.EvaluateString(exp);
                    }
                    else
                    {
                    }
                }
                catch (Exception)
                {
                }

                return sRtn;
            }

            #endregion

            //Member
            public static List<DbElement> Members(DbElement rPart, DbElementType oDbElementType)
            {
                return rPart.Members(oDbElementType).ToList();
            }

            //CREATE
            public static DbElement CreateLast(string sOwner, DbElementType oType, string sName)
            {
                return CreateLast(DbElement.GetElement(sOwner), oType, sName);
            }
            public static DbElement CreateLast(DbElement rOwner, DbElementType oType, string sName)
            {
                DbElement rRtn = rOwner.CreateLast(oType);
                rRtn.SetAttribute(DbAttributeInstance.NAME, sName);
                return rRtn;
            }

            //Delete
            public static string Delete(string sPart)
            {
                return Delete(DbElement.GetElement(sPart));
            }
            public static string Delete(DbElement rPart)
            {
                string sErrMsg = string.Empty; 
                if (rPart.IsDeleteable == false) { sErrMsg = "This Element Can't be Deleted. (IsDeleteable = false)"; }

                try
                {
                    rPart.Delete();
                }
                catch
                {
                    sErrMsg = "This element has failed to be deleted. ( .Delete())";
                }
                return sErrMsg;
            }

        }

        /// <summary>
        /// PIPE 관련한 Collect 모음.
        /// </summary>
        public static class PipeCollect
        {
            /// <summary>
            /// Part로 부터 Site 이름을 가져옴.
            /// </summary>
            /// <param name="rPart"></param>
            /// <param name="bContainSlash">"/" 포함여부.</param>
            /// <returns></returns>
            public static string GetSiteNameFromPart(DbElement rPart, bool bContainSlash = true)
            {
                string sRtn = string.Empty;

                if (A.IsValidElement(rPart) == false)
                {
                    return sRtn;
                }
                try
                {
                    sRtn = ATT.FLNM(rPart.Owner.Owner.Owner.Owner, bContainSlash);
                }
                catch { }

                return sRtn;
            }

            /// <summary>
            /// BRAN이름에서 Pipe이름을 빼면 나오는 숫자. B1
            /// </summary>
            /// <param name="rPart"></param>
            /// <returns></returns>
            public static string GetBranchNoFromPart(DbElement rPart)
            {
                string sRtn = string.Empty;

                if (A.IsValidElement(rPart) == false)
                {
                    return sRtn;
                }
                try
                {
                    string sPipeName = A.PipeCollect.GetPipeNameFromPart(rPart);
                    string sBranName = ATT.FLNM(rPart.Owner);

                    sRtn = sBranName.Replace(sPipeName, "");

                }
                catch { }

                return sRtn;
            }

            /// <summary>
            /// PART ELEMENT이름에서 TYPE+번호 EX) ELBO 1, 
            /// </summary>
            /// <param name="rPart"></param>
            /// <returns></returns>
            public static string GetTypeNoFromPart(DbElement rPart)
            {
                string sRtn = string.Empty;

                if (A.IsValidElement(rPart) == false)
                {
                    return sRtn;
                }
                try
                {
                    string sName = ATT.FLNM(rPart);
                    string sType = ATT.TYPE(rPart);

                    if (sName.Contains(sType))
                    {
                        sRtn = sType + "_" + sName.Replace(sType, "").Split(' ')[1];
                    }
                    else
                    {
                        sRtn = sType + "_XX";
                        if (sType == "TUBI")
                        {
                            sRtn = sType + "__";
                        }
                    }

                }
                catch { }

                return sRtn;
            }

            /// <summary>
            /// 주어진 Element 로부터 Pipe 이름을 가져온다.
            /// </summary>
            /// <param name="rPart"></param>
            /// <param name="bContainSlash">"/"를 포함할지 여부.</param>
            /// <returns></returns>
            public static string GetPipeNameFromPart(DbElement rPart, bool bContainSlash = true)
            {
                string sRtn = string.Empty;

                if (A.IsValidElement(rPart) == false)
                {
                    return sRtn;
                }
                try
                {
                    sRtn = ATT.FLNM(rPart.Owner.Owner, bContainSlash);
                }
                catch { }

                return sRtn;
            }

            //JST 20190115
            public static DbElement[] GetSpoolsFromPipe(DbElement rPipe)
            {
                DbElement spldrg = DbElement.GetElement($"//{ATT.FLNM(rPipe)}");
                if (spldrg.IsValid == false) return null;

                return spldrg.Members(DbElementTypeInstance.SPOOL);
            }

            //dmkim 170207
            /// <summary>
            /// 주어진 Element 로부터 Branch 이름을 가져온다.
            /// </summary>
            /// <param name="rPart"></param>
            /// <param name="bContainSlash">"/"를 포함할지 여부.</param>
            /// <returns></returns>
            public static string GetBranNameFromPart(DbElement rPart, bool bContainSlash = true)
            {
                string sRtn = string.Empty;

                if (A.IsValidElement(rPart) == false)
                {
                    return sRtn;
                }
                try
                {
                    sRtn = ATT.FLNM(rPart.Owner, bContainSlash);
                }
                catch { }

                return sRtn;
            }

            //dmkim 170207
            /// <summary>
            /// 주어진 Element 로부터 Branch No를 가져온다. (XXXX/B1 -> 1)
            /// </summary>
            /// <param name="rPart"></param>
            /// <param name="bContainSlash">"/"를 포함할지 여부.</param>
            /// <returns></returns>
            public static int GetBranNoFromPart(DbElement rPart, bool bContainSlash = true)
            {
                int iRtn = -1;

                if (A.IsValidElement(rPart) == false)
                {
                    return iRtn;
                }
                try
                {
                    string sRtn = ATT.FLNM(rPart.Owner, bContainSlash);
                    iRtn = Convert.ToInt32(sRtn.Split('/').Last().Replace("B", "").Trim());
                }
                catch { }

                return iRtn;
            }


            //dmkim 171027
            /// <summary>
            /// Part의 이름 Part Number를 가져온다. sfref로 해당 스풀 찾은다음에 SPLTBL에서. 
            /// </summary>
            /// <param name="rPart"></param>
            /// <returns>=24170/97461*9</returns>
            public static string GetPartNoFromPartName(string sPart)
            {
                string sRtn = string.Empty;

                DbElement rPart = DbElement.GetElement(sPart);

                if (A.IsValidElement(rPart))
                {
                    DbElement rSpool = ATT.SFREF(rPart);

                    if (A.IsValidElement(rSpool))
                    {
                        List<int> olRtn = ATT.SPLTBL(rSpool);

                        for (int i = 0; i < olRtn.Count / 4; i++)
                        {
                            string sRefNo = "=" + olRtn[i * 4 + 0].ToString() + "/" + olRtn[i * 4 + 1].ToString();
                            string sName = ATT.FLNM(DbElement.GetElement(sRefNo));

                            if (sName == sPart)
                            {
                                int iRtn = olRtn[i * 4 + 3];

                                sRtn = iRtn.ToString();

                                sRtn = sRefNo + "*" + sRtn;

                            }
                        }
                    }
                    //dmkim 180125 SPOOLING이 되지 않았을 경우.
                    else
                    {
                        sRtn = ATT.REFNO(rPart);
                    }
                }

                return sRtn;
            }

            //dmkim 190515
            /// <summary>
            /// Branch 로 부터 모든 SPOOL (Part 의 SFREF) 을 가져온다.
            /// </summary>
            /// <param name="rBran"></param>
            /// <returns></returns>
            public static List<DbElement> GetSpoolElementFromBranch(DbElement rBran)
            {
                List<DbElement> olrRtn = new List<DbElement> { };

                List<DbElement> olrMember = A.PipeCollect.GetBranchMemberFromBranch(rBran);

                foreach (DbElement rPart in olrMember)
                {
                    DbElement rSpoolField = ATT.SFREF(rPart);
                    if (olrRtn.Contains(rSpoolField) == false) { olrRtn.Add(rSpoolField); }
                }

                return olrRtn;
            }

            /// <summary>
            /// 파이프의 모든 PART 검색
            /// bIncludeBran = true (BRAN 포함)
            /// </summary>
            /// <param name="rPipe"></param>
            /// <param name="bIncludeBran"></param>
            /// <returns></returns>
            public static List<DbElement> GetBranchMemberFromPipe(DbElement rPipe, bool bIncludeBran = false)
            {
                List<DbElement> olRtn = new List<DbElement>();

                List<DbElement> olBran = A.GetCollect(rPipe, DbElementTypeInstance.BRANCH);

                foreach (DbElement rBran in olBran)
                {
                    olRtn.AddRange(GetBranchMemberFromBranch(rBran, false, bIncludeBran));
                }

                return olRtn;
            }
            //dmkim 170316
            /// <summary>
            /// Branch의 Member를 가져온다. (옵션에따라 특정파트는 안가져올수도 있음)
            /// </summary>
            /// <param name="rBran"></param>
            /// <param name="bGetZeroDistPipe">길이가 0인 Tube 혹은 Bend 가져옴.</param>
            /// <param name="bIncludeBran"></param>
            /// <returns></returns>
            public static List<DbElement> GetBranchMemberFromBranch(DbElement rBran, bool bGetZeroDistPipe = false, bool bIncludeBran = false)
            {
                List<DbElement> olRtn = new List<DbElement>();

                DBElementCollection collection = new DBElementCollection(rBran);
                foreach (DbElement rMember in collection)
                {
                    if (ATT.TYPE(rMember) == "BRAN")
                    {
                        if (bIncludeBran) olRtn.Add(rBran);
                        continue;
                    }

                    if (bGetZeroDistPipe == false &&
                        (rMember.GetElementType().Equals(DbElementTypeInstance.TUBING) ||
                        rMember.GetElementType().Equals(DbElementTypeInstance.BEND)))
                    {
                        //dmkim 190318
                        try
                        {
                            Point3D oAPos = A.Element.GetPos(rMember, DbAttributeInstance.APOS);
                            Point3D oLPos = A.Element.GetPos(rMember, DbAttributeInstance.LPOS);
                            if (oAPos.DistanceToPoint(oLPos) < D.dMinGapPartConn)
                            {
                                continue;
                            }
                        }
                        catch { }
                    }

                    olRtn.Add(rMember);
                }
                return olRtn;
            }


            //dmkim 160822
            /// <summary>
            /// (SYSTEM ISO)파이프에 연결된 파이프들 모두 가져오기
            /// </summary>
            /// <remarks> 다른파이프내에서 연결되지 않은 Bran이 있으면. 제외. 체크할 것. </remarks>
            /// <param name="rCE"></param>
            /// <returns></returns>
            public static List<DbElement> GetSystemPipesFromPipe(DbElement rCE)
            {
                List<DbElement> olrRtn = new List<DbElement>();

                List<DbElement> olrConnectedBran = new List<DbElement> { };

                List<DbElement> olrOriBran = A.GetCollect(rCE, DbElementTypeInstance.BRANCH);

                List<DbElement> olrOriFitting = new List<DbElement> { };


                List<DbElement> olrAllBranMember = A.PipeCollect.GetBranchMemberFromPipe(rCE, true);


                return olrRtn;
            }

            /// <summary>
            /// BRANCH의 Member Type 들. Q list (ELBO,BEND....TUBI)
            /// </summary>
            /// <returns></returns>
            public static List<string> GetBranMemberType()
            {
                List<string> olRtn = new List<string> { };
                DbElementType[] olMember = DbElementTypeInstance.BRANCH.MemberTypes();

                foreach (DbElementType oDBElementType in olMember)
                {
                    olRtn.Add(oDBElementType.ShortName);
                }
                return olRtn;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="rSpldrg"></param>
            /// <param name="bContainSlash"></param>
            /// <returns></returns>
            public static string GetPipeNameFromSpldrg(DbElement rSpldrg, bool bContainSlash = true)
            {
                string sRtn = string.Empty;

                List<DbElement> olrSpool = A.GetCollect(rSpldrg, DbElementTypeInstance.SPOOL);
                List<DbElement> olrField = A.GetCollect(rSpldrg, DbElementTypeInstance.FIELD);

                foreach (DbElement rSpool in olrSpool)
                {
                    List<DbElement> olrPart = ATT.SPLMEM(rSpool);

                    if (olrPart.Count > 0)
                    {
                        if (A.IsValidElement(olrPart[0]))
                        {
                            sRtn = A.PipeCollect.GetPipeNameFromPart(olrPart[0]);
                            break;
                        }
                    }
                }
                if (sRtn.Length == 0)
                {
                    foreach (DbElement rField in olrField)
                    {
                        List<DbElement> olrPart = ATT.SPLMEM(rField);
                        if (olrPart.Count > 0)
                        {
                            if (A.IsValidElement(olrPart[0]))
                            {
                                sRtn = A.PipeCollect.GetPipeNameFromPart(olrPart[0]);
                                break;
                            }
                        }
                    }
                }


                return sRtn;
            }
            //dmkim 180228
            /// <summary>
            /// 파이프로 부터 SPLDRG 이름 가져온다. 처음으로검색된 SFREF의 Owner
            /// </summary>
            /// <param name="rPipe"></param>
            /// <param name="bContainSlash"></param>
            /// <returns></returns>
            public static string GetSpldrgNameFromPipe(string sPipe, bool bContainSlash = true)
            {
                string sRtn = string.Empty;

                List<DbElement> olrMember = A.PipeCollect.GetBranchMemberFromPipe(DbElement.GetElement(sPipe));

                foreach (DbElement rPart in olrMember)
                {
                    DbElement rSFREF = ATT.SFREF(rPart);
                    if (A.IsValidElement(rSFREF))
                    {
                        sRtn = ATT.FLNM(rSFREF.Owner); break;
                    }
                }

                return sRtn;
            }

            //dmkim 180530
            /// <summary>
            /// PIPE,SPLDRG,SPOOL의 모든 멤버 갯수 가져오기. (볼트는 제외됨)
            /// </summary>
            /// <param name="rElement"></param>
            /// <returns></returns>
            public static int GetPartCountFromDBElement(DbElement rElement)
            {
                int iRtn = 0;

                if (rElement.GetElementType().Equals(DbElementTypeInstance.PIPE))
                {
                    List<DbElement> olrPart = A.PipeCollect.GetBranchMemberFromPipe(rElement);
                    iRtn = olrPart.Count();
                }
                else if (rElement.GetElementType().Equals(DbElementTypeInstance.SPLDRG))
                {
                    DbElement[] olrSpool = rElement.Members(DbElementTypeInstance.SPOOL);
                    DbElement[] olrField = rElement.Members(DbElementTypeInstance.SPOOL);

                    foreach (DbElement rSpoolField in olrSpool)
                    {
                        List<DbElement> olrPart = ATT.SPLMEM(rSpoolField);
                        iRtn += olrPart.Count();
                    }
                    foreach (DbElement rSpoolField in olrField)
                    {
                        List<DbElement> olrPart = ATT.SPLMEM(rSpoolField);
                        iRtn += olrPart.Count();
                    }
                }
                else if (rElement.GetElementType().Equals(DbElementTypeInstance.SPOOL))
                {
                    List<DbElement> olrPart = ATT.SPLMEM(rElement);
                    iRtn += olrPart.Count();
                }

                return iRtn;
            }


            //dmkim 200727
            /// <summary>
            /// 현재 CE 또는 주어진 Element로 부터 PIPE이름 추출. 없을경우에 ""반환.
            /// SITE/ZONE일 경우 그대로반환.
            /// </summary>
            /// <param name="sCE"></param>
            /// <returns></returns>
            public static string GetPipeNameFromCE(string sCE)
            {
                string sRtn = string.Empty;

                DbElement rCE = DbElement.GetElement(sCE);
                if (A.IsValidElement(rCE))
                {
                    string sType = ATT.TYPE(rCE);

                    //BRAN (OWNER)
                    if (sType == "BRAN") { rCE = rCE.Owner; }
                    //BRAN Member (OWNER of OWNER)
                    else if (A.PipeCollect.GetBranMemberType().Contains(sType)) { rCE = rCE.Owner.Owner; }

                    sRtn = A.Element.Flnm(rCE);
                }
                return sRtn;

            }

            /// <summary>
            /// get PREV ( ileave Tubi 포함) & spkbrk 속성이 false 인 Atta 제외.
            /// </summary>
            /// <param name="rPart"></param>
            /// <param name="rPrev"></param>
            /// <returns></returns>
            public static bool GetPrevWithOutSpkBrkFalseAtta(DbElement rPart, ref DbElement rPrev)
            {
                if (ATT.TYPE(rPart) == "TUBI")
                {
                    rPrev = A.PipeCollect.getFittfromileaveTube(rPart);
                }
                else
                {
                    if (A.IsValidElement(rPart.Previous))
                    {
                        bool bSuccess = true;
                        DbElement rTubi = A.PipeCollect.getileaveTube(rPart.Previous, ref bSuccess);
                        if (bSuccess) rPrev = rTubi;
                        else
                        {
                            rPrev = rPart.Previous;
                            while (rPrev.IsValid && rPrev.GetElementType() == DbElementTypeInstance.ATTACHMENT && !rPrev.GetBool(DbAttributeInstance.SPKBRK))
                            {
                                rPrev = rPrev.Previous;
                            }
                        }
                    }
                    else
                    {
                        //BRAN의 ileave Tubi
                        bool bSuccess = true;
                        DbElement rTubi = A.PipeCollect.getileaveTube(rPart.Owner, ref bSuccess);
                        if (bSuccess) rPrev = rTubi;

                        //시작 Fitting 일 경우. rPrev is Null..

                    }
                }
                return A.IsValidElement(rPrev);
            }

            /// <summary>
            /// get Next ( ileave Tubi 포함) & spkbrk 속성이 false 인 Atta 제외.
            /// </summary>
            /// <param name="rPart"></param>
            /// <param name="rNext"></param>
            /// <returns></returns>
            public static bool GetNextWithOutSpkBrkFalseAtta(DbElement rPart, ref DbElement rNext)
            {
                bool bSuccess = true;
                DbElement rTubi = DbElement.GetElement();

                if (rPart.GetElementType() == DbElementTypeInstance.TUBING)
                    bSuccess = false;
                else
                    A.PipeCollect.getileaveTube(rPart, ref bSuccess);

                if (bSuccess) rNext = rTubi;
                else
                {
                    rNext = rPart.Next();
                    while (rNext.IsValid && rNext.GetElementType() == DbElementTypeInstance.ATTACHMENT && !rNext.GetBool(DbAttributeInstance.SPKBRK))
                    {
                        rNext = rNext.Next();
                    }
                }
                return A.IsValidElement(rNext);
            }

            //dmkim 190806
            /// <summary>
            /// BOLT의 NEXT 정보를 가져오기 위한.
            /// </summary>
            /// <param name="rPart"></param>
            /// <param name="rNext"></param>
            /// <returns></returns>
            public static bool GetBoltFittingNext(DbElement rPart, ref DbElement rNext)
            {

                if (A.Element.GetNext(rPart, ref rNext) == false)
                {
                    DbElement rBran = rPart.Owner;
                    if (rBran.GetElementType().Equals(DbElementTypeInstance.BRANCH))
                    {
                        DbElement rTref = ATT.TREF(rBran);

                        if (rTref.GetElementType().Equals(DbElementTypeInstance.BRANCH))
                        {
                            rNext = A.PipeCollect.GetFirstMember(rTref);
                        }
                        else
                        {
                            rNext = rTref;
                        }

                    }
                }


                return A.IsValidElement(rNext);
            }

            /// <summary>
            /// 주어진 Part로 부터 연결된 이전 스풀이름 가져오기. (SPLN 방식)
            /// </summary>
            /// <param name="rPart"></param>
            /// <param name="sSpoolName"></param>
            /// <returns></returns>
            public static string GetPrevConnSpoolName(DbElement rPart, string sSpoolName)
            {
                string sRtn = string.Empty;

                DbElement rPrev = DbElement.GetElement();
                if (A.Element.GetPrev(rPart, ref rPrev))
                {
                    string sPrevSpoolName = A.PipeCollect.GetSplnSpltSplh(rPrev);

                    if (sPrevSpoolName != sSpoolName && sPrevSpoolName != "")
                    {
                        return sPrevSpoolName;
                    }

                    if (A.Element.GetPrev(rPrev, ref rPrev) == false)
                    {
                        DbElement rHref = ATT.HREF(rPart.Owner);
                        if (A.IsValidElement(rHref))
                        {
                            string sType = ATT.TYPE(rHref);
                            if (sType == "BRAN")
                            {
                                //LAST Member
                                rPrev = A.PipeCollect.GetLastMember(rHref);
                            }
                            else if (A.PipeCollect.GetBranMemberType().Contains(sType))
                            {
                                rPrev = rHref;
                            }
                            else { rPrev = rHref; }
                        }
                    }

                    sPrevSpoolName = A.PipeCollect.GetSplnSpltSplh(rPrev);

                    if (sPrevSpoolName != sSpoolName && sPrevSpoolName != "")
                    {
                        return sPrevSpoolName;
                    }

                }
                else
                {
                    DbElement rHref = ATT.HREF(rPart.Owner);
                    if (A.IsValidElement(rHref))
                    {
                        string sType = ATT.TYPE(rHref);
                        if (sType == "BRAN")
                        {
                            //LAST Member
                            rPrev = A.PipeCollect.GetLastMember(rHref);
                        }
                        else if (A.PipeCollect.GetBranMemberType().Contains(sType))
                        {
                            rPrev = rHref;
                        }
                        else { rPrev = rHref; }
                    }

                    string sPrevSpoolName = A.PipeCollect.GetSplnSpltSplh(rPrev);

                    if (sPrevSpoolName != sSpoolName && sPrevSpoolName != "")
                    {
                        return sPrevSpoolName;
                    }
                }

                return sRtn;
            }

            /// <summary>
            /// 주어진 Part로 부터 연결된 다음 스풀이름 가져오기. (SPLN 방식)
            /// </summary>
            /// <param name="rPart"></param>
            /// <param name="sSpoolName"></param>
            /// <returns></returns>
            public static string GetNextConnSpoolName(DbElement rPart, string sSpoolName)
            {
                string sRtn = string.Empty;

                DbElement rNext = DbElement.GetElement();
                if (A.Element.GetNext(rPart, ref rNext))
                {
                    string sNextSpoolName = A.PipeCollect.GetSplnSpltSplh(rNext);

                    if (sNextSpoolName != sSpoolName && sNextSpoolName != "")
                    {
                        return sNextSpoolName;
                    }

                    if (A.Element.GetNext(rNext, ref rNext) == false)
                    {
                        DbElement rTref = ATT.TREF(rPart.Owner);
                        if (A.IsValidElement(rTref))
                        {
                            string sType = ATT.TYPE(rTref);
                            if (sType == "BRAN")
                            {
                                //First Member
                                rNext = A.PipeCollect.GetFirstMember(rTref);
                            }
                            else if (A.PipeCollect.GetBranMemberType().Contains(sType))
                            {
                                rNext = rTref;
                            }
                            else { rNext = rTref; }
                        }
                    }

                    sNextSpoolName = A.PipeCollect.GetSplnSpltSplh(rNext);

                    if (sNextSpoolName != sSpoolName && sNextSpoolName != "")
                    {
                        return sNextSpoolName;
                    }

                }
                else
                {
                    DbElement rTref = ATT.TREF(rPart.Owner);
                    if (A.IsValidElement(rTref))
                    {
                        string sType = ATT.TYPE(rTref);
                        if (sType == "BRAN")
                        {
                            //First Member
                            rNext = A.PipeCollect.GetFirstMember(rTref);
                        }
                        else if (A.PipeCollect.GetBranMemberType().Contains(sType))
                        {
                            rNext = rTref;
                        }
                        else { rNext = rTref; }
                    }

                    string sNextSpoolName = A.PipeCollect.GetSplnSpltSplh(rNext);

                    if (sNextSpoolName != sSpoolName && sNextSpoolName != "")
                    {
                        return sNextSpoolName;
                    }
                }

                return sRtn;
            }

            /// <summary>
            /// 주어진 Part로 부터 연결된 (CREF) 스풀이름 가져오기. (SPLN 방식)
            /// </summary>
            /// <param name="oComp"></param>
            /// <param name="sSpoolName"></param>
            /// <returns></returns>
            public static string GetElseConnSpoolName(DbElement rPart, string sSpoolName)
            {
                string sRtn = string.Empty;

                string sConnSpoolName = string.Empty;


                DbElement rConn = DbElement.GetElement();
                DbElement rCref = ATT.CREF(rPart);
                if (A.IsValidElement(rCref))
                {
                    string sType = ATT.TYPE(rCref);
                    if (sType == "BRAN")
                    {
                        //First Member
                        rConn = A.PipeCollect.GetFirstMember(rCref);
                    }
                    else if (A.PipeCollect.GetBranMemberType().Contains(sType))
                    {
                        rConn = rCref;
                    }
                    else { rConn = rCref; }

                    sConnSpoolName = A.PipeCollect.GetSplnSpltSplh(rConn);
                }
                else
                {
                    //PREV
                    sConnSpoolName = A.PipeCollect.GetPrevConnSpoolName(rPart, sSpoolName);
                    if (sConnSpoolName != sSpoolName && sConnSpoolName != "")
                    {
                        return sConnSpoolName;
                    }

                    //NEXT
                    sConnSpoolName = A.PipeCollect.GetNextConnSpoolName(rPart, sSpoolName);
                    if (sConnSpoolName != sSpoolName && sConnSpoolName != "")
                    {
                        return sConnSpoolName;
                    }

                }



                if (sConnSpoolName != sSpoolName && sConnSpoolName != "")
                {
                    return sConnSpoolName;
                }


                return sRtn;
            }


            public static string GetSplnSpltSplh(DbElement rPart)
            {
                string sRtn = string.Empty;

                if (A.IsValidElement(rPart) == false)
                {
                    return sRtn;
                }

                string sType = ATT.TYPE(rPart);
                if (sType == "BRAN")
                {
                    sRtn = ATT.SPLH(rPart);
                }
                else
                {
                    if (sType == "TUBI")
                    {
                        DbElement rFitting = A.PipeCollect.getFittfromileaveTube(rPart, true);
                        string sFitType = ATT.TYPE(rFitting);
                        if (sFitType == "BRAN")
                        {
                            sRtn = ATT.SPLH(rFitting);
                        }
                        else
                        {
                            sRtn = ATT.SPLT(rFitting);
                        }
                    }
                    else
                    {
                        sRtn = ATT.SPLN(rPart);
                    }
                }

                return sRtn;

            }


            /// <summary>
            /// BRAN 의 첫번째 Member (Bran Tubi 포함)
            /// </summary>
            /// <param name="rBran"></param>
            /// <param name="bExceptAtta">ATTA는 제외.</param>
            /// <returns></returns>
            public static DbElement GetFirstMember(DbElement rBran, bool bExceptAtta = false)
            {
                List<DbElement> olrMember = A.PipeCollect.GetBranchMemberFromBranch(rBran);

                DbElement rRtn = DbElement.GetElement();

                if (olrMember.Count > 0)
                {
                    rRtn = olrMember[0];

                    //dmkim 170315 ATTA 항목제외.
                    if (bExceptAtta)
                    {
                        foreach (DbElement rMember in olrMember)
                        {
                            if (rMember.GetElementType().Equals(DbElementTypeInstance.ATTACHMENT)) { continue; }
                            rRtn = rMember; break;
                        }
                    }

                }

                return rRtn;
            }
            /// <summary>
            /// BRAN의 마지막 Member (Fitting Tubi 포함)
            /// </summary>
            /// <param name="rBran"></param>
            /// <param name="bExceptAtta">ATTA는 제외.</param>
            /// <returns></returns>
            public static DbElement GetLastMember(DbElement rBran, bool bExceptAtta = false)
            {
                List<DbElement> olrMember = A.PipeCollect.GetBranchMemberFromBranch(rBran);
                olrMember.Reverse();

                DbElement rRtn = DbElement.GetElement();
                if (olrMember.Count > 0)
                {
                    rRtn = olrMember[0];

                    //dmkim 170315 ATTA 항목제외.
                    if (bExceptAtta)
                    {
                        foreach (DbElement rMember in olrMember)
                        {
                            if (rMember.GetElementType().Equals(DbElementTypeInstance.ATTACHMENT)) { continue; }
                            rRtn = rMember; break;
                        }
                    }

                }

                return rRtn;
            }


            public static bool isContainLeaveTube(DbElement rPart)
            {
                bool bRtn = false;
                A.PipeCollect.getileaveTube(rPart, ref bRtn);
                return bRtn;
            }


            public static DbElement getileaveTube(DbElement rPart, ref bool bSuccess)
            {

                string sFlnm = ATT.FLNM(rPart);
                string sTube = "ileave tube of " + sFlnm;
                DbElement rRtn = DbElement.GetElement(sTube);
                if (A.IsValidElement(rRtn) == false) bSuccess = false;

                return rRtn;
            }


            /// <summary>
            /// TUBI로 부터 Fitting 정보 가져오기. (BRAN일 경우 NulRef)
            /// </summary>
            /// <param name="rPart"></param>
            /// <param name="bGetBran">Branch를 가져오려고한다면.</param>
            /// <returns></returns>
            public static DbElement getFittfromileaveTube(DbElement rPart, bool bGetBran = false)
            {
                string sFlnm = ATT.FLNM(rPart);
                string sFitt = sFlnm.Replace("ileave tube of ", "");
                DbElement rRtn = DbElement.GetElement(sFitt);

                if (ATT.TYPE(rRtn) == "BRAN" && bGetBran == false)
                {
                    rRtn = DbElement.GetElement();
                }

                return rRtn;
            }

            //dmkim 190212  CRFA(CROSS) 도 추가하기 위해서.
            //dmkim 190722 CRFA index 정보 가져옴. 
            /// <summary>
            /// rPart에 연결된 Cref의 Dbelement (Bran일경우. First/Last Member 중 하나.)
            /// </summary>
            /// <param name="rPart">Tee/Olet 등.</param>
            /// <param name="olrConnPart">Element List</param>
            /// <param name="olArriveOrLeave">"ARRIVE","LEAVE","" 's List</param>
            /// <param name="olConnIndex"></param>
            /// <returns>성공/실패</returns>
            public static bool GetConnComp(DbElement rPart, ref List<DbElement> olrConnPart, ref List<string> olArriveOrLeave, ref List<int> olConnIndex)
            {
                bool bRtn = false;

                //dmkim 190722
                Dictionary<int, DbElement> dicCref = new Dictionary<int, DbElement> { };

                if (rPart.GetElementType().Equals(DbElementTypeInstance.CROSS))
                {
                    //System.Diagnostics.Debugger.Launch();
                    dicCref = ATT.CRFA_INDEX(rPart);
                }
                else
                {
                    DbElement rConnPart = ATT.CREF(rPart);
                    dicCref.Add(3, rConnPart);
                }

                foreach (int iConnIndex in dicCref.Keys)
                {
                    DbElement rCref = dicCref[iConnIndex];

                    if (A.IsValidElement(rCref))
                    {

                        string sType = ATT.TYPE(rCref);

                        if (sType == "BRAN")
                        {
                            int iIndex = -1;
                            if (rPart.GetElementType().Equals(DbElementTypeInstance.CROSS))
                            {
                                //iIndex = A.GetCrfaIndexNo(rPart, rCref);
                                iIndex = iConnIndex;
                            }
                            else
                            {
                                iIndex = A.PipeCollect.GetCRefIndexNo(rPart);
                            }
                            Point3D oCPos = A.Element.GetPos(rPart, DbAttributeInstance.PPOS, iIndex);

                            double dMinDist = D.dMinNewGapPartConn;
                            //dmkim 170331 아주약간 벌어진 경우. /H110T-2IN-HL-1331-111-F49-01 를 위해.
                            dMinDist = 1;

                            Point3D oHpos = A.Element.GetPos(rCref, DbAttributeInstance.HPOS);
                            Point3D oTpos = A.Element.GetPos(rCref, DbAttributeInstance.TPOS);
                            string sArriveOrLeave = string.Empty;
                            DbElement rConnPart = DbElement.GetElement();

                            if (oCPos.DistanceToPoint(oHpos) < dMinDist)
                            {
                                //dmkim 170329 ATTA 제외.
                                rConnPart = A.PipeCollect.GetFirstMember(rCref, true);
                                sArriveOrLeave = "ARRIVE";
                            }
                            else
                            {
                                //dmkim 170329 ATTA 제외.
                                rConnPart = A.PipeCollect.GetLastMember(rCref, true);
                                sArriveOrLeave = "LEAVE";
                            }
                            olrConnPart.Add(rConnPart);
                            olArriveOrLeave.Add(sArriveOrLeave);
                            olConnIndex.Add(iIndex);

                        }
                        bRtn = true;
                    }
                }




                return bRtn;
            }

            /// <summary>
            /// Cref Point ;Arrive, Leave가 아닌 포인트중에서 가장 작은값, (0제외)
            /// </summary>
            /// <param name="rPart"></param>
            /// <returns></returns>
            public static int GetCRefIndexNo(DbElement rPart)
            {
                int iConnIndex = -1;

                List<int> olPPVI = ATT.PPVI(rPart);



                int iArrive = 1;
                int iLeave = 2;

                //TUBI 일경우 없음.
                try
                {
                    iArrive = ATT.ARRI(rPart);
                    iLeave = ATT.LEAV(rPart);
                }
                catch
                {
                }

                //dmkim 190219 PPVI가 순서대로 안나올수도 있다.
                foreach (int iIndex in olPPVI)
                {
                    //dmkim 190329
                    if (iIndex == 0) { continue; }
                    if (iIndex != iArrive && iIndex != iLeave)
                    {
                        if (iConnIndex == -1) { iConnIndex = iIndex; }
                        if (iConnIndex > iIndex) { iConnIndex = iIndex; }
                    }
                }
                return iConnIndex;
            }

            //dmkim 190211
            /// <summary>
            /// CRFA에서 가까운 Index 추출하기.
            /// </summary>
            /// <param name="rPart"></param>
            /// <returns></returns>
            public static int GetCrfaIndexNo(DbElement rPart, DbElement rCref)
            {
                int iConnIndex = -1;

                List<int> olPPVI = ATT.PPVI(rPart);



                int iArrive = 1;
                int iLeave = 2;

                //TUBI 일경우 없음.
                try
                {
                    iArrive = ATT.ARRI(rPart);
                    iLeave = ATT.LEAV(rPart);
                }
                catch
                {
                }

                DbElement rFindPart = DbElement.GetElement();
                if (rCref.GetElementType().Equals(DbElementTypeInstance.BRANCH))
                {
                    //HREF이면 첫번째 모델의 첫 정보.
                    //TREF이면 마지막 모델의 마지막 정보.
                    DbElement rHref = ATT.HREF(rCref);
                    if (A.IsValidElement(rHref))
                    {
                        if (rHref == rPart)
                        {
                            rFindPart = A.PipeCollect.GetFirstMember(rCref);
                        }
                    }
                    DbElement rTref = ATT.TREF(rCref);
                    if (A.IsValidElement(rTref))
                    {
                        if (rTref == rPart)
                        {
                            rFindPart = A.PipeCollect.GetFirstMember(rCref);
                        }
                    }
                }
                else if (A.PipeCollect.GetBranMemberType().Contains(ATT.TYPE(rCref)))
                {
                    //Component일 경우.
                    //string sType = ATT.TYPE(rHref);
                    rFindPart = rCref;
                }


                List<Point3D> olConnPos = new List<Point3D> { };
                if (A.IsValidElement(rFindPart))
                {
                    olConnPos.Add(A.Element.GetPos(rFindPart, DbAttributeInstance.APOS));
                    olConnPos.Add(A.Element.GetPos(rFindPart, DbAttributeInstance.LPOS));
                    if (rFindPart.GetElementType().Equals(DbElementTypeInstance.TEE) || rFindPart.GetElementType().Equals(DbElementTypeInstance.OLET))
                    {
                        try
                        {
                            olConnPos.Add(A.Element.GetPos(rFindPart, DbAttributeInstance.PPOS, 3));
                        }
                        catch { }
                    }
                    //dmkim 190212
                    else if (rFindPart.GetElementType().Equals(DbElementTypeInstance.CROSS))
                    {
                        try
                        {
                            olConnPos.Add(A.Element.GetPos(rFindPart, DbAttributeInstance.PPOS, 3));
                            olConnPos.Add(A.Element.GetPos(rFindPart, DbAttributeInstance.PPOS, 4));
                        }
                        catch { }
                    }
                }


                foreach (int iIndex in olPPVI)
                {
                    if (iIndex != iArrive && iIndex != iLeave)
                    {

                        Point3D oPos = A.Element.GetPos(rPart, DbAttributeInstance.PPOS, iIndex);

                        foreach (Point3D oConnPos in olConnPos)
                        {
                            if (oConnPos.DistanceToPoint(oPos) < D.dMinGapPartConn)
                            {
                                iConnIndex = iIndex;
                                break;
                            }
                        }

                        if (iConnIndex != -1) { break; }

                    }
                }
                return iConnIndex;
            }


            /// <summary>
            /// Bran의 연결된 Href/Tref, Fitting에 연결된 Cref 
            /// </summary>
            /// <remarks> SYSTEM ISO에서 연결된 PIPE 찾기 위해서.</remarks>
            /// <param name="rPart"></param>
            /// <param name="rContainPipe">여기에 속해있으면 Null로 </param>
            /// <returns></returns>
            public static List<DbElement> GetConnElement(DbElement rPart, DbElement rContainPipe)
            {
                List<DbElement> olrRtn = new List<DbElement> { };

                List<DbElement> olrTemp = new List<DbElement> { };

                //HREF, TREF
                if (rPart.GetElementType() == DbElementTypeInstance.BRANCH)
                {
                    DbElement rHref = ATT.HREF(rPart);
                    if (A.IsValidElement(rHref)) { olrTemp.Add(rHref); }

                    DbElement rTref = ATT.TREF(rPart);
                    if (A.IsValidElement(rTref)) { olrTemp.Add(rTref); }
                }
                //CREF
                else
                {
                    DbElement rCref = ATT.CREF(rPart);
                    if (A.IsValidElement(rCref)) { olrTemp.Add(rCref); }
                }

                if (olrTemp.Count > 0)
                {
                    if (A.IsValidElement(rContainPipe))
                    {

                    }
                }

                return olrRtn;
            }
            public static void GetSplnDicFromPipe(DbElement rPipe, ref SortedDictionary<string, List<DbElement>> dicRtn)
            {

                foreach (DbElement rPart in A.PipeCollect.GetBranchMemberFromPipe(rPipe, true))
                {
                    string sType = ATT.TYPE(rPart);

                    if (sType == "TUBI") continue;


                    bool bSuccess = true;
                    bool SHOP = ATT.SHOP(rPart);
                    if (bSuccess == false) continue;



                    #region if (sType == "BRAN")
                    if (sType == "BRAN")
                    {
                        string SPLH = ATT.SPLH(rPart);
                        if (SPLH == "unset" || SPLH == "") continue;

                        DbElement rTubi = A.PipeCollect.getileaveTube(rPart, ref bSuccess);
                        if (bSuccess == false) continue;


                        A.PipeCollect.MakeSPLNDictionary(ref dicRtn, SPLH, rTubi);
                        continue;
                    }
                    #endregion

                    string SPLN = ATT.SPLN(rPart);
                    string SPLT = ATT.SPLT(rPart);

                    if (SHOP)
                    {
                        if (SPLN != "unset" && SPLN != "")
                        {
                            A.PipeCollect.MakeSPLNDictionary(ref dicRtn, SPLN, rPart);
                        }
                    }


                    if (SPLT != "unset" && SPLT != "")
                    {
                        bSuccess = true;
                        DbElement rTubi = A.PipeCollect.getileaveTube(rPart, ref bSuccess);
                        if (bSuccess == false) continue;


                        A.PipeCollect.MakeSPLNDictionary(ref dicRtn, SPLT, rTubi);
                        continue;
                    }

                }

            }

            public static void GetSplnDicFromPart(DbElement rPart, ref SortedDictionary<string, List<DbElement>> dicRtn)
            {


                string sType = ATT.TYPE(rPart);

                if (sType == "TUBI") return;


                bool bSuccess = true;
                bool SHOP = ATT.SHOP(rPart);
                if (bSuccess == false) return;



                #region if (sType == "BRAN")
                if (sType == "BRAN")
                {
                    string SPLH = ATT.SPLH(rPart);
                    if (SPLH == "unset" || SPLH == "") return;

                    DbElement rTubi = A.PipeCollect.getileaveTube(rPart, ref bSuccess);
                    if (bSuccess == false) return;


                    A.PipeCollect.MakeSPLNDictionary(ref dicRtn, SPLH, rTubi);
                    return;
                }
                #endregion

                string SPLN = ATT.SPLN(rPart);
                string SPLT = ATT.SPLT(rPart);

                if (SHOP)
                {
                    if (SPLN != "unset" && SPLN != "")
                    {
                        A.PipeCollect.MakeSPLNDictionary(ref dicRtn, SPLN, rPart);
                    }
                }


                if (SPLT != "unset" && SPLT != "")
                {
                    bSuccess = true;
                    DbElement rTubi = A.PipeCollect.getileaveTube(rPart, ref bSuccess);
                    if (bSuccess == false) return;


                    A.PipeCollect.MakeSPLNDictionary(ref dicRtn, SPLT, rTubi);
                }
            }

            public static void GetSplnMemberFromPart(DbElement rPart, ref string sSpln, ref List<DbElement> olrPart)
            {
                sSpln = ATT.SPLN(rPart);

                DbElement rPipe = rPart.Owner.Owner;
                SortedDictionary<string, List<DbElement>> dicRtn = new SortedDictionary<string, List<DbElement>> { };

                foreach (DbElement rAttaPart in A.PipeCollect.GetBranchMemberFromPipe(rPipe, true))
                {
                    string sType = ATT.TYPE(rAttaPart);

                    if (sType == "TUBI") continue;


                    bool bSuccess = true;
                    bool SHOP = ATT.SHOP(rAttaPart);
                    if (bSuccess == false) continue;



                    #region if (sType == "BRAN")
                    if (sType == "BRAN")
                    {
                        string SPLH = ATT.SPLH(rAttaPart);
                        if (SPLH == "unset" || SPLH == "") continue;

                        DbElement rTubi = A.PipeCollect.getileaveTube(rAttaPart, ref bSuccess);
                        if (bSuccess == false) continue;


                        A.PipeCollect.MakeSPLNDictionary(ref dicRtn, SPLH, rTubi);
                        continue;
                    }
                    #endregion

                    string SPLN = ATT.SPLN(rAttaPart);
                    string SPLT = ATT.SPLT(rAttaPart);

                    if (SHOP)
                    {
                        if (SPLN != "unset" && SPLN != "")
                        {
                            A.PipeCollect.MakeSPLNDictionary(ref dicRtn, SPLN, rAttaPart);
                        }
                    }


                    if (SPLT != "unset" && SPLT != "")
                    {
                        bSuccess = true;
                        DbElement rTubi = A.PipeCollect.getileaveTube(rAttaPart, ref bSuccess);
                        if (bSuccess == false) continue;


                        A.PipeCollect.MakeSPLNDictionary(ref dicRtn, SPLT, rTubi);
                        continue;
                    }

                }

                if (dicRtn.ContainsKey(sSpln))
                {
                    olrPart = dicRtn[sSpln];
                }

            }
            public static void MakeSPLNDictionary(ref SortedDictionary<string, List<DbElement>> dicRtn, string sKey, DbElement rPart)
            {
                if (dicRtn.Count == 0)
                {
                    dicRtn.Add(sKey, new List<DbElement> { rPart });
                }
                else
                {
                    if (dicRtn.ContainsKey(sKey))
                    {
                        List<DbElement> olTmp = dicRtn[sKey];
                        if (olTmp.Contains(rPart) == false)
                        {
                            olTmp.Add(rPart);
                            dicRtn[sKey] = olTmp;
                        }
                    }
                    else
                    {
                        dicRtn.Add(sKey, new List<DbElement> { rPart });
                    }
                }
            }


            /// <summary>
            /// Graphic 3D View에서 선택된 파이프를 가져온다. / 모든Part.
            /// </summary>
            /// <param name="bIsSpool">True 이면 스풀을 가져옴.</param>
            /// <returns></returns>
            public static List<DbElement> GetPipeGraphicSelection(bool bIsSpool = false)
            {
                List<DbElement> olrPipe = new List<DbElement> { };

                if (SearchManager.PopulationSource("Graphical Selection").Elements.Count != 0)
                {
                    foreach (DbElement rElement in SearchManager.PopulationSource("Graphical Selection").Elements)
                    {
                        //PIPE
                        if (bIsSpool == false)
                        {
                            A.PipeCollect.AddPipeFromPart(ref olrPipe, rElement);
                        }
                        else
                        {
                            olrPipe.Add(rElement);
                        }
                    }
                }

                return olrPipe;
            }


            /// <summary>
            /// Graphic 3D View에서 모든 파이프를 가져온다.
            /// </summary>
            /// <returns></returns>
            public static List<DbElement> GetPipeGraphicDrawlist()
            {
                List<DbElement> olrPipe = new List<DbElement> { };

                if (SearchManager.PopulationSource("Draw List").Elements.Count != 0)
                {
                    foreach (DbElement rElement in SearchManager.PopulationSource("Draw List").Elements)
                    {
                        A.PipeCollect.AddPipeFromPart(ref olrPipe, rElement);
                    }
                }

                return olrPipe;
            }

            /// <summary>
            /// 주어진 part/bran/pipe로 부터 파이프 이름 가져옴(중복이면 추가 안함.)
            /// </summary>
            /// <param name="olrPipe"></param>
            private static void AddPipeFromPart(ref List<DbElement> olrPipe, DbElement rPart)
            {
                string sType = ATT.TYPE(rPart);

                DbElement rPipe = DbElement.GetElement();

                //Bran Member Type
                if (A.PipeCollect.GetBranMemberType().Contains(sType))
                {
                    rPipe = rPart.Owner.Owner;
                }
                else if (sType == "BRAN")
                {
                    rPipe = rPart.Owner;
                }
                else if (sType == "PIPE")
                {
                    rPipe = rPart;
                }
                else
                {
                    INFOGET_ZERO_HULL.Display.WriteLine("None");
                }

                if (A.IsValidElement(rPipe))
                {
                    if (olrPipe.Contains(rPipe) == false)
                    {
                        olrPipe.Add(rPipe);
                    }
                }

            }

            /// <summary>
            /// 주어진 Data Set의 Member인 Data의 DTitle(또는 DKey)로 부터 찾을텍스트를 비교해서 Number를 넘김.
            /// 1부터 반환하므로 Para 0에서 1빼주는 작업해야함.
            /// </summary>
            /// <param name="rDtref"></param>
            /// <param name="olFindText"></param>
            /// <param name="bDTitle">True = DTitle, False = DKEY</param>
            /// <returns></returns>
            public static int GetParaIndexFromDataSet(DbElement rDtref, List<string> olFindText, bool bDTitle = true)
            {
                int iRtn = -1;

                if (A.IsValidElement(rDtref))
                {

                    List<DbElement> olrData = A.GetCollect(rDtref, DbElementTypeInstance.DATA);

                    foreach (DbElement rText in olrData)
                    {
                        string sDataTitle = ATT.DTIT(rText);
                        if (bDTitle == false)
                        {
                            sDataTitle = ATT.DKEY(rText);
                        }

                        bool bFind = false;
                        foreach (string sFindText in olFindText)
                        {
                            if (sDataTitle.ToUpper().Contains(sFindText))
                            {
                                iRtn = ATT.NUMB(rText);
                                bFind = true;
                                break;
                            }
                        }
                        if (bFind) { break; }


                    }

                }

                return iRtn;
            }

            //dmkim 180724
            //dmkim 190220
            /// <summary>
            /// PCOM의 P3Pos이 다른 브랜치와 연결되어있는지 체크.
            /// </summary>
            /// <param name="rPart"></param>
            /// <param name="rConnRef">연결된 DBElement</param>
            /// <returns></returns>
            public static bool isConnPCOM(DbElement rPart, ref DbElement rConnRef)
            {
                bool bRtn = false;

                if (ATT.PPVI(rPart).Contains(3) == false)
                {
                    return bRtn;
                }

                Point3D oPoint3D = A.Element.GetPos(rPart, DbAttributeInstance.PPOS, 3);

                // 해당하는 모든 Branch의 HREF/TREF가 해당 PCOM을 가리키고, H/TPOS가 PCOM의 P3와 동일하고 H/TDIR이 P3DIR과 같고

                string sPipe = A.PipeCollect.GetPipeNameFromPart(rPart);
                DbElement rPipe = DbElement.GetElement(sPipe);

                if (A.IsValidElement(rPipe))
                {
                    DbElement[] olrBran = rPipe.Members(DbElementTypeInstance.BRANCH);
                    foreach (DbElement rBran in olrBran)
                    {
                        DbElement rHref = ATT.HREF(rBran);
                        DbElement rTref = ATT.TREF(rBran);
                        if (A.IsValidElement(rHref))
                        {
                            if (rHref == rPart)
                            {
                                //HPOS = P3POS
                                Point3D oHpos = A.Element.GetPos(rBran, DbAttributeInstance.HPOS);
                                if (oHpos.DistanceToPoint(oPoint3D) < D.dMinNewGapPartConn)
                                {
                                    rConnRef = rHref;
                                    bRtn = true;
                                    break;
                                }
                            }
                        }
                        if (A.IsValidElement(rTref))
                        {
                            if (rTref == rPart)
                            {
                                //TPOS = P3POS
                                Point3D oTpos = A.Element.GetPos(rBran, DbAttributeInstance.HPOS);
                                if (oTpos.DistanceToPoint(oPoint3D) < D.dMinNewGapPartConn)
                                {
                                    rConnRef = rTref;
                                    bRtn = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                return bRtn;
            }

            //dmkim 190520
            /// <summary>
            /// FLAN의 P3Pos이 다른 브랜치와 연결되어있는지 체크.
            /// </summary>
            /// <param name="rPart"></param>
            /// <returns></returns>
            public static bool isConnFLAN(DbElement rPart)
            {
                bool bRtn = false;

                if (ATT.PPVI(rPart).Contains(3) == false)
                {
                    return bRtn;
                }

                List<string> olOrificeFlanCompSkey = new List<string> { "FOSO", "FOWN", "FLRE" };

                if (olOrificeFlanCompSkey.Contains(ATT.SKEY(rPart)) == false) { return bRtn; }



                Point3D oPoint3D = A.Element.GetPos(rPart, DbAttributeInstance.PPOS, 3);
                DbElement rCref = ATT.CREF(rPart);

                if (rCref.GetElementType().Equals(DbElementTypeInstance.BRANCH))
                {
                    DbElement rHref = ATT.HREF(rCref);
                    DbElement rTref = ATT.TREF(rCref);
                    if (A.IsValidElement(rHref))
                    {
                        if (rHref == rPart) { bRtn = true; return bRtn; }
                    }
                    if (A.IsValidElement(rTref))
                    {
                        if (rTref == rPart) { bRtn = true; return bRtn; }
                    }
                }

                return bRtn;
            }


            //dmkim 180828
            /// <summary>
            /// Break Atta인지 체크.
            /// </summary>
            /// <param name="rPipeOrAtta"></param>
            /// <param name="olrAtta"></param>
            /// <returns></returns>
            public static bool isBreakAtta(DbElement rPipeOrAtta, ref List<DbElement> olrAtta)
            {
                bool bRtn = false;

                List<DbElement> olrTempAtta = new List<DbElement> { };
                if (rPipeOrAtta.GetElementType().Equals(DbElementTypeInstance.PIPE))
                {
                    List<DbElement> olrPart = A.PipeCollect.GetBranchMemberFromPipe(rPipeOrAtta);

                    foreach (DbElement rPart in olrPart)
                    {
                        if (rPart.GetElementType().Equals(DbElementTypeInstance.ATTACHMENT))
                        {
                            olrTempAtta.Add(rPart);
                        }
                    }
                }
                else if (rPipeOrAtta.GetElementType().Equals(DbElementTypeInstance.ATTACHMENT))
                {
                    olrTempAtta.Add(rPipeOrAtta);
                }

                foreach (DbElement rPart in olrTempAtta)
                {
                    if (ATT.ATTY(rPart) == "XXXX")
                    {
                        bRtn = true; olrAtta.Add(rPart);
                    }
                }

                return bRtn;
            }

            //dmkim 190219
            /// <summary>
            /// CROSS 타입인지 체크한다. 커넥션이 4개.
            /// </summary>
            /// <param name="rPart"></param>
            /// <returns></returns>
            public static bool isCrossType(DbElement rPart)
            {
                bool bRtn = false;

                if (rPart.GetElementType().Equals(DbElementTypeInstance.CROSS))
                {
                    bRtn = true;
                }
                else if (rPart.GetElementType().Equals(DbElementTypeInstance.INSTRUMENT))
                {
                    List<DbElement> olrCref = ATT.CRFA(rPart);
                    if (olrCref.Count >= 2)
                    {
                        bRtn = true;
                    }
                }
                ////dmkim 190313
                //else if (rPart.GetElementType().Equals(DbElementTypeInstance.VFWAY))
                //{
                //    bRtn = true;
                //}

                return bRtn;
            }

            /// <summary>
            /// ATTA가 걸처져있는 PIPE를 찾는다.
            /// </summary>
            /// <param name="rAtta"></param>
            /// <param name="rFindTube"></param>
            /// <returns></returns>
            public static bool FindConnectPipe(DbElement rAtta, ref DbElement rFindTube)
            {
                bool bRtn = false;

                Point3D oPosAtta = A.Element.GetPos(rAtta, DbAttributeInstance.APOS);

                foreach (DbElement rPart in rAtta.Owner.Members(DbElementTypeInstance.TUBING))
                {
                    double dAbore = A.Element.GetBore(rPart, DbAttributeInstance.ABOR);

                    Point3D oApos = A.Element.GetPos(rPart, DbAttributeInstance.APOS);
                    Point3D oLpos = A.Element.GetPos(rPart, DbAttributeInstance.LPOS);

                    //dmkim 181214
                    if (oPosAtta.DistanceToPoint(oApos) < D.dMinNewGapPartConn ||
                        (oPosAtta.DistanceToPoint(oLpos) < D.dMinNewGapPartConn))
                    {
                        continue;
                    }

                    Line3D oLine3D = new Line3D(oApos, oLpos);

                    int iResult = -1;
                    double dDist = 0.0;
                    Point3D oPoint = oLine3D.DistanceFromPoint3D(oPosAtta, ref iResult, ref dDist);

                    //dmkim 181214
                    //if (dDist < dAbore / 2.0 + 10)
                    if (dDist < D.dMinNewGapPartConn)
                    {
                        bRtn = true;
                        rFindTube = rPart;
                        break;
                    }
                }

                return bRtn;
            }


            /// <summary>
            /// rConn의 Cref에 rPart가 포함되어있는지 확인.
            /// </summary>
            /// <param name="rPart"></param>
            /// <param name="rConn"></param>
            /// <returns></returns>
            public static bool IsConnMember(DbElement rPart, DbElement rConn)
            {
                bool bRtn = false;

                DbElement rCref = ATT.CREF(rConn);
                if (A.IsValidElement(rCref))
                {
                    if (rCref.GetElementType().Equals(DbElementTypeInstance.BRANCH))
                    {
                        if (A.PipeCollect.GetBranchMemberFromBranch(rCref).Contains(rPart)) { bRtn = true; }
                    }
                    else if (A.PipeCollect.GetBranMemberType().Contains(ATT.TYPE(rCref)))
                    {
                        if (rCref == rPart) { bRtn = true; }
                    }
                }

                return bRtn;
            }
        }

        /// <summary>
        /// 도면 관련한 Collect 모음.
        /// </summary>
        public static class DraftCollect
        {

            /// <summary>
            /// 현재 DRAFT상의 CE를 가져옴 REGI, 하위일 경우 상위이 REGI를 가져옴.
            /// </summary>
            /// <param name="sMsg"></param>
            /// <returns></returns>
            public static string GetRegiDraftCE(ref string sMsg)
            {
                string sRtn = string.Empty;

                DbElement rCE = CurrentElement.Element;

                //dmkim 171027 초기선택안할시 NulRef (<DBREF> =0/0 - Unset)
                bool bOK = true;
                if (A.IsValidElement(rCE) == false) { bOK = false; }
                if (rCE.DbType != Aveva.Pdms.Database.DbType.Draft) { bOK = false; }
                if (bOK == false)
                {
                    sMsg = LogMsg.UI.U09;
                    if (A.Display.DraftExplorer_IsShown() == false)
                    {
                        sMsg = sMsg + LogMsg.UI.U10;
                    }
                    return sRtn;
                }

                if (rCE.GetElementType() == DbElementTypeInstance.REGISTRY)
                {
                    sRtn = ATT.FLNM(rCE);
                }
                else
                {
                    var regi = ATT.AHLIST(rCE).FirstOrDefault(pr => pr == "REGI");
                    if (regi == null)
                    {
                        sMsg = LogMsg.UI.U09;
                        return sRtn;
                    }
                    sRtn = ATT.FLNM(rCE.EvaluateElement(DbExpression.Parse("REGI")));
                }
                return sRtn;
            }


            /// <summary>
            /// 현재 MDB에 있는 모든 DEPT 정보를 가져온다. (DbElement)
            /// </summary>
            /// <returns></returns>
            public static List<DbElement> GetDeptFromCurrentMDB()
            {
                List<DbElement> olrRtn = new List<DbElement> { };


                MDB oMDB = MDB.CurrentMDB;
                Db[] olDB = oMDB.GetDBArray(DbType.Draft);

                //System.Diagnostics.Debugger.Launch();
                foreach (Db oDb in olDB)
                {
                    try
                    {
                        DbElement rDB = DbElement.GetElement("*" + oDb.Name);
                        string sForign = ATT.FOREDB(rDB); //FOREIGN, LOCAL

                        if (sForign == "LOCAL")
                        {
                            olrRtn.AddRange(A.GetCollect(oDb.World, DbElementTypeInstance.DEPT));
                        }
                    }
                    catch
                    {
                    }
                }

                return olrRtn;

            }

            public static string GetRegiFromDwgName(string sDwgName)
            {
                string sRegi = string.Empty;
                //dmkim 180302
                DbElement rShee = DbElement.GetElement("/" + sDwgName);
                if (A.IsValidElement(rShee))
                {
                    sRegi = ATT.FLNM(rShee.Owner.Owner);
                }
                return sRegi;

            }
            //dmkim 180423
            public static DbElement GetLibyFromShee(DbElement rShee)
            {
                DbElement rRtn = DbElement.GetElement();

                DbElement[] olrLiby = rShee.Owner.Members(DbElementTypeInstance.LIBY);

                if (olrLiby.Length > 0)
                {
                    rRtn = olrLiby[0];
                }
                return rRtn;
            }

        }

        /// <summary>
        /// AM 화면 관련 
        /// </summary>
        public static class Display
        {
            #region AM UI Show/Hide
            public static bool DraftExplorer_IsShown()
            {
                foreach (IWindow window in WindowManager.Instance.Windows)
                {
                    if (window.Key.Equals("DraftExplorer"))
                    {
                        return window.Visible;
                    }
                }

                return false;
            }
            public static void DraftExplorer_Show()
            {
                foreach (IWindow window in WindowManager.Instance.Windows)
                {
                    if (window.Key.Equals("DraftExplorer"))
                    {
                        window.Show();
                        break;
                    }
                }
            }

            public static void DraftExplorer_Hide()
            {
                foreach (IWindow window in WindowManager.Instance.Windows)
                {
                    if (window.Key.Equals("DraftExplorer"))
                    {
                        window.Hide();
                        break;
                    }
                }
            }
            public static bool CommandWindow_IsShown()
            {
                foreach (IWindow window in WindowManager.Instance.Windows)
                {
                    if (window.Key.Equals("!!CADCBTH"))
                    {
                        return window.Visible;
                    }
                }

                return false;
            }
            public static void CommandWindow_Show()
            {
                foreach (IWindow window in WindowManager.Instance.Windows)
                {
                    if (window.Key.Equals("!!CADCBTH"))
                    {
                        window.Show();
                        break;
                    }
                }
            }
            public static void CommandWindow_Hide()
            {
                foreach (IWindow window in WindowManager.Instance.Windows)
                {
                    if (window.Key.Equals("!!CADCBTH"))
                    {
                        window.Hide();
                        break;
                    }
                }
            }

            public static bool MD_3DView_IsShown()
            {
                foreach (IWindow window in WindowManager.Instance.Windows)
                {
                    if (window.Key.Equals("!!DRA3DVIEW"))
                    {
                        return window.Visible;
                    }
                }
                return false;
            }
            public static void MD_3DView_Show()
            {
                A.Command.CommandForPDMS("!!draShowView3dinDraft()");
            }
            public static void MD_3DView_Hide()
            {
                A.Command.CommandForPDMS("kill !!DRA3DVIEW");
            }

            public static void MD_3DView_Add(string sPipe)
            {
                CommandManager.Instance.Commands["AVEVA.Marine.UI.Menu.GeneralRemoveFrom3DView"].Execute();
                //AvevaUtil.CommandForPDMS(string.Format("$P 11111"));
                //AvevaUtil.CommandForPDMS(string.Format("!!DRA3DVIEW.viewRem()"));

                //AvevaUtil.CommandForPDMS(string.Format("!olPipe = Array()"));
                //AvevaUtil.CommandForPDMS(string.Format("!olPipe.Add('{0}')", sPipe));
                //AvevaUtil.CommandForPDMS(string.Format("q var !olPipe"));
                //AvevaUtil.CommandForPDMS(string.Format("!!DRA3DVIEW.viewAdd(!olPipe)"));


                CommandManager.Instance.Commands["AVEVA.Marine.UI.Menu.GeneralAddTo3DView"].Execute();

                //if (AvevaUtil.IsValidElement(DbElement.GetElement(sPipe)))
                //{
                //    CurrentElement.Element = DbElement.GetElement(sPipe);


                //}
            }

            /// <summary>
            /// 3D View ADD
            /// </summary>
            /// <param name="sModel"></param>
            /// <param name="bRemoveAll"></param>
            public static void Run3DShow(string sModel, bool bRemoveAll)
            {
                CommandManager.Instance.Commands["AVEVA.View.WalkTo.DrawList"].Execute();

                if (bRemoveAll) { A.Command.CommandForPDMS("remove all"); }
                A.Command.CommandForPDMS(string.Format("add {0}", sModel));
            }
            #endregion
        }

        public static class Misc
        {
            /// <summary>
            /// 주어진 파트들로 부터 MIN,MAX값을 가져온다.
            /// </summary>
            /// <param name="olPart"></param>
            /// <param name="oMaxP3"></param>
            /// <param name="oMinP3"></param>
            public static void GetMinMax(List<string> olPart, ref Position oMaxP3, ref Position oMinP3)
            {
                oMaxP3 = Position.Create(-10e10, -10e10, -10e10);
                oMinP3 = Position.Create(10e10, 10e10, 10e10);


                foreach (string sPart in olPart)
                {
                    DbElement rPart = DbElement.GetElement(sPart);

                    double[] olData = ATT.WVOL(rPart);
                    Position oOtherMinP3 = Position.Create(olData[0], olData[1], olData[2]);
                    Position oOtherMaxP3 = Position.Create(olData[3], olData[4], olData[5]);

                    if (oOtherMinP3.X < oMinP3.X) oMinP3.X = oOtherMinP3.X;
                    if (oOtherMinP3.Y < oMinP3.Y) oMinP3.Y = oOtherMinP3.Y;
                    if (oOtherMinP3.Z < oMinP3.Z) oMinP3.Z = oOtherMinP3.Z;

                    if (oOtherMaxP3.X > oMaxP3.X) oMaxP3.X = oOtherMaxP3.X;
                    if (oOtherMaxP3.Y > oMaxP3.Y) oMaxP3.Y = oOtherMaxP3.Y;
                    if (oOtherMaxP3.Z > oMaxP3.Z) oMaxP3.Z = oOtherMaxP3.Z;
                }
            }
        }
        /// <summary>
        /// AM 고유 ProcessBar 우측하단.
        /// </summary>
        public static class FMSYS
        {
            public static void SetProgress(int iPersentage, string sMessage)
            {
                A.Command.CommandForPDMS(string.Format("!!fmsys.setprogresstext('{0}')", sMessage));
                A.Command.CommandForPDMS(string.Format("!!fmsys.setprogress({0})", iPersentage));
            }
            public static void Clear()
            {
                A.Command.CommandForPDMS("!!fmsys.setprogress(0)");
                A.Command.CommandForPDMS("!!fmsys.setprogresstext('')");
            }
        }

        public static class Aid
        {
            public static void AidClearAll()
            {
                A.Command.CommandForPDMS("AID CLEAR ALL");
            }
            public static void AidLine(double dMinX, double dMinY, double dMinZ, double dMaxX, double dMaxY, double dMaxZ)
            {
                string sCmd = string.Format("AID LINE X {0} Y {1} Z {2} to  X {3} Y {4} Z {5}", dMinX, dMinY, dMinZ, dMaxX, dMaxY, dMaxZ);
                A.Command.CommandForPDMS(sCmd);
            }
            public static void AidLine(Position oMinPos, Position oMaxPos)
            {
                AidLine(oMinPos.X, oMinPos.Y, oMinPos.Z, oMaxPos.X, oMaxPos.Y, oMaxPos.Z);
            }

            public static void AidBox(Position oMidPos, double dXLen, double dYLen, double dZLen)
            {
                string sCmd = string.Format("AID BOX POS X {0} Y {1} Z {2} XLEN {3} YLEN {4} ZLEN {5}", oMidPos.X, oMidPos.Y, oMidPos.Z, dXLen, dYLen, dZLen);
                A.Command.CommandForPDMS(sCmd);
            }

            public static void AidBox(LimitsBox oBox)
            {
                AidBox(oBox.Centre(), Math.Abs(oBox.Maximum.X - oBox.Minimum.X), Math.Abs(oBox.Maximum.Y - oBox.Minimum.Y), Math.Abs(oBox.Maximum.Z - oBox.Minimum.Z));
            }
            public static void AidText(string text, Position point)
            {
                string sCmd = string.Format("AID TEXT '{0}' AT X {1} Y {2} Z {3}",text,point.X,point.Y,point.Z);
                A.Command.CommandForPDMS(sCmd);
            }
        }
        public static class View3D
        {
            static DrawListMember[] temp = new DrawListMember[] { };
            public static void ADD(string sPart)
            {
                A.Command.CommandForPDMS(string.Format("ADD {0}", sPart));
            }
            public static void ADD(DbElement rPart)
            {
                View3D.ADD(A.Element.Flnm(rPart));
            }
            public static void ADD(List<DbElement> olrPart)
            {
                foreach (DbElement rPart in olrPart)
                {
                    View3D.ADD(rPart);
                }
            }

            public static void REM(string sPart)
            {
                A.Command.CommandForPDMS(string.Format("REM {0}", sPart));
            }
            public static void REM(DbElement rPart)
            {
                View3D.REM(A.Element.Flnm(rPart));
            }
            public static void REM(List<DbElement> olrPart)
            {
                foreach (DbElement rPart in olrPart)
                {
                    View3D.REM(rPart);
                }
            }
            public static void REMALL()
            {
                A.Command.CommandForPDMS(string.Format("REM ALL"));
            }
            /// <summary>
            /// 투명도 조절
            /// </summary>
            /// <param name="Value"></param>
            public static void Translucency(int Value)
            {
                DrawList viewDrawList = DrawListManager.Instance.CurrentDrawList;
                DrawListMember[] drawListMemList = viewDrawList.Members();
                for(int i =0; i<drawListMemList.Length; i++)
                {
                    DrawListMember drmem = drawListMemList[i];
                    DbElement volRef = drmem.DbElement;

                    drmem.Transparency = Value;
                }
                //A.Command.CommandForPDMS("!currentView = !!fmsys.currentdocument().view");
                //A.Command.CommandForPDMS("!drawlist = !!gphdrawlists.drawlist(!!gphdrawlists.token(!currentView))");
                //A.Command.CommandForPDMS(string.Format("!drawlist.TRANSLUCENCY(!drawlist.members(), {0})",Value));
            }
            /// <summary>
            /// 색상 조절
            /// </summary>
            /// <param name="Value"></param>
            public static void Colour(int Value)
            {
                DrawList viewDrawList = DrawListManager.Instance.CurrentDrawList;
                DrawListMember[] drawListMemList = viewDrawList.Members();
                for (int i = 0; i < drawListMemList.Length; i++)
                {
                    DrawListMember drmem = drawListMemList[i];
                    DbElement volRef = drmem.DbElement;

                    drmem.Colour = Value;
                }
                //A.Command.CommandForPDMS("!currentView = !!fmsys.currentdocument().view");
                //A.Command.CommandForPDMS("!drawlist = !!gphdrawlists.drawlist(!!gphdrawlists.token(!currentView))");
                //A.Command.CommandForPDMS(string.Format("!drawlist.TRANSLUCENCY(!drawlist.members(), {0})",Value));
            }
            /// <summary>
            /// 색상 조절
            /// </summary>
            /// <param name="Value"></param>
            public static void LastColour(int Value)
            {
                DrawList viewDrawList = DrawListManager.Instance.CurrentDrawList;
                DrawListMember[] drawListMemList = viewDrawList.Members();
                DrawListMember drmem = drawListMemList.First();
                if (temp.Length != 0)
                {
                    foreach(var member in drawListMemList)
                    {
                        if(!temp.Contains(member))
                        {
                            drmem = member;
                        }
                    }
                }
                DbElement volRef = drmem.DbElement;
                A.Command.PrintInCommandWindow(volRef.ToString());
                drmem.Colour = Value;
                A.Command.PrintInCommandWindow(Value.ToString());

                temp = drawListMemList;

                //A.Command.CommandForPDMS("!currentView = !!fmsys.currentdocument().view");
                //A.Command.CommandForPDMS("!drawlist = !!gphdrawlists.drawlist(!!gphdrawlists.token(!currentView))");
                //A.Command.CommandForPDMS(string.Format("!drawlist.TRANSLUCENCY(!drawlist.members(), {0})",Value));
            }
            public static List<string> GetElementOf3DViewSelected()
            {
                List<string> elementof3dview = new List<string>();
                A.Command.PrintInCommandWindow("#1");
                foreach(DictionaryEntry ele in SearchManager.PopulationSource("Graphical Selection").Elements)
                {
                    elementof3dview.Add(ele.Value.ToString());
                }
                A.Command.PrintInCommandWindow("#2");
                return elementof3dview;
            }
        }

        public static class Calculate
        {
            public static LimitsBox getBox(LimitsBox A, LimitsBox B, double factor)
            {
                Position Tmax = A.Maximum;
                Position Tmin = A.Minimum;

                Position Pmax = B.Maximum;
                Position Pmin = B.Minimum;

                Position minvv = Position.Create();
                Position maxvv = Position.Create();

                //minX
                if (Tmin.X - factor < Pmin.X)
                {
                    minvv.X = Pmin.X;
                }
                else
                {
                    minvv.X = Tmin.X - factor;
                }
                //minY
                if (Tmin.Y - factor < Pmin.Y)
                {
                    minvv.Y = Pmin.Y;
                }
                else
                {
                    minvv.Y = Tmin.Y - factor;
                }
                //minZ
                if (Tmin.Z - factor < Pmin.Z)
                {
                    minvv.Z = Pmin.Z;
                }
                else
                {
                    minvv.Z = Tmin.Z - factor;
                }

                //maxX
                if (Tmax.X + factor > Pmax.X)
                {
                    maxvv.X = Pmax.X;
                }
                else
                {
                    maxvv.X = Tmax.X + factor;
                }
                //maxY
                if (Tmax.Y + factor > Pmax.Y)
                {
                    maxvv.Y = Pmax.Y;
                }
                else
                {
                    maxvv.Y = Tmax.Y + factor;
                }
                //maxZ
                if (Tmax.Z + factor > Pmax.Z)
                {
                    maxvv.Z = Pmax.Z;
                }
                else
                {
                    maxvv.Z = Tmax.Z + factor;
                }

                if ((maxvv.X - minvv.X) < 0 || (maxvv.Y - minvv.Y) < 0 || (maxvv.Z - minvv.Z < 0))
                {
                    LimitsBox returnBOX = LimitsBox.Create();

                    return returnBOX;
                }
                else
                {
                    LimitsBox returnBOX = LimitsBox.Create(minvv, maxvv);

                    return returnBOX;
                }
            }
        }
    }
}
