﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    void Start() 
    {
        OnSceneLoaded();
    }

    void OnSceneLoaded()
    {
        print("Loading Scene!");
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

            //resets each player's health to max and sets the player to be active
            player.GetComponentInChildren<PlayerHealth>().ResetHealth();
            player.SetActive(true);

            //spawns the player at a random spawn point
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            GameObject spawnPos = spawnPoints[spawnIndex];
            spawnPoints.Remove(spawnPos);
            player.transform.position = spawnPos.transform.position;
        }
    }
}