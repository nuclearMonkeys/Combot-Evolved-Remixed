using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriftPassive : PassiveBase
{
    private float driftSpeed = 15;
    private float driftDuration = .75f;
    private bool isDrifting = false;

    public override void ActivatePassive(PlayerController pc)
    {
        Drift(pc);
    }

    public void Drift(PlayerController pc)
    {
        if (!isDrifting)
        {
            StartCoroutine(driftEnumerator(pc));
        }
    }

    IEnumerator driftEnumerator(PlayerController pc)
    {
        isDrifting = true;
        float oldSpeed = pc.GetMovementSpeed();
        pc.SetMovementSpeed(driftSpeed);
        yield return new WaitForSeconds(driftDuration);
        pc.SetMovementSpeed(oldSpeed);
        isDrifting = false;
    }
}
