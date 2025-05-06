using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public float pickupRange = 3f;
    public KeyCode pickupKey = KeyCode.E;
    public Transform holdPoint;
    private GameObject heldObject;

    public PlayerAbilities abilities;  

    void Update()
    {
        if (Input.GetKeyDown(pickupKey))
        {
            Debug.Log("Trying to pickup object");
            Debug.Log("Current ability = "+ abilities.currentAbility);
            if (heldObject == null)
            {
                TryPickup();
            }
            else
            {  
                DropObject();
            }
        }

        if(abilities.currentAbility == PlayerAbilities.Ability.WalkThroughHotObjects)
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
                // èia kreipiamës á abilities
                if (abilities.currentAbility == PlayerAbilities.Ability.LiftHeavyObjects)
                {
                    heldObject = hit.collider.gameObject;
                    heldObject.GetComponent<Rigidbody>().isKinematic = true;
                    heldObject.transform.position = holdPoint.position;
                    heldObject.transform.parent = holdPoint;
                }
                else
                {
                    Debug.Log("You can't pick up heavy objects right now!");
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
            heldObject = null;
        }
    }
}
