using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
[assembly: CommandClass(typeof(PrintAutocad.Test1))]

namespace PrintAutocad
{
    class Test1
    {
        public static BlockTableRecord acBlkTblRec;
        public static BlockReference acBlkRef;
        public static int AttributeID;
        public static Vector3d InsertPointToBottomPoint;
        public static Vector3d InsertPointToTopPoint;
        public static Scale3d acScl3d;
        public static string acPlDev;
        public static string acCanMed;
        [CommandMethod("b",CommandFlags.Session)]
        public static void b()
        {
            DocumentCollection acDocMgr = Application.DocumentManager;
            Document acDoc = acDocMgr.MdiActiveDocument;
            Database acCurDb = acDocMgr.MdiActiveDocument.Database;
            Editor acDocEd = acDocMgr.MdiActiveDocument.Editor;
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                DBObjectCollection acBlkTblRecSpcColl = GetBlockTableRecordSpaceCollection(acDoc);
                BlockTable acBlkTbl = (BlockTable)acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead);
                if (acBlkRef == null)
                {
                    acBlkRef = GetBlockReference(acDoc, "Pick a title block", "You must pick a title block!");
                    acBlkTblRec = (BlockTableRecord)acTrans.GetObject(acBlkRef.BlockTableRecord, OpenMode.ForRead);
                    AttributeID = GetAttributeID(acDoc, acBlkTblRec);
                    Point3d OriginBottomPoint = GetPoint3dFromPrompt(acDoc, "Pick first point");
                    Point3d OriginTopPoint = GetPoint3dFromPrompt(acDoc, "Pick second point");
                    InsertPointToBottomPoint = acBlkRef.Position.GetVectorTo(OriginBottomPoint);
                    InsertPointToTopPoint = acBlkRef.Position.GetVectorTo(OriginTopPoint);
                    acScl3d = acBlkRef.ScaleFactors;
                    string[] acPlDevCanMed = GetPlotDeviceAndCanonicalMediaName();
                    acPlDev = acPlDevCanMed[0]; acCanMed = acPlDevCanMed[1];
                    acBlkTblRec = (BlockTableRecord)acBlkTblRec.Clone();
                    acBlkRef = (BlockReference)acBlkRef.Clone();
                }
            }
            foreach (Document acDoc1 in acDocMgr)
            {
                acDocMgr.MdiActiveDocument = acDoc1;
                using (DocumentLock acDocLck1 = acDoc1.LockDocument())
                {
                    UserPlot2(acDoc1);
                }
            }
        }

        public static void UserPlot2(Document acDoc)
        {

            //Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            HostApplicationServices.WorkingDatabase = acCurDb;
            Editor acDocEd = acDoc.Editor;
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                DBObjectCollection acBlkTblRecSpcColl = GetBlockTableRecordSpaceCollection(acDoc);
                BlockTable acBlkTbl = (BlockTable)acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead);
                foreach (DBObject acDbBlkTblRec in acBlkTblRecSpcColl)
                {
                    BlockTableRecord acBlkTblRecSpc = (BlockTableRecord)acDbBlkTblRec;
                    foreach (ObjectId acObjId in acBlkTblRecSpc)
                    {
                        Entity acEnt = (Entity)acTrans.GetObject(acObjId, OpenMode.ForRead);
                        if (acEnt is BlockReference)
                        {
                            BlockReference acBlkRef1 = (BlockReference)acEnt;
                            BlockTableRecord acBlkTblRec1 = (BlockTableRecord)acTrans.GetObject(acBlkRef1.BlockTableRecord, OpenMode.ForRead);
                            if (acBlkTblRec1.Name.Equals(acBlkTblRec.Name))
                            {
                                Extents2d acExt2d = GetPlotArea(acBlkRef1, acScl3d, InsertPointToBottomPoint, InsertPointToTopPoint);
                                string path = Path.Combine(DirectoryFolder(acDoc.Database.OriginalFileName), Path.GetFileName(GetAttributeString(acDoc, acBlkRef1, AttributeID)));
                                InKhungTen(acBlkTblRecSpc, acExt2d, path, acPlDev, acCanMed);
                            }
                        }
                    }
                }
                acTrans.Commit();
            }
        }

        public static void UserPlot(Document acDoc)
        {
            
            //Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            Editor acDocEd = acDoc.Editor;
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                DBObjectCollection acBlkTblRecSpcColl = GetBlockTableRecordSpaceCollection(acDoc);
                BlockTable acBlkTbl = (BlockTable)acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead);
                if (acBlkRef == null)
                {
                    acBlkRef = GetBlockReference(acDoc, "Pick a title block", "You must pick a title block!");
                    acBlkTblRec = (BlockTableRecord)acTrans.GetObject(acBlkRef.BlockTableRecord, OpenMode.ForRead);
                    AttributeID = GetAttributeID(acDoc, acBlkTblRec);
                    Point3d OriginBottomPoint = GetPoint3dFromPrompt(acDoc, "Pick first point");
                    Point3d OriginTopPoint = GetPoint3dFromPrompt(acDoc, "Pick second point");
                    InsertPointToBottomPoint = acBlkRef.Position.GetVectorTo(OriginBottomPoint);
                    InsertPointToTopPoint = acBlkRef.Position.GetVectorTo(OriginTopPoint);
                    acScl3d = acBlkRef.ScaleFactors;
                    string[] acPlDevCanMed = GetPlotDeviceAndCanonicalMediaName();
                    acPlDev = acPlDevCanMed[0]; acCanMed = acPlDevCanMed[1];

                    acBlkTblRec =(BlockTableRecord) acBlkTblRec.Clone();
                    acBlkRef = (BlockReference)acBlkRef.Clone();

                }
                foreach (DBObject acDbBlkTblRec in acBlkTblRecSpcColl)
                {
                    BlockTableRecord acBlkTblRecSpc = (BlockTableRecord)acDbBlkTblRec;
                    foreach (ObjectId acObjId in acBlkTblRecSpc)
                    {
                        Entity acEnt = (Entity)acTrans.GetObject(acObjId, OpenMode.ForRead);
                        if (acEnt is BlockReference)
                        {
                            BlockReference acBlkRef1 = (BlockReference)acEnt;
                            BlockTableRecord acBlkTblRec1 = (BlockTableRecord)acTrans.GetObject(acBlkRef1.BlockTableRecord, OpenMode.ForRead);
                            if (acBlkTblRec1.Name.Equals(acBlkTblRec.Name))
                            {
                                Extents2d acExt2d = GetPlotArea(acBlkRef1, acScl3d, InsertPointToBottomPoint, InsertPointToTopPoint);
                                string path = Path.Combine(DirectoryFolder(acDoc.Database.OriginalFileName), Path.GetFileName(GetAttributeString(acDoc, acBlkRef1, AttributeID)));
                                InKhungTen(acBlkTblRecSpc, acExt2d, path, acPlDev, acCanMed);
                            }
                        }
                    } 
                }
                acTrans.Commit();
            }
        }
    
        public static void InKhungTen(BlockTableRecord acBlkTblRecSpc,Extents2d PlotArea, string path, string PlotDevice, string CanonicalMediaName)
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            //string folder = DirectoryFolder(ac.Database.OriginalFileName);
            //string filename = DocumentShortName(ac.Database.OriginalFileName) + ".pdf";
            //Application.ShowAlertDialog(path);
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                Application.SetSystemVariable("BACKGROUNDPLOT", 0);

                Layout acLayout = (Layout)acTrans.GetObject(acBlkTblRecSpc.LayoutId, OpenMode.ForRead);

                PlotInfo acPtInfo = new PlotInfo();
                acPtInfo.Layout = acLayout.ObjectId;

                PlotSettings acPtSet = new PlotSettings(acLayout.ModelType);
                acPtSet.CopyFrom(acLayout);

                PlotSettingsValidator acPtSetVlr = PlotSettingsValidator.Current;
                //acPtSetVlr.SetPlotType(acPtSet, Autodesk.AutoCAD.DatabaseServices.PlotType.Extents);
                acPtSetVlr.RefreshLists(acPtSet);
                acPtSetVlr.SetPlotWindowArea(acPtSet, PlotArea);
                acPtSetVlr.SetPlotType(acPtSet, Autodesk.AutoCAD.DatabaseServices.PlotType.Window);
                acPtSetVlr.SetUseStandardScale(acPtSet, true);
                acPtSetVlr.SetStdScaleType(acPtSet, StdScaleType.ScaleToFit);
                acPtSetVlr.SetPlotCentered(acPtSet, true);
                acPtSetVlr.SetCurrentStyleSheet(acPtSet, "monochrome.ctb");
                //acPtSetVlr.SetPlotConfigurationName(acPtSet, "DWG To PDF.pc3", "ISO_full_bleed_A1_(594.00_x_841.00_MM)");
                //acPtSetVlr.SetPlotConfigurationName(acPtSet, "DWG To PDF.pc3", "ISO_A1_(594.00_x_841.00_MM)");
                acPtSetVlr.SetPlotConfigurationName(acPtSet, acPlDev, acCanMed);
                //acPtSetVlr.SetPlotConfigurationName(acPtSet, "DWF6 ePlot.pc3", "ANSI_A_(8.50_x_11.00_Inches)");
                acPtInfo.OverrideSettings = acPtSet;
                LayoutManager.Current.CurrentLayout = acLayout.LayoutName;

                PlotInfoValidator acPtInfoVlr = new PlotInfoValidator();
                acPtInfoVlr.MediaMatchingPolicy = MatchingPolicy.MatchEnabled;
                acPtInfoVlr.Validate(acPtInfo);

                //Check if plot in process
                if (PlotFactory.ProcessPlotState == ProcessPlotState.NotPlotting)
                {
                    using (PlotEngine acPtEng = PlotFactory.CreatePublishEngine())
                    {
                        PlotProgressDialog acPtProgDlg = new PlotProgressDialog(false, 1, true);
                        using (acPtProgDlg)
                        {
                            //Define message when plot start
                            acPtProgDlg.set_PlotMsgString(PlotMessageIndex.DialogTitle, "Plot Process");
                            acPtProgDlg.set_PlotMsgString(PlotMessageIndex.CancelJobButtonMessage, "Cancel Job");
                            acPtProgDlg.set_PlotMsgString(PlotMessageIndex.CancelSheetButtonMessage, "Cancel Sheet");
                            acPtProgDlg.set_PlotMsgString(PlotMessageIndex.SheetSetProgressCaption, "Sheet Set Progress");
                            acPtProgDlg.set_PlotMsgString(PlotMessageIndex.SheetProgressCaption, "Sheet Process");

                            //Set the process range
                            acPtProgDlg.LowerPlotProgressRange = 0;
                            acPtProgDlg.UpperPlotProgressRange = 100;
                            acPtProgDlg.PlotProgressPos = 0;

                            //Display the process dialog
                            acPtProgDlg.OnBeginPlot();
                            acPtProgDlg.IsVisible = true;

                            //Start the layout plot
                            acPtEng.BeginPlot(acPtProgDlg, null);

                            //Define the plot output
                            acPtEng.BeginDocument(acPtInfo, acDoc.Name, null, 1, true, @path);

                            //Display the process message
                            acPtProgDlg.set_PlotMsgString(PlotMessageIndex.Status, "Plotting " + acDoc.Name + " - " + acLayout.LayoutName);

                            //Set the sheet process range
                            acPtProgDlg.OnBeginSheet();
                            acPtProgDlg.LowerSheetProgressRange = 0;
                            acPtProgDlg.UpperPlotProgressRange = 100;
                            acPtProgDlg.SheetProgressPos = 0;

                            //Plot the first sheet
                            PlotPageInfo acPtPageInfo = new PlotPageInfo();
                            acPtEng.BeginPage(acPtPageInfo, acPtInfo, true, null);
                            acPtEng.BeginGenerateGraphics(null);
                            acPtEng.EndGenerateGraphics(null);

                            //End plot sheet
                            acPtEng.EndPage(null);
                            acPtProgDlg.SheetProgressPos = 100;
                            acPtProgDlg.OnEndSheet();

                            //End document
                            acPtEng.EndDocument(null);

                            //End plot
                            acPtProgDlg.PlotProgressPos = 100;
                            acPtProgDlg.OnEndPlot();
                            acPtEng.EndPlot(null);
                        }
                    }
                }
            }
            }
        public static string[] GetPlotDeviceAndCanonicalMediaName()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Editor acDocEd = acDoc.Editor;
            PlotSettingsValidator acPlSetVdr = PlotSettingsValidator.Current;
            StringCollection acPlDevColl = acPlSetVdr.GetPlotDeviceList();
            for (int i = 0; i < acPlDevColl.Count; i++)
            {
                acDocEd.WriteMessage("\nNumber of device: {0}  -  Device name: {1}", i, acPlDevColl[i]);
            }
            int n =GetIntPrompt(acDoc, "Pick number of device to plot", 1, acPlDevColl.Count);
            string acPlDev = acPlDevColl[n];
            string acCanMed;
            Application.ShowAlertDialog("Plot device was chosen: "+ acPlDev);
            using (PlotSettings acPlSet = new PlotSettings(true))
            {
                acPlSetVdr.SetPlotConfigurationName(acPlSet, acPlDev, null);
                acPlSetVdr.RefreshLists(acPlSet);
                StringCollection acCanMedColl = acPlSetVdr.GetCanonicalMediaNameList(acPlSet);
                for (int i = 0; i < acCanMedColl.Count; i++)
                {
                    acDocEd.WriteMessage("\nNumber of Canonical media name: {0}  -  Canonical media name: {1}", i, acCanMedColl[i]);
                }
                n = GetIntPrompt(acDoc, "Pick number of Canonical media name",1, acCanMedColl.Count);
                acCanMed = acCanMedColl[n];
                Application.ShowAlertDialog("Canonical media name: "+acCanMed);
            }

            return new string[2] { acPlDev, acCanMed };
        }
        public static DBObjectCollection GetBlockTableRecordSpaceCollection(Document acDoc)
        {
            Database acCurDb = acDoc.Database;
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl = (BlockTable)acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead);
                DBObjectCollection acBlkTblRecSpcColl = new DBObjectCollection();
                foreach (ObjectId acObj in acBlkTbl)
                {
                    DBObject acDbObj = acTrans.GetObject(acObj, OpenMode.ForRead);
                    if (acDbObj is BlockTableRecord)
                    {
                        BlockTableRecord acBlkTblRec = (BlockTableRecord)acDbObj;
                        if (acBlkTblRec.Name.Contains("Space"))
                            acBlkTblRecSpcColl.Add(acBlkTblRec);
                    }
                }
                return acBlkTblRecSpcColl;
            }
        }
        public static BlockReference GetBlockReference(Document acDoc, string PromptMessage, string PromptRejectMessage)
        {
            Database acCurDb = acDoc.Database;
            Editor acDocEd = acDoc.Editor;
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                PromptEntityOptions pEntOpt = new PromptEntityOptions("\n" + PromptMessage);
                pEntOpt.SetRejectMessage("\n" + PromptRejectMessage);
                pEntOpt.AddAllowedClass(typeof(BlockReference), true);
                PromptEntityResult pEntRes = acDocEd.GetEntity(pEntOpt);
                if (pEntRes.Status == PromptStatus.OK)
                {
                    Entity acEnt = (Entity)acTrans.GetObject(pEntRes.ObjectId, OpenMode.ForRead);
                    BlockReference acBlkRef = (BlockReference)acEnt;
                    BlockTableRecord acBlkTblRec;
                    if (!acBlkRef.IsDynamicBlock)
                    {
                        acBlkTblRec = (BlockTableRecord)acTrans.GetObject(acBlkRef.BlockTableRecord, OpenMode.ForRead);
                    }
                    else
                    {
                        acBlkTblRec = (BlockTableRecord)acTrans.GetObject(acBlkRef.DynamicBlockTableRecord, OpenMode.ForRead);
                    }
                    Application.ShowAlertDialog("Title block was chosen: " + acBlkTblRec.Name);
                    return acBlkRef;
                }
                else
                {
                    Application.ShowAlertDialog("Bạn đã thoát lệnh!");
                    return null;
                }
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
        public static Point2d ConvertPoint3dTo2d(Point3d Point)
        {
            return new Point2d(Point.X, Point.Y);
        }
        public static Vector3d GetVectorFromInsertPoint(BlockReference KhungTen, Point3d Point)
        {
            return KhungTen.Position.GetVectorTo(Point);
        }
        public static Extents2d GetPlotArea(BlockReference KhungTen, Scale3d OriginScaleFactors, Vector3d InsertPointToBottomPoint, Vector3d InsertPointToTopPoint)
        {
            Point3d BottomPoint = new Point3d(KhungTen.Position.X + InsertPointToBottomPoint.X * KhungTen.ScaleFactors.X / OriginScaleFactors.X,
                KhungTen.Position.Y + InsertPointToBottomPoint.Y * KhungTen.ScaleFactors.Y / OriginScaleFactors.Y, 0);
            Point3d TopPoint = new Point3d(KhungTen.Position.X + InsertPointToTopPoint.X * KhungTen.ScaleFactors.X / OriginScaleFactors.X,
                KhungTen.Position.Y + InsertPointToTopPoint.Y * KhungTen.ScaleFactors.Y / OriginScaleFactors.Y, 0);
            //double ScaleX = Math.Abs(BottomPoint.X - TopPoint.X) / (841 - 32.5 * 2);
            //double ScaleY = Math.Abs(BottomPoint.Y - TopPoint.Y) / (594 - 16.5 * 2);
            //double ExtendX = 32.5 * ScaleX, ExtendY = 16.5 * ScaleY;
            //BottomPoint = new Point3d(BottomPoint.X - ExtendX, BottomPoint.Y - ExtendY, 0);
            //TopPoint = new Point3d(TopPoint.X + ExtendX, TopPoint.Y + ExtendY, 0);
            return new Extents2d(new Point2d(BottomPoint.X, BottomPoint.Y), new Point2d(TopPoint.X, TopPoint.Y));
        }
        public static int GetAttributeID(Document acDoc, BlockTableRecord KhungTen)
        {
            Database acCurDb = acDoc.Database;
            Editor acDocEd = acDoc.Editor;
            DBObjectCollection acAttDefColl = new DBObjectCollection();
            string acStrAtt = string.Empty; int i = 0;
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                foreach (ObjectId acObjId in KhungTen)
                {
                    DBObject acDbObj = acTrans.GetObject(acObjId, OpenMode.ForRead);
                    if (acDbObj is AttributeDefinition)
                    {
                        acStrAtt += "Attribute " + i.ToString() + ": " + ((AttributeDefinition)acDbObj).Prompt.ToString() + "\n";
                        i++;
                        acAttDefColl.Add(acDbObj);
                    }
                }
                Application.ShowAlertDialog(acStrAtt);
                int n = GetIntPrompt(acDoc, "Get the numberical order to the Attribute Definition expectively:",1, acAttDefColl.Count);
                Application.ShowAlertDialog("Attribute was chosen: " + ((AttributeDefinition)acAttDefColl[n]).Prompt.ToString());
                return n;
            }
        }
        public static int GetIntPrompt(Document acDoc, string Message, int Min, int Max)
        {
            Database acCurDb = acDoc.Database;
            Editor acDocEd = acDoc.Editor;
            PromptIntegerOptions pIntOpt = new PromptIntegerOptions("\n" + Message);
            pIntOpt.LowerLimit = Min;
            pIntOpt.UpperLimit = Max;
            PromptIntegerResult pIntRes = acDocEd.GetInteger(pIntOpt);
            if (pIntRes.Status == PromptStatus.OK)
                return pIntRes.Value;
            else return 0;
        }
        public static string GetAttributeString(Document acDoc, BlockReference KhungTen, int NumbericalOrder)
        {
            Database acCurDb = acDoc.Database;
            Editor acDocEd = acDoc.Editor;
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                AttributeCollection acAttColl = KhungTen.AttributeCollection;
                DBObjectCollection acAttRefColl = new DBObjectCollection();
                List<string> acStrAtt = new List<string>();
                foreach (ObjectId acObjId in acAttColl)
                {
                    DBObject acDbObj = acTrans.GetObject(acObjId, OpenMode.ForRead);
                    if (acDbObj is AttributeReference)
                    {
                        AttributeReference acAttRef = (AttributeReference)acDbObj;
                        acStrAtt.Add(acAttRef.TextString);
                    }
                }
                return acStrAtt[NumbericalOrder];
            }
        }
        public static BlockTableRecord GetBlockTableRecord(Document acDoc, string PromptMessage, string PromptRejectMessage)
        {
            Database acCurDb = acDoc.Database;
            Editor acDocEd = acDoc.Editor;
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                PromptEntityOptions pEntOpt = new PromptEntityOptions("\n" + PromptMessage);
                pEntOpt.SetRejectMessage("\n" + PromptRejectMessage);
                pEntOpt.AddAllowedClass(typeof(BlockReference), true);
                PromptEntityResult pEntRes = acDocEd.GetEntity(pEntOpt);
                if (pEntRes.Status == PromptStatus.OK)
                {
                    Entity acEnt = (Entity)acTrans.GetObject(pEntRes.ObjectId, OpenMode.ForRead);
                    BlockReference acBlkRef = (BlockReference)acEnt;
                    BlockTableRecord acBlkTblRec;
                    if (!acBlkRef.IsDynamicBlock)
                    {
                        acBlkTblRec = (BlockTableRecord)acTrans.GetObject(acBlkRef.BlockTableRecord, OpenMode.ForRead);
                    }
                    else
                    {
                        acBlkTblRec = (BlockTableRecord)acTrans.GetObject(acBlkRef.DynamicBlockTableRecord, OpenMode.ForRead);
                    }
                    Application.ShowAlertDialog("Title block was chosen: " + acBlkTblRec.Name);
                    return acBlkTblRec;
                }
                else
                {
                    Application.ShowAlertDialog("Bạn đã thoát lệnh!");
                    return null;
                }
            }
        }
        public static string DirectoryFolder(string Tenbanve)
        {
            return Tenbanve.Remove(Tenbanve.LastIndexOf(@"\") + 1);
        }
        public static string DocumentShortName(string Tenbanve)
        {
            return (Tenbanve.Remove(0, Tenbanve.LastIndexOf(@"\") + 1)).Remove(Tenbanve.Remove(0, Tenbanve.LastIndexOf(@"\") + 1).Length - 4);
        }
    }
}
