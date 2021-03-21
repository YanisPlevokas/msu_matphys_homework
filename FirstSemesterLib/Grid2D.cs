using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace FirstSemesterLib
{
    [Serializable]

    public struct Grid2D
    {
        public float OX_step { get; set; }
        public int OX_net_counter { get; set; }
        public float OY_step { get; set; }
        public int OY_net_counter { get; set; }

        public Grid2D(float step_x, int net_x, float step_y, int net_y)
        {
            this.OX_step = step_x;
            this.OX_net_counter = net_x;
            this.OY_step = step_y;
            this.OY_net_counter = net_y;
        }
        public Vector2 GetCoord(int x, int y)
        {
            return new Vector2(OX_step * x, OY_step * y);
        }

        public override string ToString()
        {
            return OX_step.ToString() + " - OX_step, " + OX_net_counter.ToString() + " - OX_net_counter, " +
                OY_step.ToString() + " - OY_step, " + OY_net_counter.ToString() + " - OY_net_counter.";
        }
        public string ToString(string format)
        {
            return OX_step.ToString(format) + " - OX_step, " + OX_net_counter.ToString(format) + " - OX_net_counter, " +
                OY_step.ToString(format) + " - OY_step, " + OY_net_counter.ToString(format) + " - OY_net_counter.";
        }
    }
}
