using UnityEngine;
using System.Collections;

public class SlidingDoor : MonoBehaviour
{
    public Vector3 openPosition; // Set this in Inspector
    public float openSpeed = 2f;

    private Vector3 closedPosition;
    private bool isOpening = false;

    void Start()
    {
        closedPosition = transform.position; // Save the initial position
    }

    public void OpenDoor()
    {
        if (!isOpening)
        {
            StartCoroutine(SlideDoor(openPosition));
        }
    }

    public IEnumerator SlideDoor(Vector3 targetPosition)
    {
        isOpening = true;
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(closedPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * openSpeed;
            yield return null;
        }
        transform.position = targetPosition;
    }
}
