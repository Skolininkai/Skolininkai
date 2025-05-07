using UnityEngine;

public class ScaleManager : MonoBehaviour
{
    public WeighingPlate leftPlate;
    public WeighingPlate rightPlate;

    public int requiredObjectCount = 3;

    public AudioClip balancedSound;
    public AudioClip leftHeavySound;
    public AudioClip rightHeavySound;

    private AudioSource audioSource;
    private string lastState = ""; // Чтобы не повторять звук при одинаковом состоянии

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("На объекте нет AudioSource! Добавь компонент AudioSource.");
        }
    }

    void Update()
    {
        int totalCount = leftPlate.ObjectCount + rightPlate.ObjectCount;

        if (totalCount == requiredObjectCount)
        {
            float diff = leftPlate.totalMass - rightPlate.totalMass;

            if (Mathf.Abs(diff) < 0.01f && lastState != "balanced")
            {
                Debug.Log("✅ Весы в равновесии");
                PlaySound(balancedSound);
                lastState = "balanced";
            }
            else if (diff > 0 && lastState != "left")
            {
                Debug.Log("⬅️ Левая чаша тяжелее");
                PlaySound(leftHeavySound);
                lastState = "left";
            }
            else if (diff < 0 && lastState != "right")
            {
                Debug.Log("➡️ Правая чаша тяжелее");
                PlaySound(rightHeavySound);
                lastState = "right";
            }
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
