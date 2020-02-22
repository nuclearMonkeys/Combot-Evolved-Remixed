using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingArea : MonoBehaviour
{
    private HomingBullet homingBullet;

    private void Start()
    {
        homingBullet = GetComponentInParent<HomingBullet>();
    }

    // if player target enters
    private void OnTriggerEnter2D(Collider2D other) 
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController && homingBullet.source != playerController) 
        {
            homingBullet.SetPlayerToTarget(playerController.gameObject);
        }
    }

    // if target leaves
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.Equals(homingBullet.GetPlayerToTarget()))
        {
            homingBullet.SetPlayerToTarget(null);
        }
    }
}
