using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class TileSlot : MonoBehaviour
{
    Vector2 originSize;
    public int id;
    public TileInventory inventory;
    TMP_Text count;
    [SerializeField]
    public MyTile.Type type;
    //AnimationBuffer animationBuffer;
    [SerializeField]
    GameObject messageBubblePrefab;
    Image sprite;
    int lastCount;
    Sequence animationSequence;

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
        if (MyGrid.currentTileType == type) {
            MyGrid.currentTileType = MyTile.Type.NULL;
            GameMessage.OnToolReturnedToSlot.Invoke();
        }
        else MyGrid.currentTileType = type;
        //Debug.Log(MyGrid.currentTileType);
    }

    void Awake()
    {
        count = transform.Find("Count").GetComponent<TMP_Text>();
        sprite = GetComponentInChildren<Image>();
        sprite.enabled = false;
        count.enabled = false;
        originSize = transform.localScale;  
    }
    void Start()
    {
        lastCount = inventory.myGrid.tileCount[(int)type];
        StartCoroutine(Appear());
    }
    void StopTween()
    {
        animationSequence?.Kill();
        transform?.DOKill();
    }
    IEnumerator Appear()
    {
        yield return new WaitForSeconds(Random.Range(0, 0.2f));
        sprite.enabled = true;
        count.enabled = true;
        Sequence seq = DOTween.Sequence();
        transform.localScale = Vector3.zero;
        seq.Append(transform.DOScale(Vector3.one * 1.2f, 0.1f));
        seq.Append(transform.DOScale(Vector3.one * 0.9f, 0.07f));
        seq.Append(transform.DOScale(Vector3.one * 1f, 0.07f));
        
        //animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.Appear, 0.1f));

    }

    void Update()
    {
        if (lastCount != inventory.myGrid.tileCount[(int)type])
        {
            StopTween();
            animationSequence = DOTween.Sequence();
            animationSequence.Append(transform.DOScale(Vector3.one * 1.2f, 0.1f));
            animationSequence.Append(transform.DOScale(Vector3.one * 0.95f, 0.1f));
            animationSequence.Append(transform.DOScale(Vector3.one * 1f, 0.1f));
            //animationBuffer.Add(new PopAnimatorInfo(gameObject, PopAnimator.Type.Emphasize, 0.1f));
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
    public MessageBubble PopMessage(string message, Vector3 pos)
    {
        MessageBubble newBubble = Instantiate(messageBubblePrefab).GetComponent<MessageBubble>();
        newBubble.transform.position = pos;
        newBubble.SetMessage(message);
        return newBubble;
    }
}
