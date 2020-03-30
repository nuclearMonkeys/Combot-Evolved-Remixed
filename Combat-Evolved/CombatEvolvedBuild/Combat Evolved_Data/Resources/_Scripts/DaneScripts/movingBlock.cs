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
    public float respawnTime = 4f;

    private float distance;

    public float lineWidth = 0.2f;
    public Color lineColor = Color.white;
    public Material m;
    float dotSize = 0.3f;
    public bool canRespawn = true;
    private bool hasRespawned = false;

    private GameObject go;

    private void Start()
    {
        box = transform.GetChild(0);
        go = GameObject.Instantiate(transform.GetChild(0).gameObject);
        go.SetActive(false);
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
                ln.SetPosition(1, positions[0]);
            }
            else
            {
                ln.SetPosition(1, positions[i + 1]);
            }
        }
        box.position = positions[0];
        i = 1;
        rb.velocity = (positions[i] - box.position).normalized * speed;
        distance = Vector3.Distance(positions[i], box.position);
    }

    private void Update()
    {
        if (transform.childCount == positions.Count)
        {
            if (canRespawn == false)
            {
                Destroy(this.gameObject);
                return;
            }
            else if (canRespawn == true && hasRespawned == false)
            {
                hasRespawned = true;
                StartCoroutine(respawn());
                
            }
        }
        else
        {
            if (distance >= Vector3.Distance(positions[i], box.position))
            {
                rb.velocity = (positions[i] - box.position).normalized * speed;
            }
            if((positions[i] - box.position).magnitude <= .1f)
            {
                i++;
                if (i >= positions.Count)
                {
                    i = 0;
                }
                rb.velocity = (positions[i] - box.position).normalized * speed;
            }
            distance = Vector3.Distance(positions[i], box.position);
        
            if (box.rotation.z != 0)
            {
                box.eulerAngles = new Vector3(box.rotation.x, box.rotation.y, 0f);
            }
        }
        
    }

    private IEnumerator respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        GameObject g = GameObject.Instantiate(go, positions[0], Quaternion.identity);
        g.SetActive(true);
        g.transform.parent = this.gameObject.GetComponent<Transform>();
        box = g.transform;
        rb = box.gameObject.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.mass = 0f;
        rb.gravityScale = 0f;
        hasRespawned = false;
        i = 0;

    }

}
