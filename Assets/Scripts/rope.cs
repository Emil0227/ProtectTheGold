using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class rope : MonoBehaviour
{
    [SerializeField] private float m_rotateSpeed;
    [SerializeField] private float m_stretchSpeed;
    [SerializeField] private float m_shortenSpeed;
    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject m_addGoldEffect;
    [SerializeField] private GameObject m_rebirthEffect;
    
    private Camera m_cam;
    private Timer m_timer;
    private Transform m_transHooker;
    private RopeState m_ropeState;
    private Vector3 m_ropeDir;
    private float m_stretchDelay;
    private float m_length; 

    public RopeState RopeState
    {
        set{ m_ropeState = value; }
        get{ return m_ropeState; }
    }

    void Start()
    {
        //initialize rope state & swing timer
        m_ropeState = RopeState.Swing;
        m_stretchDelay = 2;
        m_timer = gameObject.GetComponent<Timer>();
        m_timer.Duration = m_stretchDelay;
        m_timer.Run();
        m_ropeDir = Vector3.back;

        m_cam = GameObject.FindObjectOfType<Camera>();
        m_transHooker = transform.GetChild(0);
        m_length = 1;
    }
    void Update()
    {
        if (!levelManager.IsLevelFinished)
        {
            if (m_ropeState == RopeState.Swing)
            {
                SwingRope();
            }
            else if (m_ropeState == RopeState.Stretch)
            {
                StretchRope();
            }
            else if (m_ropeState == RopeState.Shorten)
            {
                ShortenRope();
            }
        }
    }
    private void SwingRope()
    {   
        if (m_timer.Finished)//swing till the timer is finished, then stretch
        {
            m_ropeState = RopeState.Stretch;
            return;
        }
        m_transHooker.GetComponent<Collider2D>().enabled = false;
        if (transform.localRotation.z <= -0.5f)//-60бу
        {
            m_ropeDir = Vector3.forward;
        }
        else if (transform.localRotation.z >= 0.5f)//60бу
        {
            m_ropeDir = Vector3.back;
        }
        transform.Rotate(m_ropeDir * m_rotateSpeed * Time.deltaTime);
    }
    private void StretchRope()
    {  
        if (m_length >= 10)//stretch till the max length, then shorten
        {
            m_ropeState = RopeState.Shorten;
            return;
        }
        m_transHooker.GetComponent<Collider2D>().enabled = true;
        m_length += Time.deltaTime * m_stretchSpeed;
        transform.localScale = new Vector3(transform.localScale.x, m_length, transform.localScale.z);
        m_transHooker.localScale = new Vector3(m_transHooker.localScale.x, 1/ m_length, m_transHooker.localScale.z);
    }
    private void ShortenRope()
    {
        if (m_length <= 1)//shorten till the min length, then swing
        {
            m_length = 1;
            m_transHooker.GetComponent<hook>().SetHookSprite(true);//set hook sprite to open
            if (m_transHooker.childCount != 0)//the hook has something to grab
            {
                Destroy(m_transHooker.GetChild(0).gameObject);//destroy the object 
                //if the player is in grab, reset player state and instantiate new player
                if (levelManager.IsPlayerCatched == true)
                {
                    levelManager.IsPlayerCatched = false;        
                    GameObject rebirth = GameObject.Instantiate(m_rebirthEffect);
                    rebirth.transform.position = new Vector3(0.2f, -3.3f,0);
                    Destroy(rebirth, 0.3f);
                    GameObject tempPlayer = GameObject.Instantiate(m_player);
                    return;
                }
                AddGold(m_transHooker.GetChild(0).tag);           
            }
            //reset swing timer
            m_stretchDelay = Random.Range(0.2f, 4);
            m_timer.Duration = m_stretchDelay;
            m_timer.Run();
            m_ropeState = RopeState.Swing;
            return;
        }
        m_transHooker.GetComponent<Collider2D>().enabled = false;//stop hook collision detection
        m_length -= Time.deltaTime * m_shortenSpeed;
        transform.localScale = new Vector3(transform.localScale.x, m_length, transform.localScale.z);
        m_transHooker.localScale = new Vector3(m_transHooker.localScale.x, 1 / m_length, m_transHooker.localScale.z);
    }
    private void AddGold(string tag)
    {
        GameObject fx;
        GetComponent<AudioSource>().Play();
        switch (tag)
        {
            case "goldL":
                levelManager.CumulativeGoldCount += levelManager.GoldLscore;
                m_cam.GetComponent<gameManager>().TextGold.text = "GOLD: " + levelManager.CumulativeGoldCount.ToString();
                //set sfx
                fx = GameObject.Instantiate(m_addGoldEffect);
                fx.transform.GetChild(0).GetComponent<Text>().text = "+"+ levelManager.GoldLscore.ToString();
                Destroy(fx, 0.8f);
                break;
            case "goldM":
                levelManager.CumulativeGoldCount += levelManager.GoldMscore;
                m_cam.GetComponent<gameManager>().TextGold.text = "GOLD: " + levelManager.CumulativeGoldCount.ToString();
                //set sfx
                fx = GameObject.Instantiate(m_addGoldEffect);
                fx.transform.GetChild(0).GetComponent<Text>().text = "+" + levelManager.GoldMscore.ToString();
                Destroy(fx, 0.8f);
                break;
            case "goldS":
                levelManager.CumulativeGoldCount += levelManager.GoldSscore;
                m_cam.GetComponent<gameManager>().TextGold.text = "GOLD: " + levelManager.CumulativeGoldCount.ToString();
                //set sfx
                fx = GameObject.Instantiate(m_addGoldEffect);
                fx.transform.GetChild(0).GetComponent<Text>().text = "+" + levelManager.GoldSscore.ToString();
                Destroy(fx, 0.8f);
                break;
            case "stone":
                levelManager.CumulativeGoldCount += levelManager.StoneScore;
                m_cam.GetComponent<gameManager>().TextGold.text = "GOLD: " + levelManager.CumulativeGoldCount.ToString();
                //set sfx
                fx = GameObject.Instantiate(m_addGoldEffect);
                fx.transform.GetChild(0).GetComponent<Text>().text = "+" + levelManager.StoneScore.ToString();
                Destroy(fx, 0.8f);
                break;
        }
    }

}
