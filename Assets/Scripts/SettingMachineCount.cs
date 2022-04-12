using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMachineCount : Setting
{
    public Slider SlSlotCount;
    public Text TxtSlotCount;
    public int SlotCount;

    public const string PREF_NAME = "SlotCount";
    public const int DEFAULT_COUNT = 3;

    void Start()
    {
        
    }

    public void SetUI(int val)
    {
        SetValueText(val);
        SlSlotCount.value = val;
    }

    public override void GetValue()
    {
        base.GetValue();

        SlotCount = GetInt(PREF_NAME, DEFAULT_COUNT);

        SetUI(SlotCount);
    }

    public override void SetValue()
    {
        base.SetValue();

        SetInt(PREF_NAME, SlotCount);
    }

    public void OnSliderValueChanged()
    {
        SetValueText(SlSlotCount.value);
    }

    public void SetValueText(float val)
    {
        SlotCount = (int)val;
        TxtSlotCount.text = val.ToString() + "개";
    }


}
