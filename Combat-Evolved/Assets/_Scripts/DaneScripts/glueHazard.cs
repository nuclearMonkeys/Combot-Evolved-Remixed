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
        if (speed == -1 && collision.GetComponentInParent<PlayerController>() != null)
        {
            speed = collision.GetComponentInParent<PlayerController>().GetMovementSpeed();
        }
        if (collision.tag == "Player" && collision.GetComponentInParent<PlayerController>() != null)
        {
            collision.GetComponentInParent<PlayerController>().SetMovementSpeed(speed * speedReducer);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponentInParent<PlayerController>().SetMovementSpeed(speed);
        }
        
    }
}
