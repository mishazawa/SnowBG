using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public Camera cam;

    public int worldSize = 20;
    public int tileSize  = 10;
    public int viewDistance = 2;

    public GameObject prefabPlayer;
    public GameObject prefabTile;

    [HideInInspector]
    public List<GameObject> players;
    [HideInInspector]
    public Dictionary<(int, int), TileObject> xtiles;
    [HideInInspector]
    public TileObject[,] tiles;

    // tmp

    TileCoords playerCoords;

    List<TileCoords> activeTiles;

    [HideInInspector]
    public static WorldManager Instance { get; private set; }
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
            Init();
        }
    }

    void Init() {
        players = new List<GameObject>();
        xtiles   = new Dictionary<(int,int), TileObject>();
        tiles    = new TileObject[worldSize, worldSize];
        activeTiles = new List<TileCoords>();
    }

    void Start() {
        AddPlayer(Vector3.up + new Vector3(worldSize/2, 0, worldSize/2) * tileSize);
        players[0].GetComponent<Controls>().AttachCamera(cam);
    }

    void Update() {
        CheckTiles();
    }

    public void AddPlayer(Vector3 spawnPos) {
        var pl = Instantiate(prefabPlayer, spawnPos, Quaternion.identity);
        pl.transform.position = spawnPos;
        playerCoords = new TileCoords(-1, -1);
        players.Add(pl);
    }

    public void AddTile(TileCoords coords) {
        var tile = new TileBuilder(prefabTile)
                      .SetScale(tileSize)
                      .SetCoords(coords)
                      .Build();

        tiles[coords.x, coords.y] = tile;
    }

    public void CheckTiles () {



        foreach (var pl in players) {
            // tmp
            var pos = TileCoords.ConvertToTileCoords(pl.transform.position, tileSize);
            if (!pos.Eq(playerCoords)) {
                playerCoords = pos;

                var prevActiveTiles = new List<TileCoords>(activeTiles);

                for (int x = pos.x - viewDistance; x < pos.x + viewDistance; x++) {
                    for (int y = pos.y - viewDistance; y < pos.y + viewDistance; y++) {
                        if (!IsInWorldBounds(x, y)) continue;

                        var coords = new TileCoords(x, y);

                        if (tiles[x, y] == null) {
                            AddTile(coords);
                            activeTiles.Add(coords);
                        } else {
                            Debug.Log($"{coords.x}, {coords.y} = {tiles[x, y].visible}");
                            if (!tiles[x, y].visible) {
                                tiles[x, y].SetVisibility(true);
                                activeTiles.Add(coords);
                            }
                        }

                        // intersection
                        for (int i = 0; i < prevActiveTiles.Count; i++) {
                            if (prevActiveTiles[i].Eq(coords)) {
                                prevActiveTiles.RemoveAt(i);
                            }
                        }
                    }
                }

                foreach (var t in prevActiveTiles) {
                    tiles[t.x, t.y].SetVisibility(false);
                }
            }
        }
    }

    int PositionToIndex (int x) {
        return x + worldSize / 2;
    }

    bool IsInWorldBounds (int x, int y) {
        return x < worldSize && x >= 0 && y < worldSize && y >= 0;
    }

    public void NotifyTrigger((int, int) id) {
        // Enter
        // TileObject t;
        // if (!tiles.TryGetValue(id, out t)) return;

        // var ns = t.Neighbours();

        //     t.Leave();
        //     foreach(var pair in ns) {
        //         if (tiles.ContainsKey(pair)) {
        //             if (tiles[pair].visitors <= 0) {
        //                 tiles[pair].SetActive(true);
        //                 Debug.Log($"{pair} hidden.");
        //             }
        //         }
        //     }

        //     // populate new tiles on enable old
        //     foreach(var pair in ns) {
        //         if (!tiles.ContainsKey(pair)) {
        //             AddTile(pair);
        //         } else {
        //             tiles[pair].SetActive(true);
        //         }
        //     }

        //     // mark visited
        //     t.Visit();
    }


}

