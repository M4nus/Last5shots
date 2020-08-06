using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject James;

    public GameObject bulletSpawnPoint;

    public float walkRadius = 5f;
    public float shootDelay = 2f;

    // Start is called before the first frame update
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        James = GameObject.FindGameObjectWithTag("James");
        StartCoroutine(ControlAI());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Good"))
        {
            GameObject deathParticles = ObjectPooler.sharedInstance.GetPooledObject("DeathParticles");
            if(deathParticles != null)
            {
                deathParticles.transform.position = gameObject.transform.position;
                deathParticles.transform.rotation = gameObject.transform.rotation;
                deathParticles.SetActive(true);
            }
            gameObject.SetActive(false);

            GameObject prisoner = ObjectPooler.sharedInstance.GetPooledObject("Prisoner");
            if(prisoner != null)
            {
                prisoner.transform.position = RandomNavmeshLocation(20);
                prisoner.transform.rotation = gameObject.transform.rotation;
                prisoner.SetActive(true);
            }
        }
    }


    public IEnumerator ControlAI()
    {
        while(gameObject.activeSelf)
        {
            yield return StartCoroutine(CheckForPlayer());
        }
        yield return null; 
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if(NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public IEnumerator MoveAI(Vector3 destination)
    {
        //do
        {
            agent.SetDestination(destination);
            yield return null;
        } //while(Vector3.Distance(agent.transform.position, RandomNavmeshLocation(15f)) > Random.RandomRange(0f, 2f));
    }

    public IEnumerator CheckForPlayer()
    {
        if(Vector3.Distance(transform.position, James.transform.position) < 4f)
        {
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(Shoot());
            yield return new WaitForSeconds(shootDelay);
            yield return StartCoroutine(MoveAI(RandomNavmeshLocation(10f)));
        }
        else
        {
            yield return StartCoroutine(MoveAI(James.transform.position));
        }
        yield return null;
    }

    public IEnumerator Shoot()
    {
        int layerMask = 1 << 14;
        layerMask = ~layerMask;

        //transform.LookAt(James.transform.position);

        RaycastHit hit;
        if(Physics.Raycast(bulletSpawnPoint.transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            GameObject bullet = ObjectPooler.sharedInstance.GetPooledObject("RedBullet");
            if(bullet != null)
            {
                bullet.transform.position = (bulletSpawnPoint.transform.position + hit.point) / 2;
                bullet.transform.rotation = transform.rotation;
                bullet.transform.rotation *= Quaternion.Euler(0, 90f, 0);
                bullet.SetActive(true);
                NewScale(bullet, hit.distance);
                ParticleShape(bullet);
            }
            GameObject hitExplosion = ObjectPooler.sharedInstance.GetPooledObject("HitExplosion");
            if(hitExplosion != null)
            {
                hitExplosion.transform.position = hit.point;
                hitExplosion.transform.rotation = Quaternion.identity;
                hitExplosion.SetActive(true);
            }
            if(hit.collider.gameObject.tag != "RedBullet")
            {
                GameObject wallMark = ObjectPooler.sharedInstance.GetPooledObject("WallMark");
                if(wallMark != null)
                {
                    wallMark.transform.position = hit.point + Vector3.forward * 0f;
                    wallMark.transform.rotation = Quaternion.LookRotation(hit.normal);
                    wallMark.SetActive(true);
                }
            }
        }
        yield return null;
    }


    public void NewScale(GameObject theGameObject, float newSize)
    {
        theGameObject.transform.localScale = new Vector3((newSize + 0.1f) * 10f, 1f, 1f);
    }

    public void ParticleShape(GameObject parent)
    {
        //electricity = parent.GetComponentsInChildren<ParticleSystem>()[0];
        //laserSpawn = parent.GetComponentsInChildren<ParticleSystem>()[1];
        //var shElectricity = electricity.shape;
        //var shLaser = laserSpawn.shape;

        //shElectricity.scale = new Vector3(parent.transform.localScale.x / 10f, 0.2f, 0.2f);
        //shLaser.scale = new Vector3(parent.transform.localScale.x / 10f, 0.2f, 0.2f);
    }
}
