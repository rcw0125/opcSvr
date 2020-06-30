namespace cfyc
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lAlloyConfigBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colcode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcode_des = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colc_cal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsi_cal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMn_cal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCr_cal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSaveEnable = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.lAlloyRateBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colsteelgrade = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colmn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSaveEnable1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.lAlloyConfigBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lAlloyRateBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // lAlloyConfigBindingSource
            // 
            this.lAlloyConfigBindingSource.DataSource = typeof(cfyc.L_Alloy_Config);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button2);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridControl1);
            this.splitContainer1.Size = new System.Drawing.Size(810, 478);
            this.splitContainer1.SplitterDistance = 57;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(377, 13);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 31);
            this.button2.TabIndex = 7;
            this.button2.Text = "修改";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(186, 13);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 31);
            this.button1.TabIndex = 6;
            this.button1.Text = "查询";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.lAlloyConfigBindingSource;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(4);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(810, 416);
            this.gridControl1.TabIndex = 4;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 12F);
            this.gridView1.Appearance.Row.Options.UseFont = true;
            this.gridView1.Appearance.TopNewRow.Font = new System.Drawing.Font("Tahoma", 12F);
            this.gridView1.Appearance.TopNewRow.Options.UseFont = true;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colcode,
            this.colcode_des,
            this.gridColumn1,
            this.colc_cal,
            this.colsi_cal,
            this.colMn_cal,
            this.colCr_cal,
            this.colSaveEnable,
            this.gridColumn2});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // colcode
            // 
            this.colcode.FieldName = "code";
            this.colcode.Name = "colcode";
            this.colcode.OptionsColumn.AllowEdit = false;
            this.colcode.Visible = true;
            this.colcode.VisibleIndex = 0;
            // 
            // colcode_des
            // 
            this.colcode_des.FieldName = "code_des";
            this.colcode_des.Name = "colcode_des";
            this.colcode_des.OptionsColumn.AllowEdit = false;
            this.colcode_des.Visible = true;
            this.colcode_des.VisibleIndex = 1;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "增碳剂";
            this.gridColumn1.FieldName = "zcj";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // colc_cal
            // 
            this.colc_cal.Caption = "C ";
            this.colc_cal.FieldName = "c_cal";
            this.colc_cal.Name = "colc_cal";
            this.colc_cal.Visible = true;
            this.colc_cal.VisibleIndex = 2;
            // 
            // colsi_cal
            // 
            this.colsi_cal.Caption = "Si";
            this.colsi_cal.FieldName = "si_cal";
            this.colsi_cal.Name = "colsi_cal";
            this.colsi_cal.Visible = true;
            this.colsi_cal.VisibleIndex = 3;
            // 
            // colMn_cal
            // 
            this.colMn_cal.Caption = "Mn";
            this.colMn_cal.FieldName = "Mn_cal";
            this.colMn_cal.Name = "colMn_cal";
            this.colMn_cal.Visible = true;
            this.colMn_cal.VisibleIndex = 4;
            // 
            // colCr_cal
            // 
            this.colCr_cal.Caption = "Cr";
            this.colCr_cal.FieldName = "Cr_cal";
            this.colCr_cal.Name = "colCr_cal";
            this.colCr_cal.Visible = true;
            this.colCr_cal.VisibleIndex = 5;
            // 
            // colSaveEnable
            // 
            this.colSaveEnable.FieldName = "SaveEnable";
            this.colSaveEnable.Name = "colSaveEnable";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "吸收率";
            this.gridColumn2.FieldName = "rate";
            this.gridColumn2.Name = "gridColumn2";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 3600000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(816, 507);
            this.xtraTabControl1.TabIndex = 1;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.splitContainer1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(810, 478);
            this.xtraTabPage1.Text = "合金品位    ";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.splitContainer2);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(810, 478);
            this.xtraTabPage2.Text = "钢种吸收率";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.button4);
            this.splitContainer2.Panel1.Controls.Add(this.button3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.gridControl2);
            this.splitContainer2.Size = new System.Drawing.Size(810, 478);
            this.splitContainer2.SplitterDistance = 60;
            this.splitContainer2.TabIndex = 0;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(442, 17);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(81, 30);
            this.button4.TabIndex = 0;
            this.button4.Text = "修改";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(294, 17);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(81, 30);
            this.button3.TabIndex = 0;
            this.button3.Text = "查询";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // gridControl2
            // 
            this.gridControl2.DataSource = this.lAlloyRateBindingSource;
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.Location = new System.Drawing.Point(0, 0);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(810, 414);
            this.gridControl2.TabIndex = 0;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // lAlloyRateBindingSource
            // 
            this.lAlloyRateBindingSource.DataSource = typeof(cfyc.L_Alloy_Rate);
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colsteelgrade,
            this.colc,
            this.colsi,
            this.colmn,
            this.colcr,
            this.colSaveEnable1,
            this.gridColumn3});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            // 
            // colsteelgrade
            // 
            this.colsteelgrade.FieldName = "steelgrade";
            this.colsteelgrade.Name = "colsteelgrade";
            this.colsteelgrade.OptionsColumn.AllowEdit = false;
            this.colsteelgrade.Visible = true;
            this.colsteelgrade.VisibleIndex = 0;
            // 
            // colc
            // 
            this.colc.FieldName = "c";
            this.colc.Name = "colc";
            this.colc.Visible = true;
            this.colc.VisibleIndex = 2;
            // 
            // colsi
            // 
            this.colsi.FieldName = "si";
            this.colsi.Name = "colsi";
            this.colsi.Visible = true;
            this.colsi.VisibleIndex = 3;
            // 
            // colmn
            // 
            this.colmn.FieldName = "mn";
            this.colmn.Name = "colmn";
            this.colmn.Visible = true;
            this.colmn.VisibleIndex = 4;
            // 
            // colcr
            // 
            this.colcr.FieldName = "cr";
            this.colcr.Name = "colcr";
            this.colcr.Visible = true;
            this.colcr.VisibleIndex = 5;
            // 
            // colSaveEnable1
            // 
            this.colSaveEnable1.FieldName = "SaveEnable";
            this.colSaveEnable1.Name = "colSaveEnable1";
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "钢种分类";
            this.gridColumn3.FieldName = "steelgrade_des";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 507);
            this.Controls.Add(this.xtraTabControl1);
            this.Font = new System.Drawing.Font("宋体", 12F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "合金品位配置";
            ((System.ComponentModel.ISupportInitialize)(this.lAlloyConfigBindingSource)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lAlloyRateBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource lAlloyConfigBindingSource;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colcode;
        private DevExpress.XtraGrid.Columns.GridColumn colcode_des;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn colc_cal;
        private DevExpress.XtraGrid.Columns.GridColumn colsi_cal;
        private DevExpress.XtraGrid.Columns.GridColumn colMn_cal;
        private DevExpress.XtraGrid.Columns.GridColumn colCr_cal;
        private DevExpress.XtraGrid.Columns.GridColumn colSaveEnable;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private System.Windows.Forms.BindingSource lAlloyRateBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colsteelgrade;
        private DevExpress.XtraGrid.Columns.GridColumn colc;
        private DevExpress.XtraGrid.Columns.GridColumn colsi;
        private DevExpress.XtraGrid.Columns.GridColumn colmn;
        private DevExpress.XtraGrid.Columns.GridColumn colcr;
        private DevExpress.XtraGrid.Columns.GridColumn colSaveEnable1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
    }
}

