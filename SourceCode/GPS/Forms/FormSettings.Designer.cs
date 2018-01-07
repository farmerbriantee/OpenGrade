namespace OpenGrade
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabVehicle = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.chkIsPivotBehindAntenna = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.nudAntennaPivot = new System.Windows.Forms.NumericUpDown();
            this.nudWheelbase = new System.Windows.Forms.NumericUpDown();
            this.nudAntennaHeight = new System.Windows.Forms.NumericUpDown();
            this.tabTool = new System.Windows.Forms.TabPage();
            this.tabWorkSwitch = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkWorkSwActiveLow = new System.Windows.Forms.CheckBox();
            this.chkEnableWorkSwitch = new System.Windows.Forms.CheckBox();
            this.tabDisplay = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.nudMinFixStepDistance = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.lblInchesCm = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.bntOK = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabVehicle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAntennaPivot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWheelbase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAntennaHeight)).BeginInit();
            this.tabWorkSwitch.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabDisplay.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinFixStepDistance)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabVehicle);
            this.tabControl1.Controls.Add(this.tabTool);
            this.tabControl1.Controls.Add(this.tabWorkSwitch);
            this.tabControl1.Controls.Add(this.tabDisplay);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            // 
            // tabVehicle
            // 
            this.tabVehicle.BackgroundImage = global::OpenGrade.Properties.Resources.VehicleSettings;
            resources.ApplyResources(this.tabVehicle, "tabVehicle");
            this.tabVehicle.Controls.Add(this.label5);
            this.tabVehicle.Controls.Add(this.label2);
            this.tabVehicle.Controls.Add(this.label1);
            this.tabVehicle.Controls.Add(this.label15);
            this.tabVehicle.Controls.Add(this.label28);
            this.tabVehicle.Controls.Add(this.label26);
            this.tabVehicle.Controls.Add(this.label12);
            this.tabVehicle.Controls.Add(this.chkIsPivotBehindAntenna);
            this.tabVehicle.Controls.Add(this.label13);
            this.tabVehicle.Controls.Add(this.label25);
            this.tabVehicle.Controls.Add(this.label18);
            this.tabVehicle.Controls.Add(this.nudAntennaPivot);
            this.tabVehicle.Controls.Add(this.nudWheelbase);
            this.tabVehicle.Controls.Add(this.nudAntennaHeight);
            this.tabVehicle.Name = "tabVehicle";
            this.tabVehicle.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // label28
            // 
            resources.ApplyResources(this.label28, "label28");
            this.label28.Name = "label28";
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // chkIsPivotBehindAntenna
            // 
            resources.ApplyResources(this.chkIsPivotBehindAntenna, "chkIsPivotBehindAntenna");
            this.chkIsPivotBehindAntenna.Image = global::OpenGrade.Properties.Resources.PivotBehind;
            this.chkIsPivotBehindAntenna.Name = "chkIsPivotBehindAntenna";
            this.chkIsPivotBehindAntenna.UseVisualStyleBackColor = true;
            this.chkIsPivotBehindAntenna.CheckedChanged += new System.EventHandler(this.chkIsPivotBehindAntenna_CheckedChanged);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.Name = "label25";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // nudAntennaPivot
            // 
            resources.ApplyResources(this.nudAntennaPivot, "nudAntennaPivot");
            this.nudAntennaPivot.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudAntennaPivot.Name = "nudAntennaPivot";
            this.nudAntennaPivot.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.nudAntennaPivot.ValueChanged += new System.EventHandler(this.nudAntennaPivot_ValueChanged);
            // 
            // nudWheelbase
            // 
            resources.ApplyResources(this.nudWheelbase, "nudWheelbase");
            this.nudWheelbase.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudWheelbase.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudWheelbase.Name = "nudWheelbase";
            this.nudWheelbase.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.nudWheelbase.ValueChanged += new System.EventHandler(this.nudWheelbase_ValueChanged);
            // 
            // nudAntennaHeight
            // 
            resources.ApplyResources(this.nudAntennaHeight, "nudAntennaHeight");
            this.nudAntennaHeight.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudAntennaHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudAntennaHeight.Name = "nudAntennaHeight";
            this.nudAntennaHeight.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.nudAntennaHeight.ValueChanged += new System.EventHandler(this.nudAntennaHeight_ValueChanged);
            // 
            // tabTool
            // 
            this.tabTool.BackgroundImage = global::OpenGrade.Properties.Resources.ImplementSettings;
            resources.ApplyResources(this.tabTool, "tabTool");
            this.tabTool.Name = "tabTool";
            this.tabTool.UseVisualStyleBackColor = true;
            // 
            // tabWorkSwitch
            // 
            this.tabWorkSwitch.BackgroundImage = global::OpenGrade.Properties.Resources.WorkSwitch;
            resources.ApplyResources(this.tabWorkSwitch, "tabWorkSwitch");
            this.tabWorkSwitch.Controls.Add(this.groupBox3);
            this.tabWorkSwitch.Name = "tabWorkSwitch";
            this.tabWorkSwitch.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkWorkSwActiveLow);
            this.groupBox3.Controls.Add(this.chkEnableWorkSwitch);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // chkWorkSwActiveLow
            // 
            resources.ApplyResources(this.chkWorkSwActiveLow, "chkWorkSwActiveLow");
            this.chkWorkSwActiveLow.Name = "chkWorkSwActiveLow";
            this.chkWorkSwActiveLow.UseVisualStyleBackColor = true;
            this.chkWorkSwActiveLow.CheckedChanged += new System.EventHandler(this.chkWorkSwActiveLow_CheckedChanged);
            // 
            // chkEnableWorkSwitch
            // 
            resources.ApplyResources(this.chkEnableWorkSwitch, "chkEnableWorkSwitch");
            this.chkEnableWorkSwitch.Name = "chkEnableWorkSwitch";
            this.chkEnableWorkSwitch.UseVisualStyleBackColor = true;
            this.chkEnableWorkSwitch.CheckedChanged += new System.EventHandler(this.chkEnableWorkSwitch_CheckedChanged);
            // 
            // tabDisplay
            // 
            this.tabDisplay.Controls.Add(this.groupBox4);
            resources.ApplyResources(this.tabDisplay, "tabDisplay");
            this.tabDisplay.Name = "tabDisplay";
            this.tabDisplay.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label21);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Controls.Add(this.nudMinFixStepDistance);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // nudMinFixStepDistance
            // 
            this.nudMinFixStepDistance.DecimalPlaces = 1;
            resources.ApplyResources(this.nudMinFixStepDistance, "nudMinFixStepDistance");
            this.nudMinFixStepDistance.Increment = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.nudMinFixStepDistance.Maximum = new decimal(new int[] {
            40,
            0,
            0,
            65536});
            this.nudMinFixStepDistance.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            this.nudMinFixStepDistance.Name = "nudMinFixStepDistance";
            this.nudMinFixStepDistance.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.nudMinFixStepDistance.ValueChanged += new System.EventHandler(this.nudMinFixStepDistance_ValueChanged);
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // lblInchesCm
            // 
            resources.ApplyResources(this.lblInchesCm, "lblInchesCm");
            this.lblInchesCm.ForeColor = System.Drawing.Color.Red;
            this.lblInchesCm.Name = "lblInchesCm";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::OpenGrade.Properties.Resources.Cancel64;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // bntOK
            // 
            resources.ApplyResources(this.bntOK, "bntOK");
            this.bntOK.Image = global::OpenGrade.Properties.Resources.OK64;
            this.bntOK.Name = "bntOK";
            this.bntOK.UseVisualStyleBackColor = true;
            this.bntOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FormSettings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ControlBox = false;
            this.Controls.Add(this.lblInchesCm);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.bntOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabVehicle.ResumeLayout(false);
            this.tabVehicle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAntennaPivot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWheelbase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAntennaHeight)).EndInit();
            this.tabWorkSwitch.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabDisplay.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinFixStepDistance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button bntOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabPage tabDisplay;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TabPage tabTool;
        private System.Windows.Forms.TabPage tabWorkSwitch;
        private System.Windows.Forms.CheckBox chkEnableWorkSwitch;
        private System.Windows.Forms.CheckBox chkWorkSwActiveLow;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown nudMinFixStepDistance;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lblInchesCm;
        private System.Windows.Forms.TabPage tabVehicle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox chkIsPivotBehindAntenna;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.NumericUpDown nudAntennaPivot;
        private System.Windows.Forms.NumericUpDown nudWheelbase;
        private System.Windows.Forms.NumericUpDown nudAntennaHeight;
    }
}