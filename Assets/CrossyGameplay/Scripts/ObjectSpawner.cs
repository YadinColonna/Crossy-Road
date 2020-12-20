using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public float startDelay = 0f;
    public float spawnRate = 3f;
    public GameObject spawnPrefab;
    public Transform spawnPosition;

    public Vector3 objectSpeed = Vector3.zero;
    public float objectLifetime = 10f;

    private void Start()
    {
        StartCoroutine(SpawnAnim());
    }

    public IEnumerator SpawnAnim()
    {
        yield return new WaitForSeconds(startDelay);
        
        while (this.gameObject.activeInHierarchy)
        {
            GameObject spawned = GameObject.Instantiate(spawnPrefab, spawnPosition.position, Quaternion.identity, this.transform);
            MovingObject movingObject = spawned.GetComponent<MovingObject>();

            movingObject.speed = objectSpeed;
            movingObject.lifetime = objectLifetime;

            yield return new WaitForSeconds(spawnRate);
        }
    }
}
