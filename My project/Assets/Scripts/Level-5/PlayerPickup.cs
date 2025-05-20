using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public float pickupRange = 3f;
    public KeyCode pickupKey = KeyCode.E;
    public Transform holdPoint;
    private GameObject heldObject;

    public PlayerAbilities abilities;

    [Header("Sound Settings")]
    public AudioSource audioSource;
    public AudioClip pickupSound;
    public AudioClip dropSound;

    void Update()
    {
        if (Input.GetKeyDown(pickupKey))
        {
            Debug.Log("Trying to pickup object");
            Debug.Log("Current ability = " + abilities.currentAbility);
            if (heldObject == null)
            {
                TryPickup();
            }
            else
            {
                DropObject();
            }
        }

        if (abilities.currentAbility == PlayerAbilities.Ability.WalkThroughHotObjects)
        {
            DropObject();
        }

        Debug.DrawRay(transform.position, transform.forward, Color.yellow, pickupRange);
    }

    void TryPickup()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange))
        {
            if (hit.collider.CompareTag("Heavy"))
            {
                Debug.Log("Objektas atitinka heavy klase");
                // �ia kreipiam�s � abilities
                if (abilities.currentAbility == PlayerAbilities.Ability.LiftHeavyObjects)
                {
                    heldObject = hit.collider.gameObject;
                    heldObject.GetComponent<Rigidbody>().isKinematic = true;
                    heldObject.transform.position = holdPoint.position;
                    heldObject.transform.parent = holdPoint;

                    // Проигрываем звук поднятия
                    if (audioSource != null && pickupSound != null)
                    {
                        audioSource.PlayOneShot(pickupSound);
                    }
                }
                else
                {
                    Hints.instance.ShowHint("With this ability you cant pickup heavy objects", 2);
                }
            }
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject.transform.parent = null;

            // Проигрываем звук отпускания
            if (audioSource != null && dropSound != null)
            {
                audioSource.PlayOneShot(dropSound);
            }

            heldObject = null;
        }
    }
}
