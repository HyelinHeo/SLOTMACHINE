using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XMLList : XMLObject, IListInterface
{
    private const int DEFAULT_LINE_COUNT = 3;

    public List<XMLListLine> Lines;

    public XMLList() { }

    public XMLListLine AddLine(string machineId)
    {
        if (Lines == null)
            Lines = new List<XMLListLine>();

        XMLListLine line = new XMLListLine();
        line.ID = machineId;

        Lines.Add(line);

        return line;
    }

    public XMLListLine AddLastLine()
    {
        int idx = GetCount();
        Debug.Log(idx);

        return AddLine(idx.ToString());
    }

    public void InitLines()
    {
        for (int i = 0; i < DEFAULT_LINE_COUNT; i++)
        {
            AddLastLine();
        }
    }

    public void RemoveLastLine()
    {
        if (Lines != null)
        {
            int idx = GetCount() - 1;

            RemoveLine(idx.ToString());
        }
    }

    public void RemoveLine(string machineId)
    {
        XMLListLine line = FindLine(machineId);
        if (line != null)
        {
            line.Clear();
            Lines.Remove(line);
        }
    }

    public XMLListLine FindLine(string machineId)
    {
        if (Lines != null)
        {
            return Lines.Find(o => o.ID == machineId);
        }
        return null;

    }

    public void AddItem(string machineId, string txt, string imgPath)
    {
        XMLListLine line = FindLine(machineId);
        if (line != null)
        {
            line.AddItem(txt, imgPath);
        }
    }

    public void Clear()
    {
        // todo
        if (Lines != null)
        {
            for (int i = 0; i < Lines.Count; i++)
            {
                Lines[i].Clear();
            }
            Lines.Clear();
        }
    }

    public int GetCount()
    {
        if (Lines == null)
            return 0;
        else
            return Lines.Count;
    }
}
