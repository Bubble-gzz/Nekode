using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class MyTile : MonoBehaviour
{
    public enum Type{
        NULL, // 0
        InputTile, // 1
        OutputTile, // 2
        ISwitch, // 3
        OSwitch, // 4
        RegisterTile, // 5
        ADD, // 6
        SUB, // 7
        MUL, // 8
        DIV, // 9
        MOD, // 10
        ArithmeticMenu, // 11
        ArithmeticBack, // 12
        EQU, // 13
        GEQ, // 14
        GTR, // 15
        LEQ, // 16
        LSS, // 17
        NEQ, // 18
        LogicMenu, // 19
        LogicBack, // 20
        Blank, // 21
        Arrow, //22
        FlipArrow // 23
    }
    public enum ButtonType{
        Delete,
        Edit,
        Free,
        Protected,
        ReadOnly
    }
    [SerializeField]
    GameObject buttonPrefab;
    [SerializeField]
    Color ReadOnlyColor;
    [SerializeField]
    List<Sprite> buttonTextures = new List<Sprite>();
    [SerializeField]
    public Type type;
    public bool isGhost;
    public int value;
    bool hasValue;
    TMP_Text text;
    Vector2 textOffset = new Vector2(0, 0);
    SpriteRenderer sprite;
    public MyGrid myGrid;
    bool mouseEnter;
    bool editing;
    Vector3 originScale;
    AnimationBuffer animationBuffer;
    List<EditTileButton> buttons = new List<EditTileButton>();
    List<Type> arithmeticTiles = new List<Type>() {
        Type.ADD, Type.SUB, Type.MUL, Type.DIV, Type.MOD
    };
    List<Type> logicTiles = new List<Type>() {
        Type.EQU, Type.GEQ, Type.LEQ, Type.LSS, Type.GTR, Type.NEQ
    };
    Camera myCamera;
    bool deleted;
    public bool logicState = true;
    public enum Permission{
        ReadOnly, // read
        Protected, // read | edit
        Free      // read | edit | delete
    }
    public Permission permission;
    public int i, j;
    Collider2D tileCollider;

    GameObject valueScaler;
    bool editingValue;

    public Arrow[] arrows = new Arrow[4];
    public Vector3[] arrowPos;
    [SerializeField]
    public ArrowDetectArea[] arrowDetectAreas = new ArrowDetectArea[4];
    [SerializeField]
    GameObject arrowPrefab;

    void Awake()
    {
        sprite = transform.Find("Texture").GetComponent<SpriteRenderer>();
        mouseEnter = false;
        editing = false;
        editingValue = false;
        deleted = false;
        gameObject.AddComponent<PopAnimator>();
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        text = gameObject.GetComponentInChildren<TMP_Text>();
        valueScaler = transform.Find("ValueScaler").gameObject;
        valueScaler.AddComponent<PopAnimator>();
        tileCollider = GetComponent<Collider2D>();
        arrowPos =  new Vector3[4]{
            new Vector3(0.45f, 0, 0),
            new Vector3(0, -0.45f, 0),
            new Vector3(-0.45f, 0, 0),
            new Vector3(0, 0.45f, 0)
        };
    }
    void Start()
    {
        if (type == Type.Blank)
        {
            transform.Find("Texture").rotation = Quaternion.Euler(0, 0, 90 * Random.Range(0, 3));
        }
        if (isGhost)
        {
            Color newColor = sprite.color;
            newColor.a = 0.5f;
            sprite.color = newColor;
        }
        for (int i = 0; i < 4; i++)
            arrowDetectAreas[i].tile = this;
        myCamera = Global.mainCam;
        
        originScale = transform.localScale;
        sprite.sprite = myGrid.GetTileTexture(type);
    
        hasValue = CheckHasValue();
        if (hasValue) text.enabled = true;
        else text.enabled = false;

        if (arithmeticTiles.Contains(type)) textOffset = new Vector2(0, -0.15f);
        if (logicTiles.Contains(type)) {
            textOffset = new Vector2(0.14f, -0.17f);
            logicState = false;
        }
        valueScaler.transform.position = transform.position + (Vector3)textOffset;
        Debug.Log("type:" + (int)type);
        text.color = myGrid.tileTextColors[(int)type];
    }
    bool CheckHasValue()
    {
        switch (type)
        {
            case Type.ISwitch: return false;
            case Type.OSwitch: return false;
            case Type.Blank: return false;
            default: return true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (permission == Permission.Free) sprite.color = new Color(1,1,1,1);
        else sprite.color = ReadOnlyColor;

        if (permission != Permission.ReadOnly) text.color = myGrid.tileTextColors[(int)type];
        else {
            Color newColor = myGrid.tileTextColors[(int)type];
            newColor.a = 0.4f;
            text.color = newColor;
        }
        if (Input.GetMouseButtonDown(1)) OnEdit();
        CheckExitEditArea();
        EditValue();
        if (Arrow.IsArrow(MyGrid.currentTileType)) tileCollider.enabled = false;
        else tileCollider.enabled = true;
    }
    void CheckExitEditArea()
    {
        if (!editing) return;
        Vector2 mousePos = myCamera.ScreenToWorldPoint(Input.mousePosition);
        float dist = (mousePos - (Vector2)transform.position).magnitude;
        if (dist < myGrid.tileSize * 0.7f && !Input.GetKeyDown(KeyCode.Return)) return;
        DropEditButtons();
        QuitEditValue();
    }

    public void DropEditButtons()
    {
        editing = false;
        foreach(var button in buttons)
            if (button != null) button.Disappear(transform.position);
        buttons.Clear();
    }

    void OnEdit()
    {
        if (Global.mouseOverUI || Global.mouseOverArrow) return;
        if (!mouseEnter || editing || isGhost) return;
        editing = true;
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.LinearBack, 0.07f));
        buttons.Clear();

        bool deletable, editable;
        bool isWorkshop = Global.currentGameMode == Global.GameMode.Workshop;
        if (isWorkshop)
        {
            deletable = true;
            editable = true;
        }
        else
        {
            deletable = permission == Permission.Free;
            editable = !(permission == Permission.ReadOnly);
        }

        if (deletable) AddButton(Delete, (int)ButtonType.Delete);
        if (editable && hasValue) AddButton(StartEditValue, (int)ButtonType.Edit);
        if (permission != Permission.Free && isWorkshop) AddButton(SetFree, (int)ButtonType.Free);
        if (permission != Permission.Protected && isWorkshop) AddButton(SetProtected, (int)ButtonType.Protected);
        if (permission != Permission.ReadOnly && isWorkshop && hasValue) AddButton(SetReadOnly, (int)ButtonType.ReadOnly);
        

        int n = buttons.Count;
        if (n == 0) return;
        float size = myGrid.tileSize;
        float interval = size / (n + 1);
        for (int i = 0; i < n; i++)
        {
            Vector3 targetPos = transform.position + new Vector3(-size/2 + (i + 1) * interval, size/2*0.95f, -2f);
            buttons[i].Appear(transform.position, targetPos);
            /*
            buttons[i].transform.rotation = Quaternion.Euler(0, 0, 0);
            buttons[i].transform.position = transform.position;
            buttons[i].animationBuffer.Add(new UpdatePosAnimatorInfo(buttons[i].gameObject, targetPos));
            buttons[i].animationBuffer.Add(new PopAnimatorInfo(buttons[i].gameObject, PopAnimator.Type.Appear));
            */
        }
    }
    void AddButton(UnityAction buttonEvent, int id)
    {
        EditTileButton newButton = Instantiate(buttonPrefab).GetComponent<EditTileButton>();
        SpriteRenderer sprite = newButton.GetComponent<SpriteRenderer>();
        sprite.sprite = buttonTextures[id];
        newButton.onClick.AddListener(buttonEvent);
        buttons.Add(newButton);        
    }
    void SetFree() { permission = Permission.Free; }
    void SetProtected() { permission = Permission.Protected; }
    void SetReadOnly() { permission = Permission.ReadOnly; }
    public void Delete()
    {
        if (deleted) return;
        deleted = true;
        //Debug.Log("delete");
        foreach(var button in buttons)
            if (button != null) Destroy(button.gameObject);
        if (type != Type.Blank)
        {
            GameObject blankTile = myGrid.NewTile(Type.Blank, i, j);
            myGrid.grid[i, j] = blankTile;
            blankTile.transform.position = myGrid.GetWorldPos(i, j);
        }
        if (myGrid.tileCount[(int)type] >= 0) myGrid.tileCount[(int)type]++;
        Destroy(gameObject);
    }
    void StartEditValue()
    {
        if (editingValue) return;
        editingValue = true;
        animationBuffer.Add(new PopAnimatorInfo(valueScaler, PopAnimator.Type.PopOut_TileText, 0.07f));
    }
    void QuitEditValue()
    {
        if (!editingValue) return;
        editingValue = false;
        animationBuffer.Add(new PopAnimatorInfo(valueScaler, PopAnimator.Type.PopBack, 0.07f));
    }
    Dictionary<KeyCode, int> alphaKeys = new Dictionary<KeyCode, int>(){
        {KeyCode.Alpha0, 0},
        {KeyCode.Alpha1, 1},
        {KeyCode.Alpha2, 2},
        {KeyCode.Alpha3, 3},
        {KeyCode.Alpha4, 4},
        {KeyCode.Alpha5, 5},
        {KeyCode.Alpha6, 6},
        {KeyCode.Alpha7, 7},
        {KeyCode.Alpha8, 8},
        {KeyCode.Alpha9, 9}
    };
    Dictionary<KeyCode, int> keypadKeys = new Dictionary<KeyCode, int>(){
        {KeyCode.Keypad0, 0},
        {KeyCode.Keypad1, 1},
        {KeyCode.Keypad2, 2},
        {KeyCode.Keypad3, 3},
        {KeyCode.Keypad4, 4},
        {KeyCode.Keypad5, 5},
        {KeyCode.Keypad6, 6},
        {KeyCode.Keypad7, 7},
        {KeyCode.Keypad8, 8},
        {KeyCode.Keypad9, 9}
    };
    void EditValue()
    {
        text.text = value.ToString();
        if (Mathf.Abs(value) < 100) text.fontSize = 0.35f;
        else if (Mathf.Abs(value) < 1000) text.fontSize = 0.3f;
        else text.fontSize = 0.25f;
        if (!editingValue) return;
        int x = -1;
        foreach(var keyCode in alphaKeys.Keys)
            if (Input.GetKeyDown(keyCode))
            {
                x = alphaKeys[keyCode];
                break;
            }
        foreach(var keyCode in keypadKeys.Keys)
            if (Input.GetKeyDown(keyCode))
            {
                x = keypadKeys[keyCode];
                break;
            }
        if (x != -1)
        {
            value = value * 10 + (int)Mathf.Sign(value) * x;
            value = Mathf.Clamp(value, -32768, 32767);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            value *= -1;
            value = Mathf.Clamp(value, -32768, 32767);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            value = value / 10;
        }
    }
    void OnMouseEnter()
    {
        mouseEnter = true;
        if (editing) return;
        
        if (permission == Permission.Free) animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.LinearOut, 0.07f));
        else if (permission == Permission.Protected && hasValue) animationBuffer.Add(new PopAnimatorInfo(valueScaler.gameObject, PopAnimator.Type.LinearOut, 0.07f));
        //transform.localScale = originScale * 1.2f;
    }
    
    void OnMouseExit()
    {
        mouseEnter = false;
        if (editing) return;
        if (permission == Permission.Free) animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.LinearBack, 0.07f));
        else if (permission == Permission.Protected && hasValue) animationBuffer.Add(new PopAnimatorInfo(valueScaler.gameObject, PopAnimator.Type.LinearBack, 0.07f));
       
        //transform.localScale = originScale;
    }
    public void PlaceArrow(int id, Arrow.Type type, int direction = 0)
    {
        GameObject newArrow = Instantiate(arrowPrefab, transform);
        Debug.Log("arrowPos: " + arrowPos[id]);
        newArrow.transform.localPosition = arrowPos[id];
        arrows[id] = newArrow.GetComponent<Arrow>();
        arrows[id].type = type;
        arrows[id].id = id;
        arrows[id].tile = this;
        arrows[id].direction = direction;
        arrowDetectAreas[id].enabled = false;
    }
    public void DeleteArrow(int id)
    {
        Destroy(arrows[id].gameObject);
        arrowDetectAreas[id].enabled = true;
    }
    public void UpdateValue(int newValue, bool animated = true)
    {
        value = newValue;
    }
    public void SetLogitState(bool newState)
    {
        logicState = newState;
    }
    static public bool NotTile(Type type)
    {
        if (type == Type.ArithmeticBack) return true;
        if (type == Type.ArithmeticMenu) return true;
        if (type == Type.LogicMenu) return true;
        if (type == Type.LogicBack) return true;
        return false;
    }
    public TileData ConvertToData()
    {
        TileData res = new TileData(i, j, (int)type, (int)permission, value);
        for (int i = 0; i < 4; i++)
            if (arrows[i] != null)
                res.arrows.Add(arrows[i].ConvertToData());
        return res;
    }
    public void BuildArrowFromData(ArrowData arrowData)
    {
        PlaceArrow(arrowData.side, (Arrow.Type)arrowData.type, arrowData.direction);
    }
}
