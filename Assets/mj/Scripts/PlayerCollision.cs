using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public enum ColliderDirection
    {
        LEFT,
        TOP,
        RIGHT,
        BOTTOM
    }
    public ColliderDirection colliderDir;
    private PlayerMovement m_Movement = null;
    private void Start()
    {
        m_Movement = transform.parent.GetComponent<PlayerMovement>();
    }

    // Start is called before the first frame update

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
