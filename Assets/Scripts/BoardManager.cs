using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject tiger;
    public GameObject Lamb;
    public List<int> TigerPositions = new List<int>();
    public List<GameGraph> gameGraph = new();
    private System.Random random = new System.Random();
    public int LambCount = 10;

    public static BoardManager Instance {get;private set;}

    void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    
    void Start()
    {
        PlaceTigers();
        PlaceLambs();
    }

    // Update is called once per frame

    void PlaceTigers()
    {
        for(int i = 0;i<3;i++)
        {
            int randPlace =  random.Next(0,gameGraph.Count);
            TigerPositions.Add(randPlace);
            GameObject Tiger = Instantiate(tiger, gameGraph[randPlace].gameObject.transform);
            Tiger.GetComponentInParent<Player>().SetPlayerAnimal();
        }
        
    }

    void PlaceLambs()
    {
        int maxCount =LambCount;
        foreach(GameGraph x in gameGraph)
        {
            if(x.gameObject.transform.childCount==0 && maxCount>0)
            {
                GameObject t_lamb = Instantiate(Lamb,x.gameObject.transform);
                t_lamb.GetComponentInParent<Player>().SetPlayerAnimal();
                maxCount--;
            }
        }
    }

    

}

[Serializable]
public class GameGraph
{
    public GameObject gameObject;
    public List<GameObject> ConnectedObjects = new List<GameObject>();
}
