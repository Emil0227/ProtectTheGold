using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public Text TextGold;

    [SerializeField] private float m_TimeCount;
    [SerializeField] private float m_TargetGold;
    [SerializeField] private Text m_TextTime;
    [SerializeField] private GameObject m_MaskUI;
    [SerializeField] private GameObject m_LooseUI;
    [SerializeField] private GameObject m_WinUI;
    [SerializeField] private GameObject m_LoadSceneAnim;
    [SerializeField] private GameObject m_Rope;
    
    private void Awake()
    {
        ScreenUtils.Initialize();
        levelManager.IsPlayerCatched = false;
        levelManager.IsLevelFinished = false;
    }
    void Start()
    {
        GameObject loadScene = GameObject.Instantiate(m_LoadSceneAnim);
        TextGold.text = "GOLD: "+ levelManager.CumulativeGoldCount.ToString();
    }
    void Update()
    {
        //count down
        m_TimeCount -= Time.deltaTime;
        m_TextTime.text = "TIME: " + ((int)m_TimeCount).ToString();

        if (levelManager.IsLevelFinished == false)
        {
            CheckWinOrLoose();
        }
        else
        {
            m_TimeCount = 0;
        }
    }
    private void CheckWinOrLoose()
    {
        if (levelManager.CumulativeGoldCount >= m_TargetGold)//golds exceed limitation, player looses
        {
            gameObject.GetComponent<AudioSource>().Stop();
            levelManager.IsLevelFinished = true;
            m_Rope.GetComponent<rope>().RopeState = RopeState.Paused;
            m_MaskUI.SetActive(true);
            m_LooseUI.SetActive(true);
        }
        else if (m_TimeCount <= 0)//player wins
        {
            gameObject.GetComponent<AudioSource>().Stop();
            levelManager.IsLevelFinished = true;
            m_Rope.GetComponent<rope>().RopeState = RopeState.Paused;
            m_MaskUI.SetActive(true);
            m_WinUI.SetActive(true);
        }
    }
}
