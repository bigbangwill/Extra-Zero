using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentTreeOrbitalMovement : MonoBehaviour
{
    [SerializeField] private GameObject nodePrefab;

    public float speed;
    public List<Transform> nodeList = new List<Transform>();
    public float timeOffset = 5f;

    private List<TalentLibrary> givenTalents = new List<TalentLibrary>();

    private GameStateManagerRefrence gameStateManagerRefrence;
    private TalentManagerRefrence talentManagerRefrence;
    private List<NodePassive> passives = new();

    private void LoadSORefrence()
    {
        gameStateManagerRefrence = (GameStateManagerRefrence)FindSORefrence<GameStateManager>.FindScriptableObject("Game State Manager Refrence");
        talentManagerRefrence = (TalentManagerRefrence)FindSORefrence<TalentManager>.FindScriptableObject("Talent Manager Refrence");
    }

    private void Start()
    {
        LoadSORefrence();
    }

    public List<Transform> GetAllNodes()
    {
        return nodeList;
    }


    public void AddToTalentList(TalentLibrary talent)
    {
        givenTalents.Add(talent);
    }

    
    public void StartSummonNodes()
    {
        passives.Clear();
        foreach(var talent in givenTalents)
        {
            GameObject summonedNode = Instantiate(nodePrefab,transform);
            NodePassive passive = summonedNode.GetComponent<NodePassive>();
            CreatedTalents.SetNodeToTalent(talent, passive);
            passives.Add(passive);
            NodeMovement movement = summonedNode.GetComponent<NodeMovement>();
            passive.SetTalent(talent);
            talent.SetTalentManagerRefrence(talentManagerRefrence.val);
            passive.SetTalentManagerRefrence(talentManagerRefrence);
            GameState currentstate = gameStateManagerRefrence.val.GetGameState();
            if (currentstate == GameState.InGame)
            {
                if (passive.IsPurchased)
                    passive.SetNodeState(NodePurchaseState.IsNotPurchased);
                else
                    passive.SetNodeState(NodePurchaseState.IsPurchased);
            }
            else if (currentstate == GameState.OnMenu)
            {
                passive.SetNodeState(NodePurchaseState.IsMenuPassive);
            }
            nodeList.Add(summonedNode.transform);
        }
        
        
        StartRotating();
    }

    public void SetPassiveStats()
    {
        foreach (var passive in passives)
        {
            passive.ApplyTalentState();
        }
    }


    public void StartRotating()
    {
        int count = nodeList.Count;
        if (count % 2 == 0)
        {
            count++;
        }

        float angleStep = 360f / count; // Angle between each object

        for (int i = 0; i < nodeList.Count; i++)
        {
            nodeList[i].Rotate(new Vector3(0, angleStep * i, 0));
        }
        StartCoroutine(StartMovement());
    }

    private IEnumerator StartMovement()
    {
        float timeSpent = 0;
        int counter = 0;
        int sending = 1;
        while (true)
        {
            if (timeSpent > timeOffset)
            {
                nodeList[counter].GetComponent<NodeMovement>().Move(sending);
                sending *= -1;
                counter++;
                timeSpent = 0;
            }
            timeSpent += Time.deltaTime;
            yield return null;
            if (counter > nodeList.Count - 1)
            {
                foreach (Transform t in nodeList)
                {
                    t.GetComponent<NodeMovement>().SlowDown();
                }
                break;
            }
        }
    }

}
