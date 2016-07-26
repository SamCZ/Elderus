using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.renderer {
    public class MeshBuilder {

        public List<Vector3> vertices = new List<Vector3>();
        public List<int> triangles = new List<int>();
        public List<Vector2> uv = new List<Vector2>();
        public List<Color> colors = new List<Color>();
        public List<Vector3> colVertices = new List<Vector3>();
        public List<int> colTriangles = new List<int>();

        private GameObject obj;
        private Mesh mesh;
        private MeshCollider col;
        private int squareCount;
        private int colCount;

        public MeshBuilder(Material material) {
            this.obj = new GameObject();
            this.obj.AddComponent<MeshRenderer>();
            this.obj.GetComponent<Renderer>().material = material;
            this.mesh = this.obj.AddComponent<MeshFilter>().mesh;
            this.col = this.obj.AddComponent<MeshCollider>();
        }

        public bool isInFrustum() {
            return this.obj.GetComponent<Renderer>().isVisible;
        }

        public void setPosition(float x, float y, float z) {
            this.obj.transform.position.Set(x, y, z);
        }

        public void setName(String name) {
            this.obj.gameObject.name = name;
        }

        public void addQuad(float[] par1, float[] par2, float[] par3, float[] par4, Boolean genCollider) {
            vertices.Add(new Vector3(par1[0], par1[1], par1[2]));
            vertices.Add(new Vector3(par2[0], par2[1], par2[2]));
            vertices.Add(new Vector3(par3[0], par3[1], par3[2]));
            vertices.Add(new Vector3(par4[0], par4[1], par4[2]));

            triangles.Add(squareCount * 4);
            triangles.Add((squareCount * 4) + 1);
            triangles.Add((squareCount * 4) + 3);
            triangles.Add((squareCount * 4) + 1);
            triangles.Add((squareCount * 4) + 2);
            triangles.Add((squareCount * 4) + 3);
            squareCount++;

            if (genCollider) {
                this.addQuadCollider(par1, par2, par3, par4);
            }
        }

        public void addQuad(float[] par1, float[] par2, float[] par3, float[] par4) {
            this.addQuad(par1, par2, par3, par4, false);
        }

        public void addQuadCollider(float[] par1, float[] par2, float[] par3, float[] par4) {
            colVertices.Add(new Vector3(par1[0], par1[1], par1[2]));
            colVertices.Add(new Vector3(par2[0], par2[1], par2[2]));
            colVertices.Add(new Vector3(par3[0], par3[1], par3[2]));
            colVertices.Add(new Vector3(par4[0], par4[1], par4[2]));

            colTriangles.Add(colCount * 4);
            colTriangles.Add((colCount * 4) + 1);
            colTriangles.Add((colCount * 4) + 3);
            colTriangles.Add((colCount * 4) + 1);
            colTriangles.Add((colCount * 4) + 2);
            colTriangles.Add((colCount * 4) + 3);
            colCount++;
        }

        public void addQuadCollider(Vector3 par1, Vector3 par2, Vector3 par3, Vector3 par4) {
            this.addQuadCollider(
                   new float[] { par1.x, par1.y, par1.z
                }, new float[] { par2.x, par2.y, par2.z
                }, new float[] { par3.x, par3.y, par3.z
                }, new float[] { par4.x, par4.y, par4.z
            });
        }

        public void addUV(float u, float v) {
            this.uv.Add(new Vector2(u, v));
        }

        public void addColor(float r, float g, float b, float a) {
            this.colors.Add(new Color(r, g, b, a));
        }

        public void addColor(float r, float g, float b) {
            this.colors.Add(new Color(r, g, b, 1F));
        }

        public void build() {
            this.mesh.Clear();
            this.mesh.vertices = this.vertices.ToArray();
            this.mesh.triangles = this.triangles.ToArray();
            this.mesh.uv = this.uv.ToArray();
            this.mesh.colors = this.colors.ToArray();
            this.mesh.Optimize();
            this.mesh.RecalculateNormals();

            this.squareCount = 0;
            this.vertices.Clear();
            this.triangles.Clear();
            this.uv.Clear();
            this.colors.Clear();

            Mesh newMesh = new Mesh();
            newMesh.vertices = this.colVertices.ToArray();
            newMesh.triangles = this.colTriangles.ToArray();
            this.col.sharedMesh = newMesh;

            this.colVertices.Clear();
            this.colTriangles.Clear();
            this.colCount = 0;
        }

    }
}
