using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// The GOAP Agent controller.
/// This class is responsible for the agent's status and decision making
/// </summary>
public class GOAP_NPC : MonoBehaviour {


    /// <summary>
    /// The agent's navigation mesh
    /// </summary>
    private NavMeshAgent _nav;

    /// <summary>
    /// //Highest Priority ( 50 moderate hunger, 75 extreme, 100 death)
    /// </summary>
    public float _tiredness = 0;

    /// <summary>
    /// Second Priority ( 50 moderate tiredness, 75 extreme, 100 death)
    /// </summary>
    public float _hunger = 0;

    /// <summary>
    /// The value of food for the NPC
    /// </summary>
    protected float _nutritionalValue;

    /// <summary>
    /// The quality of this NPCs sleep
    /// </summary>
    protected float _sleepQuality;

    
    /// <summary>
    /// The home base for the agent 
    /// </summary>
    public GameObject _home;

    /// <summary>
    /// The workplace for the agent
    /// </summary>
    public GameObject _workPlace;

    /// <summary>
    /// The agent's current state (DEBUG)
    /// </summary>
    public string _state;

    /// <summary>
    /// The current list of actions for the agent to take
    /// </summary>
    private List<Node> _actionList;

    /// <summary>
    /// The current action of the agent
    /// </summary>
    private IEnumerator<Node> _currentNode;

    // Sets the initial state for the agent
    void Start () {

        //Agents will start at 0 hunger & tiredness
        //This will make their first action seeking entertainment
        _tiredness = 0f;
        _hunger = 0f;

        //The quality of rest and nutrition from their meals is randomized
        _sleepQuality = Random.Range(0.1f,0.3f);
        _nutritionalValue= Random.Range(0.2f, 0.4f);

        //Sets the navigation option
        _nav = GetComponent<NavMeshAgent>();
        _nav.speed *= WorldManager._timeScale;

       
        _currentNode = null;
        _state = "Initialization";
	}

    /// <summary>
    /// Increases the agent's hunger and tiredness 
    /// </summary>
    protected void increaseCosts()
    {
        //Increment hunger and sleep as necessary
        _hunger += 0.05f * WorldManager._timeScale * Time.deltaTime;
        _tiredness += 0.02f * WorldManager._timeScale * Time.deltaTime;
    }

    /// <summary>
    /// Executes the current action node for the agent, 
    /// or moves to a new one as necessary
    /// </summary>
    void Update () {

        //Changes base status
        increaseCosts();

        //If the user has no current goal,
        //decide a new goal and begin executing the necessary actions
        if (_currentNode == null)
        {
            //Decide action
            _actionList = DecideAction();

            //Grab an ienumerator for the list (iterator)
            //IENumerator begins at the spot before the first entry in the list
            _currentNode = _actionList.GetEnumerator();

            //Move to and initialize the first node in the list 
            _currentNode.MoveNext();
            _currentNode.Current.CommenceNode();

            //Update the currently displayed NodeID for the agent
            _state = _currentNode.Current.getNodeID();
        }

        //Check if the current action node has completed it's operation
        if (!_currentNode.Current.isActionComplete())
        {
            //Continue to run the node's action 
            _currentNode.Current.ExecuteNode();

        //If the action node has completed it's operation
        } else {

            //If at the final node, set current node to null
            if (!_currentNode.MoveNext())
            {
                _currentNode = null;

            //Otherwise move to the next node and initialize it
            } else
            {
                _currentNode.Current.CommenceNode();
                _state = _currentNode.Current.getNodeID();
            }
        }

    }

    /// <summary>
    /// Based on the agent's status values, decide what goal they should seek
    /// </summary>
    /// <returns>The list of actions necessary to accomplish the agent's goals</returns>
    public List<Node> DecideAction()
    {

        List<Node> actionList = null;
        if (_tiredness > 50)
        {

            actionList = SearchAlgorithms.findPath(new Sleep(this));
        } else if (_hunger > 60)
        {
            actionList = SearchAlgorithms.findPath(new EatFood(this), "GreedyDFS");
        } else
        {
            actionList = SearchAlgorithms.findPath(new Relaxation(this), "GreedyDFS");
        }

        //The action list to determine the agent's goals
        return actionList;

    }

    /// <summary>
    /// Whether or not the agent's navmeshagent has reached it's destination
    /// </summary>
    /// <returns></returns>
    public bool ReachedDestination()
    {
        if (!_nav.pathPending)
        {
            if (_nav.remainingDistance <= _nav.stoppingDistance)
            {
                if (!_nav.hasPath || _nav.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Provides the agent one 'unit' of rest
    /// </summary>
    public void rest()
    {
        _tiredness -= _sleepQuality * WorldManager._timeScale * Time.deltaTime;

    }

    /// <summary>
    /// Provides the agent one 'unit' of nutrition
    /// </summary>
    public void eat()
    {
        _hunger -= _nutritionalValue* WorldManager._timeScale * Time.deltaTime;
    }


}
