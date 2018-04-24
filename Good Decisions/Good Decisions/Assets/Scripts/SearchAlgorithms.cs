using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SearchAlgorithms {


    /// <summary>
    /// A function to find a viable path to a goal node
    /// </summary>
    /// <param name="p_goal">the goal node to find</param>
    /// <param name="p_search_algorithm">the search algorithm is use, defaulted to Depth First Search</param>
    /// <returns>Returns the path to the target node</returns>
    public static List<Node> findPath(Node p_goal, string p_search_algorithm = "DFS")
    {

        List<Node> path = null;

        if (p_search_algorithm == "DFS")
        {
            Queue<Node> searchQueue = new Queue<Node>();
            //SearchAlgorithms.DFS(p_npc, p_goal, ref searchQueue);
            SearchAlgorithms.DFS(p_goal, ref searchQueue);
            path = searchQueue.ToList<Node>();
        }else if(p_search_algorithm == "GreedyDFS")
        {
            Queue<Node> searchQueue = new Queue<Node>();
            SearchAlgorithms.GreedyDFS(p_goal, ref searchQueue);
            path = searchQueue.ToList<Node>();
        }


        return path;
    }

    /// <summary>
    /// DFS, search until a node is found with no requirements
    /// </summary>
    /// <param name="p_goal">the node to find, can be a goal or action node</param>
    /// <param name="p_nodes">the output queue for the path, passed in by reference</param>
    /// <returns></returns>
    public static bool DFS(Node p_goal, ref Queue<Node> p_nodes)
    {


        if (p_goal.getPreq().Count == 0)
        {
            p_nodes.Enqueue(p_goal);
            return true;
        }
        foreach (Node req in p_goal.getPreq())
        {

            if (DFS(req, ref p_nodes))
            {
                p_nodes.Enqueue(p_goal);
                return true;
            }
        }
        return false;
    }

    public static bool GreedyDFS(Node p_goal, ref Queue<Node> p_nodes)
    {
        List<Node> preq = p_goal.getPreq();
        if (preq.Count == 0)
        {
            p_nodes.Enqueue(p_goal);
            return true;
        }
        Node lowestCostNode = null;
        // compare every node cost with current lowest cost, unless it hasn't been set yet
        for(int i = 0; i<preq.Count; i++){
            if(lowestCostNode == null || preq[i].getCost()<lowestCostNode.getCost()){
                lowestCostNode = preq[i];
            }
        }
        //once it's gotten through the for loop, the lowest cost node for this level of the tree is decided.
        Debug.Log("lowest cost was " + lowestCostNode.getCost() + " for " + lowestCostNode + ".");
        // do another dfs to get the lowest cost requirements for this lowest cost node
        if (GreedyDFS(lowestCostNode, ref p_nodes))
        {
            p_nodes.Enqueue(p_goal);
            return true;
        }
        return false;
    }



}
