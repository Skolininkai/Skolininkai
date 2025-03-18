using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private float totalTime;
    [SerializeField] private TextMeshProUGUI timerText;
    void Start()
    {
        
    }
    void Update()
    {
        totalTime += Time.deltaTime;
        int min = Mathf.FloorToInt(totalTime / 60f);
        int sec = Mathf.FloorToInt(totalTime - min * 60);

        timerText.text = string.Format("{0:00}:{1:00}", min, sec);
    }
}
