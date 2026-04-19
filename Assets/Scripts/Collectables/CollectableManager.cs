using UnityEngine;
using TMPro;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance;

    [SerializeField] int collectedCount = 0;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] UIManager uiManager;

    private void Awake()
    {
        Instance = this;
        text.text = "Collectible Counter: " + collectedCount;
    }

    public void AddCollectible()
    {
        collectedCount++;
        text.text = "Collectible Counter: " + collectedCount;
    }

    private void Update()
    {
        if (collectedCount == 2)
        {
            uiManager.ShowWinScreen();
        }
    }
}