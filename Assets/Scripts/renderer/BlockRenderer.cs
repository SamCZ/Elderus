using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cubix.world;
using Assets.Scripts.world.blocks;
using UnityEngine;

namespace Assets.Scripts.renderer {
    public class BlockRenderer {

        private IBlockAccess blockAccess;
        private MeshBuilder chunkMesh;
        private MeshBuilder chunkMesh_transparent;

        public BlockRenderer(IBlockAccess blockAccess, MeshBuilder chunkMesh, MeshBuilder chunkMesh_transparent) {
            this.blockAccess = blockAccess;
            this.chunkMesh = chunkMesh;
            this.chunkMesh_transparent = chunkMesh_transparent;
        }

        public void renderBlock(Block block, int x, int y, int z) {
            MeshBuilder builder = null;
            if (block.isTransparent()) {
                builder = this.chunkMesh_transparent;
            } else {
                builder = this.chunkMesh;
            }

            this.renderDefaultBlock(builder, block, x, y, z);
        }

        private void renderDefaultBlock(MeshBuilder builder, Block block, int x, int y, int z) {
            float size = 1.00001F;

            int topBlock = this.blockAccess.getBlockID(x, y + 1, z);
            int bottomBlock = this.blockAccess.getBlockID(x, y - 1, z);
            int leftBlock = this.blockAccess.getBlockID(x - 1, y, z);
            int rightBlock = this.blockAccess.getBlockID(x + 1, y, z);
            int backBlock = this.blockAccess.getBlockID(x, y, z + 1);
            int frontBlock = this.blockAccess.getBlockID(x, y, z - 1);

            int blockID = block.blockID;

            if (Block.blocks[topBlock].isTransparent()) {
                if (topBlock != blockID) topBlock = 0;
            }

            if (Block.blocks[bottomBlock].isTransparent()) {
                if (bottomBlock != blockID) bottomBlock = 0;
            }

            if (Block.blocks[backBlock].isTransparent()) {
                if (backBlock != blockID) backBlock = 0;
            }

            if (Block.blocks[frontBlock].isTransparent()) {
                if (frontBlock != blockID) frontBlock = 0;
            }

            if (Block.blocks[leftBlock].isTransparent()) {
                if (leftBlock != blockID) leftBlock = 0;
            }

            if (Block.blocks[rightBlock].isTransparent()) {
                if (rightBlock != blockID) rightBlock = 0;
            }

            if (topBlock > 0 && bottomBlock > 0 && backBlock > 0 && frontBlock > 0 && leftBlock > 0 && rightBlock > 0) {
                return;
            }

            if (topBlock == 0) {
                this.addBoxFace(blockID, 0, 
                    new float[] { x, y + size, z },
                    new float[] { x, y + size, z + size },
                    new float[] { x + size, y + size, z + size },
                    new float[] { x + size, y + size, z });
            }

            if (bottomBlock == 0) {
                this.addBoxFace(blockID, 5,
                        new float[] { x, y, z },
                        new float[] { x + size, y, z },
                        new float[] { x + size, y, z + size },
                        new float[] { x, y, z + size });
            }

            if (leftBlock == 0) {
                this.addBoxFace(blockID, 3,
                        new float[] { x, y + size, z },
                        new float[] { x, y, z },
                        new float[] { x, y, z + size },
                        new float[] { x, y + size, z + size });
            }

            if (rightBlock == 0) {
                this.addBoxFace(blockID, 4,
                        new float[] { x + size, y + size, z + size },
                        new float[] { x + size, y, z + size },
                        new float[] { x + size, y, z },
                        new float[] { x + size, y + size, z });
            }

            if (frontBlock == 0) {
                this.addBoxFace(blockID, 1,
                        new float[] { x + size, y + size, z },
                        new float[] { x + size, y, z },
                        new float[] { x, y, z },
                        new float[] { x, y + size, z });
            }

            if (backBlock == 0) {
                this.addBoxFace(blockID, 2,
                        new float[] { x, y + size, z + size },
                        new float[] { x, y, z + size },
                        new float[] { x + size, y, z + size },
                        new float[] { x + size, y + size, z + size });
            }
        }

        public void addBoxFace(int blockID, int side, float[] par1, float[] par2, float[] par3, float[] par4) {

            Block block = Block.blocks[blockID];
            Vector2 texture = block.getTexture();

            if (block.getBlockID() == Block.GRASS.getBlockID()) {
                if (side == 0) {
                    texture = Block.GRASS.getTexture();
                } else if (side == 5) {
                    texture = Block.DIRT.getTexture();
                } else {
                    texture = Block.GRASS_SIDE.getTexture();
                }
            }

            MeshBuilder builder = null;

            if (block.isTransparent()) {
                builder = this.chunkMesh_transparent;
            } else {
                builder = this.chunkMesh;
            }

            builder.addQuad(par1, par2, par3, par4, true);

            float tUnit = 1F / (512F / 32F);

            builder.addUV(tUnit * texture.x, -(tUnit * texture.y));
            builder.addUV(tUnit * texture.x, -(tUnit * texture.y + tUnit));
            builder.addUV(tUnit * texture.x + tUnit, -(tUnit * texture.y + tUnit));
            builder.addUV(tUnit * texture.x + tUnit, -(tUnit * texture.y));
        }

    }
}
