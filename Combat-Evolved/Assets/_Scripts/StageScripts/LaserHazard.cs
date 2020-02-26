using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHazard : HazardDamage
{
    public GameObject point1;
    public GameObject point2;
    public LineRenderer laser;

    void LateUpdate()
    {
        laser.SetPosition(0, point1.transform.position);
        laser.SetPosition(1, point2.transform.position);

        float distance = (point1.transform.position - point2.transform.position).magnitude;
        RaycastHit2D playerhit = Physics2D.Raycast(point1.transform.position, -transform.up, distance);
        print(playerhit.collider);
        Debug.DrawLine(point1.transform.position, point2.transform.position, Color.blue);
        
        PlayerHealth playerHealth = playerhit.collider.GetComponent<PlayerHealth>();
        if (playerhit && playerHealth) 
        {
            playerHealth.TakeDamage(damage * Time.deltaTime);
            laser.SetPosition(1, playerhit.point);
        }
    }
}
