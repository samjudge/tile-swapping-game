using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileManagerService : MonoBehaviour
{
    [SerializeField]
    private List<Tile> TilePrefabs;
    [SerializeField]
    private TileContainer TileContainerPrefab;
    [SerializeField]
    private int Columns;
    [SerializeField]
    private int Rows;
    private List<List<TileContainer>> TileMap;
    private static TileManagerService Instance;
    [SerializeField]
    private Transform UiTransformBase;
    [SerializeField]
    private Transform UiTilesTransformBase;
    [SerializeField]
    private UnitSpawner UnitSpawner;

    private bool FreezeUpdateLoop = true;
    public bool IsLocked = false;

    void Awake() {
        TileMap = new List<List<TileContainer>>();
        Instance = this;
    }
   
    public void RemoveCurrentTiles() {
        foreach(List<TileContainer> TileRow in TileMap) {
            foreach(TileContainer Tile in TileRow) {
                Destroy(Tile.gameObject);
            }
        }
        TileMap.Clear();
    }

    public void InitTiles() {
        RemoveCurrentTiles();
        for(int y = 0; y < Rows; y++) {
            TileMap.Add(new List<TileContainer>());
            for(int x = 0; x < Columns; x++) {
                Tile t = CreateNewRandomTile();
                //add tile container
                TileContainer container = Instantiate(TileContainerPrefab);
                //t.transform.position = container.transform.position;
                container.AttachNewTile(t);
                container.transform.SetParent(UiTilesTransformBase, false);
                container.transform.SetSiblingIndex(0);
                //add reference
                TileMap[y].Add(container);
            }
        }
        FreezeUpdateLoop = false;
    }

    public enum CardinalDirection { Up, Down, Left, Right }

    private Vector2 CardinalDirectionAsOffset(CardinalDirection direction){
        if(direction == CardinalDirection.Up) {
            return new Vector2(0, 1);
        }
        if(direction == CardinalDirection.Down) {
            return new Vector2(0, -1);
        }
        if(direction == CardinalDirection.Left) {
            return new Vector2(1, 0);
        }
        if(direction == CardinalDirection.Right) {
            return new Vector2(-1, 0);
        }
        return new Vector2(0, 0);
    }

    void Update() {
        if(FreezeUpdateLoop) return;
        if(Input.GetKey(KeyCode.Space)) UnitSpawner.SpawnSoldier();
        List<Tile> tilesToRemove = GetRemovableTiles();
        Dictionary<string, int> removedTileCounts = new Dictionary<string, int>();
        if(tilesToRemove.Count > 0) {
            foreach(Tile ttr in tilesToRemove){
                if(!removedTileCounts.ContainsKey(ttr.Name)) {
                    removedTileCounts[ttr.Name] = 0;
                }
                removedTileCounts[ttr.Name]++;
                Vector2 ttrc = GetTileCoordinates(ttr);
                TileMap[(int)ttrc.y][(int)ttrc.x].Tile.Remove();
                TileMap[(int)ttrc.y][(int)ttrc.x].Tile = null;
            }
            if(removedTileCounts.ContainsKey("Sword") ||
               removedTileCounts.ContainsKey("Arrow") ||
               removedTileCounts.ContainsKey("Thunder") ||
               removedTileCounts.ContainsKey("Shield") ||
               removedTileCounts.ContainsKey("Void")
            ) {
                SfxManagerService.GetInstance().PlayTilePop();
            }
            if(removedTileCounts.ContainsKey("Skull")) {
                SfxManagerService.GetInstance().PlaySkullPop();
            }
            if(removedTileCounts.ContainsKey("Sword")) {
                while(removedTileCounts["Sword"] >= 3) {
                    UnitSpawner.SpawnSoldier();
                    removedTileCounts["Sword"] -= 3;
                }
            }
            if(removedTileCounts.ContainsKey("Arrow")) {
                while(removedTileCounts["Arrow"] >= 3) {
                    UnitSpawner.SpawnArcher();
                    removedTileCounts["Arrow"] -= 3;
                }
            }
            if(removedTileCounts.ContainsKey("Thunder")) {
                while(removedTileCounts["Thunder"] >= 3) {
                    UnitSpawner.SpawnWizard();
                    removedTileCounts["Thunder"] -= 3;
                }
            }
            if(removedTileCounts.ContainsKey("Void")) {
                while(removedTileCounts["Void"] >= 3) {
                    if(LevelManagerService.GetInstance().CurrentPlayerMana < 3) {
                        StartCoroutine(IncreaseManaByXOverT(1, 0.5f));
                    }
                    removedTileCounts["Void"] -= 3;
                }
            }
            if(removedTileCounts.ContainsKey("Skull")) {
                float skullDamage = Mathf.Min(removedTileCounts["Skull"] / 2);
                while(removedTileCounts["Skull"] >= 3) {
                    removedTileCounts["Skull"] -= 3;
                    List<GameObject> PlayerSpawnedUnits = UnitSpawner.GetAllSpawnedUnits();
                    foreach(GameObject u in PlayerSpawnedUnits) {
                        if(u != null) {
                            DamagableUnit d = u.GetComponent<DamagableUnit>();
                            if(d != null) {
                                d.TakeDamage(skullDamage);
                            }
                        }
                    }
                }
            }
            StartCoroutine(PushTiles());
        }
    }

    private IEnumerator IncreaseManaByXOverT(float amount, float time) {
        float cTime = 0f;
        while(cTime < time) {
            if(LevelManagerService.GetInstance().CurrentPlayerMana >= 3) {
                break;
            }
            yield return new WaitForEndOfFrame();
            LevelManagerService.GetInstance().CurrentPlayerMana += 
                amount * ( Time.deltaTime / time );
            cTime += Time.deltaTime;
        }
    }

    private List<Tile> GetRemovableTiles(){
        List<Tile> removableTiles = new List<Tile>();
        for(int y = 0; y < TileMap.Count; y++) {
            List<TileContainer> tl = TileMap[y];
            for(int x = 0; x < tl.Count; x++) {
                foreach(CardinalDirection d in Enum.GetValues(typeof(CardinalDirection))){
                    List<Tile> tilesInLine = GetSameTilesInSerialDirection(TileMap[y][x].Tile, d);
                    if(tilesInLine.Count >= 2){
                        foreach(Tile tileInLine in tilesInLine){
                            if(!removableTiles.Contains(tileInLine)){
                                removableTiles.Add(tileInLine);
                            }
                        }
                    }
                }
            }
        }
        return removableTiles;
    }

    public IEnumerator PushTiles() {
        FreezeUpdateLoop = true;
        for(int x = 0; x <= TileMap.Count - 1; x++) {
            if(IsLocked) break;
            for(int y = TileMap[x].Count - 1; y >= 1; y--) {
                if(IsLocked) break;
                //if this node is the top node and it is empty
                //a new node must be spawned
                if(y == TileMap.Count - 1 &&
                   TileMap[y][x].Tile == null
                ) {
                    Tile nt = CreateNewRandomTile();
                    TileMap[y][x].AttachNewTile(nt);
                }
                //if the node beneith this one is empty
                if(TileMap[y - 1][x].Tile == null) {
                    //if this is the top node then a new tile must be spawned first
                    //before it can be dropped
                    //if(y == TileMap.Count - 1) {
                    //    Tile nt = CreateNewRandomTile();
                    //    TileMap[y][x].AttachNewTile(nt);
                    //}
                    //swap the node beneith this one for this
                    TileMap[y][x].TransitionToThenSwap(
                        TileMap[y][x],
                        TileMap[y - 1][x]
                    );
                    //wait for the swap to complete
                    while(TileMap[y][x].IsTransitioning) {
                        yield return null;
                    }
                    //reset 
                    y = TileMap.Count;
                    //SwapTilesInContainer(
                    //    TileMap[y][x],
                    //    TileMap[y + 1][x]
                    //);
                }
                //yield return null;
                //Debug.Break();
            }
        }
        FreezeUpdateLoop = false;
    }

    public List<Tile> GetSameTilesInSerialDirection(Tile t, CardinalDirection direction){
        List<Tile> sameTilesInARow = new List<Tile>();
        while(t != null) {
            Tile nt = GetTileInCardinalDirection(t, direction);
            //Debug.Log("ticd - " + nt);
            if(nt == null) break;
            if(nt.Name != t.Name){
                break;
            }
            t = nt;
            sameTilesInARow.Add(t);
        }
        return sameTilesInARow;
    }

    private Tile GetTileInCardinalDirection(Tile tile, CardinalDirection inDirection) {
        Vector2 pos = GetTileCoordinates(tile);
        Vector2 offset = CardinalDirectionAsOffset(inDirection);
        Vector2 newPos = pos + offset;
        if(newPos.x < 0 || newPos.x > Columns - 1 ||
           newPos.y < 0 || newPos.y > Rows - 1
        ) {
            return null;
        }
        return TileMap[(int)newPos.y][(int)newPos.x].Tile;
    }

    private bool IsTileInCardinalDirectionTo(Tile tileFrom, Tile tileTo, CardinalDirection inDirection) {
        Vector2 from = GetTileCoordinates(tileFrom);
        Vector2 to = GetTileCoordinates(tileTo);
        CardinalDirection isDirection = GetCardinalDirectionOfTile(tileFrom, tileTo);
        if(inDirection == isDirection){
            return true;
        }
        return false;
    }

    private CardinalDirection GetCardinalDirectionOfTile(Tile tileFrom, Tile tileTo) {
        Vector2 from = GetTileCoordinates(tileFrom);
        Vector2 to = GetTileCoordinates(tileTo);
        if(from.x - to.x == 1) return CardinalDirection.Left;
        if(from.x - to.x == -1) return CardinalDirection.Right;
        if(from.y - to.y == 1) return CardinalDirection.Up;
        if(from.y - to.y == -1) return CardinalDirection.Down;
        throw new Exception(
            "`TileManagerService` ->\n" +
            "`GetCardinalDirectionOfTile` ->\n" +
            "Attempted to get the direction to/from an untracked tile"
        );
    }

    private Vector2 GetTileCoordinates(Tile tile) {
        for(int y = 0; y < Rows; y++) {
            for(int x = 0; x < Columns; x++) {
                if(TileMap[y][x].Tile == tile) {
                    return new Vector2(x, y);
                }
            }
        }
        throw new Exception(
            "`TileManagerService` ->\n" +
            "`GetTileCoordinates` ->\n" +
            "Attempted to get an untracked tile"
        );
    }

    private Vector2 GetTileContainerCoordinates(TileContainer tileContainer) {
        for(int y = 0; y < Rows; y++) {
            for(int x = 0; x < Columns; x++) {
                if(TileMap[y][x] == tileContainer) {
                    return new Vector2(x, y);
                }
            }
        }
        throw new Exception(
            "`TileManagerService` ->\n" +
            "`GetTileContainerCoordinates` ->\n" +
            "Attempted to get an untracked tile container"
        );
    }

    public void SwapTilesInContainer(TileContainer tileFrom, TileContainer tileTo) {
        if(AreTilesAdjacent(tileFrom, tileTo)){
            Vector2 fromCoord = GetTileContainerCoordinates(tileFrom);
            Vector2 toCoord = GetTileContainerCoordinates(tileTo);
            SwapTiles(fromCoord, toCoord);
            tileTo.AttachNewTile(TileMap[(int)toCoord.y][(int)toCoord.x].Tile);
            tileFrom.AttachNewTile(TileMap[(int)fromCoord.y][(int)fromCoord.x].Tile);
        }
    }

    private void SwapTiles(Vector2 fromCoord, Vector2 toCoord) {
        Tile tileFrom = TileMap[(int)fromCoord.y][(int)fromCoord.x].Tile;
        Tile tileTo = TileMap[(int)toCoord.y][(int)toCoord.x].Tile;
        TileMap[(int)fromCoord.y][(int)fromCoord.x].Tile = tileTo;
        TileMap[(int)toCoord.y][(int)toCoord.x].Tile = tileFrom;
    }

    public bool AreTilesAdjacent(TileContainer tileA, TileContainer tileB){
        Vector2 from = GetTileContainerCoordinates(tileA);
        Vector2 to = GetTileContainerCoordinates(tileB);
        if((Math.Abs(from.x - to.x) == 0 || Math.Abs(from.y - to.y) == 0) &&
           (Math.Abs(from.x - to.x) == 1 || Math.Abs(from.y - to.y) == 1)
        ) {
            return true;
        }
        return false;
    }

    public static TileManagerService GetInstance() {
        return Instance;
    }

    public Transform GetUiTransfromBase() {
        return UiTransformBase;
    }

    private Tile CreateNewRandomTile() {
        int roll = Dice.Roll(0, TilePrefabs.Count);
        return Instantiate(TilePrefabs[roll]);
    }



}