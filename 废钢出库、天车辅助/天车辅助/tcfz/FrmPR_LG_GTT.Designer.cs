namespace tcfz
{
    partial class FrmPR_LG_GTT
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraScheduler.TimeScaleYear timeScaleYear1 = new DevExpress.XtraScheduler.TimeScaleYear();
            DevExpress.XtraScheduler.TimeScaleQuarter timeScaleQuarter1 = new DevExpress.XtraScheduler.TimeScaleQuarter();
            DevExpress.XtraScheduler.TimeScaleMonth timeScaleMonth1 = new DevExpress.XtraScheduler.TimeScaleMonth();
            DevExpress.XtraScheduler.TimeScaleWeek timeScaleWeek1 = new DevExpress.XtraScheduler.TimeScaleWeek();
            DevExpress.XtraScheduler.TimeScaleDay timeScaleDay1 = new DevExpress.XtraScheduler.TimeScaleDay();
            DevExpress.XtraScheduler.TimeScaleHour timeScaleHour1 = new DevExpress.XtraScheduler.TimeScaleHour();
            DevExpress.XtraScheduler.TimeScale15Minutes timeScale15Minutes1 = new DevExpress.XtraScheduler.TimeScale15Minutes();
            DevExpress.XtraScheduler.TimeRuler timeRuler1 = new DevExpress.XtraScheduler.TimeRuler();
            DevExpress.XtraScheduler.TimeRuler timeRuler2 = new DevExpress.XtraScheduler.TimeRuler();
            DevExpress.XtraScheduler.TimeScaleYear timeScaleYear2 = new DevExpress.XtraScheduler.TimeScaleYear();
            DevExpress.XtraScheduler.TimeScaleQuarter timeScaleQuarter2 = new DevExpress.XtraScheduler.TimeScaleQuarter();
            DevExpress.XtraScheduler.TimeScaleMonth timeScaleMonth2 = new DevExpress.XtraScheduler.TimeScaleMonth();
            DevExpress.XtraScheduler.TimeScaleWeek timeScaleWeek2 = new DevExpress.XtraScheduler.TimeScaleWeek();
            DevExpress.XtraScheduler.TimeScaleDay timeScaleDay2 = new DevExpress.XtraScheduler.TimeScaleDay();
            DevExpress.XtraScheduler.TimeScaleHour timeScaleHour2 = new DevExpress.XtraScheduler.TimeScaleHour();
            DevExpress.XtraScheduler.TimeScale15Minutes timeScale15Minutes2 = new DevExpress.XtraScheduler.TimeScale15Minutes();
            DevExpress.XtraScheduler.TimeRuler timeRuler3 = new DevExpress.XtraScheduler.TimeRuler();
            this.schedulerStorage1 = new DevExpress.XtraScheduler.SchedulerStorage(this.components);
            this.schedulerControl1 = new DevExpress.XtraScheduler.SchedulerControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btn_Query = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerStorage1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerControl1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // schedulerStorage1
            // 
            this.schedulerStorage1.RemindersCheckInterval = 10000;
            // 
            // schedulerControl1
            // 
            this.schedulerControl1.ActiveViewType = DevExpress.XtraScheduler.SchedulerViewType.Gantt;
            this.schedulerControl1.Appearance.Appointment.Options.UseTextOptions = true;
            this.schedulerControl1.Appearance.Appointment.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.schedulerControl1.DataStorage = this.schedulerStorage1;
            this.schedulerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schedulerControl1.GroupType = DevExpress.XtraScheduler.SchedulerGroupType.Resource;
            this.schedulerControl1.Location = new System.Drawing.Point(0, 0);
            this.schedulerControl1.Name = "schedulerControl1";
            this.schedulerControl1.OptionsCustomization.AllowAppointmentCopy = DevExpress.XtraScheduler.UsedAppointmentType.None;
            this.schedulerControl1.OptionsCustomization.AllowAppointmentCreate = DevExpress.XtraScheduler.UsedAppointmentType.None;
            this.schedulerControl1.OptionsCustomization.AllowAppointmentDelete = DevExpress.XtraScheduler.UsedAppointmentType.None;
            this.schedulerControl1.OptionsCustomization.AllowAppointmentDrag = DevExpress.XtraScheduler.UsedAppointmentType.None;
            this.schedulerControl1.OptionsCustomization.AllowAppointmentDragBetweenResources = DevExpress.XtraScheduler.UsedAppointmentType.None;
            this.schedulerControl1.OptionsCustomization.AllowAppointmentEdit = DevExpress.XtraScheduler.UsedAppointmentType.None;
            this.schedulerControl1.OptionsCustomization.AllowAppointmentMultiSelect = false;
            this.schedulerControl1.OptionsCustomization.AllowDisplayAppointmentDependencyForm = DevExpress.XtraScheduler.AllowDisplayAppointmentDependencyForm.Never;
            this.schedulerControl1.OptionsCustomization.AllowDisplayAppointmentForm = DevExpress.XtraScheduler.AllowDisplayAppointmentForm.Never;
            timeScaleYear1.Enabled = false;
            timeScaleQuarter1.Enabled = false;
            timeScaleMonth1.Enabled = false;
            timeScaleHour1.Enabled = false;
            timeScale15Minutes1.Width = 100;
            this.schedulerControl1.OptionsRangeControl.Scales.Add(timeScaleYear1);
            this.schedulerControl1.OptionsRangeControl.Scales.Add(timeScaleQuarter1);
            this.schedulerControl1.OptionsRangeControl.Scales.Add(timeScaleMonth1);
            this.schedulerControl1.OptionsRangeControl.Scales.Add(timeScaleWeek1);
            this.schedulerControl1.OptionsRangeControl.Scales.Add(timeScaleDay1);
            this.schedulerControl1.OptionsRangeControl.Scales.Add(timeScaleHour1);
            this.schedulerControl1.OptionsRangeControl.Scales.Add(timeScale15Minutes1);
            this.schedulerControl1.OptionsView.NavigationButtons.Visibility = DevExpress.XtraScheduler.NavigationButtonVisibility.Never;
            this.schedulerControl1.Size = new System.Drawing.Size(1184, 861);
            this.schedulerControl1.Start = new System.DateTime(2019, 6, 3, 0, 0, 0, 0);
            this.schedulerControl1.TabIndex = 4;
            this.schedulerControl1.Text = "schedulerControl1";
            this.schedulerControl1.Views.DayView.TimeRulers.Add(timeRuler1);
            this.schedulerControl1.Views.FullWeekView.Enabled = true;
            this.schedulerControl1.Views.FullWeekView.TimeRulers.Add(timeRuler2);
            timeScaleYear2.Enabled = false;
            timeScaleQuarter2.Enabled = false;
            timeScaleMonth2.Enabled = false;
            timeScaleHour2.Enabled = false;
            this.schedulerControl1.Views.GanttView.Scales.Add(timeScaleYear2);
            this.schedulerControl1.Views.GanttView.Scales.Add(timeScaleQuarter2);
            this.schedulerControl1.Views.GanttView.Scales.Add(timeScaleMonth2);
            this.schedulerControl1.Views.GanttView.Scales.Add(timeScaleWeek2);
            this.schedulerControl1.Views.GanttView.Scales.Add(timeScaleDay2);
            this.schedulerControl1.Views.GanttView.Scales.Add(timeScaleHour2);
            this.schedulerControl1.Views.GanttView.Scales.Add(timeScale15Minutes2);
            this.schedulerControl1.Views.GanttView.WorkTime = new DevExpress.XtraScheduler.WorkTimeInterval(System.TimeSpan.Parse("00:00:00"), System.TimeSpan.Parse("23:59:59"));
            this.schedulerControl1.Views.WeekView.Enabled = false;
            this.schedulerControl1.Views.WorkWeekView.TimeRulers.Add(timeRuler3);
            this.schedulerControl1.PopupMenuShowing += new DevExpress.XtraScheduler.PopupMenuShowingEventHandler(this.schedulerControl1_PopupMenuShowing);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.btn_Query);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1184, 39);
            this.panel1.TabIndex = 5;
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("宋体", 12F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "1#炉",
            "2#炉",
            "3#炉",
            "4#炉",
            "1#LF",
            "2#LF",
            "3#LF",
            "RH",
            "3#机",
            "4#机",
            "5#机"});
            this.comboBox1.Location = new System.Drawing.Point(151, 11);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 141;
            // 
            // btn_Query
            // 
            this.btn_Query.ImageOptions.ImageUri.Uri = "Zoom;Size16x16";
            this.btn_Query.Location = new System.Drawing.Point(311, 11);
            this.btn_Query.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.btn_Query.Name = "btn_Query";
            this.btn_Query.Size = new System.Drawing.Size(85, 23);
            this.btn_Query.TabIndex = 140;
            this.btn_Query.Text = "查询";
            this.btn_Query.Click += new System.EventHandler(this.btn_Query_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择区域：";
            // 
            // FrmPR_LG_GTT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1184, 861);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.schedulerControl1);
            this.Name = "FrmPR_LG_GTT";
            this.Text = "生产计划甘特图";
            this.Load += new System.EventHandler(this.FrmPR_LG_GTT_Load);
            ((System.ComponentModel.ISupportInitialize)(this.schedulerStorage1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerControl1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraScheduler.SchedulerStorage schedulerStorage1;
        private DevExpress.XtraScheduler.SchedulerControl schedulerControl1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btn_Query;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}