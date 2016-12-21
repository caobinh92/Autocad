using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.GraphicsInterface;

namespace Test
{
    class Autocad
    {
        //Properties
        public Document Document
        {
            get
            {
                return Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            }
            set { Document = value; }
        }
        public Database Database
        {
            get
            {
                return Document.Database;
            }
            set { Database = value; }
        }
        public Editor Editor
        {
            get
            {
                return Document.Editor;
            }
            set { Editor = value; }
        }
        public Transaction Transaction
        {
            get 
            {
                return Database.TransactionManager.StartTransaction();
            }
            set { Transaction = value; }
        }
        public BlockTable BlockTable
        {
            get 
            {
                using (Transaction acTrans = Database.TransactionManager.StartTransaction())
                {
                    return (BlockTable)acTrans.GetObject(Database.BlockTableId, OpenMode.ForRead);
                }
            }
            set { BlockTable = value; }
        }
        public DBObjectCollection BlockTableRecordNamedSpaceCollection
        {
            get 
            {
                using (Transaction acTrans = Database.TransactionManager.StartTransaction())
                {
                    foreach (ObjectId acObjId in BlockTable)
                    {
                        DBObject acDbObj = acTrans.GetObject(acObjId,OpenMode.ForRead);
                        if (acDbObj is BlockTableRecord)
                        {
                            BlockTableRecord acBlkTblRec = (BlockTableRecord)acDbObj;
                            if (acBlkTblRec.Name.Contains("Space"))
                                BlockTableRecordNamedSpaceCollection.Add(acBlkTblRec);
                        }
                    }
                    return BlockTableRecordNamedSpaceCollection;
                }
            }
            set { BlockTableRecordNamedSpaceCollection = value; }
        }
        public DBObjectCollection BlockTableRecordCollection
        {
            get
            {
                using (Transaction acTrans = Database.TransactionManager.StartTransaction())
                {
                    foreach (ObjectId acObjId in BlockTable)
                    {
                        DBObject acDbObj = acTrans.GetObject(acObjId, OpenMode.ForRead);
                        if (acDbObj is BlockTableRecord)
                        {
                            BlockTableRecord acBlkTblRec = (BlockTableRecord)acDbObj;
                            if (!acBlkTblRec.Name.Contains("Space"))
                                BlockTableRecordCollection.Add(acBlkTblRec);
                        }
                    }
                    return BlockTableRecordCollection;
                }
            }
            set { BlockTableRecordCollection = value; }
        }
        
        //Methods
        public BlockTableRecord GetBlockTableRecord(string BlockName)
        {

            using (Transaction acTrans = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl = (BlockTable)acTrans.GetObject(Database.BlockTableId, OpenMode.ForRead);
                return (BlockTableRecord)acTrans.GetObject(acBlkTbl[BlockName], OpenMode.ForRead);
            }
        }
        public BlockTableRecord GetBlockTableRecord(string PromptMessage, string PromptRejectMessage)
        {
            using (Transaction acTrans = Database.TransactionManager.StartTransaction())
            {
                PromptEntityOptions pEntOpt = new PromptEntityOptions(PromptMessage);
                pEntOpt.SetRejectMessage(PromptRejectMessage);
                pEntOpt.AddAllowedClass(typeof(BlockReference), true);
                PromptEntityResult pEntRes = Editor.GetEntity(pEntOpt);
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
                    Application.ShowAlertDialog("Khung tên đã chọn là: " + acBlkTblRec.Name);
                    return acBlkTblRec;
                }
                else
                {
                    Application.ShowAlertDialog("Bạn đã thoát lệnh!");
                    return null;
                }
            }
        }
        public BlockTableRecord GetBlockTableRecord()
        {
            using (Transaction acTrans = Database.TransactionManager.StartTransaction())
            {
                StringBuilder text = null;
                int i = 1;
                foreach (DBObject acDbObj in BlockTableRecordCollection)
                {
                    BlockTableRecord acBlckRecArr = (BlockTableRecord)acDbObj;
                    text.AppendFormat("Blockname thứ {0}:\t{1}\n", i, acBlckRecArr.Name);
                    i++;
                }
                Application.ShowAlertDialog("Liệt kê các block trên bản vẽ:\n");
                PromptIntegerOptions pIntOpt = new PromptIntegerOptions("Nhập vào số thứ tự của Block cần lấy:");
                PromptIntegerResult pIntRes = Editor.GetInteger(pIntOpt);
                if (pIntRes.Status == PromptStatus.OK)
                {
                    int n = pIntRes.Value;
                    return (BlockTableRecord)BlockTableRecordCollection[n - 1];

                }
                else
                {
                    Application.ShowAlertDialog("Bạn đã thoát lệnh!");
                    return null;
                }
            }
        }
        public DBObjectCollection AttributeDefinitionCollection(BlockTableRecord BlockTableRecord)
        {
            using (Transaction acTrans = Database.TransactionManager.StartTransaction())
            {
                DBObjectCollection acAttDefCol1 = new DBObjectCollection();
                foreach (ObjectId acObjId in BlockTableRecord)
                {
                    DBObject acEnt = acTrans.GetObject(acObjId, OpenMode.ForRead);
                    if (acEnt is AttributeDefinition)
                    {
                        acAttDefCol1.Add(acEnt);
                    }
                }
                return acAttDefCol1;
            }
        }

        public BlockReference GetBlockRefence(string PromptMessage, string PromptRejectMessage)
        {
            using (Transaction acTrans = Database.TransactionManager.StartTransaction())
            {
                PromptEntityOptions pEntOpt = new PromptEntityOptions(PromptMessage);
                pEntOpt.SetRejectMessage(PromptRejectMessage);
                pEntOpt.AddAllowedClass(typeof(BlockReference), true);
                PromptEntityResult pEntRes = Editor.GetEntity(pEntOpt);
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
                    Application.ShowAlertDialog("Khung tên đã chọn là: " + acBlkTblRec.Name);
                    return acBlkRef;
                }
                else
                {
                    Application.ShowAlertDialog("Bạn đã thoát lệnh!");
                    return null;
                }
            }
        }
    }

}
