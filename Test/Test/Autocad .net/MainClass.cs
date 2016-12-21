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

[assembly: CommandClass(typeof(Test.MainClass))]

namespace Test
{
    class MainClass
    {
        public InputForm acFrm;
        public BlockReference acBlkRef;
        public Vector3d[] acVec3dOrgArr;
        public Scale3d acScl3dOrg;
        public Vector3d[] acTxtVec3dOrgArr;
        public BlockTableRecord acBlkTblRec;
        public Layout acLayoutCur;

        [CommandMethod("UserPlot", CommandFlags.Session)]
        public void UserPlot()
        {
            // Step 1: CREATE INSTANCE OF SUBCLASS
            SubClass acSC = new SubClass();

            
            // Step 2: GET ALL DOCUMENT NAME OPENED
            DocumentCollection acDocMgr = Application.DocumentManager;
            string[] acDocNamCol = acSC.GetAllDocumentNameInDocumentCollection(acDocMgr);

            // Step 3: GET ACTIVE DOCUMENT, DATABASE AND EDITOR, START A TRANSACTION
            Document acDoc = acDocMgr.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            Editor acDocEd = acDoc.Editor;
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Step 4: GET BLOCKTABLE AND BLOCKTABLERECORD LAYOUT
                BlockTable acBlkTbl =(BlockTable) acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead);  //BlockTable
                DBObjectCollection acBlkTblRecSpcCol = acSC.GetBlockTableRecordSpaceCollection(acDoc, acTrans);  //BlockTableRecordSpaceCollection

                // Step 5: SHOW INPUT FORM, IF NULL : ADD PLOT DEVICE, CANONICAL MEDIA NAME, PLOT STYLE
                if (acFrm == null)
                {
                    acFrm = new InputForm();  //Create new form
                    acFrm.PlotDeviceCollection = acSC.GetPlotDevice(acDoc);  //Plot Device
                    acFrm.CanonicalMediaNameCollection = acSC.GetCanonicalMediaName(acDoc);  //Canonical Media Name
                    acFrm.PlotStyleCollection = acSC.GetPlotStyle(acDoc);  //Plot Style
                    acFrm.CreatePlotDeviceCollection();  //Create ComboBox Plot Device
                    for (int i = 0; i < acFrm.PlotDeviceCollection.Length; i++) 
                    {
                        if (acFrm.PlotDeviceCollection[i].Equals("DWG To PDF.pc3"))  acFrm.cbbPlDev.SelectedIndex = i;  //Set default Plot Device
                    }
                    acFrm.CreateCanonicalMediaNameCollection();  //Create ComboBox Canonical Media Name
                    /*
                    for (int i = 0; i < acFrm.CanonicalMediaNameCollection.GetLength(1); i++)
                    {
                        if ((acFrm.CanonicalMediaNameCollection[acFrm.cbbPlDev.SelectedIndex, i]).Equals("ISO_full_bleed_A1_(841.00_x_594.00_MM)")) acFrm.cbbCanMed.SelectedIndex = i; // Set default Paper Size
                    }
                    */
                    acFrm.CreatePlotStyleCollection();  //Create ComboBox Plot Style
                    for (int i = 0; i < acFrm.PlotStyleCollection.Length; i++)
                    {
                        if (acFrm.PlotStyleCollection[i].Equals("monochrome.ctb")) acFrm.cbbPlStl.SelectedIndex = i;  //Set default Plot Style
                    }
                    acFrm.btnClose.Enabled = false;
                }

                //Step 6: ADD DOCUMENT NAME
                acFrm.DocumentNameCollection = acDocNamCol;  //Document Name Collection
                acFrm.CreateDocumentNameCollection();  // Create Document Name List
                acFrm.IsButtonOkClicked = false; acFrm.IsButtonCancelClicked = false; acFrm.IsClosed = false;
                acFrm.IsButtonBlockReferenceClicked = false; acFrm.IsButtonTextPromptClicked = false;
                acFrm.ShowDialog();

                // Step ...
                
                while (acFrm.IsClosed==false)
                //do
                {
                    //if (acFrm.IsClosed == true) goto CheckPoint;

                    // Step 7: CHECK IF BUTTON QUERY BLOCK REFERENCE CLICKED -> TRUE : ADD BLOCKREFERENCE (NAME, TWOVECTORS, SCALE, NAME) AND BLOCKRECORDTABLE (ATTRIBUTEDEFINITION NAME COLLECTION)
                    if (acFrm.IsButtonBlockReferenceClicked)
                    {
                        acBlkRef = acSC.GetBlockReference(acDoc, acTrans, "Pick a title block", " Pick a title block");  //BlockReferece Origin
                        if (acBlkRef != null)
                        {
                            acBlkTblRec = (BlockTableRecord)acTrans.GetObject(acBlkRef.BlockTableRecord, OpenMode.ForRead); //BlockTableRecord
                            Point3d acPt1 = acSC.GetPoint3dFromPrompt(acDoc, "Pick first point");
                            if (acPt1.Z !=50)
                            {
                                Point3d acPt2 = acSC.GetPoint3dFromPrompt(acDoc, "Pick second point");  //TwoPoints
                                if (acPt2.Z != 50) 
                                {
                                    acVec3dOrgArr = acSC.GetTwoVectors3dDefinedExtents3d(acBlkRef, acPt1, acPt2, 1);  //TwoVectors Orgin
                                    acScl3dOrg = acBlkRef.ScaleFactors;  //Scale Origin
                                    //acBlkRefCol = acSC.GetBlockReferenceCollectionByName(acDocMgr, acBlkRef.Name);  //Get All Block Reference In Document Collection
                                    //Application.ShowAlertDialog(acBlkRefCol.Count.ToString());
                                    //acExt3dCol = acSC.GetExtents3dBlockReferenceCollection(acDocMgr, acBlkRef.Name, acVec3dOrgArr, acScl3dOrg); //Get All Extent3d In Document Collection
                                    //Application.ShowAlertDialog(acExt3dCol.Length.ToString());
                                    //DBText acDbTxt; acDbTxt.

                                    acFrm.txtBlkTblRecNam.Text = acBlkRef.Name;  //Name
                                    //DBObjectCollection acBlkTblRecCol = acSC.GetBlockTableRecordCollection(acDocMgr);
                                    //acFrm.AttributeDefinitionCollection = acSC.GetAttributeDefinitionNameAllCollection(acDocMgr, acBlkTblRecCol);
                                    acFrm.AttributeDefinitionCollection = acSC.GetAttributeDefinitionNameCollection(acDoc, acTrans, acBlkTblRec);
                                    //acFrm.AttributeDefinitionCollection = acSC.GetAttributeDefinitionNameCollection(acDoc,acTrans, acBlkTblRec);  //AttributeDefinition Collection
                                    acFrm.CreateAttributeDefinitionCollection(); //Create ComboBoxAttributeDefinition
                                    if (acFrm.cbbAttDef.Items.Count != 0)
                                    {
                                        acFrm.cbbAttDef.Enabled = true; acFrm.rbtAttribute.Enabled = true;
                                    }
                                    else { acFrm.cbbAttDef.Enabled = false; acFrm.rbtAttribute.Enabled = false; }
                                    acFrm.btnClose.Enabled = true;
                                }
                            }
                        }
                        acFrm.IsButtonBlockReferenceClicked = false;
                        //acFrm.ShowDialog();
                    }

                    // Step 8: CHECK IF BUTTON QUERY TEXT PROMPT CLICKED -> TRUE: ADD TEXT 
                    if (acFrm.IsButtonTextPromptClicked)
                    {
                        Point3d acPt1 = acSC.GetPoint3dFromPrompt(acDoc, "Pick first point");
                        if (acPt1.Z != 50)
                        {
                            Point3d acPt2 = acSC.GetPoint3dFromPrompt(acDoc, "Pick second point");  //TwoPoints
                            if (acPt2.Z != 50) 
                            {
                                //Application.ShowAlertDialog("Point 1: /n"+acPt1.ToString()+"Point 2: /n" + acPt2.ToString());
                                //Extents3d abc = new Extents3d(acPt1, acPt2);
                                //Application.ShowAlertDialog("OK");
                                BlockTableRecord acBlkTblRecSpcCur = (BlockTableRecord)acTrans.GetObject(acCurDb.CurrentSpaceId, OpenMode.ForRead); //Current BlockTableRecord Layout
                                //acLayoutCur = (Layout)acTrans.GetObject(acBlkTblRecSpcCur.LayoutId, OpenMode.ForRead);
                                bool IsCheckedOk = false;
                                foreach (ObjectId item in acBlkTblRecSpcCur)
                                {
                                    Entity acEnt = (Entity)acTrans.GetObject(item, OpenMode.ForRead);
                                    if (acEnt is BlockReference)
                                    {
                                        BlockReference acBlkRefArr = (BlockReference)acEnt;
                                        if (acBlkRefArr.Name.Equals(acFrm.txtBlkTblRecNam.Text))
                                        {
                                            Extents3d acExt3dArr = acSC.GetExtents3dFromInsertPointOfBlockReferenceAndTwoVectorsWithScale(acBlkRefArr, acVec3dOrgArr, acScl3dOrg);
                                            //Application.ShowAlertDialog(acExt3dArr.ToString());
                                            //Application.ShowAlertDialog("OK");
                                            if (acSC.IsPoint3dInsideExtents3d(acExt3dArr, acPt1) || (acSC.IsPoint3dInsideExtents3d(acExt3dArr, acPt2)))
                                            {
                                                //Application.ShowAlertDialog("true");
                                                IsCheckedOk = true;
                                                acTxtVec3dOrgArr = acSC.GetTwoVectors3dDefinedExtents3d(acBlkRefArr, acPt1, acPt2, acScl3dOrg.X / acBlkRefArr.ScaleFactors.X);
                                            }
                                            //else Application.ShowAlertDialog("false");
                                        }
                                        //
                                    }
                                    //if (IsCheckedOk) break;
                                }
                                if (IsCheckedOk)
                                {
                                    Extents3d acTxtExt3d = new Extents3d(acPt1, acPt2);
                                    DBObject acDbObj = acSC.GetTextInsideExtents3d(acDoc, acTrans, acTxtExt3d);
                                    if ((acDbObj is DBText) || (acDbObj is MText))
                                    {
                                        //if (acDbObj is DBText) Application.ShowAlertDialog(((DBText)acDbObj).TextString);
                                        //if (acDbObj is MText) Application.ShowAlertDialog(((MText)acDbObj).Contents);
                                        acFrm.txtTextPromptValidator.Text = acFrm.IsTextPromptInvalid[1];
                                        if (acFrm.rbtTextPrompt.Enabled == false) acFrm.rbtTextPrompt.Enabled = true;
                                    }
                                    else
                                    {
                                        Application.ShowAlertDialog("No Text was picked!");
                                        acFrm.txtTextPromptValidator.Text = acFrm.IsTextPromptInvalid[0];
                                        if (acFrm.rbtTextPrompt.Enabled == true) acFrm.rbtTextPrompt.Enabled = false;
                                        if (acFrm.rbtTextPrompt.Checked == true) acFrm.rbtIndex.Checked = true;
                                    }

                                }
                                else
                                {
                                    Application.ShowAlertDialog("Windows Area was not inside a Block Name!");
                                    acFrm.txtTextPromptValidator.Text = acFrm.IsTextPromptInvalid[0];
                                    if (acFrm.rbtTextPrompt.Enabled == true) acFrm.rbtTextPrompt.Enabled = false;
                                    if (acFrm.rbtTextPrompt.Checked == true) acFrm.rbtIndex.Checked = true;
                                }
                                //acFrm.ShowDialog();
                            }
                        }
                        acFrm.IsButtonTextPromptClicked = false;
                    }
                    acFrm.ShowDialog();
                }
                //while (acFrm.IsClosed == false);
                acTrans.Commit();
            }
            
            //CheckPoint:

            if (acFrm.IsButtonOkClicked)
            {
                foreach (Document acDoc1 in acDocMgr)
                {
                    //Application.ShowAlertDialog("");
                    bool check = false;
                    foreach (string item in acFrm.DocumentNameCollection)
                    {
                        if (item.Equals(acSC.DocumentShortName(acDoc1.Database.OriginalFileName)))
                            check = true;
                        //Application.ShowAlertDialog(item + "/ncompare to /n" + DocumentShortName(acDoc1.Database.OriginalFileName));
                    }
                    //Application.ShowAlertDialog(check.ToString());
                    if (check)
                    {
                        if (acDocMgr.MdiActiveDocument != acDoc1)
                        {
                            acDocMgr.MdiActiveDocument = acDoc1;
                        }
                        acCurDb = acDoc1.Database;
                        HostApplicationServices.WorkingDatabase = acCurDb;
                        acDocEd = acDoc1.Editor;
                        using (DocumentLock acDocLck1 = acDocMgr.MdiActiveDocument.LockDocument())
                        {
                            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                            {
                                int CountSheet = 1;
                                DBObjectCollection acBlkTblRecSpcCol = acSC.GetBlockTableRecordSpaceCollection(acDoc1, acTrans);
                                foreach (DBObject acDbBlkTblRec in acBlkTblRecSpcCol)
                                {
                                    BlockTableRecord acBlkTblRecSpc = (BlockTableRecord)acDbBlkTblRec;
                                    Layout acLayout = (Layout)acTrans.GetObject(acBlkTblRecSpc.LayoutId, OpenMode.ForRead);
                                    //LayoutManager.Current.layoutcu
                                    LayoutManager.Current.CurrentLayout = acLayout.LayoutName;
                                    foreach (ObjectId acObjId in acBlkTblRecSpc)
                                    {
                                        Entity acEnt = (Entity)acTrans.GetObject(acObjId, OpenMode.ForRead);
                                        if (acEnt is BlockReference)
                                        {
                                            BlockReference acBlkRef1 = (BlockReference)acEnt;
                                            if (acBlkRef1.Name.Equals(acFrm.txtBlkTblRecNam.Text))
                                            {
                                                string name = "";
                                                string path = "";
                                                Extents3d acExt3d = acSC.GetExtents3dFromInsertPointOfBlockReferenceAndTwoVectorsWithScale(acBlkRef1, acVec3dOrgArr, acScl3dOrg);
                                                Extents2d acExt2d = new Extents2d(new Point2d(acExt3d.MinPoint.X, acExt3d.MinPoint.Y), new Point2d(acExt3d.MaxPoint.X, acExt3d.MaxPoint.Y));

                                                /*
                                                //Line acLine = new Line(acExt3d.MinPoint, acExt3d.MaxPoint); acLine.SetDatabaseDefaults();
                                                Line acLine = new Line(new Point3d(acExt2d.MinPoint.X, acExt2d.MinPoint.Y, 0), new Point3d(acExt2d.MaxPoint.X, acExt2d.MaxPoint.Y, 0));
                                                acBlkTblRecSpc.UpgradeOpen(); acBlkTblRecSpc.AppendEntity(acLine); acTrans.AddNewlyCreatedDBObject(acLine, true);
                                                acBlkTblRecSpc.DowngradeOpen();
                                                */

                                                if (acFrm.rbtAttribute.Checked)
                                                {
                                                    name = acSC.DocumentShortName(acDoc1.Database.OriginalFileName) + " - " + acSC.GetAttributeReferenceText(acDoc1, acTrans, acBlkRef1, acFrm.cbbAttDef.SelectedIndex);
                                                    path = Path.Combine(acSC.DirectoryFolder(acDoc1.Database.OriginalFileName), Path.GetFileName(acSC.DocumentShortName(acDoc1.Database.OriginalFileName))) + " - " + acSC.GetAttributeReferenceText(acDoc1, acTrans, acBlkRef1, acFrm.cbbAttDef.SelectedIndex);
                                                }
                                                if (acFrm.rbtIndex.Checked)
                                                {
                                                    name = acSC.DocumentShortName(acDoc1.Database.OriginalFileName) + " - " + CountSheet.ToString();
                                                    path = Path.Combine(acSC.DirectoryFolder(acDoc1.Database.OriginalFileName), Path.GetFileName(acSC.DocumentShortName(acDoc1.Database.OriginalFileName) + " - " + CountSheet.ToString()));
                                                    CountSheet++;
                                                }
                                                if (acFrm.rbtTextPrompt.Checked)
                                                {
                                                    Extents3d acTxtExt3d = acSC.GetExtents3dFromInsertPointOfBlockReferenceAndTwoVectorsWithScale(acBlkRef1, acTxtVec3dOrgArr, acScl3dOrg);

                                                    /*
                                                    Line acLine = new Line(acTxtExt3d.MinPoint, acTxtExt3d.MaxPoint); acLine.SetDatabaseDefaults();
                                                    acBlkTblRecSpc.UpgradeOpen(); acBlkTblRecSpc.AppendEntity(acLine); acTrans.AddNewlyCreatedDBObject(acLine, true);
                                                    acBlkTblRecSpc.DowngradeOpen();
                                                    */

                                                    DBObject acDbObj = acSC.GetTextInsideExtents3d(acDoc1, acTrans, acTxtExt3d);
                                                    string txt = "";
                                                    if (acDbObj is DBText)
                                                    {
                                                        //Application.ShowAlertDialog(((DBText)acDbObj).TextString);
                                                        txt = ((DBText)acDbObj).TextString;
                                                    }
                                                    if (acDbObj is MText)
                                                    {
                                                        //Application.ShowAlertDialog(((MText)acDbObj).Contents);
                                                        txt = ((MText)acDbObj).Contents;
                                                    }
                                                    if (acDbObj == null) txt = "";
                                                    name = acSC.DocumentShortName(acDoc1.Database.OriginalFileName) + " - " + txt;
                                                    path = Path.Combine(acSC.DirectoryFolder(acDoc1.Database.OriginalFileName), Path.GetFileName(acSC.DocumentShortName(acDoc1.Database.OriginalFileName) + " - " + txt)); 
                                                }
                                                
                                                //string OrgLayoutName = acLayout.LayoutName;
                                                //LayoutManager.Current.RenameLayout(OrgLayoutName, name);
                                                acSC.PlotTitleBlock(acDoc1, acTrans, acBlkTblRecSpc, acExt2d, path, acFrm.cbbPlDev.SelectedItem.ToString(), acFrm.cbbPlStl.SelectedItem.ToString(), acFrm.cbbCanMed.SelectedItem.ToString());
                                                //LayoutManager.Current.RenameLayout(name, OrgLayoutName);
                                                
                                                if (File.Exists(@path))
                                                {
                                                    if (("DWF6 ePlot.pc3").Equals(acFrm.cbbPlDev.SelectedItem.ToString()))
                                                    {
                                                        string path4 = path+".dwf";
                                                        File.Move(@path, @path4);
                                                    }
                                                    if (("DWG To PDF.pc3").Equals(acFrm.cbbPlDev.SelectedItem.ToString()))
                                                    {
                                                        string path4 = path + ".pdf";
                                                        File.Move(@path, @path4);
                                                    }
                                                }
                                                string path2 = path + ".plt";
                                                if (File.Exists(@path2)) 
                                                {
                                                    if (("DWF6 ePlot.pc3").Equals(acFrm.cbbPlDev.SelectedItem.ToString()))
                                                    {
                                                        string path3 = path + ".dwf";
                                                        if (File.Exists(@path3)) File.Delete(@path3);
                                                        File.Move(@path2, Path.ChangeExtension(@path2, ".dwf"));
                                                    }
                                                    if (("DWG To PDF.pc3").Equals(acFrm.cbbPlDev.SelectedItem.ToString()))
                                                    {
                                                        string path3 = path + ".pdf";
                                                        if (File.Exists(@path3)) File.Delete(@path3);
                                                        File.Move(@path2, Path.ChangeExtension(@path2, ".pdf"));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                //LayoutManager.Current.CurrentLayout = acLayoutCur.LayoutName;
                                acTrans.Commit();
                            }
                            
                        }
                    }
                }
            }
        }
        

        [CommandMethod("CheckForPickfirstSelection", CommandFlags.UsePickSet)]
        public static void CheckForPickfirstSelection()
        {
            SubClass acSC = new SubClass();
            DocumentCollection acDocMgr = Application.DocumentManager;
            string[] acDocNamCol = acSC.GetAllDocumentNameInDocumentCollection(acDocMgr);
            // Get the current document
            Editor acDocEd = Application.DocumentManager.MdiActiveDocument.Editor;
            using (Transaction acTrans = Application.DocumentManager.MdiActiveDocument.Database.TransactionManager.StartTransaction())
            {
                // Get the PickFirst selection set
                PromptSelectionResult acSSPrompt;
                acSSPrompt = acDocEd.SelectImplied();

                SelectionSet acSSet;

                // If the prompt status is OK, objects were selected before
                // the command was started
                if (acSSPrompt.Status == PromptStatus.OK)
                {
                    acSSet = acSSPrompt.Value;

                    Application.ShowAlertDialog("Number of objects in Pickfirst selection: " +
                                                acSSet.Count.ToString());
                }
                else
                {
                    Application.ShowAlertDialog("Number of objects in Pickfirst selection: 0");
                }
                /*

                // Clear the PickFirst selection set
                ObjectId[] idarrayEmpty = new ObjectId[0];
                acDocEd.SetImpliedSelection(idarrayEmpty);

                // Request for objects to be selected in the drawing area
                acSSPrompt = acDocEd.GetSelection();

                // If the prompt status is OK, objects were selected
                if (acSSPrompt.Status == PromptStatus.OK)
                {
                    acSSet = acSSPrompt.Value;

                    Application.ShowAlertDialog("Number of objects selected: " +
                                                acSSet.Count.ToString());
                }
                else
                {
                    Application.ShowAlertDialog("Number of objects selected: 0");
                }
                 */
            }
        }
    }
}
