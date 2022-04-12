using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingMachine : MonoBehaviour
{
    public Setting[] Settings;

    private void Start()
    {
        Settings = GetComponentsInChildren<Setting>();
        for (int i = 0; i < Settings.Length; i++)
        {
            Settings[i].Init();
        }
    }

    public void Save()
    {
        for (int i = 0; i < Settings.Length; i++)
        {
            Settings[i].SetValue();
        }

        Debug.Log("Save OK");
    }

    public void Confirm()
    {
        Save();
        LoadScene();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Main");
    }
}
