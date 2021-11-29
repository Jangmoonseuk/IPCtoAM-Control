using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aveva.Pdms.Clasher;
using Aveva.Pdms.Database;
using Aveva.Pdms.Geometry;

using A = INFOGET_ZERO_HULL.AM.AvevaUtil;

namespace INFOGET_ZERO_HULL.AM
{
    public static class AvevaClash
    {
        /// <summary>
        /// CLASH CHECK
        /// </summary>
        /// <param name="rCheck"></param>
        /// <param name="olObstruction"></param>
        /// <returns></returns>
        public static List<ClashData> Check(DbElement rCheck, List<DbElement> olObstruction)
        {
            List<ClashData> olRtn = new List<ClashData> { };

            //PMLBOJECT 실행.

            A.Command.CommandForPDMS(string.Format("!sCheckName = '{0}'", A.Element.Flnm(rCheck)));
            A.Command.CommandForPDMS(string.Format("!olObstruction = Array()"));
            foreach (DbElement rPart in olObstruction)
            {
                A.Command.CommandForPDMS(string.Format("!olObstruction.Append('{0}')", A.Element.Flnm(rPart)));
            }
            A.Command.CommandForPDMS(string.Format("!olRtn = !!IFGHullCheckClash(!sCheckName, !olObstruction)"));

            //A.Command.PrintInCommandWindow("START");
            //205개 리스트 약 6초정도 걸림.
            A.Command.CommandForPDMS("!!ZEROHULLCLASHSIZE.delete()");
            A.Command.CommandForPDMS("!!ZEROHULLCLASHTYPE.delete()");
            A.Command.CommandForPDMS("!!ZEROHULLCLASHDESC.delete()");
            A.Command.CommandForPDMS("!!ZEROHULLCLASHFIRS.delete()");
            A.Command.CommandForPDMS("!!ZEROHULLCLASHSECO.delete()");
            A.Command.CommandForPDMS("!!ZEROHULLCLASHPOSX.delete()");
            A.Command.CommandForPDMS("!!ZEROHULLCLASHPOSY.delete()");
            A.Command.CommandForPDMS("!!ZEROHULLCLASHPOSZ.delete()");


            A.Command.CommandForPDMS("!!ZEROHULLCLASHSIZE = !olRtn.Size()");
            double dSize = A.Command.GetPMLValueDouble("ZEROHULLCLASHSIZE");

            for (int i = 1; i <= dSize; i++)
            {
                A.Command.CommandForPDMS(string.Format("!!ZEROHULLCLASHTYPE = !olRtn[{0}][1]", i));
                A.Command.CommandForPDMS(string.Format("!!ZEROHULLCLASHDESC = !olRtn[{0}][2]", i));
                A.Command.CommandForPDMS(string.Format("!!ZEROHULLCLASHFIRS = !olRtn[{0}][3]", i));
                A.Command.CommandForPDMS(string.Format("!!ZEROHULLCLASHSECO = !olRtn[{0}][4]", i));
                A.Command.CommandForPDMS(string.Format("!ZEROHULLCLASHPOS = !olRtn[{0}][5]", i));
                A.Command.CommandForPDMS(string.Format("!ZEROHULLCLASHPOSObj = Object Position(!ZEROHULLCLASHPOS)"));
                A.Command.CommandForPDMS(string.Format("!!ZEROHULLCLASHPOSX = !ZEROHULLCLASHPOSObj.East"));
                A.Command.CommandForPDMS(string.Format("!!ZEROHULLCLASHPOSY = !ZEROHULLCLASHPOSObj.North"));
                A.Command.CommandForPDMS(string.Format("!!ZEROHULLCLASHPOSZ = !ZEROHULLCLASHPOSObj.Up"));

                string sClashType = A.Command.GetPMLValueString("ZEROHULLCLASHTYPE");
                string sClashDesc = A.Command.GetPMLValueString("ZEROHULLCLASHDESC");
                string sClashFirs = A.Command.GetPMLValueString("ZEROHULLCLASHFIRS");
                string sClashSend = A.Command.GetPMLValueString("ZEROHULLCLASHSECO");
                double dClashPosX = A.Command.GetPMLValueDouble("ZEROHULLCLASHPOSX");
                double dClashPosY = A.Command.GetPMLValueDouble("ZEROHULLCLASHPOSY");
                double dClashPosZ = A.Command.GetPMLValueDouble("ZEROHULLCLASHPOSZ");

                string sPos = string.Format("X : {0}, Y : {1}, Z : {2}", dClashPosX, dClashPosY, dClashPosZ);
                string sPrint = string.Format("TYPE : {0}, DESC : {1}, FIRST : {2}, SECOND : {3}, POS : {4}", sClashType, sClashDesc, sClashFirs, sClashSend, sPos);
                //Console.WriteLine(sPrint);

                ClashData oClashData = new ClashData(sClashFirs, sClashSend, sClashType, sClashDesc, dClashPosX, dClashPosY, dClashPosZ);
                olRtn.Add(oClashData);
            }
            //A.Command.PrintInCommandWindow("END");

            return olRtn;

        }

        private static void Test(DbElement rCheck, List<DbElement> olObstruction)
        {
            System.Diagnostics.Debugger.Launch();

            try
            {
                //Aveva.Pdms.Clasher.SpatialMap oSpatialMapInst = Aveva.Pdms.Clasher.SpatialMap.Instance;
                //oSpatialMapInst.BuildSpatialMap();

                Clasher oClasher = Clasher.Instance; //DESCLASH?

                ClashOptions oOption = ClashOptions.Create();

                //OVERRIDE ON
                //!clasher.overrideSet(TRUE)
               

                //MIDPOINT OFF
                oOption.Midpoint = false;

                //TOUCH GAP $!gap
                oOption.TouchGap = 0.0;

                //CLEARANCE $!clearance
                oOption.Clearance = 0.0;

                //TOUCH OVER $!overlap
                oOption.TouchOverlap = 0.0;

                //INCLUDE TOUCHES
                oOption.IncludeTouches = true;

                //BRANCH A
                oOption.BranchCheckType = BranchCheck.ACHECK;

                //WIThin $!type
                //DbElementType[] olType = new DbElementType[] { DbElementTypeInstance.CPLATE, DbElementTypeInstance.HPLATE };
                //oOption.Within(olType);

                //INCLUDE CONNECTIONS
                oOption.IncludeConnections = true;



                ObstructionList oObstructionList = ObstructionList.Create();
                oObstructionList.AddObstructions(olObstruction.ToArray());

                

                ClashSet oClashSet = ClashSet.Create();

                oClasher.CheckAddAll(oOption, oObstructionList, oClashSet);

                //oClasher.CheckAdd(new DbElement[] { rCheck }, oOption, oObstructionList, oClashSet);

                Clash[] olClash = oClashSet.Clashes;

                foreach (Clash oClash in olClash)
                {
                    Console.WriteLine(oClash.First);
                }
            }
            catch { }
            finally
            {
                End();
            }

        }

        private static void End()
        {
            A.Command.CommandForPDMS("EXIT");
        }
    }

    public class ClashData
    {
        //oClash.First;
        //oClash.Second;
        //oClash.Type
        //oClash.ClashPosition

        /// <summary>
        /// Tank Equipment
        /// </summary>
        public DbElement First;
        /// <summary>
        /// HPlate or CPlate
        /// </summary>
        public DbElement Second;
        public string Type;
        public string Desc;
        public Position ClashPosition = Position.Create();

        public ClashData(string sFirst, string sSecond, string sType, string sDesc, double dX, double dY, double dZ)
        {
            this.First = DbElement.GetElement(sFirst);
            this.Second = DbElement.GetElement(sSecond);
            this.Type = sType;
            this.Desc = sDesc;
            this.ClashPosition = Position.Create();
        }

    }
}
