using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows.Forms;

using A = INFOGET_ZERO_HULL.AM.AvevaUtil;
using U = INFOGET_ZERO_HULL.Util.Utils;

namespace INFOGET_ZERO_HULL
{

    /// <summary>
    /// 사용자/개발자를 위한 로그파일 관련.
    /// </summary>
    public static class Log
    {

        /// <summary>
        /// Result Log File Name (3D_BOM 생성 후 결과 Log)
        /// </summary>
        private const string sResultLogFileName = "3D_BOM_RESULT.Log";

        /// <summary>
        /// (FullPath)Result Log Path
        /// </summary>
        private static string sResultLogFilePath = string.Empty;

        /// <summary>
        /// Log File Name
        /// </summary>
        private const string sLogFileName = "3D_BOM.Log";

        /// <summary>
        /// (FullPath)Log Path (Server or Local)
        /// </summary>
        private static string sLogFilePath = string.Empty;

        /// <summary>
        /// Log File Name (개발자/관리자 사용)
        /// </summary>
        private const string sDevLogFileName = "3D_BOM_DEV.Log";

        /// <summary>
        /// (FullPath)DEV Log Path
        /// </summary>
        private static string sDevLogFilePath = string.Empty;

        /// <summary>
        /// Error Sum File Name (에러파일 누적)
        /// </summary>
        private const string sDevErrorLogFileName = "3D_BOM_DEV_ERROR(누적).Log";

        /// <summary>
        /// (FullPath)DEV Error 누적
        /// </summary>
        private static string sDevErrorLogPath = string.Empty;

        public static List<string> olResult = new List<string> { };

        /// <summary>
        /// USER Log List
        /// </summary>
        public static List<string> olLog = new List<string> { };

        /// <summary>
        /// DEV Log List
        /// </summary>
        public static List<string> olDevLog = new List<string> { };

        /// <summary>
        /// 파일경로 지정. 파일경로 없으면 Directory 만들기
        /// </summary>
        /// <param name="sFileDir"></param>
        public static void InitLogPath(string sFileDir)
        {
            U._File.CreateDir(sFileDir);

            Log.sResultLogFilePath = sFileDir + Log.sResultLogFileName;
            Log.sLogFilePath = sFileDir + Log.sLogFileName;
            Log.sDevLogFilePath = sFileDir + Log.sDevLogFileName;
            Log.sDevErrorLogPath = sFileDir + Log.sDevErrorLogFileName;
        }

        #region LOG Add (Log, Dev)
        /// <summary>
        /// Log Message Add
        /// </summary>
        /// <param name="sMsg">메시지</param>
        /// <param name="bAddDateTime">메시지앞에 시간 추가; Defaut False</param>
        /// <param name="bWrite">파일에 쓰기; Defaut False</param>
        public static void Add(string sMsg, bool bAddDateTime = false, bool bWrite = false)
        {
            Log.AddLog(false, sMsg, bAddDateTime, bWrite);
        }
        /// <summary>
        /// DEV Log Message Add
        /// </summary>
        /// <param name="sMsg">메시지</param>
        /// <param name="bAddDateTime">메시지앞에 시간 추가; Defaut False</param>
        /// <param name="bWrite">파일에 쓰기; Defaut False</param>
        public static void AddDev(string sMsg, bool bAddDateTime = true, bool bWrite = false)
        {
            Log.AddLog(true, sMsg, bAddDateTime, bWrite);
        }

        private static void AddLog(bool bIsDev, string sMsg, bool bAddDateTime = true, bool bWrite = false)
        {
            //Current Time
            string sDT = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff");

            if (bAddDateTime) { sMsg = sDT + " " + sMsg; }

            if (bIsDev)
            {
                olDevLog.Add(sMsg);
                if (bWrite) { WriteDevFile(); }
            }
            else
            {
                olLog.Add(sMsg);
                if (bWrite) { WriteFile(); }
            }
        }
        #endregion

        #region LOG 출력 (Log, Dev)
        /// <summary>
        /// Log File 출력
        /// </summary>
        /// <param name="bAppend">Append 모드; Defaut False</param>
        /// <param name="bOpen">메모장으로 열기</param>
        private static void WriteFile(bool bAppend = false, bool bOpen = false)
        {
            using (StreamWriter file = new StreamWriter(Log.sLogFilePath, bAppend))
            {
                foreach (string listItem in olLog)
                {
                    file.WriteLine(listItem);

                }
            }

            if (bOpen) { System.Diagnostics.Process.Start("Notepad.exe", Log.sLogFilePath); }
        }

        /// <summary>
        /// DEV Log File 출력
        /// </summary>
        /// <param name="bAppend">Append 모드; Defaut True</param>
        /// <param name="bOpen">메모장으로 열기</param>
        private static void WriteDevFile(bool bAppend = true, bool bOpen = false)
        {

            using (StreamWriter file = new StreamWriter(Log.sDevLogFilePath, bAppend))
            {
                foreach (string listItem in olDevLog)
                {
                    file.WriteLine(listItem);
                }
            }

            if (bOpen) { System.Diagnostics.Process.Start("Notepad.exe", Log.sDevLogFilePath); }
        }
        #endregion

        /// <summary>
        /// PIPEISO의 Result 로그를 만든다. Line by로 Append.할때.
        /// </summary>
        /// <param name="sResult"></param>
        public static void MakeResultLog(string sResult)
        {
            try
            {
                string sPath = Log.sResultLogFilePath;
                if (File.Exists(sPath))
                {
                    if (sResult.Contains(","))
                    {
                        if (sResult.Split(',').Length == 6)
                        {
                            string sNo = sResult.Split(',')[0];
                            string sStatus = sResult.Split(',')[1];
                            string sBOM = sResult.Split(',')[2];
                            string sName = sResult.Split(',')[3];
                            string sLog = sResult.Split(',')[4];
                            string sDwgName = sResult.Split(',')[5];

                            sResult = string.Format("{0,-4},{1,-10},{2,-10},{3,-50},{4}", sNo, sStatus, sBOM, sName, sLog);
                        }
                    }
                    StreamWriter sw = new StreamWriter(sPath, true);
                    sw.WriteLine(sResult);
                    sw.Close();
                }

            }
            catch { };
        }

        /// <summary>
        /// 로그를 만든다.
        /// </summary>
        /// <param name="olResult"></param>
        public static void MakeResultLog(List<string> olResult, List<string> olErrorMsg)
        {
            List<string> olInsert = new List<string> { };

            olInsert.Add(U._Time.GetNow());
            olInsert.Add(string.Format("PROJECT : {0}", A.Info.ID));
            olInsert.Add(string.Format("MDB     : {0}", A.Info.MDB));
            olInsert.Add(string.Format("USER    : {0}", A.Info.USER));
            olInsert.Add("");

            //ERROR 누적용.
            List<string> olError = new List<string> { };



            if (olErrorMsg.Count > 0)
            {
                olInsert.AddRange(olErrorMsg);
            }

            olInsert.Add("");

            if (olResult.Count > 0)
            {
                string sHeader = string.Format("{0,-4} {1,-10} {2,-10} {3,-50} {4}", "NO", "STATUS", "BOM", "NAME", "LOG");
                olInsert.Add(sHeader);

                //int iTotal = 0;
                //int iFail = 0;
                foreach (string sResult in olResult)
                {
                    if (sResult.Contains(","))
                    {
                        if (sResult.Split(',').Length == 6)
                        {
                            string sNo = sResult.Split(',')[0];
                            string sStatus = sResult.Split(',')[1];
                            //
                            string sBOM = sResult.Split(',')[2];
                            string sName = sResult.Split(',')[3];
                            string sLog = sResult.Split(',')[4];
                            string sDwgName = sResult.Split(',')[5];

                            string sInsert = string.Format("{0,-4},{1,-10},{2,-10},{3,-50},{4}", sNo, sStatus, sBOM, sName, sLog);
                            olInsert.Add(sInsert);

                            if (sStatus == "X") { olError.Add(sInsert); }
                        }
                        else { olInsert.Add(sResult); }
                    }
                    else { olInsert.Add(sResult); }
                }
            }


            try
            {
                string sPath = Log.sResultLogFilePath;
                StreamWriter sw = new StreamWriter(sPath);
                foreach (string sResult in olInsert)
                {
                    sw.WriteLine(sResult);
                }
                sw.WriteLine("");
                sw.Close();
            }
            catch { };


            //테스트용 에러누적로그.
            if (olError.Count > 0)
            {
                try
                {
                    string sPath = Log.sDevErrorLogPath;
                    if (File.Exists(sPath) == false)
                    {
                        FileInfo ofile = new FileInfo(sPath);
                        DirectoryInfo oDir = ofile.Directory;
                        if (oDir.Exists == false)
                        {
                            oDir.Create();
                        }

                    }
                    StreamWriter sw = new StreamWriter(sPath, true);
                    sw.WriteLine(U._Time.GetNow());
                    foreach (string sError in olError)
                    {
                        sw.WriteLine(sError);
                    }
                    sw.Close();
                }
                catch { }
            }


        }
    }


    public static class LogMsg
    {
        public static class UI
        {


            public const string U09 = "(U09) Please select 'REGI' from 'Draft Explorer'";
            public const string U10 = "(U10) \nIf you can not find the information\n(Menu-View-Explorer-Draft Explorer(Checked)";
        }
        public static class INIT
        {
            public const string I00_0 = "(I00) License Expired...\n{0}";
            public const string I01_0 = "(I01) INI파일({0})이 존재하지 않습니다.";
            public const string I02_0 = "(I02) INI파일({0})에 'DATA_DIR' 정보가 없습니다.\n프로그램 수행이 정상적이지 못합니다.";
            public const string I03_0 = "(I03) INI파일({0})에 'REV_EXPORT_CONDITION_ADD' 정보가 없습니다.\n프로그램 수행이 정상적이지 못합니다.";

            public const string I04_0 = "(I04) 폴더({0})를 생성할 수 없습니다.";
            public const string I05_0 = "(I05) 폴더({0})가 존재하지 않습니다.";
            public const string I06_0 = "(I06) 폴더({0})에 파일이 존재하지 않습니다.";

            public const string I07_01 = "(I07) ASWL({0})를 생성할수있는 DB({1})가 없습니다.";
            public const string I08_01 = "(I08) APPLDW({0})를 생성할수있는 DB({1})가 없습니다.";
        }

        public static class COMMON
        {
            public const string C00_0 = "(I00) License Expired...\n{0}";

        }

        public static class AP
        {
            public const string A00 = "STRU모델 Purpose값 존재";                 
            public const string A01 = "ZONE Valid 등 부적합";
            public const string A02 = "ZONE Naming 부적합";
            public const string A03 = "ZONE Naming 부적합(최초 '_' 위치)";
            public const string A04 = "Spln/Splt 값 존재";
            public const string A05 = "Zone등 Naming 오류";

        }

        public static class APPLDW
        {
            public const string D00_012 = "(D00) (APPLDW MODEL) NODE의 DESC의 REFNO 가 존재하지 않음: TREE: {0} , NODE:{1}, REFNO :{2}";
            public const string D01_0123 = "(D01) (APPLDW MODEL) VIZ/REV파일에서 해당모델 못찾음: TREE: {0}, NODE:{1}, FIND_KEY:{2}, VIZ:{3}";
            public const string D02_0123 = "(D02) (APPLDW MODEL) 테이블에 AM_REF 정보 없음: TREE: {0}, TABLE(TB_SNSD_3DBOM_OUTF_{1}), REFNO :{2}, NODE:{3}";
            public const string D03_0123 = "(D03) (APPLDW MODEL) VIZ/REV파일 없음: TREE: {0},  NODE:{1}, FIND_KEY:{2}, VIZ:{3}";

        }

        public static class DEV
        {
            public const string DEV00_0 = "(DEV00) APPDAR 생성시 설치번호기준으로 노드생성 실패 : {0}";
        }


    }


    /// <summary>
    /// Console에 출력.
    /// 
    /// </summary>
    public static class Display
    {
        public static bool bWrite;
        public static void WriteLine() { if (Display.bWrite) { Console.WriteLine(); } }
        public static void WriteLine(bool value) { if (Display.bWrite) { Console.WriteLine(value); } }
        public static void WriteLine(char[] buffer) { if (Display.bWrite) { Console.WriteLine(buffer); } }
        public static void WriteLine(char value) { if (Display.bWrite) { Console.WriteLine(value); } }
        public static void WriteLine(decimal value) { if (Display.bWrite) { Console.WriteLine(value); } }
        public static void WriteLine(double value) { if (Display.bWrite) { Console.WriteLine(value); } }
        public static void WriteLine(int value) { if (Display.bWrite) { Console.WriteLine(value); } }
        public static void WriteLine(long value) { if (Display.bWrite) { Console.WriteLine(value); } }
        public static void WriteLine(object value) { if (Display.bWrite) { Console.WriteLine(value); } }
        public static void WriteLine(float value) { if (Display.bWrite) { Console.WriteLine(value); } }
        public static void WriteLine(string value) { if (Display.bWrite) { Console.WriteLine(value); } }
        public static void WriteLine(uint value) { if (Display.bWrite) { Console.WriteLine(value); } }
        public static void WriteLine(ulong value) { if (Display.bWrite) { Console.WriteLine(value); } }
        public static void WriteLine(string format, object arg0) { if (Display.bWrite) { Console.WriteLine(format, arg0); } }
        public static void WriteLine(string format, params object[] arg) { if (Display.bWrite) { Console.WriteLine(format, arg); } }
        public static void WriteLine(string format, object arg0, object arg1) { if (Display.bWrite) { Console.WriteLine(format, arg0, arg1); } }
        public static void WriteLine(char[] buffer, int index, int count) { if (Display.bWrite) { Console.WriteLine(buffer, index, count); } }
        public static void WriteLine(string format, object arg0, object arg1, object arg2) { if (Display.bWrite) { Console.WriteLine(format, arg0, arg1, arg2); } }
        public static void WriteLine(string format, object arg0, object arg1, object arg2, object arg3, __arglist)
        {
            if (Display.bWrite)
            {
                ArgIterator iterator = new ArgIterator(__arglist);
                int num = iterator.GetRemainingCount() + 4;
                object[] arg = new object[num];
                arg[0] = arg0;
                arg[1] = arg1;
                arg[2] = arg2;
                arg[3] = arg3;
                for (int i = 4; i < num; i++)
                {
                    arg[i] = TypedReference.ToObject(iterator.GetNextArg());
                }


                Console.WriteLine(format, arg);
            }
        }


        #region 메시지 출력 (MessageBox.Show)
        /// <summary>
        /// 메시지 출력 System.Windows.Forms.MessageBox.Show()
        /// </summary>
        /// <param name="sMsg"></param>
        public static void Show(string sMsg)
        {
            MessageBox.Show(sMsg);
        }
        /// <summary>
        /// 메시지 출력 System.Windows.Forms.MessageBox.Show()
        /// </summary>
        /// <param name="sMsg"></param>
        /// <param name="sCaption"></param>
        public static void Show(string sMsg, string sCaption)
        {
            MessageBox.Show(sMsg, sCaption);
        }
        #endregion
    }
}
