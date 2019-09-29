using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileContainer : MonoBehaviour, IPointerClickHandler
{
    public Tile Tile;

    public void AttachNewTile(Tile tile){
        Tile = tile;
        if(Tile != null){
            tile.transform.position = transform.position;
            Tile.transform.SetParent(transform);
            //since I'm keeping world position, the scale can get screwy here
            //for a frame unless I reset it here
            tile.transform.localScale = new Vector3(1,1,1);
        }
    }

    void Update(){
        TileManagerService tileManager = TileManagerService.GetInstance();
    }

    private void SwapTiles(TileContainer A, TileContainer B) {
        TileManagerService tileManager = TileManagerService.GetInstance();
        tileManager.SwapTilesInConitainer(
            A,
            B
        );
    }

    public void OnPointerClick(PointerEventData eventData) {
        TileSelector selector = TileSelector.GetInstance();
        if(selector.IsTileSelected() && selector.GetSelected() != this){
            TileContainer tileB = selector.GetSelected();
            TransitionToThenSwap(selector.GetSelected(), tileB);
            selector.Unselect();
        } else {
            selector.Unselect();
            selector.Select(this);
        }
    }

    public bool IsTransitioning = false;

    public void TransitionToThenSwap(TileContainer tileA, TileContainer tileB) {
        IsTransitioning = true;
        bool tileATransitionComplete = false;
        bool tileBTransitionComplete = false;
        if(tileA.Tile != null){
            StartCoroutine(tileA.Tile.TransitionTo(
                tileB.transform.position,
                delegate() {
                    tileATransitionComplete = true;
                    if(tileATransitionComplete && tileBTransitionComplete) {
                        SwapTiles(tileA, tileB);
                        IsTransitioning = false;
                    }
                }
            ));
        } else {
            tileATransitionComplete = true;
        }
        if(tileB.Tile != null){
            StartCoroutine(tileB.Tile.TransitionTo(
                tileA.transform.position,
                delegate() {
                    tileBTransitionComplete = true;
                    if(tileATransitionComplete && tileBTransitionComplete) {
                        SwapTiles(tileA, tileB);
                        IsTransitioning = false;
                    }
                }
            ));
        } else {
            tileBTransitionComplete = true;
        }
        if(tileATransitionComplete && tileBTransitionComplete) {
            SwapTiles(tileA, tileB);
            IsTransitioning = false;
        }
    }

    private void DetatchTile(){
        TileManagerService tileManager = TileManagerService.GetInstance();
        Tile.transform.SetParent( tileManager.GetUiTransfromBase() );
        Tile = null;
    }
}
