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

        private double antennaHeight, minSlope, toolWidth;
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
            toolWidth = Properties.Vehicle.Default.setVehicle_toolWidth;
            minSlope = Properties.Vehicle.Default.setVehicle_minSlope * 100;

            nudAntennaHeight.ValueChanged -= nudAntennaHeight_ValueChanged;
            nudAntennaHeight.Value = (decimal)(antennaHeight * m2MetImp);
            nudAntennaHeight.ValueChanged += nudAntennaHeight_ValueChanged;

            nudToolWidth.ValueChanged -= nudToolWidth_ValueChanged;
            nudToolWidth.Value = (decimal)(toolWidth * m2MetImp);
            nudToolWidth.ValueChanged += nudToolWidth_ValueChanged;

            nudMinSlope.ValueChanged -= nudMinSlope_ValueChanged;
            nudMinSlope.Value = (decimal)(minSlope);
            nudMinSlope.ValueChanged += nudMinSlope_ValueChanged;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //Vehicle settings -------------------------------------------------------------------------------
            mf.vehicle.minSlope = minSlope/100;
            Properties.Vehicle.Default.setVehicle_minSlope = mf.vehicle.minSlope;

            mf.vehicle.antennaHeight = antennaHeight;
            Properties.Vehicle.Default.setVehicle_antennaHeight = mf.vehicle.antennaHeight;

            mf.vehicle.toolWidth = toolWidth;
            Properties.Vehicle.Default.setVehicle_toolWidth = toolWidth;

            //Sections ------------------------------------------------------------------------------------------

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

         private void nudMinSlope_ValueChanged(object sender, EventArgs e)
        {
            minSlope = (double)nudMinSlope.Value;
        }

        private void nudToolWidth_ValueChanged(object sender, EventArgs e)
        {
            toolWidth = (double)nudToolWidth.Value * metImp2m;
        }

      #endregion Vehicle
    }
}