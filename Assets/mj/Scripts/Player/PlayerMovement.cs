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

    [HideInInspector] public Animator m_Anime;
    [HideInInspector] public Rigidbody2D m_Rigid;

    [HideInInspector] public enum CollDir
    {
        CD_Left,
        CD_Right,
        CD_Top,
        CD_Bottom
    }

    [HideInInspector] public CollDir m_CollDir;
    
    private Camera m_Camera;
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
        m_Renderer  = GetComponentInChildren<SpriteRenderer>();
        m_Rigid     = this.GetComponentInParent<Rigidbody2D>();

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
                    m_Rigid.velocity *= GameManager.g_GameSpeed;

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
                m_Rigid.gravityScale = 0;

                //버튼 터치 업
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    m_Rigid.gravityScale = 1;

                    //애니메이션 움직이는거로 변경
                    m_Anime.SetBool("b_Move", true);

                    //플레이어 회전 초기화
                    this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                    //게임 속도 다시 원상태로 돌려주고
                    GameManager.g_GameSpeed = 1.0f;

                    //기존에 날라가던 velocity 힘을 0으로     
                    m_Rigid.velocity = Vector2.zero;
                    
                    //그다음에 방향벡터에 이동속도를 곱해서 날라간다.
                    //FixedDeltaTime으로 한 이유는 날라가는 속도를 일정하게 맞춰주기 위해
                    Vector2 Power = m_Direction * MoveSpeed *100* Time.fixedDeltaTime;

                    //AddForce로 velocity값을 증가시키면 무게의 영향을 받게되므로 직접 대입
                    m_Rigid.velocity = Power;

                    //플레이어 좌우 반전
                    if (m_Rigid.velocity.x < 0)
                    {
                        m_Renderer.flipX = true;
                    }
                    else if (m_Rigid.velocity.x > 0)
                    {
                        m_Renderer.flipX = false;
                    }

                    //그다음 다시 땐상태로 enum값 되돌림
                    m_Status = PlayerStatus.PS_Idle;
                }
                break;
        }
    }


    public void OnTrigger(GameObject Child)
    {
        if (Child.gameObject.tag == "Player_Left")
            m_CollDir = CollDir.CD_Left;
        if (Child.gameObject.tag == "Player_Right")
            m_CollDir = CollDir.CD_Right;
        if (Child.gameObject.tag == "Player_Top")
            m_CollDir = CollDir.CD_Top;
        if (Child.gameObject.tag == "Player_Bottom")
        {
            m_CollDir = CollDir.CD_Bottom;
            m_Anime.SetBool("b_Bottom", true);
        }
        else
        {
            m_Anime.SetBool("b_Bottom", false);
        }
    }
}