using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.world {
    public interface WorldProvider {

        void markBlockUpdate(int x, int y, int z);
    }
}
