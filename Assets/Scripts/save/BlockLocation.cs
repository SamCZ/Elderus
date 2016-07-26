using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.save {
    public class BlockLocation {

        public int x, y, z, blockID;

        public String toString() {
            return this.x + ":" + this.y + ":" + this.z + ":" + this.blockID;
        }

        public static BlockLocation fromString(String str) {
            BlockLocation loc = new BlockLocation();
            String[] ex = str.Split(':');
            loc.x = Convert.ToInt16(ex[0]);
            loc.y = Convert.ToInt16(ex[1]);
            loc.z = Convert.ToInt16(ex[2]);
            loc.blockID = Convert.ToInt16(ex[3]);
            return loc;
        }

    }
}
