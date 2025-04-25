using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraZoomToggle : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float zoomedSize = 20f;
    public float zoomedFOV = 40f;
    public float zoomDuration = 0.5f;
    public float holdDuration = 3f;
    
    private float originalSize;
    private float originalFOV;
    private Coroutine zoomCoroutine;
    private bool isZooming = false;

    void Start()
    {
        isZooming = true;

        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.3f);

        if (virtualCamera.m_Lens.Orthographic)
        {
            originalSize = virtualCamera.m_Lens.OrthographicSize;
        }
        else
        {
            originalFOV = virtualCamera.m_Lens.FieldOfView;
        }

        isZooming = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && !isZooming)
        {
            if (zoomCoroutine != null)
                StopCoroutine(zoomCoroutine);

            zoomCoroutine = StartCoroutine(ZoomRoutine());
        }
    }

    IEnumerator ZoomRoutine()
    {
        isZooming = true;

        if (virtualCamera.m_Lens.Orthographic)
        {
            yield return StartCoroutine(ZoomOrthographic(originalSize, zoomedSize));
            yield return new WaitForSeconds(holdDuration);
            yield return StartCoroutine(ZoomOrthographic(zoomedSize, originalSize));
        }
        else
        {
            yield return StartCoroutine(ZoomPerspective(originalFOV, zoomedFOV));
            yield return new WaitForSeconds(holdDuration);
            yield return StartCoroutine(ZoomPerspective(zoomedFOV, originalFOV));
        }

        isZooming = false;
        zoomCoroutine = null;
    }

    IEnumerator ZoomOrthographic(float fromSize, float toSize)
    {
        float elapsed = 0f;
        while (elapsed < zoomDuration)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(fromSize, toSize, elapsed / zoomDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        virtualCamera.m_Lens.OrthographicSize = toSize;
    }

    IEnumerator ZoomPerspective(float fromFOV, float toFOV)
    {
        float elapsed = 0f;
        while (elapsed < zoomDuration)
        {
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(fromFOV, toFOV, elapsed / zoomDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        virtualCamera.m_Lens.FieldOfView = toFOV;
    }
}
