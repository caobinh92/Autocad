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
using Autodesk.AutoCAD.PlottingServices;

[assembly: CommandClass(typeof(Test.Test3))]

namespace Test
{
    class Test3
    {
        public InputForm acFrm = new InputForm();
        //public static System.Windows.Forms.TextBox acFrmText;
        [CommandMethod("T3")]
        public void T3 ()
        {
            if (acFrm == null)
            {
                //acFrm = new AutocadForm();
            }
            //acFrmText = acFrm.textBox1;
            acFrm.ShowDialog();
            acFrm.txtBlkTblRecNam.Text = "abc";
            Application.ShowAlertDialog("Nhap vao form: " + acFrm.txtBlkTblRecNam.Text);
            acFrm.ShowDialog();
            Application.ShowAlertDialog("Check");
            acFrm.ShowDialog();
        }
    }
}
