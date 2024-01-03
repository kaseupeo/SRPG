using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinding
{
    public List<T> FindPath<T>(T start, T end) where T : OverlayTile
    {
        if (start.Node == null || end.Node == null)
            return null;
        
        List<T> openList = new List<T>();
        List<T> closedList = new List<T>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            T currentNode = openList.OrderBy(x => x.Node.F).First();

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == end)
            {
                // finalize our path
                return GetFinishedList(start, end);
            }

            var neighbourNodes = GetNeighbourNodes(currentNode);

            foreach (var neighbour in neighbourNodes)
            {
                // 1은 넘어갈수 있는 타일 높이
                if (neighbour.Node.IsBlocked || closedList.Contains(neighbour) ||
                    Mathf.Abs(currentNode.Node.GridLocation.z - neighbour.Node.GridLocation.z) > 1)
                {
                    continue;
                }

                neighbour.Node.G = GetManhattanDistance(start, neighbour);
                neighbour.Node.H = GetManhattanDistance(end, neighbour);

                neighbour.Node.Previous = currentNode;
                
                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }
            }
        }

        return new List<T>() ;
    }

    private List<T> GetNeighbourNodes<T>(T currentNode) where T : OverlayTile
    {
        var map = Managers.Map.MapTiles;
        List<T> neighbours = new List<T>();
        
        // Top
        Vector2Int locationToCheck = new Vector2Int(currentNode.Node.GridLocation.x, currentNode.Node.GridLocation.y);
        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add((T)map[locationToCheck]);
        }
        
        // Bottom
        locationToCheck =
            new Vector2Int(currentNode.Node.GridLocation.x, currentNode.Node.GridLocation.y - 1);
        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add((T)map[locationToCheck]);
        }
        
        // Right
        locationToCheck = new Vector2Int(currentNode.Node.GridLocation.x + 1, currentNode.Node.GridLocation.y);
        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add((T)map[locationToCheck]);
        }
        
        // Left
        locationToCheck = new Vector2Int(currentNode.Node.GridLocation.x - 1, currentNode.Node.GridLocation.y);
        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add((T)map[locationToCheck]);
        }

        return neighbours;
    }

    private int GetManhattanDistance<T>(T start, T neighbour) where T : OverlayTile
    {
        return Mathf.Abs(start.Node.GridLocation.x - neighbour.Node.GridLocation.x) +
               Mathf.Abs(start.Node.GridLocation.y - neighbour.Node.GridLocation.y);
    }

    private List<T> GetFinishedList<T>(T start, T end) where T : OverlayTile
    {
        List<T> finishedList = new List<T>();

        T currentNode = end;

        while (currentNode != start)
        {
            finishedList.Add(currentNode);
            currentNode = currentNode.Node.Previous as T;
        }

        finishedList.Reverse();

        return finishedList;
    }
}