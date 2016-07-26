using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.renderer {
    public class Cube {

        private GameObject cube;

        public Cube(Vector3 vec, Material mat) {
            this.cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            this.cube.transform.position = vec;
            this.cube.GetComponent<Renderer>().material = mat;
            //cube.GetComponent<MeshCollider>().sharedMesh = new Mesh();;
            this.cube.transform.localScale = new Vector3(1.01f, 1.01f, 1.01f);
        }

        public void setPosition(Vector3 vec) {
            this.cube.transform.position = new Vector3(vec.x + this.cube.transform.localScale.x / 2F, vec.y + this.cube.transform.localScale.y / 2F, vec.z + this.cube.transform.localScale.z / 2F);
        }

    }
}
