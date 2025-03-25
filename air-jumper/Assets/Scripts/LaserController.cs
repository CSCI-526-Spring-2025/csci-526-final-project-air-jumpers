using UnityEngine;

public class LaserController : MonoBehaviour
{
    public GameObject laserVisual; 
    public Collider2D laserCollider; 

    public void TurnOn()
    {
        laserVisual.SetActive(true);
        laserCollider.enabled = true;
    }

    public void TurnOff()
    {
        laserVisual.SetActive(false);
        laserCollider.enabled = false;
    }
}
