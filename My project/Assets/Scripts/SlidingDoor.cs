using UnityEngine;
using System.Collections;

public class SlidingDoor : MonoBehaviour
{
    public Vector3 openPosition; // Set this in Inspector
    public float openSpeed = 2f;

    private Vector3 closedPosition;
    private bool isOpening = false;
    private Transform doorTransform;

    void Start()
    {
        doorTransform = transform;
        closedPosition = doorTransform.position; // Save the initial position
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
        Vector3 startPos = doorTransform.position;
        float distance = Vector3.Distance(startPos, targetPosition);
        float duration = distance / openSpeed;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            doorTransform.position = Vector3.Lerp(closedPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        doorTransform.position = targetPosition;
    }
}
