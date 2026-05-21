using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyDigitButton : MonoBehaviour
{
    private Button button;
    public TMP_Text maxButtonText;
    public Button maxButton;
    

    public float cost;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
        //maxButtonText = maxButton.GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        bool interactible = GameManager.Ins.count >= 67 && GameManager.Ins.digitCount < 67;
        button.interactable = interactible;

        button.gameObject.SetActive(GameManager.Ins.digitCount < 67);
        
        
        int buyAmount = (int)(GameManager.Ins.count / cost);
        int maxBuyAmount = 67 - GameManager.Ins.digitCount;
        buyAmount = Mathf.Clamp(buyAmount, 0, maxBuyAmount);
        bool interactable = buyAmount > 0;
        maxButton.interactable = interactable;
        maxButtonText.text = interactable ? $"Buy\nMax\n(<u>{buyAmount}</u>)":"Buy\nMax";
    }

    public void BuyDigit()
    {
        if (GameManager.Ins.count >= cost)
        {
            GameManager.Ins.count -= cost;
            GameManager.Ins.digitCount++;
        }
    }

    public void BuyMax()
    {
        int buyAmount = (int)(GameManager.Ins.count / cost);
        int maxBuyAmount = 67 - GameManager.Ins.digitCount;
        buyAmount = Mathf.Clamp(buyAmount, 0, maxBuyAmount);
        GameManager.Ins.count -= buyAmount * cost;
        GameManager.Ins.digitCount += buyAmount;
        bool interactable = buyAmount > 0;
        maxButton.interactable = interactable;
        maxButtonText.text = interactable ? $"Buy\nMax\n(<u>{buyAmount}</u>)":"Buy\nMax";
    }
}
