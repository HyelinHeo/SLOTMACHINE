using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XMLListLine : XMLObject, IListInterface
{
    public List<XMLListItem> Items;

    public XMLListLine() { }

    public void AddItem(string txt, string imgPath)
    {
        if (Items == null)
        {
            Items = new List<XMLListItem>();
        }

        XMLListItem item = new XMLListItem();
        item.ID = GetCount().ToString();
        item.Txt = txt;
        item.ImgSrc = imgPath;

        Items.Add(item);
    }

    public void Clear()
    {
        //todo   
        if (Items != null)
        {
            Items.Clear();
        }
    }

    public int GetCount()
    {
        if (Items == null)
            return 0;
        else
            return Items.Count;
    }
}
