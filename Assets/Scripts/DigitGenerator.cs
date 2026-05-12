using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DigitGenerator : MonoBehaviour
{
    public bool is67;
    public TMP_Text text;
    public Image bg;
    public Color forceColor;
    private Color defaultColor;

    public float probability => GameManager.Ins.procChance;

    private string col6string => GameManager.Ins.col6string;

    private string col7string => GameManager.Ins.col7string;
    //0.488091

    void Start()
    {
        defaultColor = bg.color;
    }
    public void SetDigits()
    {
        bg.color = defaultColor;
        is67 = Random.value < probability; // chance of a 67 appearing in a 67 digit string
        
        if (is67)
        {
            GameManager.Ins.AddToBank(67);
            text.text =
                $"<b><color=#{col6string}>6</color><color=#{col7string}>7</color></b>";
        }
        else
        {
            text.text = "";
        }
        AddTo67Tally();
    }

    public void ForceTrigger()
    {
        bg.color = forceColor;
        bool successfulReroll = Random.value < probability;
        if(successfulReroll)is67 = true;
        AddTo67Tally(); // only add if it hasn't already been counted
        GameManager.Ins.AddToBank(67);
        if(is67)text.text =
            $"<b><color=#{col6string}>6</color><color=#{col7string}>7</color></b>";
    }

    private void AddTo67Tally()
    {
        if (is67) GameManager.Ins.generatorsTriggeredTally++;
    }
}


