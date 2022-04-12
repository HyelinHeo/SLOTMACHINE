using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public enum Permission { Denied = 0, Granted = 1, ShouldAsk = 2 };

interface IListInterface
{
    void Clear();
    int GetCount();
}


public class CustomSlotMachine : MonoBehaviour, IListInterface
{

    public static string SAVE_FOLDER;

    public int Count;
    public string Name;

    public int SlotCount;
    public SlotMachineLine SlotLinePrefab;

    public List<SlotMachineLine> Lines;
    public string FOLDER;
    private string fileName;


    public int GetCount()
    {
        if (Lines == null)
            return 0;
        else
            return Lines.Count;
    }

    public SlotMachineLine GetItem(string idx)
    {
        if (Lines != null)
        {
            return Lines.Find(o => o.MyIdx == idx);
        }
        return null;
    }


    void CheckMachineLine(int SettingSlotCount)
    {
        int xmlSlotCount = Count;
        if (xmlSlotCount != SettingSlotCount)
        {
            SlotCount = SettingSlotCount;

            int m = xmlSlotCount - SettingSlotCount;
            //if (m > 0)
            //{
            //    for (int i = 0; i < m; i++)
            //    {
            //        xmlList.RemoveLastLine();
            //    }
            //}
            //else if (m < 0)
            //{
            //    int addCount = Mathf.Abs(m);
            //    for (int i = 0; i < addCount; i++)
            //    {
            //        xmlList.AddLastLine();
            //    }
            //}
        }
        else
        {
            SlotCount = xmlSlotCount;
        }
    }

    private IEnumerator Start()
    {
        SAVE_FOLDER = Application.dataPath + "/../data";
        //FOLDER = string.Format("{0}/{1}/", SAVE_FOLDER, ID);
        //fileName = string.Format("{0}{1}.xml", FOLDER, ID);

        int SettingSlotCount = Setting.GetInt(SettingMachineCount.PREF_NAME, SettingMachineCount.DEFAULT_COUNT);
        DirectoryInfo info = new DirectoryInfo(SAVE_FOLDER);
        Count = 0;
        foreach (var item in info.GetDirectories())
        {
            Count++;
        }
        CheckMachineLine(SettingSlotCount);
        Setting.SetInt(SettingMachineCount.PREF_NAME, Count);

        yield return StartCoroutine(CreateLine());
    }

    public void MoveSetup()
    {
        SceneManager.LoadScene("Settings");
    }

    public void Clear()
    {
        if (Lines != null)
        {
            for (int i = 0; i < Lines.Count; i++)
            {
                Lines[i].Clear();
                Destroy(Lines[i].gameObject);
            }
            Lines.Clear();
        }
    }

    public IEnumerator RefreshLine()
    {
        //if (LoadXml())
        //{
        //    Clear();
        //    yield return StartCoroutine(CreateLine());
        //}
        yield return null;
    }

    public IEnumerator CreateLine()
    {
        for (int i = 0; i < SlotCount; i++)
        {
            SlotMachineLine newLine = Instantiate(SlotLinePrefab);
            newLine.MyIdx = i.ToString();
            newLine.transform.SetParent(SlotLinePrefab.transform.parent);
            newLine.gameObject.SetActive(true);

            Lines.Add(newLine);
            //////////////////////////////////////////////////////////////////////
            //if (xmlList != null)
            //{
            //    XMLListLine xmlLine = xmlList.Lines[i];
            //    yield return StartCoroutine(newLine.LoadItemList(xmlLine.Items));
            //}
            yield return null;
        }
    }

}
