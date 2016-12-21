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
[assembly: CommandClass(typeof(Test.MainClassCTB))]

namespace Test
{
    class MainClassCTB
    {
        public ChangeForm acFrm;
        public DBObjectCollection acBlkTblRecCol;
        public int[] acAttRefIDCol;

        [CommandMethod("ReplaceTitle", CommandFlags.Session)]
        public void ReplaceTitle()
        {
            // Step 1: CREATE INSTANCE OF SUBCLASS
            SubClass acSC = new SubClass();

            // Step 2: GET ALL DOCUMENT NAME OPENED
            DocumentCollection acDocMgr = Application.DocumentManager;
            if (acFrm == null) acFrm = new ChangeForm();
            acFrm.DocumentNameCollection = acSC.GetAllDocumentNameInDocumentCollection(acDocMgr);
            //Application.ShowAlertDialog("1");
            acFrm.BlockTableRecordNameCollection = acSC.GetBlockTableRecordNameCollection(acDocMgr);
           // Application.ShowAlertDialog("2");
            acBlkTblRecCol = acSC.GetBlockTableRecordCollection(acDocMgr);
            //Application.ShowAlertDialog("3");
            acFrm.CreateDocumentNameCollection();
            //Application.ShowAlertDialog("4");
            //Application.ShowAlertDialog(acFrm.BlockTableRecordNameCollection[0]);
            //Application.ShowAlertDialog(acFrm.BlockTableRecordNameCollection[1]);
            acFrm.CreateBlockTableNameCollection();
            //acFrm.ShowDialog();
            //Application.ShowAlertDialog("5");
            acFrm.AttributeDefinitionCollection = acSC.GetAttributeDefinitionNameAllCollection(acDocMgr, acBlkTblRecCol);
            //Application.ShowAlertDialog("6");
            acFrm.cbbBlkTblRec1.SelectedIndex = 0;
            //acFrm.CreateAttributeDefinitionCollectionRemoved();
            //Application.ShowAlertDialog("7");
            acFrm.cbbBlkTblRec2.SelectedIndex = 0;
            //acFrm.CreateAttributeDefinitionCollectionReplaced();
            //Application.ShowAlertDialog("8");
            acFrm.IsButtonOkClicked = false;
            acFrm.ShowDialog();
            while (acFrm.IsClosed == false)
            {
                if (acFrm.IsButtonBlockTableRecordName1Clicked)
                {
                    string txt;
                    using (Transaction acTrans = acDocMgr.MdiActiveDocument.Database.TransactionManager.StartTransaction())
                    {
                        txt = (acSC.GetBlockReference(acDocMgr.MdiActiveDocument, acTrans, "Select a Title Block", "Select a Title Block")).Name;
                    }
                    int select = 0;
                    for (int i = 0; i < acFrm.cbbBlkTblRec1.Items.Count; i++)
                    {
                        if (acFrm.cbbBlkTblRec1.Items[i].ToString() == txt) select = i;

                    }
                    acFrm.cbbBlkTblRec1.SelectedIndex = select;
                }

                if (acFrm.IsButtonBlockTableRecordName2Clicked)
                {
                    string txt;
                    using (Transaction acTrans = acDocMgr.MdiActiveDocument.Database.TransactionManager.StartTransaction())
                    {
                        txt = (acSC.GetBlockReference(acDocMgr.MdiActiveDocument, acTrans, "Select a Title Block", "Select a Title Block")).Name;
                    }
                    int select = 0;
                    for (int i = 0; i < acFrm.cbbBlkTblRec2.Items.Count; i++)
                    {
                        if (acFrm.cbbBlkTblRec2.Items[i].ToString() == txt) select = i;

                    }
                    acFrm.cbbBlkTblRec2.SelectedIndex = select;
                }
            }
            if (acFrm.IsButtonOkClicked)
            {
                //Application.ShowAlertDialog("Start");
                BlockTableRecord acBlkTblRecReplaced = new BlockTableRecord();
                for (int i = 0; i < acBlkTblRecCol.Count; i++)
                {
                    if (((BlockTableRecord) acBlkTblRecCol[i]).Name.Equals(acFrm.cbbBlkTblRec2.SelectedItem.ToString()))
                    {
                        //Application.ShowAlertDialog("");
                        acBlkTblRecReplaced = ((BlockTableRecord)acBlkTblRecCol[i]);
                        //Application.ShowAlertDialog(acBlkTblRecReplaced.Name);
                        foreach (Document acDoc in acDocMgr)
                        {
                            using (Transaction acTrans = acDoc.Database.TransactionManager.StartTransaction())
                            {
                                BlockTable acBlkTbl =(BlockTable) acTrans.GetObject(acDoc.Database.BlockTableId, OpenMode.ForRead);
                                if (acBlkTbl.Has(acBlkTblRecReplaced.Name))
                                {
                                    BlockTableRecord acBlkTblRecReplacedClone = (BlockTableRecord)acTrans.GetObject(acBlkTbl[acBlkTblRecReplaced.Name], OpenMode.ForRead);
                                    string[] acAttDefNamCol = acSC.GetAttributeDefinitionNameCollection(acDoc, acTrans, acBlkTblRecReplacedClone);
                                    acAttRefIDCol = new int[acAttDefNamCol.Length];
                                    for (int j = 0; j < acAttDefNamCol.Length; j++)
                                    {
                                        int count = 0;
                                        for (int l = j; l < acAttDefNamCol.Length; l++)
                                        {
                                            if (acAttDefNamCol[j].Equals(acAttDefNamCol[l]))
                                                count++;
                                        }
                                        //Application.ShowAlertDialog(acAttDefNamCol[j]+"\ncount \n"+count.ToString());
                                        int count1 = 0;
                                        bool IsCheckedOk = false;
                                        for (int k = 0; k < acFrm.gpbAttDefNamCol.Controls.Count; k++)
                                        {
                                            if (acAttDefNamCol[j].Equals(((System.Windows.Forms.ComboBox)(acFrm.gpbAttDefNamCol.Controls[k])).SelectedItem.ToString()))
                                            {
                                                IsCheckedOk = true;
                                                if (count < 2) acAttRefIDCol[j] = k;
                                                //Application.ShowAlertDialog(k.ToString());
                                                else
                                                {
                                                    count1 = 0;
                                                    for (int l = k; l < acFrm.gpbAttDefNamCol.Controls.Count; l++)
                                                    {
                                                        string txt1 = ((System.Windows.Forms.ComboBox)acFrm.gpbAttDefNamCol.Controls[k]).SelectedItem.ToString();
                                                        string txt2 = ((System.Windows.Forms.ComboBox)acFrm.gpbAttDefNamCol.Controls[l]).SelectedItem.ToString();
                                                        if (txt1.Equals(txt2)) count1++;
                                                    }
                                                    if (count == count1) acAttRefIDCol[j] = k;
                                                }
                                            }
                                        }
                                        if (!IsCheckedOk) acAttRefIDCol[j] = acAttDefNamCol.Length * 2;
                                        //Application.ShowAlertDialog(acAttDefNamCol[j]+"\ncount1 \n"+count1.ToString());
                                }
                                }
                            }
                        }
                        //Application.ShowAlertDialog("Finish");
                        break;
                    }
                }
                foreach (Document acDoc in acDocMgr)
                {
                    //Application.ShowAlertDialog("");
                    bool check = false;
                    foreach (string item in acFrm.DocumentNameCollection)
                    {
                        if (item.Equals(acSC.DocumentShortName(acDoc.Database.OriginalFileName)))
                            check = true;
                        //Application.ShowAlertDialog(item + "/ncompare to /n" + DocumentShortName(acDoc1.Database.OriginalFileName));
                    }
                    //Application.ShowAlertDialog(check.ToString());
                    if (check)
                    {
                        if (acDocMgr.MdiActiveDocument != acDoc)
                        {
                            acDocMgr.MdiActiveDocument = acDoc;
                        }
                        Database acCurDb = acDoc.Database;
                        //HostApplicationServices.WorkingDatabase = acCurDb;
                        Editor acDocEd = acDoc.Editor;
                        using (DocumentLock acDocLck = acDocMgr.MdiActiveDocument.LockDocument())
                        {
                            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                            {
                                BlockTable acBlkTbl =(BlockTable) acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead);
                                DBObjectCollection acBlkTblRecSPcCol = acSC.GetBlockTableRecordSpaceCollection(acDoc, acTrans);
                                //Application.ShowAlertDialog("Ready to Get into");
                                foreach (DBObject acDbObj in acBlkTblRecSPcCol)
                                {
                                    BlockTableRecord acBlkTblRecSPc = (BlockTableRecord)acDbObj;
                                    //Application.ShowAlertDialog("Already Get into");
                                    DBObjectCollection acDbObjCol = acSC.GetObjectsInBLockTableRecordSpace(acTrans, acBlkTblRecSPc);
                                    foreach (DBObject acDbObj2 in acDbObjCol)
                                    {
                                        //Application.ShowAlertDialog("Already Get into Object");
                                        try
                                        {
                                            //Application.ShowAlertDialog("Already AcTrnas");
                                            if (acDbObj2 is BlockReference)
                                            {
                                                BlockReference acBlkRefRemoved = (BlockReference)acDbObj2;
                                                if (acBlkRefRemoved.Name.Equals(acFrm.cbbBlkTblRec1.SelectedItem.ToString()))
                                                {
                                                    //Application.ShowAlertDialog("Ready To Change");
                                                    acSC.ReplaceBlockReferenceWithBlockTableRecord(acDocMgr, acDoc, acTrans, acBlkTblRecSPc, acBlkRefRemoved, acBlkTblRecReplaced, acAttRefIDCol);
                                                    //Application.ShowAlertDialog("Already Changed");
                                                    continue;
                                                }
                                            }
                                        }
                                        catch (Autodesk.AutoCAD.Runtime.Exception e)
                                        {
                                            Application.ShowAlertDialog("Message \n" + e.Message);
                                            Application.ShowAlertDialog("StackTrace \n" + e.StackTrace.ToString());
                                            Application.ShowAlertDialog("HelpLink \n" + e.HelpLink.ToString());
                                            Application.ShowAlertDialog("Source \n" + e.Source.ToString());
                                        }
                                    }
                                }
                                acTrans.Commit();
                            }
                        }
                    }
                }
            }
        }
    }
}
