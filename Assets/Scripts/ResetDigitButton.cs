using UnityEngine;
using UnityEngine.UI;

public class ResetDigitButton : MonoBehaviour
{
    private Button button;
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
}
