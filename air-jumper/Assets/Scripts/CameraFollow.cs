using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform playerManager;  
    public float followSpeed = 5f;   
    public Vector3 offset = new Vector3(0, 2, -10);

    public Camera mainCamera;
    public float zoomOutSize = 20f;
    public float zoomInSize = 10f;
    public float zoomDuration = 3f;
    public float zoomSpeed = 1f;
    public float zoomSpeedStart = 1f;


    private bool isZooming = false;

    void Start()
    {
        // StartCoroutine(StartZoomEffect());
    }

    void LateUpdate()
    {
        if (playerManager != null)
        {
            Vector3 targetPosition = playerManager.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.V) && !isZooming)
        {
            StartCoroutine(ZoomEffect(zoomOutSize, zoomDuration, zoomInSize));
        }
    }

    IEnumerator StartZoomEffect()
    {
        yield return StartCoroutine(ChangeZoom(100));
        yield return new WaitForSeconds(5f);        
        yield return StartCoroutine(ChangeZoom(zoomInSize));
    }

    IEnumerator ZoomEffect(float outSize, float duration, float inSize)
    {
        isZooming = true;
        yield return StartCoroutine(ChangeZoomStart(outSize));
        yield return new WaitForSeconds(duration);
        yield return StartCoroutine(ChangeZoomStart(inSize));
        isZooming = false;
    }

    IEnumerator ChangeZoomStart(float targetSize)
    {
        float startSize = mainCamera.orthographicSize;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * zoomSpeedStart;
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
            yield return null;
        }
        mainCamera.orthographicSize = targetSize;
    }
    IEnumerator ChangeZoom(float targetSize)
    {
        float startSize = mainCamera.orthographicSize;
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * zoomSpeed;
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
            yield return null;
        }
        mainCamera.orthographicSize = targetSize;
    }
}
