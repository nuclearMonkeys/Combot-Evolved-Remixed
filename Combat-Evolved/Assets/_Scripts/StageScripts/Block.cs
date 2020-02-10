using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float hitPoints = 3.0f;
    [SerializeField] private ParticleSystem hitParticle;
    [SerializeField] private List<Sprite> spriteList = new List<Sprite>();

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Bullet")) 
        {
            hitPoints--;
            ParticleSystem hitParticleClone = Instantiate(hitParticle, 
                                    other.gameObject.transform.position, 
                                    other.gameObject.transform.rotation);

            hitParticleClone.transform.Rotate(0, 0, 180);

            Destroy(hitParticleClone.gameObject, 0.75f);

            if(hitPoints <= 0.0f)
                Destroy(this.gameObject);
            else {

                // print(other.gameObject.transform.rotation);
                this.GetComponent<SpriteRenderer>().sprite = spriteList[(int)Mathf.Ceil(hitPoints) - 1];
            }
            Destroy(other.gameObject);
        }
    }
}
