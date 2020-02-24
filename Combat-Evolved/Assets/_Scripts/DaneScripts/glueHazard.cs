using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glueHazard : MonoBehaviour
{
    float speed = -1f;

    public float speedReducer = 0.33f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (speed == -1)
        {
            speed = collision.GetComponentInParent<PlayerController>().GetMovementSpeed();
        }
        
        collision.GetComponentInParent<PlayerController>().SetMovementSpeed(speed * speedReducer);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.GetComponentInParent<PlayerController>().SetMovementSpeed(speed);
    }
}
