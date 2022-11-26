using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TileInventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    public GameObject mainInventory, arithInventory, logicInventory;
    [SerializeField]
    Vector2 slotSize;
    [SerializeField]
    GameObject tileSlotPrefab;
    [SerializeField]
    List<MyTile.Type> tileTypes = new List<MyTile.Type>();
    [SerializeField]
    public MyGrid myGrid;
    RectTransform rect;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    void Start()
    {

    }
    void OnEnable()
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
    void Update()
    {
        rect.sizeDelta = new Vector2(slotSize.x * tileTypes.Count, slotSize.y);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Global.mouseOverUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Global.mouseOverUI = false;
    }
    public void Hide()
    {
        foreach(Transform child in transform)
            Destroy(child.gameObject);
        gameObject.SetActive(false);
    }
    public void SwitchTo(GameObject otherInventory)
    {
        otherInventory.SetActive(true);
        Hide();
    }
}
