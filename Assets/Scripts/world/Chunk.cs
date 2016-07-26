using Assets.Scripts.world.storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Cubix.world {
    public class Chunk {

        public static int SIZE = 16;
        private int xPos;
        private int zPos;
        private BlockStorage[] storageArrays = new BlockStorage[16];
        private int[,] heightMap = new int[SIZE, SIZE];

        public Chunk(int chunkX, int chunkZ, byte[] chunkNoise) {
            this.xPos = chunkX;
            this.zPos = chunkZ;
            int k = chunkNoise.Length / 256;
            for (int x = 0; x < 16; ++x) {
                for (int z = 0; z < 16; ++z) {
                    for (int y = 0; y < k; ++y) {
                        byte blockID = chunkNoise[x << 11 | z << 7 | y];
                        if (blockID != 0) {
                            int ybase = y >> 4;
                            if (this.storageArrays[ybase] == null) {
                                this.storageArrays[ybase] = new BlockStorage(ybase << 4, false);
                            }

                            this.storageArrays[ybase].setExtBlockID(x, y & 15, z, blockID);
                        }
                    }
                }
            }

            this.generateHeightMap();
        }

        public void generateHeightMap() {
            for (int x = 0; x < SIZE; x++) {
                for (int z = 0; z < SIZE; z++) {
                    int y = 256;
                    while (true) {
                        int blockID = this.getBlock(x, y, z);
                        if (blockID != 0) {
                            this.heightMap[x, z] = y;
                            break;
                        }
                        y--;
                    }
                }
            }
        }

        public bool getAreLevelsEmpty(int minY, int maxY) {
            if (minY < 0) {
                minY = 0;
            }

            if (maxY >= 256) {
                maxY = 255;
            }

            for (int var3 = minY; var3 <= maxY; var3 += 16) {
                BlockStorage var4 = this.storageArrays[var3 >> 4];

                if (var4 != null && !var4.isEmpty()) {
                    return false;
                }
            }

            return true;
        }

        public bool getAreLevelsFilled(int minY, int maxY) {
            if (minY < 0) {
                minY = 0;
            }

            if (maxY >= 256) {
                maxY = 255;
            }

            for (int var3 = minY; var3 <= maxY; var3 += 16) {
                BlockStorage var4 = this.storageArrays[var3 >> 4];

                if (var4 != null && !var4.isFilled()) {
                    return false;
                }
            }

            return true;
        }

        public int getBlock(int x, int y, int z) {
            if (y < 0 || y > 255) return 0;
            BlockStorage var4 = this.storageArrays[y >> 4];
            return var4 != null ? var4.getExtBlockID(x, y & 15, z) : 0;
        }

        public int getBlockMetadata(int x, int y, int z) {
            if (y >> 4 >= this.storageArrays.Length) {
                return 0;
            } else {
                BlockStorage var4 = this.storageArrays[y >> 4];
                return var4 != null ? var4.getExtBlockMetadata(x, y & 15, z) : 0;
            }
        }

        public void setBlock(int x, int y, int z, int blockID) {
            if (y < 0 || y > 255) return;
            this.setBlockIDWithMetadata(x, y, z, blockID, 0);
        }

        public void setBlockIDWithMetadata(int x, int y, int z, int blockID, int metadata) {
            int var6 = y << 4 | x;

            int var8 = this.getBlock(x, y, z);
            int var9 = this.getBlockMetadata(x, y, z);

            if (var8 == blockID && var9 == metadata) {
                return;
            } else {
                BlockStorage blockStorage = this.storageArrays[y >> 4];
                if (blockStorage == null) {
                    blockStorage = (this.storageArrays[y >> 4] = new BlockStorage(y >> 4 << 4, false));
                }

                blockStorage.setExtBlockID(x, y & 15, z, blockID);
                blockStorage.setExtBlockMetadata(x, y & 15, z, metadata);
            }
        }

        public int getHeight(int x, int z) {
            return this.heightMap[x, z];
        }

        public int getXPos() {
            return this.xPos;
        }

        public int getZPos() {
            return this.zPos;
        }

        public static long chunkXZ2Int(int x, int z) {
            return (long)z & 4294967295L | ((long)x & 4294967295L) << 32;
        }

    }
}
