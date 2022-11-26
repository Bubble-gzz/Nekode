using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileSlot : MonoBehaviour
{
    Vector2 originSize;
    public int id;
    public TileInventory inventory;
    TMP_Text count;
    [SerializeField]
    public MyTile.Type type;
    AnimationBuffer animationBuffer;
    public void OnPickUp()
    {
        //Debug.Log(type);
        if (MyGrid.currentTileType == type) MyGrid.currentTileType = MyTile.Type.NULL;
        else MyGrid.currentTileType = type;
        Debug.Log(MyGrid.currentTileType);
    }

    void Awake()
    {
        count = transform.Find("Count").GetComponent<TMP_Text>();
    }
    void Start()
    {
        originSize = transform.localScale;   
    }

    void Update()
    {
        count.text = inventory.counts[id].ToString();
    }

}
