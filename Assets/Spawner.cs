using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    public IEnumerator Spawn()
    {
        while(true)
        {
            int rand = Random.Range(0, 2);
            if(rand == 0)
                SpawnEnemy("Prisoner");
            else if(rand == 1)
                SpawnEnemy("SphereChaser");
            else
                SpawnEnemy("Prisoner");
            yield return new WaitForSeconds(10f);
        }
    }

    public void SpawnEnemy(string enemyTag)
    {
        GameObject enemy = ObjectPooler.sharedInstance.GetPooledObject(enemyTag);
        if(enemy != null)
        {
            enemy.transform.position = RandomNavmeshLocation(10);
            enemy.transform.rotation = gameObject.transform.rotation;
            enemy.SetActive(true);
        }
    }
    

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += new Vector3(10, 0, -10);
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if(NavMesh.SamplePosition(randomDirection, out hit, radius, areaMask:0))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
