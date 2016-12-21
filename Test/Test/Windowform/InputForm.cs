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
    public partial class InputForm : Form
    {
        public string[] AttributeDefinitionCollection;
        public string[] PlotDeviceCollection;
        public string[,] CanonicalMediaNameCollection;
        public string[] PlotStyleCollection;
        public string[] DocumentNameCollection;
        public bool IsButtonBlockReferenceClicked;
        public bool IsButtonTextPromptClicked;
        public bool IsButtonOkClicked;
        public bool IsButtonCancelClicked;
        public bool IsClosed;

        public string[] IsTextPromptInvalid = new string[] { "Text Invalided", "Text Valided" };
        //private MainClass acMain;
        //private SubClass acSub;

        public InputForm()
        {
            InitializeComponent();
            txtTextPromptValidator.Text = IsTextPromptInvalid[0];
            rbtTextPrompt.Enabled = false;
            rbtAttribute.Enabled = false;
            rbtIndex.Checked = true;
            btnCheck.Visible = false;
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
           
        }
        public void CreateAttributeDefinitionCollection()
        {
            cbbAttDef.Items.Clear();
            foreach (string AttributeDefinition in AttributeDefinitionCollection)
            {
                cbbAttDef.Items.Add(AttributeDefinition);
            }
            if (cbbAttDef.Items.Count != 0)
                cbbAttDef.SelectedIndex = 0;
            else
                cbbAttDef.SelectedIndex = -1;
        }

        public void CreatePlotDeviceCollection()
        {
            cbbPlDev.Items.Clear();
            foreach (string PlotDevice in PlotDeviceCollection)
            {
                cbbPlDev.Items.Add(PlotDevice);
            }
        }
        public void CreateCanonicalMediaNameCollection()
        {
            cbbCanMed.Items.Clear();
            for (int i = 0; i < CanonicalMediaNameCollection.GetLength(1); i++)
            {
                if (!string.IsNullOrEmpty(CanonicalMediaNameCollection[cbbPlDev.SelectedIndex, i]))
                    cbbCanMed.Items.Add(CanonicalMediaNameCollection[cbbPlDev.SelectedIndex, i]);
            }
            cbbCanMed.SelectedIndex = 0;
        }
        public void CreatePlotStyleCollection()
        {
            cbbPlStl.Items.Clear();
            foreach (string PlotStyle in PlotStyleCollection)
            {
                cbbPlStl.Items.Add(PlotStyle);
            }
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

        private void btnCheck_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(cbbAttDef.SelectedIndex.ToString());
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
            IsButtonBlockReferenceClicked = true;
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            IsButtonTextPromptClicked = true;
            if (txtBlkTblRecNam.Text.Equals(""))
                MessageBox.Show("Please pick a Title Block!");
            else Close();
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

        private void cbbPlDev_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateCanonicalMediaNameCollection();
        }

        
    }
}
