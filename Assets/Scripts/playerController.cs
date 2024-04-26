using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Rigidbody2D m_rBody;
    private Animator m_anim;
    private float m_moveSpeed;
    private float m_colliderHalfWidth;
    private float m_colliderHalfHeight;

    void Start()
    {
        m_rBody = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
        m_moveSpeed = 2.0f;
        m_colliderHalfWidth = 0.42f;
        m_colliderHalfHeight = 0.27f;
    }
    void FixedUpdate()
    {
        if(levelManager.IsLevelFinished == false)
        {
            if (levelManager.IsPlayerCatched == false)
            {
                float moveX = Input.GetAxisRaw("Horizontal");
                float moveY = Input.GetAxisRaw("Vertical");
                Vector2 position = transform.position;
                position.x += moveX * m_moveSpeed * Time.fixedDeltaTime;
                position.y += moveY * m_moveSpeed * Time.fixedDeltaTime;
                m_rBody.MovePosition(position);
                if (moveX < 0)
                {
                    m_anim.SetInteger("state", 2);
                }
                else if (moveX > 0)
                {
                    m_anim.SetInteger("state", 1);
                }
                else 
                {
                    m_anim.SetInteger("state", 0);
                }
            }
            else//player is catched
            {
                m_anim.SetInteger("state", 3);
                //set the player's layer to "Gold" to avoid collision with other objects
                gameObject.layer = LayerMask.NameToLayer("Gold");
            }
        }
        else//finish level
        {
            m_anim.SetInteger("state", 0);
        }
    }
    void Update()
    {
        if (levelManager.IsPlayerCatched == false)
        {
            ClampInScreen();
        }
    }
    void ClampInScreen()
    {
        Vector3 position = transform.position;
        if (position.x - m_colliderHalfWidth < ScreenUtils.ScreenLeft)
        {
            position.x = ScreenUtils.ScreenLeft + m_colliderHalfWidth;
        }
        else if (position.x + m_colliderHalfWidth > ScreenUtils.ScreenRight)
        {
            position.x = ScreenUtils.ScreenRight - m_colliderHalfWidth;
        }
        if (position.y - m_colliderHalfHeight < ScreenUtils.ScreenBottom)
        {
            position.y = ScreenUtils.ScreenBottom + m_colliderHalfHeight;
        }
        else if (position.y > 0)
        {
            position.y = 0;
        }
        transform.position = position;
    }
}
