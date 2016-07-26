using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.world.storage {
    public class NibbleArray {

        public readonly byte[] data;
        private readonly int depthBits;
        private readonly int depthBitsPlusFour;

        public NibbleArray(int par1, int par2) {
            this.data = new byte[par1 >> 1];
            this.depthBits = par2;
            this.depthBitsPlusFour = par2 + 4;
        }

        public NibbleArray(byte[] par1ArrayOfByte, int par2) {
            this.data = par1ArrayOfByte;
            this.depthBits = par2;
            this.depthBitsPlusFour = par2 + 4;
        }

        public int get(int x, int y, int z) {
            int var4 = y << this.depthBitsPlusFour | z << this.depthBits | x;
            int var5 = var4 >> 1;
            int var6 = var4 & 1;
            return var6 == 0 ? this.data[var5] & 15 : this.data[var5] >> 4 & 15;
        }

        public void set(int par1, int par2, int par3, int par4) {
            int var5 = par2 << this.depthBitsPlusFour | par3 << this.depthBits | par1;
            int var6 = var5 >> 1;
            int var7 = var5 & 1;

            if (var7 == 0) {
                this.data[var6] = (byte)(this.data[var6] & 240 | par4 & 15);
            } else {
                this.data[var6] = (byte)(this.data[var6] & 15 | (par4 & 15) << 4);
            }
        }

    }
}
