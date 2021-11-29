using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using IPC_RemoteObject;
using System.Diagnostics;
using A = INFOGET_ZERO_HULL.AM.AvevaUtil;
using ATT = INFOGET_ZERO_HULL.AM.AvevaAtt;
using Aveva.Pdms.Database;
using System.IO;
using Aveva.Marine.Geometry;
using Aveva.Marine.Drafting;
using Aveva.Marine.Utility;
using Aveva.Marine.UI;
using Aveva.ApplicationFramework.Presentation;

namespace IPC_Client
{
    public partial class frmIPC_Client : Form
    {
        IpcClientChannel ClientChannel;
        RemoteObject ro;

        Timer Tmr = new Timer();
        string CheckCnt = "";
        string TempCnt = "";
        Process ps = new Process();

        private MarUtil marUtil = null;
        private MarDrafting mDraft = null;
        private MarUi mUi = null;

        public frmIPC_Client()
        {
            InitializeComponent();

            marUtil = new MarUtil();
            mDraft = new MarDrafting();
            mUi = new MarUi();

            ps.StartInfo.FileName = @"C:\AVEVA\Marine\OH12.1.SP4\IPC_Server.exe";
            try
            {
                ps.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                //최초 실행시 연결
                ClientChannel = new IpcClientChannel();
                ChannelServices.RegisterChannel(ClientChannel, false);
                RemotingConfiguration.RegisterWellKnownClientType(typeof(RemoteObject), "ipc://remote/Cnt");
                ro = new RemoteObject();

                Tmr.Interval = 1000;
                Tmr.Enabled = true;

                Tmr.Tick += new EventHandler(Tmr_Tick);
                CheckCnt = ro.GetCount();
            }
            catch (Exception ex)
            {
                //재 실행시 연결
                try
                {
                    ro = new RemoteObject();

                    Tmr.Interval = 1000;
                    Tmr.Enabled = true;

                    Tmr.Tick += new EventHandler(Tmr_Tick);
                    CheckCnt = ro.GetCount();
                }
                catch (Exception ex2)
                {
                }
            }
        }

        private void frmIPC_Client_Load(object sender, EventArgs e)
        {
            
        }

        void Tmr_Tick(object sender, EventArgs e)
        {
            try
            {
                if (ps.HasExited)
                {
                    Tmr.Enabled = false;
                    Tmr.Tick -= new EventHandler(Tmr_Tick);
                    return;
                }
                TempCnt = CheckCnt;
                if (CheckCnt != ro.GetCount()&& ro.GetCount()!= "LOAD_END")
                {
                    CheckCnt = ro.GetCount();
                    if (ro.GetCount() == "TEST")
                    {
                        TESTCODE();
                    }
                    else if(ro.GetCount().StartsWith("C "))
                    {
                        string command = ro.GetCount().Replace("C ", "").Trim();
                        A.Command.CommandForPDMS(command);
                    }
                    else
                    {
                        DbElement db = DbElement.GetElement(ro.GetCount());
                        A.Command.CommandForPDMS(ro.GetCount());
                        if (db.IsValid)
                        {
                            string Node = db.ToString().Contains('/') ? db.ToString() : "/" + db.ToString();
                            string path = Path.Combine(@"C:\temp", "test.rvm");
                            A.Command.CommandForPDMS(string.Format(@"EXPORT FILE ""{0}"" ", path));
                            A.Command.CommandForPDMS("export filenote ''");
                            A.Command.CommandForPDMS("EXPORT " + Node);
                            A.Command.CommandForPDMS("REPR DARC 1");
                            A.Command.CommandForPDMS("Export repr on");
                            A.Command.CommandForPDMS("EXPORT FINISH");

                            ro.SetCount(path);
                        }
                    }
                }                
            }
            catch
            {
            }
        }
        private void btnGet_Click(object sender, EventArgs e)
        {
            this.textBox1.AppendText("Cnt Get : " + ro.GetCount() + Environment.NewLine);
        }
        private void TESTCODE()
        {
            //2D 화면 전환
            Change2D();

            mUi.MessageNoConfirm("협소개소 검도");

            double size = 2000;
            double volume = 5000;

            // View 선택
            MarPointPlanar pickPos = new MarPointPlanar();
            bool finish = false;
            while (!finish)
            {
                int res = this.mUi.PointPlanarReq("Indicate the view", pickPos);
                if (res == this.marUtil.Ok())
                {
                    try
                    {
                        MarElementHandle viewHandle = this.mDraft.ViewIdentify(pickPos);

                        // View direction 구하기
                        MarTransformation viewProject = mDraft.ViewProjectionGet(viewHandle);
                        viewProject.Invert();
                        MarVector dirX = new MarVector(1, 0, 0);
                        MarVector dirY = new MarVector(0, 1, 0);
                        MarVector dirZ = new MarVector(0, 0, 1);
                        dirX.Transform(viewProject);
                        dirY.Transform(viewProject);
                        dirZ.Transform(viewProject);
                        dirZ.X = dirZ.X * -1;
                        dirZ.Y = dirZ.Y * -1;
                        dirZ.Z = dirZ.Z * -1;

                        // View scale
                        double[] scale = this.mDraft.ElementTransformationGet(viewHandle).GetScale();
                        double cursorSize = (size / 2) * scale[0];

                        // 마우스 커서 설정
                        MarContourPlanar circle = new MarContourPlanar(new MarPointPlanar(-cursorSize, 0));
                        circle.AddArc(new MarPointPlanar(cursorSize, 0), cursorSize);
                        circle.AddArc(new MarPointPlanar(-cursorSize, 0), cursorSize);

                        MarHighlightSet highlight = new MarHighlightSet();
                        highlight.AddGeometry2D(circle);

                        MarCursorType cursor = new MarCursorType();
                        cursor.SetDragCursor(highlight, new MarPointPlanar(0, 0));

                        MarStatPointPlanarReq status = new MarStatPointPlanarReq();
                        status.Cursor = cursor;

                        while (true)
                        {
                            int res2 = mUi.PointPlanarReq("Pick Position", pickPos, status);
                            if (res2 == marUtil.Ok())
                            {
                                try
                                {
                                    MarModel panel = new MarModel();
                                    try
                                    {
                                        this.mDraft.ModelIdentify(pickPos, panel);
                                    }
                                    catch
                                    {
                                    }

                                    // 3D 좌표로 변환
                                    MarPoint point3D = new MarPoint();
                                    if (!this.marUtil.TraCoordShip(pickPos, panel != null ? panel.Name : "", point3D)) throw new Exception();

                                    // 모델정보 가져오기
                                    circle = new MarContourPlanar(new MarPointPlanar(pickPos.X - cursorSize, pickPos.Y));
                                    circle.AddArc(new MarPointPlanar(pickPos.X + cursorSize, pickPos.Y), cursorSize);
                                    circle.AddArc(new MarPointPlanar(pickPos.X - cursorSize, pickPos.Y), cursorSize);

                                    MarCaptureRegionPlanar region = new MarCaptureRegionPlanar();
                                    region.SetContour(circle);
                                    region.Cut = 1;
                                    MarElementHandle[] handles = mDraft.ModelCapture(region);

                                    List<string> modelNames = new List<string>();

                                    foreach (MarElementHandle handle in handles)
                                    {
                                        MarModel model = null;

                                        try
                                        {
                                            model = mDraft.ModelPropertiesGet(handle);

                                            // 모델이 삭제되었는지 체크                                    
                                            mDraft.ElementDbrefGet(handle);

                                            modelNames.Add("/" + model.Name);
                                        }
                                        catch
                                        {
                                            this.mUi.MessageNoConfirm("이미 삭제된 모델입니다: " + model.Name);
                                            continue;
                                        }
                                    }
                                    if(point3D != null && modelNames.Count > 0)
                                    {
                                        // 3D View 표시
                                        volume = volume / 2;
                                        MarPoint clipMin = new MarPoint(point3D.X - volume, point3D.Y - volume, point3D.Z - volume);
                                        MarPoint clipMax = new MarPoint(point3D.X + volume, point3D.Y + volume, point3D.Z + volume);
                                        this.DisplayModel(modelNames, clipMin, clipMax, 0, new MarVector[2] { dirY, dirZ });
                                        finish = true;
                                        break;
                                    }
                                    else
                                    {
                                        MessageBox.Show("선택된 모델이 없습니다.");
                                    }
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                finish = true;
                                break;
                            }
                        }
                    }
                    catch
                    {
                        // invalid view
                    }
                }
                else
                {
                    finish = true;
                    break;
                }
            }

            mDraft.DwgRepaint();
        }
        private void Change2D()
        {
            try
            {
                foreach (var win in WindowManager.Instance.Windows)
                {
                    if (win.GetType().ToString().Contains("MdiWindowImpl"))
                    {
                        MdiWindowImpl mdi = (MdiWindowImpl)win;
                        if (mdi.Key == "Main Viewport")
                        {
                            mdi.Form.BringToFront();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n\r\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Change3D()
        {
            try
            {
                foreach (var win in WindowManager.Instance.Windows)
                {
                    if (win.GetType().ToString().Contains("MdiWindowImpl"))
                    {
                        MdiWindowImpl mdi = (MdiWindowImpl)win;
                        if (mdi.Title.Contains("3D "))
                        {
                            mdi.Form.BringToFront();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n\r\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DisplayModel(List<string> modelNames, MarPoint clipMin, MarPoint clipMax, double transparent, MarVector[] viewDir)
        {
            bool findView = false;

            try
            {
                // 3D View 화면 표시
                findView = false;
                foreach (var win in WindowManager.Instance.Windows)
                {
                    if (win.GetType().ToString().Contains("MdiWindowImpl"))
                    {
                        MdiWindowImpl mdi = (MdiWindowImpl)win;
                        if (mdi.Title.Contains("3D "))
                        {
                            findView = true;
                            mdi.Form.BringToFront();
                            break;
                        }
                    }
                }

                if (!findView)
                {
                    MessageBox.Show("3D View가 없습니다", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                A.Command.CommandForPDMS("!drawlist.delete()");

                // 모델추가
                A.Command.CommandForPDMS("!currentView = !!fmsys.currentDocument().view");
                A.Command.CommandForPDMS("!drawList = !!gphDrawlists.drawlist(!!gphDrawlists.token(!currentView))");
                A.Command.CommandForPDMS("!drawlist.removeall()");
                foreach (string name in modelNames)
                {
                    A.Command.CommandForPDMS(string.Format("!drawlist.add( {0} )", name));
                }

                // 투명도 설정
                if (transparent > 0)
                {
                    A.Command.CommandForPDMS(string.Format("!drawlist.TRANSLUCENCY(!drawlist.members(), {0})", transparent));
                }

                // View direction
                if (viewDir != null)
                {
                    A.Command.CommandForPDMS("!dir.delete()");
                    foreach (MarVector vec in viewDir)
                    {
                        A.Command.CommandForPDMS("!dir = object DIRECTION()");
                        A.Command.CommandForPDMS(string.Format("!dir.east = {0:F2}", vec.X));
                        A.Command.CommandForPDMS(string.Format("!dir.north = {0:F2}", vec.Y));
                        A.Command.CommandForPDMS(string.Format("!dir.up = {0:F2}", vec.Z));
                        A.Command.CommandForPDMS("!!gphViews.look(!currentView, !dir)");
                    }
                }

                if (clipMin != null && clipMax != null)
                {
                    // Clipping
                    A.Command.CommandForPDMS("!volume.delete()");
                    A.Command.CommandForPDMS(string.Format("!volume = object VOLUME(object POSITION('X{0} Y{1} Z{2}'), object POSITION('X{3} Y{4} Z{5}'))", clipMin.X, clipMin.Y, clipMin.Z, clipMax.X, clipMax.Y, clipMax.Z));
                    A.Command.CommandForPDMS("!!gphViews.clipboxElement(!currentView, !volume)");

                    // 화면 맞춤
                    A.Command.CommandForPDMS("!!gphViews.limits(!currentView, !volume)");
                }
                else
                {
                    // Clipping 해제
                    A.Command.CommandForPDMS("!!fmsys.currentDocument().clippingButton.val = false");
                    A.Command.CommandForPDMS("!!gphViews.clipButton(!currentView, | SELECT | )");

                    // 화면 맞춤
                    A.Command.CommandForPDMS("import 'PDMSCommands'");
                    A.Command.CommandForPDMS("using namespace 'Aveva.Pdms.Presentation.PDMSCommands'");
                    A.Command.CommandForPDMS("!commandManager.delete()");
                    A.Command.CommandForPDMS("!commandManager = object PMLNETCOMMANDMANAGER()");
                    A.Command.CommandForPDMS("!commandManager.executeCommand(|AVEVA.View.WalkTo.DrawList|)");
                    A.Command.CommandForPDMS("!commandManager.executeCommand(|AVEVA.View.Limits CE & Options|)");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CheckHole()
        {
            //int parent, button, combobox, listbox;
            //int delayTime = 800;

            //CommandManager.Instance.Commands["AVEVA.Marine.UI.Button.GeneralZapAll"].Execute();

            //// Seach Prop : 5. Colour
            //parent = FindWindowHandle("Modify General: Key Property");
            //button = FindExByCaption(parent, "&5   Colour");
            //SendMessage(button, WM_LBUTTONDOWN, 0, 1);
            //SendMessage(button, WM_LBUTTONUP, 0, 1);
            //Thread.Sleep(delayTime);

            //// Cyan
            //parent = FindWindowHandle("Modify General: Key Property");
            //combobox = FindExByType(parent, "ComboBox");
            //SendMessage(combobox, CB_SETCURSEL, 1, 0);

            //button = FindExByCaption(parent, "OK");
            //SendMessage(button, WM_LBUTTONDOWN, 0, 1);
            //SendMessage(button, WM_LBUTTONUP, 0, 1);
            //Thread.Sleep(delayTime);

            //// New Prop : 4. Line type
            //parent = FindWindowHandle("Modify General: New Property");
            //button = FindExByCaption(parent, "&4   Line type");
            //SendMessage(button, WM_LBUTTONDOWN, 0, 1);
            //SendMessage(button, WM_LBUTTONUP, 0, 1);
            //Thread.Sleep(delayTime);

            //// Track
            //parent = FindWindowHandle("Modify General: New Property");
            //listbox = FindExByType(parent, "SysListView32");
            //LV_ITEM lvi = new LV_ITEM();
            //lvi.stateMask = LVIS_FOCUSED | LVIS_SELECTED;
            //lvi.state = LVIS_FOCUSED | LVIS_SELECTED;
            //IntPtr ptrLvi = Marshal.AllocHGlobal(Marshal.SizeOf(lvi));
            //Marshal.StructureToPtr(lvi, ptrLvi, false);
            //SendMessage(listbox, LVM_SETITEMSTATE, 16, ptrLvi.ToInt32());

            //button = FindExByCaption(parent, "OK");
            //SendMessage(button, WM_LBUTTONDOWN, 0, 1);
            //SendMessage(button, WM_LBUTTONUP, 0, 1);
            //Thread.Sleep(delayTime);
        }
    }   
}
