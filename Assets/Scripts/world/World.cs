using Assets.Scripts.save;
using Assets.Scripts.world;
using Assets.Scripts.world.gen;
using Assets.Scripts.world.gen.noise;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Cubix.world {
    public class World : IBlockAccess {

        private int seed;
        private IChunkProvider chunkProvider;
        private List<WorldProvider> worldProviders = new List<WorldProvider>();
        private List<IWorldSave> worldSavers = new List<IWorldSave>();

        public World() {
            this.seed = 1;//new System.Random().Next(100000000);
			this.chunkProvider = new ChunkProvider(new MinecraftGenerator(this));
        }

        public int getBlockID(int x, int y, int z) {
            Chunk chunk = this.getChunkFromBlockCoords(x, z);
            return chunk.getBlock(x & 15, y, z & 15);
        }

        public void setBlockID(int x, int y, int z, int blockID, bool update) {
            Chunk chunk = this.getChunkFromBlockCoords(x, z);
            chunk.setBlock(x & 15, y, z & 15, blockID);
            if (update) {
                foreach (WorldProvider provider in this.worldProviders) {
                    provider.markBlockUpdate(x, y, z);
                }

                foreach (IWorldSave saver in this.worldSavers) {
                    saver.onBlockChange(x, y, z, blockID);
                }
            }
        }

        public void setBlockID(int x, int y, int z, int blockID) {
            this.setBlockID(x, y, z, blockID, false);
        }

        public Chunk getChunkFromChunkCoords(int x, int z) {
            return this.chunkProvider.provideChunk(x, z);
        }

        public Chunk getChunkFromBlockCoords(int x, int z) {
            return this.getChunkFromChunkCoords(x >> 4, z >> 4);
        }

        public void addWorldProvider(WorldProvider worldProvider) {
            this.worldProviders.Add(worldProvider);
        }

        public void addWorldSaver(IWorldSave saver) {
            this.worldSavers.Add(saver);
        }

        public int getSeed() {
            return this.seed;
        }
    }
}
