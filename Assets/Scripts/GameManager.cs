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
            digitsCountText.gameObject.SetActive(!(_digitCount >= 67 && digitGeneratorCount >= 67));
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
            
            digitsCountText.gameObject.SetActive(!(_digitCount >= 67 && digitGeneratorCount >= 67));

        }
    }
    public float countBank;

    public Color col6;
    public Color col7;
    //public Color col67;
    
    public string col6string;
    public string col7string;
    //public string col67string;

    public float procChance;
    public float roundedProcChance => Mathf.Round(procChance * 100f) / 100f;
    public int forcedProcCount;
    public bool reachedLayer3;

    private int _generatorsTriggeredTally;
    public int generatorsTriggeredTally
    {
        get => _generatorsTriggeredTally;
        set
        {
            _generatorsTriggeredTally = value;
            if (bestTally < _generatorsTriggeredTally) bestTally = _generatorsTriggeredTally;
        }
    }
    private float bestTally;
    
    public TMP_Text countText;
    public TMP_Text digitsText;
    public TMP_Text digitsBankText;
    public TMP_Text digitsCountText;
    public TMP_Text generatorCountText;
    public TMP_Text triggerChanceText;
    public TMP_Text forcedTriggerText;
    public TMP_Text generatorTriggerCountText;

    public Button BuyDigitButton;
    public Button ResetDigitButton;
    public Button BuyMaxDigitButton;
    public Button BuyMaxGeneratorButton;
    public Button UpgradeChanceButton;
    public Button UpgradeForceButton;

    public Transform digitGeneratorGroup;
    public GameObject generatorPrefab;
    public List<GameObject> digitGenerator;

    public GameObject debugPanel;

    public Canvas canvas;

    private float timeSinceAdd1;
    public float timeSinceAddBank;

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
        timeSinceAddBank = 1000f;
        countText.text = count.ToString();
        digitsText.text = "";
        procChance = 0.488091f;
        reachedLayer3 = false;
        forcedProcCount = 0;
        generatorsTriggeredTally = 0;
        bestTally = 0;

        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))Add1();
        if (Input.GetKeyDown(KeyCode.D)) debugPanel.SetActive(!debugPanel.activeInHierarchy);

        if (digitCount >= 67 && digitGeneratorCount >= 67) reachedLayer3 = true;
        UpgradeChanceButton.gameObject.SetActive(reachedLayer3);
        UpgradeForceButton.gameObject.SetActive(reachedLayer3);
        float bankAdd = countBank * 2f * Mathf.Clamp01(timeSinceAdd1 - 0.3f) * Time.deltaTime;
        countBank -= bankAdd;
        countBank = Mathf.Clamp(countBank, 0, Mathf.Infinity);
        count += bankAdd;

        digitsBankText.text = '+' + countBank.ToString("N0");
        if (countBank < 1) digitsBankText.text = "";
        countText.text = count.ToString("N0");
        
        BuyDigitButton.gameObject.SetActive(!(digitCount >= 67));
        ResetDigitButton.gameObject.SetActive(digitCount >= 67 && digitGeneratorCount <67);
        BuyMaxDigitButton.gameObject.SetActive(reachedLayer3 || digitGeneratorCount>0);
        BuyMaxGeneratorButton.gameObject.SetActive(reachedLayer3);

        if (reachedLayer3) //&& digitGeneratorCount >= 67)
        {
            generatorTriggerCountText.text = $"Generators triggered: {generatorsTriggeredTally}/<b><color=#FF8300>6</color><color=#389FB2>7</color></b>\nBest progress: {Mathf.Round(bestTally/67f*100)}%";
        }
        else
        {
            generatorTriggerCountText.text = "";
        }
        
        // put forced proc here
        UpgradeForceButton.interactable = forcedProcCount < 49 && digitGeneratorCount >= 67;
        UpgradeChanceButton.interactable = roundedProcChance < 0.67 && digitGeneratorCount >= 67;

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

        if (digitGeneratorCount < digitGenerator.Count)
        {
            int toDestroy = digitGenerator.Count - digitGeneratorCount;
            print("toDestroy: " + toDestroy + "\nchildCount: " + digitGenerator.Count);
            
            for (int i = 0; i < toDestroy; i++)
            {
                digitGenerator.Remove(digitGeneratorGroup.GetChild(i).gameObject);
                Destroy(digitGeneratorGroup.GetChild(i).gameObject);
            }
        }
        
        UpdateEffect(countTransform, timeSinceAdd1);
        UpdateEffect(countBankTransform, timeSinceAddBank);
        
        timeSinceAdd1 += Time.deltaTime;
        timeSinceAddBank += Time.deltaTime;
    }

    public void UpdateEffect(Transform t, float timeSince)
    {
        Vector3 transformScale = Vector3.one * Mathf.Clamp(1.1f-timeSince, 1f, 1.1f);
        t.localScale = transformScale;
    }

    public void Add(int x)
    {
        count += x;

        countText.text = count.ToString();
    }

    public Transform countTransform;
    public Transform countBankTransform;
    public void Add1()
    {
        timeSinceAdd1 = 0f;
        generatorsTriggeredTally = 0;
        Add(1);
        foreach (GameObject g in digitGenerator)
        {
            g.GetComponent<DigitGenerator>().SetDigits();
        }

        StartCoroutine(ForceTriggers());
    }

    public void AddToBank(float x)
    {
        countBank += x;
        if(x!=0)timeSinceAddBank = 0;
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
        

        AddToBank(count6 * 6);
        AddToBank(count7 * 7);
        AddToBank((int)Mathf.Clamp((count67 * 67) - 12, 0, Mathf.Infinity));

        //modifies text with formatting
        col6string = ColorUtility.ToHtmlStringRGB(col6);
        col7string = ColorUtility.ToHtmlStringRGB(col7);
        //col67string = ColorUtility.ToHtmlStringRGB(col67);

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

    public void UpgradeChance(float percent)
    {
        procChance += percent;
        triggerChanceText.text = $"Current trigger chance: ~{roundedProcChance * 100}%";
        digitGeneratorCount = 0;
    }

    public void UpgradeForceTriggers()
    {
        forcedProcCount+=7;
        forcedTriggerText.text = $"Current forced triggers: {forcedProcCount}/49";
        digitGeneratorCount = 0;
    }

    public IEnumerator ForceTriggers()
    {
        float preWait = 0.1f;
        float waitBetweenProcs = 0.01f; //0.00833f
        
        int triggersRemaining = forcedProcCount;
        yield return new WaitForSeconds(preWait);
        //#warning
        foreach (GameObject generator in digitGenerator) //THIS CAUSES CRASH IF MAKE SPACE BUTTON PRESSED WHILE COROUTINE RUNNING!!!
        {
            if (triggersRemaining <= 0) break;
            
            DigitGenerator digitGenerator = generator.GetComponent<DigitGenerator>();
            if (!digitGenerator.is67)
            {
                digitGenerator.ForceTrigger();
                triggersRemaining--;

                int safetyCheck = digitGeneratorCount;
                yield return new WaitForSeconds(waitBetweenProcs);
                if (safetyCheck != digitGeneratorCount) StopCoroutine("ForceTriggers");
            }
        }
    }

    public void AddGeneratorDebug(int i)
    {
        digitGeneratorCount+=i;
    }
}
