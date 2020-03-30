using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : BulletBase
{
    GameObject playerToTarget;
    public float honeStrength = 4;

    public void SetPlayerToTarget(GameObject target)
    {
        playerToTarget = target;
    }

    public GameObject GetPlayerToTarget()
    {
        return playerToTarget;
    }

    private void Update()
    {
        if(playerToTarget != null)
        {
            Vector3 transitionDirection = Vector3.Slerp(
                rb.velocity.normalized,
                (playerToTarget.transform.position - transform.position).normalized,
                honeStrength * Time.deltaTime);
            SetDirection(transitionDirection);
        }
    }
}
