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
        mouseEnter = false;
        if (arrowGhost != null) Destroy(arrowGhost);
        Global.mouseOverArrow = false;
    }
    void OnMouseDown()
    {
        if (!Arrow.IsArrow(MyGrid.currentTileType)) return;
        mouseEnter = false;
        if (arrowGhost != null) Destroy(arrowGhost);
        tile.PlaceArrow(id, transform.position);
    }
}
