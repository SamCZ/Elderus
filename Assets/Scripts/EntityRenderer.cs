using UnityEngine;
using System.Collections;
using Assets.Scripts.renderer;
using Cubix.world;
using System;
using Assets.Scripts.world.blocks;

public class EntityRenderer : MonoBehaviour {

    public Material blocks_opaque;
    public Material blocks_translucent;
    public Material block_selector;
    public Texture crosshair;
    private Material[] materials;

	private World world;
    private RenderGlobal renderGlobal;
    private Vector3 playerLocation;
    private Vector3 playerRotation;
    private Cube cube;

	void Start () {
        this.materials = new Material[] { this.blocks_opaque, this.blocks_translucent };
		this.world = new World ();
        this.renderGlobal = new RenderGlobal(Camera.main);
        this.renderGlobal.setWorldAndLoad(this.world);
        this.world.addWorldProvider(this.renderGlobal);

        this.cube = new Cube(Vector3.zero, this.block_selector);
	}
	
	void Update () {
        this.playerLocation = Camera.main.transform.position;
        this.playerRotation = Camera.main.transform.eulerAngles;
        this.renderGlobal.updateAndRender(this.materials, Camera.main);
        this.handleBlockEvent();
	}

    public void handleBlockEvent() {
        Vector3[] look = this.getPlayerLook();
        Vector3 lookingAt = look[0];
        Vector3 PlaceAt = look[1];

        this.cube.setPosition(lookingAt);

        if (Input.GetMouseButtonDown(0)) {
            this.world.setBlockID((int)Math.Floor(lookingAt.x), (int)Math.Floor(lookingAt.y), (int)Math.Floor(lookingAt.z), Block.AIR.blockID, true);
        } else if (Input.GetMouseButtonDown(1)) {

        }
    }

    public Vector3[] getPlayerLook() {
        Vector3 lookingAt = Vector3.zero;
        Vector3 placeAt = Vector3.zero;
        int pickBlockDistance = 8;

        float xn = (float)this.playerLocation.x;
        float yn = (float)this.playerLocation.y;
        float zn = (float)this.playerLocation.z;

        float xl;
        float yl;
        float zl;

        float yChange = (float)Math.Cos((this.playerRotation.x + 90) / 180 * Math.PI);
        float ymult = (float)Math.Sin((this.playerRotation.x + 90) / 180 * Math.PI);

        float xChange = (float)(-Math.Cos((this.playerRotation.y + 90) / 180 * Math.PI) * ymult);
        float zChange = (float)(Math.Sin((this.playerRotation.y + 90) / 180 * Math.PI) * ymult);

        for (float f = 0; f <= pickBlockDistance; f += 0.01f) {
            xl = xn;
            yl = yn;
            zl = zn;

            xn = (float)(this.playerLocation.x + f * xChange);
            yn = (float)(this.playerLocation.y + f * yChange);
            zn = (float)(this.playerLocation.z + f * zChange);

            int blockID = this.world.getBlockID((int)Math.Floor(xn), (int)Math.Floor(yn), (int)Math.Floor(zn));
            if (blockID > 0) {
                lookingAt = new Vector3((int)Math.Floor(xn), (int)Math.Floor(yn), (int)Math.Floor(zn));
                placeAt = new Vector3((int)Math.Floor(xl), (int)Math.Floor(yl), (int)Math.Floor(zl));
                break;
            }
        }

        return new Vector3[] { lookingAt, placeAt };
    }

    void OnGUI() {
        if (this.crosshair != null) {
            GUI.DrawTexture(new Rect(Screen.width / 2 - crosshair.width / 2, Screen.height / 2 - crosshair.height / 2, crosshair.width, crosshair.height), crosshair);
        }
    }
}
