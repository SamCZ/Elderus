using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cubix.world {
    public interface IChunkProvider {
        Chunk provideChunk(int x, int z);

    }
}
