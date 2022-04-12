using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;

public class ControlAddPopup : GUI
{
    public CustomSlotMachine Machine;

    public GameObject Panel;
    public InputField InputName;


    private SlotMachineLine machineLine;

    private byte[] imageData;
    private string oriUrl;
    private string fileName;
    private string saveUrl;
    

    private void Start()
    {
        Hide();
    }

    public void Show(string machineIdx)
    {
        imageData = null;
        oriUrl = string.Empty;
        InputName.text = string.Empty;
        machineLine = Machine.GetItem(machineIdx);

        Panel.SetActive(true);
    }

    public override void Hide()
    {
        Panel.SetActive(false);
    }

    public void OnClickOK()
    {
        SaveXmlFile();
        StartCoroutine(SaveImageFile());

        Hide();
    }


    public void AddImage()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"));

        FileBrowser.SetDefaultFilter(".jpg");

        //FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        FileBrowser.AddQuickLink("Users", Application.dataPath + "/../", null);

        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(false, true, null, "Load File", "Load");

        if (FileBrowser.Success)
        {
            oriUrl = "";

            for (int i = 0; i < FileBrowser.Result.Length; i++)
            {
                oriUrl = FileBrowser.Result[i];
            }
            imageData = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

            string ext = Path.GetExtension(oriUrl);
            fileName = Path.GetFileNameWithoutExtension(oriUrl);

            //if (!Directory.Exists(CustomSlotMachine.SAVE_FOLDER))
            //{
            //    Directory.CreateDirectory(CustomSlotMachine.SAVE_FOLDER);
            //}
            //Debug.Log("ext : " + ext);
            //Debug.Log("machine idx : " + machineLine.MyIdx);
            saveUrl = string.Format("{0}{1}{2}", machineLine.FOLDER, fileName, ext);

            Debug.Log(saveUrl);
        }
    }


    void SaveXmlFile()
    {
        machineLine.SaveXml(machineLine.MyIdx, fileName, saveUrl);
    }

    IEnumerator SaveImageFile()
    {
        if (imageData != null)
        {
            File.WriteAllBytes(saveUrl, imageData);

            yield return StartCoroutine(Machine.RefreshLine());
        }
    }
}
