using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [ShowOnly]public bool playerNearby;
    [ShowOnly]public Vector3 openAngle = new Vector3(0f, 90f, 0f);
    [ShowOnly] public Vector3 closedAngle = Vector3.zero;
    [ShowOnly]public Vector3 currentAngle;
    public float swingTime = 1.0f;
    public GameObject doorHinge;


    // Start is called before the first frame update
    void Start()
    {
        currentAngle = transform.eulerAngles;
        doorHinge = transform.GetChild(2).gameObject;
        closedAngle = currentAngle;
        openAngle = closedAngle + new Vector3(0, 90, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            playerNearby = true;
            StartCoroutine("OpenDoorAnimation");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerNearby = false;
            StartCoroutine("CloseDoorAnimation");
        }
    }

    IEnumerator OpenDoorAnimation()
    {
        float startTime = Time.time;
        bool isOpen = false;
        while(playerNearby && !isOpen)
        {
            float percentComplete = (Time.time - startTime) / swingTime;
            doorHinge.transform.rotation = Quaternion.Euler(Vector3.Slerp(closedAngle, openAngle, percentComplete));
            if (percentComplete >= 1)
                isOpen = true;

            yield return null;
        }
    }

    IEnumerator CloseDoorAnimation()
    {
        float startTime = Time.time;
        bool isClosed = false;
        while (!playerNearby && !isClosed)
        {
            float percentComplete = (Time.time - startTime) / swingTime;
            doorHinge.transform.rotation = Quaternion.Euler(Vector3.Slerp(openAngle, closedAngle, percentComplete));
            if (percentComplete >= 1)
                isClosed = true;

            yield return null;
        }
    }
}
