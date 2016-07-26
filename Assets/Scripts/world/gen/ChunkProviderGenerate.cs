using Assets.Scripts.world.blocks;
using Assets.Scripts.world.gen.noise;
using Cubix.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.world.gen {
    class ChunkProviderGenerate : IChunkProvider {

        private World world;
        private SimplexNoise biome_noise;
        private System.Random rand = new System.Random();

        public ChunkProviderGenerate(World world) {
            this.world = world;
            int seed = this.world.getSeed();
            this.biome_noise = new SimplexNoise(2048, 0.3, seed + 1);
        }

        public Chunk provideChunk(int x, int z) {
            byte[] noise = new byte[32768];
            this.generateNoise(x, z, noise);
            Chunk chunk = new Chunk(x, z, noise);
            return chunk;
        }

        private void generateNoise(int x, int z, byte[] noise) {
            int chunkX = x * 16;
            int chunkZ = z * 16;
            for (int blockX = 0; blockX < 16; blockX++) {
                for (int blockZ = 0; blockZ < 16; blockZ++) {
                    //int maxY = NoiseGen(chunkX + blockX, chunkZ + blockZ, 30, 20 * this.world.getSeed(), 1);
                    int biome = this.getBiomeAtLocation(chunkX + blockX, chunkZ + blockZ);
                    int maxY = this.GetTerrainHeight(chunkX + blockX, chunkZ + blockZ, this.world.getSeed(), biome);
                    //noise[blockX << 11 | blockZ << 7 | maxY] = (byte)2;

                    for (int blockY = 0; blockY < maxY; blockY++) {
                        if (blockY == maxY - 1) {
                            noise[blockX << 11 | blockZ << 7 | blockY] = (byte)Block.GRASS.getBlockID();
                        } else {
                            noise[blockX << 11 | blockZ << 7 | blockY] = (byte)Block.DIRT.getBlockID();
                        }
                    }

                    if (maxY < 31) {
                        //noise[blockX << 11 | blockZ << 7 | maxY] = (byte)Block.WATER.getBlockID();
                    }
                }
            }
        }

        public void generateTree(byte[] noise, int x, int y, int z) {
            int log;
            for (log = y; log < y + 4; log++) {
                this.setBlock(x, log, z, noise, Block.WOOD_LOG.getBlockID());
            }

            this.setBlock(x, log, z, noise, Block.WOOD_LEAVES.getBlockID());
        }

        public int getBiomeAtLocation(int x, int z) {
            float noise = (float)this.biome_noise.getNoise(x, z);
            return noise > 0 ? 100 : 200;
        }

        public void setBlock(int x, int y, int z, byte[] noise, int blockID) {
            if (x < 0 || y < 0 || z < 0) return;
            if (x > 16 || z > 16) return;
            if (y > 256) return;
            noise[x << 11 | z << 7 | y] = (byte)blockID;
        }

        float Noise(int x, int y, float seed, float size) {
            return Mathf.PerlinNoise(seed + (x / size), seed + (y / size));
        }

        public int GetTerrainHeight(int x, int z, float seed, int maxY) {
            float var1 = 0F;
            var1 = this.Noise(x, z, seed, 128);
            var1 *= 100;
            return (int)Math.Floor(var1);
        }

        int NoiseGen(int x, int y, float scale, float mag, float exp) {
            return (int)(Mathf.Pow((Mathf.PerlinNoise(x / scale, y / scale) * mag), (exp)));
        }

    }
}
