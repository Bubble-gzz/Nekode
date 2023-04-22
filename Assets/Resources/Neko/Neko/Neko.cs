using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Neko : MonoBehaviour
{
    [SerializeField]
    public float z_pos;
    [SerializeField]
    public MyGrid grid;
    public int direction; // 0-right 1-down 2-left 3-up
    public int i, j;
    int[] dx = new int[4]{0,-1,0,1};
    int[] dy = new int[4]{1,0,-1,0};
    [SerializeField]
    Sprite[] sprites = new Sprite[4];
    public int value;
    public enum Mode{
        Read,
        Write,
        Protect
    }
    [SerializeField]
    public Mode mode;
    public bool atDestination;
    [SerializeField]
    List<Color> bubbleTextColors = new List<Color>();
    SpriteRenderer sprite;
    Animator bubble;
    TMP_Text bubbleText;
    public bool playMode;
    bool running;
    AnimationBuffer animationBuffer;
    void Awake()
    {
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        bubble = transform.Find("Bubble").GetComponent<Animator>();
        bubbleText = transform.Find("Bubble").GetComponentInChildren<TMP_Text>();
        playMode = false; running = false;
        Global.currentNeko = this;
        Global.nekoPlaySpeed = 1;
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        gameObject.AddComponent<UpdatePosAnimator>();
        mouseEnter = false;
        atDestination = false;
    }
    void Start()
    {
        bubble.transform.GetComponentInChildren<TMP_Text>().color = bubbleTextColors[(int)mode];
        if (Global.currentGameMode == Global.GameMode.Play)
        {
            if (GamePlay.puzzleSetting.bubble) bubble.gameObject.SetActive(true);
        }
        else bubble.gameObject.SetActive(true);
        UpdateValue(value);
        UpdateDirection(direction);
    }

    void Update()
    {
        bubble.SetInteger("state", (int)mode);
        bubbleText.color = bubbleTextColors[(int)mode];
        if (playMode)
        {
            if (!running)
                StartCoroutine(RunOneStep());
        }
        CheckPickUp();
        Cheat();
    }
    void Cheat()
    {
        if (Global.currentGameMode == Global.GameMode.Play) return;
        if (!mouseEnter) return;
        if (Input.GetKeyDown(KeyCode.T))
        {
            UpdateDirection((direction + 1) % 4);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            SwitchMode(Mode.Read);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchMode(Mode.Write);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            UpdateValue(value + 1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            UpdateValue(value - 1);
        }
    }
    IEnumerator RunOneStep()
    {
        running = true;
        CalcDirection();
        int _i = i + dx[direction], _j = j + dy[direction];
        //Debug.Log("(" + i + ", " + j + ") (" + _i +", " + _j + ")");
        if (_i < 0 || _j < 0 || _i >= grid.n || _j >=grid.m) {
            running = false;
            yield break;
        }
        //Debug.Log("Pass1");
        if (grid.grid[_i, _j] == null) {
            running = false;
            yield break;
        }
        Leave(i, j);
        //Debug.Log("Pass2");       
        Vector3 targetPos = grid.GetWorldPos(_i, _j);
        targetPos.z = z_pos;
        AnimationInfo moveAnimation = new UpdatePosAnimatorInfo(gameObject, targetPos, true, 0.2f/Global.nekoPlaySpeed, true);
        animationBuffer.Add(moveAnimation);
        while (!moveAnimation.completed) yield return null;
        i = _i; j = _j;
        Interact(i, j);
        yield return new WaitForSeconds(0.5f / Global.nekoPlaySpeed);
        running = false;
    }
    void CalcDirection()
    {
        MyTile tile = grid.grid[i, j].GetComponent<MyTile>();
        Arrow arrow = tile.arrows[direction];
        if (arrow == null) return;
        if (!tile.logicState) return;
        UpdateDirection(arrow.direction);
        arrow.ChangeState();
    }
    void UpdateDirection(int newDir)
    {
        direction = newDir;
        sprite.sprite = sprites[direction];
    }
    void Leave(int i, int j)
    {
        MyTile tile = grid.grid[i,j].GetComponent<MyTile>();
        if (tile.IsLogicType()) tile.logicState = false;
    }
    void Interact(int i, int j)
    {
        MyTile tile = grid.grid[i, j].GetComponent<MyTile>();
        MyTile.Type tileType = tile.type;
        atDestination = false;
        switch (tileType)
        {
            case (MyTile.Type.InputTile):
                tile.UpdateValue(value);
                break;
            case (MyTile.Type.OutputTile):
                UpdateValue(tile.value);
                break;
            case (MyTile.Type.ISwitch):
                SwitchMode(Mode.Read);
                break;
            case (MyTile.Type.OSwitch):
                SwitchMode(Mode.Write);
                break;
            case (MyTile.Type.RegisterTile):
                if (mode == Mode.Write)
                {
                    tile.UpdateValue(value);
                }
                else
                {
                    UpdateValue(tile.value);
                }
                break;
            case (MyTile.Type.ADD):
                if (mode == Mode.Write)
                {
                    tile.UpdateValue(value);
                }
                else UpdateValue(value + tile.value);
                break;
            case (MyTile.Type.SUB):
                if (mode == Mode.Write)
                {
                    tile.UpdateValue(value);
                }
                else UpdateValue(value - tile.value);
                break;
            case (MyTile.Type.MUL):
                if (mode == Mode.Write)
                {
                    tile.UpdateValue(value);
                }
                else UpdateValue(value * tile.value);
                break;
            case (MyTile.Type.DIV):
                if (mode == Mode.Write)
                {
                    tile.UpdateValue(value);
                }
                else
                if (tile.value != 0) UpdateValue(value / tile.value);
                break;
            case (MyTile.Type.MOD):
                if (mode == Mode.Write)
                {
                    tile.UpdateValue(value);
                }
                else if (tile.value != 0) UpdateValue(value % tile.value);
                break;
            case (MyTile.Type.EQU):
                if (mode == Mode.Write)
                {
                    tile.UpdateValue(value);
                    tile.SetLogitState(true);
                }
                else
                {
                    tile.SetLogitState(value == tile.value);
                }
                break;
            case (MyTile.Type.GEQ):
                if (mode == Mode.Write)
                {
                    tile.UpdateValue(value);
                    tile.SetLogitState(true);
                }
                else
                {
                    tile.SetLogitState(value >= tile.value);
                }
                break;
            case (MyTile.Type.GTR):
                if (mode == Mode.Write)
                {
                    tile.UpdateValue(value);
                    tile.SetLogitState(true);
                }
                else
                {
                    tile.SetLogitState(value > tile.value);
                }
                break;
            case (MyTile.Type.LEQ):
                if (mode == Mode.Write)
                {
                    tile.UpdateValue(value);
                    tile.SetLogitState(true);
                }
                else
                {
                    tile.SetLogitState(value <= tile.value);
                }
                break;
            case (MyTile.Type.LSS):
                if (mode == Mode.Write)
                {
                    tile.UpdateValue(value);
                    tile.SetLogitState(true);
                }
                else
                {
                    tile.SetLogitState(value < tile.value);
                }
                break;
            case (MyTile.Type.NEQ):
                if (mode == Mode.Write)
                {
                    tile.UpdateValue(value);
                    tile.SetLogitState(true);
                }
                else
                {
                    tile.SetLogitState(value != tile.value);
                }
                break;
            case (MyTile.Type.Destination):
                atDestination = true;
                break;
            default: break;
        }
    }
    void UpdateValue(int newValue, bool animated = true)
    {
        newValue = Mathf.Clamp(newValue, -32768, 32767);
        value = newValue;
        bubble.transform.GetComponentInChildren<TMP_Text>().text = value.ToString();
    }
    void SwitchMode(Mode newMode)
    {
        mode = newMode;
        bubble.SetInteger("state", (int)newMode);
    }
    bool mouseEnter, followMouse;
    void OnMouseEnter()
    {
        if (MyGrid.currentTileType != MyTile.Type.NULL) return;
        if (Global.mouseOverUI) return;
        if (Global.currentGameMode == Global.GameMode.Play) return;
        mouseEnter = true;
        sprite.color = new Color(1, 1, 1, 0.5f);
    }
    void OnMouseExit()
    {
        if (followMouse) return;
        mouseEnter = false;
        if (Global.currentGameMode == Global.GameMode.Play) return;
        sprite.color = new Color(1, 1, 1, 1);
    }
    void CheckPickUp()
    {
        if (!mouseEnter && !followMouse) return;
        if (followMouse) transform.position = Global.MoveToMouse(transform.position);
        if (Input.GetMouseButtonDown(0))
        {
            if (mouseEnter && !followMouse)
            {
                followMouse = true;
                return;
            }
            else
            {
                Vector2 pos = grid.GridPosOfMouse();
                int _i = (int)pos.x, _j = (int)pos.y;
                if (_i < 0 || _j < 0 || _i >= grid.n || _j >= grid.m) return;
                if (grid.grid[_i, _j] == null) return;
                i = _i; j = _j;
                transform.position = grid.GetWorldPos(i, j);
                followMouse = false;
                sprite.color = new Color(1, 1, 1, 1);
            }
        }
    }
    public NekoData ConvertToData()
    {
        return new NekoData(i, j, (int)mode, value, direction);
    }
    int backup_i, backup_j, backup_value, backup_direction;
    Mode backup_mode;
    public void Backup()
    {
        backup_i = i; backup_j = j;
        backup_value = value;
        backup_direction = direction;
        backup_mode = mode;
    }
    public void Recover()
    {
        i = backup_i; j = backup_j;
        transform.position = grid.GetWorldPos(i, j);
        UpdateDirection(backup_direction);
        UpdateValue(backup_value);
        SwitchMode(backup_mode);
    }
}
