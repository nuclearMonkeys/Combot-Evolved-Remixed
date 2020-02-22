using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class movingBlock : MonoBehaviour
{
    List<Vector3> positions = new List<Vector3>();
    int i = 0;
    public float speed = 1f;
    Transform box;
    Rigidbody2D rb;

    public float lineWidth = 0.2f;
    public Color lineColor = Color.white;
    public Material m;
    float dotSize = 0.3f;

    private void Start()
    {
        box = transform.GetChild(0);
        rb = box.gameObject.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.mass = 0f;
        rb.gravityScale = 0f;
        foreach (Transform child in transform)
        {
            child.position = new Vector3(child.position.x, child.position.y, 0f);
            if (child.position != transform.GetChild(0).transform.position)
            {
                child.localScale = new Vector3(dotSize, dotSize, 1f);
                positions.Add(child.transform.position);
            }
            
        }

        for (int i = 0; i < positions.Count; i++)
        {
            LineRenderer ln = transform.GetChild(i).gameObject.AddComponent<LineRenderer>();
            ln.material = m;
            ln.widthMultiplier = lineWidth;
            ln.positionCount = 2;
            ln.endColor = lineColor;
            ln.startColor = lineColor;
            ln.SetPosition(0, positions[i]);
            if (i == positions.Count - 1)
            {
                print("to first");
                ln.SetPosition(1, positions[0]);
            }
            else
            {
                print("not to first");
                ln.SetPosition(1, positions[i + 1]);
            }
        }
        box.position = positions[0];
        i = 1;
        rb.velocity = (positions[i] - box.position).normalized * speed;

    }

    Vector3 setSpeed()
    {
        Vector3 temp = Vector3.zero;
        if (Math.Round(box.position.x, 1) != Math.Round(positions[i].x, 1))
        {
            temp.x = Mathf.Sign(positions[i].x - box.position.x) * speed;
        }
        if (Math.Round(box.position.y, 1) != Math.Round(positions[i].y, 1))
        {
            temp.y = Mathf.Sign(positions[i].y - box.position.y) * speed;
        }
        return temp;
    }

    private void Update()
    {
        //print("Moving to position " + positions[i] + " at index " + i);
        if((positions[i] - box.position).magnitude <= .1f)
        {
            i++;
            if (i >= positions.Count)
            {
                i = 0;
            }
            rb.velocity = (positions[i] - box.position).normalized * speed;
        }
    }

}
