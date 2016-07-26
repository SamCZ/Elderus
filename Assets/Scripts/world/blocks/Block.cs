using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.world.blocks {
    public class Block {

        public static Block[] blocks = new Block[256];

        public static Block AIR         = new Block(0, new Vector2(0, 0)).setTransparent();
        public static Block STONE       = new Block(1, new Vector2(1, 0));
        public static Block DIRT        = new Block(2, new Vector2(2, 0));
        public static Block GRASS       = new Block(3, new Vector2(0, 0));
        public static Block GRASS_SIDE  = new Block(4, new Vector2(3, 0));
        public static Block WOOD_LOG    = new Block(5, new Vector2(4, 1));
        public static Block WOOD_LEAVES = new Block(6, new Vector2(4, 3)).setTransparent();
        public static Block WATER       = new Block(7, new Vector2(13, 12)).setTransparent();

        public static Block BEDROCK     = new Block(8, new Vector2(1, 1));
        public static Block SAND = new Block(9, new Vector2(2, 1));
        public static Block SANDSTONE = new Block(9, new Vector2(2, 1));

        public int blockID;
        private Vector2 texture;
        private bool transparent = false;

        public Block(int blockID, Vector2 texture) {
            this.blockID = blockID;
            this.texture = texture;
            blocks[blockID] = this;
        }

        public Block setTransparent() {
            this.transparent = true;
            return this;
        }

        public bool isTransparent() {
            return this.transparent;
        }

        public int getBlockID() {
            return this.blockID;
        }

        public Vector2 getTexture() {
            return this.texture;
        }

    }
}
