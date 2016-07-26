using Assets.Scripts.save;
using Cubix.world;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Assets.Scripts {
    public class WorldSaver : IWorldSave {

        public String worldPath;
        private Dictionary<string, BlockLocation> blockLocations = new Dictionary<string, BlockLocation>();

        public WorldSaver() {
            this.worldPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "world.txt");
            if (File.Exists(this.worldPath)) {
                this.setupBlocks();
            } else {
                using (File.Create(this.worldPath));
            }
        }

        private void setupBlocks() {
            using (StreamReader reader = new StreamReader(this.worldPath)) {
                String line = "";
                while ((line = reader.ReadLine()) != null) {
                    if (line.Length > 0) {
                        BlockLocation loc = BlockLocation.fromString(line);
                        this.blockLocations.Add((loc.x + ":" + loc.y + ":" + loc.z), loc);
                    }
                }
            }
        }

        public void onBlockChange(int x, int y, int z, int blockID) {
            String key = x + ":" + y + ":" + z;
            BlockLocation loc = new BlockLocation();
            loc.x = x;
            loc.y = y;
            loc.z = z;
            loc.blockID = blockID;
            if (this.blockLocations.ContainsKey(key)) {
                this.blockLocations[key] = loc;
            } else {
                this.blockLocations.Add(key, loc);
            }
        }

        public void save() {
            using (StreamWriter writer = new StreamWriter(this.worldPath)) {
                if (this.blockLocations.Count() > 0) {
                    List<BlockLocation> list = new List<BlockLocation>(this.blockLocations.Values);
                    foreach (BlockLocation loc in list) {
                        writer.Write(loc.toString() + "\n");
                    }
                }
            }
        }

        public void load(World world) {
            if (this.blockLocations.Count() > 0) {
                List<BlockLocation> list = new List<BlockLocation>(this.blockLocations.Values);
                foreach (BlockLocation loc in list) {
                    world.setBlockID(loc.x, loc.y, loc.z, loc.blockID);
                }
            }
        }
    }
}
