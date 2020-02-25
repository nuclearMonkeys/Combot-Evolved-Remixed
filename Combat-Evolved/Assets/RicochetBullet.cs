using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetBullet : BulletBase
{
    public int bounces = 1;

    public static Vector2 GetNormalVector(GameObject obj, GameObject wall)
    {
        if (wall.CompareTag("Border"))
        {
            if (wall.name.Contains("Top"))
            {
                return Vector3.down;
            }
            else if (wall.name.Contains("Bottom"))
            {
                return Vector3.up;
            }
            else if (wall.name.Contains("Left"))
            {
                return Vector3.right;
            }
            else if (wall.name.Contains("Right"))
            {
                return Vector3.left;
            }
            return Vector3.zero;
        }
        else
        {
            Vector3 worldPosition = wall.transform.position;
            float deltaX = Mathf.Abs(obj.transform.position.x - worldPosition.x);
            float deltaY = Mathf.Abs(obj.transform.position.y - worldPosition.y);
            if (deltaX > deltaY)
            {
                if (obj.transform.position.x > worldPosition.x)
                    return Vector2.right;
                else
                    return Vector2.left;
            }
            else
            {
                if (obj.transform.position.y > worldPosition.y)
                    return Vector2.up;
                else
                    return Vector2.down;
            }
        }
    }

    public override bool ExtendedOnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerManager.BLOCK)
        {
            bounces--;
            if (bounces >= 0)
            {
                // if bouncing, dont check other collisions
                float speed = rb.velocity.magnitude;
                rb.velocity = speed * Vector2.Reflect(rb.velocity.normalized, GetNormalVector(gameObject, other.gameObject));
                return false;
            }
        }
        return true;
    }
}
