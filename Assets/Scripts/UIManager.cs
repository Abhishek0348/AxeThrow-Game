using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TMPro.TextMeshProUGUI axeCountText;

    void Awake() => Instance = this;

    public void UpdateAxeCount(int count)
    {
        axeCountText.text = "- " + count;
    }
}
