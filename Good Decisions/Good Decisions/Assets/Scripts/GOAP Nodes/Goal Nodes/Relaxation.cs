using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A goal state to let the agent pass time
/// </summary>
public class Relaxation : Node {

    public Relaxation(GOAP_NPC p_npc)
    {
        _npc = p_npc;
        _node_id = "Relaxation";
        _requirements.Add(new Run(p_npc));
        _requirements.Add(new EatFood(p_npc));
        _requirements.Add(new WatchMovie(p_npc));
        CommenceNode();
    }

    public override float getCost()
    {

        //Distance to go home
        return 0f;

    }

    public override void CommenceNode()
    {

    }

    public override void ExecuteNode()
    {
        _actionComplete = true;
    }

}
