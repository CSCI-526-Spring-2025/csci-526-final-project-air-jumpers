using UnityEngine;
using System.Collections;

public class CameraOnButtonTrigger : MonoBehaviour
{
    public Transform cameraTarget;         
    public float cameraMoveDuration = 2f;  
    private bool triggered = false;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Button"))
        {
            triggered = true;
            StartCoroutine(MoveCameraToTarget());
        }
    }

    IEnumerator MoveCameraToTarget()
    {
        Time.timeScale = 0f;

        Vector3 startPos = mainCam.transform.position;
        Vector3 targetPos = new Vector3(cameraTarget.position.x, cameraTarget.position.y, startPos.z);
        float elapsed = 0f;

        while (elapsed < cameraMoveDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / cameraMoveDuration);
            mainCam.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        
        yield return new WaitForSecondsRealtime(2f);

        
        elapsed = 0f;
        Vector3 returnPos = new Vector3(transform.position.x, transform.position.y, startPos.z);

        while (elapsed < cameraMoveDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / cameraMoveDuration);
            mainCam.transform.position = Vector3.Lerp(targetPos, returnPos, t);
            yield return null;
        }

        Time.timeScale = 1f;
    }
}
