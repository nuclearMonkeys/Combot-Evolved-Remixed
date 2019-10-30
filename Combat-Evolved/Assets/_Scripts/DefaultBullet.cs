using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public Vector3 angle;
    
    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() 
    {
        rb.velocity = transform.right * speed;
        // rb.AddForce(transform.forward * speed);
        // rb.AddForce(Quaternion.AngleAxis(angle, Vector3.forward) * );
    }
}
