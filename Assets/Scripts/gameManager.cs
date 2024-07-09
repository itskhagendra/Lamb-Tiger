using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class gameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int Timer = 120;
    [SerializeField]
    private int timeSpent = 0;
    public GameObject SelectedGameObject;
    public GameObject TargetPoition;
    public static gameManager Instance { get; private set; }

    public Player Selectedplayer;
    public Player TargetPlayer;
    public TMP_Text TurnText;
    [SerializeField]
    TMP_Text TigerScoreText;
    [SerializeField]
    TMP_Text GoatScoreText;
    [SerializeField]
    Image timer;
    [SerializeField]
    GameObject WinPanel;
    [SerializeField]
    TMP_Text WinText;

    public ScoreManager LambScore = new ScoreManager();
    public ScoreManager TigerScore = new();

    public Animal Turn;
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    void Start()
    {
        WinPanel.SetActive(false);
        Turn = Animal.GOAT;
        TurnText.text  = Turn.ToString();
        Selectedplayer = this.GetComponent<Player>();
        StartCoroutine("GameTimer");
        LambScore.TotalScore = LambScore.TotalScore;
        TigerScore.TotalScore = TigerScore.TotalScore;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D raycast = Physics2D.Raycast(rayPos, Vector2.zero);
            if (raycast.collider != null)
            {
                if (SelectedGameObject == null && TargetPoition == null && CheckTurn(raycast.collider.gameObject))
                {
                    SelectedGameObject = raycast.collider.gameObject;
                    Selectedplayer = SelectedGameObject.GetComponent<Player>();
                }
                else if (SelectedGameObject != null && (isEmpty(raycast.collider.gameObject) || isEnemy(raycast.collider.gameObject)))
                {
                    TargetPoition = raycast.collider.gameObject;
                    TargetPlayer = TargetPoition.GetComponent<Player>();
                    StartCoroutine("MovePlayer");
                    AddScore(Turn,ScoreType.Basic);
                }
                else
                {
                    Debug.Log("Invalid State");
                }

            }
        }
    }
    bool CheckTurn(GameObject selection)
    {
        return selection.GetComponent<Player>().PlayerAnimal == Turn;

    }

    IEnumerator MovePlayer()
    {
        yield return new WaitForEndOfFrame();
        if (CheckMove())
        {
            SelectedGameObject.transform.GetChild(0).transform.SetParent(TargetPoition.transform);
            TargetPoition.transform.GetChild(0).transform.localPosition = Vector2.zero;
            TargetPlayer.SetPlayerAnimal();
            Debug.Log($"Move Completed {SelectedGameObject.transform.childCount} and {TargetPoition.transform.childCount}");
            StartCoroutine("nextTurn");
        }
        else
        {
            Debug.Log("Move Not allowed");
        }
        ClearSelection();
       
    }
    public bool CheckMove()
    {
        bool flag = false;
        foreach (GameObject connections in Selectedplayer.MyConnetions)
        {
            if (connections == TargetPoition)
            {
                Debug.Log($"Selected Gameobject {this.gameObject.name} Connection {connections.gameObject.name}");
                flag = true;
                break;
            }
        }
        return flag;
    }
    void ClearSelection()
    {
        SelectedGameObject = null;
        TargetPoition = null;

    }

    bool isEmpty(GameObject target)
    {
        if (target.transform.childCount <= 0 )
            return true;
        else
            return false;
    }

    bool isEnemy(GameObject target)
    {
        if (target.GetComponent<Player>().PlayerAnimal != Selectedplayer.PlayerAnimal && Selectedplayer.PlayerAnimal == Animal.TIGER)
        {
            Destroy(target.transform.GetChild(0).gameObject);
            target.transform.GetChild(0).position = Vector2.zero;
            AddScore(Animal.TIGER,ScoreType.Bonus);
            return true;
        }
        else
            return false;
    }
    IEnumerator nextTurn()
    {
        yield return new WaitForEndOfFrame();
        if (Turn == Animal.GOAT)
            Turn = Animal.TIGER;
        else if (Turn == Animal.TIGER)
            Turn = Animal.GOAT;
        TurnText.text = Turn.ToString(); 
    }

    void AddScore(Animal animal, ScoreType score)
    {
        if (score == ScoreType.Basic)
        {
            if (animal == Animal.TIGER)
                TigerScore.TotalScore += TigerScore.BasicScore;
            else if (animal == Animal.GOAT)
                LambScore.TotalScore+= LambScore.BasicScore;
        }
        else if(score == ScoreType.Bonus)
        {
            if (animal == Animal.TIGER)
                TigerScore.TotalScore+=TigerScore.BonusScore;
            else if (animal == Animal.GOAT)
                LambScore.TotalScore+= LambScore.BonusScore;
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        TigerScoreText.text = TigerScore.TotalScore.ToString();
        GoatScoreText.text = LambScore.TotalScore.ToString();
    }

    public void CloseApplication()
    {
        Application.Quit();
    }
    void Updatetimer()
    {
        timer.fillAmount = (float)timeSpent/Timer;
    }

    IEnumerator GameTimer()
    {
        while (timeSpent <= Timer)
        {
            yield return new WaitForSeconds(1);
            timeSpent++;
            Updatetimer();
        }
        GameOver();
        
    }

    public void GameOver()
    {
        String message = "IS THE WINNER";
        if(LambScore.TotalScore > TigerScore.TotalScore)
        {
            WinText.text = $"GOAT {message}";
        }
        else if(LambScore.TotalScore < TigerScore.TotalScore)
        {
            WinText.text = $"TIGER {message}";
        }
        else{

        }
        WinPanel.SetActive(true);

    }
}
public enum Animal
{
    GOAT,
    TIGER
}
public enum ScoreType
{
    Basic,
    Bonus,
    Chain,

}

public class ScoreManager{
    public int BasicScore { get; set; }
    public int BonusScore { get; set; }
    public int ChainBonus { get; set; }
    public int TotalScore { get; set; }

    public ScoreManager()
    {
        BasicScore = 1;
        BonusScore = 5;
        ChainBonus = 1;
        TotalScore = 0;
    }
    public int getTotalScore()
    {
        this.TotalScore = this.BasicScore+this.BonusScore+this.ChainBonus;
        return TotalScore;
    }
}
