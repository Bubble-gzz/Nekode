using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOrder
{
    int capacity;
    int latestOrder;
    int currentOrder;
    public AnimationOrder(int capacity = 1000)
    {
        this.capacity = capacity;
        this.latestOrder = -1;
        this.currentOrder = 0;
    }
    public int NewOrder()
    {
        latestOrder = (latestOrder + 1) % capacity;
        return latestOrder;
    }
    public void FinishOrder()
    {
        currentOrder = (currentOrder + 1) % capacity;
    }
    public bool isLatest(int order)
    {
        return order == latestOrder;
    }
}
