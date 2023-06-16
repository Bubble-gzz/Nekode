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
    public int backupDirection;
    public int id;
    public MyTile tile;
    AnimationBuffer animationBuffer;
    public bool mouseEnter;
    Transform texture;
    Transform myCollider;
    SpriteRenderer sprite;
    bool logicState, lastLogicState;
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

        sprite = texture.GetComponent<SpriteRenderer>();
        sprite.sprite = arrowTextures[(int)type];
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position += new Vector3(0, 0, -0.1f);
        myCollider.rotation = Quaternion.Euler(0, 0, 90 * id);
        TurnTo(direction);

        if (tile != null) logicState = tile.logicState;
        else logicState = false;
        lastLogicState = !logicState; // force init update
    }

    // Update is called once per frame
    void Update()
    {
        if (isGhost) return;
        if (Global.gameState == Global.GameState.Editing)
        {
            if (Input.GetMouseButtonDown(0) && mouseEnter) Delete();
            if (Input.GetMouseButtonDown(1) && mouseEnter) Turn(1);
        }
        CheckState();
    }
    void CheckState()
    {
        if (tile != null) logicState = tile.logicState;
        else logicState = false;
        if (logicState == lastLogicState) return;
        SetState(logicState);
        lastLogicState = logicState;
    }
    void Turn(int delta)
    {
        TurnTo((direction + delta) % 4);
    }
    void TurnTo(int dir)
    {
        direction = dir;
        transform.rotation = Quaternion.Euler(0, 0, -90 * direction);        
    }
    public void ChangeState() {
        if (type == Type.Flip) Turn(2);
        if (type == Type.Normal) OnBumped();
    }
    void OnMouseEnter()
    {
        if (isGhost) return;
        if (Global.mouseOverUI) return;
        if (Global.gameState != Global.GameState.Editing) return;
        Global.mouseOverArrow = true;
        mouseEnter = true;
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopOut_TileText, 0.07f));
    }
    void OnMouseExit()
    {
        if (isGhost) return;
        if (Global.gameState != Global.GameState.Editing) return;
        Global.mouseOverArrow = false;
        mouseEnter = false;
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.PopBack, 0.07f));
    }
    public void OnBumped()
    {
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.Emphasize, 0.1f));
    }
    public void SetState(bool active)
    {
        if (active == false) sprite.color = new Color(1,1,1,0.5f);
        else sprite.color = new Color(1,1,1,1);
    }
    public void Delete()
    {
        if (Global.gameState != Global.GameState.Editing) return;
        //Debug.Log("arrow to tile: " + (int)ArrowToTileType(type));
        if (tile.myGrid.tileCount[(int)ArrowToTileType(type)] >= 0) tile.myGrid.tileCount[(int)ArrowToTileType(type)]++;
        tile.DeleteArrow(id);
        GameMessage.OnArrowIsDeleted.Invoke();
    }
    public ArrowData ConvertToData()
    {
        return new ArrowData((int)type, id, direction);
    }
    public void Backup()
    {
        backupDirection = direction;
    }
    public void Recover()
    {
        TurnTo(backupDirection);
    }
}
