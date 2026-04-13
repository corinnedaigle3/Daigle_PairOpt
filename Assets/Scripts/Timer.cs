using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float time;
    [SerializeField] UIManager ui;


    void Update()
    {
        if (ui.startTimer)
        {
            time += Time.deltaTime;
            UpdateText();
        }
    }

    void UpdateText()
    {
        text.text = time.ToString("F0");
    }
}