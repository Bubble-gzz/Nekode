using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;

public class MyGrid : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject nekoPrefab;
    [SerializeField]
    public int n, m;
    [SerializeField]
    public float tileSize;
    [SerializeField]
    public GameObject mouseGhost;
    [SerializeField]
    GameObject tilePrefab;
    public GameObject[,] grid;
    public Dictionary<string, List<MyTile> > tileTable = new Dictionary<string, List<MyTile> >();
    Camera myCamera;
    [SerializeField]
    public List<Sprite> tileTextures = new List<Sprite>();
    [SerializeField]
    public List<Color> tileTextColors = new List<Color>();
    [SerializeField]
    public List<Sprite> blankTileTextures = new List<Sprite>();
    static public MyTile.Type currentTileType;
    int lasti, lastj, i, j;
    GameObject lastGhost;
    Neko neko;
    public List<int> tileCount = new List<int>();
    Rect border = new Rect();
    AudioSource sfx_place;
    public bool isTitleBackground = false;
    class Rect{
        public int l,u,r,d;
        public Rect(int _l, int _u, int _r, int _d)
        {
            l = _l; u = _u; r = _r; d = _d;
        }
        public Rect() {}
    }
    [SerializeField]
    int tileKindN;
    static public bool canAddLabelAsPlayer = false;
    void Awake()
    {
        canAddLabelAsPlayer = false;
        grid = new GameObject[n, m];
        currentTileType = MyTile.Type.NULL;
        lasti = lastj = -100;
        lastGhost = null;
        tileKindN = 25;
        Global.grid = this;
        GameMessage.OnResetGridState.AddListener(MapRecover);

        tileTable = new Dictionary<string, List<MyTile>>();
        for (int i = 0; i < tileKindN; i++)
            tileCount.Add(-1);
        sfx_place = GameObject.Find("sfx/place")?.GetComponent<AudioSource>();
        borderCalculated = false;
        cameraCalibrated = false;
    }
    bool borderCalculated, cameraCalibrated;
    void Start()
    {
        myCamera = Global.mainCam;
    }
    void CameraInitialize()
    {
        if (isTitleBackground) return;
        if (myCamera.GetComponent<MyCamera>().mode == MyCamera.Mode.WSAD)
        {
            Debug.Log("l:" + border.l + " r:" + border.r + " u:" + border.u + " d:" + border.d);
            Vector3 newPos = (GetWorldPos(border.d, border.l) + GetWorldPos(border.d, border.r) + 
                                GetWorldPos(border.u, border.l) + GetWorldPos(border.u, border.r)) / 4;
            newPos.z = -10;
            myCamera.transform.position = newPos;
        }
        cameraCalibrated = true;
    }
    public void SetTileCount(List<TilePresetPair> tileCountPreset)
    {
        for (int i = 0; i < tileKindN; i++)
            tileCount[i] = 0;
        foreach(var preset in tileCountPreset)
            tileCount[(int)preset.type] = preset.count;
    }

    public void Init()
    {
        grid[n/2, m/2] = NewTile(MyTile.Type.Blank, n/2, m/2);
        grid[n/2, m/2].transform.position = GetWorldPos(n/2, m/2);
        if (!isTitleBackground) InviteNeko(n/2, m/2);
        cameraCalibrated = false;
        UpdateRect();
    }

    public void InviteNeko(int i, int j, int dir = 0)
    {
        neko = Instantiate(nekoPrefab).GetComponent<Neko>();
        Vector3 nekoPos = GetWorldPos(i, j);
        nekoPos.z = neko.z_pos;
        neko.transform.position = nekoPos;
        neko.i = i; neko.j = j;
        neko.grid = this;
        neko.direction = dir;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GridPosOfMouse().ToString("f0"));
        //Debug.Log("current:" + currentTileType);
        Vector2 pos = GridPosOfMouse();
        i = (int)pos.x; j = (int)pos.y;
        CheckCancelCurrentTile();
        ClearGhost();
        CheckBlankTile();
        CheckPlaceTile();
        lasti = i; lastj = j;
        if (borderCalculated && !cameraCalibrated)
            CameraInitialize();
    }
    void CheckCancelCurrentTile()
    {
        if (Arrow.IsArrow(currentTileType)) return;
        if (Input.GetMouseButtonDown(1)) currentTileType = MyTile.Type.NULL;
    }
    public Vector2 GridPosOfMouse()
    {
        Vector2 mousePos = myCamera.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2(Mathf.Floor(mousePos.y / tileSize), Mathf.Floor(mousePos.x / tileSize));
    }
    public Sprite GetTileTexture(MyTile.Type type, bool randomFlag = true)
    {
        Sprite sprite;
        if (type == MyTile.Type.Blank)
        {
            int n = blankTileTextures.Count;
            if (randomFlag) sprite = blankTileTextures[Random.Range(0, n)];
            else sprite = blankTileTextures[0];
        }
        else sprite = tileTextures[(int)type];
        return sprite;
    }
    void ClearGhost()
    {
        if (i != lasti || j != lastj)
        {
            if (lastGhost != null) Destroy(lastGhost);
        }    
    }
    public GameObject NewTile(MyTile.Type type, int i, int j, int value = 0, MyTile.Permission permission = MyTile.Permission.Free)
    {
        MyTile newTile = Instantiate(tilePrefab, transform).GetComponent<MyTile>();
        newTile.myGrid = this;
        newTile.type = type;
        //SpriteRenderer sprite = newTile.transform.Find("Texture").GetComponent<SpriteRenderer>();
        //sprite.sprite = GetTileTexture(type);
        newTile.permission = permission;
        newTile.value = value;
        newTile.i = i;
        newTile.j = j;
        return newTile.gameObject;
    }
    public Vector2 GetWorldPos(int i, int j)
    {
        //Debug.Log("(" + i + ", " + j + ")");
        return new Vector2((j + 0.5f) * tileSize, (i + 0.5f) * tileSize);
    }
    void CheckBlankTile()
    {
        //Debug.Log(currentTileType);
        if (currentTileType != MyTile.Type.Blank) return;
        //Debug.Log("Place Blank Tile");
        if (Global.mouseOverUI) return;

        if (i < 0 || i >= n || j < 0 || j >= m) return;
        if (i == lasti  && j == lastj) return;
        if (grid[i, j] != null) return;


        GameObject newGhost = NewTile(MyTile.Type.Blank, i, j);
        newGhost.transform.position = GetWorldPos(i, j);
        newGhost.GetComponent<MyTile>().isGhost = true;

        lastGhost = newGhost;
 
    }
    void CheckPlaceTile()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (currentTileType == MyTile.Type.NULL) return;
        if (Arrow.IsArrow(currentTileType)) return;
        if (Global.mouseOverUI) return;
        if (i < 0 || i >= n || j < 0 || j >= m) return;
        
        if (currentTileType == MyTile.Type.Blank)
        {
            Destroy(lastGhost);
            if (grid[i, j] != null) return;
        }
        else 
        {
            if (grid[i, j] == null) return;
            if (grid[i, j].GetComponent<MyTile>().type != MyTile.Type.Blank) return;
        }
        
        MyTile lastTile = null;
        if (grid[i, j] != null) lastTile = grid[i, j].GetComponent<MyTile>();

        if (tileCount[(int)currentTileType] == 0) return;
        if (tileCount[(int)currentTileType] > 0) tileCount[(int)currentTileType]--;

        if (sfx_place)
        {
            sfx_place.volume = AudioManager.sfxVolume;
            sfx_place.Play();
        }
        grid[i, j] = NewTile(currentTileType, i, j);
        if (lastTile != null)
        {
            lastTile.CopyTo(grid[i, j].GetComponent<MyTile>());
            lastTile.Delete();
        }
        grid[i, j].transform.position = GetWorldPos(i, j);
        
        if (tileCount[(int)currentTileType] == 0) currentTileType = MyTile.Type.NULL;
    }
    public GridData ConvertToData()
    {
        GridData res = new GridData(n, m);
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                if (grid[i, j] != null)
                    res.tiles.Add(grid[i, j].GetComponent<MyTile>().ConvertToData());
        res.neko = neko.ConvertToData();
        return res;
    }
    public void ClearGrid()
    {
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
            {
                if (grid[i, j] != null) Destroy(grid[i, j]);
            }
        if (neko != null) Destroy(neko.gameObject);
    }
    
    public void LoadFromFile(string path, bool fromResource = true)
    {
        LoadFromFileWithReturnValue(path, fromResource);
    }

    public bool LoadFromFileWithReturnValue(string path, bool fromResource = true)
    {
        Debug.Log("loading from:" + path + "get: " + Resources.Load<TextAsset>(path));
        string jsonData = null;
        if (fromResource)
        {
            jsonData = Resources.Load<TextAsset>(path).text; //File.ReadAllText(path);
        }
        else
        {
            if (File.Exists(path))
                jsonData = File.ReadAllText(path);
        }
        if (jsonData == null) return false;
        LoadFromJson(jsonData);
        Global.mouseOverUI = false;
        return true;
    }
    void LoadFromJson(string jsonData)
    {
        ClearGrid();
        
        cameraCalibrated = false;
        borderCalculated = false;
        GridData gridData = JsonUtility.FromJson<GridData>(jsonData);
        n = gridData.n; m = gridData.m;
        grid = new GameObject[n, m];
        foreach (var tileData in gridData.tiles)
            BuildTileFromData(tileData);
        if (!isTitleBackground)
        {
            NekoData nekoData = gridData.neko;
            InviteNeko(nekoData.i, nekoData.j);
            neko.mode = (Neko.Mode)nekoData.mode;
            neko.value = nekoData.value;
            neko.direction = nekoData.direction;
        }
        UpdateRect();

    }
    void UpdateRect()
    {
        if (grid == null) return;
        border = new Rect(m, -1, -1, n);
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                if (grid[i, j] != null)
                {
                    if (j < border.l) border.l = j;
                    if (j > border.r) border.r =j;
                    if (i < border.d) border.d = i;
                    if (i > border.u) border.u = i;
                }
        borderCalculated = true;
    }
    void BuildTileFromData(TileData tileData)
    {
        int i = tileData.i, j = tileData.j;
        grid[i, j] = NewTile((MyTile.Type)tileData.type, i, j, tileData.value, (MyTile.Permission)tileData.permission);
        grid[i, j].GetComponent<MyTile>().label = tileData.label;
        grid[i, j].transform.position = GetWorldPos(i, j);
        foreach (var arrowData in tileData.arrows)
            grid[i, j].GetComponent<MyTile>().BuildArrowFromData(arrowData);
    }
    public void MapBackUp()
    {
        //Debug.Log("Backup");
        neko.Backup();
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                if (grid[i, j] != null)
                {
                    grid[i, j].GetComponent<MyTile>().Backup();
                }
    }
    public void MapRecover()
    {
        neko.Recover();
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                if (grid[i, j] != null)
                {
                    grid[i, j].GetComponent<MyTile>().Recover();
                    grid[i, j].GetComponent<MyTile>().RefreshAnimation();
                }       
    }
}
