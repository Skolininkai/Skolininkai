using UnityEngine;

public class TriggerSquare : MonoBehaviour
{
    public int buttonIndex = 0;
    public string requiredObjectName = "GerasKubas";
    [SerializeField] private Light lightSquare; // Privatus + SerializeField

    void Start()
    {
        if (lightSquare == null) 
        {
            lightSquare = GetComponent<Light>(); // Automatinis priskyrimas
        }
        lightSquare.color = Color.red;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("canPickUp") && other.gameObject.name == requiredObjectName)
        {
            Debug.Log($"Correct object entered: {other.name}");
            lightSquare.color = Color.green;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("canPickUp") && other.gameObject.name == requiredObjectName)
        {
            Debug.Log($"Correct object exited: {other.name}");
            lightSquare.color = Color.red;
        }
    }
}