using Assets.Scripts.world.blocks;
using Assets.Scripts.world.gen.noise;
using Cubix.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.world.gen {
    public class BlockPopulator {

        private bool populated = false;
        private World world;
        private SimplexNoise biome_noise;
        private TreeGenerator treeGen;

        public BlockPopulator(World world) {
            this.world = world;
            this.treeGen = new TreeGenerator(this.world);
            int seed = this.world.getSeed();
            this.biome_noise = new SimplexNoise(2048, 0.3, seed + 1);
        }

        public void populateChunk(Chunk chunk, Random rand) {
            if (!this.populated) {
                this.populated = true;
                //this.genTrees(chunk);
            }
        }

        private void genTrees(Chunk chunk) {
            Random rand = new Random();
            int x = chunk.getXPos() * 16;
            int z = chunk.getZPos() * 16;
            if (this.getBiomeAtLocation(x, z) == 1) {
                if (rand.Next(5) == 0) {
                    int tx = chunk.getXPos() * 16 + 8;
                    int tz = chunk.getZPos() * 16 + 8;
                    int y = chunk.getHeight(tx & 15, tz & 15);
                    if (Block.blocks[this.world.getBlockID(tx, y - 1, tz)].isTransparent()) return;
                    
                    this.treeGen.generateTree(tx, y, tz);
                }
            }
        }

        public int getBiomeAtLocation(int x, int z) {
            float noise = (float)this.biome_noise.getNoise(x, z);
            return noise > 0 ? 1 : 0;
        }

    }
}
