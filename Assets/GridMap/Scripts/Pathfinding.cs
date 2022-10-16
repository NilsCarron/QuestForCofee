    using System.Collections;
using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

public class Pathfinding
{


    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;
    
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    public static Pathfinding Instance { get; private set; }

    public Pathfinding(int width, int height)
    {
        grid = new Grid<PathNode>(width, height, 10f, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));

    }

    public Grid<PathNode> GetGrid()
    {
        Instance = this;
        return grid;
    }



    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        (int startX, int startY) = grid.GetXY(startWorldPosition);
        (int endX, int endY) = grid.GetXY(endWorldPosition);
        List<PathNode> path = FindPath(startX, startY, endX, endY);


        if (path == null)
            return null;
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach(PathNode pathNode in path)
            {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
            }
            return vectorPath;
        }

    }



    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        Debug.Log("appel");
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        openList = new List<PathNode> { startNode };
        
        
        if (GameManager.Instance.ControlledByAstar)
        {
            
            closedList = new List<PathNode>();

            for (int x = 0; x < grid.GetWidth(); ++x)
            {
                for (int y = 0; y < grid.GetHeight(); ++y)
                {
                    PathNode pathNode = grid.GetGridObject(x, y);
                    pathNode.gCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.cameFromNode = null;


                }
            }


            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode); //ASTAR ICI
            startNode.CalculateFCost();
            
            
            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(openList);
                if (currentNode == endNode)
                {
                    //final node
                    return CalculatePath(endNode);

                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);


                foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
                {
                    if (closedList.Contains(neighbourNode))
                    {
                        continue;
                    }

                    if (!neighbourNode.isWalkable)
                    {
                        closedList.Add(neighbourNode);
                        continue;
                    }

                    int tentativeCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode) - neighbourNode.Acceleration;
                    Debug.Log(tentativeCost);

                    if (tentativeCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tentativeCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();
                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);

                        }
                    }
                }


            }
            //Out of nodes in the open List No Path
            //Apeller fontion enfermement?
            GameManager.Instance.scoring.Emprisonned();
            
            return null;
        }
        else
        {
            
            closedList = new List<PathNode>();

            for (int x = 0; x < grid.GetWidth(); ++x)
            {
                for (int y = 0; y < grid.GetHeight(); ++y)
                {
                    PathNode pathNode = grid.GetGridObject(x, y);
                    pathNode.gCost = int.MaxValue;
                    pathNode.cameFromNode = null;
                    if (!pathNode.isWalkable)
                    {
                        closedList.Add(pathNode);
                        continue;
                    }
                    else
                    {
                        openList.Add(pathNode);

                    }

                }
            }

            startNode.gCost = 0;
            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestGCostNode(openList);
                

                openList.Remove(currentNode);
                foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
                {
                    if (closedList.Contains(neighbourNode) || neighbourNode == currentNode.cameFromNode)
                    {
                        continue;
                    }

                    if (!neighbourNode.isWalkable)
                    {
                        closedList.Add(neighbourNode);
                        continue;
                    }

                    int tentativeCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode) - neighbourNode.Acceleration;
                    
                    if (tentativeCost < neighbourNode.gCost )
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tentativeCost;



                    }
                    

                    
                    
                    
                }


            }
            //final node
            

            
            

        }
        List<PathNode> correctPath  = CalculatePath(endNode);

        return correctPath;
    }
        
        

    


    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighnourlist = new List<PathNode>();
        if (currentNode.x - 1 >= 0)
        {
            //Left
            neighnourlist.Add(GetNode(currentNode.x - 1, currentNode.y));

            //left down
            if (currentNode.y - 1 >= 0)
            {
                neighnourlist.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            }
            //left up
            if (currentNode.y + 1 < grid.GetHeight())
            {
                neighnourlist.Add(GetNode(currentNode.x - 1, currentNode.y + 1));

            }

        }
        if (currentNode.x + 1 < grid.GetWidth())
        {
            //Right
            neighnourlist.Add(GetNode(currentNode.x + 1, currentNode.y));

            //Right down
            if (currentNode.y - 1 >= 0)
            {
                neighnourlist.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            }
            //Right up
            if (currentNode.y + 1 < grid.GetHeight())
            {
                neighnourlist.Add(GetNode(currentNode.x + 1, currentNode.y + 1));

            }

        }

        //Down
        if (currentNode.y - 1 >= 0)
        {
            neighnourlist.Add(GetNode(currentNode.x, currentNode.y - 1));
        }

        //Up
        if (currentNode.y + 1 < grid.GetHeight())
        {
            neighnourlist.Add(GetNode(currentNode.x, currentNode.y + 1));
        }
        return neighnourlist;
    }


    public PathNode GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    private List<PathNode> CalculatePath(PathNode endNode){
        

        List<PathNode> path = new List<PathNode>();
        
        path.Add(endNode);
        PathNode currentNode = endNode;
        while(currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        
        path.Reverse();
        
        return path;
    }
    



    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for(int i=1; i<pathNodeList.Count; ++i)
        {
            if(pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }
    private PathNode GetLowestGCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestGCostNode = pathNodeList[0];
        for(int i=1; i<pathNodeList.Count; ++i)
        {
            if(pathNodeList[i].gCost < lowestGCostNode.gCost)
            {
                lowestGCostNode = pathNodeList[i];
            }
        }
        return lowestGCostNode;
    }
}


