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
using ACSMCOMPONENTS21Lib;

[assembly: CommandClass(typeof(Test1.Class1))]


namespace Test1
{
    public class Class1
    {
        [CommandMethod("T7")]
        public void T7()
        {
            DocumentCollection acDocMgr = Application.DocumentManager;
            Document acDoc = acDocMgr.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            Editor acDocEd = acDoc.Editor;
            IAcSmSheetSetMgr sheetSetMgr;
            //foreach (IAcSmSheetSet item in sheetSetMgr)
            {
                
            }
        }
    }
}
