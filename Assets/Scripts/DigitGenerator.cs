using System.Numerics;
using TMPro;
using UnityEngine;

public class DigitGenerator : MonoBehaviour
{
    public bool is67;
    public TMP_Text text;

    public float probability => GameManager.Ins.procChance;

    private string col6string => GameManager.Ins.col6string;

    private string col7string => GameManager.Ins.col7string;
    //0.488091
    
    public void SetDigits()
    {
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

    }

    public void ForceTrigger()
    {
        is67 = true;
        GameManager.Ins.AddToBank(67);
        text.text =
            $"<b><color=#{col6string}>6</color><color=#{col7string}>7</color></b>";
    }
}


