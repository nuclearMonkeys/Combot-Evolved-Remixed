using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class edgeCheckBurrow : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "outerWall")
        {
            //this.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "outerWall")
        {
            //this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }

    }

}
