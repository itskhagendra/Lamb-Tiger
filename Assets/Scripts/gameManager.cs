using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using TMPro;

public class gameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject SelectedGameObject;
    public GameObject TargetPoition;
    public static gameManager Instance { get; private set; }

    public Player Selectedplayer;
    public Player TargetPlayer;
    public TMP_Text TurnText;


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
        Turn = Animal.GOAT;
        TurnText.text  = Turn.ToString();
        Selectedplayer = this.GetComponent<Player>();
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
        TurnText.text = Turn.ToSafeString(); 
    }

    public void CloseApplication()
    {
        Application.Quit();
    }
}
public enum Animal
{
    GOAT,
    TIGER
}
