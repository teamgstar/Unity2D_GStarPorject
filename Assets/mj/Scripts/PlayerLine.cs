using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLine : MonoBehaviour
{
    public GameObject line;
    public GameObject Player;
    private SpriteRenderer render;
    void Start()
    {
      
        render = line.GetComponent<SpriteRenderer>();
        render.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
       
        if(PlayerMovement.m_Status == PlayerMovement.PlayerStatus.Slide_On)
        {
            Vector3 PlayerPos = Player.transform.position;
            this.transform.position = PlayerPos;
            render.enabled = true;
        }
        if(PlayerMovement.b_Move)
        {
            render.enabled = false;
        }
        
        //Rot = playerScript.Rot;S
        //this.transform.Rotate(new Vector3(0 ,0, PlayerMovement.Rot));
        this.transform.rotation =  Quaternion.Euler(0, 0, PlayerMovement.Rot + 90);
        this.transform.localScale = new Vector3(1, Mathf.Abs(Vector2.Distance(PlayerMovement.m_vSlideEndPos , PlayerMovement.m_vSlideStartPos)) / 75 , 1);
    }
}