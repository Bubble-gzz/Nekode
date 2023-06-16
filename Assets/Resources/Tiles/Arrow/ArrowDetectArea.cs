using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDetectArea : MonoBehaviour
{
    public MyTile tile;
    [SerializeField]
    public int id;
    [SerializeField]
    GameObject arrowPrefab;
    GameObject arrowGhost;
    bool mouseEnter;
    void Awake()
    {
        mouseEnter = false;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseEnter()
    {
        if (!Arrow.IsArrow(MyGrid.currentTileType)) return;
        if (Global.gameState != Global.GameState.Editing) return;
        arrowGhost = Instantiate(arrowPrefab, transform);
        arrowGhost.transform.position = transform.position;
        arrowGhost.GetComponent<Arrow>().isGhost = true;
        arrowGhost.GetComponent<Arrow>().type = Arrow.TileToArrowType(MyGrid.currentTileType);
        SpriteRenderer sprite = arrowGhost.GetComponentInChildren<SpriteRenderer>();
        Color newColor = sprite.color;
        newColor.a = 0.5f;
        sprite.color = newColor;
        Global.mouseOverArrow = true;
        mouseEnter = true;
    }
    void OnMouseExit()
    {
        if (Global.gameState != Global.GameState.Editing) return;
        mouseEnter = false;
        if (arrowGhost != null) Destroy(arrowGhost);
        Global.mouseOverArrow = false;
    }
    void OnMouseDown()
    {
        if (Global.gameState != Global.GameState.Editing) return;
        if (!Arrow.IsArrow(MyGrid.currentTileType)) return;
        mouseEnter = false;
        if (arrowGhost != null) Destroy(arrowGhost);
        MyTile.Type type = MyGrid.currentTileType;
        tile.PlaceArrow(id,  Arrow.TileToArrowType(type));
        if (tile.myGrid.tileCount[(int)type] > 0) tile.myGrid.tileCount[(int)type]--;
        if (tile.myGrid.tileCount[(int)type] == 0) MyGrid.currentTileType = MyTile.Type.NULL;
    }
}
