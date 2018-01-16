//Please, if you use this, share the improvements

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using SharpGL;
using OpenGrade.Properties;
using Microsoft.Win32;

namespace OpenGrade
{
    public partial class FormGPS
    {
        private void LoadGUI()
        {
            //set the flag mark button to red dot
            btnFlag.Image = Properties.Resources.FlagRed;

            //metric settings
            isMetric = Settings.Default.setMenu_isMetric;
            metricToolStrip.Checked = isMetric;

            if (isMetric)
            {
                lblSpeedUnits.Text = "kmh";
                metricToolStrip.Checked = true;
                imperialToolStrip.Checked = false;
            }
            else
            {
                lblSpeedUnits.Text = "mph";
                metricToolStrip.Checked = false;
                imperialToolStrip.Checked = true;
            }

            //load up colors
            redField = (Settings.Default.setF_FieldColorR);
            grnField = (Settings.Default.setF_FieldColorG);
            bluField = (Settings.Default.setF_FieldColorB);

            redSections = Settings.Default.setF_SectionColorR;
            grnSections = Settings.Default.setF_SectionColorG;
            bluSections = Settings.Default.setF_SectionColorB;

            //set up grid and lightbar
            isGridOn = Settings.Default.setMenu_isGridOn;
            gridToolStripMenuItem.Checked = isGridOn;

            //log NMEA 
            isLogNMEA = Settings.Default.setMenu_isLogNMEA;
            logNMEAMenuItem.Checked = isLogNMEA;

            isLightbarOn = Settings.Default.setMenu_isLightbarOn;
            lightbarToolStripMenuItem.Checked = isLightbarOn;

            isPureDisplayOn = Settings.Default.setMenu_isPureOn;
            pursuitLineToolStripMenuItem.Checked = isPureDisplayOn;

            isSkyOn = Settings.Default.setMenu_isSkyOn;
            skyToolStripMenu.Checked = isSkyOn;

            simulatorOnToolStripMenuItem.Checked = Settings.Default.setMenu_isSimulatorOn;
            if (simulatorOnToolStripMenuItem.Checked)
            {
                panelSimControls.Visible = true;
                timerSim.Enabled = true;
            }
            else
            {
                panelSimControls.Visible = false;
                timerSim.Enabled = false;
            }

            btnDoneDraw.Enabled = false;
            btnDeleteLastPoint.Enabled = false;
            btnStartDraw.Enabled = true;
            lblBarGraphMax.Text = barGraphMax.ToString();
        }

        //hide the left panel
        public void HideTabControl()
        {
            if (openGLControlBack.Visible)
            {
                openGLControl.Height = this.Height - 200;
                openGLControlBack.Visible = false;
            }
            else
            {
                openGLControl.Height = 300;
                openGLControlBack.Visible = true;
            }
        }

        //Open the dialog of tabbed settings
        private void SettingsPageOpen(int page)
        {
            using (var form = new FormSettings(this, page))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK) { }
            }
        }

        // Buttons //-----------------------------------------------------------------------

        //auto steer off and on
        private void btnAutoSteer_Click(object sender, EventArgs e)
        {
            if (isAutoSteerBtnOn)
            {
                isAutoSteerBtnOn = false;
                btnAutoSteer.Image = Properties.Resources.AutoSteerOff;
            }
            else
            {
                if (ABLine.isABLineSet | ct.isContourBtnOn)
                {
                    isAutoSteerBtnOn = true;
                    btnAutoSteer.Image = Properties.Resources.AutoSteerOn;
                }
                else
                {
                    var form = new FormTimedMessage(2000,(gStr.gsNoGuidanceLines),(gStr.gsTurnOnContourOrABLine));
                    form.Show();
                }
            }
        }
        //ABLine
        private void btnABLine_Click(object sender, EventArgs e)
        {
            //if contour is on, turn it off
            if (ct.isContourBtnOn)
            {
                ct.isContourBtnOn = !ct.isContourBtnOn;
                btnContour.Image = Properties.Resources.ContourOff;
            }

            using (var form = new FormABLine(this))
            {
                ABLine.isABLineBeingSet = true;
                txtDistanceOffABLine.Visible = true;
                var result = form.ShowDialog();

                //Comes back

                //if ABLine isn't set, turn off the YouTurn
                if (!ABLine.isABLineSet)
                {
                    ABLine.isABLineBeingSet = false;
                    txtDistanceOffABLine.Visible = false;
                    //change image to reflect on off
                    btnABLine.Image = Properties.Resources.ABLineOff;
                    ABLine.isABLineBeingSet = false;

                    if (isAutoSteerBtnOn) btnAutoSteer.PerformClick();
                }
                //ab line is made
                else
                {
                    //change image to reflect on off
                    btnABLine.Image = Properties.Resources.ABLineOn;
                    ABLine.isABLineBeingSet = false;
                }
            }
        }
        //turn on contour guidance or off
        private void btnContour_Click(object sender, EventArgs e)
        {
            ct.isContourBtnOn = !ct.isContourBtnOn;
            btnContour.Image = ct.isContourBtnOn ? Properties.Resources.ContourOn : Properties.Resources.ContourOff;
        }

        //zoom up close and far away
        private void btnMinMax_Click(object sender, EventArgs e)
        {
            //keep a copy to go back to previous zoom
            if (zoomValue < 56)
            {
                previousZoom = zoomValue;
                zoomValue = 60;
            }
            else
            {
                zoomValue = previousZoom;
            }
            camera.camSetDistance = zoomValue * zoomValue * -1;
            SetZoom();
        }

        //button for Manual On Off of the sections
        private void btnManualOffOn_Click(object sender, EventArgs e)
        {
            switch (manualBtnState)
            {
                case btnStates.Off:
                    manualBtnState = btnStates.Rec;
                    btnManualOffOn.Image = Properties.Resources.ManualOn;
                    userDistance = 0;
                    lblCut.Text = "*";
                    lblFill.Text = "*";
                    lblCutFillRatio.Text = "*";
                    lblDrawSlope.Text = "*";

                    cboxLastPass.Checked = false;
                    cboxRecLastOnOff.Checked = false;
                    btnDoneDraw.Enabled = false;
                    btnDeleteLastPoint.Enabled = false;
                    btnStartDraw.Enabled = false;


                    break;

                case btnStates.Rec:
                    manualBtnState = btnStates.Off;
                    btnManualOffOn.Image = Properties.Resources.ManualOff;
                    CalculateContourPointDistances();
                    FileSaveContour();
                    btnDoneDraw.Enabled = false;
                    btnDeleteLastPoint.Enabled = false;
                    btnStartDraw.Enabled = true;

                    break;
            }
        }

        //The main flag marker button 
        private void btnFlag_Click(object sender, EventArgs e)
        {
            int nextflag = flagPts.Count + 1;
            CFlag flagPt = new CFlag(pn.latitude, pn.longitude, pn.easting, pn.northing, flagColor, nextflag);
            flagPts.Add(flagPt);
            FileSaveFlags();
        }

        //The zoom buttons in out
        private void btnZoomIn_MouseDown(object sender, MouseEventArgs e)
        {
            if (zoomValue <= 20) zoomValue += zoomValue * 0.1;
            else zoomValue += zoomValue * 0.05;
            camera.camSetDistance = zoomValue * zoomValue * -1;
            SetZoom();
        }
        private void btnZoomOut_MouseDown(object sender, MouseEventArgs e)
        {
            if (zoomValue <= 20)
            { if ((zoomValue -= zoomValue * 0.1) < 6.0) zoomValue = 6.0; }
            else { if ((zoomValue -= zoomValue * 0.05) < 6.0) zoomValue = 6.0; }

            camera.camSetDistance = zoomValue * zoomValue * -1;
            SetZoom();
        }

        //view tilt up down and saving in settings
        private void btnTiltUp_MouseDown(object sender, MouseEventArgs e)
        {
            camera.camPitch -= ((camera.camPitch * 0.03) - 1);
            if (camera.camPitch > 0) camera.camPitch = 0;
        }
        private void btnTiltDown_MouseDown(object sender, MouseEventArgs e)
        {
            camera.camPitch += ((camera.camPitch * 0.03) - 1);
            if (camera.camPitch < -80) camera.camPitch = -80;
        }

        private void btnSnap_Click(object sender, EventArgs e)
        {
           ABLine.SnapABLine();
        }

        //panel buttons
        private void btnSettings_Click_1(object sender, EventArgs e)
        {
            SettingsPageOpen(0);
        }
        private void btnComm_Click(object sender, EventArgs e)
        {
            SettingsCommunications();
        }
        private void btnUDPSettings_Click(object sender, EventArgs e)
        {
            SettingsUDP();
        }
        private void btnUnits_Click(object sender, EventArgs e)
        {
            isMetric = !isMetric;
            Settings.Default.setMenu_isMetric = isMetric;
            Settings.Default.Save();
            if (isMetric)
            {
                lblSpeedUnits.Text = "kmh";
                metricToolStrip.Checked = true;
                imperialToolStrip.Checked = false;
            }
            else
            {
                lblSpeedUnits.Text = "mph";
                metricToolStrip.Checked = false;
                imperialToolStrip.Checked = true;
            }
        }
        private void btnGPSData_Click(object sender, EventArgs e)
        {
            Form form = new FormGPSData(this);
            form.Show();
        }
        private void btnAutoSteerConfig_Click(object sender, EventArgs e)
        {
            //check if window already exists
            Form fc = Application.OpenForms["FormSteer"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            //
            Form form = new FormSteer(this);
            form.Show();
        }
        private void btnFileExplorer_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                FileSaveFlagsKML();
            }
            Process.Start(fieldsDirectory + currentFieldDirectory);
        }

        private void btnClearLastPass_Click(object sender, EventArgs e)
        {
            int cnnt = ct.ptList.Count;
            if (cnnt > 0)
            {
                for (int i = 0; i < cnnt; i++) ct.ptList[i].lastPassAltitude = -1;
            }
        }
        private void btnZeroAltitude_Click(object sender, EventArgs e)
        {
            ct.zeroAltitude = pn.altitude;
        }
        private void btnStartDraw_Click(object sender, EventArgs e)
        {
            if (ct.ptList.Count > 50)
            {
                cboxLastPass.Checked = false;

                ct.drawList.Clear();
                ct.isDrawingRefLine = true;
                lblCut.Text = "-";
                lblFill.Text = "-";
                lblCutFillRatio.Text = "-";

                btnDoneDraw.Enabled = true;
                btnDeleteLastPoint.Enabled = true;
                btnStartDraw.Enabled = false;
            }
            else TimedMessageBox(1500, "No Surveyed Points", "Survey a Contour First");
        }
        private void btnDoneDraw_Click(object sender, EventArgs e)
        {
            btnDoneDraw.Enabled = false;
            btnDeleteLastPoint.Enabled = false;
            btnStartDraw.Enabled = true;

            ct.isDrawingRefLine = false;
            int cnt = ct.ptList.Count;
            int drawPts = ct.drawList.Count-1;
            double slope = 0.5;

            if (drawPts > 0)
            {
                for (int i = 0; i < cnt; i++)
                {
                    //points before the drawn line are -1
                    if (i < ct.drawList[0].easting)
                    {
                        ct.ptList[i].cutAltitude = -1;
                        continue;
                    }

                    //points after drawn line are -1
                    if (i > ct.drawList[drawPts].easting)
                    {
                        ct.ptList[i].cutAltitude = -1;
                        continue;
                    }

                    //find out where its between
                    for (int j = 0; j < drawPts; j++)
                    {
                        if (i >= ct.drawList[j].easting && i <= ct.drawList[j + 1].easting)
                        {
                            slope = (ct.drawList[j + 1].northing - ct.drawList[j].northing) / (ct.drawList[j + 1].easting - ct.drawList[j].easting);
                            ct.ptList[i].cutAltitude = ((i - ct.drawList[j].easting) * slope) + ct.drawList[j].northing;
                            break;
                        }
                    }
                }

                //Fill in cut and fill
                CalculateTotalCutFillLabels();
            }
        }
        private void CalculateTotalCutFillLabels()
        {
            lblDrawSlope.Text = "-";

            double cut = 0; double fill = 0; double delta;
            int cnt = ct.ptList.Count;

            if (cnt > 0)
            {

                for (int i = 0; i < cnt; i++)
                {
                    if (ct.ptList[i].cutAltitude == -1) continue;

                    delta = ct.ptList[i].cutAltitude - ct.ptList[i].altitude;
                    delta *= (ct.ptList[i].distance * vehicle.toolWidth);

                    if (delta > 0)
                    {
                        fill += delta;
                        delta = 0;
                    }
                    else
                    {
                        delta *= -1;
                        cut += delta;
                        delta = 0;
                    }
                }

                lblCut.Text = cut.ToString("N2");
                lblFill.Text = fill.ToString("N2");

                delta = (cut - fill);
                lblCutFillRatio.Text = delta.ToString("N2");
            }
            else
            {
                lblCut.Text = "-";
                lblFill.Text = "-";
                lblCutFillRatio.Text = "-";
            }
        }
        private void btnDeleteLastPoint_Click(object sender, EventArgs e)
        {
            int ptCnt = ct.drawList.Count;
            if (ptCnt > 0) ct.drawList.RemoveAt(ptCnt - 1);
        }

        //progress bar "buttons" for gain
        private void pbarCutAbove_Click(object sender, EventArgs e)
        {
            barGraphMax++;
            lblBarGraphMax.Text = barGraphMax.ToString();

        }

        private void pbarCutBelow_Click(object sender, EventArgs e)
        {
            if (barGraphMax-- < 1) barGraphMax = 1;
            lblBarGraphMax.Text = barGraphMax.ToString();
        }

        // Menu Items ------------------------------------------------------------------

        //File drop down items
        private void loadVehicleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                var form = new FormTimedMessage(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                form.Show();
                return;
            }
            FileOpenVehicle();
        }
        private void saveVehicleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileSaveVehicle();
        }
        private void fieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JobNewOpenResume();
        }
        private void setWorkingDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                var form = new FormTimedMessage(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                form.Show();
                return;
            }

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            fbd.Description = "Currently: " + Settings.Default.setF_workingDirectory;

            if (Settings.Default.setF_workingDirectory == "Default") fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            else fbd.SelectedPath = Settings.Default.setF_workingDirectory;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\OpenGrade",true);

                if (fbd.SelectedPath != Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
                {
                    //save the user set directory in Registry
                    regKey.SetValue("Directory", fbd.SelectedPath);
                    regKey.Close();
                    Settings.Default.setF_workingDirectory = fbd.SelectedPath;
                    Settings.Default.Save();
                }
                else
                {
                    regKey.SetValue("Directory", "Default");
                    regKey.Close();
                    Settings.Default.setF_workingDirectory = "Default";
                    Settings.Default.Save();
                }

                //restart program
                MessageBox.Show(gStr.gsProgramExitAndRestart);
                Close();
            }
        }

        //Help menu drop down items
        private void aboutToolStripMenuHelpAbout_Click(object sender, EventArgs e)
        {
            using (var form = new Form_About())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK) { }
            }
        }
        private void helpToolStripMenuHelpHelp_Click(object sender, EventArgs e)
        {
            //string appPath = Assembly.GetEntryAssembly().Location;
            //string filename = Path.Combine(Path.GetDirectoryName(appPath), "help.htm");
            //Process.Start("http://OpenGrade.gh-ortner.com/doku.php");
        }

        //Options Drop down menu items
        private void resetALLToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                MessageBox.Show(gStr.gsCloseFieldFirst);
            }
            else
            {
                DialogResult result2 = MessageBox.Show("Really Reset Everything?", "Reset settings",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result2 == DialogResult.Yes)
                {
                    Settings.Default.Reset();
                    Settings.Default.Save();
                    Vehicle.Default.Reset();
                    Vehicle.Default.Save();
                    MessageBox.Show(gStr.gsProgramExitAndRestart);
                    Application.Exit();
                }
            }
        }
        private void logNMEAMenuItem_Click(object sender, EventArgs e)
        {
            isLogNMEA = !isLogNMEA;
            logNMEAMenuItem.Checked = isLogNMEA;
            Settings.Default.setMenu_isLogNMEA = isLogNMEA;
            Settings.Default.Save();
        }
        private void lightbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isLightbarOn = !isLightbarOn;
            lightbarToolStripMenuItem.Checked = isLightbarOn;
            Settings.Default.setMenu_isLightbarOn = isLightbarOn;
            Settings.Default.Save();
        }
        private void polygonsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isDrawPolygons = !isDrawPolygons;
            polygonsToolStripMenuItem.Checked = !polygonsToolStripMenuItem.Checked;
        }
        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isGridOn = !isGridOn;
            gridToolStripMenuItem.Checked = isGridOn;
            Settings.Default.setMenu_isGridOn = isGridOn;
            Settings.Default.Save();
        }
        private void pursuitLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isPureDisplayOn = !isPureDisplayOn;
            pursuitLineToolStripMenuItem.Checked = isPureDisplayOn;
            Settings.Default.setMenu_isPureOn = isPureDisplayOn;
            Settings.Default.Save();
        }
        private void metricToolStrip_Click(object sender, EventArgs e)
        {
            metricToolStrip.Checked = true;
            imperialToolStrip.Checked = false;
            isMetric = true;
            Settings.Default.setMenu_isMetric = isMetric;
            Settings.Default.Save();
            lblSpeedUnits.Text = "kmh";
        }
        private void skyToolStripMenu_Click(object sender, EventArgs e)
        {
            isSkyOn = !isSkyOn;
            skyToolStripMenu.Checked = isSkyOn;
            Settings.Default.setMenu_isSkyOn = isSkyOn;
            Settings.Default.Save();
        }
        private void imperialToolStrip_Click(object sender, EventArgs e)
        {
            metricToolStrip.Checked = false;
            imperialToolStrip.Checked = true;
            isMetric = false;
            Settings.Default.setMenu_isMetric = isMetric;
            Settings.Default.Save();
            lblSpeedUnits.Text = "mph";

        }
        private void simulatorOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (simulatorOnToolStripMenuItem.Checked)
            {
                panelSimControls.Visible = true;
                nudElevation.Visible = true;
                timerSim.Enabled = true;
            }
            else
            {
                panelSimControls.Visible = false;
                nudElevation.Visible = false;
                timerSim.Enabled = false;
            }

            Settings.Default.setMenu_isSimulatorOn = simulatorOnToolStripMenuItem.Checked;
            Settings.Default.Save();
        }

        //setting color off Options Menu
        private void sectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //color picker for sections
            ColorDialog colorDlg = new ColorDialog
            {
                FullOpen = true,
                AnyColor = true,
                SolidColorOnly = false,
                Color = Color.FromArgb(255, redSections, grnSections, bluSections)
            };

            if (colorDlg.ShowDialog() != DialogResult.OK) return;

            redSections = colorDlg.Color.R;
            if (redSections > 253) redSections = 253;
            grnSections = colorDlg.Color.G;
            if (grnSections > 253) grnSections = 253;
            bluSections = colorDlg.Color.B;
            if (bluSections > 253) bluSections = 253;

            Settings.Default.setF_SectionColorR = redSections;
            Settings.Default.setF_SectionColorG = grnSections;
            Settings.Default.setF_SectionColorB = bluSections;
            Settings.Default.Save();
        }
        private void fieldToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //color picker for fields

            ColorDialog colorDlg = new ColorDialog
            {
                FullOpen = true,
                AnyColor = true,
                SolidColorOnly = false,
                Color = Color.FromArgb(255, Settings.Default.setF_FieldColorR,
                Settings.Default.setF_FieldColorG, Settings.Default.setF_FieldColorB)
            };

            if (colorDlg.ShowDialog() != DialogResult.OK) return;

            redField = colorDlg.Color.R;
            if (redField > 253) redField = 253;
            grnField = colorDlg.Color.G;
            if (grnField > 253) grnField = 253;
            bluField = colorDlg.Color.B;
            if (bluField > 253) bluField = 253;

            Settings.Default.setF_FieldColorR = redField;
            Settings.Default.setF_FieldColorG = grnField;
            Settings.Default.setF_FieldColorB = bluField;
            Settings.Default.Save();
        }

        //Tools drop down items
        private void explorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                FileSaveFlagsKML();
            }
            Process.Start(fieldsDirectory + currentFieldDirectory);
        }
        private void webCamToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Form form = new FormWebCam();
            form.Show();
        }
        private void googleEarthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                //save new copy of flags
                FileSaveFlagsKML();

                //Process.Start(@"C:\Program Files (x86)\Google\Google Earth\client\googleearth", workingDirectory + currentFieldDirectory + "\\Flags.KML");
                Process.Start(fieldsDirectory + currentFieldDirectory + "\\Flags.KML");
            }
            else
            {
                var form = new FormTimedMessage(1500, gStr.gsFieldNotOpen, gStr.gsStartNewField);
                form.Show();
            }
        }

        private void btnGoogleEarth_Click(object sender, EventArgs e)
        {
            //save new copy of contour
            FileSaveCutKML();

            //make sure google is installed
            Process.Start(fieldsDirectory + currentFieldDirectory + "\\Cut.KML");
        }

        private void fieldViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //in the current application directory
            //string AOGViewer = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\AOG.exe";
            //Process.Start(AOGViewer);
            {
                var form = new FormTimedMessage(2000, "Not yet Implemented", "But soon....");
                form.Show();
            }
        }
        private void gPSDataToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form form = new FormGPSData(this);
            form.Show();
        }

        //The flag context menus
        private void toolStripMenuItemFlagRed_Click(object sender, EventArgs e)
        {
            flagColor = 0;
            btnFlag.Image = Properties.Resources.FlagRed;
        }
        private void toolStripMenuGrn_Click(object sender, EventArgs e)
        {
            flagColor = 1;
            btnFlag.Image = Properties.Resources.FlagGrn;
        }
        private void toolStripMenuYel_Click(object sender, EventArgs e)
        {
            flagColor = 2;
            btnFlag.Image = Properties.Resources.FlagYel;
        }
        private void toolStripMenuFlagDelete_Click(object sender, EventArgs e)
        {
            //delete selected flag and set selected to none
            DeleteSelectedFlag();
            FileSaveFlags();
        }
        private void toolStripMenuFlagDeleteAll_Click(object sender, EventArgs e)
        {
            flagNumberPicked = 0;
            flagPts.Clear();
            FileSaveFlags();
        }
        private void contextMenuStripFlag_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            toolStripMenuFlagDelete.Enabled = flagNumberPicked != 0;

            toolStripMenuFlagDeleteAll.Enabled = flagPts.Count > 0;
        }

        //OpenGL Window context Menu and functions
        private void deleteFlagToolOpenGLContextMenu_Click(object sender, EventArgs e)
        {
            //delete selected flag and set selected to none
            DeleteSelectedFlag();
        }
        private void contextMenuStripOpenGL_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //dont bring up menu if no flag selected
            if (flagNumberPicked == 0) e.Cancel = true;
        }
        private void googleEarthOpenGLContextMenu_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                //save new copy of kml with selected flag and view in GoogleEarth
                FileSaveSingleFlagKML(flagNumberPicked);

                //Process.Start(@"C:\Program Files (x86)\Google\Google Earth\client\googleearth", workingDirectory + currentFieldDirectory + "\\Flags.KML");
                Process.Start(fieldsDirectory + currentFieldDirectory + "\\Flag.KML");
            }
        }

        //function mouse down in window for picking
        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //0 at bottom for opengl, 0 at top for windows, so invert Y value
                Point point = openGLControl.PointToClient(Cursor.Position);
                mouseX = point.X;
                mouseY = openGLControl.Height - point.Y;
                leftMouseDownOnOpenGL = true;
            }
        }

        //taskbar buttons
        private void toolstripAutoSteerConfig_Click(object sender, EventArgs e)
        {
            //check if window already exists
            Form fc = Application.OpenForms["FormSteer"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            //
            Form form = new FormSteer(this);
            form.Show();
        }
        private void toolstripVehicleConfig_Click(object sender, EventArgs e)
        {
            SettingsPageOpen(0);
            CalculateTotalCutFillLabels();
        }
        private void toolstripUSBPortsConfig_Click(object sender, EventArgs e)
        {
            SettingsCommunications();
        }
        private void toolstripUDPConfig_Click(object sender, EventArgs e)
        {
            SettingsUDP();
        }
        private void toolstripResetTrip_Click_1(object sender, EventArgs e)
        {
            userDistance = 0;
        }
        private void toolstripField_Click(object sender, EventArgs e)
        {
            JobNewOpenResume();
        }

        //batman to maximize GPS mapping - hide tab control
        private void btnHideTabs_Click(object sender, EventArgs e)
        {
            HideTabControl();
        }        
        
        //Sim controls
        private void timerSim_Tick(object sender, EventArgs e)
        {
            //if a GPS is connected disable sim
            if (!sp.IsOpen)
            {
                if (isAutoSteerBtnOn) sim.DoSimTick(guidanceLineSteerAngle / 10.0);
                else sim.DoSimTick(sim.steerAngleScrollBar);
            }
        }
        private void tbarStepDistance_Scroll(object sender, EventArgs e)
        {
            sim.stepDistance = ((double)(tbarStepDistance.Value)) / 10.0 / (double)fixUpdateHz;
        }
        private void tbarSteerAngle_Scroll(object sender, EventArgs e)
        {
            sim.steerAngleScrollBar = (tbarSteerAngle.Value) * 0.1;
            lblSteerAngle.Text = sim.steerAngleScrollBar.ToString("N1");
        }
        private void btnResetSteerAngle_Click(object sender, EventArgs e)
        {
            sim.steerAngleScrollBar = 0;
            tbarSteerAngle.Value = 0;
            lblSteerAngle.Text = sim.steerAngleScrollBar.ToString("N1");
        }
        private void btnResetSim_Click(object sender, EventArgs e)
        {
            sim.ResetSim();
        }

        //simulator altitude
        private void nudElevation_ValueChanged(object sender, EventArgs e)
        {
            sim.altitude = (double)nudElevation.Value + Properties.Vehicle.Default.setVehicle_antennaHeight;
        }

        #region Properties // ---------------------------------------------------------------------

        public string Zone { get { return Convert.ToString(pn.zone); } }
        public string FixNorthing { get { return Convert.ToString(Math.Round(pn.northing + pn.utmNorth, 2)); } }
        public string FixEasting { get { return Convert.ToString(Math.Round(pn.easting + pn.utmEast, 2)); } }
        public string Latitude { get { return Convert.ToString(Math.Round(pn.latitude, 7)); } }
        public string Longitude { get { return Convert.ToString(Math.Round(pn.longitude, 7)); } }

        public string SatsTracked { get { return Convert.ToString(pn.satellitesTracked); } }
        public string HDOP { get { return Convert.ToString(pn.hdop); } }
        public string NMEAHz { get { return Convert.ToString(fixUpdateHz); } }
        public string PassNumber { get { return Convert.ToString(ABLine.passNumber); } }
        public string Heading { get { return Convert.ToString(Math.Round(glm.toDegrees(fixHeading), 1)) + "\u00B0"; } }
        public string GPSHeading { get { return (Math.Round(glm.toDegrees(gpsHeading), 1)) + "\u00B0"; } }
        public string Status { get { if (pn.status == "A") return "Active"; else return "Void"; } }
        public string FixQuality
        {
            get
            {
                if (pn.fixQuality == 0) return "Invalid";
                else if (pn.fixQuality == 1) return "GPS fix";
                else if (pn.fixQuality == 2) return "DGPS fix";
                else if (pn.fixQuality == 3) return "PPS fix";
                else if (pn.fixQuality == 4) return "RTK fix";
                else if (pn.fixQuality == 5) return "Flt RTK";
                else if (pn.fixQuality == 6) return "Estimate";
                else if (pn.fixQuality == 7) return "Man IP";
                else if (pn.fixQuality == 8) return "Sim";
                else return "Unknown";
            }
        }

        public string GyroInDegrees
        {
            get
            {
                if (mc.gyroHeading != 9999)
                    return Math.Round(mc.gyroHeading * 0.0625, 1) + "\u00B0";
                else return "-";
            }
        }
        public string RollInDegrees
        {
            get
            {
                if (mc.rollRaw != 9999)
                    return Math.Round(mc.rollRaw * 0.0625, 1) + "\u00B0";
                else return "-";
            }
        }
        public string PureSteerAngle { get { return ((double)(guidanceLineSteerAngle) * 0.1) + "\u00B0"; } }

        public string FixHeading { get { return Math.Round(fixHeading, 4).ToString(); } }

        public string StepFixNum { get { return (currentStepFix).ToString(); } }
        public string CurrentStepDistance { get { return Math.Round(distanceCurrentStepFix, 3).ToString(); } }
        public string TotalStepDistance { get { return Math.Round(fixStepDist, 3).ToString(); } }

        public string WorkSwitchValue { get { return mc.workSwitchValue.ToString(); } }
        public string AgeDiff { get { return pn.ageDiff.ToString(); } }

        //Metric and Imperial Properties
        public string SpeedMPH
        {
            get
            {
                double spd = 0;
                for (int c = 0; c < 10; c++) spd += avgSpeed[c];
                spd *= 0.0621371;
                return Convert.ToString(Math.Round(spd, 1));
            }
        }
        public string SpeedKPH
        {
            get
            {
                double spd = 0;
                for (int c = 0; c < 10; c++) spd += avgSpeed[c];
                spd *= 0.1;
                return Convert.ToString(Math.Round(spd, 1));
            }
        }

        public string Altitude { get { return Convert.ToString(Math.Round(pn.altitude,3)); } }
        public string AltitudeFeet { get { return Convert.ToString((Math.Round((pn.altitude * 3.28084),3))); } }

        public Texture ParticleTexture { get; set; }

        #endregion properties 

        //Timer triggers at 20 msec, 50 hz, and is THE clock of the whole program//
        private void tmrWatchdog_tick(object sender, EventArgs e)
        {
            //go see if data ready for draw and position updates
            //tmrWatchdog.Enabled = false;
            ScanForNMEA();
            //tmrWatchdog.Enabled = true;
            statusUpdateCounter++;

            if (fiveSecondCounter++ > 100) { fiveSecondCounter = 0; }


            //every half of a second update all status
            if (statusUpdateCounter > 4)
            {
                //reset the counter
                statusUpdateCounter = 0;

                //counter used for saving field in background
                saveCounter++;

                if (tabControl1.SelectedIndex == 0 && tabControl1.Visible)
                {

                    //both
                    lblLatitude.Text = Latitude;
                    lblLongitude.Text = Longitude;
                    lblFixQuality.Text = FixQuality;
                    lblSats.Text = SatsTracked;

                    lblRoll.Text = RollInDegrees;
                    lblGyroHeading.Text = GyroInDegrees;
                    lblGPSHeading.Text = GPSHeading;

                    //up in the menu a few pieces of info
                    if (isJobStarted)
                    {
                        lblEasting.Text = "E: " + Math.Round(pn.easting, 1).ToString();
                        lblNorthing.Text = "N: " + Math.Round(pn.northing, 1).ToString();
                    }
                    else
                    {
                        lblEasting.Text = "E: " + ((int)pn.actualEasting).ToString();
                        lblNorthing.Text = "N: " + ((int)pn.actualNorthing).ToString();
                    }

                    lblZone.Text = pn.zone.ToString();
                    tboxSentence.Text = recvSentenceSettings;
                }

                //the main formgps window
                if (isMetric)  //metric or imperial
                {
                    //Hectares on the master section soft control and sections
                    lblSpeed.Text = SpeedKPH;

                    //status strip values
                    stripDistance.Text = Convert.ToString((UInt16)(userDistance)) + " m";
                    lblAltitude.Text = Altitude;
                    btnZeroAltitude.Text = (pn.altitude - ct.zeroAltitude).ToString("N2");
                }
                else  //Imperial Measurements
                {
                    //acres on the master section soft control and sections
                    lblSpeed.Text = SpeedMPH;

                    //status strip values
                    stripDistance.Text = Convert.ToString((UInt16)(userDistance * 3.28084)) + " ft";
                    lblAltitude.Text = AltitudeFeet;
                    btnZeroAltitude.Text = ((pn.altitude - ct.zeroAltitude)*glm.m2ft).ToString("N2");
                }

                //not Metric/Standard units sensitive
                lblHeading.Text = Heading;
                btnABLine.Text = PassNumber;
                lblPureSteerAngle.Text = PureSteerAngle;

                if (cutDelta == 9999)
                {
                    lblCutDelta.Text = "--";
                    lblCutDelta.BackColor = Color.Lavender;
                    pbarCutAbove.Value = 0;
                    pbarCutBelow.Value = 0;
                }
                else
                {
                    lblCutDelta.Text = cutDelta.ToString("N1");
                    lblCutDelta.BackColor = SystemColors.ControlText;

                    if (cutDelta < 0)
                    {
                        int val = (int)(cutDelta / barGraphMax * -100);
                        pbarCutAbove.Value = 0;
                        pbarCutBelow.Value = val;
                        lblCutDelta.BackColor = Color.Tomato;
                    }
                    else
                    {
                        int val = (int)(cutDelta / barGraphMax * 100);
                        pbarCutAbove.Value = val;
                        pbarCutBelow.Value = 0;
                        lblCutDelta.BackColor = Color.Lime;

                    }
                }

                //update the online indicator
                if (recvCounter > 50)
                {
                    stripOnlineGPS.Value = 1;
                    lblEasting.Text = "-";
                    lblNorthing.Text = gStr.gsNoGPS;
                    lblZone.Text = "-";
                    tboxSentence.Text = gStr.gsNoSentenceData;
                }
                else stripOnlineGPS.Value = 100;
            }
            //wait till timer fires again.     
        }


    }//end class
}//end namespace