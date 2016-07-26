using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cubix.world {
    public interface IBlockAccess {

        int getBlockID(int x, int y, int z);

    }
}
