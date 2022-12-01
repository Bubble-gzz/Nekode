using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GridData
{
    public int n, m;
    public NekoData neko;
    public List<TileData> tiles;
    public GridData(int _n, int _m)
    {
        this.n = _n; this.m = _m;
        tiles = new List<TileData>();
    }
}

[Serializable]
public class TileData{
    public int i, j, type, permission, value;
    public string label;
    public List<ArrowData> arrows;
    public TileData(int _i, int _j, int _type, int _permission, string _label, int _value = 0)
    {
        this.i = _i;
        this.j = _j;
        this.type = _type;
        this.permission = _permission;
        this.label = _label;
        this.value = _value;
        arrows = new List<ArrowData>();
    }
}

[Serializable]
public class ArrowData{
    public int type, side, direction;
    public ArrowData(int _type, int _side, int _direction) {
        this.type = _type; this.side = _side; this.direction = _direction;
    }
}

[Serializable]
public class NekoData{
    public int i, j, mode, value, direction;
    public NekoData(int _i, int _j, int _mode, int _value, int _direction) {
        this.i = _i; this.j = _j;
        this.mode = _mode;
        this.value = _value;
        this.direction = _direction;
    }
}