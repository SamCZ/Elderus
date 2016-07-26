using Assets.Scripts.world.blocks;
using Cubix.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.world.gen {
    public class TreeGenerator {

        private World world;
        private Random rand = new Random();

        public TreeGenerator(World world) {
            this.world = world;
        }

        public void generateTree(int x, int y, int z) {

            int trunkHeight = 5 + rand.Next(3);
            int leavesStart = trunkHeight - 1 - rand.Next(2);
            int leavesEnd = trunkHeight + 2 + rand.Next(1);

            int leaves = Block.WOOD_LEAVES.getBlockID();
            this.fill(x - 2, y + leavesStart, z - 2, x + 2, y + leavesEnd - 2, z + 2, leaves);
            this.fill(x - 2, y + leavesStart + 2, z - 1, x + 2, y + leavesEnd - 1, z + 1, leaves);
            this.fill(x - 1, y + leavesStart + 2, z - 2, x + 1, y + leavesEnd - 1, z + 2, leaves);
            this.world.setBlockID(x - 1, y + leavesEnd, z, leaves);
            this.world.setBlockID(x + 1, y + leavesEnd, z, leaves);
            this.world.setBlockID(x, y + leavesEnd, z - 1, leaves);
            this.world.setBlockID(x, y + leavesEnd, z + 1, leaves);
            this.world.setBlockID(x, y + leavesEnd, z, leaves);

            this.fill(x, y, z, x, y + trunkHeight, z, Block.WOOD_LOG.getBlockID());
        }

        public void fill(int x0, int y0, int z0, int x1, int y1, int z1, int id) {
            for (int x = x0; x <= x1; x++) {
                for (int y = y0; y <= y1; y++) {
                    for (int z = z0; z <= z1; z++) {
                        this.world.setBlockID(x, y, z, id);
                    }
                }
            }
        }

    }
}
