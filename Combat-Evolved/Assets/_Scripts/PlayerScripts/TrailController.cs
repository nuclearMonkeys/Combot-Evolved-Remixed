using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour {
    public float minSpeed;
    private TrailRenderer trailRenderer;
    private Rigidbody2D rigidbody;

    void Start() 
    {
        trailRenderer = this.GetComponent<TrailRenderer>();
        rigidbody     = this.GetComponent<Rigidbody2D>();
        trailRenderer.emitting = false;
    }

    void Update() 
    {
        if (trailRenderer.emitting) 
        {
            if (rigidbody.velocity.magnitude < minSpeed)
                trailRenderer.emitting = false;
        }
        else 
        {
            if (rigidbody.velocity.magnitude > minSpeed)
                trailRenderer.emitting = true;
        }
    }
}
