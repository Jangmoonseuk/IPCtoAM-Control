using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

using System.Security;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Collections;

using INFOGET_ZERO_HULL.Default;
using INFOGET_ZERO_HULL.Geometry;
using INFOGET_ZERO_HULL;

using A = INFOGET_ZERO_HULL.AM.AvevaUtil;
using D = INFOGET_ZERO_HULL.Default.DefaultDefine;
using E = INFOGET_ZERO_HULL.Default.DefaultEnum;
using G = INFOGET_ZERO_HULL.Default.DefaultGlobal;
using U = INFOGET_ZERO_HULL.Util.Utils;
using INFOGET_ZERO_HULL.Geometry;

namespace INFOGET_ZERO_HULL.Util
{
    public static class Utils
    {
        public static class _File
        {
            /// <summary>
            /// 파일의 Directory가 없으면 생성
            /// </summary>
            /// <param name="sFile"></param>
            public static void CreateDirFromFile(string sFile)
            {
                if (File.Exists(sFile) == false)
                {
                    U._File.CreateDir(Path.GetDirectoryName(sFile));
                }
            }
            /// <summary>
            /// 파일의 Directory가 없으면 생성.
            /// </summary>
            /// <param name="sPath"></param>
            public static bool CreateDir(string sDir)
            {
                var bRtn = true;
                try
                {
                    if (ExistsDir(sDir) == false)
                    {
                        Directory.CreateDirectory(sDir);
                    }
                }
                catch { bRtn = false; }
                return bRtn;
            }
            /// <summary>
            /// REV 생성
            /// </summary>
            /// <param name="data">REV 생성 Data</param>
            /// <param name="SavePath">REV 파일 경로</param>
            /// <param name="centerP">중심 점</param>
            /// <param name="Scale">Scale 값</param>
            /// <param name="subinfo">추가 정보</param>
            public static void CreateREV(List<IGES> data, string SavePath, Point3D centerP, double Scale, string subinfo)
            {
                StreamWriter sw = new StreamWriter(SavePath);
                StreamWriter swsub = new StreamWriter(subinfo);
                sw.WriteLine("HEAD");
                sw.WriteLine("     1     1");
                sw.WriteLine("AVEVA  Marine HullDesign");
                sw.WriteLine("");
                sw.WriteLine("Date");
                sw.WriteLine("UserId");
                sw.WriteLine("MODL");
                sw.WriteLine("     1     1");
                sw.WriteLine("HULL");
                sw.WriteLine("/MODEL");
                int i = 1;
                foreach (var iges in data)
                {
                    i++;
                    if (i == 2)
                        continue;
                    List<Point3D> point = new List<Point3D>();
                    List<Point3D> pointOrigin = new List<Point3D>();
                    foreach (var b in iges.Lines)
                    {
                        Point3D result = centerP.DownScale(centerP, b.StartPoint, Scale);

                        point.Add(result);

                        pointOrigin.Add(b.StartPoint);
                    }
                    Point3D publicP = new Point3D();

                    Point3D cen = publicP.getCenter(point);
                    Point3D min = publicP.getminP(point);
                    Point3D max = publicP.getmaxP(point);

                    Point3D minori = publicP.getminP(pointOrigin);
                    Point3D maxori = publicP.getmaxP(pointOrigin);

                    sw.WriteLine("CNTB");
                    sw.WriteLine("     1     2");
                    sw.WriteLine(string.Format("/{0}", iges.Plane.getName()));
                    sw.WriteLine(string.Format("{0:F0}    {1:F0}     {2:F0}", cen.X, cen.Y, cen.Z));//중심점
                    sw.WriteLine("    5");//색상
                    sw.WriteLine("PRIM");
                    sw.WriteLine("     1     1");
                    sw.WriteLine("    11"); //Plate
                    sw.WriteLine("     0.0010000     0.0000000     0.0000000     0.0000000");// u 백터
                    sw.WriteLine("     0.0000000     0.0010000     0.0000000     0.0000000");// v 벡터
                    sw.WriteLine("     0.0000000     0.0000000     0.0010000     0.0000000");// w 벡터
                    sw.WriteLine(string.Format("{0:F0}    {1:F0}     {2:F0}", min.X, min.Y, min.Z));//최소점
                    sw.WriteLine(string.Format("{0:F0}    {1:F0}     {2:F0}", max.X, max.Y, max.Z));//최대점
                    sw.WriteLine("     1 ");
                    sw.WriteLine("     1 ");
                    sw.WriteLine(string.Format("      {0}", point.Count()));
                    foreach (var b in point)
                    {
                        sw.WriteLine(string.Format("{0:F0}      {1:F0}     {2:F0}", b.X, b.Y, b.Z));//포인트
                        sw.WriteLine(string.Format("{0:F0}      {1:F0}     {2:F0}", iges.Plane.getNormal().X, iges.Plane.getNormal().Y, iges.Plane.getNormal().Z));//포인트
                    }
                    sw.WriteLine("CNTE");
                    sw.WriteLine("     1     2");

                    //Subinfo
                    swsub.WriteLine(string.Format("{0:F0},{1:F0},{2:F0}", minori.X, minori.Y, minori.Z));//최소점
                    swsub.WriteLine(string.Format("{0:F0},{1:F0},{2:F0}", maxori.X, maxori.Y, maxori.Z));//최대점
                }

                sw.WriteLine("END:");
                sw.WriteLine("     1     1");

                sw.Close();
                swsub.Close();
            }
            public static List<Line3D> GetREVPoint(List<IGES> data, string SavePath, Point3D centerP, string subinfo)
            {
                List<Line3D> returndata = new List<Line3D>();
                foreach (var iges in data)
                {
                    foreach (var Line in iges.Lines)
                    {
                        if(Math.Abs(Line.StartPoint.X-Line.EndPoint.X)<2&& Math.Abs(Line.StartPoint.Y - Line.EndPoint.Y)<2 && Line.Length>10)
                        {
                            returndata.Add(Line);
                        }
                        if (Math.Abs(Line.StartPoint.X - Line.EndPoint.X) < 2 && Math.Abs(Line.StartPoint.Z - Line.EndPoint.Z) < 2 && Line.Length > 10)
                        {
                            returndata.Add(Line);
                        }
                        if (Math.Abs(Line.StartPoint.Z - Line.EndPoint.Z) < 2 && Math.Abs(Line.StartPoint.Y - Line.EndPoint.Y) < 2 && Line.Length > 10)
                        {
                            returndata.Add(Line);
                        }
                    }
                }

                return returndata;
            }
            public static string GetDirectory(string sPath)
            {
                return Path.GetDirectoryName(sPath);
            }
            /// <summary>
            /// Directory Exist 체크.
            /// </summary>
            /// <param name="sDir"></param>
            /// <param name="bSpeedUp">True 일 경우에 타임아웃 쓰레드를 이용하여 체크.0.5초</param>
            /// <returns></returns>
            public static bool ExistsDir(string sDir, bool bSpeedUp = false )
            {
                return bSpeedUp ? PathExists(sDir) : Directory.Exists(sDir);
            }

            /// <summary>
            ///  File Exist 체크.
            /// </summary>
            /// <param name="sFile"></param>
            /// <param name="bSpeedUp">True 일 경우에 타임아웃 쓰레드를 이용하여 체크.0.5초</param>
            /// <returns></returns>
            public static bool ExistsFile(string sFile, bool bSpeedUp = false)
            {
                return bSpeedUp ? PathExists(sFile,false) : File.Exists(sFile);
            }

            private static bool PathExists(string path, bool isDir = true)
            {
                bool exists = true;
                Thread t = new Thread
                (
                    new ThreadStart(delegate ()
                    {
                        if (isDir) { exists = Directory.Exists(path); }
                        else { exists = File.Exists(path); }
                    })
                );
                t.Start();
                bool completed = t.Join(500); //half a sec of timeout
                if (!completed) { exists = false; t.Abort(); }
                return exists;
            }

            public static void WriteFileString(string sFullPath, string sInfo)
            {
                if (File.Exists(sFullPath)) { File.Delete(sFullPath); }
                FileStream fs2 = new FileStream(sFullPath, FileMode.Create, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(fs2, System.Text.Encoding.Default);
                sw.Write(sInfo);
                sw.Close();
            }

            public static void WriteFile(ArrayList lineArrayLists, string fileName, bool bANSI, bool isAppend = false)
            {

                //File Write (ANSI or UTF-8 가능)
                FileMode oFileMode = isAppend ? FileMode.Append : FileMode.Create;
                FileStream fileStreamOutputXXX = new FileStream(fileName, oFileMode);
                try
                {
                    if (isAppend == false) { fileStreamOutputXXX.Seek(0, SeekOrigin.Begin); }

                    if (lineArrayLists.Count == 0)
                    {
                        string wrieLine = "";

                        byte[] info = System.Text.Encoding.Default.GetBytes(wrieLine + "\r\n"); //ANSI
                        if (bANSI == false)
                        {
                            info = new UTF8Encoding(true).GetBytes(wrieLine + "\r\n"); //UTF-8
                        }
                        fileStreamOutputXXX.Write(info, 0, info.Length);
                    }
                    else
                    {
                        foreach (string wrieLine in lineArrayLists)
                        {
                            byte[] info = System.Text.Encoding.Default.GetBytes(wrieLine + "\r\n"); //ANSI
                            if (bANSI == false)
                            {
                                info = new UTF8Encoding(true).GetBytes(wrieLine + "\r\n"); //UTF-8
                            }
                            fileStreamOutputXXX.Write(info, 0, info.Length);
                        }
                    }
                }
                catch { Console.WriteLine("Write Error : " + fileName); }
                finally
                {
                    fileStreamOutputXXX.Flush();
                    fileStreamOutputXXX.Close();
                }

            }

            public static void WriteFile(List<string> lineArrayLists, string fileName, bool bANSI, bool isAppend = false)
            {
                ArrayList olLine = new ArrayList();
                foreach (string sLine in lineArrayLists)
                {
                    olLine.Add(sLine);
                }

                U._File.WriteFile(olLine, fileName, bANSI, isAppend);
            }
            
            /// <summary>
            /// 해당파일을 읽어드림.
            /// </summary>
            /// <param name="sFullPath"></param>
            /// <returns></returns>
            public static List<string> ReadFile(string sFullPath)
            {
                var olData = new List<string>();

                StreamReader sr = new StreamReader(sFullPath, Encoding.UTF8);
                string sLine;

                while ((sLine = sr.ReadLine()) != null)
                {
                    olData.Add(sLine);
                }
                sr.Close();
                return olData;
            }
            /// <summary>
            /// 해당파일을 읽어드림.
            /// </summary>
            /// <param name="sFullPath"></param>
            /// <returns></returns>
            public static List<Point3D> ReadFiletoPoint(string sFullPath)
            {
                List<Point3D> olData = new List<Point3D>();

                StreamReader sr = new StreamReader(sFullPath, Encoding.Default);
                string sLine;

                while ((sLine = sr.ReadLine()) != null)
                {
                    Point3D point = new Point3D(sLine,false);
                    olData.Add(point);
                }
                sr.Close();
                return olData;
            }
            public static List<string> ReadFile(FileStream oFileStream)
            {
                var olData = new List<string>();

                StreamReader sr = new StreamReader(oFileStream, Encoding.Default);
                string sLine;

                while ((sLine = sr.ReadLine()) != null)
                {
                    olData.Add(sLine);
                }
                sr.Close();
                return olData;
            }

            /// <summary>
            /// Source Directory의 파일들을 Target Directory에 복사한다 (단, 최신의 파일 일 경우; File.GetLastWriteTime로 비교)
            /// </summary>
            /// <param name="sSourceDir"></param>
            /// <param name="sTargetDir">없으면 Directory를 생성한다.</param>
            /// <returns>에러메시지</returns>
            public static string CopyIfUpToDateFiles(string sSourceDir, string sTargetDir)
            {
                string sRtn = string.Empty;

                var iTotal = 0; var iPass = 0; var iCopied = 0; var iFail = 0;
                var olCopied = new List<string> { };
                var olFailed = new List<string> { };

                if (U._File.CreateDir(sTargetDir) == false)
                {
                    sRtn = string.Format(LogMsg.INIT.I04_0, sTargetDir); return sRtn;
                }

                if (U._File.ExistsDir(sSourceDir, true))
                {
                    var olFile = (from file in Directory.GetFiles(sSourceDir)
                                 let info = new DirectoryInfo(file)
                                 select new { Name = info.Name, Attributes = info.Attributes }).ToList();

                    iTotal = olFile.Count;

                    if (iTotal == 0) { sRtn = string.Format(LogMsg.INIT.I06_0, sSourceDir); return sRtn; }

                    foreach (var file in olFile)
                    {
                        bool bCopy = false;

                        string sFile_Source = Path.Combine(sSourceDir, file.Name.ToString());
                        string sFile_Target = Path.Combine(sTargetDir, file.Name.ToString());

                        if (File.Exists(sFile_Target) == false) { bCopy = true; }
                        else
                        {
                            DateTime oDtSource = File.GetLastWriteTime(sFile_Source);
                            DateTime oDtTarget = File.GetLastWriteTime(sFile_Target);

                            if (oDtSource > oDtTarget)
                            {
                                try { File.Delete(sFile_Target); }
                                catch { }
                                bCopy = true;
                            }
                        }

                        if (bCopy)
                        {
                            try
                            {
                                File.Copy(sFile_Source, sFile_Target);
                                iCopied += 1;
                                olCopied.Add(file.Name.ToString());
                            }
                            catch
                            {
                                iFail += 1;
                                olFailed.Add(file.Name.ToString());
                            }
                        }
                        else { iPass += 1; }
                       
                    }

                }
                else
                {
                    sRtn = string.Format(LogMsg.INIT.I05_0, sSourceDir); return sRtn;
                }

                string sMsg = string.Format("Result=> Total:{0}, Pass:{1}, Copy:{2}, Fail:{3}", iTotal, iPass, iCopied, iFail);
                Console.WriteLine(sMsg);
                if (olCopied.Count > 0)
                {
                    for (int i = 0; i < olCopied.Count; i++)
                    {
                        Console.WriteLine(string.Format("Copy File : {0} {1}",i+1,olCopied[i])); ;
                    }
                }
                if (olFailed.Count > 0)
                {
                    Console.WriteLine("Copy Source Dir : " + sSourceDir);
                    Console.WriteLine("Copy Target Dir : " + sTargetDir);

                    for (int i = 0; i < olFailed.Count; i++)
                    {
                        Console.WriteLine(string.Format("Fail File : {0} {1}", i + 1, olFailed[i])); ;
                    }
                }

                return sRtn;
            }
        }
        
        public static class _System
        {
            /// <summary>
            /// GAC에 등록된 DLL을 호출했는지 여부.
            /// </summary>
            /// <returns></returns>
            public static bool isGAC()
            {
                return Assembly.GetExecutingAssembly().GlobalAssemblyCache;
            }
            /// <summary>
            /// Garbage Collection
            /// </summary>
            public static void GC_Collect()
            {
                GC.Collect();
                GC.WaitForPendingFinalizers(); //Garbage Collect이 객체를 해제할때까지 대기 상태유지.
            }

            public static void PrintMemorySize()
            {
                Int64 usageMemory = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;

                double memorySize = 0.0;
                StringBuilder sb = new StringBuilder();

                int i = 0;

                while (usageMemory > 1024L)
                {
                    usageMemory = (Int64)(usageMemory / 1024L);
                    i++;
                }

                E.MemorySizeType sizeType = (E.MemorySizeType)i;

                memorySize = usageMemory;

                Console.WriteLine("Memory Size:{0}, Size:{1}", memorySize, usageMemory);
            }


            //CMD 실행
            public static void RunCMDOnly(string runFile, bool consoleYes, bool waitForExit, ref string sRtn)
            {
                //
                try
                {
                    //string execStmt = String.Format(@" /C ""{0}""", runFile);
                    string execStmt = String.Format(@" /C " + @"{0}", runFile);
                    sRtn = string.Format("execStmt : {0}\r\n", execStmt);
                    System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", execStmt);

                    procStartInfo.RedirectStandardOutput = false; //true; (중요)
                    procStartInfo.UseShellExecute = false;

                    procStartInfo.CreateNoWindow = consoleYes; //true->WINDOWS(GUI) BASE, false->CONSOLE BASE

                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo = procStartInfo;
                    proc.Start();
                    if (waitForExit) { proc.WaitForExit(); }
                    proc.Close();
                }
                catch
                {
                }
                //
                return;
            }

            public static string GetAssemblyVerInfo()
            {
                //dmkim 201026 dump 정보가져옴.

                var assembly = Assembly.GetExecutingAssembly();
                var arr = assembly.FullName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var version = arr.FirstOrDefault(p => p.Contains("Version")).Trim();

                string sLocation = assembly.Location == "" ? assembly.CodeBase.Replace("file:///","") : assembly.Location;
                string sRtn = string.Empty;
                try
                {
                    sRtn = string.Format("{0} {1} {2}", version, File.GetLastWriteTime(sLocation).ToShortDateString(), File.GetLastWriteTime(sLocation).ToLongTimeString());
                }
                catch { }
                return sRtn;
            }
        }

        public static class _AM
        {
            //dmkim 170222
            /// <summary>
            /// 도면생성을 위한 특수문자 기호 빼기. / Special Calse
            /// </summary>
            /// <param name="sDwgName"></param>
            /// <returns></returns>
            public static string GetAvailableDrawingName(string sDwgName)
            {
                string sRtn = sDwgName;

                string sSpecial = @"~!@$%^*\""'";
                foreach (var item in sSpecial)
                {
                    if (sRtn.Contains(item)) { sRtn = sRtn.Replace(item.ToString(), ""); }
                }

                //dmkim 170307
                // SPOOL 1 of SPLDRG.. 식으로 이름없는 SPOOL일 경우
                // 시스템이 알아서 대문자 형태로 도면저장을 한다.. 도면을 체크하기 위해서는 대문자형태로 변경해준다.
                // /SPOOL1ofSPLDRGC620A-2-NH-21010-61001-01-DWG => /SPOOL1OFSPLDRGC620A-2-NH-21010-61001-01-DWG
                sRtn = sRtn.Replace("of", "OF");



                return sRtn;
            }
        }

        public static class _Time
        {
            /// <summary>
            /// 지금 시간.
            /// </summary>
            /// <returns></returns>
            public static string GetNow()
            {
                Dictionary<int, string> months = new Dictionary<int, string>()
            {
                { 1, "Jan"},
                { 2, "Feb"},
                { 3, "Mar"},
                { 4, "Apr"},
                { 5, "May"},
                { 6, "Jun"},
                {7, "Jul"},
                { 8, "Aug"},
                { 9, "Sep"},
                { 10, "Oct"},
                { 11, "Nov"},
                { 12, "Dec"},
            };


                return string.Format("{0:HH:mm:ss dd }", DateTime.Now) + months[DateTime.Now.Month] + string.Format("{0: yyyy}", DateTime.Now);
            }
            public static string GetNowTime()
            {
                return string.Format("{0:yyyyMMdd-HHmmss}", DateTime.Now);
            }


        }

        public static class _Xml
        {
            //JST 20190115 Generic 으로 수정
            public static void SerializeOption<T>(T oInst, string sPath)
            {
                XmlWriterSettings xs = new XmlWriterSettings();
                xs.Encoding = Encoding.UTF8;
                xs.Indent = true;
                xs.IndentChars = "\t";
                xs.NewLineHandling = NewLineHandling.Replace;

                XmlWriter oWrite = XmlWriter.Create(sPath, xs);

                try
                {
                    DataContractSerializer ser = new DataContractSerializer(typeof(T));
                    ser.WriteObject(oWrite, oInst);
                }
                catch (Exception ex)
                {
                    //exception을 throw하면 finally가 실행되지 않는다.
                    oWrite.Close();

                    throw ex;
                }
                finally
                {
                    oWrite.Close();
                }
            }

            public static Object DeserializeOption(Type Type, string sPath)
            {
                if (File.Exists(sPath) == false)
                    throw new Exception($"File not Exist : {sPath}");

                object obj = null;
                using (FileStream fs = new FileStream(sPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    DataContractSerializer ser = new DataContractSerializer(Type);
                    obj = ser.ReadObject(fs);
                    fs.Close();
                }
                return obj;
            }
        }

        public static class _Env
        {
            public static string MachineName = System.Environment.MachineName;
            public static string UserName = System.Environment.UserName;
            public static string UserDomainName = System.Environment.UserDomainName;

            public static string GetEnvironmentVariable(string variable)
            {
                return Environment.GetEnvironmentVariable(variable);
            }
            public static void SetEnvironmentVariable(string variable, string value)
            {
                Environment.SetEnvironmentVariable(variable, value);
            }
        }

        public static class _Text
        {
            public static bool isKorean(string str)
            {
                bool bRtn = false;
                foreach (char ch in str)
                {
                    if ((0xAC00 <= ch && ch <= 0xD7A3) || (0x3131 <= ch && ch <= 0x318E)) { bRtn = true; return bRtn; }
                }
                return bRtn;
            }
            public static bool isNumericStr(string str)
            {
                bool bRtn = true;
                double dou = 0.0;
                try
                {
                    dou = double.Parse(str);
                }
                catch
                {
                    bRtn = false;
                }
                return bRtn;
            }

            /// <summary>
            /// string의 길이가 1개여야 한다.
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public static bool isDigit(string s)
            {
                char c = Convert.ToChar(s);
                return char.IsDigit(c);
            }

            /// <summary>
            /// Compares wildcard to string
            /// </summary>
            /// <param name="input">string to compare</param>
            /// <param name="mask">Wildcard mask (ex: *.jpg)(</param>
            /// <returns>True if match found</returns>
            public static bool CompareWildCard(IEnumerable<char> input, string mask)
            {
                for (int i = 0; i < mask.Length; i++)
                {
                    switch (mask[i])
                    {
                        case '?':
                            if (!input.Any())
                            {
                                return false;
                            }
                            input = input.Skip(1);
                            break;
                        case '*':
                            while(input.Any() && !CompareWildCard(input, mask.Substring(i+1)))
                            {
                                input = input.Skip(1);
                            }
                            break;
                        default:
                            if (!input.Any() || input.First() != mask[i])
                            {
                                return false;
                            }
                            input = input.Skip(1);
                            break;
                    }
                }
                return !input.Any();
            }
        }
    }
}
