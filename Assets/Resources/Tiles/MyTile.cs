using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using DG.Tweening;

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
        FlipArrow, // 23
        Destination // 24
    }
    public enum ButtonType{
        Delete,
        Edit,
        Free,
        Protected,
        ReadOnly,
        Label,
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
    public int backupValue;
    bool hasValue;
    public string label = "";
    public string lastLabel;
    TMP_Text valueText;
    Vector2 valueTextOffset = new Vector2(0, 0);
    TMP_Text labelText;
    Vector2 labelTextOffset = new Vector2(-0.3f, 0.3f);
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
    List<Type> fixedTileTypes = new List<Type>() {
        Type.InputTile, Type.OutputTile, Type.Destination, Type.Blank
    };
    [SerializeField]
    List<Sprite> logicTileInactiveTexture = new List<Sprite>();
    Camera myCamera;
    bool deleted;
    public bool logicState = true, lastLogicState;
    public enum Permission{
        ReadOnly, // read
        Protected, // read | edit
        Free      // read | edit | delete
    }
    public Permission permission;
    public int i, j;
    Collider2D tileCollider;

    GameObject valueScaler;
    GameObject labelScaler;

    bool editingValueActively;

    bool editingValue;
    bool editingLabel;

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
        editingValueActively = false;

        editingLabel = false;
        deleted = false;
        Global.isTyping = false;
        
        gameObject.AddComponent<PopAnimator>();
        animationBuffer = gameObject.AddComponent<AnimationBuffer>();
        valueText = transform.Find("ValueScaler").gameObject.GetComponentInChildren<TMP_Text>();
        valueScaler = transform.Find("ValueScaler").gameObject;
        valueScaler.AddComponent<PopAnimator>();

        labelText = transform.Find("LabelScaler").gameObject.GetComponentInChildren<TMP_Text>();
        labelScaler = transform.Find("LabelScaler").gameObject;
        labelScaler.AddComponent<PopAnimator>();
        label = "";

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
            permission = Permission.Protected;
        }
        if (type == Type.Destination)
            permission = Permission.Protected;
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
        if (hasValue) valueText.enabled = true;
        else valueText.enabled = false;

        if (arithmeticTiles.Contains(type)) valueTextOffset = new Vector2(0, -0.15f);
        if (logicTiles.Contains(type)) {
            valueTextOffset = new Vector2(0.14f, -0.17f);
            logicState = false;
        }
        valueScaler.transform.position = transform.position + (Vector3)valueTextOffset;
        //Debug.Log("type:" + (int)type);
        valueText.color = myGrid.tileTextColors[(int)type];

        labelScaler.transform.position = transform.position + (Vector3)labelTextOffset;
        labelText.color = myGrid.tileTextColors[(int)type];

        if (label != "" && label != null) {
            if (!myGrid.tileTable.ContainsKey(label)) myGrid.tileTable.Add(label, new List<MyTile>());
            myGrid.tileTable[label].Add(this);
        }
        lastLabel = label;
        labelText.text = label;

        lastLogicState = !logicState; //force initial update
    }
    bool CheckHasValue()
    {
        switch (type)
        {
            case Type.ISwitch: return false;
            case Type.OSwitch: return false;
            case Type.Blank: return false;
            case Type.Destination: return false;
            default: return true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (permission == Permission.Free || fixedTileTypes.Contains(type)) sprite.color = new Color(1,1,1,1);
        else sprite.color = ReadOnlyColor;

        if (permission != Permission.ReadOnly) valueText.color = myGrid.tileTextColors[(int)type];
        else {
            Color newColor = myGrid.tileTextColors[(int)type];
            newColor.a = 0.4f;
            valueText.color = newColor;
        }
        if (Input.GetMouseButtonDown(1)) OnEdit();
        CheckExitEditArea();
        EditValue();
        EditLabel();
        if (Arrow.IsArrow(MyGrid.currentTileType)) tileCollider.enabled = false;
        else tileCollider.enabled = true;

        LogicStateCheck();
    }
    void LogicStateCheck()
    {
        if (lastLogicState == logicState) return;
        if (logicState)
        {
            sprite.sprite = myGrid.GetTileTexture(type);
        }
        else sprite.sprite = logicTileInactiveTexture[logicTiles.IndexOf(type)];
        lastLogicState = logicState;
    }
    void CheckExitEditArea()
    {
        if (!editing) return;
        Vector2 mousePos = myCamera.ScreenToWorldPoint(Input.mousePosition);
        float dist = (mousePos - (Vector2)transform.position).magnitude;
        if (dist < myGrid.tileSize * 0.7f && !Input.GetKeyDown(KeyCode.Return)) return;
        DropEditButtons();
        QuitEditValueActively();
        QuitEditLabel();
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
        if (Global.gameState != Global.GameState.Editing) return;

        editing = true;
        buttons.Clear();

        bool isWorkshop = Global.inWorkshop;
        bool deletable, editable = hasValue, canBeFree, canBeProtected, canBeReadOnly, hasLabel;
        
        canBeFree = permission != Permission.Free && isWorkshop;
        canBeProtected = permission != Permission.Protected && isWorkshop;
        canBeReadOnly = permission != Permission.ReadOnly && isWorkshop && hasValue;
        hasLabel = Global.inWorkshop;

        if (isWorkshop)
        {
            deletable = true;
            if (type == Type.Destination || type == Type.Blank) canBeFree = false;
        }
        else
        {
            deletable = (permission == Permission.Free);
            editable &= !(permission == Permission.ReadOnly);
        }

        if (deletable) AddButton(Delete, (int)ButtonType.Delete);
        if (editable && hasValue) {
            if (!(fixedTileTypes.Contains(type) && Global.gameMode == Global.GameMode.Test))
            {
                AddButton(StartEditValueActively, (int)ButtonType.Edit);
            }
        }
        if (canBeFree) AddButton(SetFree, (int)ButtonType.Free);
        if (canBeProtected) AddButton(SetProtected, (int)ButtonType.Protected);
        if (canBeReadOnly) AddButton(SetReadOnly, (int)ButtonType.ReadOnly);
        if (hasLabel) AddButton(StartEditLabel, (int)ButtonType.Label);

        int n = buttons.Count;
        if (n == 0) {
            editing = false;
            return;
        }
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
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.LinearBack, 0.07f));
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
            CopyTo(blankTile.GetComponent<MyTile>());
        }
        if (myGrid.tileCount[(int)type] >= 0) myGrid.tileCount[(int)type]++;
        Destroy(gameObject);
    }
    void StartEditValueActively()
    {
        if (editingValueActively) return;
        QuitEditLabel();
        editingValueActively = true;
        Global.isTyping = true;
        if (label != "")
        {
            foreach(MyTile tile in myGrid.tileTable[label])
                tile.StartEditValue();
        }
        else StartEditValue();
    }
    public void StartEditValue()
    {
        editingValue = true;
        animationBuffer.Add(new PopAnimatorInfo(valueScaler, PopAnimator.Type.PopOut_TileText, 0.07f));
    }
    void QuitEditValueActively()
    {
        if (!editingValueActively) return;
        editingValueActively = false;
        Global.isTyping = false;
        if (label != "")
        {
            foreach(MyTile tile in myGrid.tileTable[label])
                tile.QuitEditValue();
        }
        else QuitEditValue();
    }
    public void QuitEditValue()
    {
        editingValue = false;
        animationBuffer.Add(new PopAnimatorInfo(valueScaler, PopAnimator.Type.PopBack, 0.07f));
    }
    void StartEditLabel()
    {
        if (editingLabel) return;
        lastLabel = label;
        QuitEditValueActively();
        editingLabel = true;
        Global.isTyping = true;
        if (label == null) label = "";
        if (label == "") label = "*";
        animationBuffer.Add(new PopAnimatorInfo(labelScaler, PopAnimator.Type.PopOut_TileText, 0.07f));
    }
    void QuitEditLabel()
    {
        if (!editingLabel) return;
        if (label != lastLabel)
        {
            if (lastLabel != null) {
                if (myGrid.tileTable.ContainsKey(lastLabel))
                    myGrid.tileTable[lastLabel].Remove(this);
            }
            if (label == "*") label = "";
            if (label != "") {
                if (!myGrid.tileTable.ContainsKey(label)) myGrid.tileTable.Add(label, new List<MyTile>());
                if (myGrid.tileTable[label].Count > 0) value = myGrid.tileTable[label][0].value; // forcing value consistency
                myGrid.tileTable[label].Add(this);
            }
        }
        editingLabel = false;
        Global.isTyping = false;
        animationBuffer.Add(new PopAnimatorInfo(labelScaler, PopAnimator.Type.PopBack, 0.07f));
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
    Dictionary<KeyCode, char> charKeys = new Dictionary<KeyCode, char>{
        {KeyCode.A, 'A'},
        {KeyCode.B, 'B'},
        {KeyCode.C, 'C'},
        {KeyCode.D, 'D'},
        {KeyCode.E, 'E'},
        {KeyCode.F, 'F'},
        {KeyCode.G, 'G'},
        {KeyCode.H, 'H'},
        {KeyCode.I, 'I'},
        {KeyCode.J, 'J'},
        {KeyCode.K, 'K'},
        {KeyCode.L, 'L'},
        {KeyCode.M, 'M'},
        {KeyCode.N, 'N'},
        {KeyCode.O, 'O'},
        {KeyCode.P, 'P'},
        {KeyCode.Q, 'Q'},
        {KeyCode.R, 'R'},
        {KeyCode.S, 'S'},
        {KeyCode.T, 'T'},
        {KeyCode.U, 'U'},
        {KeyCode.V, 'V'},
        {KeyCode.W, 'W'},
        {KeyCode.X, 'X'},
        {KeyCode.Y, 'Y'},
        {KeyCode.Z, 'Z'}
    };
    void EditValue()
    {
        if (value < -32767 || value >= 32767) valueText.text = "?";
        else valueText.text = value.ToString();
        if (Mathf.Abs(value) < 100) valueText.fontSize = 0.35f;
        else if (Mathf.Abs(value) < 1000) valueText.fontSize = 0.3f;
        else valueText.fontSize = 0.25f;
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
    void EditLabel()
    {
        labelText.text = label;
        if (!editingLabel) return;
        if (label.Length == 1) labelText.fontSize = 0.22f;
        else if (label.Length == 2) labelText.fontSize = 0.2f;
        else if (label.Length == 3) labelText.fontSize = 0.18f;
        else if (label.Length == 4) labelText.fontSize = 0.15f;
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
            label += x.ToString();
        foreach(var keyCode in charKeys.Keys)
            if (Input.GetKeyDown(keyCode))
            {
                if (label.Length == 1 && label[0] == '*') label = "";
                label += charKeys[keyCode];
                break;
            }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (label.Length > 0) label = label.Substring(0, label.Length - 1);
            if (label.Length == 0) label = "*";
        }
    }
    void OnMouseEnter()
    {
        mouseEnter = true;
        if (editing) return;
        
        if (permission == Permission.Free || (type == Type.Blank || type == Type.Destination)) animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.LinearOut, 0.07f));
        else if (permission == Permission.Protected && hasValue) animationBuffer.Add(new PopAnimatorInfo(valueScaler.gameObject, PopAnimator.Type.LinearOut, 0.07f));
        //transform.localScale = originScale * 1.2f;
    }
    
    void OnMouseExit()
    {
        mouseEnter = false;
        if (editing) return;
        if (permission == Permission.Free || (type == Type.Blank || type == Type.Destination)) animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.LinearBack, 0.07f));
        else if (permission == Permission.Protected && hasValue) animationBuffer.Add(new PopAnimatorInfo(valueScaler.gameObject, PopAnimator.Type.LinearBack, 0.07f));
       
        //transform.localScale = originScale;
    }
    public void PlaceArrow(int id, Arrow.Type type, int direction = 0)
    {
        GameObject newArrow = Instantiate(arrowPrefab, transform);
        //Debug.Log("arrowPos: " + arrowPos[id]);
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
        if (label != "")
            foreach(var tile in myGrid.tileTable[label])
                tile.value = newValue;
        else value = newValue;
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
    public void CopyTo(MyTile other)
    {
        for (int i = 0; i < 4; i++) // copy arrows
        {
            if (other.arrows[i] != null)
                other.arrows[i].Delete();
            if (arrows[i] != null)
                other.PlaceArrow(i, arrows[i].type, arrows[i].direction);
        }
    }
    public TileData ConvertToData()
    {
        Permission _permission = permission;
        Type _type = type;
        if (Global.inWorkshop)
        {
            if (permission == Permission.Free)
            {
                _permission = Permission.Protected;
            }
        }

        TileData res = new TileData(i, j, (int)_type, (int)_permission, label, value);
        for (int i = 0; i < 4; i++)
            if (arrows[i] != null)
                res.arrows.Add(arrows[i].ConvertToData());
        return res;
    }
    public void BuildArrowFromData(ArrowData arrowData)
    {
        PlaceArrow(arrowData.side, (Arrow.Type)arrowData.type, arrowData.direction);
    }
    public void Backup()
    {
        backupValue = value;
        for (int i = 0; i < 4; i++)
            if (arrows[i] != null) arrows[i].Backup();
    }
    public void Recover()
    {
        UpdateValue(backupValue);
        for (int i = 0; i < 4; i++)
            if (arrows[i] != null) arrows[i].Recover();   
    }
    public bool IsLogicType() {
        return logicTiles.Contains(type);
    }
    public bool IsArithmeticType() {
        return arithmeticTiles.Contains(type);
    }

    public GameObject CorrectIconPrefab, WrongIconPrefab;
    Transform resultOfTest;
    public IEnumerator ResultCorrect()
    {
        bool animationComplete = false;
        GameObject correct = Instantiate(CorrectIconPrefab, transform);
        var sequence = DOTween.Sequence();
        sequence.Append( correct.transform.DOScale(Vector3.one * 1.3f, 0.3f) );
        sequence.Append( correct.transform.DOScale(Vector3.one * 0.9f, 0.1f) );
        sequence.Append( correct.transform.DOScale(Vector3.one * 1f, 0.1f) );
        sequence.OnComplete(()=>{
            animationComplete = true;
        });
        correct.transform.localScale = new Vector3(0, 0, 0);
        resultOfTest = correct.transform;
        while (!animationComplete) yield return null;
    }
    public IEnumerator ResultWrong()
    {
        bool animationComplete = false;
        GameObject wrong = Instantiate(WrongIconPrefab, transform);
        var sequence = DOTween.Sequence();
        sequence.Append( wrong.transform.DOScale(Vector3.one * 1.3f, 0.3f) );
        sequence.Append( wrong.transform.DOScale(Vector3.one * 0.9f, 0.1f) );
        sequence.Append( wrong.transform.DOScale(Vector3.one * 1f, 0.1f) );
        sequence.OnComplete(()=>{
            animationComplete = true;
        });
        wrong.transform.localScale = new Vector3(0, 0, 0);
        resultOfTest = wrong.transform;
        while (!animationComplete) yield return null;
    }
    public IEnumerator ClearResultOfTest()
    {
        bool animationComplete = false;
        var sequence = DOTween.Sequence();
        sequence.Append( resultOfTest.DOScale(Vector3.one * 1.2f, 0.1f) );
        sequence.Append( resultOfTest.transform.DOScale(Vector3.one * 0f, 0.3f) );
        sequence.OnComplete(()=>{
            animationComplete = true;
            Destroy(resultOfTest.gameObject);
        });
        while (!animationComplete) yield return null;        
    }
}
