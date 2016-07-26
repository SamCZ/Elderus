using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.world.gen.noise {
    public class Grad {

        public double x, y, z, w;

        public Grad(double x, double y, double z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Grad(double x, double y, double z, double w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

    }
}
