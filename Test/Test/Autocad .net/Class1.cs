using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;
using System.Reflection;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.PlottingServices;
[assembly: CommandClass(typeof(Test.Class1))]

namespace Test
{
    public class Class1
    {
        public static void abcdef()
        {
            //InputForm frm = new InputForm();
            //frm.TextBox1.Text = Application.DocumentManager.MdiActiveDocument.Name;
            //frm.ShowDialog();
            //Application.ShowAlertDialog(frm.TextBox1.Text);
        }

        //[CommandMethod("NoErrorHandler")]
        public void NoErrorHandler()
        {
            // Create a new database with no document window
            using (Database acDb = new Database(false, true))
            {
                // Read the drawing file named "Drawing123.dwg" on the C: drive.
                // If the "Drawing123.dwg" file does not exist, an eFileNotFound
                // exception is tossed and the program halts.
                acDb.ReadDwgFile("c:\\Drawing123.dwg", System.IO.FileShare.None, false, "");
            }
            // Message will not be displayed since the exception caused by
            // ReadDwgFile is not handled.
            Application.ShowAlertDialog("End of command reached");
        }
        [CommandMethod("ErrorTryCatchFinally")]
        public void ErrorTryCatchFinally()
        {
            // Create a new database with no document window
            using (Database acDb = new Database(false, true))
            {
                try
                {
                    // Read the drawing file named "Drawing123.dwg" on the C: drive.
                    // If the "Drawing123.dwg" file does not exist, an eFileNotFound
                    // exception is tossed and the catch statement handles the error.
                    acDb.ReadDwgFile("c:\\Drawing123.dwg",System.IO.FileShare.None, false, "");
                }
                catch (Autodesk.AutoCAD.Runtime.Exception Ex)
                {
                    Application.ShowAlertDialog("The following exception was caught:\n" +Ex.Message);
                    Application.ShowAlertDialog("Source:\n" + Ex.Source.ToString());
                    Application.ShowAlertDialog("StackTrace:\n" + Ex.Source.ToString());
                    Application.ShowAlertDialog("TargetSite:\n" + Ex.TargetSite.ToString());
                }
                finally
                {
                    // Message is displayed since the exception caused
                    // by ReadDwgFile is handled.
                    Application.ShowAlertDialog("End of command reached");
                }
            }
        }
        /*
        [CommandMethod("RegisterMyApp")]
        public void RegisterMyApp()
        {
            // Get the AutoCAD Applications key
            string sProdKey = HostApplicationServices.Current.RegistryProductRootKey;
            string sAppName = "MyApp";
            Autodesk.AutoCAD.Runtime.RegistryKey regAcadProdKey = Autodesk.AutoCAD.Runtime.RegistryKey.CurrentUser.OpenSubKey(sProdKey);
            Autodesk.AutoCAD.Runtime.RegistryKey regAcadAppKey = regAcadProdKey.OpenSubKey("Applications", true);
            // Check to see if the "MyApp" key exists
            string[] subKeys = regAcadAppKey.GetSubKeyNames();
            foreach (string subKey in subKeys)
            {
                // If the application is already registered, exit
                if (subKey.Equals(sAppName))
                {
                    regAcadAppKey.Close();
                    return;
                }
            }
            
            // Get the location of this module
            string sAssemblyPath = Assembly.GetExecutingAssembly().Location;
            // Register the application
            Autodesk.AutoCAD.Runtime.RegistryKey regAppAddInKey = regAcadAppKey.CreateSubKey(sAppName);
            regAppAddInKey.SetValue("DESCRIPTION", sAppName, RegistryValueKind.String);
            regAppAddInKey.SetValue("LOADCTRLS", 14, RegistryValueKind.DWord);
            regAppAddInKey.SetValue("LOADER", sAssemblyPath, RegistryValueKind.String);
            regAppAddInKey.SetValue("MANAGED", 1, RegistryValueKind.DWord);
            regAcadAppKey.Close();
        }
        [CommandMethod("UnregisterMyApp")]
        public void UnregisterMyApp()
        {
            // Get the AutoCAD Applications key
            string sProdKey = HostApplicationServices.Current.RegistryProductRootKey;
            string sAppName = "MyApp";
            Autodesk.AutoCAD.Runtime.RegistryKey regAcadProdKey = Autodesk.AutoCAD.Runtime.RegistryKey.CurrentUser.OpenSubKey(sProdKey);
            Autodesk.AutoCAD.Runtime.RegistryKey regAcadAppKey = regAcadProdKey.OpenSubKey("Applications", true);
            // Delete the key for the application
            regAcadAppKey.DeleteSubKeyTree(sAppName);
            regAcadAppKey.Close();
        }
         * */
    }
}
