using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// An action node where the agent goes home to put on their workout pants
/// </summary>
public class GetWorkoutPants : Node {



    public GetWorkoutPants(GOAP_NPC p_npc)
    {
        _npc = p_npc;
        _completionTimer = 7f;
        _node_id = "GetWorkoutPants";
        CommenceNode();
    }

    public override float getCost()
    {

        //Distance to go home
        return Vector3.Distance(_npc.transform.position, _npc._home.transform.position) + _completionTimer;

    }

    public override void CommenceNode()
    {

       NavMeshAgent nav = _npc.gameObject.GetComponent<NavMeshAgent>();
        nav.SetDestination(_npc._home.transform.position);
    }

    public override void ExecuteNode()
    {

        if (_walkedToGoal && Time.time > _currentTimer)
        {
            _actionComplete = true;

            //Walk to location, pass time
        }
        else if (_npc.ReachedDestination() && !_actionComplete && !_walkedToGoal)

        {
            _currentTimer = Time.time + _completionTimer;
            _walkedToGoal = true;
        }
    }

}
