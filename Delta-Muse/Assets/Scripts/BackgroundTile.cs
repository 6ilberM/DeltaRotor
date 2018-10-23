using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BackgroundTile : MonoBehaviour
{
    [SerializeField]
    private GameObject go_tileobj;
    private BoxCollider2D canvasCollider;

    // Use this for initialization
    void Start()
    {
        if (go_tileobj != null)
        {
            DrawTiledBackground();
        }
        //How to find specific obj
        GameObject sceneCamObj = GameObject.Find("Main Camera");
        // Should output the real dimensions of scene viewport
        Debug.Log(sceneCamObj.GetComponent<Camera>().pixelWidth);
        Debug.Log(sceneCamObj.GetComponent<Camera>().pixelHeight);

    }

    private void Awake()
    {
        canvasCollider = GetComponent<BoxCollider2D>();

    }
    // Update is called once per frame
    void Update()
    {
    }

    void DrawTiledBackground()
    {
        Vector2 canvasSize = canvasCollider.bounds.size;

        // instantiate one tile to measure its size, then destroy it
        var templateTile = Instantiate(go_tileobj, Vector2.zero, Quaternion.identity) as GameObject;
        Vector2 tileSize = templateTile.GetComponent<Renderer>().bounds.size;

        float tilesX = canvasSize.x / tileSize.x;
        float tilesY = canvasSize.y / tileSize.y;
        Destroy(templateTile);

        // start placing tiles from the bottom left
        Vector2 bottomLeft = new Vector2(canvasCollider.transform.position.x - (canvasSize.x / 2), canvasCollider.transform.position.y - (canvasSize.y / 2));

        for (int i = 0; i < tilesX; i++)
        {
            for (int j = 0; j < tilesY; j++)
            {
                var newTilePos = new Vector2(bottomLeft.x + i * tileSize.x, bottomLeft.y + tileSize.y * j);
                var newTile = Instantiate(go_tileobj, newTilePos, Quaternion.identity) as GameObject;
                newTile.transform.parent = transform;
            }
        }

        // turn the template's renderer off
        go_tileobj.GetComponent<SpriteRenderer>().enabled = false;
    }
}
