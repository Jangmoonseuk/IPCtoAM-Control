using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;

using E = INFOGET_ZERO_HULL.Default.DefaultEnum;
using U = INFOGET_ZERO_HULL.Util.Utils;

namespace INFOGET_ZERO_HULL.Default
{
    /// <summary>
    /// DefaultOption의 GUI class를 XML로 만들기 위한 Class
    /// </summary>
    [DataContract]
    public class DefaultSettingInst
    {
       
        [DataMember]
        public oGUI GUI = new oGUI();

        public class oGUI
        {
            public E.DRAW_MODE DRAW_MODE = E.DRAW_MODE.PIECE;

            public bool bDrawNonSpooling = false;

            public int iSheetSplitMode = 1;
            public int iSheetSplitCount = 5;
            public bool bDrawIsoView = true;
            public bool bDrawPlanView = false;
            public bool bDrawElevView = false;
            public bool bDrawSecView = false;

            [DataMember]
            public oHistory History = new oHistory();
            public class oHistory
            {
                //dmkim 191008
                /// <summary>
                /// True Old Method ,  False New Method
                /// </summary>
                public bool bOldMethodHistory = true;

                public bool bHistory = false;
                public string sHistoryName = string.Empty;

                public string sHistory1Date = string.Empty;
                public string sHistory2Desc = string.Empty;
                public string sHistory3Drawn = string.Empty;
                public string sHistory4Checked = string.Empty;
                public string sHistory5Approved = string.Empty;

            }



            public string sRegi = string.Empty;

            public bool bHullType = true;

            public bool bBOMGeneration = true;

            //dmkim 171114
            public bool bRefViewGetServer = true;

            [DataMember]
            public oDwgColor DwgColor = new oDwgColor();
            public class oDwgColor
            {
                /// <summary>
                /// ALL Color 적용여부.
                /// </summary>
                public bool bAll = false;

                public E.COLOR ALL = E.COLOR.BLACK;
                public E.COLOR SYMBOL = E.COLOR.BLACK;
                public E.COLOR DIMENSION = E.COLOR.RED;
                public E.COLOR NOTE_PARTNO = E.COLOR.CYAN;
                public E.COLOR NOTE_WELDNO = E.COLOR.ORANGE;
                public E.COLOR NOTE_SPOOLNO = E.COLOR.GREEN;
                public E.COLOR NOTE_CONNINFO = E.COLOR.CYAN;
                public E.COLOR NOTE_ETC = E.COLOR.CYAN;
            }

            public bool bDrawFlangeAngleView = false;


            public string ReferViewLocationA3Iso  = string.Empty;
            public string ReferViewLocationA3Plan = string.Empty;
            public string ReferViewLocationA3Elev = string.Empty;
            public string ReferViewLocationA3Sec  = string.Empty;
            public string ReferViewLocationA4Iso  = string.Empty;
            public string ReferViewLocationA4Plan = string.Empty;
            public string ReferViewLocationA4Elev = string.Empty;
            public string ReferViewLocationA4Sec  = string.Empty;


            //dmkim 170223 PROJ별 REGI
            [DataMember]
            public Dictionary<string, string> dicRegi = new Dictionary<string, string> { };

            public bool bTestDrawing = true;

            public bool bOnlyGetMaterial = false;

            public bool bSendBOM = false;

        }

        public  void SaveFile(DefaultSettingInst oInst,string sPath)
        {
            U._Xml.SerializeOption(oInst, sPath);
        }

        public DefaultSettingInst LoadFile(string sPath)
        {
            return (DefaultSettingInst) U._Xml.DeserializeOption(typeof(DefaultSettingInst),sPath);
        }
    }
}
