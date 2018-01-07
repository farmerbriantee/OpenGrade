//Please, if you use this, share the improvements

using System;
using SharpGL;

namespace OpenGrade
{
    public class CVehicle
    {
        private readonly OpenGL gl;
        private readonly FormGPS mf;

        //vehicle specific
        public bool isPivotBehindAntenna;
        public double antennaHeight;
        public double antennaPivot;
        public double wheelbase;

        //width of cutting tool
        public double toolWidth;

        //autosteer values
        public double goalPointLookAhead;
        public double maxSteerAngle;
        public double maxAngularVelocity;

        public CVehicle(OpenGL _gl, FormGPS _f)
        {
            //constructor
            gl = _gl;
            mf = _f;

            //from settings grab the vehicle specifics
            isPivotBehindAntenna = Properties.Vehicle.Default.setVehicle_isPivotBehindAntenna;
            antennaHeight = Properties.Vehicle.Default.setVehicle_antennaHeight;
            antennaPivot = Properties.Vehicle.Default.setVehicle_antennaPivot;
            toolWidth = Properties.Vehicle.Default.setVehicle_toolWidth;

            wheelbase = Properties.Vehicle.Default.setVehicle_wheelbase;
            goalPointLookAhead = Properties.Vehicle.Default.setVehicle_goalPointLookAhead;
            maxAngularVelocity = Properties.Vehicle.Default.setVehicle_maxAngularVelocity;
            maxSteerAngle = Properties.Vehicle.Default.setVehicle_maxSteerAngle;
        }

        public void DrawVehicle()
        {
            //translate and rotate at pivot axle
            gl.Translate(mf.pn.easting, mf.pn.northing, 0);
            gl.Rotate(glm.toDegrees(-mf.fixHeading), 0.0, 0.0, 1.0);

            //draw the vehicle Body
            gl.Color(0.9, 0.5, 0.530);
            gl.Begin(OpenGL.GL_TRIANGLE_FAN);
            gl.Vertex(0, 0, -0.2);
            gl.Vertex(2.0, -antennaPivot, 0.0);
            gl.Vertex(0, -antennaPivot + 2, 0.0);
            gl.Color(0.520, 0.0, 0.9);
            gl.Vertex(-2.0, -antennaPivot, 0.0);
            gl.Vertex(2.0, -antennaPivot, 0.0);
            gl.End();

            //tool
            gl.Color(0.95f, 0.90f, 0.0f);
            gl.LineWidth(8.0f);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(-toolWidth/2, 0, 0);
            gl.Vertex(toolWidth/2, 0, 0);
            gl.End();

            gl.LineWidth(1);
        }
    }
}

