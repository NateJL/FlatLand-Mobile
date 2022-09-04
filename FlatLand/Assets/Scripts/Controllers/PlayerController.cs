using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager manager;
    private TextMeshProUGUI debugText;

    [Space(20)]
    private Vector2 screenSize;
    private Vector2 localMousePosition;
    private float mouseDistance;

    public bool canMove;
    public GameObject playerModelParent;
    public GameObject playerPolygon;
    public Material polygonMaterial;
    [Space(10)]
    public PlayerAttackData attackData;
    public Vector3 prevPosition;
    private CharacterController playerCharacterController;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerCharacterController = GetComponent<CharacterController>();
        prevPosition = transform.position;

        playerModelParent = transform.GetChild(1).gameObject;
        playerPolygon = playerModelParent.transform.GetChild(0).gameObject;
        playerPolygon.GetComponent<PolygonGenerator>().SetMaterials(polygonMaterial);
        GeneratePlayerPolygon();

        screenSize = new Vector2(Screen.width, Screen.height);
        debugText = GameObject.FindGameObjectWithTag("Debug").GetComponent<TextMeshProUGUI>();
        canMove = true;
        attackData = new PlayerAttackData();

    }

    /// <summary>
    /// Generate the players polygon based off of how many faces they have.
    /// </summary>
    public void GeneratePlayerPolygon()
    {
        playerPolygon.GetComponent<PolygonGenerator>().GenerateFullPolygon(manager.playerData.edgeCount + 1, manager.playerData.polygonScalar);
    }

    /// <summary>
    /// Clean up the players polygon mesh leftovers when finished.
    /// </summary>
    public void CleanUpPlayerPolygon()
    {
        playerPolygon.GetComponent<PolygonGenerator>().ClearPolygon();
    }

    /// <summary>
    /// Fixed update to handle physics calculations at regular intervals.
    /// </summary>
    void FixedUpdate()
    {
        //CheckGestures();
        if(Input.GetMouseButton(0) 
            && manager.gameOverlayController.overlayMode == ApplicationConstants.GameOverlay.CLOSED 
            && (Input.touchCount == 1 || Application.isEditor))
        {
            CharacterControllerMovement();

            manager.playerData.speed = (transform.position - prevPosition).magnitude;
            prevPosition = transform.position;
        }
        else
        {
            //debugText.SetText(transform.position.ToString());
            //gameObject.GetComponent<Rigidbody>().MovePosition(prevPosition);
        }

        manager.playerData.playerPosition = transform.position;

        CheckAttack();

        float fps = 1.0f / Time.deltaTime;
        debugText.SetText("FPS: " + fps.ToString("F1"));
    }

    private void RigidbodyMovement()
    {
        localMousePosition = new Vector2(Input.mousePosition.x - (screenSize.x / 2), Input.mousePosition.y - (screenSize.y / 2));
        //debugText.SetText(localMousePosition.ToString());
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200f))
        {
            Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            mouseDistance = Vector2.Distance(new Vector2(playerScreenPos.x, playerScreenPos.y), new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            //mouseDistance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(hit.point.x, hit.point.z));
            //if (mouseDistance > ApplicationConstants.PLAYER_MAX_MOUSE_DISTANCE)
            //    mouseDistance = ApplicationConstants.PLAYER_MAX_MOUSE_DISTANCE;
            Vector3 targetDirection = hit.point - playerModelParent.transform.position;
            targetDirection = Vector3.Scale(targetDirection, new Vector3(1, 0, 1));

            float singleStep = manager.playerData.maxSpeed * mouseDistance * Time.deltaTime;
            if (singleStep > ApplicationConstants.PLAYER_MAX_SINGLE_STEP)
                singleStep = ApplicationConstants.PLAYER_MAX_SINGLE_STEP;
            debugText.SetText(mouseDistance.ToString("F3") + "||" + singleStep.ToString("F3"));
            Vector3 newDirection = Vector3.RotateTowards(playerModelParent.transform.forward, targetDirection, singleStep, 0.0f);
            playerModelParent.transform.rotation = Quaternion.LookRotation(newDirection);

            Vector3 increment = Vector3.MoveTowards(transform.position, hit.point, singleStep);
            gameObject.GetComponent<Rigidbody>().MovePosition(increment);
            //playerCharacterController.Move(increment);

        }

        manager.playerData.speed = (transform.position - prevPosition).magnitude;
        prevPosition = transform.position;

        //debugText.SetText(manager.playerData.speed.ToString("F2") + "||" + );
    }

    /// <summary>
    /// Function to handle movement via the players charactercontroller.
    /// </summary>
    private void CharacterControllerMovement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200f))
        {
            Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
            float tempMD = mouseDistance = Vector2.Distance(new Vector2(playerScreenPos.x, playerScreenPos.y), new Vector2(Input.mousePosition.x, Input.mousePosition.y));

            if(mouseDistance > 550)
                return;
            if (mouseDistance > ApplicationConstants.PLAYER_MAX_MOUSE_DISTANCE)
                mouseDistance = ApplicationConstants.PLAYER_MAX_MOUSE_DISTANCE;

            Vector3 targetDirection = hit.point - playerModelParent.transform.position;
            targetDirection = Vector3.Scale(targetDirection, new Vector3(1, 0, 1));

            float singleStep = manager.playerData.maxSpeed * mouseDistance * Time.deltaTime;

            Vector3 newDirection = Vector3.RotateTowards(playerModelParent.transform.forward, targetDirection, singleStep, 0.0f);
            if(!attackData.isAttacking)
                playerModelParent.transform.rotation = Quaternion.LookRotation(newDirection);

            Vector3 offset = new Vector3(hit.point.x, 0, hit.point.z) - new Vector3(transform.position.x, 0, transform.position.z);
            if (offset.magnitude > .1f)
            {
                offset = offset.normalized * manager.playerData.maxSpeed * (mouseDistance/ApplicationConstants.PLAYER_MAX_MOUSE_DISTANCE);
                playerCharacterController.Move(offset * Time.deltaTime);
            }
        }
    }

    public void CheckAttack()
    {
        if(attackData.isAttacking)
        {
            attackData.currentAttackTime += Time.deltaTime;
            if (attackData.currentAttackTime >= attackData.attackDuration)
            {
                attackData.isAttacking = false;
                attackData.currentAttackTime = 0.0f;
                Debug.Log("Finishing attack.");
            }

            float smooth = attackData.currentAttackTime / attackData.attackDuration;
            playerModelParent.transform.Rotate(0, attackData.attackSpeed, 0); 
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void BeginAttack()
    {
        if (attackData.isAttacking)
            return;

        attackData.isAttacking = true;
        attackData.currentAttackTime = 0.0f;
        Debug.Log("Starting attack...");
    }


    /// <summary>
    /// 
    /// </summary>
    private void CheckGestures()
    {
        localMousePosition = new Vector2(Input.mousePosition.x - (screenSize.x / 2), Input.mousePosition.y - (screenSize.y / 2));

        if (Input.GetMouseButtonDown(0))
        {
            if((localMousePosition.x > -100) && (localMousePosition.x < 100) && (localMousePosition.y > -100) && (localMousePosition.y < 100))
            {

            }
        }
    }
}

[System.Serializable]
public class PlayerAttackData
{
    public enum Attack
    {
        NONE = 0,
        SPIN
    }

    public bool isAttacking = false;
    
    public float currentAttackTime = 0.0f;
    public float attackSpeed = 20.0f;
    public float attackDuration = 1.0f;
    public float attackPower = 5.0f;
}
