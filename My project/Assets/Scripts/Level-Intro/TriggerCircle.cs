using UnityEngine;

public class TriggerCircle : MonoBehaviour
{
    public int buttonIndex = 0;
    public string requiredObjectName = "GeraSfera";
    [SerializeField] private Light lightCircle; // Privatus + SerializeField

    void Start()
    {
        if (lightCircle == null) 
        {
            lightCircle = GetComponent<Light>(); // Automatinis priskyrimas
        }
        lightCircle.color = Color.red;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("canPickUp") && other.gameObject.name == requiredObjectName)
        {
            Debug.Log($"Correct object entered: {other.name}");
            lightCircle.color = Color.green;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("canPickUp") && other.gameObject.name == requiredObjectName)
        {
            Debug.Log($"Correct object exited: {other.name}");
            lightCircle.color = Color.red;
        }
    }
}