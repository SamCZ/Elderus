using Assets.Scripts.world;
using Cubix.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.renderer {
    public class RenderGlobal : WorldProvider {

        private World world;
        private Camera camera;
        private WorldRenderer[] worldRenderers;
        public List<WorldRenderer> worldRenderersToUpdate = new List<WorldRenderer>();
        public List<WorldRenderer> worldRenderersToImmediatelyUpdate = new List<WorldRenderer>();
        private int chunkWide;
        private int chunkDeep;
        private int chunkTall;
        private int worldRenderersCheckIndex;
        private int viewDistance = 16;
        private float prevSortX = 0;
        private float prevSortY = 0;
        private float prevSortZ = 0;
        private bool sorted;
        private int prewChunkRender = -1;
        private int frustumCheckOffset;

        public RenderGlobal(Camera camera) {
            this.camera = camera;
        }

        public void loadRenderers() {
            int distance = 64 << 3 - this.viewDistance;

            if (distance > 400) {
                distance = 400;
            }

            this.chunkWide = distance / 16 + 1;
            this.chunkDeep = distance / 16 + 1;
            this.chunkTall = 16;
            this.worldRenderers = new WorldRenderer[this.chunkWide * this.chunkDeep * this.chunkTall];
            int chunkIndex = 0;
            for (int var1 = 0; var1 < this.chunkWide; ++var1) {
                for (int var2 = 0; var2 < this.chunkDeep; ++var2) {
                    for (int var3 = 0; var3 < this.chunkTall; ++var3) {
                        WorldRenderer renderer = this.worldRenderers[(var2 * this.chunkTall + var3) * this.chunkWide + var1] = new WorldRenderer(this.world, var1 * 16, var3 * 16, var2 * 16);

                        renderer.setDirty();
                        renderer.setChunkIndex(chunkIndex);
                        this.worldRenderersToUpdate.Add(renderer);

                        chunkIndex++;

                        float maxWorld = this.worldRenderers.Length;
                        float par1 = chunkIndex;
                        int per = (int)((par1 / maxWorld) * 100F);
                        if (per % 10 == 0) {
                            if (this.prewChunkRender != per) {
                                //Debug.Log("Generating world... " + per + "%");
                                this.prewChunkRender = per;
                            }
                        }


                    }
                }
            }

            int playerX = (int)this.camera.transform.position.x;
            int playerY = (int)this.camera.transform.position.y;
            int playerZ = (int)this.camera.transform.position.z;
            this.updateForNewPosition(playerX, playerY, playerZ);
            this.sortRenderers(this.camera.transform.position);
        }

        public void updateAndRender(Material[] materials, Camera camera) {
            this.updateRenderers(materials, camera);

            for (int i = 0; i < 10; ++i) {
                this.worldRenderersCheckIndex = (this.worldRenderersCheckIndex + 1) % this.worldRenderers.Length;
                WorldRenderer renderer = this.worldRenderers[this.worldRenderersCheckIndex];
                if (renderer.isNeedsUpdate() && !this.worldRenderersToUpdate.Contains(renderer)) {
                    this.worldRenderersToUpdate.Add(renderer);
                }
            }

            float playerX = this.camera.transform.position.x;
            float playerY = this.camera.transform.position.y;
            float playerZ = this.camera.transform.position.z;

            float var1 = playerX - this.prevSortX;
            float var2 = playerY - this.prevSortY;
            float var3 = playerZ - this.prevSortZ;

            if (var1 * var1 + var2 * var2 + var3 * var3 > 16F) {
                this.prevSortX = playerX;
                this.prevSortY = playerY;
                this.prevSortZ = playerZ;
                this.updateForNewPosition((int)playerX, (int)playerY, (int)playerZ);
                this.sortRenderers(new Vector3(playerX, playerY, playerZ));
            }
        }

        private void updateRenderers(Material[] materials, Camera camera) {

            List<WorldRenderer> worldRenderersToRemove = new List<WorldRenderer>();
            List<WorldRenderer> worldRenderersToImRemove = new List<WorldRenderer>();
            int i = 0;
            //Debug.Log(this.worldRenderersToUpdate.Count());
            foreach (WorldRenderer renderer in this.worldRenderersToUpdate.ToArray()) {
                renderer.updateRenderer(materials);
                renderer.setClean();
                worldRenderersToRemove.Add(renderer);
                i++;
                if (i > 2) {
                    break;
                }
            }

            if (this.worldRenderersToImmediatelyUpdate.Count() > 0) {
                foreach (WorldRenderer renderer in this.worldRenderersToImmediatelyUpdate.ToArray()) {
                    renderer.updateRenderer(materials);
                    renderer.setClean();
                    worldRenderersToImRemove.Add(renderer);
                }

                foreach (WorldRenderer renderer in worldRenderersToImRemove) {
                    this.worldRenderersToImmediatelyUpdate.Remove(renderer);
                }
            }

            foreach (WorldRenderer renderer in worldRenderersToRemove) {
                this.worldRenderersToUpdate.Remove(renderer);
            }
        }

        public void sortRenderers(Vector3 camera) {
            Thread t = new Thread(sort);
            t.Start(camera);
        }

        private void sort(object obj) {
            Vector3 camera = (Vector3)obj;
            RenderSorter sorter = new RenderSorter(camera);
            if (this.worldRenderersToUpdate.Count() > 0) {
                this.worldRenderersToUpdate.Sort(sorter);
            }
        }

        public void setWorldAndLoad(World world) {
            this.world = world;
            if (world != null) {
                this.loadRenderers();
            }
        }

        private void updateForNewPosition(int playerX, int playerY, int playerZ) {
            int pX = playerX - 8;
            int pZ = playerZ - 8;

            int var0 = this.chunkWide * 16;
            int var1 = var0 / 2;

            for (int var2 = 0; var2 < this.chunkWide; ++var2) {
                int newPosX = var2 * 16;
                int var5 = newPosX + var1 - pX;
                if (var5 < 0) {
                    var5 -= var0 - 1;
                }
                var5 /= var0;
                newPosX -= var5 * var0;
                for (int var3 = 0; var3 < this.chunkDeep; ++var3) {
                    int newPosZ = var3 * 16;
                    int var6 = newPosZ + var1 - pZ;
                    if (var6 < 0) {
                        var6 -= var0 - 1;
                    }
                    var6 /= var0;
                    newPosZ -= var6 * var0;

                    for (int var4 = 0; var4 < this.chunkTall; ++var4) {
                        int newPosY = var4 * 16;
                        WorldRenderer worldRenderer = this.worldRenderers[(var3 * this.chunkTall + var4) * this.chunkWide + var2];
                        bool needUpdate = worldRenderer.isNeedsUpdate();
                        worldRenderer.setPosition(newPosX, newPosY, newPosZ);
                        if (!needUpdate && worldRenderer.isNeedsUpdate()) {
                            this.worldRenderersToUpdate.Add(worldRenderer);
                        }
                    }
                }
            }
        }

        public void markBlockUpdate(int x, int y, int z) {
            this.markBlocksForUpdate(x - 1, y - 1, z - 1, x + 1, y + 1, z + 1);
        }

        private void markBlocksForUpdate(int par1, int par2, int par3, int par4, int par5, int par6) {
            int var7 = bucketInt(par1, 16);
            int var8 = bucketInt(par2, 16);
            int var9 = bucketInt(par3, 16);
            int var10 = bucketInt(par4, 16);
            int var11 = bucketInt(par5, 16);
            int var12 = bucketInt(par6, 16);

            for (int var13 = var7; var13 <= var10; ++var13) {
                int var14 = var13 % this.chunkWide;

                if (var14 < 0) {
                    var14 += this.chunkWide;
                }

                for (int var15 = var8; var15 <= var11; ++var15) {
                    int var16 = var15 % this.chunkTall;

                    if (var16 < 0) {
                        var16 += this.chunkTall;
                    }

                    for (int var17 = var9; var17 <= var12; ++var17) {
                        int var18 = var17 % this.chunkDeep;

                        if (var18 < 0) {
                            var18 += this.chunkDeep;
                        }

                        int var19 = (var18 * this.chunkTall + var16) * this.chunkWide + var14;
                        WorldRenderer var20 = this.worldRenderers[var19];

                        if (var20 != null && !var20.isNeedsUpdate()) {
                            var20.setDirty();
                            this.worldRenderersToImmediatelyUpdate.Add(var20);
                        }
                    }
                }
            }
        }

        public int bucketInt(int p_76137_0_, int p_76137_1_) {
            return p_76137_0_ < 0 ? -((-p_76137_0_ - 1) / p_76137_1_) - 1 : p_76137_0_ / p_76137_1_;
        }

    }
}
