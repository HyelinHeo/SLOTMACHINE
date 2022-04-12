using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotListItem : MonoBehaviour
{
    public string ID;
    public RawImage Img;
    public Text Txt;
    public float Height;
    public RectTransform myTr;


    void Start()
    {

        Height = myTr.sizeDelta.y;
    }

    public void Clear()
    {
        Destroy(Img.texture);
        Txt.text = "";
    }
}
