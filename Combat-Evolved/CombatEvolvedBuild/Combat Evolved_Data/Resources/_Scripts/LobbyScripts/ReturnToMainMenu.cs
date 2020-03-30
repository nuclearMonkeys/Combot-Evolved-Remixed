using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMainMenu : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.tag == "Bullet") 
        {
            print("hello");
            sceneManager.Instance.backToMainMenu();
            //if(GameObject.Find("Manager").GetComponent<TankSelectionManager>().)
            Destroy(GameObject.Find("Manager"));
        }
    }
}
