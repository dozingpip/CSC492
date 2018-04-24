using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Node  {

 
    /// <summary>
    /// The unique ID for the GOAP Node
    /// </summary>
    protected string _node_id = "NODE";

    /// <summary>
    /// The list of pre-requisite nodes to reach the given state
    /// (child nodes)
    /// </summary>
    protected List<Node> _requirements = new List<Node>();

    /// <summary>
    /// A location associated with the given node
    /// </summary>
    protected Vector3 _location;

    /// <summary>
    /// A boolean flag to indicate if the action has been completed
    /// </summary>
    protected bool _actionComplete = false;

    /// <summary>
    /// A boolean flag to indicate if the agent has reached the start location for the action
    /// </summary>
    protected bool _walkedToGoal = false;

    /// <summary>
    /// The time taken to complete the action
    /// </summary>
    protected float _completionTimer;

    /// <summary>
    /// The timer to specify when the agent will complete the action
    /// </summary>
    protected float _currentTimer;

    /// <summary>
    /// A reference to the agent
    /// </summary>
    protected GOAP_NPC _npc;


    /// <summary>
    /// Returns the cost of the node
    /// </summary>
    /// <returns>The cost of the node has a float</returns>
    public abstract float getCost();

    /// <summary>
    /// Returns the time taken to complete the action.
    /// This could be used in the cost estimation
    /// </summary>
    /// <returns></returns>
    public float getTimer()
    {
        return _completionTimer;
    }

    /// <summary>
    /// An initializer for the node.
    /// Often it will set the nav location to start the action
    /// </summary>
    public virtual void CommenceNode()
    {
        _actionComplete = false;

    }

    /// <summary>
    /// The regular update function for the node
    /// </summary>
    public abstract void ExecuteNode();

    /// <summary>
    /// Returns the node's pre-requisite action list
    /// </summary>
    /// <returns>The pre-requisite node list</returns>
    public List<Node> getPreq()
    {
        return _requirements;
    }

    /// <summary>
    /// Returns whether the current node's action has been completed
    /// </summary>
    /// <returns>The node's completion flag</returns>
    public bool isActionComplete()
    {
        return _actionComplete;
    }

    /// <summary>
    /// Returns the node's ID
    /// </summary>
    /// <returns></returns>
    public string getNodeID()
    {
        return _node_id;
    }



}
