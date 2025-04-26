using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraFocusL2 : MonoBehaviour
{
    [Header("References")]
    public CinemachineVirtualCamera playerCam; 
    public CinemachineVirtualCamera focusCam;  
    public Transform player;                    
    public Transform target;                  

    [Header("Focus Settings")]
    public float focusDuration = 3.5f;          

    private bool hasTriggered = false;

    private void Start()
    {
        playerCam.Follow = player;
        focusCam.Follow = target;
        if (playerCam.Priority <= focusCam.Priority)
            playerCam.Priority = focusCam.Priority + 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(FocusRoutine());
        }
    }

    private IEnumerator FocusRoutine()
    {
        focusCam.Priority = playerCam.Priority + 1;

        yield return new WaitForSeconds(focusDuration);

        playerCam.Priority = focusCam.Priority + 1;
    }
}
