using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// A goal state that provides the agent with rest
/// </summary>
public class Sleep : Node {



    
    public Sleep(GOAP_NPC p_npc)
    {
        _npc = p_npc;
        _completionTimer = 30f;
        _node_id = "Sleep";
        CommenceNode();
    }

    public override float getCost()
    {

        //Distance to go home
        return Vector3.Distance(_npc.transform.position, _npc._home.transform.position);

    }

    public override void CommenceNode()
    {

        //Go home to sleep
        NavMeshAgent nav = _npc.gameObject.GetComponent<NavMeshAgent>();
        nav.SetDestination(_npc._home.transform.position);
    }

    public override void ExecuteNode()
    {

        if (_walkedToGoal && _npc._tiredness<=0)
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
            _npc.rest();
        }
    }
}
