using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrowPassive : PassiveBase
{
    bool isBurrowed;
    public float burrowSeconds = 4f;
    GameObject wallCollider;

    public bool isEnabled()
    {
        return isBurrowed;
    }
    public override void ActivatePassive(PlayerController pc)
    {
        wallCollider = pc.gameObject.transform.Find("WallCollider").gameObject;
        if (wallCollider != null && isBurrowed == false)
        {
            StartCoroutine(burrowing(pc));
        }
        
    }

    IEnumerator burrowing(PlayerController pc)
    {
        print("started burrowing");
        isBurrowed = true;
        wallCollider.GetComponent<CircleCollider2D>().isTrigger = true;
        
        yield return new WaitForSeconds(burrowSeconds);
        isBurrowed = false;
        wallCollider.GetComponent<CircleCollider2D>().isTrigger = false;
        print("done burrowing");
    }
}
