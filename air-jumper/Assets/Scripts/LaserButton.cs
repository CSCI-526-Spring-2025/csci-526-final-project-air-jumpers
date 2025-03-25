using UnityEngine;
using System.Collections;

public class LaserButton : MonoBehaviour
{
    public LaserController targetLaser;
    public float disableDuration = 10f;
    private bool isUsed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isUsed && collision.CompareTag("Player"))
        {
            StartCoroutine(DisableLaserTemporarily());
        }
    }

    private IEnumerator DisableLaserTemporarily()
    {
        isUsed = true;

        if (targetLaser != null)
        {
            targetLaser.TurnOff();
        }

        yield return new WaitForSeconds(disableDuration);

        if (targetLaser != null)
        {
            targetLaser.TurnOn();
        }

        isUsed = false; 
    }
}
