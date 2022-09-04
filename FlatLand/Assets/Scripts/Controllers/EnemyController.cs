using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public CharacterController characterController;
    public bool isRecoiling = false;

    public bool hasTarget = false;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        isRecoiling = false;
        hasTarget = false;
        target = null;

    }

    public void BeginRecoilAnimation(Vector3 recoilVector)
    {
        StartCoroutine("recoilFromAttack", recoilVector);
    }
    

    IEnumerator recoilFromAttack(Vector3 recoilDir)
    {
        float maxRecoilTime = 1.0f;
        float recoilTime = 0.0f;
        isRecoiling = true;
        while (true)
        {
            recoilTime += Time.deltaTime;
            if (recoilTime < maxRecoilTime)
            {
                characterController.Move(recoilDir * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, 0, transform.position.y);
                yield return null;
            }
            else
            {
                isRecoiling = false;
                yield break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Isosceles Hit: " + other.name);
        PlayerController playerController = other.transform.parent.gameObject.GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogWarning("Isosceles failed to get player controller.");
            return;
        }
        if (playerController.attackData.isAttacking)
        {
            Debug.Log("Attacked by player!");
            // Vector3 OD = Destination - Origin
            Vector3 recoilDir = (new Vector3(transform.position.x, 0, transform.position.z) - new Vector3(other.transform.position.x, 0, other.transform.position.z)).normalized * playerController.attackData.attackPower;
            BeginRecoilAnimation(recoilDir);
        }

    }
}
