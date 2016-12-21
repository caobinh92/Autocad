using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

using Autodesk.AutoCAD.PlottingServices;



[assembly: CommandClass(typeof(AcadPaperSizes.MyCommands))]

namespace AcadPaperSizes
{


    public class MyCommands
    {

        [CommandMethod("qPL")]
        public void QueryPlotters()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            LayoutManager layMgr = LayoutManager.Current;
            PlotSettingsValidator plSetVdr = PlotSettingsValidator.Current;

            db.TileMode = false;        //goto Paperspace if not already
            ed.SwitchToPaperSpace();    //turn PVP off

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                Layout theLayout = (Layout)tr.GetObject(layMgr.GetLayoutId(layMgr.CurrentLayout), OpenMode.ForRead);

                PlotSettings np = new PlotSettings(theLayout.ModelType);
                np.CopyFrom(theLayout);

                //string devName = "DWG To PDF.pc3";
                StringCollection devNames = plSetVdr.GetPlotDeviceList();
                ed.WriteMessage("\n{0} Plotters", devNames.Count);

                int i = 0;
                foreach (string devName in devNames)
                {
                    ed.WriteMessage("\n{0}: {1}", i, devName);
                    i++;
                }
            }
        }


        [CommandMethod("qMN")]
        public void QueryMediaNames()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            LayoutManager layMgr = LayoutManager.Current;
            PlotSettingsValidator plSetVdr = PlotSettingsValidator.Current;


            db.TileMode = false;        //goto Paperspace if not already
            ed.SwitchToPaperSpace();    //turn PVP off

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                Layout theLayout = (Layout)tr.GetObject(layMgr.GetLayoutId(layMgr.CurrentLayout), OpenMode.ForRead);

                PlotSettings np = new PlotSettings(theLayout.ModelType);
                np.CopyFrom(theLayout);

                //string canMedName = "ANSI_B_(11.00_x_17.00_Inches)"
                StringCollection canMedNames = plSetVdr.GetCanonicalMediaNameList(np);
                ed.WriteMessage("\n{0} MediaNames for {1}", canMedNames.Count, np.PlotConfigurationName);

                int i = 0;
                foreach (string canMedName in canMedNames)
                {
                    ed.WriteMessage("\n{0}: {1} {2}", i, canMedName, plSetVdr.GetLocaleMediaName(np, i));
                    i++;
                }
            }
        }

        [CommandMethod("qPS")]
        public void QueryPlotStyles()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            LayoutManager layMgr = LayoutManager.Current;
            PlotSettingsValidator plSetVdr = PlotSettingsValidator.Current;

            db.TileMode = false;        //goto Paperspace if not already
            ed.SwitchToPaperSpace();    //turn PVP off

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                Layout theLayout = (Layout)tr.GetObject(layMgr.GetLayoutId(layMgr.CurrentLayout), OpenMode.ForRead);

                PlotSettings np = new PlotSettings(theLayout.ModelType);
                np.CopyFrom(theLayout);

                //string plotStyle="Monochrome.ctb"
                StringCollection plotStyles = plSetVdr.GetPlotStyleSheetList();
                ed.WriteMessage("\n{0} PlotStyles", plotStyles.Count);

                int i = 0;
                foreach (string plotStyleName in plotStyles)
                {
                    ed.WriteMessage("\n{0}: {1}", i, plotStyleName);
                    i++;
                }
            }
        }

        [CommandMethod("qPZ")]
        public void QueryPaperSize()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            LayoutManager layMgr = LayoutManager.Current;
            PlotSettingsValidator plSetVdr = PlotSettingsValidator.Current;

            Dictionary<string, Point2d> customPaperSize = new Dictionary<string, Point2d>(StringComparer.CurrentCultureIgnoreCase);
            customPaperSize.Add("A4", new Point2d(210.0, 297.0));
            customPaperSize.Add("A3", new Point2d(297.0, 420.0));
            customPaperSize.Add("A2", new Point2d(420.0, 594.0));
            customPaperSize.Add("A1", new Point2d(594.0, 841.0));
            customPaperSize.Add("A0", new Point2d(841.0, 1189.0));
            customPaperSize.Add("A0Plus", new Point2d(841.0, 1480.0));
            customPaperSize.Add("A0+", new Point2d(841.0, 1470.0));
            customPaperSize.Add("GBKN", new Point2d(594.0, 1051.0));

            customPaperSize.Add("4Z", new Point2d(297.0, 750.0));
            customPaperSize.Add("5Z", new Point2d(297.0, 930.0));
            customPaperSize.Add("6Z", new Point2d(297.0, 1110.0));
            customPaperSize.Add("7Z", new Point2d(297.0, 1290.0));

            customPaperSize.Add("Z3", new Point2d(297.0, 594.0));
            customPaperSize.Add("Z4", new Point2d(297.0, 841.0));
            customPaperSize.Add("Z6", new Point2d(297.0, 1189.0));

            customPaperSize.Add("3A4", new Point2d(297.0, 630.0));
            customPaperSize.Add("4A4", new Point2d(297.0, 841.0));
            customPaperSize.Add("5A4", new Point2d(297.0, 1050.0));
            customPaperSize.Add("6A4", new Point2d(297.0, 1260.0));
            customPaperSize.Add("7A4", new Point2d(297.0, 1470.0));



            db.TileMode = false;        //goto Paperspace if not already
            ed.SwitchToPaperSpace();    //turn PVP off

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                Layout theLayout = (Layout)tr.GetObject(layMgr.GetLayoutId(layMgr.CurrentLayout), OpenMode.ForWrite);

                // Get the PlotInfo from the layout
                PlotInfo plInfo = new PlotInfo();
                plInfo.Layout = theLayout.ObjectId;

                PlotSettings np = new PlotSettings(theLayout.ModelType);
                np.CopyFrom(theLayout);

                string devName = "DWG To PDF.pc3";  //OceTds600.pc3
                plSetVdr.SetPlotConfigurationName(np, devName, null);
                plSetVdr.RefreshLists(np);

                StringCollection canMedNames = plSetVdr.GetCanonicalMediaNameList(np);
                for (int i = 0; i < canMedNames.Count; i++)
                {
                    plSetVdr.SetCanonicalMediaName(np, canMedNames[i]);
                    plSetVdr.RefreshLists(np);
                    string medName = plSetVdr.GetLocaleMediaName(np, i);
                    //ed.WriteMessage("\n{0}: Can. MediaName={1}/{2}", i, np.CanonicalMediaName, medName);
                    if (medName.Equals("ISO A1 (841.00 x 594.00 MM)"))
                    {
                        ed.WriteMessage("\n{0}: Can. MediaName={1}/{2}", i, np.CanonicalMediaName, medName);
                        Point2d pSize = np.PlotPaperSize;
                        ed.WriteMessage("\n\tPaperSize={0}", pSize.ToString());
                        PlotRotation pRot = np.PlotRotation;
                        ed.WriteMessage("\n\tPlot Rotation={0}", pRot.ToString());
                        Extents2d pM = np.PlotPaperMargins;
                        ed.WriteMessage("\n\tPaper Margins={0}, {1}", pM.MinPoint.ToString(), pM.MaxPoint.ToString());
                    }
                }
            }
        }

        [CommandMethod("qNP")]
        public void QueryNamedPlotStyles()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                DBDictionary plotDict = (DBDictionary)db.PlotSettingsDictionaryId.GetObject(OpenMode.ForRead);
                ed.WriteMessage("\n{0} NamedPlotStyles", plotDict.Count);
                int i = 0;
                foreach (DBDictionaryEntry plotDictEntry in plotDict)
                {
                    PlotSettings npStyle = (PlotSettings)tr.GetObject(plotDictEntry.Value, OpenMode.ForRead);
                    ed.WriteMessage("\n{0}: NamedPlotStyle: {1}", i, npStyle.PlotConfigurationName);
                    i++;
                }

                tr.Commit();
            }

        }


        [CommandMethod("qDNP")]
        public void QueryDetailedNamedPlotStyles()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PlotSettingsValidator plSetVdr = PlotSettingsValidator.Current;


            db.TileMode = false;        //goto Paperspace if not already
            ed.SwitchToPaperSpace();    //turn PVP off

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                DBDictionary plotDict = (DBDictionary)db.PlotSettingsDictionaryId.GetObject(OpenMode.ForRead);
                ed.WriteMessage("\n{0} NamedPlotStyles", plotDict.Count);
                int i = 0;
                foreach (DBDictionaryEntry plotDictEntry in plotDict)
                {
                    PlotSettings np = (PlotSettings)tr.GetObject(plotDictEntry.Value, OpenMode.ForRead);
                    ed.WriteMessage("\n{0}: NamedPlotStyle: {1}", i, np.PlotSettingsName);
                    ed.WriteMessage("\n\tPlotter={0}", np.PlotConfigurationName);
                    ed.WriteMessage("\n\tPlotStyle={0}", np.CurrentStyleSheet);
                    ed.WriteMessage("\n\tPlotArea={0}", np.PlotType);

                    StringCollection medNames = plSetVdr.GetCanonicalMediaNameList(np);
                    for (int j = 0; j < medNames.Count; j++)
                    {
                        if (np.CanonicalMediaName.Equals(medNames[j], StringComparison.InvariantCultureIgnoreCase))
                        {
                            ed.WriteMessage("\n\tPlotPaper={0}/\t{1}", plSetVdr.GetLocaleMediaName(np, j), np.CanonicalMediaName);
                            break;
                        }
                    }

                    ed.WriteMessage("\n\tPaperSize={0}", np.PlotPaperSize.ToString());
                    ed.WriteMessage("\n\tPlot Rotation={0}", np.PlotRotation.ToString());
                    ed.WriteMessage("\n\tPaper Margins={0}, {1}", np.PlotPaperMargins.MinPoint.ToString(), np.PlotPaperMargins.MaxPoint.ToString());

                    i++;
                }

                tr.Commit();
            }

        }



    }

}