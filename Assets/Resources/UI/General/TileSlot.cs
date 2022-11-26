using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TileSlot : MonoBehaviour
{
    Vector2 originSize;
    public int id;
    public TileInventory inventory;
    TMP_Text count;
    [SerializeField]
    public MyTile.Type type;
    AnimationBuffer animationBuffer;
    Image sprite;
    int lastCount;
    public void OnPickUp()
    {
        //Debug.Log(type);
        if (type == MyTile.Type.ArithmeticMenu)
        {
            inventory.SwitchTo(inventory.arithInventory);
            MyGrid.currentTileType = MyTile.Type.NULL;
            return ;
        }
        if (type == MyTile.Type.LogicMenu)
        {
            inventory.SwitchTo(inventory.logicInventory);
            MyGrid.currentTileType = MyTile.Type.NULL;
            return ;
        }
        if (type == MyTile.Type.ArithmeticBack || type == MyTile.Type.LogicBack)
        {
            inventory.SwitchTo(inventory.mainInventory);
            MyGrid.currentTileType = MyTile.Type.NULL;
            return ;
        }
        if (inventory.myGrid.tileCount[(int)type] == 0) return;
        if (MyGrid.currentTileType == type) MyGrid.currentTileType = MyTile.Type.NULL;
        else MyGrid.currentTileType = type;
        Debug.Log(MyGrid.currentTileType);
    }

    void Awake()
    {
        count = transform.Find("Count").GetComponent<TMP_Text>();
        sprite = GetComponent<Image>();
    }
    void Start()
    {
        sprite.enabled = false;
        count.enabled = false;
        animationBuffer = GetComponent<AnimationBuffer>();
        originSize = transform.localScale;  
        StartCoroutine(Appear());
    }
    IEnumerator Appear()
    {
        yield return new WaitForSeconds(Random.Range(0, 0.2f));
        sprite.enabled = true;
        count.enabled = true;
        animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.Appear, 0.1f));
    }

    void Update()
    {
        if (lastCount != inventory.myGrid.tileCount[(int)type])
        {
            animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.Emphasize, 0.1f));
        }
        if (!MyTile.NotTile(type) && inventory.myGrid.tileCount[(int)type] == 0)
        {
            sprite.color = new Color(1,1,1,0.5f);
            count.color = new Color(0,0,0,0.5f);
        }
        else {
            sprite.color = new Color(1,1,1,1);
            count.color = new Color(0,0,0,1);  
        }
        if (MyTile.NotTile(type) || inventory.myGrid.tileCount[(int)type] < 0) count.text = "";
        else count.text = inventory.myGrid.tileCount[(int)type].ToString();
        lastCount = inventory.myGrid.tileCount[(int)type];
    }
}
