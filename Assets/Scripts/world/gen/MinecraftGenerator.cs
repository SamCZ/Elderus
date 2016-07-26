using Assets.Scripts.world.biome;
using Assets.Scripts.world.blocks;
using Cubix.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.world.gen.noise {
    class MinecraftGenerator : IChunkProvider {

        private World world;
        private Random rand;
        private NoiseGeneratorOctaves noiseGen1;
        private NoiseGeneratorOctaves noiseGen2;
        private NoiseGeneratorOctaves noiseGen3;
        private NoiseGeneratorOctaves noiseGen4;
        private NoiseGeneratorOctaves noiseGen5;
        private NoiseGeneratorOctaves noiseGen6;
        private SimplexNoise biome_noise;
        private double[] noiseArray;
        private double[] stoneNoise = new double[256];

        double[] noise3;
        double[] noise1;
        double[] noise2;
        double[] noise5;
        double[] noise6;
        float[] parabolicField;

        public MinecraftGenerator(World world) {
            this.world = world;
            this.rand = new Random(world.getSeed());
            this.noiseGen1 = new NoiseGeneratorOctaves(this.rand, 16);
            this.noiseGen2 = new NoiseGeneratorOctaves(this.rand, 16);
            this.noiseGen3 = new NoiseGeneratorOctaves(this.rand, 8);
            this.noiseGen4 = new NoiseGeneratorOctaves(this.rand, 4);
            this.noiseGen5 = new NoiseGeneratorOctaves(this.rand, 10);
            this.noiseGen6 = new NoiseGeneratorOctaves(this.rand, 16);
            this.biome_noise = new SimplexNoise(2048, 0.3, world.getSeed() + 1);
        }

        public Chunk provideChunk(int x, int z) {
            byte[] var3 = new byte[32768];
            this.generateTerrain(x, z, var3);
            this.replaceBlocksForBiome(x, z, var3);
            Chunk chunk = new Chunk(x, z, var3);
            return chunk;
        }

        public void replaceBlocksForBiome(int par1, int par2, byte[] par3ArrayOfByte) {
            byte var5 = 63;
            double var6 = 0.03125D;
            this.stoneNoise = this.noiseGen4.generateNoiseOctaves(this.stoneNoise, par1 * 16, par2 * 16, 0, 16, 16, 1, var6 * 2.0D, var6 * 2.0D, var6 * 2.0D);

            for (int var8 = 0; var8 < 16; ++var8) {
                for (int var9 = 0; var9 < 16; ++var9) {
                    Biome biome = this.getBiomeAtLocation(par1, par2);
                    float var11 = 0.2f;
                    int var12 = (int)(this.stoneNoise[var8 + var9 * 16] / 3.0D + 3.0D + this.rand.NextDouble() * 0.25D);
                    int var13 = -1;
                    byte var14 = biome.topBlock;
                    byte var15 = biome.fillerBlock;

                    for (int var16 = 127; var16 >= 0; --var16) {
                        int var17 = (var9 * 16 + var8) * 128 + var16;

                        if (var16 <= 0 + this.rand.Next(5)) {
                            par3ArrayOfByte[var17] = (byte)Block.BEDROCK.getBlockID();
                        } else {
                            byte var18 = par3ArrayOfByte[var17];

                            if (var18 == 0) {
                                var13 = -1;
                            } else if (var18 == Block.STONE.getBlockID()) {
                                if (var13 == -1) {
                                    if (var12 <= 0) {
                                        var14 = 0;
                                        var15 = (byte)Block.STONE.getBlockID();
                                    } else if (var16 >= var5 - 4 && var16 <= var5 + 1) {
                                        var14 = biome.topBlock;
                                        var15 = biome.fillerBlock;
                                    }

                                    if (var16 < var5 && var14 == 0) {
                                        if (var11 < 0.15F) {
                                            //var14 = (byte)Block.ice.getBlockID();
                                        } else {
                                            var14 = (byte)Block.WATER.getBlockID();
                                        }
                                    }

                                    var13 = var12;

                                    if (var16 >= var5 - 1) {
                                        par3ArrayOfByte[var17] = var14;
                                    } else {
                                        par3ArrayOfByte[var17] = var15;
                                    }
                                } else if (var13 > 0) {
                                    --var13;
                                    par3ArrayOfByte[var17] = var15;

                                    if (var13 == 0 && var15 == Block.SAND.getBlockID()) {
                                        var13 = this.rand.Next(4);
                                        var15 = (byte)Block.SANDSTONE.getBlockID();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void generateTerrain(int x, int z, byte[] par3ArrayOfByte) {
            byte var4 = 4;
            byte var5 = 16;
            byte var6 = 63;
            int var7 = var4 + 1;
            byte var8 = 17;
            int var9 = var4 + 1;
            this.noiseArray = this.initializeNoiseField(this.noiseArray, x * var4, 0, z * var4, var7, var8, var9, x, z);

            for (int var10 = 0; var10 < var4; ++var10) {
                for (int var11 = 0; var11 < var4; ++var11) {
                    for (int var12 = 0; var12 < var5; ++var12) {
                        double var13 = 0.125D;
                        double var15 = this.noiseArray[((var10 + 0) * var9 + var11 + 0) * var8 + var12 + 0];
                        double var17 = this.noiseArray[((var10 + 0) * var9 + var11 + 1) * var8 + var12 + 0];
                        double var19 = this.noiseArray[((var10 + 1) * var9 + var11 + 0) * var8 + var12 + 0];
                        double var21 = this.noiseArray[((var10 + 1) * var9 + var11 + 1) * var8 + var12 + 0];
                        double var23 = (this.noiseArray[((var10 + 0) * var9 + var11 + 0) * var8 + var12 + 1] - var15) * var13;
                        double var25 = (this.noiseArray[((var10 + 0) * var9 + var11 + 1) * var8 + var12 + 1] - var17) * var13;
                        double var27 = (this.noiseArray[((var10 + 1) * var9 + var11 + 0) * var8 + var12 + 1] - var19) * var13;
                        double var29 = (this.noiseArray[((var10 + 1) * var9 + var11 + 1) * var8 + var12 + 1] - var21) * var13;

                        for (int var31 = 0; var31 < 8; ++var31) {
                            double var32 = 0.25D;
                            double var34 = var15;
                            double var36 = var17;
                            double var38 = (var19 - var15) * var32;
                            double var40 = (var21 - var17) * var32;

                            for (int var42 = 0; var42 < 4; ++var42) {
                                int var43 = var42 + var10 * 4 << 11 | 0 + var11 * 4 << 7 | var12 * 8 + var31;
                                short var44 = 128;
                                var43 -= var44;
                                double var45 = 0.25D;
                                double var49 = (var36 - var34) * var45;
                                double var47 = var34 - var49;

                                for (int var51 = 0; var51 < 4; ++var51) {
                                    if ((var47 += var49) > 0.0D) {
                                        par3ArrayOfByte[var43 += var44] = (byte)Block.STONE.getBlockID();
                                    } else if (var12 * 8 + var31 < var6) {
                                        par3ArrayOfByte[var43 += var44] = (byte)Block.WATER.getBlockID();
                                    } else {
                                        par3ArrayOfByte[var43 += var44] = 0;
                                    }
                                }

                                var34 += var38;
                                var36 += var40;
                            }

                            var15 += var23;
                            var17 += var25;
                            var19 += var27;
                            var21 += var29;
                        }
                    }
                }
            }
        }

        private double[] initializeNoiseField(double[] par1ArrayOfDouble, int par2, int par3, int par4, int par5, int par6, int par7, int x, int z) {
            if (par1ArrayOfDouble == null) {
                par1ArrayOfDouble = new double[par5 * par6 * par7];
            }

            if (this.parabolicField == null) {
                this.parabolicField = new float[25];

                for (int var8 = -2; var8 <= 2; ++var8) {
                    for (int var9 = -2; var9 <= 2; ++var9) {
                        float var10 = 10.0F / (float)Math.Sqrt((float)(var8 * var8 + var9 * var9) + 0.2F);
                        this.parabolicField[var8 + 2 + (var9 + 2) * 5] = var10;
                    }
                }
            }

            double var44 = 684.412D;
            double var45 = 684.412D;
            this.noise5 = this.noiseGen5.generateNoiseOctaves(this.noise5, par2, par4, par5, par7, 1.121D, 1.121D, 0.5D);
            this.noise6 = this.noiseGen6.generateNoiseOctaves(this.noise6, par2, par4, par5, par7, 200.0D, 200.0D, 0.5D);
            this.noise3 = this.noiseGen3.generateNoiseOctaves(this.noise3, par2, par3, par4, par5, par6, par7, var44 / 80.0D, var45 / 160.0D, var44 / 80.0D);
            this.noise1 = this.noiseGen1.generateNoiseOctaves(this.noise1, par2, par3, par4, par5, par6, par7, var44, var45, var44);
            this.noise2 = this.noiseGen2.generateNoiseOctaves(this.noise2, par2, par3, par4, par5, par6, par7, var44, var45, var44);
            bool var43 = false;
            bool var42 = false;
            int var12 = 0;
            int var13 = 0;

            for (int var14 = 0; var14 < par5; ++var14) {
                for (int var15 = 0; var15 < par7; ++var15) {
                    float var16 = 0.0F;
                    float var17 = 0.0F;
                    float var18 = 0.0F;
                    byte var19 = 2;

                    for (int var21 = -var19; var21 <= var19; ++var21) {
                        for (int var22 = -var19; var22 <= var19; ++var22) {
                            Biome biome = this.getBiomeAtLocation(x, z);

                            float var24 = this.parabolicField[var21 + 2 + (var22 + 2) * 5] / (biome.minHeight + 2.0F);

                            if (true) {
                                var24 /= 2.0F;
                            }

                            var16 += biome.maxHeight * var24;
                            var17 += biome.minHeight * var24;
                            var18 += var24;
                        }
                    }

                    var16 /= var18;
                    var17 /= var18;
                    var16 = var16 * 0.9F + 0.1F;
                    var17 = (var17 * 4.0F - 1.0F) / 8.0F;
                    double var46 = this.noise6[var13] / 8000.0D;

                    if (var46 < 0.0D) {
                        var46 = -var46 * 0.3D;
                    }

                    var46 = var46 * 3.0D - 2.0D;

                    if (var46 < 0.0D) {
                        var46 /= 2.0D;

                        if (var46 < -1.0D) {
                            var46 = -1.0D;
                        }

                        var46 /= 1.4D;
                        var46 /= 2.0D;
                    } else {
                        if (var46 > 1.0D) {
                            var46 = 1.0D;
                        }

                        var46 /= 8.0D;
                    }

                    ++var13;

                    for (int var47 = 0; var47 < par6; ++var47) {
                        double var48 = (double)var17;
                        double var26 = (double)var16;
                        var48 += var46 * 0.2D;
                        var48 = var48 * (double)par6 / 16.0D;
                        double var28 = (double)par6 / 2.0D + var48 * 4.0D;
                        double var30 = 0.0D;
                        double var32 = ((double)var47 - var28) * 12.0D * 128.0D / 128.0D / var26;

                        if (var32 < 0.0D) {
                            var32 *= 4.0D;
                        }

                        double var34 = this.noise1[var12] / 512.0D;
                        double var36 = this.noise2[var12] / 512.0D;
                        double var38 = (this.noise3[var12] / 10.0D + 1.0D) / 2.0D;

                        if (var38 < 0.0D) {
                            var30 = var34;
                        } else if (var38 > 1.0D) {
                            var30 = var36;
                        } else {
                            var30 = var34 + (var36 - var34) * var38;
                        }

                        var30 -= var32;

                        if (var47 > par6 - 4) {
                            double var40 = (double)((float)(var47 - (par6 - 4)) / 3.0F);
                            var30 = var30 * (1.0D - var40) + -10.0D * var40;
                        }

                        par1ArrayOfDouble[var12] = var30;
                        ++var12;
                    }
                }
            }

            return par1ArrayOfDouble;
        }

        public Biome getBiomeAtLocation(int x, int z) {
            float noise = (float)this.biome_noise.getNoise(x, z);
            return Biome.hills;
        }

    }
}
