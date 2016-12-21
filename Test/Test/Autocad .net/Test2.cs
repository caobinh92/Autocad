using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Specialized;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.PlottingServices;

[assembly: CommandClass(typeof(Test.Test2))]

namespace Test
{
    class Test2
    {
        //public static bool CheckValid;
        [CommandMethod("T2")]
        public void T2()
        {
            try
            {
                DocumentCollection acDocMgr = Application.DocumentManager;
                Document acDoc = acDocMgr.MdiActiveDocument;
                Database acCurDb = acDocMgr.MdiActiveDocument.Database;
                Editor acDocEd = acDocMgr.MdiActiveDocument.Editor;
                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    Point3d acPtBotTxt3d = GetPoint3dFromPrompt(acDoc, "Pick First Point");
                    Point3d acPtTopTxt3d = GetPoint3dFromPrompt(acDoc, "Pick Second Point");
                    Point2d acPtBotTxt2d = new Point2d(acPtBotTxt3d.X, acPtBotTxt3d.Y);
                    Point2d acPtTopTxt2d = new Point2d(acPtTopTxt3d.X, acPtTopTxt3d.Y);
                    if ((acDocEd.SelectWindow(acPtBotTxt3d, acPtBotTxt3d)).Value == null) return;
                    SelectionSet acSSet = (acDocEd.SelectWindow(acPtBotTxt3d, acPtBotTxt3d)).Value;
                    foreach (ObjectId acObjId in acSSet.GetObjectIds())
                    {
                        Application.ShowAlertDialog(acObjId.ToString());
                    }
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception e)
            {
                Application.ShowAlertDialog("Message: \n" + e.Message);
                Application.ShowAlertDialog("StackTrace: \n" + e.StackTrace.ToString());
                Application.ShowAlertDialog("TargerSite: " + e.TargetSite.ToString());
            }
        }

        public static Point3d GetPoint3dFromPrompt(Document acDoc, string TextPrompt)
        {
            Editor acDocEd = acDoc.Editor;
            PromptPointOptions pPtOpt = new PromptPointOptions("\n" + TextPrompt);
            PromptPointResult pPtRes = acDocEd.GetPoint(pPtOpt);
            if (pPtRes.Status == PromptStatus.OK)
            {
                return new Point3d(pPtRes.Value.X, pPtRes.Value.Y, 0);
            }
            else
            {
                Application.ShowAlertDialog("Bạn đã thoát lệnh!");
                return new Point3d(0, 0, 0);
            }
        }

    }
}