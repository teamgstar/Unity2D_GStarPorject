using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParent : MonoBehaviour
{
    private PlayerMovement m_PlayerMovement;
    private GameObject m_Player;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerMovement = GetComponentInChildren<PlayerMovement>();
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //충돌체 태그가 벽이면
        if (collision.gameObject.tag == "Wall")
        {
            //velocity값 0으로
            this.GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
            this.GetComponentInParent<Rigidbody2D>().gravityScale = 0;
            //플레이어 상태도 뗀 상태로 돌아감
            m_PlayerMovement.m_Status = PlayerMovement.PlayerStatus.PS_Idle;
            //애니메이션 가만히 있는거로 변경
            m_PlayerMovement.m_Anime.SetBool("b_Move", false);

            if (m_PlayerMovement.m_CollDir == PlayerMovement.CollDir.CD_Left)
                m_Player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
            if (m_PlayerMovement.m_CollDir == PlayerMovement.CollDir.CD_Right)
                m_Player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            if (m_PlayerMovement.m_CollDir == PlayerMovement.CollDir.CD_Top)
                m_Player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -180));
            if (m_PlayerMovement.m_CollDir == PlayerMovement.CollDir.CD_Bottom)
                m_Player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            ////충돌체와 위치 비교해 충돌면 구하고 회전
            //if (collision.gameObject.transform.position.y - collision.gameObject.GetComponent<SpriteRenderer>().size.y / 2 >
            //    this.transform.position.y)
            //{
            //    this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -180));
            //}
            //
            //if (collision.gameObject.transform.position.y + collision.gameObject.GetComponent<SpriteRenderer>().size.y / 2 <
            //    this.transform.position.y)
            //{
            //    this.transform.rotation = Quaternion.Euler( new Vector3(0, 0, 0));
            //}
            //
            //if (collision.gameObject.transform.position.x - collision.gameObject.GetComponent<SpriteRenderer>().size.x / 2 >
            //   this.transform.position.x )
            //{
            //    this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            //}
            //
            //if (collision.gameObject.transform.position.x + collision.gameObject.GetComponent<SpriteRenderer>().size.x / 2 <
            //    this.transform.position.x)
            //{
            //    this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
            //}

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (m_PlayerMovement.m_Rigid.velocity == Vector2.zero)
        {
            if (collision.gameObject.tag == "Wall")
            {
                if (m_PlayerMovement.m_CollDir == PlayerMovement.CollDir.CD_Left)
                    m_Player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                if (m_PlayerMovement.m_CollDir == PlayerMovement.CollDir.CD_Right)
                    m_Player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                if (m_PlayerMovement.m_CollDir == PlayerMovement.CollDir.CD_Top)
                    m_Player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -180));
                if (m_PlayerMovement.m_CollDir == PlayerMovement.CollDir.CD_Bottom)
                    m_Player.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
        }

    }

}
