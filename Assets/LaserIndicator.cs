using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserIndicator : MonoBehaviour
{
    public static LaserIndicator instance;

    public List<GameObject> lasers = new List<GameObject>();
    public int currentLaser = -1;

    public UnityAction onLaserUsed;
    public UnityAction onLaserRestored;
    
    void OnEnable()
    {
        if(instance != null)
            Destroy(gameObject);
        else
            instance = this;

        onLaserUsed += OnLaserUsed;
        onLaserRestored += OnLaserRestored;
    }
    
    void OnDisable()
    {
        onLaserUsed -= OnLaserUsed;
        onLaserRestored -= OnLaserRestored;
    }

    void OnLaserUsed()
    {
        if(currentLaser < 4 && currentLaser >= 0)
        {
            lasers[currentLaser].SetActive(false);
        }
        else if(currentLaser < 0)
        {
            currentLaser = 0;
            lasers[currentLaser].SetActive(false);
        }
        else
        {
            currentLaser = 4;
            lasers[currentLaser].SetActive(false);
        }
        currentLaser++;
    }

    void OnLaserRestored()
    {
        if(currentLaser > 0 && currentLaser <= 5)
        {
            lasers[currentLaser-1].SetActive(true);
        }
        else if(currentLaser > 5)
        {
            currentLaser = 4;
            lasers[currentLaser - 1].SetActive(true);
        }
        else
        {
            currentLaser = 0;
            lasers[currentLaser].SetActive(true);
        }
        currentLaser--;
    }
}
