using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Tooltip("초당 해당 이동속도의 100 배수 만큼 이동합니다.")]
    public float MoveSpeed;

    [HideInInspector] public enum PlayerStatus : byte
    {
        PS_Idle, //아무것도 안누르고 있을 때
        PS_Press, //누르고 있을 때
    }
    [HideInInspector] public PlayerStatus m_Status;
    
    [HideInInspector] public Vector2 m_Direction;

    [HideInInspector] public Vector2 m_StartPos;
    [HideInInspector] public Vector2 m_EndPos;

    private Camera m_Camera;
    private Animator m_Anime;
    private SpriteRenderer m_Renderer;

    private void Awake()
    {
        m_Direction = Vector2.zero;

        m_StartPos  = Vector2.zero;
        m_EndPos    = Vector2.zero;

        m_Status    = PlayerStatus.PS_Idle;
 
    }

    void Start()
    {
        m_Camera    = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        m_Anime     = GetComponentInChildren<Animator>();
        m_Renderer = GetComponent<SpriteRenderer>();

        //애니메이션 상태 초기화
        m_Anime.SetBool("b_Move", false);
    }

    private void Update()
    {
        switch (m_Status)
        {
            case PlayerStatus.PS_Idle:
                //화면 터치 다운시에
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    //게임속도 느리게해주고
                    GameManager.g_GameSpeed = 0.025f;

                    //처음 좌표값 설정
                    m_StartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    //기존 Velocity값에 게임속도값을 곱해서 기존 날라가고있는경우에 감속
                    this.GetComponent<Rigidbody2D>().velocity *= GameManager.g_GameSpeed;

                    //열거형 값을 Press상태로 변경
                    m_Status = PlayerStatus.PS_Press;
                }
                break;
            case PlayerStatus.PS_Press:
                //EndPos를 항상 커서좌표로 갱신해줘서 라인 좌표값도 갱신되게 해줌
                m_EndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                //방향벡터 구하고 아크탄젠트 구해서 버튼 터치 다운 좌표값과 터치 업 좌표값의 각도값을 구해준다.
                m_Direction = m_EndPos - m_StartPos;
                m_Direction.Normalize();

                float Rot = Mathf.Atan2(-m_Direction.y, -m_Direction.x) * 180 / Mathf.PI;

                Debug.Log(Rot);

                if (m_Renderer.flipX)
                {
                    if (Mathf.Abs(Rot) < 90)
                    {
                        m_Renderer.flipX = false;
                        Debug.Log("AAAA");
                    }
                }
                else
                {
                    if (Rot > 180)
                    {
                        m_Renderer.flipX = true;
                        Debug.Log("BBB");
                    }
                }

                //버튼 터치 업
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    //애니메이션 움직이는거로 변경
                    m_Anime.SetBool("b_Move", true);

                    //플레이어 회전 초기화
                    this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                    //게임 속도 다시 원상태로 돌려주고
                    GameManager.g_GameSpeed = 1.0f;

                    //기존에 날라가던 velocity 힘을 0으로     
                    this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    
                    //그다음에 방향벡터에 이동속도를 곱해서 날라간다.
                    //FixedDeltaTime으로 한 이유는 날라가는 속도를 일정하게 맞춰주기 위해
                    Vector2 Power = m_Direction * MoveSpeed *100* Time.fixedDeltaTime;

                    //AddForce로 velocity값을 증가시키면 무게의 영향을 받게되므로 직접 대입
                    this.GetComponent<Rigidbody2D>().velocity = Power;

                    //플레이어 좌우 반전
                    if (this.GetComponent<Rigidbody2D>().velocity.x < 0)
                    {
                        m_Renderer.flipX = true;
                    }
                    else if (this.GetComponent<Rigidbody2D>().velocity.x > 0)
                    {
                        m_Renderer.flipX = false;
                    }

                    //그다음 다시 땐상태로 enum값 되돌림
                    m_Status = PlayerStatus.PS_Idle;
                }
                break;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //충돌체 태그가 벽이면
        if (collision.gameObject.tag == "Wall")
        {
            //velocity값 0으로
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            
            //플레이어 상태도 뗀 상태로 돌아감
            m_Status = PlayerStatus.PS_Idle;
            //애니메이션 가만히 있는거로 변경
            m_Anime.SetBool("b_Move", false);



            //충돌체와 위치 비교해 충돌면 구하고 회전
            if (collision.gameObject.transform.position.y - collision.gameObject.GetComponent<SpriteRenderer>().size.y / 2 >
                this.transform.position.y)
            {
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -180));
            }

            if (collision.gameObject.transform.position.y + collision.gameObject.GetComponent<SpriteRenderer>().size.y / 2 <
                this.transform.position.y)
            {
                this.transform.rotation = Quaternion.Euler( new Vector3(0, 0, 0));
            }

            if (collision.gameObject.transform.position.x - collision.gameObject.GetComponent<SpriteRenderer>().size.x / 2 >
               this.transform.position.x )
            {
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            }

            if (collision.gameObject.transform.position.x + collision.gameObject.GetComponent<SpriteRenderer>().size.x / 2 <
                this.transform.position.x)
            {
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
            }
            //else
            //    this.transform.Rotate(Vector3.zero);
        }
    }

    void LeftCollision()
    {

    }
    void RightCollision()
    {

    }
    void TopCollision()
    {

    }
    void BotomCollision()
    {

    }
}