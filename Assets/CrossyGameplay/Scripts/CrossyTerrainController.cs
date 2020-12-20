using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CrossyTerrainController : MonoBehaviour
{
    public Vector3 currentPosition = Vector3.zero;
    public float blockSize = 1.5f;
    public int requiredRowsFromDistance = 21;

    public BlocksByDistance[] blocksByDistance;

    public bool canRepeatTypes = true;

    private CrossyBlockType lastType = CrossyBlockType.Water;
    
    public void SpawnBlock(GameObject prefab)
    {
        GameObject spawned = GameObject.Instantiate(prefab, currentPosition, Quaternion.identity, this.transform);
        int size = spawned.GetComponent<CrossyBlock>().blockSize;
        //Actualizamos el lastType
        lastType = spawned.GetComponent<CrossyBlock>().type;

        currentPosition.z += blockSize * size;
        //agregar el tamaño del terreno al contador
        createdRowsCount += size;
    }

    public void SpawnBlock(GameObject[] prefabs)
    {
        int randomN = Random.Range(0, prefabs.Length);
        //Spawnear prefab en específico
        SpawnBlock(prefabs[randomN]);
    }

    private void Awake()
    {
        UpdateBlocks();
    }

    private void Update()
    {
        UpdateBlocks();
    }

    private void UpdateBlocks()
    {
        int playerDistance = ScoreManager.Instance.score;
        int requiredRows = playerDistance + requiredRowsFromDistance;

        GameObject[] usablePrefabs = null;

        if (requiredRows > createdRowsCount)
        {
            for (int i = 0; i < blocksByDistance.Length; i++)
            {
                //Si es utilizable
                if (playerDistance >= blocksByDistance[i].minDistance)
                {
                    //Sobreescribir con los prefabs por dificultad
                    usablePrefabs = blocksByDistance[i].blocks;

                    //Si no puedo repetir tipos, remover todos los bloques que son del tipo anterior
                    if (!canRepeatTypes)
                    {
                        usablePrefabs = BlocksByDistance.RemoveByType(usablePrefabs, lastType);
                    }
                }
            }

            SpawnBlock(usablePrefabs);
        }
    }
    
    private int createdRowsCount = 0;
}

[System.Serializable]
public struct BlocksByDistance
{
    public int minDistance;
    public GameObject[] blocks;
    
    public static GameObject[] RemoveByType(GameObject[] allBlocks, CrossyBlockType type)
    {
        List<GameObject> listGO = allBlocks.ToList();
        listGO.RemoveAll(go => go.GetComponent<CrossyBlock>().type == type);
        return listGO.ToArray();
    }
}
