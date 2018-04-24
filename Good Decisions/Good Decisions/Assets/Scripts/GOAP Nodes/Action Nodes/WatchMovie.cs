using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// An action node where the agent goes to watch a movie
/// </summary>
public class WatchMovie : Node {

    private GameObject _theater;


    public WatchMovie(GOAP_NPC p_npc)
    {
        _npc = p_npc;
        _completionTimer = 30f;
        _node_id = "WatchMovie";
        _requirements.Add(new GetWallet(_npc));
        CommenceNode();

    }

    public override float getCost()
    {

        //Distance to go to the theater
        return Vector3.Distance(_theater.transform.position, _npc._home.transform.position);

    }

    public override void CommenceNode()
    {

        //Go to t he theater
       _theater = GameObject.FindGameObjectWithTag("Theater");
        
        NavMeshAgent nav = _npc.gameObject.GetComponent<NavMeshAgent>();
        nav.SetDestination(_theater.transform.position);
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
