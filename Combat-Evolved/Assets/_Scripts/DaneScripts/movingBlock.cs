using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingBlock : MonoBehaviour
{

    List<Transform> positions = new List<Transform>();
    //List<LineRenderer>() lines;
    Transform block;
    int i = 0;

    public float speed = 1f;
    public Material defaultLine;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            positions.Add(child);
        }
        block = positions[0];
        positions.RemoveAt(0);
        for (int i = 0; i < positions.Count; ++i)
        {
            print("in loop");
            LineRenderer ln = gameObject.AddComponent<LineRenderer>();
                //.
            ln.material = defaultLine;
            ln.SetPosition(0, positions[i].position);
            Vector3 secondPos;
            if (i < positions.Count)
            {
                secondPos = positions[i + 1].position;
            }
            else
            {
                secondPos = positions[0].position;
            }
            ln.SetPosition(1, secondPos);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if ((int)(block.position.x * 10) == (int)(positions[i].position.x * 10) && (int)(block.position.y * 10) == (int)(positions[i].position.y * 10))
        {
            i++;
            if (i == positions.Count)
            {
                i = 0;
            }
        }

        //add rounding

        if ((int)(block.position.x * 10) < (int)(positions[i].position.x * 10))
        {
            block.position = new Vector3(block.position.x + Time.deltaTime * speed, block.position.y, 0f);
        }
        else if ((int)(block.position.x * 10) > (int)(positions[i].position.x * 10))
        {
            block.position = new Vector3(block.position.x - Time.deltaTime * speed, block.position.y, 0f);
        }
        if ((int)(block.position.y * 10) < (int)(positions[i].position.y * 10))
        {
            block.position = new Vector3(block.position.x, block.position.y + Time.deltaTime * speed, 0f);
        }
        else if ((int)(block.position.y * 10) > (int)(positions[i].position.y * 10))
        {
            block.position = new Vector3(block.position.x, block.position.y - Time.deltaTime * speed, 0f);
        }


        
    }
}
