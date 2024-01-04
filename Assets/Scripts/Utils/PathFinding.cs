using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TODO : 길찾기 제네릭으로 할지 아니면 그냥 쓸지 고민하고 수정하기
public class PathFinding
{
    public static List<OverlayTile> FindPath(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> openList = new List<OverlayTile>();
        List<OverlayTile> closedList = new List<OverlayTile>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            OverlayTile currentOverlayTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentOverlayTile);
            closedList.Add(currentOverlayTile);

            if (currentOverlayTile == end)
                return GetFinishedList(start, end);
            
            foreach (OverlayTile tile in GetNeighbourNodes(currentOverlayTile))
            {
                // 1은 넘어갈수 있는 타일 높이
                if (tile.IsBlocked || closedList.Contains(tile) ||
                    Mathf.Abs(currentOverlayTile.transform.position.z - tile.transform.position.z) > 1)
                {
                    continue;
                }

                tile.G = GetManhattanDistance(start, tile);
                tile.H = GetManhattanDistance(end, tile);

                tile.Previous = currentOverlayTile;
                
                if (!openList.Contains(tile))
                {
                    openList.Add(tile);
                }
            }
        }

        return new List<OverlayTile>() ;
    }

    private static List<OverlayTile> GetNeighbourNodes(OverlayTile currentOverlayTile)
    {
        Dictionary<Vector2Int, OverlayTile> map = Managers.Map.MapTiles;
        List<OverlayTile> neighbours = new List<OverlayTile>();
        
        // Right
        Vector2Int locationToCheck = new Vector2Int(currentOverlayTile.GridLocation.x + 1, currentOverlayTile.GridLocation.y);
        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }
        
        // Left
        locationToCheck = new Vector2Int(currentOverlayTile.GridLocation.x - 1, currentOverlayTile.GridLocation.y);
        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }
        
        // Top
        locationToCheck = new Vector2Int(currentOverlayTile.GridLocation.x, currentOverlayTile.GridLocation.y + 1);
        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }
        
        // Bottom
        locationToCheck =
            new Vector2Int(currentOverlayTile.GridLocation.x, currentOverlayTile.GridLocation.y - 1);
        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }
        
        return neighbours;
    }

    private static int GetManhattanDistance(OverlayTile start, OverlayTile tile)
    {
        return Mathf.Abs(start.GridLocation.x - tile.GridLocation.x) +
               Mathf.Abs(start.GridLocation.y - tile.GridLocation.y);
    }

    private static List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> finishedList = new List<OverlayTile>();
        OverlayTile currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.Previous;
        }

        finishedList.Reverse();

        return finishedList;
    }
}