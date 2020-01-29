using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    public GameObject firePoint;
    private LineRenderer line;
    private bool is_enabled;

    private int cooldown = 0;
    [System.NonSerialized] int maxCooldown = 5;

    void Start()
    {
        firePoint = this.GetComponentsInChildren<Transform>()[4].gameObject;
        line = this.GetComponent<LineRenderer>();
    }

    // I've spent way too long to make dotted lines
    // Come back to this later
    void Update()
    {
        if(cooldown > 0)
            cooldown--;

        line.enabled = is_enabled;
        if (is_enabled) 
        {
            RaycastHit2D hit = Physics2D.Raycast(firePoint.transform.position, firePoint.transform.right);
            line.SetPosition(0, firePoint.transform.position);
            line.SetPosition(1, hit.point);
        }
    }

    public void Aim() 
    {
        if (cooldown > 0)
            return;
        is_enabled = !is_enabled;
        cooldown = maxCooldown;
        print(is_enabled);
    }
}
