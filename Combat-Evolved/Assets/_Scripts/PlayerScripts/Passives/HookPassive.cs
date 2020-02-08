using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookPassive : PassiveBase
{
    public Hook hook;
    bool isHooking;
    public float hookRange = 8;
    float hookSpeed = 15;
    // when to stop dragging
    float hookVicinity = 1;
    public PlayerController pc;

    private void Start()
    {
        pc = GetComponentInParent<PlayerController>();
        hook.gameObject.SetActive(false);
    }

    public override void ActivatePassive(PlayerController pc)
    {
        StopAllCoroutines();
        hook.transform.parent = null;
        hook.transform.position = transform.position;
        hook.transform.up = -1 * pc.GetGunDirection();
        hook.gameObject.SetActive(true);
        hook.SetVelocity(pc.GetGunDirection().normalized, hookSpeed);
    }

    public void HookTerrain(Vector2 terrainPosition)
    {
        StartCoroutine(HookTerrainEnumerator(terrainPosition));
    }

    IEnumerator HookTerrainEnumerator(Vector2 terrainPosition)
    {
        float originalSpeed = pc.GetMovementSpeed();
        pc.SetCanMove(false);
        pc.SetDirection(Vector2.zero);
        while (pc.transform.position != (Vector3)terrainPosition)
        {
            pc.transform.position = Vector2.MoveTowards(pc.transform.position, terrainPosition, hookSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        pc.SetCanMove(true);
    }

    public void HookPlayer(PlayerController otherpc)
    {
        StartCoroutine(HookPlayerEnumerator(otherpc));
    }

    IEnumerator HookPlayerEnumerator(PlayerController otherpc)
    {
        float originalSpeed = otherpc.GetMovementSpeed();
        otherpc.SetCanMove(false);
        pc.SetDirection(Vector2.zero);
        while ((otherpc.transform.position - transform.position).magnitude > 1)
        {
            otherpc.transform.position = Vector2.MoveTowards(otherpc.transform.position, transform.position, hookSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        otherpc.SetCanMove(true);
    }

    private void OnDestroy()
    {
        Destroy(hook.gameObject);
    }
}
