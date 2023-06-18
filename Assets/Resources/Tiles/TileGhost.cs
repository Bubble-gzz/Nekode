using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGhost : MonoBehaviour
{
    Camera myCamera;
    public SpriteRenderer sprite;
    [SerializeField]
    MyGrid myGrid;
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        myCamera = Global.mainCam;
    }

    // Update is called once per frame
    MyTile.Type lastType = MyTile.Type.NULL;
    void Update()
    {

        transform.position = (Vector2)myCamera.ScreenToWorldPoint(Input.mousePosition);
        if (MyGrid.currentTileType != lastType)
        {
            if (MyGrid.currentTileType == MyTile.Type.NULL) CursorManager.Show();
            else CursorManager.Hide();
            lastType = MyGrid.currentTileType;
        }
        if (MyGrid.currentTileType == MyTile.Type.NULL) sprite.enabled = false;
        else {
            if (Arrow.IsArrow(MyGrid.currentTileType))
            {
                if (Global.mouseOverArrow) Hide();
                else Show();
            }
            else Show();
            sprite.sprite = myGrid.GetTileTexture(MyGrid.currentTileType, false);
            Color newColor = sprite.color;
            newColor.a = 0.5f;
            sprite.color = newColor;
        }
    }
    void Hide()
    {
        sprite.enabled = false;
    }
    void Show()
    {
        sprite.enabled = true;
    }
}
