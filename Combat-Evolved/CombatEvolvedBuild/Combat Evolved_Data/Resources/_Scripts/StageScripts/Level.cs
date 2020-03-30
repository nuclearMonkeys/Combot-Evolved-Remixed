using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
    public GameObject cratePrefab;
    Bounds levelBounds;

    float spawnOffset = 5;

    void Start() 
    {
        SetPlayerSpawn();
        SetCrateSpawn();
    }

    void SetPlayerSpawn()
    {
        // find all players
        List<GameObject> players = TankSelectionManager.instance.players;
        //resets the current living player count to the proper max
        sceneManager.Instance.currentLiving = players.Count;
        // find spawn points
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("spawnPoint");
        List<GameObject> spawnPoints = new List<GameObject>(spawns);

        //clears the camera of players
        if(CameraController.instance)
            CameraController.instance.targets.Clear();
        foreach (GameObject player in players)
        {
            //adds each player to the camera
            if (CameraController.instance)
                CameraController.instance.targets.Add(player.transform);

            // Resets player
            player.GetComponent<PlayerController>().Reset();

            //spawns the player at a random spawn point
            if(spawnPoints.Count >  0)
            {
                int spawnIndex = Random.Range(0, spawnPoints.Count);
                GameObject spawnPos = spawnPoints[spawnIndex];
                spawnPoints.Remove(spawnPos);
                player.transform.position = (Vector2)spawnPos.transform.position;
            }
        }
    }

    void SetCrateSpawn() 
    {
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("ItemDropPoint");
        List<GameObject> cratePoints = new List<GameObject>(spawns);

        foreach (GameObject point in cratePoints) {
            Instantiate(cratePrefab, point.transform.position, Quaternion.identity);
        }
    }
}
