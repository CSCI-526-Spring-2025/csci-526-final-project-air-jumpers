using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public List<GameObject> laserVisuals;
    public Collider2D laserCollider; 

    public void TurnOn()
    {
        foreach (var laser in laserVisuals)
        {
            if (laser != null)
                laser.SetActive(true);
        }
    }

    public void TurnOff()
    {
        foreach (var laser in laserVisuals)
        {
            if (laser != null)
                laser.SetActive(false);
        }
        laserCollider.enabled = false;
    }
}
