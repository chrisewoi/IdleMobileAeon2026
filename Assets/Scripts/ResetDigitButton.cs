using UnityEngine;
using UnityEngine.UI;

public class ResetDigitButton : MonoBehaviour
{
    private Button button;
    public Button maxButton;
    public int digitGeneratorCount => GameManager.Ins.digitGeneratorCount;
    void Start()
    {
        button = GetComponent<Button>();
    }


    public void OnClick()
    {
        GameManager.Ins.digitGeneratorCount++;
        GameManager.Ins.digitCount = 0;
    }

    int cost = 67 * 67;
    public void ResetMax()
    {
        int buyAmount = (int)(GameManager.Ins.count / cost);
        int maxBuyAmount = 67 - GameManager.Ins.digitGeneratorCount;
        buyAmount = Mathf.Clamp(buyAmount, 0, maxBuyAmount);
        GameManager.Ins.count -= (buyAmount-1) * cost;
        GameManager.Ins.digitGeneratorCount += buyAmount;
        GameManager.Ins.digitCount = 0;
    }

    void Update()
    {
        maxButton.interactable = (int)(GameManager.Ins.count / cost) != 0;
    }
}
