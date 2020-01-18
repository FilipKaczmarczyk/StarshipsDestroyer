using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRoomSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct RandomSpawner
    {
        public string name;
        public SpawnerData spawnerData;
    }

    public GridController grid;
    public RandomSpawner[] spawnerData;


    void Start()
    {
        //grid = GetComponentInChildren<GridController>();
    }

    public void InitializeObjectSpawining()
    {
        foreach(RandomSpawner rs in spawnerData)
        {
            SpawnObjects(rs);
        }
    }

    void SpawnObjects(RandomSpawner data)
    {
        if(PlayerController.currentLevel < 3)
        {
            int randomIteration = Random.Range(data.spawnerData.minSpawn, data.spawnerData.maxSpawn + 1);
            if (data.name != "Enemy3")
            {
                for (int i = 0; i < randomIteration; i++)
                {
                    int randomPos = Random.Range(0, grid.availablePoints.Count - 1);
                    GameObject go = Instantiate(data.spawnerData.itemToSpawn, grid.availablePoints[randomPos], Quaternion.identity, transform) as GameObject;
                    grid.availablePoints.RemoveAt(randomPos);
                }
            }
            else
            {
                for (int i = 0; i < randomIteration; i++)
                {
                    int randomPos = Random.Range(0, grid.availablePoints.Count - 1);
                    GameObject go = Instantiate(data.spawnerData.itemToSpawn, grid.availablePoints[randomPos], Quaternion.identity, transform) as GameObject;
                    grid.availablePoints.RemoveAt(randomPos);
                }
            }

        }
        else
        {
            int randomIteration = Random.Range(data.spawnerData.minSpawn + PlayerController.currentLevel-2, data.spawnerData.maxSpawn + PlayerController.currentLevel - 2);
            if (data.name != "Enemy")
            {
                for (int i = 0; i < randomIteration; i++)
                {
                    int randomPos = Random.Range(0, grid.availablePoints.Count - 1);
                    GameObject go = Instantiate(data.spawnerData.itemToSpawn, grid.availablePoints[randomPos], Quaternion.identity, transform) as GameObject;
                    grid.availablePoints.RemoveAt(randomPos);
                }
            }
            else
            {
                for (int i = 0; i < randomIteration; i++)
                {
                    int randomPos = Random.Range(0, grid.availablePoints.Count - 1);
                    GameObject go = Instantiate(data.spawnerData.itemToSpawn, grid.availablePoints[randomPos], Quaternion.identity, transform) as GameObject;
                    grid.availablePoints.RemoveAt(randomPos);
                }
            }
        }
    }

}
