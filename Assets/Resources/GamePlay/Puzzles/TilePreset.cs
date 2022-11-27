using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Tile Preset", menuName = "ScriptableObjects/Tile Preset", order = 1)]
public class TilePreset : ScriptableObject
{
    public List<TilePresetPair> tileCounts = new List<TilePresetPair>();
    public List<MyTile.Type> mainInventory;
    public List<MyTile.Type> arithInventory;
    public List<MyTile.Type> logicInventory;
}

[Serializable]
public class TilePresetPair{
    public MyTile.Type type;
    public int count;
}