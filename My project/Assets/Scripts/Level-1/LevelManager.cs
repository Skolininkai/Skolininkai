using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public Animator leverAnimator;
    public GameObject Lever;

    void Start()
    {
        Hints.instance.ShowHint("Press the 3 buttons to maneuver over the pillars", 3);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            PressLever();
        }
    }

    public void PressLever()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f) && hit.collider.gameObject == Lever)
        {
            Debug.Log("Lever pressed.");

            leverAnimator.SetBool("ON", true);

            GameObject doorObj = GameObject.FindWithTag("Door");
            if (doorObj != null)
            {
                doorObj.GetComponent<SlidingDoor>().OpenDoor();
            }
        }
    }
}
