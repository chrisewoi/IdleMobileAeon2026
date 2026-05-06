using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public float count;
    public int digits;

    private int _digitCount;
    public int digitCount
    {
        get => _digitCount;
        set
        {
            _digitCount = Mathf.Clamp(value, 0, 67);
            digitsCountText.text = $"{_digitCount}/67";
        }
    }

    private int _digitGeneratorCount;
    public int digitGeneratorCount
    {
        get => _digitGeneratorCount;
        set
        {
            _digitGeneratorCount = Mathf.Clamp(value, 0, 67);
            if (_digitGeneratorCount > 10)
            {
                generatorCountText.text = $"{_digitGeneratorCount}/67";
            }
            else
            {
                generatorCountText.text = "";
            }
        }
    }
    public float countBank;

    public Color col6;
    public Color col7;
    public Color col67;
    
    public string col6string;
    public string col7string;
    public string col67string;
    
    public TMP_Text countText;
    public TMP_Text digitsText;
    public TMP_Text digitsBankText;
    public TMP_Text digitsCountText;
    public TMP_Text generatorCountText;

    public Button BuyDigitButton;
    public Button ResetDigitButton;
    public Button BuyMaxDigitButton;

    public Transform digitGeneratorGroup;
    public GameObject generatorPrefab;
    public List<GameObject> digitGenerator;

    public Canvas canvas;

    private float timeSinceAdd1;

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
        timeSinceAdd1 = 1000f;
        countText.text = count.ToString();
        digitsText.text = "";

        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))Add1();
        float bankAdd = countBank * 2f * Mathf.Clamp01(timeSinceAdd1 - 0.3f) * Time.deltaTime;
        countBank -= bankAdd;
        countBank = Mathf.Clamp(countBank, 0, Mathf.Infinity);
        count += bankAdd;

        digitsBankText.text = '+' + countBank.ToString("N0");
        if (countBank < 1) digitsBankText.text = "";
        countText.text = count.ToString("N0");
        
        BuyDigitButton.gameObject.SetActive(!(digitCount >= 67));
        ResetDigitButton.gameObject.SetActive(digitCount >= 67 && digitGeneratorCount <67);
        BuyMaxDigitButton.gameObject.SetActive(digitGeneratorCount>0);

        if (digitGeneratorCount > digitGenerator.Count)
        {
            int toSpawn = digitGeneratorCount - digitGenerator.Count;
            for (int i = 0; i < toSpawn; i++)
            {
                GameObject g = Instantiate(generatorPrefab, digitGeneratorGroup);
                digitGenerator.Add(g);
                StartCoroutine(AnimateGeneratorGeneration(digitsText.gameObject, g.transform));
            }
        }

        timeSinceAdd1 += Time.deltaTime;
    }

    public void Add(int x)
    {
        count += x;

        countText.text = count.ToString();
    }

    public void Add1()
    {
        Add(1);
        foreach (GameObject g in digitGenerator)
        {
            g.GetComponent<DigitGenerator>().SetDigits();
        }
        timeSinceAdd1 = 0f;
    }

    public void AddToBank(float x)
    {
        countBank += x;
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
        col6string = ColorUtility.ToHtmlStringRGB(col6);
        col7string = ColorUtility.ToHtmlStringRGB(col7);
        col67string = ColorUtility.ToHtmlStringRGB(col67);

        int bigSize = 55 * 2;
        float textSize = digitsText.fontSize + bigSize;

        float textSizeSmall = digitsText.fontSize + (int)(bigSize / 4);


        string newText = text;
        for (int i = text.Length-1; i >= 0; i--)
        {
            char c = text[i];
            

            if (i + 1 < text.Length)
            {
                char d = text[i + 1];
                if (c == '6' && d == '7')
                {
                    newText = newText.Insert(i + 2, "!");
                    newText = newText.Insert(i, "@");
                }
            }
        }
        newText = newText.Replace("6", "(6%");
        newText = newText.Replace("7", "^7&");

        newText = newText.Replace("@(6%^7&!", "@67!");

        newText = newText.Replace("@67!", $"<size={(textSize).ToString()}><b><color=#{col6string}>6</color><color=#{col7string}>7</color></b></size>");
        //newText = newText.Replace("!", "</b></size>");
        newText = newText.Replace("(", $"<color=#{col6string}><size={textSizeSmall.ToString()}>");
        newText = newText.Replace("%", "</size></color>");
        newText = newText.Replace("^", $"<color=#{col7string}><size={textSizeSmall.ToString()}>");
        newText = newText.Replace("&", "</size></color>");

        digitsText.text = newText;

    }

    
    public IEnumerator AnimateGeneratorGeneration(GameObject g, Transform destination)
    {
        // set up the object
        TMP_Text text = g.GetComponent<TMP_Text>();
        string s = text.text;
        GameObject obj = Instantiate(g, canvas.transform);
        obj.transform.position = g.transform.position;
        obj.transform.localScale = g.transform.localScale;
        text.text = "";
        obj.GetComponent<TMP_Text>().text = s;

        Vector3 startPos = obj.transform.position;
        Vector3 endPos = destination.position;
        print("destination pos: " + destination.position);
        print("destination locpos: " + destination.localPosition);
        float t = 0;
        float scale = 1;
        while (t <= 1)//(destination.position - obj.transform.position).magnitude > 0.1f)
        {
            endPos = destination.position;
            Vector3 newPosition = Vector3.Slerp(startPos, endPos, t);

            obj.transform.position = newPosition;
            scale = Mathf.Clamp01((1 - t)+0.1f);
            obj.transform.localScale = new Vector3(scale, scale,scale);
            
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
    }
}
