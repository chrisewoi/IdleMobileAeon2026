using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float count;
    public int digits;
    public int digitCount;
    public float countBank;

    public Color col6;
    public Color col7;
    public Color col67;
    
    public TMP_Text countText;
    public TMP_Text digitsText;
    public TMP_Text digitsBankText;

    private static GameManager _instance;
    public static GameManager Ins
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(GameManager).ToString());
                    _instance = singleton.AddComponent<GameManager>();
                    DontDestroyOnLoad(singleton);
                }
            }
            return _instance;
        }
    }
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    void Start()
    {
        //count = 0;
        //digitCount = 20;
        digits = 0;
        countBank = 0;
        countText.text = count.ToString();
        digitsText.text = "";

        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        float bankAdd = countBank * 1.5f * Time.deltaTime;
        //print("bankAdd: " + bankAdd);
        countBank -= bankAdd;
        countBank = Mathf.Clamp(countBank, 0, Mathf.Infinity);
        count += bankAdd;

        digitsBankText.text = countBank.ToString("N0");
        if (countBank == 0) digitsBankText.text = "";
        countText.text = count.ToString("N0");
    }

    public void Add(int x)
    {
        count += x;

        countText.text = count.ToString();
    }

    public void Add1()
    {
        Add(1);
    }


    public void SetDigits()
    {
        string text = "";
        for (int i = 0; i < digitCount; i++)
        {
            int number = Random.Range(0, 9);
            text += number.ToString();
        }
        //on unmodified text
        string textCount = text;
        int count67 = Regex.Matches(textCount, "67").Count;
        textCount = textCount.Replace("67", "");
        int count6 = Regex.Matches(textCount, "6").Count;
        int count7 = Regex.Matches(textCount, "7").Count;
        

        countBank += count6 * 6;
        countBank += count7 * 7;
        countBank += (int)Mathf.Clamp((count67 * 67) - 12, 0, Mathf.Infinity);

        //modifies text with formatting
        string col6string = ColorUtility.ToHtmlStringRGB(col6);
        string col7string = ColorUtility.ToHtmlStringRGB(col7);
        string col67string = ColorUtility.ToHtmlStringRGB(col67);

        int bigSize = 55 * 2;
        float textSize = digitsText.fontSize + bigSize;
        //textSize = int.Parse(textSize.ToString().Replace('6', '8'));
        //textSize = int.Parse(textSize.ToString().Replace('7', '8'));
        
        float textSizeSmall = digitsText.fontSize + (int)(bigSize / 4);
        //textSizeSmall = int.Parse(textSizeSmall.ToString().Replace('6', '8'));
        //textSizeSmall = int.Parse(textSizeSmall.ToString().Replace('7', '8'));


        string newText = text;
        for (int i = text.Length-1; i >= 0; i--)
        {
            char c = text[i];
            

            if (i + 1 < text.Length)
            {
                char d = text[i + 1];
                //print("c: " + c + "d: " + d);
                if (c == '6' && d == '7')
                {
                    //print("TRIGGERED");
                    newText = newText.Insert(i + 2, "!");
                    newText = newText.Insert(i, "@");
                }
            }
        }
        newText = newText.Replace("6", "(6%");
        newText = newText.Replace("7", "^7&");

        newText = newText.Replace("@(6%^7&!", "@67!");

        //print("newText: " + newText);

        newText = newText.Replace("@67!", $"<size={(textSize).ToString()}><b><color=#{col6string}>6</color><color=#{col7string}>7</color></b></size>");
        //newText = newText.Replace("!", "</b></size>");
        newText = newText.Replace("(", $"<color=#{col6string}><size={textSizeSmall.ToString()}>");
        newText = newText.Replace("%", "</size></color>");
        newText = newText.Replace("^", $"<color=#{col7string}><size={textSizeSmall.ToString()}>");
        newText = newText.Replace("&", "</size></color>");
        
        
        
        /*text = text.Replace("67", $"<size={(textSize).ToString()}><b>67</b></size>");
        text = text.Replace("6", $"<color=#{col6string}><size={textSizeSmall.ToString()}6</size></color>");
        text = text.Replace("7", $"<color=#{col7string}>7</color>");
        
        text = text.Replace($"<size=<color=#{col6string}>6</color>", $"<size=6");
        text = text.Replace($"<size=<color=#{col7string}>7</color>", $"<size=7");*/
        //text = text.Replace($"<size=<color=#{col7string}>7</color>", $"<size=7");
        

        
        
        digitsText.text = newText;

    }
}
