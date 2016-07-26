using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.renderer {
    public class RenderSorter : IComparer<WorldRenderer> {

        private Vector3 entity;
        private Plane[] planes;

        public RenderSorter(Vector3 entity) {
            this.entity = entity;
        }

        public int doCompare(WorldRenderer chunkRender1, WorldRenderer chunkRender2) {
            if (chunkRender1.visible && !chunkRender2.visible) {
                return 1;
            } else if (chunkRender2.visible && !chunkRender1.visible) {
                return -1;
            } else {
                double pos1 = (double)chunkRender1.distanceToEntitySquared(this.entity);
                double pos2 = (double)chunkRender2.distanceToEntitySquared(this.entity);
                //return pos1 < pos2 ? -1 : (pos1 > pos2 ? 1 : (chunkRender1.getChunkIndex() < chunkRender2.getChunkIndex() ? -1 : 1));
                return pos1 < pos2 ? -1 : (pos1 > pos2 ? 1 : -1);
            }
        }

        public int Compare(WorldRenderer x, WorldRenderer y) {
            return this.doCompare(x, y);
        }

    }
}
