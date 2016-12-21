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
    public partial class ChangeForm : Form
    {
        public string[] BlockTableRecordNameCollection;
        public string[,] AttributeDefinitionCollection;
        public string[] PlotDeviceCollection;
        public string[,] CanonicalMediaNameCollection;
        public string[] PlotStyleCollection;
        public string[] DocumentNameCollection;
        public bool IsButtonBlockTableRecordName1Clicked;
        public bool IsButtonBlockTableRecordName2Clicked;
        public bool IsButtonOkClicked;
        public bool IsButtonCancelClicked;
        public bool IsClosed;

        
        public int[] AttributeReferenceIndexCollection;
        //private MainClass acMain;
        //private SubClass acSub;

        public ChangeForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            IsButtonOkClicked = true; IsClosed = true;
            int i = 0;
            DocumentNameCollection = new string[ltvDocColTo.Items.Count];
            foreach (ListViewItem item in ltvDocColTo.Items)
            {
                DocumentNameCollection[i] = item.Text;
                i++;
            }
            Close();
        }

        private void InputForm_Load(object sender, EventArgs e)
        {
            /*
            BlockTableRecordNameCollection = new string[] { "a", "b", "c" };
            AttributeDefinitionCollection = new string[,]
            { { "1a", "2a", "3a", "4a", "5a", "6a", "7a", "8a", "9a", "10a", "11a", "12a", "13a", "14a", "15a" }, 
            { "1b", "2b", "3b", "4b", "5b", "6b", "7b", "8b", "9b", "10b", "11b", "12b", "13b", "14b", "15b" }, 
            { "1c", "2c", "3c","4c","5c","6c","7c","8c","9c","10c","11c","12c","13c","14c","15c"} };
            CreateBlockTableNameCollection();
            CreateDocumentNameCollection();
            */
        }
        private void btnCheck_Click(object sender, EventArgs e)
        {
            CreateAttributeDefinitionCollectionRemoved();
            CreateAttributeDefinitionCollectionReplaced();
        }
        public void CreateAttributeDefinitionCollectionRemoved()
        {
            ltvAttDefNamCol.Items.Clear();
            if (cbbBlkTblRec1.Items.Count ==0) return;
            for (int i = 0; i < AttributeDefinitionCollection.GetLength(1); i++)
			{
                if (AttributeDefinitionCollection[cbbBlkTblRec1.SelectedIndex,i] !=null)
                {
			        ListViewItem ltviAttDefNam = new ListViewItem();
                    ltvAttDefNamCol.Items.Add(ltviAttDefNam);
                    ltviAttDefNam.Text= AttributeDefinitionCollection[cbbBlkTblRec1.SelectedIndex,i];
                }
			}
        }
        public void CreateAttributeDefinitionCollectionReplaced()
        {
            gpbAttDefNamCol.Controls.Clear();
            //pnAttDefNamCol.Controls.Clear();
            if (cbbBlkTblRec2.Items.Count ==0) return;
            //
            List<string> AttDefNam = new List<string>();
            for (int i = 0; i < AttributeDefinitionCollection.GetLength(1); i++)
			{
                if (AttributeDefinitionCollection[cbbBlkTblRec2.SelectedIndex,i] !=null)
                {
                    AttDefNam.Add(AttributeDefinitionCollection[cbbBlkTblRec2.SelectedIndex, i]);
                }
            }
            AttDefNam.Add("No attribute selected!");
            if (ltvAttDefNamCol.Items.Count != 0)
            {
                cbbAttDefNam2 = new ComboBox[ltvAttDefNamCol.Items.Count];
                for (int i = 0; i < cbbAttDefNam2.Length; i++)
                {
                    cbbAttDefNam2[i] = new ComboBox();
                    cbbAttDefNam2[i].Text = "";
                    cbbAttDefNam2[i].Visible = true;
                    cbbAttDefNam2[i].Location = new Point(4, 12 + i * 30);
                    //cbbAttDefNam2[i].Show();
                    for (int j = 0; j < AttDefNam.Count; j++)
                    {
                        cbbAttDefNam2[i].Items.Add(AttDefNam[j]);
                    }
                    if (i < AttDefNam.Count) cbbAttDefNam2[i].SelectedIndex = i;
                    else cbbAttDefNam2[i].SelectedIndex = AttDefNam.Count - 1;
                    gpbAttDefNamCol.Controls.Add(cbbAttDefNam2[i]);
                }
            }
        }

        public void CreateBlockTableNameCollection()
        {
            cbbBlkTblRec1.Items.Clear();
            cbbBlkTblRec2.Items.Clear();
            foreach (string BlockTableRecordName in BlockTableRecordNameCollection)
            {
                cbbBlkTblRec1.Items.Add(BlockTableRecordName);
                cbbBlkTblRec2.Items.Add(BlockTableRecordName);
            }
            /*
            if (cbbBlkTblRec1.Items.Count != 0)
                cbbBlkTblRec1.SelectedIndex = 0;
            else
                cbbBlkTblRec1.SelectedIndex = -1;
            if (cbbBlkTblRec2.Items.Count != 0)
                cbbBlkTblRec2.SelectedIndex = 0;
            else
                cbbBlkTblRec2.SelectedIndex = -1;
             */
        }

        public void CreateDocumentNameCollection()
        {
            ltvDocColTo.Items.Clear();
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
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
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

        private void btnBlkRef_Click(object sender, EventArgs e)
        {
            IsButtonBlockTableRecordName1Clicked = true;
            Close();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsButtonCancelClicked = true; IsClosed = true;
            Close();
        }

        private void txtBlkTblRecNam_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtTextPromptValidator_TextChanged(object sender, EventArgs e)
        {
        }

        private void rbtTextPrompt_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnBlkTblRec2_Click(object sender, EventArgs e)
        {
            IsButtonBlockTableRecordName2Clicked=true;
            Close();
        }

        private void cbbBlkTblRec2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbBlkTblRec2.SelectedIndex != -1)
            {
                CreateAttributeDefinitionCollectionReplaced();
            }
        }

        private void cbbBlkTblRec1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbBlkTblRec1.SelectedIndex != -1) 
            {
                CreateAttributeDefinitionCollectionRemoved();
                /*
                if (ltvAttDefNamCol.Items.Count < gpbAttDefNamCol.Controls.Count)
                {
                    for (int i = ltvAttDefNamCol.Items.Count; i < gpbAttDefNamCol.Controls.Count; i++)
                    {
                        gpbAttDefNamCol.Controls.Remove(gpbAttDefNamCol.Controls[i]);
                    }
                }
                 */
                if (cbbBlkTblRec2.SelectedIndex != -1)
                CreateAttributeDefinitionCollectionReplaced();
            }
        }

        
    }
}
