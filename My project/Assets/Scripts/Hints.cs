using UnityEngine;
using TMPro;

public class Hints : MonoBehaviour
{
    public static Hints instance;

    public TextMeshProUGUI hintText;
    public CanvasGroup hintCanvasGroup;

    private void Awake()
    {
        // Uþtikrinam kad bûtø tik vienas HintSystem
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        HideHint();
    }

    public void ShowHint(string message, float duration)
    {
        StopAllCoroutines();
        hintText.text = message;
        hintCanvasGroup.alpha = 1f;
        Invoke("HideHint", duration);
    }

    public void HideHint()
    {
        StartCoroutine(FadeOut());
    }

    System.Collections.IEnumerator FadeOut()
    {
        while (hintCanvasGroup.alpha > 0)
        {
            hintCanvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }
        hintText.text = "";
    }
}
