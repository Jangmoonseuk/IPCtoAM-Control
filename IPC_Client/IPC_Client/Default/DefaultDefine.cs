using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


using A = INFOGET_ZERO_HULL.AM.AvevaUtil;
using D = INFOGET_ZERO_HULL.Default.DefaultDefine;
using E = INFOGET_ZERO_HULL.Default.DefaultEnum;


namespace INFOGET_ZERO_HULL.Default
{
    /// <summary>
    /// 고정된 상수 정의.
    /// 프로그램 초기 실행 후 변하지 않는 값들.
    /// D = INFOGET.Default.DefaultDefine; 형태로 사용
    /// </summary>
    public static class DefaultDefine
    {
        public static string INI_PATH = @"C:\\INFOGET_PIPEISO.ini";

        public static string PROGRAM_TITLE = "INFOGET 치명적 설계 오류 Zero화(선체)";

        public static string USER_OPTION_PATH = string.Format(@"C:\INFOGET_ISO\NISO_Setting.xml");

        /// <summary>
        /// 2.5 Part간 연결체크를 위한 최소 거리 (이 거리에 있으면 연결되었다고 봄)
        /// </summary>
        public const double dMinGapPartConn = 2.5;

        /// <summary>
        /// 0.01 AM버전 Hierachy PREV/NEXT 체크 후 최소 거리체크.
        /// </summary>
        public const double dMinNewGapPartConn = 0.01;

        public static class HHISS
        {
            
        }


       

        public static class INI
        {
            
        }

        public static class FullPath
        {
            //public const string _vizzardEXE = @"C:\Program Files\Softhills\VIZZARD Manager\V4.0.0.20253\VIZZARD.exe";
        }

        public static class Path
        {
            //public const string _convertVIZXML2VIZIFGSource = @"\\10.10.71.31\amhenv\VM_Projects\AM121SP4VITESSE\Infoget\hhi3DBOM\CopyFolder\INFOGET_3DBOM_VIZCONVERT";
        }
        public static class FileName
        {
            //public const string _convertVIZXML2VIZIFG_Name = "INFOGET_3DBOM_VIZCONVERT.exe"; // @"convertVIZXML2VIZIFG.exe"; //dmkim 201022 파일명 변경.
           
        }

        public const E.SHIPYARD SHIPYARD = E.SHIPYARD.HHISS;

        /// <summary>
        /// 개발자 컴퓨터이름.
        /// </summary>
        public static List<String> olDeveloper = new List<string> { "DMKIM"};

    }
}
