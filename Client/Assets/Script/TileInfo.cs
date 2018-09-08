using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TileInfo : MonoBehaviour {

    public enum TileType {
        PLAIN = 0,

        MAX
    }

    public Image imgTile;

    //
    private TileType _tileType;
    private int _grade;
    private int _level;
    private long _settedUnitID;

    public void Initialize(TileType type, ) {
        _tileType = type;
    }


    public bool SetUnit(long UnitID) {
        if (ResourceManager.Get().GetUnitByID(UnitID) == null)
            return false;

        return true;
    }
}
