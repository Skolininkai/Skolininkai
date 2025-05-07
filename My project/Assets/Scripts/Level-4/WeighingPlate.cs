using System.Collections.Generic;
using UnityEngine;

public class WeighingPlate : MonoBehaviour
{
    public float totalMass { get; private set; } = 0f;
    public int ObjectCount => objectsOnPlate.Count;

    [Header("Audio")]
    public AudioClip objectPlacedSound;

    private AudioSource audioSource;
    private readonly List<Rigidbody> objectsOnPlate = new List<Rigidbody>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null && !objectsOnPlate.Contains(rb))
        {
            objectsOnPlate.Add(rb);
            totalMass += rb.mass;

            // Звук при добавлении объекта
            if (objectPlacedSound != null)
            {
                audioSource.PlayOneShot(objectPlacedSound);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null && objectsOnPlate.Contains(rb))
        {
            objectsOnPlate.Remove(rb);
            totalMass -= rb.mass;
        }
    }
}
