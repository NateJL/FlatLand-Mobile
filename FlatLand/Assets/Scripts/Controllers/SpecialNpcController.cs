using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialNpcController : MonoBehaviour
{
    private GameManager manager;
    public bool showGizmos;
    [Space(15)]
    public string npcName;

    [Header("Movement Data")]
    public float speed;
    public float distanceThreshold = 1;
    public GameObject waypointParent;
    public Transform currentTarget;
    [Space(20)]
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        currentTarget = waypointParent.GetComponent<Waypoint>().GetNextWaypoint(null);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = Vector3.Distance(currentTarget.position, transform.position);
        if(distance < distanceThreshold)
        {
            currentTarget = waypointParent.GetComponent<Waypoint>().GetNextWaypoint(currentTarget);
        }

        transform.LookAt(currentTarget);
        rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player Entered Trigger.");
        }
    }
}
