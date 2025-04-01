using UnityEngine;
using System.Collections;

public class CameraFocusTrigger : MonoBehaviour
{
    public Transform player;           
    public Transform cameraTarget;     
    public float triggerX = 10f;       
    public float cameraMoveDuration = 2f;

    private bool hasTriggered = false;
    private Camera mainCam;
    private Vector3 originalCameraPos;

    void Start()
    {
        mainCam = Camera.main;
        originalCameraPos = mainCam.transform.position;
    }

    void Update()
    {
        if (!hasTriggered && player.position.x >= triggerX && player.position.y > -8.7f && player.position.y < -0.8f)
        {
            hasTriggered = true;
            StartCoroutine(FocusCameraOnce());
        }
    }

    IEnumerator FocusCameraOnce()
    {
        
        Time.timeScale = 0f;

        
        float elapsed = 0f;
        Vector3 startPos = mainCam.transform.position;
        Vector3 targetPos = new Vector3(cameraTarget.position.x, cameraTarget.position.y, startPos.z);

        while (elapsed < cameraMoveDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / cameraMoveDuration);
            mainCam.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        
        yield return new WaitForSecondsRealtime(2f);

        elapsed = 0f;
        Vector3 followTargetPos = new Vector3(player.position.x, player.position.y, mainCam.transform.position.z);
        while (elapsed < cameraMoveDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / cameraMoveDuration);
            mainCam.transform.position = Vector3.Lerp(targetPos, followTargetPos, t);
            yield return null;
        }

       
        Time.timeScale = 1f;
    }
}
