using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TileInventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    GameObject tileSlotPrefab;
    [SerializeField]
    List<MyTile.Type> tileTypes = new List<MyTile.Type>();
    [SerializeField]
    public List<int> counts = new List<int>();
    [SerializeField]
    MyGrid myGrid;

    void Start()
    {
        int curID = 0;
        foreach (var tileType in tileTypes)
        {
            TileSlot tileSlot = Instantiate(tileSlotPrefab, transform).GetComponent<TileSlot>();
            tileSlot.GetComponent<Image>().sprite = myGrid.GetTileTexture(tileType);
            tileSlot.id = curID++;
            tileSlot.type = tileType;
            tileSlot.inventory = this;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Global.mouseOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Global.mouseOverUI = false;
    }
}
