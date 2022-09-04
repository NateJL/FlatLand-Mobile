using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrierController : MonoBehaviour
{
    public GameOverlayController overlay;
    private PoolManager objectPool;
    [Space(10)]
    [Header("Crier Data")]
    public NpcDialogueData dialogueData;

    // Start is called before the first frame update
    void Start()
    {
        objectPool = GameManager.manager.poolManager;
    }

    /// <summary>
    /// Trigger dialogue box when player walks into trigger area.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (overlay == null)
            {
                overlay = GameManager.manager.gameOverlayController;
                if (overlay == null)
                    Debug.LogWarning("CrierController still failed to get game manager overlay.");
            }

            Debug.Log("Player Entered Crier Trigger.");
            GameManager.manager.gameOverlayController.OpenDialogueOverlay(dialogueData);
        }
    }
}
