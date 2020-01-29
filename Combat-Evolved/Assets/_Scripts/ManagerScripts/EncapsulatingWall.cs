﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Place This Script on the WallMap Prefab!
public class EncapsulatingWall : MonoBehaviour
{
    public GameObject indestructableBlockPrefab;
    // maps a tilemap name to an array of its tiles' positions
    // Key may be ["LeftMap", "RightMap", "TopMap", "BottomMap"]
    // Values are lists of world positions for each tile in the tilemap
    Dictionary<string, List<Vector2>> tilesDictionary;
    // depth of the encapsulation
    int depth;

    void Start()
    {
        depth = 1;
        tilesDictionary = new Dictionary<string, List<Vector2>>();
        Tilemap[] tilemaps = GetComponentsInChildren<Tilemap>();

        if(tilemaps.Length != 4)
        {
            Debug.Log("WallMap Should have 4 Children!! ['LeftMap', 'RightMap', 'TopMap', 'BottomMap']");
            Debug.Break();
        }

        foreach (Tilemap tilemap in tilemaps)
        {
            foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
            {
                if (!tilemap.HasTile(position))
                {
                    continue;
                }
                if (!tilesDictionary.ContainsKey(tilemap.name))
                    tilesDictionary.Add(tilemap.name, new List<Vector2>());
                tilesDictionary[tilemap.name].Add(tilemap.CellToWorld(position));
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Encapsulate());
        }
    }

    IEnumerator Encapsulate()
    {
        // for each side of the four sides
        foreach (KeyValuePair<string, List<Vector2>> tilePair in tilesDictionary)
        {
            // gets the direction to spawn
            Vector2 direction = Vector2.zero;
            if (tilePair.Key.Contains("Left"))
                direction = Vector2.right;
            else if (tilePair.Key.Contains("Right"))
                direction = Vector2.left;
            else if (tilePair.Key.Contains("Top"))
                direction = Vector2.down;
            else if (tilePair.Key.Contains("Bottom"))
                direction = Vector2.up;
            // for each block in the side
            foreach (Vector2 tilePosition in tilePair.Value)
            {
                Vector2 spawnPosition = tilePosition + direction * depth + new Vector2(.5f, .5f);
                // only spwan if no tile exists there
                if (!Physics2D.Raycast(spawnPosition, Vector2.zero, .1f, 1 << LayerManager.BLOCK))
                {
                    RaycastHit2D playerHit = Physics2D.Raycast(spawnPosition, Vector2.zero, .1f, 1 << LayerManager.TANKBODY);
                    // spawning block on a player
                    if (playerHit)
                        playerHit.collider.GetComponent<PlayerHealth>().Die(null);
                    Instantiate(indestructableBlockPrefab, spawnPosition, Quaternion.identity);
                    yield return new WaitForSeconds(.1f);
                }
            }
        }
        depth++;
    }
}