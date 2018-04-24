using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// An action node where the agent goes to a cafeteria to eat food
/// </summary>
public class EatFood : Node {

    private GameObject _cafeteria;


    public EatFood(GOAP_NPC p_npc)
    {
        _npc = p_npc;
        _completionTimer = 30f;
        _node_id = "EatFood";

        _requirements.Add(new GetUtensils(p_npc));
        _requirements.Add(new GetWallet(p_npc));
        CommenceNode();
    }

    public override float getCost()
    {

        //Distance to go home
        return Vector3.Distance(_cafeteria.transform.position, _npc._home.transform.position);

    }

    public override void CommenceNode()
    {
 
        //Find and go to the cafeteria
        _cafeteria = GameObject.FindGameObjectWithTag("Cafeteria");

        NavMeshAgent nav = _npc.gameObject.GetComponent<NavMeshAgent>();
        nav.SetDestination(_cafeteria.transform.position);
    }

    public override void ExecuteNode()
    {

        if (_walkedToGoal &&  _npc._hunger <= 0)
        {
            _actionComplete = true;

            //Walk to location, pass time
        }
        else if (_npc.ReachedDestination() && !_actionComplete && !_walkedToGoal)
        {
            _currentTimer = Time.time + _completionTimer;
            _walkedToGoal = true;
        } else
        {
            _npc.eat();
        }
    }

}
