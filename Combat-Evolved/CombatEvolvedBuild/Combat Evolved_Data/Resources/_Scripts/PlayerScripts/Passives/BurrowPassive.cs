using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrowPassive : PassiveBase
{
    bool isBurrowed;
    public float burrowSeconds = 4f;
    GameObject wallCollider;
    BoxCollider2D body;

    public float transparency = 0.3f;
    public float speedModifier = 1.2f;

    public bool isEnabled()
    {
        return isBurrowed;
    }
    public override void ActivatePassive(PlayerController pc)
    {
        wallCollider = pc.gameObject.transform.Find("WallCollider").gameObject;
        body = pc.gameObject.transform.Find("Body").GetComponent<BoxCollider2D>();
        if (wallCollider != null && isBurrowed == false)
        {
            StartCoroutine(burrowing(pc));
        }
        
    }

    Color changeAlpha(Color c, float a)
    {
        Color temp = new Color();
        temp.r = c.r;
        temp.g = c.g;
        temp.b = c.b;
        temp.a = a;
        return temp;
    }

    IEnumerator burrowing(PlayerController pc)
    {
        print("started burrowing");
        isBurrowed = true;
        wallCollider.GetComponent<CircleCollider2D>().isTrigger = true;
        body.enabled = false;
        pc.SetCanFire(false);

        float oldSpeed = pc.GetMovementSpeed();
        pc.SetMovementSpeed(oldSpeed * speedModifier);

        List<string> names = new List<string>{ "Body", "Head", "Barrel"};
        foreach (SpriteRenderer s in pc.GetComponentsInChildren<SpriteRenderer>())
        {
            if (names.Contains(s.gameObject.name))
            {
                s.color = changeAlpha(s.color, transparency);
            }
        }
        yield return new WaitForSeconds(burrowSeconds);

        //now the burrowing is done, return everything to normal
        isBurrowed = false;
        wallCollider.GetComponent<CircleCollider2D>().isTrigger = false;
        pc.SetCanFire(true);
        body.enabled = true;

        pc.SetMovementSpeed(oldSpeed);

        foreach (SpriteRenderer s in pc.GetComponentsInChildren<SpriteRenderer>())
        {
            if (names.Contains(s.gameObject.name))
            {
                s.color = changeAlpha(s.color, 1);
            }
        }
        print("done burrowing");
    }
}
