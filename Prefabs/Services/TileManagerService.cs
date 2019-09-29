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

    void Awake() {
        TileMap = new List<List<TileContainer>>();
        for(int y = 0; y < Rows; y++) {
            TileMap.Add(new List<TileContainer>());
            for(int x = 0; x < Columns; x++) {
                Tile t = CreateNewRandomTile();
                //add tile container
                TileContainer container = Instantiate(TileContainerPrefab);
                t.transform.position = container.transform.position;
                container.AttachNewTile(t);
                container.transform.SetParent(UiTilesTransformBase, false);
                container.transform.SetSiblingIndex(0);
                //add reference
                TileMap[y].Add(container);
            }
        }
        Instance = this;
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
        List<Tile> tilesToRemove = GetRemovableTiles();
        if(tilesToRemove.Count > 0) {
            foreach(Tile ttr in tilesToRemove){
                Vector2 ttrc = GetTileCoordinates(ttr);
                TileMap[(int)ttrc.y][(int)ttrc.x].Tile.Remove();
                TileMap[(int)ttrc.y][(int)ttrc.x].Tile = null;
            }
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

    public void PushTiles() {
        for(int y = 0; y < TileMap.Count; y++) {
            for(int x = 0; x < TileMap.Count; x++) {
                if(TileMap[y][x].Tile == null) {
                    if(y < TileMap.Count - 1) {
                        SwapTilesInConitainer(
                            TileMap[y][x],
                            TileMap[y + 1][x]
                        );
                    } else {
                        Tile t = CreateNewRandomTile();
                        TileMap[y][x].AttachNewTile(t);
                    }
                }
            }
        }
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

    public void SwapTilesInConitainer(TileContainer tileFrom, TileContainer tileTo) {
        if(AreTilesAdjacent(tileFrom, tileTo)) {
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

    private bool AreTilesAdjacent(TileContainer tileA, TileContainer tileB){
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