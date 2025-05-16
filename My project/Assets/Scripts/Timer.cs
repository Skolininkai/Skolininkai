using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] public float totalTime;
    [SerializeField] private TextMeshProUGUI timerText;
    private bool stop;
    void Start()
    {
        stop = false;
    }
    void Update()
    {
        if (!stop)
        {
            totalTime += Time.deltaTime;
            UpdateTimer();
        }
        
    }
    private void UpdateTimer()
    {
        int min = Mathf.FloorToInt(totalTime / 60f);
        int sec = Mathf.FloorToInt(totalTime - min * 60);

        timerText.text = string.Format("{0:00}:{1:00}", min, sec);
    }
    public bool StopTimer()
    {
        stop = true;
        return stop;
    }
}
