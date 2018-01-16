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
            this.nudMinSlope = new System.Windows.Forms.NumericUpDown();
            this.nudToolWidth = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.nudAntennaHeight = new System.Windows.Forms.NumericUpDown();
            this.tabDisplay = new System.Windows.Forms.TabPage();
            this.label17 = new System.Windows.Forms.Label();
            this.lblInchesCm = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.bntOK = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabVehicle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinSlope)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudToolWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAntennaHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabVehicle);
            this.tabControl1.Controls.Add(this.tabDisplay);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            // 
            // tabVehicle
            // 
            resources.ApplyResources(this.tabVehicle, "tabVehicle");
            this.tabVehicle.Controls.Add(this.nudMinSlope);
            this.tabVehicle.Controls.Add(this.nudToolWidth);
            this.tabVehicle.Controls.Add(this.label5);
            this.tabVehicle.Controls.Add(this.label2);
            this.tabVehicle.Controls.Add(this.label1);
            this.tabVehicle.Controls.Add(this.label15);
            this.tabVehicle.Controls.Add(this.label28);
            this.tabVehicle.Controls.Add(this.label26);
            this.tabVehicle.Controls.Add(this.label12);
            this.tabVehicle.Controls.Add(this.label13);
            this.tabVehicle.Controls.Add(this.label25);
            this.tabVehicle.Controls.Add(this.label18);
            this.tabVehicle.Controls.Add(this.nudAntennaHeight);
            this.tabVehicle.Name = "tabVehicle";
            this.tabVehicle.UseVisualStyleBackColor = true;
            // 
            // nudMinSlope
            // 
            this.nudMinSlope.DecimalPlaces = 2;
            resources.ApplyResources(this.nudMinSlope, "nudMinSlope");
            this.nudMinSlope.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudMinSlope.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudMinSlope.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            this.nudMinSlope.Name = "nudMinSlope";
            this.nudMinSlope.Value = new decimal(new int[] {
            2,
            0,
            0,
            -2147418112});
            this.nudMinSlope.ValueChanged += new System.EventHandler(this.nudMinSlope_ValueChanged);
            // 
            // nudToolWidth
            // 
            resources.ApplyResources(this.nudToolWidth, "nudToolWidth");
            this.nudToolWidth.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudToolWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudToolWidth.Name = "nudToolWidth";
            this.nudToolWidth.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.nudToolWidth.ValueChanged += new System.EventHandler(this.nudToolWidth_ValueChanged);
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
            // tabDisplay
            // 
            resources.ApplyResources(this.tabDisplay, "tabDisplay");
            this.tabDisplay.Name = "tabDisplay";
            this.tabDisplay.UseVisualStyleBackColor = true;
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
            ((System.ComponentModel.ISupportInitialize)(this.nudMinSlope)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudToolWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAntennaHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button bntOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabPage tabDisplay;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lblInchesCm;
        private System.Windows.Forms.TabPage tabVehicle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.NumericUpDown nudAntennaHeight;
        private System.Windows.Forms.NumericUpDown nudMinSlope;
        private System.Windows.Forms.NumericUpDown nudToolWidth;
    }
}