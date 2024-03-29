using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

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
    bool running;
    AnimationBuffer animationBuffer;
    List<MoveAttempt> moveAttempts = new List<MoveAttempt>();
    AudioSource sfx_updateValue;
    class MoveAttempt{
        public bool valid = true;
    }
    void Awake()
    {
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        bubble = transform.Find("Bubble").GetComponent<Animator>();
        bubbleText = transform.Find("Bubble").GetComponentInChildren<TMP_Text>();
        running = false;
        Global.currentNeko = this;
        Global.nekoPlaySpeed = 1;
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        gameObject.AddComponent<UpdatePosAnimator>();
        mouseEnter = false;
        atDestination = false;
        GameMessage.OnResetGridState.AddListener(ClearStepCount);
        sfx_updateValue = transform.Find("sfx/updateValue")?.GetComponent<AudioSource>();
    }
    void Start()
    {
        bubble.transform.GetComponentInChildren<TMP_Text>().color = bubbleTextColors[(int)mode];
        bubble.gameObject.SetActive(true);
        UpdateValue(value);
        UpdateDirection(direction);
    }

    void Update()
    {
        bubble.SetInteger("state", (int)mode);
        bubbleText.color = bubbleTextColors[(int)mode];
        if (Global.gameState == Global.GameState.Playing)
        {
            if (!running)
                StartCoroutine(RunOneStep());
        }
        CheckPickUp();
        Cheat();
    }
    void Cheat()
    {
        if (Global.gameMode == Global.GameMode.Test) return;
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
        while (Global.gameMode == Global.GameMode.Test && Global.isGeneratingTestData)
            yield return null;
        if (grid.grid[i, j].GetComponent<MyTile>().type == MyTile.Type.Destination) {
            running = false;
            yield break;
        }
        CalcDirection();
        int _i = i + dx[direction], _j = j + dy[direction];
        //Debug.Log("(" + i + ", " + j + ") (" + _i +", " + _j + ")");
        if (_i < 0 || _j < 0 || _i >= grid.n || _j >=grid.m) {
            running = false;
            yield break;
        }
        //Debug.Log("Pass1");
        if (grid.grid[_i, _j] == null) {
            Debug.Log("stop at (" + i + ", " + j + ")");
            running = false;
            ResetButton.Hint();
            PlayButton.StopPlaying();
            yield break;
        }
        Leave(i, j);
        Global.stepCount += 1;
        //Debug.Log("Pass2");       
        Vector3 targetPos = grid.GetWorldPos(_i, _j);
        targetPos.z = z_pos;
        AnimationInfo moveAnimation = new UpdatePosAnimatorInfo(gameObject, targetPos, true, 0.2f/Global.nekoPlaySpeed, true);
        animationBuffer.Add(moveAnimation);
        MoveAttempt attempt = new MoveAttempt();
        moveAttempts.Add(attempt);
        while (!moveAnimation.completed) {
            if (!attempt.valid)
            {
                moveAnimation.completed = true;
                break;
            }
            yield return null;
        }
        if (!attempt.valid) {
            running = false;
            moveAttempts.Remove(attempt);
            yield break;
        }
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
    void UpdateTileValue(MyTile tile, int newValue)
    {
        if (sfx_updateValue && !grid.isTitleBackground)
        {
            sfx_updateValue.volume = AudioManager.sfxVolume;
            sfx_updateValue.Play();
        }
        tile.UpdateValueByNeko(newValue);
    }
    void Interact(int i, int j)
    {
        MyTile tile = grid.grid[i, j].GetComponent<MyTile>();
        MyTile.Type tileType = tile.type;
        if (MyTile.IsLogicType(tileType)) GameMessage.OnPassLogicTile.Invoke();
        atDestination = false;
        switch (tileType)
        {
            case (MyTile.Type.InputTile):
                UpdateTileValue(tile,value);
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
                    UpdateTileValue(tile, value); // tile.UpdateValue(value);
                }
                else
                {
                    UpdateValue(tile.value);
                }
                break;
            case (MyTile.Type.ADD):
                if (mode == Mode.Write)
                {
                    UpdateTileValue(tile, value); // tile.UpdateValue(value);
                }
                else UpdateValue(value + tile.value);
                break;
            case (MyTile.Type.SUB):
                if (mode == Mode.Write)
                {
                    UpdateTileValue(tile, value); //tile.UpdateValue(value);
                }
                else UpdateValue(value - tile.value);
                break;
            case (MyTile.Type.MUL):
                if (mode == Mode.Write)
                {
                    UpdateTileValue(tile, value); //tile.UpdateValue(value);
                }
                else UpdateValue(value * tile.value);
                break;
            case (MyTile.Type.DIV):
                if (mode == Mode.Write)
                {
                    UpdateTileValue(tile, value); //tile.UpdateValue(value);
                }
                else
                if (tile.value != 0) UpdateValue(value / tile.value);
                break;
            case (MyTile.Type.MOD):
                if (mode == Mode.Write)
                {
                    UpdateTileValue(tile, value);// tile.UpdateValue(value);
                }
                else if (tile.value != 0) UpdateValue(value % tile.value);
                break;
            case (MyTile.Type.EQU):
                if (mode == Mode.Write)
                {
                    UpdateTileValue(tile, value); //tile.UpdateValue(value);
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
                    UpdateTileValue(tile, value);// tile.UpdateValue(value);
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
                    UpdateTileValue(tile, value);// tile.UpdateValue(value);
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
                    UpdateTileValue(tile, value); //tile.UpdateValue(value);
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
                    UpdateTileValue(tile, value); // tile.UpdateValue(value);
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
                    UpdateTileValue(tile, value); // tile.UpdateValue(value);
                    tile.SetLogitState(true);
                }
                else
                {
                    tile.SetLogitState(value != tile.value);
                }
                break;
            case (MyTile.Type.Destination):
                atDestination = true;
                GamePlay.onNekoSubmit?.Invoke();
                break;
            default: break;
        }
    }

    void UpdateValue(int newValue, bool animated = true)
    {
        if (sfx_updateValue && !grid.isTitleBackground)
        {
            sfx_updateValue.volume = AudioManager.sfxVolume;
            sfx_updateValue.Play();
        }
        newValue = Mathf.Clamp(newValue, -32768, 32767);
        value = newValue;
        bubble.transform.GetComponentInChildren<TMP_Text>().text = value.ToString();
        if (animated) UpdateValueAnimation();
    }
    Sequence bubbleAnimSeq;
    void UpdateValueAnimation()
    {
        bubbleAnimSeq?.Kill();
        bubbleAnimSeq = DOTween.Sequence();
        bubbleAnimSeq.Append(bubble.transform.DOScale(new Vector3(0.7f,1.3f,1), 0.1f/Global.nekoPlaySpeed));
        bubbleAnimSeq.Append(bubble.transform.DOScale(new Vector3(1.2f,0.8f,1), 0.1f/Global.nekoPlaySpeed));
        bubbleAnimSeq.Append(bubble.transform.DOScale(new Vector3(0.9f,1.1f,1), 0.1f/Global.nekoPlaySpeed));
        bubbleAnimSeq.Append(bubble.transform.DOScale(new Vector3(1.05f,0.95f,1), 0.1f/Global.nekoPlaySpeed));
        bubbleAnimSeq.Append(bubble.transform.DOScale(new Vector3(1,1,1), 0.1f/Global.nekoPlaySpeed));
        /*
        bubbleAnimSeq.Append(bubble.transform.DOScale(new Vector3(0.8f,1.2f,1), 0.1f/Global.nekoPlaySpeed));
        bubbleAnimSeq.Append(bubble.transform.DOScale(new Vector3(1.1f,0.9f,1), 0.1f/Global.nekoPlaySpeed));
        bubbleAnimSeq.Append(bubble.transform.DOScale(new Vector3(0.95f,1.05f,1), 0.1f/Global.nekoPlaySpeed));
        bubbleAnimSeq.Append(bubble.transform.DOScale(new Vector3(1,1,1), 0.1f/Global.nekoPlaySpeed));
        */
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
        if (Global.gameMode == Global.GameMode.Test) return;
        mouseEnter = true;
        sprite.color = new Color(1, 1, 1, 0.5f);
    }
    void OnMouseExit()
    {
        if (followMouse) return;
        mouseEnter = false;
        if (Global.gameMode == Global.GameMode.Test) return;
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
        foreach(var attempt in moveAttempts) attempt.valid = false;
        i = backup_i; j = backup_j;
        transform.position = grid.GetWorldPos(i, j);
        UpdateDirection(backup_direction);
        UpdateValue(backup_value);
        SwitchMode(backup_mode);
    }
    public void ClearStepCount()
    {
        Global.stepCount = 0;
    }
}
