using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleFileBrowser;
using UnityEngine.UI;

public class SlotMachineLine : MonoBehaviour, IListInterface
{
    public SlotListItem SourcePrefab;
    public ControlAddPopup AddPopup;
    public CustomSlotMachine SlotMachine;

    public string MyIdx;

    public List<SlotListItem> Items;
    private string oriUrl;
    private byte[] imageData;
    public string FOLDER;
    private string fileName;
    private XMLList xmlList;
    public Text SubTitle;
    public RectTransform Content;

    public int GetCount()
    {
        if (Items == null)
            return 0;
        else
            return Items.Count;
    }

    private IEnumerator Start()
    {
        SourcePrefab.gameObject.SetActive(false);
        yield return null;


        FOLDER = string.Format("{0}/{1}/", CustomSlotMachine.SAVE_FOLDER, MyIdx);
        fileName = string.Format("{0}{1}.xml", FOLDER, MyIdx);

        //int SettingSlotCount = Setting.GetInt(SettingMachineCount.PREF_NAME, SettingMachineCount.DEFAULT_COUNT);

        //if (LoadXml())
        //{
        //    CheckMachineLine(SettingSlotCount);
        //    Setting.SetInt(SettingMachineCount.PREF_NAME, xmlList.GetCount());

        //    yield return StartCoroutine(CreateLine());
        //}
        if (LoadXml())
        {
            if (xmlList != null)
            {
                SubTitle.text = xmlList.Name;
                XMLListLine xmlLine = xmlList.Lines[int.Parse(MyIdx)];
                yield return StartCoroutine(LoadItemList(xmlLine.Items));
                Content.sizeDelta = new Vector2(Content.sizeDelta.x, Items[0].Height * Items.Count);
            }
        }
        //yield return StartCoroutine(LoadItemList());
    }

    bool LoadXml()
    {
        if (!Directory.Exists(FOLDER) || !File.Exists(fileName))
        {
            xmlList = new XMLList();
            xmlList.ID = MyIdx;
            xmlList.Name = SlotMachine.Name;

            xmlList.InitLines();

            return true;
        }
        else
        {
            string txt = File.ReadAllText(fileName);

            if (xmlList != null)
                xmlList.Clear();

            xmlList = XMLSerializer.Deserialization<XMLList>(txt);
            if (xmlList != null)
            {
                return true;
            }
            else
            {
                Debug.LogErrorFormat("file not found. {0}", fileName);
                return false;
            }
        }
    }

    int currentID;
    public void OnClickRunButton()
    {
        currentID = Random.Range(0, Items.Count);
        Debug.Log(MyIdx + " , " + currentID);
    }
    
    public void SaveXml(string machineId, string txt, string imgPath)
    {
        if (!Directory.Exists(FOLDER))
        {
            Directory.CreateDirectory(FOLDER);
        }

        xmlList.AddItem(machineId, txt, imgPath);

        if (XMLSerializer.Serialization<XMLList>(xmlList, fileName))
        {
            Debug.Log("save ok");
        }
        else
        {
            Debug.LogError("save failed");
        }

    }

    string FileNameOnly;
    string FullFileName;
    public void TestSave()
    {
        if (!Directory.Exists(FOLDER))
        {
            Directory.CreateDirectory(FOLDER);
            return;
        }
        else
        {
            DirectoryInfo di = new DirectoryInfo(FOLDER);
            foreach (FileInfo File in di.GetFiles())
            {
                if (File.Extension.ToLower().CompareTo(".jpg") == 0|| File.Extension.ToLower().CompareTo(".png") == 0)
                {
                    FileNameOnly = File.Name.Substring(0, File.Name.Length - 4);
                    FullFileName = File.FullName;
                    SaveXml(MyIdx, FileNameOnly, FullFileName);
                }
            }
        }
    }

    private void ClearItemList()
    {
        if (Items != null)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Clear();
                Destroy(Items[i].gameObject);
            }
            Items.Clear();
        }
    }

    public IEnumerator LoadItemList(List<XMLListItem> xmlItems)
    {
        if (xmlItems != null)
        {
            int count = xmlItems.Count;
            for (int i = 0; i < count; i++)
            {
                SlotListItem newItem = Instantiate(SourcePrefab);
                newItem.transform.SetParent(SourcePrefab.transform.parent);

                newItem.ID = xmlItems[i].ID;
                newItem.Txt.text = xmlItems[i].Txt;

                string imgPath = xmlItems[i].ImgSrc;
                if (!string.IsNullOrEmpty(imgPath))
                {
                    string ext = Path.GetExtension(imgPath).ToLower();
                    if (ext == ".jpg" || ext == ".png")
                    {
                        byte[] bytes = File.ReadAllBytes(imgPath);

                        if (bytes.Length > 0)
                        {
                            Texture2D texture = new Texture2D(0, 0);
                            texture.LoadImage(bytes);
                            newItem.Img.texture = texture;

                            yield return null;
                        }
                    }
                }

                newItem.gameObject.SetActive(true);
                Items.Add(newItem);
            }
        }
    }


    //public IEnumerator LoadImageList()
    //{
    //    ClearItemList();
    //    Items = new List<SlotListItem>();

    //    string sufix = MyIdx + "_";
    //    string filter = string.Format("{0}*.*", sufix);
    //    //string filter = string.Format("{0}*.jpg;{0}*.png", sufix);

    //    Debug.Log(MyIdx);

    //    string[] files = Directory.GetFiles(CustomSlotMachine.SAVE_FOLDER, filter, SearchOption.TopDirectoryOnly);
    //    for (int i = 0; i < files.Length; i++)
    //    {
    //        string ext = Path.GetExtension(files[i]).ToLower();
    //        if (ext == ".jpg" || ext == ".png")
    //        {
    //            byte[] bytes = File.ReadAllBytes(files[i]);

    //            if (bytes.Length > 0)
    //            {
    //                SlotListItem newItem = Instantiate(SourcePrefab);
    //                newItem.transform.SetParent(SourcePrefab.transform.parent);

    //                Texture2D texture = new Texture2D(0, 0);
    //                texture.LoadImage(bytes);
    //                newItem.Img.texture = texture;

    //                newItem.gameObject.SetActive(true);
    //                Items.Add(newItem);

    //                yield return null;
    //            }
    //        }
    //    }
    //}

    public void ShowAddPopup()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            AddImages();
        }
        else
        {
            AddPopup.Show(MyIdx);
        }
    }


    public void AddImages()
    {
        FileBrowser.AddQuickLink("Users", Application.dataPath + "/../", null);

        StartCoroutine(ShowLoadDialogFolderCoroutine());
    }

    IEnumerator ShowLoadDialogFolderCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(true, true, null, "Load Folder", "Load");

        if (FileBrowser.Success)
        {
            oriUrl = "";

            for (int i = 0; i < FileBrowser.Result.Length; i++)
            {
                oriUrl = FileBrowser.Result[i];
            }
            if (Directory.Exists(oriUrl))
            {
                DirectoryInfo info = new DirectoryInfo(oriUrl);
                foreach (var item in info.GetFiles())
                {
                    //SaveXmlFile();
                    //StartCoroutine(SaveImageFile());
                }
            }
            Debug.Log("oriURL : " + oriUrl);
            Debug.Log("count : " + FileBrowser.Result.Length);
            //saveUrl = string.Format("{0}{1}_{2}{3}", Machine.FOLDER, machineLine.MyIdx, machineLine.GetCount(), ext);
        }
    }


    //void SaveXmlFile()
    //{
    //    Machine.SaveXml(MyIdx, InputName.text, saveUrl);
    //}

    //IEnumerator SaveImageFile()
    //{
    //    if (imageData != null)
    //    {
    //        File.WriteAllBytes(saveUrl, imageData);

    //        yield return StartCoroutine(Machine.RefreshLine());
    //    }
    //}

    public void Clear()
    {
        if (Items != null)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Clear();
                Destroy(Items[i].gameObject);
            }
            Items.Clear();
        }
    }
}
