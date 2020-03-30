using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class edgeCheckBurrow : MonoBehaviour
{
    GameObject wallCollider;
    private void Start()
    {
        wallCollider = this.gameObject.transform.parent.parent.Find("WallCollider").gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "outerWall" && gameObject.GetComponent<BurrowPassive>().isEnabled())
        {
            wallCollider.GetComponent<CircleCollider2D>().isTrigger = false;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "outerWall" && gameObject.GetComponent<BurrowPassive>().isEnabled())
        {
            wallCollider.GetComponent<CircleCollider2D>().isTrigger = true;
        }

    }

}
