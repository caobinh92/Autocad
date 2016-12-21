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

[assembly: CommandClass(typeof(Test.SubClass))]

namespace Test
{
    class SubClass
    {
        public void ReplaceBlockReferenceWithBlockTableRecord(DocumentCollection acDocMgr,  Document acDoc, Transaction acTrans,BlockTableRecord acBlkTblRecSpc, BlockReference acBlkRefRemoved, 
            BlockTableRecord acBklTblRecReplaced, int[] acAttRefIndexCol)
        {
            BlockTableRecord acBlkTblRecReplacedClone;
            BlockTable acBlkTbl = (BlockTable) acTrans.GetObject(acDoc.Database.BlockTableId, OpenMode.ForRead);
            //BlockTableRecord acBlkTblRecReplacedClone = (BlockTableRecord)acBklTblRecReplaced.Clone();
            if (!acBlkTbl.IsWriteEnabled) acBlkTbl.UpgradeOpen();
            if (!acBlkTbl.Has(acBklTblRecReplaced.Name))
            {
                //acBlkTbl.Add(acBlkTblRecReplacedClone); acTrans.AddNewlyCreatedDBObject(acBlkTblRecReplacedClone, true);
                InsertBlock(acDocMgr, acDoc, acBklTblRecReplaced);
            }
            acBlkTblRecReplacedClone = (BlockTableRecord)acTrans.GetObject(acBlkTbl[acBklTblRecReplaced.Name], OpenMode.ForRead);
            //Application.ShowAlertDialog("Start");
            //BlockTableRecord acBlkTblRecRemoved =(BlockTableRecord) acTrans.GetObject(acBlkRefRemoved.BlockTableRecord, OpenMode.ForRead);
            Extents3d acExtRemoved = acBlkRefRemoved.GeometricExtents;
            //Extents3d extents = br.GeometricExtents;
            //Vector3d acVec3d = acBlkTblRecRemoved.Origin.GetVectorTo(acExtRemoved.MinPoint);
            //Application.ShowAlertDialog("-2");
            double acLenRemoved = Math.Abs(acExtRemoved.MinPoint.Y - acExtRemoved.MaxPoint.Y);
            //Application.ShowAlertDialog(acLenRemoved.ToString());
            //Application.ShowAlertDialog("-3");
            BlockReference acBlkRefReplacedIns = new BlockReference(new Point3d(0, 0, 0), acBlkTblRecReplacedClone.Id);
            
                //Application.ShowAlertDialog("-4");
            Extents3d acExtReplaced = acBlkRefReplacedIns.GeometricExtents;
            //acVec3d = acBklTblRecReplaced.Origin.GetVectorTo(acExtReplaced.MinPoint);
            double acLenReplaced = Math.Abs(acExtReplaced.MinPoint.Y - acExtReplaced.MaxPoint.Y);
                //Application.ShowAlertDialog(acLenReplaced.ToString());
                //Point3d acPtBot = new Point3d(acBlkRefRemoved.Position.X + acVec3d.X* acBlkRefRemoved.ScaleFactors.X, acBlkRefRemoved.Position.Y + acVec3d.Y* acBlkRefRemoved.ScaleFactors.Y, 0);
            Point3d acPtBot = acExtRemoved.MinPoint;
                Point3d acPtPos = new Point3d(acPtBot.X - acExtReplaced.MinPoint.X, acPtBot.Y - acExtReplaced.MinPoint.Y, 0);
                //Application.ShowAlertDialog("-5");
            
            string[] acAttRefValCol = GetAttributeReferenceValueCollection(acTrans, acBlkRefRemoved);
            /*
            for (int j = 0; j < acAttRefValCol.Length; j++)
            {
                Application.ShowAlertDialog(acAttRefValCol[j]);
                Application.ShowAlertDialog(acAttRefIndexCol[j].ToString());
            }
             */
            //Application.ShowAlertDialog("-6");
            BlockReference acBlkRefReplaced = new BlockReference(acPtPos, acBlkTblRecReplacedClone.Id);
            
                //Application.ShowAlertDialog("-7");
                if (!acBlkTblRecSpc.IsWriteEnabled) acBlkTblRecSpc.UpgradeOpen();
                //Application.ShowAlertDialog("1");
                acBlkTblRecSpc.AppendEntity(acBlkRefReplaced); 
                acTrans.AddNewlyCreatedDBObject(acBlkRefReplaced,true);
            
                if (!acBlkRefRemoved.IsWriteEnabled) acBlkRefRemoved.UpgradeOpen();
                acBlkRefRemoved.Erase(true);
                //Application.ShowAlertDialog("2");
                int i = 0;
                foreach (ObjectId acObjId in acBlkTblRecReplacedClone)
	            {
                    //Application.ShowAlertDialog("Index \n"+i.ToString());
		            DBObject acDbObj = acTrans.GetObject(acObjId,OpenMode.ForRead);
                    if (acDbObj is AttributeDefinition)
                    {
                        
                        //Application.ShowAlertDialog("3");
                        AttributeDefinition acAttDef = (AttributeDefinition)acDbObj;
                        if (!acAttDef.Constant)
                        {
                            using (AttributeReference acAttRef = new AttributeReference())
                            {
                                if (!acBlkTblRecReplacedClone.IsWriteEnabled) acBlkTblRecReplacedClone.UpgradeOpen();
                                if (!acBlkRefReplaced.IsWriteEnabled) acBlkRefReplaced.UpgradeOpen();
                                //Application.ShowAlertDialog("4");
                                acAttRef.SetAttributeFromBlock(acAttDef, acBlkRefReplaced.BlockTransform);
                                acAttRef.Position = acAttDef.Position.TransformBy(acBlkRefReplaced.BlockTransform);
                                //Application.ShowAlertDialog("5");
                                if (i < acAttRefIndexCol.Length)
                                {
                                    if (acAttRefIndexCol[i]< acAttRefIndexCol.Length)
                                    {
                                        acAttRef.TextString = acAttRefValCol[acAttRefIndexCol[i]];
                                    }
                                    else acAttRef.TextString = acAttDef.TextString;
                                }
                                else acAttRef.TextString = acAttDef.TextString;
                                Application.ShowAlertDialog(acAttRef.TextString);
                                //Application.ShowAlertDialog("6");
                                acBlkRefReplaced.AttributeCollection.AppendAttribute(acAttRef);
                                acTrans.AddNewlyCreatedDBObject(acAttRef, true);
                                //Application.ShowAlertDialog("7");
                                i++;
                                //Application.ShowAlertDialog("Index \n"+i.ToString());
                            }
                        }
                    }
	            }
                //Application.ShowAlertDialog("8");
                //double acScl = acLenRemoved / acLenReplaced * ((double)acBlkRefRemoved.ScaleFactors.Y);
                //acScl = acLenRemoved / acLenReplaced;
                //Application.ShowAlertDialog(acScl.ToString());
                //double acSCL1 = 1;
                acBlkRefReplaced.TransformBy(Matrix3d.Scaling(acLenRemoved/acLenReplaced /*acBlkRefRemoved.ScaleFactors.X*/, acPtBot));
                
                //tBlkRef.TransformBy(Matrix3d.Scaling(acScale, tBlkRef.Position));
                //Application.ShowAlertDialog("Finish"); 
        }

        public void InsertBlock(DocumentCollection acDocMgr, Document acDoc, BlockTableRecord acBLkTblRec)
        {
            Database acCurDb = acDoc.Database;
            ObjectIdCollection acObjIdCol = new ObjectIdCollection();
            foreach (Document acDocArr in acDocMgr)
            {
                Database acCurDbArr = acDocArr.Database;
                using (Transaction acTrans = acCurDbArr.TransactionManager.StartTransaction())
                {
                    BlockTable acBlkTbl = (BlockTable)acTrans.GetObject(acCurDbArr.BlockTableId, OpenMode.ForRead);
                    if (acBlkTbl.Has(acBLkTblRec.Name))
                    {
                        acObjIdCol.Add(acBlkTbl[acBLkTblRec.Name]);
                        IdMapping acIdMap = new IdMapping();
                        acCurDbArr.WblockCloneObjects(acObjIdCol,acCurDb.BlockTableId,acIdMap,DuplicateRecordCloning.Replace,false);
                        break;
                    }
                }
            }
        }

        public DBObjectCollection GetObjectsInBLockTableRecordSpace(Transaction acTrans, BlockTableRecord acBlkRefSpc)
        {
            DBObjectCollection acDbObjCol = new DBObjectCollection();
            foreach (ObjectId acObjID in acBlkRefSpc)
            {
                DBObject acDbObj = acTrans.GetObject(acObjID, OpenMode.ForRead);
                acDbObjCol.Add(acDbObj);
            }
            return acDbObjCol;
        }

        public string[] GetAttributeReferenceValueCollection(Transaction acTrans, BlockReference acBlkRef)
        {
            AttributeCollection acAttRefCol = acBlkRef.AttributeCollection;
            string[] acAttRefValCol = new string[acAttRefCol.Count];
            int i = 0;
            foreach (ObjectId acObjId in acAttRefCol)
            {
                AttributeReference acAttRef = (AttributeReference)acTrans.GetObject(acObjId, OpenMode.ForRead);
                acAttRefValCol[i] = acAttRef.TextString;
                i++;
            }
            return acAttRefValCol;
        }

        public void PlotTitleBlock(Document acDoc, Transaction acTrans, BlockTableRecord acBlkTblRecSpc, Extents2d PlotArea, string path, string PlotDevice, string PlotStyle, string CanonicalMediaName)
        {
            Database acCurDb = acDoc.Database;
            //string folder = DirectoryFolder(ac.Database.OriginalFileName);
            //string filename = DocumentShortName(ac.Database.OriginalFileName) + ".pdf";
            //Application.ShowAlertDialog(path);
            Application.SetSystemVariable("BACKGROUNDPLOT", 0);

            Layout acLayout = (Layout)acTrans.GetObject(acBlkTblRecSpc.LayoutId, OpenMode.ForRead);

            PlotInfo acPtInfo = new PlotInfo();
            acPtInfo.Layout = acLayout.ObjectId;

            PlotSettings acPtSet = new PlotSettings(acLayout.ModelType);
            acPtSet.CopyFrom(acLayout);
            if (!acPtSet.PlotPlotStyles) acPtSet.PlotPlotStyles = true;
            //if (!acPtSet.ScaleLineweights) acPtSet.ScaleLineweights = true;
            if (acPtSet.ShadePlotResLevel != ShadePlotResLevel.Maximum) acPtSet.ShadePlotResLevel = ShadePlotResLevel.Maximum;

            PlotSettingsValidator acPtSetVlr = PlotSettingsValidator.Current;
            //acPtSetVlr.SetPlotType(acPtSet, Autodesk.AutoCAD.DatabaseServices.PlotType.Extents);
            //Application.ShowAlertDialog(acPlDev);
            acPtSetVlr.SetPlotConfigurationName(acPtSet, PlotDevice, null);
            acPtSetVlr.RefreshLists(acPtSet);
            acPtSetVlr.SetPlotWindowArea(acPtSet, PlotArea);
            acPtSetVlr.SetPlotType(acPtSet, Autodesk.AutoCAD.DatabaseServices.PlotType.Window);
            acPtSetVlr.SetPlotWindowArea(acPtSet, PlotArea);
            acPtSetVlr.SetUseStandardScale(acPtSet, true);
            acPtSetVlr.SetStdScaleType(acPtSet, StdScaleType.ScaleToFit);
            acPtSetVlr.SetPlotCentered(acPtSet, true);
            acPtSetVlr.SetCurrentStyleSheet(acPtSet, PlotStyle);
            //acPtSetVlr.SetCurrentStyleSheet(acPtSet, "monochrome.ctb");

            //acPtSetVlr.SetPlotConfigurationName(acPtSet, "DWG To PDF.pc3", "ISO_full_bleed_A1_(594.00_x_841.00_MM)");
            //acPtSetVlr.SetPlotConfigurationName(acPtSet, "DWG To PDF.pc3", "ISO_A1_(594.00_x_841.00_MM)");
            acPtSetVlr.SetPlotConfigurationName(acPtSet, PlotDevice, CanonicalMediaName);
            Point2d acPSize = acPtSet.PlotPaperSize;
            if (acPSize.X > acPSize.Y) { acPtSetVlr.SetPlotRotation(acPtSet, PlotRotation.Degrees000); }
            else { acPtSetVlr.SetPlotRotation(acPtSet, PlotRotation.Degrees090); }

            //acPtSetVlr.SetPlotConfigurationName(acPtSet, "DWF6 ePlot.pc3", "ANSI_A_(8.50_x_11.00_Inches)");
            acPtInfo.OverrideSettings = acPtSet;
            LayoutManager.Current.CurrentLayout = acLayout.LayoutName;

            //IAcSmEnumComponent  

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
                        //acPtProgDlg.set_PlotMsgString(PlotMessageIndex.SheetName, "abc");
                        
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
                        //acPtEng.BeginPage(
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

        public string[,] GetAttributeDefinitionNameAllCollection(DocumentCollection acDocMgr, DBObjectCollection acBlkTblRecCol)
        {
            string[,] acAttDefNamAllCol = new string[acBlkTblRecCol.Count, 50];
            for (int i = 0; i < acBlkTblRecCol.Count; i++)
            {
                    foreach (Document acDoc in acDocMgr)
                    {
                        Database acCurDb = acDoc.Database;
                        using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                        {
                            BlockTable acBlkTbl = (BlockTable)acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead);
                            if (acBlkTbl.Has(((BlockTableRecord) acBlkTblRecCol[i]).Name))
                            {
                                //BlockTableRecord acBlkTblRec = (BlockTableRecord) acTrans.GetObject(acBlkTbl[((BlockTableRecord)acBlkTblRecCol[i]).Name], OpenMode.ForRead);
                                string[] acAttDefNam = GetAttributeDefinitionNameCollection(acDoc, acTrans,(BlockTableRecord) acBlkTblRecCol[i]);
                                for (int j = 0; j < acAttDefNam.Length; j++)
                                {
                                    acAttDefNamAllCol[i, j] = acAttDefNam[j];
                                }
                            }
                        }
                    }
                
            }
            return acAttDefNamAllCol;
        }

        public string[] GetBlockTableRecordNameCollection(DocumentCollection acDocMgr)
        {
            DBObjectCollection acBlkTblRecCol = GetBlockTableRecordCollection(acDocMgr);
            string[] acBlkTblRecNamCol = new string[acBlkTblRecCol.Count];
            for (int i = 0; i < acBlkTblRecCol.Count; i++)
            {
                acBlkTblRecNamCol[i] = ((BlockTableRecord)acBlkTblRecCol[i]).Name;
            }
            return acBlkTblRecNamCol;
        }

        public DBObjectCollection GetBlockTableRecordCollection(DocumentCollection acDocMgr)
        {
            DBObjectCollection acBlkTblRecCol = new DBObjectCollection();
            foreach (Document acDoc in acDocMgr)
            {
                Database acCurDb = acDoc.Database;
                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    BlockTable acBlkTbl =(BlockTable) acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead);
                    foreach (ObjectId acObjId in acBlkTbl)
                    {
                        DBObject acDbObj = acTrans.GetObject(acObjId, OpenMode.ForRead);
                        if (acDbObj is BlockTableRecord)
                        {
                            BlockTableRecord acBlkTblRec = (BlockTableRecord)acDbObj;
                            if (!acBlkTblRec.IsLayout)
                            {
                                acBlkTblRecCol.Add(acBlkTblRec);
                            }
                        }
                    }
                }
            }
            DBObjectCollection acBlkTblRecFilCol = new DBObjectCollection();
            for (int i = 0; i < acBlkTblRecCol.Count; i++)
            {
                //bool IsCheckedOk = true;
                int count = 0;
                for (int j = i; j < acBlkTblRecCol.Count; j++)
                {
                    if (((BlockTableRecord)acBlkTblRecCol[i]).Name.Equals(((BlockTableRecord)acBlkTblRecCol[j]).Name))
                        count++;
                }
                if (count<2) acBlkTblRecFilCol.Add(acBlkTblRecCol[i]);
            }
            return acBlkTblRecFilCol;
        }

        public string GetAttributeReferenceText(Document acDoc,Transaction acTrans, BlockReference acBlkRef, int SelectedIndex)
        {
            Database acCurDb = acDoc.Database;
            Editor acDocEd = acDoc.Editor;
            AttributeCollection acAttCol = acBlkRef.AttributeCollection;
            DBObjectCollection acAttRefCol = new DBObjectCollection();
            List<string> acStrAtt = new List<string>();
            foreach (ObjectId acObjId in acAttCol)
            {
                DBObject acDbObj = acTrans.GetObject(acObjId, OpenMode.ForRead);
                if (acDbObj is AttributeReference)
                {
                    AttributeReference acAttRef = (AttributeReference)acDbObj;
                    acStrAtt.Add(acAttRef.TextString);
                }
            }
            return acStrAtt[SelectedIndex];
        }

        public DBObject GetTextInsideExtents3d( Document acDoc,Transaction acTrans,  Extents3d acExt3d)
        {
            Editor acDocEd = acDoc.Editor;
            TypedValue[] acTypValAr = new TypedValue[4];
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Operator, "<OR"), 0);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "TEXT"), 1);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Start, "MTEXT"), 2);
            acTypValAr.SetValue(new TypedValue((int)DxfCode.Operator, "OR>"), 3);
            SelectionFilter acSFil = new SelectionFilter(acTypValAr);
            SelectionSet acSSet;
            Matrix3d xform = acDocEd.CurrentUserCoordinateSystem.Inverse();
            Point3d acPt1 = acExt3d.MinPoint.TransformBy(xform);
            Point3d acPt2 = acExt3d.MaxPoint.TransformBy(xform);
            dynamic acadApp = Application.AcadApplication;
            //Point3d acPt1 = new Point3d(acExt3d.MinPoint.X , acExt3d.MinPoint.Y , 0);
            //Point3d acPt2 = new Point3d(acExt3d.MaxPoint.X, acExt3d.MaxPoint.Y, 0);
            //PromptSelectionResult acPSRes = acDocEd.SelectCrossingWindow(acPt1, acPt2, acSFil);
            acadApp.ZoomExtents();
            PromptSelectionResult acPSRes = acDocEd.SelectWindow(acPt1, acPt2, acSFil);
            acadApp.ZoomPrevious();
            if (acPSRes.Status == PromptStatus.OK)
            {
                acSSet = acPSRes.Value;
                if (acSSet.Count == 1)
                {
                    DBObject acDbObj = null;
                    foreach (ObjectId item in acSSet.GetObjectIds())
                    {
                        acDbObj = acTrans.GetObject(item, OpenMode.ForRead);
                    }
                    return acDbObj;
                }
                else return null;
            }
            else return null;
        }

        public bool IsPoint3dInsideExtents3d(Extents3d acExt3d, Point3d acPt3d)
        {
            double acLenFulX = Math.Abs(acExt3d.MinPoint.X - acExt3d.MaxPoint.X);
            double acLenFulY = Math.Abs(acExt3d.MinPoint.Y - acExt3d.MaxPoint.Y);
            double acLenPtToMinX = Math.Abs(acExt3d.MinPoint.X - acPt3d.X);
            double acLenPtToMinY = Math.Abs(acExt3d.MinPoint.Y - acPt3d.Y);
            double acLenPtToMaxX = Math.Abs(acExt3d.MaxPoint.X - acPt3d.X);
            double acLenPtToMaxY = Math.Abs(acExt3d.MaxPoint.Y - acPt3d.Y);
            if ((acLenPtToMinX <= acLenFulX) && (acLenPtToMaxX <= acLenFulX) && (acLenPtToMinY <= acLenFulY) && (acLenPtToMaxY <= acLenFulY)) return true;
            else return false;
        }

        public Extents3d[] GetExtents3dBlockReferenceCollection(DocumentCollection acDocMgr, string BlockName, Vector3d[] acVec3dOrgArr, Scale3d acScl3dOrg)
        {
            DBObjectCollection acBlkRefCol = GetBlockReferenceCollectionByName(acDocMgr, BlockName);
            Extents3d[] acExt3dCol = new Extents3d[acBlkRefCol.Count];
            for (int i = 0; i < acBlkRefCol.Count; i++)
            {
                acExt3dCol[i] =GetExtents3dFromInsertPointOfBlockReferenceAndTwoVectorsWithScale((BlockReference) acBlkRefCol[i],acVec3dOrgArr,acScl3dOrg);
            }
            return acExt3dCol;
        }

        public Extents3d GetExtents3dFromInsertPointOfBlockReferenceAndTwoVectorsWithScale(BlockReference acBlkRef, Vector3d[] acVec3dOrgArr, Scale3d acScl3dOrg)
        {
            Point3d acPtIns = acBlkRef.Position;
            //Application.ShowAlertDialog(acPtIns.ToString());
            //Application.ShowAlertDialog(acScl3dOrg.X.ToString());
            //Application.ShowAlertDialog("Insert Point: \n" + acPtIns.ToString() + "\nScale: \n" + acBlkRef.ScaleFactors.X.ToString() + "\nVector 0: \n" + acVec3dOrgArr[0].ToString());
            Point3d acPt1 =new Point3d (acPtIns.X + acVec3dOrgArr[0].X * acBlkRef.ScaleFactors.X/ acScl3dOrg.X,  acPtIns.Y + acVec3dOrgArr[0].Y * acBlkRef.ScaleFactors.Y/ acScl3dOrg.Y,0);
            Point3d acPt2 = new Point3d(acPtIns.X + acVec3dOrgArr[1].X * acBlkRef.ScaleFactors.X / acScl3dOrg.X, acPtIns.Y + acVec3dOrgArr[1].Y * acBlkRef.ScaleFactors.Y / acScl3dOrg.Y, 0);
            Extents3d acExt3d = new Extents3d(acPt1, acPt2);
            //Application.ShowAlertDialog(acExt3d.ToString());
            return acExt3d;
        }

        public DBObjectCollection GetBlockReferenceCollectionByName(DocumentCollection acDocMgr, string BlockName)
        {
            DBObjectCollection acBlkRefCol = new DBObjectCollection();
            foreach (Document acDoc in acDocMgr)
	        {
		        Database acCurDb = acDoc.Database;
                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    BlockTable acBlkTbl = (BlockTable) acTrans.GetObject(acCurDb.BlockTableId,OpenMode.ForRead);
                    DBObjectCollection acBlkTblRecSpcCol = GetBlockTableRecordSpaceCollection(acDoc,acTrans);
                    for (int i = 0; i < acBlkTblRecSpcCol.Count; i++)
			        {
			            BlockTableRecord acBlkTblRecSpc = (BlockTableRecord) acBlkTblRecSpcCol[i];
                        foreach (ObjectId acObjId in acBlkTblRecSpc)
	                    {
		                    Entity acEnt = (Entity) acTrans.GetObject(acObjId,OpenMode.ForRead);
                            if (acEnt is BlockReference)
                            {
                                if (((BlockReference)acEnt).Name.Equals(BlockName))
                                {
                                    acBlkRefCol.Add(acEnt);
                                    //Application.ShowAlertDialog(acEnt.ObjectId.ToString());
                                }
                            }
	                    }
			        }
                }
	        }
            return acBlkRefCol;
        }

        public DBObjectCollection GetBlockTableRecordSpaceCollection(Document acDoc, Transaction acTrans)
        {
            Database acCurDb = acDoc.Database;
            BlockTable acBlkTbl = (BlockTable)acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead);
            DBObjectCollection acBlkTblRecSpcColl = new DBObjectCollection();
            foreach (ObjectId acObj in acBlkTbl)
            {
                DBObject acDbObj = acTrans.GetObject(acObj, OpenMode.ForRead);
                if (acDbObj is BlockTableRecord)
                {
                    BlockTableRecord acBlkTblRec = (BlockTableRecord)acDbObj;
                    if (acBlkTblRec.IsLayout)
                    {
                        acBlkTblRecSpcColl.Add(acBlkTblRec);
                    }
                }
            }
            return acBlkTblRecSpcColl;
        }

        public string[] GetPlotDevice(Document acDoc)
        {
            Database acCurDb = acDoc.Database;
            PlotSettingsValidator acPlSetVdr = PlotSettingsValidator.Current;
            StringCollection acPlDevCol = acPlSetVdr.GetPlotDeviceList();
            string[] acStrPlDevCol = new string[acPlDevCol.Count];
            for (int k = 0; k < acPlDevCol.Count; k++)
            {
                acStrPlDevCol[k] = acPlDevCol[k];
            }
            return acStrPlDevCol;
        }

        public string[,] GetCanonicalMediaName(Document acDoc)
        {
            Database acCurDb = acDoc.Database;
            PlotSettingsValidator acPlSetVdr = PlotSettingsValidator.Current;
            string[] acStrPlDevCol=GetPlotDevice(acDoc);
            string[,] acStrCanMedCol = new string[acStrPlDevCol.Length, 400];
            for (int k = 0; k < acStrPlDevCol.Length; k++)
            {
                using (PlotSettings acPlSet = new PlotSettings(true))
                {
                    bool checkOk = true;
                    try { acPlSetVdr.SetPlotConfigurationName(acPlSet, acStrPlDevCol[k], null); }
                    catch (Autodesk.AutoCAD.Runtime.Exception e) { string Error = e.Message; checkOk = false; }
                    if (!checkOk) continue;
                    acPlSetVdr.RefreshLists(acPlSet);
                    StringCollection acCanMedCol = acPlSetVdr.GetCanonicalMediaNameList(acPlSet);
                    for (int l = 0; l < acCanMedCol.Count; l++)
                    {
                        acStrCanMedCol[k, l] = acCanMedCol[l];
                    }
                }
            }
            return acStrCanMedCol;
        }

        public string[] GetPlotStyle(Document acDoc)
        {
            Database acCurDb = acDoc.Database;
            PlotSettingsValidator acPlSetVdr = PlotSettingsValidator.Current;
            StringCollection acPlStlCol = acPlSetVdr.GetPlotStyleSheetList();
            string[] acStrPlStlCol = new string[acPlStlCol.Count];
            for (int k = 0; k < acPlStlCol.Count; k++)
            {
                acStrPlStlCol[k] = acPlStlCol[k];
            }
            return acStrPlStlCol;
        }

        public string[] GetAttributeDefinitionNameCollection(Document acDoc,Transaction acTrans, BlockTableRecord acBlkTblRec)
        {
            DBObjectCollection acAttDefCol = GetAttributeDefinitionCollection(acDoc,acTrans, acBlkTblRec);
            string[] acAttDefNamCol = new string[acAttDefCol.Count];
            for (int i = 0; i < acAttDefCol.Count; i++)
            {
                acAttDefNamCol[i] = ((AttributeDefinition)acAttDefCol[i]).Prompt.ToString();
            }
            return acAttDefNamCol;
        }

        public DBObjectCollection GetAttributeDefinitionCollection(Document acDoc,Transaction acTrans, BlockTableRecord acBlkTblRec)
        {
            Database acCurDb = acDoc.Database;
            DBObjectCollection acAttDefCol = new DBObjectCollection();
            string acStrAtt = string.Empty;
            //if (Application.DocumentManager.MdiActiveDocument != acDoc) Application.DocumentManager.MdiActiveDocument = acDoc;
            //HostApplicationServices.WorkingDatabase = acDoc.Database;
            //using (DocumentLock acDocLck = acDoc.LockDocument())
            //{

            //using (Transaction acTrans = acDoc.Database.TransactionManager.StartTransaction())
            //{
            BlockTable acBlkTbl = (BlockTable)acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead);
            if (acBlkTbl.Has(acBlkTblRec.Name))
            {
                BlockTableRecord acBlkTblRec2 = (BlockTableRecord)acTrans.GetObject(acBlkTbl[acBlkTblRec.Name], OpenMode.ForRead);
                foreach (ObjectId acObjId in acBlkTblRec2)
                {
                    DBObject acDbObj = acTrans.GetObject(acObjId, OpenMode.ForRead);
                    if (acDbObj is AttributeDefinition)
                    {
                        acAttDefCol.Add(acDbObj);
                    }
                }
                //}
                //}
            }
            return acAttDefCol;
        }

        public BlockReference GetBlockReference(Document acDoc,Transaction acTrans, string PromptMessage, string PromptRejectMessage)
        {
            Database acCurDb = acDoc.Database;
            Editor acDocEd = acDoc.Editor;
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
                return acBlkRef;
            }
            else
            {
                Application.ShowAlertDialog("Bạn đã thoát lệnh!");
                return null;
            }
        }

        public Point3d GetPoint3dFromPrompt(Document acDoc, string TextPrompt)
        {
            Editor acDocEd = acDoc.Editor;
            PromptPointOptions pPtOpt = new PromptPointOptions("\n" + TextPrompt);
            PromptPointResult pPtRes = acDocEd.GetPoint(pPtOpt);
            if (pPtRes.Status == PromptStatus.OK)
            {
                return pPtRes.Value;
            }
            else
            {
                Application.ShowAlertDialog("Bạn đã thoát lệnh!");
                return new Point3d(0, 0, 0);
            }
        }

        // Get two vectors 3d from insert point of block reference to two points 3d defined a extents3d
        public Vector3d[] GetTwoVectors3dDefinedExtents3d(BlockReference acBlkRef, Point3d acPt1, Point3d acPt2, double Scale)
        {
            Vector3d acVec1 = new Vector3d(((acBlkRef.Position.GetVectorTo(acPt1)).X) *Scale, ((acBlkRef.Position.GetVectorTo(acPt1)).Y) *Scale, 0);
            Vector3d acVec2 = new Vector3d(((acBlkRef.Position.GetVectorTo(acPt2)).X) *Scale, ((acBlkRef.Position.GetVectorTo(acPt2)).Y) *Scale, 0);
            return new Vector3d[] {acVec1,acVec2};
        }

        public string[] GetAllDocumentNameInDocumentCollection(DocumentCollection acDocMgr)
        {
            string[] acDocNamCol = new string[acDocMgr.Count];
            int i = 0;
            foreach (Document item in acDocMgr)
            {
                acDocNamCol[i] = DocumentShortName(item.Database.OriginalFileName);
                i++;
            }
            return acDocNamCol;
        }

        public string DocumentShortName(string DocumentName)
        {
            return (DocumentName.Remove(0, DocumentName.LastIndexOf(@"\") + 1)).Remove(DocumentName.Remove(0, DocumentName.LastIndexOf(@"\") + 1).Length - 4);
        }

        public string DirectoryFolder(string DocumentName)
        {
            return DocumentName.Remove(DocumentName.LastIndexOf(@"\") + 1);
        }
        
    }
}
