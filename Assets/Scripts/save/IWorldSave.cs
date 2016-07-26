using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.save {
    public interface IWorldSave {

        void onBlockChange(int x, int y, int z, int blockID);

    }
}
