using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TextEmitter : MonoBehaviour
{
    [SerializeField]
    private Text TextPrefab;
    [SerializeField]
    public Canvas Canvas;

    public Text MakeText(string Text, Color c){
        Text t = Instantiate(TextPrefab);
        t.color = c;
        t.text = Text;
        t.transform.SetParent(Canvas.transform);
        return t;
    }

    public Text MakeText(string Text){
        Text t = Instantiate(TextPrefab);
        t.text = Text;
        t.transform.SetParent(Canvas.transform);
        return t;
    }
}
