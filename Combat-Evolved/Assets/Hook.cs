using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    LineRenderer lr;
    public HookPassive hp;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
    }

    public void SetVelocity(Vector2 direction, float speed)
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position);
        rb.velocity = direction * speed;
    }

    private void Update()
    {
        if ((transform.position - hp.transform.position).magnitude > hp.hookRange)
        {
            gameObject.SetActive(false);
            return;
        }
        RaycastHit2D playerhit = Physics2D.Raycast(transform.position, rb.velocity.normalized, .1f, 1 << LayerManager.TANKBODY);
        if (playerhit)
        {
            PlayerController otherpc = playerhit.collider.GetComponentInParent<PlayerController>();
            if (otherpc != hp.pc)
            {
                hp.HookPlayer(otherpc);
                gameObject.SetActive(false);
            }
        }
        else
        {
            RaycastHit2D blockhit = Physics2D.Raycast(transform.position, rb.velocity.normalized, .1f, 1 << LayerManager.BLOCK);
            if (blockhit)
            {
                hp.HookTerrain(blockhit.point + (Vector2)rb.velocity.normalized * -1f);
                gameObject.SetActive(false);
            }
        }

        lr.SetPosition(0, hp.transform.position);
        lr.SetPosition(1, transform.position);
    }
}
