using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class DigitGenerator : MonoBehaviour
{
    public bool is67;
    public TMP_Text text;
    public Image bg;
    public Color forceColor1,forceColor2,forceColor3,forceColor4,forceColor5;
    private Color defaultColor;
    public Vector2 startPos;
    public int forceCount;

    public float probability => GameManager.Ins.procChance;

    private string col6string => GameManager.Ins.col6string;

    private string col7string => GameManager.Ins.col7string;
    //0.488091

    void Start()
    {
        defaultColor = bg.color;
        startPos = transform.position;
        forceCount = 0;
    }
    public void SetDigits()
    {
        forceCount = 0;
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
        if(!is67)forceCount++;
        switch (forceCount)
        {
            case 1:
                bg.color = forceColor1;
                break;
            case 2:
                bg.color = forceColor2;
                break;
            case 3:
                bg.color = forceColor3;
                break;
            case 4:
                bg.color = forceColor4;
                break;
            case 5:
                bg.color = forceColor5;
                break;
            default:
                bg.color = forceColor1;
                break;
        }
        //bg.color = forceColor1;
        bool successfulReroll = Random.value < probability;
        if(successfulReroll)is67 = true;
        AddTo67Tally(); // only add if it hasn't already been counted
        if(successfulReroll)GameManager.Ins.AddToBank(67);
        if(is67)text.text =
            $"<b><color=#{col6string}>6</color><color=#{col7string}>7</color></b>";
    }

    private void AddTo67Tally()
    {
        if (is67)
        {
            GameManager.Ins.generatorsTriggeredTally++;
            forceCount = 0;
        }
    }
}


