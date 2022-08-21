using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCoords {
    public int x;
    public int y;

    public TileCoords(int x, int y) {
        this.x = x;
        this.y = y;
    }
    public bool Eq (TileCoords other) {
        return x == other.x && y == other.y;
    }
    public static TileCoords ConvertToTileCoords (Vector3 position, int tileSize) {
        int x = Mathf.CeilToInt(position.x / tileSize);
        int y = Mathf.CeilToInt(position.z / tileSize);
        return new TileCoords(x, y);
    }
}


public class TileObject {
    public TileCoords coords;
    public GameObject instance;
    public bool visible;

    public TileObject (GameObject prefab) {
        instance = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }

    public void SetVisibility(bool v) {
        visible = v;
        instance.SetActive(v);
    }

    public (int, int)[] Neighbours () {
        var x = coords.x;
        var y = coords.y;
        return new (int, int)[]{
            (x+1, y+1),
            (x-1, y-1),
            (x-1, y+1),
            (x+1, y-1),

            (x, y+1),
            (x, y-1),
            (x+1, y),
            (x-1, y)
        };
    }
}

public class TileBuilder {

    TileObject t;

    public TileBuilder (GameObject prefab) {
        t = new TileObject(prefab);
        t.visible = true;
    }

    public TileObject Build() {
        return t;
    }

    public TileBuilder SetScale(int size) {
        t.instance.transform.localScale = Vector3.one * size;
        return this;
    }

    public TileBuilder SetCoords(TileCoords coords) {
        t.instance.transform.position = new Vector3(t.instance.transform.localScale.x * coords.x , 0, t.instance.transform.localScale.z * coords.y);
        t.coords = coords;
        return this;
    }

}
