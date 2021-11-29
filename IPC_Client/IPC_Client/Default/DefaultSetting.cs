using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using INFOGET_ZERO_HULL.Default;

using D = INFOGET_ZERO_HULL.Default.DefaultDefine;
using E = INFOGET_ZERO_HULL.Default.DefaultEnum;
using O = INFOGET_ZERO_HULL.Default.DefaultSetting;

namespace INFOGET_ZERO_HULL.Default
{
    /// <summary>
    //  파일로 저장되어 있는 값들을 가져오기. INI파일, XML형태의 GUI 셋팅 값들
    //  using O = INFOGET.Default.DefaultSetting;
    /// </summary>
    public static class DefaultSetting
    {
        // INI 파일 가져오기
        // USER GUI 셋팅값 가져오기
        // Project 셋팅값 가져오기.

        /// <summary>
        /// INI파일에서 가져오는 정보.
        /// </summary>
        public static class INI
        {
            /// <summary>
            /// Ini 파일의 정보를 저장하고 있는 Dic (Key: DefaultDefine.INI의 Member 변수, Value : String)
            /// </summary>
            public static Dictionary<string, string> IniDic = new Dictionary<string, string> { };

        }

        /// <summary>
        /// GUI에서 선택한 정보.
        /// </summary>
        public static class GUI
        {
            /// <summary>
            /// 도면생성방식 PIECE, ISO
            /// GUI에서 선택.
            /// </summary>
            public static E.DRAW_MODE DRAW_MODE = E.DRAW_MODE.PIECE;

            /// <summary>
            /// Sheet Split, 1=Only1, 2=AutoSplit, 3=InsertSplitSpool
            /// </summary>
            public static int iSheetSplitMode = 1;
            /// <summary>
            /// Split할때 Spool 포함 갯수.
            /// </summary>
            public static int iSheetSplitCount = 5;

            public static bool bDrawIsoView = true;
            public static bool bDrawPlanView = false;
            public static bool bDrawElevView = false;
            public static bool bDrawSecView = false;

            public static class History
            {
                //dmkim 191008
                /// <summary>
                /// True Old Method ,  False New Method
                /// </summary>
                public static bool bOldMethodHistory = true;

                public static bool bHistory = false;
                public static string sHistoryName = string.Empty;

                public static string sHistory1Date = string.Empty;
                public static string sHistory2Desc = string.Empty;
                public static string sHistory3Drawn = string.Empty;
                public static string sHistory4Checked = string.Empty;
                public static string sHistory5Approved = string.Empty;

            }

            /// <summary>
            /// 현재 선택된 REGI 값
            /// </summary>
            public static string sRegi = string.Empty;

            /// <summary>
            /// True :Hull, False: Marine
            /// //dmkim 190122 HullType일 경우만 마진주는 조건 제외.
            /// </summary>
            public static bool bHullType = true;

            /// <summary>
            /// BOM 생성 여부.
            /// </summary>
            public static bool bBOMGeneration = true;

            /// <summary>
            /// Weld BOM 생성 여부.
            /// </summary>
            public static bool bWeldBOMGeneration = true;

            /// <summary>
            /// 임시. FMS용으로 산출. 
            /// </summary>
            public static bool bFMSGeneration = false;



            //dmkim 171114
            /// <summary>
            /// Reference Model View의 위치를 서버에서 사용할지 로컬에서 사용할지.
            /// </summary>
            public static bool bRefViewGetServer = true;


            //dmkim 180605
            /// <summary>
            /// 기본은 NonScale이지만 도면표현이 제대로 안되었을 경우. ModelScale로 사용하기 위해.
            /// </summary>
            public static bool bRealScaleDrawing = false;

            public static class DwgColor
            {
                /// <summary>
                /// ALL Color 적용여부.
                /// </summary>
                public static bool bAll = false;

                public static E.COLOR ALL = E.COLOR.BLACK;
                public static E.COLOR SYMBOL = E.COLOR.BLACK;
                public static E.COLOR DIMENSION = E.COLOR.RED;
                public static E.COLOR NOTE_PARTNO = E.COLOR.CYAN;
                public static E.COLOR NOTE_WELDNO = E.COLOR.ORANGE;
                public static E.COLOR NOTE_SPOOLNO = E.COLOR.GREEN;
                public static E.COLOR NOTE_CONNINFO = E.COLOR.CYAN;
                public static E.COLOR NOTE_ETC = E.COLOR.CYAN;
            }

            /// <summary>
            /// FLANGE 회전각 생성 여부.
            /// </summary>
            public static bool bDrawFlangeAngleView = false;
            /// <summary>
            /// Bending Table 표시여부.
            /// </summary>
            public static bool bUseBendTable = false;


            /// <summary>
            /// Reference View 위치. Iso Drawing
            /// </summary>
            public static string ReferViewLocationA3Iso = "594, 309, 694, 409";
            public static string ReferViewLocationA3Plan = "594, 209, 694, 309";
            public static string ReferViewLocationA3Elev = "594, 109, 694, 209";
            public static string ReferViewLocationA3Sec = "594, 9, 694, 109";
            /// <summary>
            /// Reference View 위치. Spool Drawing
            /// </summary>
            public static string ReferViewLocationA4Iso = "242.1, 45.6, 313.6, 112";
            public static string ReferViewLocationA4Plan = "435, 240, 510, 290";
            public static string ReferViewLocationA4Elev = "435, 140, 510, 290";
            public static string ReferViewLocationA4Sec = "435, 40, 510, 90";

            /// <summary>
            /// Project 별 REGI 저장.
            /// </summary>
            public static Dictionary<string, string> dicRegi = new Dictionary<string, string> { };

            /// <summary>
            /// TEST Drawing
            /// </summary>
            public static bool bTestDrawing = false;

            //dmkim 180503
            /// <summary>
            /// 자재정보만 가져오려고 할 경우.
            /// </summary>
            public static bool bOnlyGetMaterial = false;

            //dmkim 180503
            /// <summary>
            /// BOM정보 전송.
            /// </summary>
            public static bool bSendBOM = false;

        }

        /// <summary>
        /// DefaultSetting <-> DefaultSettingInst 
        /// </summary>
        private static DefaultSettingInst oInst = new DefaultSettingInst();

        /// <summary>
        /// DefaultInst를 DefaultOptins으로 변경.
        /// </summary>
        private static void GetOption()
        {

            //////////////////////////////////////////////////////////////////////////
            /// Path

            //////////////////////////////////////////////////////////////////////////
            /// GUI
            O.GUI.DRAW_MODE = oInst.GUI.DRAW_MODE;
            O.GUI.iSheetSplitMode = oInst.GUI.iSheetSplitMode;
            O.GUI.iSheetSplitCount = oInst.GUI.iSheetSplitCount;
            O.GUI.bDrawIsoView = oInst.GUI.bDrawIsoView;
            O.GUI.bDrawPlanView = oInst.GUI.bDrawPlanView;
            O.GUI.bDrawElevView = oInst.GUI.bDrawElevView;
            O.GUI.bDrawSecView = oInst.GUI.bDrawSecView;
            O.GUI.History.bHistory = oInst.GUI.History.bHistory;
            O.GUI.History.sHistoryName = oInst.GUI.History.sHistoryName;
            O.GUI.History.bOldMethodHistory = oInst.GUI.History.bOldMethodHistory;
            O.GUI.History.sHistory1Date = oInst.GUI.History.sHistory1Date;
            O.GUI.History.sHistory2Desc = oInst.GUI.History.sHistory2Desc;
            O.GUI.History.sHistory3Drawn = oInst.GUI.History.sHistory3Drawn;
            O.GUI.History.sHistory4Checked = oInst.GUI.History.sHistory4Checked;
            O.GUI.History.sHistory5Approved = oInst.GUI.History.sHistory5Approved;

            O.GUI.sRegi = oInst.GUI.sRegi;

            O.GUI.bHullType = oInst.GUI.bHullType;
            O.GUI.bBOMGeneration = oInst.GUI.bBOMGeneration;


            O.GUI.bRefViewGetServer = oInst.GUI.bRefViewGetServer;

            //////////////////////////////////////////////////////////////////////////
            /// Color
            O.GUI.DwgColor.bAll = oInst.GUI.DwgColor.bAll;
            O.GUI.DwgColor.ALL = oInst.GUI.DwgColor.ALL;
            O.GUI.DwgColor.SYMBOL = oInst.GUI.DwgColor.SYMBOL;
            O.GUI.DwgColor.DIMENSION = oInst.GUI.DwgColor.DIMENSION;
            O.GUI.DwgColor.NOTE_PARTNO = oInst.GUI.DwgColor.NOTE_PARTNO;
            O.GUI.DwgColor.NOTE_WELDNO = oInst.GUI.DwgColor.NOTE_WELDNO;
            O.GUI.DwgColor.NOTE_SPOOLNO = oInst.GUI.DwgColor.NOTE_SPOOLNO;
            O.GUI.DwgColor.NOTE_CONNINFO = oInst.GUI.DwgColor.NOTE_CONNINFO;
            O.GUI.DwgColor.NOTE_ETC = oInst.GUI.DwgColor.NOTE_ETC;

            //////////////////////////////////////////////////////////////////////////
            /// Reference View Location
            if (oInst.GUI.ReferViewLocationA3Iso.Length > 0) { O.GUI.ReferViewLocationA3Iso = oInst.GUI.ReferViewLocationA3Iso; }
            if (oInst.GUI.ReferViewLocationA3Iso.Length > 0) { O.GUI.ReferViewLocationA3Plan = oInst.GUI.ReferViewLocationA3Plan; }
            if (oInst.GUI.ReferViewLocationA3Iso.Length > 0) { O.GUI.ReferViewLocationA3Elev = oInst.GUI.ReferViewLocationA3Elev; }
            if (oInst.GUI.ReferViewLocationA3Iso.Length > 0) { O.GUI.ReferViewLocationA3Sec = oInst.GUI.ReferViewLocationA3Sec; }
            if (oInst.GUI.ReferViewLocationA3Iso.Length > 0) { O.GUI.ReferViewLocationA4Iso = oInst.GUI.ReferViewLocationA4Iso; }
            if (oInst.GUI.ReferViewLocationA3Iso.Length > 0) { O.GUI.ReferViewLocationA4Plan = oInst.GUI.ReferViewLocationA4Plan; }
            if (oInst.GUI.ReferViewLocationA3Iso.Length > 0) { O.GUI.ReferViewLocationA4Elev = oInst.GUI.ReferViewLocationA4Elev; }
            if (oInst.GUI.ReferViewLocationA3Iso.Length > 0) { O.GUI.ReferViewLocationA4Sec = oInst.GUI.ReferViewLocationA4Sec; }


            O.GUI.dicRegi = oInst.GUI.dicRegi;

            O.GUI.bTestDrawing = oInst.GUI.bTestDrawing;

            O.GUI.bOnlyGetMaterial = oInst.GUI.bOnlyGetMaterial;

            O.GUI.bOnlyGetMaterial = oInst.GUI.bSendBOM;

    }
        /// <summary>
        /// DefaultOption을 DefaultInst로 변경
        /// </summary>
        private static void SetOption()
        {

            //////////////////////////////////////////////////////////////////////////
            /// Path

            //////////////////////////////////////////////////////////////////////////
            /// GUI
            oInst.GUI.DRAW_MODE = O.GUI.DRAW_MODE;
            oInst.GUI.iSheetSplitMode = O.GUI.iSheetSplitMode;
            oInst.GUI.iSheetSplitCount = O.GUI.iSheetSplitCount;
            oInst.GUI.bDrawIsoView = O.GUI.bDrawIsoView;
            oInst.GUI.bDrawPlanView = O.GUI.bDrawPlanView;
            oInst.GUI.bDrawElevView = O.GUI.bDrawElevView;
            oInst.GUI.bDrawSecView = O.GUI.bDrawSecView;
            oInst.GUI.History.bHistory = O.GUI.History.bHistory;
            oInst.GUI.History.sHistoryName = O.GUI.History.sHistoryName;
            oInst.GUI.History.bOldMethodHistory = O.GUI.History.bOldMethodHistory;
            oInst.GUI.History.sHistory1Date = O.GUI.History.sHistory1Date;
            oInst.GUI.History.sHistory2Desc = O.GUI.History.sHistory2Desc;
            oInst.GUI.History.sHistory3Drawn = O.GUI.History.sHistory3Drawn;
            oInst.GUI.History.sHistory4Checked = O.GUI.History.sHistory4Checked;
            oInst.GUI.History.sHistory5Approved = O.GUI.History.sHistory5Approved;

            oInst.GUI.sRegi = O.GUI.sRegi;

            oInst.GUI.bHullType = O.GUI.bHullType;
            oInst.GUI.bBOMGeneration = O.GUI.bBOMGeneration;


            oInst.GUI.bRefViewGetServer = O.GUI.bRefViewGetServer;


            oInst.GUI.DwgColor.bAll = O.GUI.DwgColor.bAll;
            oInst.GUI.DwgColor.ALL = O.GUI.DwgColor.ALL;
            oInst.GUI.DwgColor.SYMBOL = O.GUI.DwgColor.SYMBOL;
            oInst.GUI.DwgColor.DIMENSION = O.GUI.DwgColor.DIMENSION;
            oInst.GUI.DwgColor.NOTE_PARTNO = O.GUI.DwgColor.NOTE_PARTNO;
            oInst.GUI.DwgColor.NOTE_WELDNO = O.GUI.DwgColor.NOTE_WELDNO;
            oInst.GUI.DwgColor.NOTE_SPOOLNO = O.GUI.DwgColor.NOTE_SPOOLNO;
            oInst.GUI.DwgColor.NOTE_CONNINFO = O.GUI.DwgColor.NOTE_CONNINFO;
            oInst.GUI.DwgColor.NOTE_ETC = O.GUI.DwgColor.NOTE_ETC;


            oInst.GUI.ReferViewLocationA3Iso = O.GUI.ReferViewLocationA3Iso;
            oInst.GUI.ReferViewLocationA3Plan = O.GUI.ReferViewLocationA3Plan;
            oInst.GUI.ReferViewLocationA3Elev = O.GUI.ReferViewLocationA3Elev;
            oInst.GUI.ReferViewLocationA3Sec = O.GUI.ReferViewLocationA3Sec;
            oInst.GUI.ReferViewLocationA4Iso = O.GUI.ReferViewLocationA4Iso;
            oInst.GUI.ReferViewLocationA4Plan = O.GUI.ReferViewLocationA4Plan;
            oInst.GUI.ReferViewLocationA4Elev = O.GUI.ReferViewLocationA4Elev;
            oInst.GUI.ReferViewLocationA4Sec = O.GUI.ReferViewLocationA4Sec;

            oInst.GUI.dicRegi = O.GUI.dicRegi;

            oInst.GUI.bTestDrawing = O.GUI.bTestDrawing;

            oInst.GUI.bOnlyGetMaterial = O.GUI.bOnlyGetMaterial;
            oInst.GUI.bSendBOM = O.GUI.bSendBOM;

    }

        /// <summary>
        /// 파일로부터 현재 옵션 정보를 가져온다.
        /// </summary>
        public static void GetOptionInst()
        {
            try
            {
                oInst = oInst.LoadFile(D.USER_OPTION_PATH);
                O.GetOption();
            }
            catch { Display.WriteLine("Load Fail"); }
        }
        /// <summary>
        /// 현재 옵션 정보를 파일에 저장한다.
        /// </summary>
        public static void SetOptionInst()
        {
            try
            {
                O.SetOption();
                oInst.SaveFile(oInst, D.USER_OPTION_PATH);
            }
            catch
            { Display.WriteLine("Save Fail"); }
        }
    }
}
