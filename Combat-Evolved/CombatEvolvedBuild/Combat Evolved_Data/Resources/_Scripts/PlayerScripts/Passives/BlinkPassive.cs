using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkPassive : PassiveBase
{
    public float blinkDistance = 5;

    public override void ActivatePassive(PlayerController pc)
    {
        // blink up to blink distance
        RaycastHit2D hit = Physics2D.Raycast(transform.position, pc.GetDirection(), blinkDistance, (1 << LayerManager.BLOCK));
        float distance = hit ? hit.distance - .5f : blinkDistance;
        pc.transform.position = pc.transform.position + (Vector3)pc.GetDirection().normalized * distance;
    }
}