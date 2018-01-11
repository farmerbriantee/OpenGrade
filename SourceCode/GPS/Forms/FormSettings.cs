//Please, if you use this, share the improvements

using System;
using System.IO;
using System.Windows.Forms;

namespace OpenGrade
{
    public partial class FormSettings : Form
    {
       //class variables
        private readonly FormGPS mf = null;

        private double antennaHeight, antennaPivot, wheelbase;

        private bool isPivotBehindAntenna;


        private decimal minFixStepDistance;

        private bool isWorkSwEn, isWorkSwActiveLow;

        private readonly double metImp2m, m2MetImp;

        //constructor
        public FormSettings(Form callingForm, int page)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            if (mf.isMetric)
            {
                metImp2m = 0.01;
                m2MetImp = 100.0;
                lblInchesCm.Text = "Centimeters";
            }
            else
            {
                metImp2m = glm.in2m;
                m2MetImp = glm.m2in;
                lblInchesCm.Text = "Inches";
            }
            //select the page as per calling menu or button from mainGPS form
            tabControl1.SelectedIndex = page;
        }

        //do any field initializing for form here
        private void FormSettings_Load(object sender, EventArgs e)
        {
            //Vehicle settings to what it is in the settings page------------------------------------------------
            antennaHeight = Properties.Vehicle.Default.setVehicle_antennaHeight;
            antennaPivot = Math.Abs(Properties.Vehicle.Default.setVehicle_antennaPivot);
            wheelbase = Math.Abs(Properties.Vehicle.Default.setVehicle_wheelbase);

            nudAntennaHeight.ValueChanged -= nudAntennaHeight_ValueChanged;
            nudAntennaHeight.Value = (decimal)(antennaHeight * m2MetImp);
            nudAntennaHeight.ValueChanged += nudAntennaHeight_ValueChanged;

            nudAntennaPivot.ValueChanged -= nudAntennaPivot_ValueChanged;
            nudAntennaPivot.Value = (decimal)(antennaPivot * m2MetImp);
            nudAntennaPivot.ValueChanged += nudAntennaPivot_ValueChanged;

            nudWheelbase.ValueChanged -= nudWheelbase_ValueChanged;
            nudWheelbase.Value = (decimal)(wheelbase * m2MetImp);
            nudWheelbase.ValueChanged += nudWheelbase_ValueChanged;

            isPivotBehindAntenna = Properties.Vehicle.Default.setVehicle_isPivotBehindAntenna;

            chkIsPivotBehindAntenna.CheckedChanged -= chkIsPivotBehindAntenna_CheckedChanged;
            chkIsPivotBehindAntenna.Checked = isPivotBehindAntenna;
            chkIsPivotBehindAntenna.CheckedChanged += chkIsPivotBehindAntenna_CheckedChanged;

            UpdateIsPivotBehindAntennaCheckbox();

            minFixStepDistance = (decimal)Properties.Settings.Default.setF_minFixStep;
            nudMinFixStepDistance.Value = minFixStepDistance;

            isWorkSwActiveLow = Properties.Settings.Default.setF_IsWorkSwitchActiveLow;

            chkWorkSwActiveLow.CheckedChanged -= chkWorkSwActiveLow_CheckedChanged;
            chkWorkSwActiveLow.Checked = isWorkSwActiveLow;
            chkWorkSwActiveLow.CheckedChanged += chkWorkSwActiveLow_CheckedChanged;

            isWorkSwEn = Properties.Settings.Default.setF_IsWorkSwitchEnabled;

            chkEnableWorkSwitch.CheckedChanged -= chkEnableWorkSwitch_CheckedChanged;
            chkEnableWorkSwitch.Checked = isWorkSwEn;
            chkEnableWorkSwitch.CheckedChanged += chkEnableWorkSwitch_CheckedChanged;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
           //Vehicle settings -------------------------------------------------------------------------------

            if (!isPivotBehindAntenna) antennaPivot *= -1;
            mf.vehicle.antennaPivot = antennaPivot;
            Properties.Vehicle.Default.setVehicle_antennaPivot = mf.vehicle.antennaPivot;

            mf.vehicle.isPivotBehindAntenna = isPivotBehindAntenna;
            Properties.Vehicle.Default.setVehicle_isPivotBehindAntenna = mf.vehicle.isPivotBehindAntenna;

            mf.vehicle.wheelbase = wheelbase;
            Properties.Vehicle.Default.setVehicle_wheelbase = wheelbase;


            mf.vehicle.antennaHeight = antennaHeight;
            Properties.Vehicle.Default.setVehicle_antennaHeight = mf.vehicle.antennaHeight;

            //Sections ------------------------------------------------------------------------------------------

            mf.minFixStepDist = (double)minFixStepDistance;
            Properties.Settings.Default.setF_minFixStep = mf.minFixStepDist;

            mf.mc.isWorkSwitchActiveLow = isWorkSwActiveLow;
            Properties.Settings.Default.setF_IsWorkSwitchActiveLow = isWorkSwActiveLow;

            mf.mc.isWorkSwitchEnabled = isWorkSwEn;
            Properties.Settings.Default.setF_IsWorkSwitchEnabled = isWorkSwEn;

            Properties.Settings.Default.Save();
            Properties.Vehicle.Default.Save();

            //back to FormGPS
            DialogResult = DialogResult.OK;
            Close();
        }

        //don't save anything, leave the settings as before
        private void btnCancel_Click(object sender, EventArgs e)
        { DialogResult = DialogResult.Cancel; Close(); }

        #region Vehicle //----------------------------------------------------------------

        private void nudAntennaHeight_ValueChanged(object sender, EventArgs e)
        {
            antennaHeight = (double)nudAntennaHeight.Value * metImp2m;
        }

        private void nudAntennaPivot_ValueChanged(object sender, EventArgs e)
        {
            antennaPivot = (double)nudAntennaPivot.Value * metImp2m;
        }


        private void nudWheelbase_ValueChanged(object sender, EventArgs e)
        {
            wheelbase = (double)nudWheelbase.Value * metImp2m;
        }

        private void UpdateIsPivotBehindAntennaCheckbox()
        {
            chkIsPivotBehindAntenna.Image = chkIsPivotBehindAntenna.Checked ? Properties.Resources.PivotBehind
                                                                                    : Properties.Resources.PivotAhead;
        }

         private void chkIsPivotBehindAntenna_CheckedChanged(object sender, EventArgs e)
        {
            isPivotBehindAntenna = !isPivotBehindAntenna;
            UpdateIsPivotBehindAntennaCheckbox();
        }

       #endregion Vehicle


        #region Display //----------------------------------------------------------------


        private void nudMinFixStepDistance_ValueChanged(object sender, EventArgs e)
        {
            minFixStepDistance = nudMinFixStepDistance.Value;
        }

        #endregion

        #region WorkSwitch //---------------------------------------------------------

        private void chkWorkSwActiveLow_CheckedChanged(object sender, EventArgs e)
        {
            isWorkSwActiveLow = !isWorkSwActiveLow;
            chkWorkSwActiveLow.Checked = isWorkSwActiveLow;
        }

        private void chkEnableWorkSwitch_CheckedChanged(object sender, EventArgs e)
        {
            isWorkSwEn = !isWorkSwEn;
            chkEnableWorkSwitch.Checked = isWorkSwEn;
        }

        #endregion

    }
}