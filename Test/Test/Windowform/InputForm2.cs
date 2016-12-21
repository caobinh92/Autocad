using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class InputForm2 : Form
    {
        public string[] BlockTableRecordCollection { get; set; }
        public int BlockTableRecordSet { get; set; }
        public string[,] AttributeDefinitionCollection { get; set; }
        public int AttributeDefinitionSet { get; set; }
        public string[] PlotDeviceCollection { get; set; }
        public int PlotDeviceSet { get; set; }
        public string[,] CanonicalMediaNameCollection { get; set; }
        public int CanonicalMediaNameSet { get; set; }
        public string[] PlotStyleCollection { get; set; }
        public int PlotStyleSet { get; set; }

        public int BlockTableRecordID { get; set; }
        public string BlockTableRecordName { get; set; }
        public int AttributeID { get; set; }
        public string AttributeName { get; set; }
        public int PlotDeviceID { get; set; }
        public string PlotDeviceName { get; set; }
        public int CanonicalMediaID { get; set; }
        public string CanonicalMediaName { get; set; }
        public int PlotStyleID { get; set; }
        public string PlotStyleName { get; set; }

        public Boolean ClassedPlotNameByIndex { get; set; }

        public string[] DocumentNameCollection { get; set; }

        public InputForm2()
        {
            InitializeComponent();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            
            //try
            //{
                AttributeDefinitionSet = cbbAttDef.SelectedIndex;
                 CanonicalMediaNameSet=cbbCanMed.SelectedIndex;

                BlockTableRecordID = cbbBlkTblRec.SelectedIndex;
                
                PlotDeviceID = cbbPlDev.SelectedIndex;
                CanonicalMediaID = cbbCanMed.SelectedIndex;
                PlotStyleID = cbbPlStl.SelectedIndex;
                BlockTableRecordName = cbbBlkTblRec.SelectedItem.ToString();
                PlotDeviceName = cbbPlDev.SelectedItem.ToString();
                PlotStyleName = cbbPlStl.SelectedItem.ToString();

                if ((cbbAttDef.SelectedIndex != -1) && (cbbAttDef.SelectedIndex != -1))
                {
                    AttributeName = cbbAttDef.SelectedItem.ToString();
                    CanonicalMediaName = cbbCanMed.SelectedItem.ToString();
                }
                if (rbtAttribute.Checked == true)
                    ClassedPlotNameByIndex = false;
                else
                    ClassedPlotNameByIndex = true;
                DocumentNameCollection = new string[ltvDocColTo.Items.Count];
                int i = 0;
                foreach (ListViewItem item in ltvDocColTo.Items)
                {
                    DocumentNameCollection[i] = item.Text;
                    i++;
                }
             
            /*
            foreach (string item in DocumentNameCollection)
            {
                MessageBox.Show(item);
            }*/
            /*
            MessageBox.Show(BlockTableRecordName + " - " + BlockTableRecordID.ToString());
            MessageBox.Show(AttributeName + " - " + AttributeID.ToString());
            MessageBox.Show(PlotDeviceName + " - " + PlotDeviceID.ToString());
            MessageBox.Show(CanonicalMediaName + " - " + CanonicalMediaID.ToString());
            */
            this.Close();
            //}
            //finally { }
        }

        private void InputForm_Load(object sender, EventArgs e)
        {
            
            cbbBlkTblRec.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            cbbPlDev.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            rbtAttribute.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            rbtIndex.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            GetCollectionBlockAndAttribute();
            GetCollectionPlotDeviceAndCanonical();
            cbbAttDef.SelectedIndex = AttributeDefinitionSet;
            cbbCanMed.SelectedIndex = CanonicalMediaNameSet;
            GetPlotStyle();
            rbtIndex.Checked = true;
            CreateDocumentCheckBoxList();

            
            btnCheck.Visible = false;
            
        }
        public void GetCollectionBlockAndAttribute()
        {
            if (BlockTableRecordCollection == null)
            {
                BlockTableRecordCollection = new string[] { "a", "b", "c" };

                AttributeDefinitionCollection = new string[3, 5];
                AttributeDefinitionCollection[1, 0] = "2";
                AttributeDefinitionCollection[1, 1] = "3";
                AttributeDefinitionCollection[2, 0] = "7";
                AttributeDefinitionCollection[2, 1] = "8";
                AttributeDefinitionCollection[2, 2] = "9";
            }

            cbbBlkTblRec.Items.Clear();
            cbbAttDef.Items.Clear();
            foreach (string BlockTableRecord in BlockTableRecordCollection)
            {
                cbbBlkTblRec.Items.Add(BlockTableRecord);
            }
            cbbBlkTblRec.SelectedIndex = BlockTableRecordSet;

            for (int i = 0; i < AttributeDefinitionCollection.GetLength(1); i++)
            {
                if (!string.IsNullOrEmpty(AttributeDefinitionCollection[BlockTableRecordSet, i]))
                    cbbAttDef.Items.Add(AttributeDefinitionCollection[BlockTableRecordSet, i]);
            }
            if (cbbAttDef.Items.Count != 0)
                cbbAttDef.SelectedIndex = 0;
            else
                cbbAttDef.Text = null;
        }

        public void GetCollectionPlotDeviceAndCanonical()
        {
            if (PlotDeviceCollection == null)
            {
                PlotDeviceCollection = new string[] { "a", "b", "c", "d" };

                CanonicalMediaNameCollection = new string[3, 5];
                CanonicalMediaNameCollection[1, 0] = "2";
                CanonicalMediaNameCollection[1, 1] = "3";
                CanonicalMediaNameCollection[2, 0] = "7";
                CanonicalMediaNameCollection[2, 1] = "8";
                CanonicalMediaNameCollection[2, 2] = "9";

            }
            cbbPlDev.Items.Clear();
            cbbCanMed.Items.Clear();
            foreach (string PlotDevice in PlotDeviceCollection)
            {
                cbbPlDev.Items.Add(PlotDevice);
            }
            cbbPlDev.SelectedIndex = PlotDeviceSet;

            for (int i = 0; i < CanonicalMediaNameCollection.GetLength(1); i++)
            {
                if (!string.IsNullOrEmpty(CanonicalMediaNameCollection[PlotDeviceSet, i]))
                    cbbCanMed.Items.Add(CanonicalMediaNameCollection[PlotDeviceSet, i]);
            }
            if (cbbCanMed.Items.Count != 0)
                cbbCanMed.SelectedIndex = 0;
            else
                cbbCanMed.Text = null;
        }

        public void GetPlotStyle()
        {
            if (PlotStyleCollection == null)
            {
                PlotStyleCollection = new string[] { "f", "h", "k", "l" };

            }
            cbbPlStl.Items.Clear();
            foreach (string PlotStyle in PlotStyleCollection)
            {
                cbbPlStl.Items.Add(PlotStyle);
            }
            cbbPlStl.SelectedIndex = PlotStyleSet;

        }

        public void CreateDocumentCheckBoxList()
        {
            if (DocumentNameCollection == null)
            {
                DocumentNameCollection = new string[] { "a123456789", "b123456789", "c123456789", "d123456789" };
            }
            List<CheckBox> ckbDocCol = new List<CheckBox>();
            foreach (string DocumentName in DocumentNameCollection)
            {
                ListViewItem ltviDoc = new ListViewItem();
                ltvDocColTo.Items.Add(ltviDoc);
                ltviDoc.Text = DocumentName;
            }
        }
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if ((cbbBlkTblRec.SelectedIndex!=-1) && (cbbPlDev.SelectedIndex!=-1))
            {
                if (BlockTableRecordSet != cbbBlkTblRec.SelectedIndex) 
                {
                    BlockTableRecordSet = cbbBlkTblRec.SelectedIndex;
                    GetCollectionBlockAndAttribute();
                }
                if (PlotDeviceSet != cbbPlDev.SelectedIndex)
                {
                    PlotDeviceSet = cbbPlDev.SelectedIndex;
                    GetCollectionPlotDeviceAndCanonical();
                }
            }
            
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(cbbAttDef.SelectedIndex.ToString());
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtAttribute.Checked == true)
                cbbAttDef.Enabled = true;
            else cbbAttDef.Enabled = false;
        }

        private void btnClick_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ltvDocColFrom.Items)
            {
                if (item.Selected)
                {
                    ltvDocColFrom.Items.Remove(item);
                }
            }
            foreach (ListViewItem item in ltvDocColTo.Items)
            {
                ltvDocColTo.Items.Remove(item);
            }
            foreach (string Doc in DocumentNameCollection)
            {
                bool check = true;
                foreach (ListViewItem item in ltvDocColFrom.Items)
                {
                    if (Doc.Equals(item.Text))
                        check = false;
                }
                if (check)
                {
                    ListViewItem ltviDoc = new ListViewItem();
                    ltviDoc.Text = Doc;
                    ltvDocColTo.Items.Add(ltviDoc);
                }
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ltvDocColTo.Items)
            {
                if (item.Selected)
                {
                    ltvDocColTo.Items.Remove(item);
                }
            }
            foreach (ListViewItem item in ltvDocColFrom.Items)
            {
                ltvDocColFrom.Items.Remove(item);
            }
            foreach (string Doc in DocumentNameCollection)
            {
                bool check = true;
                foreach (ListViewItem item in ltvDocColTo.Items)
                {
                    if (Doc.Equals(item.Text))
                        check = false;
                }
                if (check)
                {
                    ListViewItem ltviDoc = new ListViewItem();
                    ltviDoc.Text = Doc;
                    ltvDocColFrom.Items.Add(ltviDoc);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ltvDocColFrom.Items)
            {
                ltvDocColFrom.Items.Remove(item);
            }
            foreach (ListViewItem item in ltvDocColTo.Items)
            {
                ltvDocColTo.Items.Remove(item);
            }
            foreach (string Doc in DocumentNameCollection)
            {
                ListViewItem ltviDoc = new ListViewItem();
                ltviDoc.Text = Doc;
                ltvDocColTo.Items.Add(ltviDoc);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in ltvDocColTo.Items)
            {
                ltvDocColTo.Items.Remove(item);
            }
            foreach (ListViewItem item in ltvDocColFrom.Items)
            {
                ltvDocColFrom.Items.Remove(item);
            }
            foreach (string Doc in DocumentNameCollection)
            {
                ListViewItem ltviDoc = new ListViewItem();
                ltviDoc.Text = Doc;
                ltvDocColFrom.Items.Add(ltviDoc);
            }
        }


    }
}
