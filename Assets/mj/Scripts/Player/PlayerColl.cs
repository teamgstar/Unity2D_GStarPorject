﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().OnTrigger(this.gameObject);
    }


}
