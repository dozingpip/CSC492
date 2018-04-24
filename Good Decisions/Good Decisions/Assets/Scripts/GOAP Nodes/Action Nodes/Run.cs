using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// An action node where the agent goes running
/// </summary>
public class Run : Node {

    private GameObject _track;
    private Transform[] _track_points;

    private int _current_index;
    private float _start_time;



    public Run(GOAP_NPC p_npc)
    {
        _npc = p_npc;
        _node_id = "Run";
        _requirements.Add(new GetWorkoutPants(p_npc));
        CommenceNode();
    }

    public override float getCost()
    {

        //Distance to go home
        return Vector3.Distance(_track.transform.position, _npc._home.transform.position);

    }

    public override void CommenceNode()
    {

        _completionTimer = 30f;

        _track = GameObject.FindGameObjectWithTag("Track");
        _track_points = _track.GetComponentsInChildren<Transform>();
        _current_index = 0;

        NavMeshAgent nav = _npc.gameObject.GetComponent<NavMeshAgent>();
        nav.SetDestination(_track.transform.position);
    }

    public override void ExecuteNode()
    {


        if (_walkedToGoal)
        {
            if (Time.time > _currentTimer)
            {
                _actionComplete = true;
            }

            //Run around the race track
            if (_npc.ReachedDestination())
            {
                _current_index += 1;
                Vector3 next_point = _track_points[_current_index % _track_points.Length].position;
                _npc.GetComponent<NavMeshAgent>().SetDestination(next_point);

            }

        } else if (_npc.ReachedDestination() && !_actionComplete && !_walkedToGoal) {
            _currentTimer = Time.time + _completionTimer;
            _walkedToGoal = true;
        }
    }

}
