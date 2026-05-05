using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyDigitButton : MonoBehaviour
{
    private Button button;
    public TMP_Text digitCountText;

    public float cost;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        bool interactible = GameManager.Ins.count >= 67 && GameManager.Ins.digitCount < 67;
        button.interactable = interactible;

        button.gameObject.SetActive(GameManager.Ins.digitCount < 67);
    }

    public void BuyDigit()
    {
        if (GameManager.Ins.count >= cost)
        {
            GameManager.Ins.count -= cost;
            GameManager.Ins.digitCount++;

            digitCountText.text = $"{GameManager.Ins.digitCount}/67";
        }
    }
}
