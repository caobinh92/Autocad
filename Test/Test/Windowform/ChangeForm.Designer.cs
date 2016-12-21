namespace Test
{
    partial class ChangeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCheck = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ltvDocColFrom = new System.Windows.Forms.ListView();
            this.ltvDocColTo = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnClick = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnBlkTblRec1 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbbBlkTblRec1 = new System.Windows.Forms.ComboBox();
            this.cbbBlkTblRec2 = new System.Windows.Forms.ComboBox();
            this.btnBlkTblRec2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ltvAttDefNamCol = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gpbAttDefNamCol = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(1069, 114);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "OK";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(1069, 69);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(75, 23);
            this.btnCheck.TabIndex = 6;
            this.btnCheck.Text = "Check";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(607, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Title Block removed";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(57, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(145, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Document not to be replaced";
            // 
            // ltvDocColFrom
            // 
            this.ltvDocColFrom.Location = new System.Drawing.Point(33, 87);
            this.ltvDocColFrom.Name = "ltvDocColFrom";
            this.ltvDocColFrom.Size = new System.Drawing.Size(177, 199);
            this.ltvDocColFrom.TabIndex = 21;
            this.ltvDocColFrom.UseCompatibleStateImageBehavior = false;
            this.ltvDocColFrom.View = System.Windows.Forms.View.List;
            // 
            // ltvDocColTo
            // 
            this.ltvDocColTo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.ltvDocColTo.Location = new System.Drawing.Point(315, 87);
            this.ltvDocColTo.Name = "ltvDocColTo";
            this.ltvDocColTo.Size = new System.Drawing.Size(173, 199);
            this.ltvDocColTo.TabIndex = 22;
            this.ltvDocColTo.UseCompatibleStateImageBehavior = false;
            this.ltvDocColTo.View = System.Windows.Forms.View.List;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Document Name";
            this.columnHeader2.Width = 237;
            // 
            // btnClick
            // 
            this.btnClick.Location = new System.Drawing.Point(247, 130);
            this.btnClick.Name = "btnClick";
            this.btnClick.Size = new System.Drawing.Size(37, 23);
            this.btnClick.TabIndex = 23;
            this.btnClick.Text = ">";
            this.btnClick.UseVisualStyleBackColor = true;
            this.btnClick.Click += new System.EventHandler(this.btnClick_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(247, 208);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(37, 23);
            this.button1.TabIndex = 25;
            this.button1.Text = "<";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(247, 159);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(37, 23);
            this.button2.TabIndex = 26;
            this.button2.Text = ">>";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(247, 237);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(37, 23);
            this.button3.TabIndex = 27;
            this.button3.Text = "<<";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(333, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Document to be replaced";
            // 
            // btnBlkTblRec1
            // 
            this.btnBlkTblRec1.Location = new System.Drawing.Point(709, 98);
            this.btnBlkTblRec1.Name = "btnBlkTblRec1";
            this.btnBlkTblRec1.Size = new System.Drawing.Size(29, 23);
            this.btnBlkTblRec1.TabIndex = 32;
            this.btnBlkTblRec1.Text = "...";
            this.btnBlkTblRec1.UseVisualStyleBackColor = true;
            this.btnBlkTblRec1.Click += new System.EventHandler(this.btnBlkRef_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(1069, 159);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 33;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbbBlkTblRec1
            // 
            this.cbbBlkTblRec1.FormattingEnabled = true;
            this.cbbBlkTblRec1.Location = new System.Drawing.Point(574, 100);
            this.cbbBlkTblRec1.Name = "cbbBlkTblRec1";
            this.cbbBlkTblRec1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbbBlkTblRec1.Size = new System.Drawing.Size(118, 21);
            this.cbbBlkTblRec1.TabIndex = 34;
            this.cbbBlkTblRec1.SelectedIndexChanged += new System.EventHandler(this.cbbBlkTblRec1_SelectedIndexChanged);
            // 
            // cbbBlkTblRec2
            // 
            this.cbbBlkTblRec2.FormattingEnabled = true;
            this.cbbBlkTblRec2.Location = new System.Drawing.Point(801, 100);
            this.cbbBlkTblRec2.Name = "cbbBlkTblRec2";
            this.cbbBlkTblRec2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbbBlkTblRec2.Size = new System.Drawing.Size(118, 21);
            this.cbbBlkTblRec2.TabIndex = 37;
            this.cbbBlkTblRec2.SelectedIndexChanged += new System.EventHandler(this.cbbBlkTblRec2_SelectedIndexChanged);
            // 
            // btnBlkTblRec2
            // 
            this.btnBlkTblRec2.Location = new System.Drawing.Point(936, 98);
            this.btnBlkTblRec2.Name = "btnBlkTblRec2";
            this.btnBlkTblRec2.Size = new System.Drawing.Size(29, 23);
            this.btnBlkTblRec2.TabIndex = 36;
            this.btnBlkTblRec2.Text = "...";
            this.btnBlkTblRec2.UseVisualStyleBackColor = true;
            this.btnBlkTblRec2.Click += new System.EventHandler(this.btnBlkTblRec2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(840, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 35;
            this.label2.Text = "Title Block replace";
            // 
            // ltvAttDefNamCol
            // 
            this.ltvAttDefNamCol.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.ltvAttDefNamCol.Location = new System.Drawing.Point(574, 148);
            this.ltvAttDefNamCol.Name = "ltvAttDefNamCol";
            this.ltvAttDefNamCol.Size = new System.Drawing.Size(155, 528);
            this.ltvAttDefNamCol.TabIndex = 38;
            this.ltvAttDefNamCol.UseCompatibleStateImageBehavior = false;
            this.ltvAttDefNamCol.View = System.Windows.Forms.View.Tile;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Document Name";
            this.columnHeader1.Width = 237;
            // 
            // gpbAttDefNamCol
            // 
            this.gpbAttDefNamCol.Location = new System.Drawing.Point(775, 148);
            this.gpbAttDefNamCol.Name = "gpbAttDefNamCol";
            this.gpbAttDefNamCol.Size = new System.Drawing.Size(166, 528);
            this.gpbAttDefNamCol.TabIndex = 40;
            this.gpbAttDefNamCol.TabStop = false;
            // 
            // ChangeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1279, 688);
            this.Controls.Add(this.gpbAttDefNamCol);
            this.Controls.Add(this.ltvAttDefNamCol);
            this.Controls.Add(this.cbbBlkTblRec2);
            this.Controls.Add(this.btnBlkTblRec2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbbBlkTblRec1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBlkTblRec1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnClick);
            this.Controls.Add(this.ltvDocColTo);
            this.Controls.Add(this.ltvDocColFrom);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.btnClose);
            this.Name = "ChangeForm";
            this.Text = "InputForm";
            this.Load += new System.EventHandler(this.InputForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        public System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListView ltvDocColFrom;
        private System.Windows.Forms.ListView ltvDocColTo;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnClick;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.Button btnBlkTblRec1;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.ComboBox cbbBlkTblRec1;
        public System.Windows.Forms.ComboBox cbbBlkTblRec2;
        public System.Windows.Forms.Button btnBlkTblRec2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView ltvAttDefNamCol;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        public System.Windows.Forms.ComboBox[] cbbAttDefNam2;
        public System.Windows.Forms.GroupBox gpbAttDefNamCol;
    }
}