using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public enum Type
    {
        Normal,
        Flip
    }
    [SerializeField]
    List<Sprite> arrowTextures = new List<Sprite>();
    public Type type;
    public bool isGhost;
    public int direction; // 0-right 1-down 2-left 3-up
    public int id;
    public MyTile tile;
    AnimationBuffer animationBuffer;
    public bool mouseEnter;
    Transform texture;
    Transform myCollider;
    static public bool IsArrow(MyTile.Type type)
    {
        return type == MyTile.Type.Arrow || type == MyTile.Type.FlipArrow;
    }
    static public Type TileToArrowType(MyTile.Type tileType)
    {
        switch(tileType)
        {
            case (MyTile.Type.Arrow): return Type.Normal;
            case (MyTile.Type.FlipArrow): return Type.Flip;
            default: return Type.Normal;
        }
    }
    static public MyTile.Type ArrowToTileType(Type type)
    {
        switch(type)
        {
            case (Type.Normal): return MyTile.Type.Arrow;
            case (Type.Flip): return MyTile.Type.FlipArrow;
            default: return MyTile.Type.Arrow;
        }
    }
    void Awake()
    {
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        gameObject.AddComponent<PopAnimator>();
        mouseEnter = false;
        myCollider = transform.Find("Collider");
        texture = transform.Find("Texture");
        //Debug.Log("texture:" + texture);
    }
    void Start()
    {
        if (isGhost) myCollider.GetComponent<Collider2D>().enabled = false;

        texture.GetComponent<SpriteRenderer>().sprite = arrowTextures[(int)type];
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position += new Vector3(0, 0, -0.1f);
        myCollider.rotation = Quaternion.Euler(0, 0, 90 * id);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGhost) return;
        if (Input.GetMouseButtonDown(0) && mouseEnter) Delete();
        if (Input.GetMouseButtonDown(1) && mouseEnter) Turn(1);
    }
    void Turn(int delta)
    {
        direction = (direction + delta) % 4;
        transform.rotation = Quaternion.Euler(0, 0, -90 * direction);
    }
    public void ChangeState() {
        if (type == Type.Flip) Turn(2);
    }
    void OnMouseEnter()
    {
        if (isGhost) return;
        if (Global.mouseOverUI) return;
        Global.mouseOverArrow = true;
        mouseEnter = true;
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopOut_TileText, 0.07f));
    }
    void OnMouseExit()
    {
        if (isGhost) return;
        Global.mouseOverArrow = false;
        mouseEnter = false;
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopBack, 0.07f));
    }
    void Delete()
    {
        //Debug.Log("arrow to tile: " + (int)ArrowToTileType(type));
        if (tile.myGrid.tileCount[(int)ArrowToTileType(type)] >= 0) tile.myGrid.tileCount[(int)ArrowToTileType(type)]++;
        tile.DeleteArrow(id);
    }
    public ArrowData ConvertToData()
    {
        return new ArrowData((int)type, id, direction);
    }
}
