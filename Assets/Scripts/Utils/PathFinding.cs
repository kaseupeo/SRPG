using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TODO : 길찾기 제네릭으로 할지 아니면 그냥 쓸지 고민하고 수정하기
public class PathFinding
{
    public static List<Tile> FindPath(Tile start, Tile end)
    {
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            Tile currentTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if (currentTile == end)
                return GetFinishedList(start, end);
            
            foreach (Tile tile in GetNeighbourTiles(currentTile))
            {
                // 1은 넘어갈수 있는 타일 높이
                if (tile.IsBlocked || closedList.Contains(tile) ||
                    Mathf.Abs(currentTile.transform.position.z - tile.transform.position.z) > 1)
                {
                    continue;
                }

                tile.G = GetManhattanDistance(start, tile);
                tile.H = GetManhattanDistance(end, tile);

                tile.Previous = currentTile;
                
                if (!openList.Contains(tile))
                {
                    openList.Add(tile);
                }
            }
        }

        return new List<Tile>() ;
    }

    private static List<Tile> GetNeighbourTiles(Tile currentTile)
    {
        Dictionary<Vector2Int, Tile> map = Managers.Map.MapTiles;
        List<Tile> neighbours = new List<Tile>();
        
        // Right
        Vector2Int locationToCheck = new Vector2Int(currentTile.GridLocation.x + 1, currentTile.GridLocation.y);
        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }
        
        // Left
        locationToCheck = new Vector2Int(currentTile.GridLocation.x - 1, currentTile.GridLocation.y);
        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }
        
        // Top
        locationToCheck = new Vector2Int(currentTile.GridLocation.x, currentTile.GridLocation.y + 1);
        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }
        
        // Bottom
        locationToCheck =
            new Vector2Int(currentTile.GridLocation.x, currentTile.GridLocation.y - 1);
        if (map.ContainsKey(locationToCheck))
        {
            neighbours.Add(map[locationToCheck]);
        }
        
        return neighbours;
    }

    private static int GetManhattanDistance(Tile start, Tile tile)
    {
        return Mathf.Abs(start.GridLocation.x - tile.GridLocation.x) +
               Mathf.Abs(start.GridLocation.y - tile.GridLocation.y);
    }

    private static List<Tile> GetFinishedList(Tile start, Tile end)
    {
        List<Tile> finishedList = new List<Tile>();
        Tile currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.Previous;
        }

        finishedList.Reverse();

        return finishedList;
    }
}