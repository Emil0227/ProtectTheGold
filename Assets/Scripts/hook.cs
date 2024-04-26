using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hook : MonoBehaviour
{
    [SerializeField] private Sprite m_HookOpenSprite;
    [SerializeField] private Sprite m_HookClosedSprite;
    private SpriteRenderer m_hookSr;

    public void SetHookSprite(bool isOpen)
    {
        if (isOpen)
        {
            m_hookSr.sprite = m_HookOpenSprite;
        }
        else
        {
            m_hookSr.sprite = m_HookClosedSprite;
        }
    }
    void Start()
    {
        m_hookSr = GetComponent<SpriteRenderer>();
        SetHookSprite(true);
    }
    public void OnTriggerEnter2D(Collider2D other)//the hook grabs something
    {
        SetHookSprite(false);//set hook sprite to closed
        GetComponent<Collider2D>().enabled = false;//stop hook collision detection
        transform.parent.GetComponent<rope>().RopeState = RopeState.Shorten;//change rope state
        other.transform.parent = transform;//make the object move together with the hook
        other.transform.position = new Vector3(transform.position.x, 
            transform.position.y - 0.5f, transform.position.z);//adjust object position
        if (other.tag == "Player")
        {
            levelManager.IsPlayerCatched = true;
            other.GetComponent<AudioSource>().Play(); 
            return;
        }
        GetComponent<AudioSource>().Play();
    }
}
