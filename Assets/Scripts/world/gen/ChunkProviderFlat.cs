using Assets.Scripts.world.blocks;
using Cubix.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.world {
    public class ChunkProviderFlat : IChunkProvider {
        private World world;

        public ChunkProviderFlat(World world) {
            this.world = world;
        }

        public Chunk provideChunk(int x, int z) {
            byte[] noise = new byte[32768];
            this.generateNoise(x, z, noise);
            Chunk chunk = new Chunk(x, z, noise);
            return chunk;
        }

        private void generateNoise(int ChunkX, int XhunkZ, byte[] noise) {
            for (int x = 0; x < 16; x++) {
                for (int z = 0; z < 16; z++) {
                    for (int y = 0; y < 5; y++) {
                        if (y < 4) {
                            this.setBlock(x, y, z, noise, Block.DIRT.getBlockID());
                        } else {
                            this.setBlock(x, y, z, noise, Block.GRASS.getBlockID());
                        }
                    }
                }
            }
        }

        public void setBlock(int x, int y, int z, byte[] noise, int blockID) {
            if (x < 0 || y < 0 || z < 0) return;
            if (x > 16 || z > 16) return;
            if (y > 256) return;
            noise[x << 11 | z << 7 | y] = (byte)blockID;
        }
    }
}
