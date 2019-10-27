using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    private TileContainer SelectedContainer;
    private static TileSelector Instance;

    public void Awake(){
        Instance = this;
        gameObject.SetActive(false);
    }

    public static TileSelector GetInstance() {
        return Instance;
    }

    void Update(){
    }

    public void Select(TileContainer TileContainer){
        SelectedContainer = TileContainer;
        transform.localPosition = TileContainer.transform.localPosition;
        gameObject.SetActive(true);
    }

    public void Unselect(){
        SelectedContainer = null;
        gameObject.SetActive(false);
    }

    public bool IsTileSelected() {
        return (SelectedContainer != null);
    }

    public TileContainer GetSelected() {
        return SelectedContainer;
    }
}
