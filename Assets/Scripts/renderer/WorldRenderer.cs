using Assets.Scripts.world;
using Assets.Scripts.world.blocks;
using Cubix.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.renderer {
    public class WorldRenderer {

        private World world;
        private int xPos;
        private int yPos;
        private int zPos;
        private int xCenter;
        private int yCenter;
        private int zCenter;
        private int chunkIndex;
        private bool needsUpdate;
        public bool visible;
        private Bounds boundingBox;

        private MeshBuilder chunkMesh;
        private MeshBuilder chunkMesh_transparent;

        public WorldRenderer(World world, int x, int y, int z) {
            this.world = world;
            this.setPosition(x, y, z);
            this.needsUpdate = true;
        }

        public void setPosition(int x, int y, int z) {
            if (this.xPos != x || this.yPos != y || this.zPos != z) {
                this.xPos = x;
                this.yPos = y;
                this.zPos = z;
                this.xCenter = this.xPos + 8;
                this.yCenter = this.yPos + 8;
                this.zCenter = this.zPos + 8;
                this.boundingBox = new Bounds(new Vector3(this.xCenter, this.yCenter, this.zCenter), new Vector3(20, 20, 20));
                if (this.chunkMesh != null) {
                    this.chunkMesh.setPosition(this.xPos, this.yPos, this.zPos);
                    this.chunkMesh_transparent.setPosition(this.xPos, this.yPos, this.zPos);
                }
                this.needsUpdate = true;
            }
        }

        public void updateRenderer(Material[] materials) {
            if (this.needsUpdate) {
                this.needsUpdate = false;
                int posX = this.xPos;
                int posY = this.yPos;
                int posZ = this.zPos;
                int posXPlus = posX + 16;
                int posYPlus = posY + 16;
                int posZPlus = posZ + 16;
                byte plus = 1;

                ChunkCache chunkCache = new ChunkCache(this.world, posX - plus, posY - plus, posZ - plus, posXPlus + plus, posYPlus + plus, posZPlus + plus, plus);
                if (!chunkCache.isEmpty()) {
                    this.createMeshes(materials);
                    BlockRenderer blockRenderer = new BlockRenderer(chunkCache, this.chunkMesh, this.chunkMesh_transparent);
                    for (int blockX = posX; blockX < posXPlus; blockX++) {
                        for (int blockZ = posZ; blockZ < posZPlus; blockZ++) {
                            for (int blockY = posY; blockY < posYPlus; blockY++) {
                                int blockID = chunkCache.getBlockID(blockX, blockY, blockZ);
                                if (blockID > 0) {
                                    blockRenderer.renderBlock(Block.blocks[blockID], blockX, blockY, blockZ);
                                }
                            }
                        }
                    }
                    this.chunkMesh.build();
                    this.chunkMesh_transparent.build();
                    this.visible = true;
                } else {
                    this.visible = false;
                }
            }
        }

        public bool isInFrustum(Plane[] planes) {
            if (this.boundingBox == null) {
                this.boundingBox = new Bounds(new Vector3(this.xCenter, this.yCenter, this.zCenter), new Vector3(20, 20, 20));
            }
            return GeometryUtility.TestPlanesAABB(planes, this.boundingBox);
        }

        public void createMeshes(Material[] materials) {
            if (this.chunkMesh == null) {
                this.chunkMesh = new MeshBuilder(materials[0]);
                this.chunkMesh.setName("Chunk(x=" + this.xPos + ", y=" + yPos + ", z=" + zPos + ")");
                this.chunkMesh.setPosition(this.xPos, this.yPos, this.zPos);
            }

            if (this.chunkMesh_transparent == null) {
                this.chunkMesh_transparent = new MeshBuilder(materials[1]);
                this.chunkMesh_transparent.setName("Chunk(x=" + this.xPos + ", y=" + yPos + ", z=" + zPos + ")_tra");
                this.chunkMesh_transparent.setPosition(this.xPos, this.yPos, this.zPos);
            }
        }

        public float distanceToEntitySquared(Vector3 entity) {
            float pos1 = (float)(entity.x - (double)this.xCenter);
            float pos2 = (float)(entity.y - (double)this.yCenter);
            float pos3 = (float)(entity.z - (double)this.zCenter);
            return pos1 * pos1 + pos2 * pos2 + pos3 * pos3;
        }

        public int getChunkIndex() {
            return this.chunkIndex;
        }

        public void setChunkIndex(int index) {
            this.chunkIndex = index;
        }

        public bool isNeedsUpdate() {
            return this.needsUpdate;
        }

        public void setDirty() {
            this.needsUpdate = true;
        }

        public void setClean() {
            this.needsUpdate = false;
        }

    }
}
