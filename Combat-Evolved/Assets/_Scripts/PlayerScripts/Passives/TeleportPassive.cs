using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPassive : PassiveBase
{
    public GameObject firepoint;
    public PlayerController owner;
    public string soundEffect;

    // Start is called before the first frame update
    void Start()
    {
        owner = GetComponentInParent<PlayerController>();
    }

    public override void ActivatePassive(PlayerController pc) 
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
