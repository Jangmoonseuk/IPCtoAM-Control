using Aveva.Marine.Drafting;
using Aveva.Marine.Drafting.MarInterpretationObject;
using Aveva.Marine.UI;
using Aveva.Marine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INFOGET_ZERO_HULL.AM
{
    public static class AvevaMarAPI
    {
        private static MarDrafting oDraft = new MarDrafting();
        private static MarUi oUI = new MarUi();
        private static MarUtil oUtil = new MarUtil();
        public static class Drafting
        {
            public static void DwgClose()
            {
                if(oDraft.DwgCurrent())
                {
                    try
                    {
                        oDraft.DwgPurge();
                    }
                    catch { }
                    finally
                    {
                        try
                        {
                            oDraft.DwgClose();
                        }
                        catch { }
                    }
                }
            }
            public static void DwgNew(string sDwgName)
            {
                oDraft.DwgNew(sDwgName);
            }
            public static MarElementHandle CreatCurvedPanelViewByCPanel(string PanelName)
            {
                MarElementHandle oHandle = new MarElementHandle();

                MarModel oModel = new MarModel();
                oModel.Type = "hull curve";
                oModel.Name = PanelName;

                MarCurvedPanelView oViewOption = new MarCurvedPanelView();

                oViewOption.Seams = 1;
                oViewOption.Stiffeners = 1;

                oHandle = oDraft.ViewCurvedPanelNew(oModel, oViewOption);

                return oHandle;
            }
        }
    }
}
