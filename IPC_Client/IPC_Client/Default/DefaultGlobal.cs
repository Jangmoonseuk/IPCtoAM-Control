using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace INFOGET_ZERO_HULL.Default
{
    /// <summary>
    /// 모든곳에서 사용할수 있는 전역변수형태로
    /// using D = INFOGET.Default.DefaultDefine;
    /// </summary>
    public static class DefaultGlobal
    {
        /// <summary>
        /// 개발자모드
        /// </summary>
        public static bool bDevMode = false;

        /// <summary>
        /// INI파일의 'DATA_DIR' 경로가 개발자 PC에 존재 하지 않으므로.. 테스트를 위해 생성.
        /// </summary>
        public const string sDev_Repository = @"D:\02.Work\INFOGET_AUTOAP\HHISS\03.Data_Repository";

        /// <summary>
        /// List Seq 사용하기 위해서.
        /// </summary>
        public static int iNo = 0;

       
        /// <summary>
        /// 전역으로 사용하는 정의값 변수들
        /// </summary>
        public static class Def
        {
            public static string am12EXEDir = System.Windows.Forms.Application.StartupPath; //@"C:\AVEVA\Marine\OH12.1.SP4";
            public static string am12USERNAME = INFOGET_ZERO_HULL.AM.AvevaUtil.Info.USER; //밑에서 정의. (SYSTEM)
            public static string am12PASSWORD = "XXXXXX"; //밑에서 재 정의.

            /// <summary>
            /// 사용자 임시 생성 폴더, 프로그램 종료시 삭제한다. EX) C:\Temp\threeDBOM_20200804171534.801
            /// </summary>
            public static string _tempDir2 = "";

            /// <summary>
            /// APPLDW의 PATH EX)2124_APPLDW
            /// </summary>
            public static string _shipNoAPPLDW = string.Empty;
            /// <summary>
            /// APPLDW의 PATH ('/'가 앞에 포함된.) EX)/2124_APPLDW
            /// </summary>
            public static string _shipNoSLASHAPPLDW = string.Empty;

            /// <summary>
            /// APTREE PATH EX)/HP165 (호선 최상위 ASMBLY)
            /// </summary>
            public static string _shipNoSLASHAPTree = string.Empty;

            /// <summary>
            /// APTREE PATH EX)/HP165/01_BLOCK_DIVI (생성하는 ASMBLY)
            /// </summary>
            public static string _shipNoSLASHAPTree01 = string.Empty;

            /// <summary>
            /// GPWL PATH EX)/P165_GPWL
            /// </summary>
            public static string _shipNoSLASHGPWL = string.Empty;

            //dmkim 200903
            /// <summary>
            /// APPLDW 의 Series Path List  EX)P166_APPLDW,... ('/'없음)
            /// </summary>
            public static List<string> olShipSeriesNoAPPLDW = new List<string> { };


            public static string _fullAssy11HEAD = "";
            public static string _fullAssy12HEAD = "";
            /// <summary>
            /// INI파일에서 'APPLDW_TREE_NAME'에서 가져옴.
            /// </summary>
            public static string _fullAssy14HEAD = "";
            public static string _fullAssy15HEAD = "";

            public static string _treeNo_Str = "14_";
        }

        /// <summary>
        /// 전역으로 사용하는 Data 변수들
        /// </summary>
        public static class Data
        {
            /// <summary>
            /// PARNAM속성이 해당값에 포함되어 있으면 제외.
            /// </summary>
            public static List<string> olSkipPart = new List<string> { };

            /// <summary>
            /// ZONE을 BaseType으로 하는 UDET들. (AHLIST에서 Zone 값 가져오기 위해서.)
            /// </summary>
            public static List<string> olZoneUdet = new List<string> { };

            /// <summary>
            /// TB_IF_DP 추출 (KEY : LTXA1 (7번째자리가 숫자인 목록만), VALUE : KEY와 6자리까지 값이 같고 7자리가 숫자가 아니고 마지막자리값이 KEY와 같은 LTXA1 리스트
            /// </summary>
            public static Dictionary<string, ArrayList> _dic_dwgA2dwgBs = new Dictionary<string, ArrayList>();


            /// <summary>
            /// 유효한 Block List
            /// </summary>
            public static List<string> _blockLists = new List<string> { };

            /// <summary>
            /// 유효한 Site List
            /// </summary>
            public static List<string> _siteLists = new List<string> { };

            /// <summary>
            /// 유효한 Zone List
            /// </summary>
            public static List<string> _zoneLists = new List<string> { };

            /// <summary>
            /// 아직 매칭 안함.
            /// </summary>
            public static  List<string> _aAllAPPLDWNodes = new List<string> { };

            /// <summary>
            /// INI의 'SITE_ALLOW_LIST_PATH' 경로에 있는 사이트 목록, 해당목록에 사이트가 있으면 Allow Column을 'O', 없으면 'X' 
            /// </summary>
            public static List<string> _allowSITEs = new List<string> { };

            //TODO 200824 왜 Dic을 사용했지?
            /// <summary>
            /// APPLDW_TREE_NAME
            /// </summary>
            public static Dictionary<string, ArrayList> dic_TreeNames = new Dictionary<string, ArrayList>();

            public static ArrayList _existAllDirs = new ArrayList();


            public static int _nVIZXML = 0; //

        }

        public static class Path
        {
            /// <summary>
            /// INI 파일 및 기타 Setting 관련된 파일이 저장되어 있는 경로.
            /// </summary>
            public static string _pythonDir = @"D:\2.INFOGET_DATA\Source\threeDBOM"; //밑에서 재 정의.
        }

        /// <summary>
        /// INI파일에서 설정한 정보들.
        /// </summary>
        public static class IniDef
        {
            //(dmkim 사용안함)
            public static string _assN = "2"; //AS 1 2 3 4

            //dmkim 200903
            public static bool APPLDW_CREATE_SERIES_SHIP = false;

            //G.Data.dic_TreeNames

            public static string _dataDir = ""; //밑에서 재 정의.

            public static string REV_EXPORT_CONDITION_ADD = "";

            //(dmkim 사용안함)
            public static string PML_PANELSITE = "0"; //0->MODELTYPE 단위, 1->Panel/Site 단위
            //(dmkim 사용안함)
            public static string HOW_TO_GET_BLOCK_SITE = "0"; //0->PML, 1->DB Element
            public static string HOW_TO_CREATE_ASSEMBLY_PLANNING_TREE = "0"; //0->AvevaMarine12,1->Explorer(탐색기)
            //(dmkim 사용안함)
            public static string SAVE_WORK_EACH_MODEL = "0";
            public static string SITE_ALLOW_LIST_PATH = ""; //"D:\2.INFOGET_DATA\Source\threeDBOM\SITE_ALLOW_LIST_PATH.txt"; //HMD
            //(dmkim 사용안함)
            public static string REV_YARD = "0";
            public static string REV_HEAD_DIR = @"D:\";
            //(dmkim 사용안함)
            public static string RVM_NORVM = "NORVM";
            //(dmkim 사용안함)
            public static string ATT_NOATT = "NOATT";
            //(dmkim 사용안함)
            public static string ATT_GET_OUTER = "0";

            public static string sHOW_TO_CONVERT_VIZXML_TO_VIZ = "ORACLE"; //"ORACLE", "PUBLISH", "VIZCORE3D.NET"
        }


        public static class FullPath
        {
            public static string _convertVIZXML2VIZIFG = string.Empty;
            public static string _diffCheckVIZ = string.Empty;

            /// <summary>
            /// INIT 파일 경로.
            /// </summary>
            public static string initFile = "";

            public static string _dataDirShip = ""; //밑에서 재 정의.
            public static string _dataDirShipAPTREE = ""; //밑에서 재 정의.
            public static string _dataDirShipAPPLDW = ""; //밑에서 재 정의.
            public static string _dataDirShipGPWL = ""; //밑에서 재 정의.
            public static string _dataDirShipLOGS = "";

            /// <summary>
            /// AP_Tree 생성 목록을 가져오는 파일
            /// </summary>
            public static string _Z00_File = "";
            /// <summary>
            /// ZONE_Tree 생성 목록을 가져오는 파일
            /// </summary>
            public static string _Z01_File = "";
            /// <summary>
            /// APTREE 생성 블록 목록을 가져오는 파일
            /// </summary>
            public static string _Z99_TREE_ALL = "";
            public static string _Z99_ERECT_ALL = "";

            /// <summary>
            /// APPDLW 생성 블록 목록을 가져오는 파일
            /// </summary>
            public static string _Z99_APPLDW_ALL = "";
            /// <summary>
            /// GPWL 생성 블록 목록을 가져오는 파일
            /// </summary>
            public static string _Z99_GPWL_ALL = "";

        }


        public static class Licence
        {
            /// <summary>
            /// 라이센스 만료 날짜 YYYY-MM-DD EX)2099-01-01
            /// </summary>
            public static string sExpiredDate = "2020-01-01";
        }
    }
}
