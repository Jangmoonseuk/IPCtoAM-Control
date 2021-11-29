using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INFOGET_ZERO_HULL.Geometry
{
    /// <summary>
    /// Marine Drafting에서 사용할 LineType
    /// 12.1.SP3.0[9060]
    /// </summary>
    public abstract class LineType
    {
        public static readonly string SOLID                     = "Solid";
        public static readonly string SOLIDWIDE                 = "SolidWide";
        public static readonly string SOLIDXWIDE                = "SolidXWide";
        public static readonly string DASHED                    = "Dashed";
        public static readonly string DASHEDWIDE                = "DashedWide";
        public static readonly string DASHEDXWIDE               = "DashedXWide";
        public static readonly string DASHEDDOTTED              = "DashedDotted";
        public static readonly string DASHEDDOTTEDWIDE          = "DashedDottedWide";
        public static readonly string DASHEDDOTTEDXWIDE         = "DashedDottedXWide";
        public static readonly string DASHEDDOUBLEDOTTED        = "DashedDoubleDotted";
        public static readonly string DASHEDDOUBLEDOTTEDWIDE    = "DashedDoubleDottedWide";
        public static readonly string DASHEDDOUBLEDOTTEDXWIDE   = "DashedDoubleDottedXWide";
        public static readonly string SHORTDASHED               = "ShortDashed";
        public static readonly string SHORTDASHEDWIDE           = "ShortDashedWide";
        public static readonly string SHORTDASHEDXWIDE          = "ShortDashedXWide";
        public static readonly string DASHEDANDSOLID            = "DashedAndSolid";
        public static readonly string TRACK                     = "Track";
        public static readonly string SYSTEM5                   = "System5";
        public static readonly string SYSTEM6                   = "System6";
        public static readonly string SYSTEM7                   = "System7";
        public static readonly string SYSTEM8                   = "System8";
        public static readonly string SYSTEM9                   = "System9";
        public static readonly string SYSTEM15                  = "System15";
        public static readonly string SYSTEM16                  = "System16";
        public static readonly string SYSTEM22                  = "System22";
        public static readonly string SYSTEM23                  = "System23";
        public static readonly string SYSTEM24                  = "System24";
        public static readonly string SYSTEM25                  = "System25";
        public static readonly string SYSTEM26                  = "System26";
        public static readonly string SYSTEM27                  = "System27";
        public static readonly string DOTTED                    = "Dotted";
        public static readonly string DOTTEDWIDE                = "DottedWide";
        public static readonly string DOTTEDXWIDE               = "DottedXWide";
        public static readonly string FINEDOTTED                = "FineDotted";
        public static readonly string FINEDOTTEDWIDE            = "FineDottedWide";
        public static readonly string FINEDOTTEDXWIDE           = "FineDottedXWide";
        public static readonly string CHAINED                   = "Chained";
        public static readonly string CHAINEDWIDE               = "ChainedWide";
        public static readonly string CHAINEDXWIDE              = "ChainedXWide";
        public static readonly string DOUBLECHAINED             = "DoubleChained";
        public static readonly string DOUBLECHAINEDWIDE         = "DoubleChainedWide";
        public static readonly string DOUBLECHAINEDXWIDE        = "DoubleChainedXWide";
        public static readonly string TRIPLECHAINED             = "TripleChained";
        public static readonly string TRIPLECHAINEDWIDE         = "TripleChainedWide";
        public static readonly string TRIPLECHAINEDXWIDE        = "TripleChainedXWide";

        public static int GetNoOfLineType(string sLineType)
        {
            int iRtn = 8011;

            if (sLineType == LineType.SOLID) { iRtn = 8001; }
            else if (sLineType == LineType.DASHED) { iRtn = 8002; }
            else if (sLineType == LineType.DOTTED) { iRtn = 8003; }
            else if (sLineType == LineType.CHAINED) { iRtn = 8004; }

            else if (sLineType == LineType.SOLIDWIDE) { iRtn = 8011; }

            else if (sLineType == LineType.SOLIDXWIDE) { iRtn = 8021; }

            else if (sLineType == LineType.SHORTDASHED) { iRtn = 8035; }
           
            //dmkim 180521
            else if (sLineType == LineType.SHORTDASHEDWIDE) { iRtn = 8040; }

            return iRtn;
        }

        #region 모양유지
        //public static readonly string SOLID                     = "Solid";
        //public static readonly string SOLIDWIDE                 = "SolidWide";
        //public static readonly string SOLIDXWIDE                = "SolidXWide";
        //public static readonly string DASHED                    = "Dashed";
        //public static readonly string DASHEDWIDE                = "DashedWide";
        //public static readonly string DASHEDXWIDE               = "DashedXWide";
        //public static readonly string DASHEDDOTTED              = "DashedDotted";
        //public static readonly string DASHEDDOTTEDWIDE          = "DashedDottedWide";
        //public static readonly string DASHEDDOTTEDXWIDE         = "DashedDottedXWide";
        //public static readonly string DASHEDDOUBLEDOTTED        = "DashedDoubleDotted";
        //public static readonly string DASHEDDOUBLEDOTTEDWIDE    = "DashedDoubleDottedWide";
        //public static readonly string DASHEDDOUBLEDOTTEDXWIDE   = "DashedDoubleDottedXWide";
        //public static readonly string SHORTDASHED               = "ShortDashed";
        //public static readonly string SHORTDASHEDWIDE           = "ShortDashedWide";
        //public static readonly string SHORTDASHEDXWIDE          = "ShortDashedXWide";
        //public static readonly string DASHEDANDSOLID            = "DashedAndSolid";
        //public static readonly string TRACK                     = "Track";
        //public static readonly string SYSTEM5                   = "System5";
        //public static readonly string SYSTEM6                   = "System6";
        //public static readonly string SYSTEM7                   = "System7";
        //public static readonly string SYSTEM8                   = "System8";
        //public static readonly string SYSTEM9                   = "System9";
        //public static readonly string SYSTEM15                  = "System15";
        //public static readonly string SYSTEM16                  = "System16";
        //public static readonly string SYSTEM22                  = "System22";
        //public static readonly string SYSTEM23                  = "System23";
        //public static readonly string SYSTEM24                  = "System24";
        //public static readonly string SYSTEM25                  = "System25";
        //public static readonly string SYSTEM26                  = "System26";
        //public static readonly string SYSTEM27                  = "System27";
        //public static readonly string DOTTED                    = "Dotted";
        //public static readonly string DOTTEDWIDE                = "DottedWide";
        //public static readonly string DOTTEDXWIDE               = "DottedXWide";
        //public static readonly string FINEDOTTED                = "FineDotted";
        //public static readonly string FINEDOTTEDWIDE            = "FineDottedWide";
        //public static readonly string FINEDOTTEDXWIDE           = "FineDottedXWide";
        //public static readonly string CHAINED                   = "Chained";
        //public static readonly string CHAINEDWIDE               = "ChainedWide";
        //public static readonly string CHAINEDXWIDE              = "ChainedXWide";
        //public static readonly string DOUBLECHAINED             = "DoubleChained";
        //public static readonly string DOUBLECHAINEDWIDE         = "DoubleChainedWide";
        //public static readonly string DOUBLECHAINEDXWIDE        = "DoubleChainedXWide";
        //public static readonly string TRIPLECHAINED             = "TripleChained";
        //public static readonly string TRIPLECHAINEDWIDE         = "TripleChainedWide";
        //public static readonly string TRIPLECHAINEDXWIDE        = "TripleChainedXWide"; 
        #endregion

    }
}
