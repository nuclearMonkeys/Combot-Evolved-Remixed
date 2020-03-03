using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveBase : MonoBehaviour
{
    public float cooldown = 5;
    public int activateStaminaUsage = 2;
    private float dashSpeed = 15;
    private float dashDuration = .75f;
    private bool isDashing = false;

    // Default passive is the dash
    public virtual void ActivatePassive(PlayerController pc)
    {
        Dash(pc);
    }

    public void Dash(PlayerController pc)
    {
        if (!isDashing)
        {
            StartCoroutine(dashEnumerator(pc));
        }
    }

    IEnumerator dashEnumerator(PlayerController pc)
    {
        isDashing = true;
        pc.SetCanMove(false);
        float oldSpeed = pc.GetMovementSpeed();
        pc.SetMovementSpeed(dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        pc.SetMovementSpeed(oldSpeed);
        pc.SetCanMove(true);
        isDashing = false;
    }
}
