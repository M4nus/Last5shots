using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrigger : MonoBehaviour
{
    private List<GameObject> bulletsHitted = new List<GameObject>();
    private PlayerController playerController;

    private void OnEnable()
    {
        playerController = GameObject.FindGameObjectWithTag("James").GetComponent<PlayerController>();
        playerController.onRewind += DisableColliding;

    }

    private void OnDisable()
    {
        bulletsHitted.Clear();
        playerController.onRewind -= DisableColliding;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Good"))
        {
            bulletsHitted.Add(collision.gameObject);
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("James"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void DisableColliding()
    {
        foreach(GameObject bullet in bulletsHitted)
        {
            if((playerController.bullets[playerController.bullets.Count - 1].GetInstanceID() - 4) ==  bullet.GetInstanceID())
            {
                GameObject redBullet = transform.parent.gameObject;
                GameObject rewindRed = ObjectPooler.sharedInstance.GetPooledObject("RewindRed");
                if(rewindRed != null)
                {
                    rewindRed.transform.position = gameObject.transform.position;
                    rewindRed.transform.rotation = gameObject.transform.rotation;
                    rewindRed.SetActive(true);
                    var shRewind = rewindRed.GetComponent<ParticleSystem>().shape;
                    shRewind.scale = new Vector3(redBullet.transform.localScale.x / 10f, 0.1f, 0.1f);
                }
                gameObject.SetActive(false);
                break;
            }
        }
    }
}
