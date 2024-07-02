using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public Animal PlayerAnimal;
    public List<GameObject> MyConnetions;
    // public static Player Instance { get; private set; }
    // void Awake()
    // {
    //     if (Instance != null && Instance != this)
    //         Destroy(this);
    //     else
    //         Instance = this;
    // }
    void Start()
    {
        foreach(GameGraph node in BoardManager.Instance.gameGraph)
        {
            if(node.gameObject == gameObject){
                MyConnetions = node.ConnectedObjects;
            }
        }
    }

    public void SetPlayerAnimal()
    {
        PlayerAnimal = gameObject.GetComponentInChildren<PlayerAnimal>().animal;
    }
    

    public bool CheckMove(GameObject source, GameObject target)
    {
        bool flag = false;
        if(source == this.gameObject)
        {
        foreach(GameObject obj in MyConnetions)
        {
            if(obj == target)
            {
                flag = true;
                Debug.Log(obj.name + target.name);
                }
            }
        }
        return flag;
    }
}
