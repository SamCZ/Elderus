using Assets.Scripts.world.blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.world.biome {
    public class Biome {

        public Biome[] biomes = new Biome[255];

        public static Biome plains = new Biome(0);
        public static Biome hills = new Biome(1).setMinMaxHeight(0.3F, 1.5F);

        public byte topBlock;
        public byte fillerBlock;

        /** The minimum height of this biome. Default 0.1. */
        public float minHeight;

        /** The maximum height of this biome. Default 0.3. */
        public float maxHeight;

        public int biomeID;

        public Biome(int id) {
            this.biomeID = id;
            this.topBlock = (byte)Block.GRASS.getBlockID();
            this.fillerBlock = (byte)Block.DIRT.getBlockID();
            this.minHeight = 0.1f;
            this.maxHeight = 0.3f;
            this.biomes[id] = this;
        }

        public Biome setMinHeight(float var1) {
            this.minHeight = var1;
            return this;
        }

        public Biome setMaxHeight(float var1) {
            this.maxHeight = var1;
            return this;
        }

        private Biome setMinMaxHeight(float p1, float p2) {
            this.setMinHeight(p1);
            this.setMaxHeight(p2);
            return this;
        }

    }
}
