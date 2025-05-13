using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public float openAngle = 90f;           // Угол открытия двери
    public float speed = 2f;                // Скорость открытия/закрытия
    public KeyCode openKey = KeyCode.E;     // Клавиша открытия
    public Transform doorPivot;             // Объект, который вращается (поворотная точка)
    
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private bool isOpen = false;
    private bool playerNear = false;

    void Start()
    {
        if (doorPivot == null)
        {
            doorPivot = transform; // Если не указано явно, используем текущий объект
        }

        closedRotation = doorPivot.rotation;
        openRotation = Quaternion.Euler(doorPivot.eulerAngles + new Vector3(0, openAngle, 0));
    }

    void Update()
    {
        if (playerNear && Input.GetKeyDown(openKey))
        {
            isOpen = !isOpen;
        }

        if (isOpen)
        {
            doorPivot.rotation = Quaternion.Lerp(doorPivot.rotation, openRotation, Time.deltaTime * speed);
        }
        else
        {
            doorPivot.rotation = Quaternion.Lerp(doorPivot.rotation, closedRotation, Time.deltaTime * speed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
}
