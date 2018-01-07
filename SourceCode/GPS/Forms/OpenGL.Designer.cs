
using System;
using System.Drawing;
using System.Windows.Forms;
using SharpGL;

namespace OpenGrade
{
    public partial class FormGPS
    {
        //extracted Near, Far, Right, Left clipping planes of frustum
        public double[] frustum = new double[24];

        //difference between blade tip and guide line
        public double cutDelta;
        private double minDist;

        double fovy = 45;
        double camDistanceFactor = -2;
        int mouseX = 0, mouseY = 0;

        //data buffer for pixels read from off screen buffer
        byte[] grnPixels = new byte[80001];

        /// Handles the OpenGLDraw event of the openGLControl control.
        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs e)
        {
            if (isGPSPositionInitialized)
            {

                //  Get the OpenGL object.
                OpenGL gl = openGLControl.OpenGL;
                //System.Threading.Thread.Sleep(500);

                //  Clear the color and depth buffer.
                gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
                gl.LoadIdentity();

                //camera does translations and rotations
                camera.SetWorldCam(gl, pn.easting, pn.northing, camHeading);

                //draw the field ground images
                worldGrid.DrawFieldSurface();
                
                ////Draw the world grid based on camera position
                gl.Disable(OpenGL.GL_DEPTH_TEST);
                gl.Disable(OpenGL.GL_TEXTURE_2D);


                gl.Enable(OpenGL.GL_LINE_SMOOTH);
                gl.Enable(OpenGL.GL_BLEND);

                gl.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_FASTEST);
                gl.Hint(OpenGL.GL_POINT_SMOOTH_HINT, OpenGL.GL_FASTEST);
                gl.Hint(OpenGL.GL_POLYGON_SMOOTH_HINT, OpenGL.GL_FASTEST);

                ////if grid is on draw it
                if (isGridOn) worldGrid.DrawWorldGrid(gridZoom);

                //turn on blend for paths
                gl.Enable(OpenGL.GL_BLEND);

                //section patch color
                gl.Color(redSections, grnSections, bluSections, (byte)160);
                if (isDrawPolygons) gl.PolygonMode(OpenGL.GL_FRONT, OpenGL.GL_LINE);

                gl.PolygonMode(OpenGL.GL_FRONT, OpenGL.GL_FILL);
                gl.Color(1, 1, 1);

                //draw contour line if button on 
                //if (ct.isContourBtnOn)
                

                // draw the current and reference AB Lines
                if (ABLine.isABLineSet | ABLine.isABLineBeingSet) ABLine.DrawABLines();
                else ct.DrawContourLine();

                //draw the flags if there are some
                int flagCnt = flagPts.Count;
                if (flagCnt > 0)
                {
                    for (int f = 0; f < flagCnt; f++)
                    {
                        gl.PointSize(8.0f);
                        gl.Begin(OpenGL.GL_POINTS);
                        if (flagPts[f].color == 0) gl.Color((byte)255, (byte)0, (byte)flagPts[f].ID);
                        if (flagPts[f].color == 1) gl.Color((byte)0, (byte)255, (byte)flagPts[f].ID);
                        if (flagPts[f].color == 2) gl.Color((byte)255, (byte)255, (byte)flagPts[f].ID);
                        gl.Vertex(flagPts[f].easting, flagPts[f].northing, 0);
                        gl.End();
                    }

                    if (flagNumberPicked != 0)
                    {
                        ////draw the box around flag
                        gl.LineWidth(4);
                        gl.Color(0.980f, 0.0f, 0.980f);
                        gl.Begin(OpenGL.GL_LINE_STRIP);

                        double offSet = (zoomValue * zoomValue * 0.01);
                        gl.Vertex(flagPts[flagNumberPicked - 1].easting, flagPts[flagNumberPicked - 1].northing + offSet, 0);
                        gl.Vertex(flagPts[flagNumberPicked - 1].easting - offSet, flagPts[flagNumberPicked - 1].northing, 0);
                        gl.Vertex(flagPts[flagNumberPicked - 1].easting, flagPts[flagNumberPicked - 1].northing - offSet, 0);
                        gl.Vertex(flagPts[flagNumberPicked - 1].easting + offSet, flagPts[flagNumberPicked - 1].northing, 0);
                        gl.Vertex(flagPts[flagNumberPicked - 1].easting, flagPts[flagNumberPicked - 1].northing + offSet, 0);

                        gl.End();

                        //draw the flag with a black dot inside
                        gl.PointSize(4.0f);
                        gl.Color(0, 0, 0);
                        gl.Begin(OpenGL.GL_POINTS);
                        gl.Vertex(flagPts[flagNumberPicked - 1].easting, flagPts[flagNumberPicked - 1].northing, 0);
                        gl.End();
                    }
                }

                //screen text for debug
                //gl.DrawText(120, 10, 1, 1, 1, "Courier Bold", 18, "Head: " + saveCounter.ToString("N1"));
                //gl.DrawText(120, 40, 1, 1, 1, "Courier Bold", 18, "Tool: " + distTool.ToString("N1"));
                //gl.DrawText(120, 70, 1, 1, 1, "Courier Bold", 18, "Where: " + yt.whereAmI.ToString());
                //gl.DrawText(120, 100, 1, 1, 1, "Courier Bold", 18, "Seq: " + yt.isSequenceTriggered.ToString());
                //gl.DrawText(120, 40, 1, 1, 1, "Courier Bold", 18, "  GPS: " + Convert.ToString(Math.Round(glm.toDegrees(gpsHeading), 2)));
                //gl.DrawText(120, 70, 1, 1, 1, "Courier Bold", 18, "Fixed: " + Convert.ToString(Math.Round(glm.toDegrees(gyroCorrected), 2)));
                //gl.DrawText(120, 100, 1, 1, 1, "Courier Bold", 18, "L/Min: " + Convert.ToString(rc.CalculateRateLitersPerMinute()));
                //gl.DrawText(120, 130, 1, 1, 1, "Courier", 18, "       Roll: " + Convert.ToString(glm.toDegrees(rollDistance)));
                //gl.DrawText(120, 160, 1, 1, 1, "Courier", 18, "       Turn: " + Convert.ToString(Math.Round(turnDelta, 4)));
                //gl.DrawText(40, 120, 1, 0.5, 1, "Courier", 12, " frame msec " + Convert.ToString((int)(frameTime)));

                //draw the vehicle/implement
                vehicle.DrawVehicle();

                //Back to normal
                gl.Color(0.98f, 0.98f, 0.98f);
                gl.Disable(OpenGL.GL_BLEND);
                gl.Enable(OpenGL.GL_DEPTH_TEST);

                //// 2D Ortho --------------------------
                gl.MatrixMode(OpenGL.GL_PROJECTION);
                gl.PushMatrix();
                gl.LoadIdentity();

                //negative and positive on width, 0 at top to bottom ortho view
                gl.Ortho2D(-(double)Width / 2, (double)Width / 2, (double)Height, 0);

                //  Create the appropriate modelview matrix.
                gl.MatrixMode(OpenGL.GL_MODELVIEW);
                gl.PushMatrix();
                gl.LoadIdentity();

                if (isSkyOn)
                {
                    ////draw the background when in 3D
                    if (camera.camPitch < -60)
                    {
                        //-10 to -32 (top) is camera pitch range. Set skybox to line up with horizon 
                        double hite = (camera.camPitch + 60) / -20 * 0.34;
                        //hite = 0.001;

                        //the background
                        double winLeftPos = -(double)Width / 2;
                        double winRightPos = -winLeftPos;
                        gl.Enable(OpenGL.GL_TEXTURE_2D);
                        gl.BindTexture(OpenGL.GL_TEXTURE_2D, texture[0]);		// Select Our Texture

                        gl.Begin(OpenGL.GL_TRIANGLE_STRIP);				// Build Quad From A Triangle Strip
                        gl.TexCoord(0, 0); gl.Vertex(winRightPos, 0.0); // Top Right
                        gl.TexCoord(1, 0); gl.Vertex(winLeftPos, 0.0); // Top Left
                        gl.TexCoord(0, 1); gl.Vertex(winRightPos, hite * (double)Height); // Bottom Right
                        gl.TexCoord(1, 1); gl.Vertex(winLeftPos, hite * (double)Height); // Bottom Left
                        gl.End();						// Done Building Triangle Strip

                        //disable, straight color
                        gl.Disable(OpenGL.GL_TEXTURE_2D);
                    }
                }

                //LightBar if AB Line is set and turned on or contour
                if (isLightbarOn)
                {
                    if (ct.isContourBtnOn)
                    {
                        string dist;
                        txtDistanceOffABLine.Visible = true;
                        //lblDelta.Visible = true;
                        if (ct.distanceFromCurrentLine == 32000) ct.distanceFromCurrentLine = 0;

                        DrawLightBar(openGLControl.Width, openGLControl.Height, ct.distanceFromCurrentLine * 0.1);
                        if ((ct.distanceFromCurrentLine) < 0.0)
                        {
                            txtDistanceOffABLine.ForeColor = Color.Green;
                            if (isMetric) dist = ((int)Math.Abs(ct.distanceFromCurrentLine * 0.1)) + " ->";
                            else dist = ((int)Math.Abs(ct.distanceFromCurrentLine / 2.54 * 0.1)) + " ->";
                            txtDistanceOffABLine.Text = dist;
                        }

                        else
                        {
                            txtDistanceOffABLine.ForeColor = Color.Red;
                            if (isMetric) dist = "<- " + ((int)Math.Abs(ct.distanceFromCurrentLine * 0.1));
                            else dist = "<- " + ((int)Math.Abs(ct.distanceFromCurrentLine / 2.54 * 0.1));
                            txtDistanceOffABLine.Text = dist;
                        }

                        //if (guidanceLineHeadingDelta < 0) lblDelta.ForeColor = Color.Red;
                        //else lblDelta.ForeColor = Color.Green;

                        if (guidanceLineDistanceOff == 32020 | guidanceLineDistanceOff == 32000) btnAutoSteer.Text = "-";
                        else btnAutoSteer.Text = "Y";
                    }

                    else
                    {
                        if (ABLine.isABLineSet | ABLine.isABLineBeingSet)
                        {
                            string dist;

                            txtDistanceOffABLine.Visible = true;
                            //lblDelta.Visible = true;
                            DrawLightBar(openGLControl.Width, openGLControl.Height, ABLine.distanceFromCurrentLine * 0.1);
                            if ((ABLine.distanceFromCurrentLine) < 0.0)
                            {
                                // --->
                                txtDistanceOffABLine.ForeColor = Color.Green;
                                if (isMetric) dist = ((int)Math.Abs(ABLine.distanceFromCurrentLine * 0.1)) + " ->";
                                else dist = ((int)Math.Abs(ABLine.distanceFromCurrentLine / 2.54 * 0.1)) + " ->";
                                txtDistanceOffABLine.Text = dist;
                            }

                            else
                            {
                                // <----
                                txtDistanceOffABLine.ForeColor = Color.Red;
                                if (isMetric) dist = "<- " + ((int)Math.Abs(ABLine.distanceFromCurrentLine * 0.1));
                                else dist = "<- " + ((int)Math.Abs(ABLine.distanceFromCurrentLine / 2.54 * 0.1));
                                txtDistanceOffABLine.Text = dist;
                            }

                            //if (guidanceLineHeadingDelta < 0) lblDelta.ForeColor = Color.Red;
                            //else lblDelta.ForeColor = Color.Green;
                            if (guidanceLineDistanceOff == 32020 | guidanceLineDistanceOff == 32000) btnAutoSteer.Text = "-";
                            else btnAutoSteer.Text = "Y";
                        }
                    }

                    //AB line is not set so turn off numbers
                    if (!ABLine.isABLineSet & !ABLine.isABLineBeingSet & !ct.isContourBtnOn)
                    {
                        txtDistanceOffABLine.Visible = false;
                        btnAutoSteer.Text = "-";
                    }
                }
                else
                {
                    txtDistanceOffABLine.Visible = false;
                    btnAutoSteer.Text = "-";
                }

                gl.Flush();//finish openGL commands
                gl.PopMatrix();//  Pop the modelview.

                //  back to the projection and pop it, then back to the model view.
                gl.MatrixMode(OpenGL.GL_PROJECTION);
                gl.PopMatrix();
                gl.MatrixMode(OpenGL.GL_MODELVIEW);

                //reset point size
                gl.PointSize(1.0f);
                gl.Flush();

                if (leftMouseDownOnOpenGL)
                {
                    leftMouseDownOnOpenGL = false;
                    byte[] data1 = new byte[192];

                    //scan the center of click and a set of square points around
                    gl.ReadPixels(mouseX - 4, mouseY - 4, 8, 8, OpenGL.GL_RGB, OpenGL.GL_UNSIGNED_BYTE, data1);

                    //made it here so no flag found
                    flagNumberPicked = 0;

                    for (int ctr = 0; ctr < 192; ctr += 3)
                    {
                        if (data1[ctr] == 255 | data1[ctr + 1] == 255)
                        {
                            flagNumberPicked = data1[ctr + 2];
                            break;
                        }
                    }
                }


                //digital input Master control (WorkSwitch)
                if (isJobStarted && mc.isWorkSwitchEnabled)
                {
                    //check condition of work switch
                    if (mc.isWorkSwitchActiveLow)
                    {
                        //if (mc.workSwitchValue == 0)
                    }
                    else
                    {
                        //if (mc.workSwitchValue == 1)
                    }
                }                

                //stop the timer and calc how long it took to do calcs and draw
                frameTime = (double)swFrame.ElapsedTicks / (double)System.Diagnostics.Stopwatch.Frequency * 1000;

                //if a couple minute has elapsed save the field in case of crash and to be able to resume            
                if (saveCounter > 60)       //2 counts per second X 60 seconds = 120 counts per minute.
                {
                    if (isJobStarted && stripOnlineGPS.Value != 1)
                    {
                        //auto save the field patches, contours accumulated so far
                        FileSaveField();
                        //FileSaveContour();

                        //NMEA log file
                        if (isLogNMEA) FileSaveNMEA();
                    }
                    saveCounter = 0;
                }

                openGLControlBack.DoRender();
            }
        }

        /// Handles the OpenGLInitialized event of the openGLControl control.
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //Load all the textures
            LoadGLTextures();

            //  Set the clear color.
            gl.ClearColor(0.22f, 0.2858f, 0.16f, 1.0f);

            // Set The Blending Function For Translucency
            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
 
            //gl.Disable(OpenGL.GL_CULL_FACE);
            gl.CullFace(OpenGL.GL_BACK);
            
            //set the camera to right distance
            SetZoom();

            //now start the timer assuming no errors, otherwise the program will not stop on errors.
            tmrWatchdog.Enabled = true;
        }

        /// Handles the Resized event of the openGLControl control.
        private void openGLControl_Resized(object sender, EventArgs e)
        {
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            //  Load the identity.
            gl.LoadIdentity();

            //  Create a perspective transformation.
            gl.Perspective(fovy, (double)openGLControl.Width / (double)openGLControl.Height, 1, camDistanceFactor * camera.camSetDistance);

            //  Set the modelview matrix.
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        //main openGL draw function
        private void openGLControlBack_OpenGLDraw(object sender, RenderEventArgs args)
        {
            OpenGL gl = openGLControlBack.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);  // Clear The Screen And The Depth Buffer

            //gl.Enable(OpenGL.GL_LINE_SMOOTH);
            //gl.Enable(OpenGL.GL_BLEND);

            //gl.Hint(OpenGL.GL_LINE_SMOOTH_HINT, OpenGL.GL_FASTEST);
            //gl.Hint(OpenGL.GL_POINT_SMOOTH_HINT, OpenGL.GL_FASTEST);
            //gl.Hint(OpenGL.GL_POLYGON_SMOOTH_HINT, OpenGL.GL_FASTEST);

            gl.LoadIdentity();                  // Reset The View
            CalculateMinMaxCut();

            gl.Translate(0, 0, -maxCutDistance);

            //translate to that spot in the world 
            gl.Translate(-CutCenterX, -CutCenterY, 0);

            gl.Enable(OpenGL.GL_TEXTURE_2D);
            gl.Color(1,1,1);

            //the floor
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, texture[1]);	// Select Our Texture
            gl.Begin(OpenGL.GL_TRIANGLE_STRIP);				            // Build Quad From A Triangle Strip
            gl.TexCoord(0, 0); gl.Vertex(-100, -100, 0.0);                // Top Right
            gl.TexCoord(1, 0); gl.Vertex(100, -100, 0.0);               // Top Left
            gl.TexCoord(0, 1); gl.Vertex(-100, 100, 0.0);               // Bottom Right
            gl.TexCoord(1, 1); gl.Vertex(100, 100, 0.0);              // Bottom Left
            gl.End();						// Done Building Triangle Strip
            gl.Disable(OpenGL.GL_TEXTURE_2D);
            //patch color

            int closestPoint = 0;
            int count2 = ct.topoList.Count;
            gl.LineWidth(2);

            if (count2 > 0)
            {
                minDist = 1000000;
                int ptCount = ct.ptList.Count-1;
                //find the closest point to current fix
                for (int t = 0; t < ptCount; t++)
                {
                    double dist = ((pn.easting - ct.ptList[t].easting) * (pn.easting - ct.ptList[t].easting))
                                    + ((pn.northing - ct.ptList[t].northing) * (pn.northing - ct.ptList[t].northing));
                    if (dist < minDist) { minDist = dist; closestPoint = t;  }
                }

                //draw the ground profile
                gl.Color(0.4f, 0.4f, 0.4f);
                gl.Begin(OpenGL.GL_TRIANGLE_STRIP);
                for (int i = 0; i < count2; i++)
                {
                    gl.Vertex(ct.topoList[i].easting,
                      (((ct.topoList[i].altitude - CutCenterY) * altitudeWindowGain) + CutCenterY), 0);
                    gl.Vertex(ct.topoList[i].easting,-200, 0);
                }
                gl.End();

                //cut line drawn in full
                int cutPts = ct.cutList.Count;
                if (cutPts > 0)
                {
                    gl.Color(0.974f, 0.0f, 0.12f);
                    gl.Begin(OpenGL.GL_LINE_STRIP);
                    for (int i = 0; i < count2; i++)
                    {
                        if (ct.cutList[i].altitude > 0)
                            gl.Vertex(ct.cutList[i].easting, (((ct.cutList[i].altitude - CutCenterY) * altitudeWindowGain) + CutCenterY), 0);
                    }
                    gl.End();

                    lblBladeDelta.Text = (pn.altitude - ct.cutList[closestPoint].altitude).ToString("N3");


                    //gl.Color(0.0f, 1.0f, 0.0f);
                    //gl.PointSize(2);
                    //gl.Begin(OpenGL.GL_POINTS);
                    //for (int i = 0; i < count2; i++)
                    //    if (ct.cutList[i].altitude > 0)
                    //        gl.Vertex(ct.cutList[i].easting, (((ct.cutList[i].altitude - CutCenterY) * altitudeWindowGain) + CutCenterY), 0);
                    //gl.End();

                }


                //crosshairs same spot as mouse - long
                gl.LineWidth(2);
                gl.Begin(OpenGL.GL_LINES);
                gl.Color(0.97020f, 0.65f, 0.0f);
                gl.Vertex(screen2FieldPt.easting, 3000, 0);
                gl.Vertex(screen2FieldPt.easting, -3000, 0);
                gl.Vertex(-10, (((screen2FieldPt.northing - CutCenterY) * altitudeWindowGain) + CutCenterY), 0);
                gl.Vertex(1000, (((screen2FieldPt.northing - CutCenterY) * altitudeWindowGain) + CutCenterY), 0);
                gl.End();


                //draw the guide line being built
                if (ct.isDrawingRefLine)
                {
                    gl.LineWidth(2);
                    gl.Color(0.995f, 0.0f, 0.150f);
                    int cutCnt = ct.drawList.Count;
                    if (cutCnt > 0)
                    {
                        gl.Begin(OpenGL.GL_LINE_STRIP);
                        for (int i = 0; i < cutCnt; i++)
                            gl.Vertex(ct.drawList[i].easting, (((ct.drawList[i].northing - CutCenterY) * altitudeWindowGain) + CutCenterY), 0);
                        gl.End();

                        gl.Color(0.0f, 1.0f, 0.0f);
                        gl.PointSize(4);
                        gl.Begin(OpenGL.GL_POINTS);
                        for (int i = 0; i < cutCnt; i++)
                            gl.Vertex(ct.drawList[i].easting, (((ct.drawList[i].northing - CutCenterY) * altitudeWindowGain) + CutCenterY), 0);
                        gl.End();
                    }
                }

                //gl.LineWidth(1);
                //gl.Begin(OpenGL.GL_LINES);
                //gl.Color(0.20f, 0.2f, 0.2f);
                //gl.Vertex(ct.cutStart.easting, (((ct.cutStart.northing + 0.2 - CutCenterY) * altitudeWindowGain) + CutCenterY), 0);
                //gl.Vertex(ct.cutStop.easting, (((ct.cutStop.northing + 0.2 - CutCenterY) * altitudeWindowGain) + CutCenterY), 0);
                //gl.Vertex(ct.cutStart.easting, (((ct.cutStart.northing - 0.2 - CutCenterY) * altitudeWindowGain) + CutCenterY), 0);
                //gl.Vertex(ct.cutStop.easting, (((ct.cutStop.northing - 0.2 - CutCenterY) * altitudeWindowGain) + CutCenterY), 0);
                //gl.End();

                if (minDist < 25)
                {
                    //draw the actual elevation lines and blade
                    gl.LineWidth(8);
                    gl.Begin(OpenGL.GL_LINES);
                    gl.Color(0.95f, 0.90f, 0.0f);
                    gl.Vertex(ct.topoList[closestPoint].easting, (((pn.altitude - CutCenterY) * altitudeWindowGain) + CutCenterY), 0);
                    gl.Vertex(ct.topoList[closestPoint].easting, 10000, 0);
                    gl.End();

                    //the skinny actual elevation lines
                    gl.LineWidth(1);
                    gl.Begin(OpenGL.GL_LINES);
                    gl.Color(0.990f, 0.9750f, 0.00f);
                    gl.Vertex(ct.topoList[closestPoint].easting - 5000, (((pn.altitude - CutCenterY) * altitudeWindowGain) + CutCenterY), 0);
                    gl.Vertex(ct.topoList[closestPoint].easting + 5000, (((pn.altitude - CutCenterY) * altitudeWindowGain) + CutCenterY), 0);
                    gl.Vertex(ct.topoList[closestPoint].easting, -10000, 0);
                    gl.Vertex(ct.topoList[closestPoint].easting, 10000, 0);

                    //the dashed accent of ground profile
                    gl.Color(0.0f, 0.0f, 0.00f);
                    for (int i = 0; i < count2; i++)
                    {
                        gl.Vertex(ct.topoList[i].easting,
                          (((ct.topoList[i].altitude - CutCenterY) * altitudeWindowGain) + CutCenterY), 0);
                    }
                    gl.End();

                    //little point at cutting edge of blade
                    gl.Color(0.0f, 0.0f, 0.0f);
                    gl.PointSize(8);
                    gl.Begin(OpenGL.GL_POINTS);
                    gl.Vertex(ct.topoList[closestPoint].easting, (((pn.altitude - CutCenterY) * altitudeWindowGain) + CutCenterY), 0);
                    gl.End();


                    //calculate blade to guideline delta
                    //double temp = (double)closestPoint / (double)count2;
                    //cutDelta = pn.altitude - (((ct.cutStop.northing - ct.cutStart.northing) * temp) + ct.cutStart.northing);
                }
            }

            ////elevation of guide line
            //if (ct.topoList.Count > 0)
            //{
            //    gl.DrawText(50, 60, 0, 1, 0, "Tahoma", 48, ct.topoList[closestPoint].altitude.ToString("N2"));
            //    gl.DrawText(openGLControlBack.Width/4, 10, 1, 0, 0, "Tahoma", 48, cutDelta.ToString("N4"));
            //}
            ////actual elevation of cutting point
            //gl.DrawText(50, 10, 1, 1, 0, "Tahoma", 48, pn.altitude.ToString("N2"));
            //gl.DrawText(50, 110, 1, 1, 0, "Tahoma", 36, minDist.ToString("N2"));


            //gl.Disable(OpenGL.GL_BLEND);
            //gl.Enable(OpenGL.GL_DEPTH_TEST);


            ////// 2D Ortho --------------------------
            //gl.MatrixMode(OpenGL.GL_PROJECTION);
            //gl.PushMatrix();
            //gl.LoadIdentity();

            ////negative and positive on width, 0 at top to bottom ortho view
            //gl.Ortho2D(-(double)Width / 2, (double)Width / 2, (double)Height, 0);

            ////  Create the appropriate modelview matrix.
            //gl.MatrixMode(OpenGL.GL_MODELVIEW);
            //gl.PushMatrix();
            //gl.LoadIdentity();

            //gl.Color(1,1,1);
            ////if (isSkyOn)
            //{
            //    //draw the background when in 3D

            //    //-10 to -32 (top) is camera pitch range. Set skybox to line up with horizon 
            //    double hite = 0.5;
            //    //hite = 0.001;

            //    //the background
            //    double winLeftPos = -(double)Width / 2;
            //    double winRightPos = -winLeftPos;
            //    gl.Enable(OpenGL.GL_TEXTURE_2D);
            //    gl.BindTexture(OpenGL.GL_TEXTURE_2D, texture[0]);       // Select Our Texture

            //    gl.Begin(OpenGL.GL_TRIANGLE_STRIP);             // Build Quad From A Triangle Strip
            //    gl.TexCoord(0, 0); gl.Vertex(winRightPos, 0.0); // Top Right
            //    gl.TexCoord(1, 0); gl.Vertex(winLeftPos, 0.0); // Top Left
            //    gl.TexCoord(0, 1); gl.Vertex(winRightPos, hite * (double)Height); // Bottom Right
            //    gl.TexCoord(1, 1); gl.Vertex(winLeftPos, hite * (double)Height); // Bottom Left
            //    gl.End();                       // Done Building Triangle Strip

            //    //disable, straight color
            //    gl.Disable(OpenGL.GL_TEXTURE_2D);

            //}

            //// dot background
            //gl.PointSize(8.0f);
            //gl.Color(0.00f, 1.0f, 0.0f);
            //gl.Begin(OpenGL.GL_POINTS);
            //for (int i = -10; i < 0; i++) gl.Vertex((i * 40), 20);
            //for (int i = 1; i < 11; i++) gl.Vertex((i * 40), 20);
            //gl.End();

            //gl.Flush();//finish openGL commands
            //gl.PopMatrix();//  Pop the modelview.

            ////  back to the projection and pop it, then back to the model view.
            //gl.MatrixMode(OpenGL.GL_PROJECTION);
            //gl.PopMatrix();
            //gl.MatrixMode(OpenGL.GL_MODELVIEW);

            ////reset point size
            //gl.PointSize(1.0f);
            //gl.Flush();
        }

            Point fixPt = new Point();
            vec2 plotPt = new vec2();

        private void openGLControlBack_MouseDown(object sender, MouseEventArgs e)
        {

            if (ct.isDrawingRefLine)
            {
                //OpenGL has line 0 at bottom, Windows at top, so convert
                Point pt = openGLControlBack.PointToClient(Cursor.Position);
                lblX.Text = (pt.X).ToString();
                lblY.Text = ((openGLControlBack.Height - pt.Y) - openGLControlBack.Height / 2).ToString();

                ////Convert to Origin in the center of window, 700 pixels
                fixPt.X = pt.X;
                fixPt.Y = ((openGLControlBack.Height - pt.Y) - openGLControlBack.Height / 2);

                //convert screen coordinates to field coordinates
                plotPt.easting = (int)(((double)fixPt.X) * (double)maxCutDistance / openGLControlBack.Width);
                plotPt.northing = ((double)fixPt.Y) * (double)maxCutDistance / (openGLControlBack.Height * altitudeWindowGain);
                plotPt.northing += CutCenterY;

                lblEast.Text = plotPt.easting.ToString();
                lblNorth.Text = plotPt.northing.ToString();

                    //make sure not going backwards
                int cnt = ct.drawList.Count;
                if (cnt > 0)
                {
                    if (ct.drawList[cnt-1].easting < plotPt.easting)
                    ct.drawList.Add(plotPt);
                }
                //is first point
                else ct.drawList.Add(plotPt);
            }
        }

        Point screenPt = new Point();
        vec2 screen2FieldPt = new vec2();

        private void openGLControlBack_MouseMove(object sender, MouseEventArgs e)
        {
            screenPt.X = e.Location.X;
            screenPt.Y = ((openGLControlBack.Height - e.Location.Y) - openGLControlBack.Height / 2);

            //convert screen coordinates to field coordinates
            screen2FieldPt.easting = ((double)screenPt.X) * (double)maxCutDistance / openGLControlBack.Width;
            //plotPt.easting += fieldCenterX;
            screen2FieldPt.northing = ((double)screenPt.Y) * (double)maxCutDistance / (openGLControlBack.Height * altitudeWindowGain);
            screen2FieldPt.northing += CutCenterY;

            stripTopoLocation.Text = ((int)(screen2FieldPt.easting)).ToString() + "," + screen2FieldPt.northing.ToString("N3");

            if (ct.isDrawingRefLine)
            {
                int cnt = ct.drawList.Count;
                if (cnt > 0)
                {
                    lblDrawSlope.Text = (((screen2FieldPt.northing - ct.drawList[cnt - 1].northing) 
                    / (screen2FieldPt.easting - ct.drawList[cnt - 1].easting))* 100).ToString("N5");
                }
            }
        }

        //Resize is called upn window creation
        private void openGLControlBack_Resized(object sender, EventArgs e)
        {
            //  Get the OpenGL object.
            OpenGL gls = openGLControlBack.OpenGL;

            gls.MatrixMode(OpenGL.GL_PROJECTION);

            //  Load the identity.
            gls.LoadIdentity();

            // change these at your own peril!!!! Very critical
            //  Create a perspective transformation.
            gls.Perspective(53.1, 1, 1, 6000);

            //  Set the modelview matrix.
            gls.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        double maxCutX, maxCutY, minCutX, minCutY, CutCenterX, CutCenterY, maxCutDistance;
        //determine mins maxs of cut line

        private void CalculateMinMaxCut()
        {
            minCutX = 9999999; minCutY = 9999999;
            maxCutX = -9999999; maxCutY = -9999999;

            //every time the section turns off and on is a new patch
            int cnt = ct.topoList.Count;

            if (cnt > 0)
            {
                //for every new chunk of patch
                for (int i = 0; i < cnt; i++)
                {
                    double x = ct.topoList[i].easting;
                    double y = ct.topoList[i].altitude;

                    //also tally the max/min of Cut x and z
                    if (minCutX > x) minCutX = x;
                    if (maxCutX < x) maxCutX = x;
                    if (minCutY > y) minCutY = y;
                    if (maxCutY < y) maxCutY = y;
                }                
            }

            if (maxCutX == -9999999 | minCutX == 9999999 | maxCutY == -9999999 | minCutY == 9999999)
            {
                maxCutX = 0; minCutX = 0; maxCutY = 0; minCutY = 0;
                maxCutDistance = 100;
            }
            else
            {
                //Max horizontal
                maxCutDistance = Math.Abs(minCutX - maxCutX);

                if (maxCutDistance < 20) maxCutDistance = 20;
                if (maxCutDistance > 6000) maxCutDistance = 6000;

                CutCenterX = (maxCutX + minCutX) / 2.0;
                CutCenterY = (maxCutY + minCutY) / 2.0;
                lblCenterY.Text = CutCenterY.ToString("N2");
                lblMaxAltitude.Text = maxCutY.ToString("N2");
                lblMinAltitude.Text = minCutY.ToString("N2");
            }
        }

        //Draw section OpenGL window, not visible
        private void openGLControlBack_OpenGLInitialized(object sender, EventArgs e)
        {
             //LoadGLTexturesBack();
           OpenGL gls = openGLControlBack.OpenGL;

            //  Set the clear color.
            gls.ClearColor(0.6f, 0.6f, 0.66f, 1.0f);

            // Set The Blending Function For Translucency
            gls.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);

            gls.Enable(OpenGL.GL_BLEND);
            //gls.Disable(OpenGL.GL_DEPTH_TEST);

            gls.Enable(OpenGL.GL_CULL_FACE);
            gls.CullFace(OpenGL.GL_BACK);
        }

        public void DrawLightBar(double Width, double Height, double offlineDistance)
        {
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;
            double down = 20;

            gl.LineWidth(1);
            
            //  Dot distance is representation of how far from AB Line
            int dotDistance = (int)(offlineDistance);

            if (dotDistance < -320) dotDistance = -320;
            if (dotDistance > 320) dotDistance = 320;

            if (dotDistance < -10) dotDistance -= 30;
            if (dotDistance > 10) dotDistance += 30;

            // dot background
            gl.PointSize(8.0f);
            gl.Color(0.00f, 0.0f, 0.0f);
            gl.Begin(OpenGL.GL_POINTS);
            for (int i = -10; i < 0; i++) gl.Vertex((i * 40), down);
            for (int i = 1; i < 11; i++) gl.Vertex((i * 40), down);
            gl.End();

            gl.PointSize(4.0f);

            //red left side
            gl.Color(0.9750f, 0.0f, 0.0f);
            gl.Begin(OpenGL.GL_POINTS);
            for (int i = -10; i < 0; i++) gl.Vertex((i * 40), down);

            //green right side
            gl.Color(0.0f, 0.9750f, 0.0f);
            for (int i = 1; i < 11; i++) gl.Vertex((i * 40), down);
            gl.End();

                //Are you on the right side of line? So its green.
                if ((offlineDistance) < 0.0)
                {
                    int dots = dotDistance * -1 / 32;

                    gl.PointSize(32.0f);
                    gl.Color(0.0f, 0.0f, 0.0f);
                    gl.Begin(OpenGL.GL_POINTS);
                    for (int i = 1; i < dots + 1; i++) gl.Vertex((i * 40), down);
                    gl.End();

                    gl.PointSize(24.0f);
                    gl.Color(0.0f, 0.980f, 0.0f);
                    gl.Begin(OpenGL.GL_POINTS);
                    for (int i = 0; i < dots; i++) gl.Vertex((i * 40 + 40), down);
                    gl.End();
                    //return;
                }

                else
                {
                    int dots = dotDistance / 32;

                    gl.PointSize(32.0f);
                    gl.Color(0.0f, 0.0f, 0.0f);
                    gl.Begin(OpenGL.GL_POINTS);
                    for (int i = 1; i < dots + 1; i++) gl.Vertex((i * -40), down);
                    gl.End();

                    gl.PointSize(24.0f);
                    gl.Color(0.980f, 0.30f, 0.0f);
                    gl.Begin(OpenGL.GL_POINTS);
                    for (int i = 0; i < dots; i++) gl.Vertex((i * -40 - 40), down);
                    gl.End();
                    //return;
                }
            
            //yellow center dot
            if (dotDistance >= -10 && dotDistance <= 10)
            {
                gl.PointSize(32.0f);                
                gl.Color(0.0f, 0.0f, 0.0f);
                gl.Begin(OpenGL.GL_POINTS);
                gl.Vertex(0, down);
                //gl.Vertex(0, down + 50);
                gl.End();

                gl.PointSize(24.0f);
                gl.Color(0.980f, 0.98f, 0.0f);
                gl.Begin(OpenGL.GL_POINTS);
                gl.Vertex(0, down);
                //gl.Vertex(0, down + 50);
                gl.End();
            }

            else
            {

                gl.PointSize(8.0f);
                gl.Color(0.00f, 0.0f, 0.0f);
                gl.Begin(OpenGL.GL_POINTS);
                gl.Vertex(-0, down);
                //gl.Vertex(0, down + 30);
                //gl.Vertex(0, down + 60);
                gl.End();

                //gl.PointSize(4.0f);
                //gl.Color(0.9250f, 0.9250f, 0.250f);
                //gl.Begin(OpenGL.GL_POINTS);
                //gl.Vertex(0, down);
                //gl.Vertex(0, down + 30);
                //gl.Vertex(0, down + 60);
                //gl.End();
            }
        }

        //endo of class
    }
}
