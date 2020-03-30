using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPassive : PassiveBase
{
    public PushArea pushArea;
    bool isPushing;
    public float pushRange = 8;
    float pushSpeed = 10;
    public PlayerController playerController;

    private void Start() 
    {
        playerController = GetComponentInParent<PlayerController>();
        pushArea.gameObject.SetActive(false);
    }

    public override void ActivatePassive(PlayerController pc) 
    {
        StopAllCoroutines();
        pushArea.transform.position = transform.position;
        pushArea.gameObject.SetActive(true);
        // Hook.transform.up = -1
    }

    public void PushPlayer(PlayerController otherpc) 
    {
        StartCoroutine(PushPlayerEnumerator(otherpc));
    }

    IEnumerator PushPlayerEnumerator(PlayerController otherpc) 
    {
        otherpc.SetCanMove(false);
        Vector3 dir = (otherpc.transform.position - transform.position).normalized;

        while ((otherpc.transform.position - transform.position).magnitude < pushRange) 
        {
            otherpc.transform.position = Vector2.MoveTowards(otherpc.transform.position, dir * pushRange, Time.deltaTime * pushSpeed);
            yield return new WaitForEndOfFrame();
        }

        if(otherpc)
            otherpc.SetCanMove(true);
        pushArea.gameObject.SetActive(false);
    }
}
