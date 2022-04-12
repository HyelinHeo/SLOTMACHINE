using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    public static int GetInt(string key, int defaultValue)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public static void SetInt(string key, int val)
    {
        PlayerPrefs.SetInt(key, val);
    }

    void Start()
    {

    }

    public virtual void Init() {
        GetValue();
    }
    public virtual void SetValue() { }
    public virtual void GetValue() { }
}
