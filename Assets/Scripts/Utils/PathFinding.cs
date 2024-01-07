using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TODO : 길찾기 제네릭으로 할지 아니면 그냥 쓸지 고민하고 수정하기
public class PathFinding
{
    private static Dictionary<Vector2Int, Tile> _searchableTiles;
    
    public static List<Tile> FindPath(Tile start, Tile end, List<Tile> inRangeTiles)
    {
        _searchableTiles = new Dictionary<Vector2Int, Tile>();
        
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        if (inRangeTiles.Count > 0)
        {
            foreach (Tile tile in inRangeTiles)
                _searchableTiles.Add(tile.Grid2DLocation, Managers.Map.MapTiles[tile.Grid2DLocation]);
        }
        else
        {
            _searchableTiles = Managers.Map.MapTiles;
        }
        
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
                // 1은 넘어갈수 있는 최대 타일 높이
                if (closedList.Contains(tile) || IsCheckToPassTile(currentTile, tile, 1))
                    continue;
                
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

    public static bool IsCheckToPassTile(Tile startTile, Tile endTile, int height = 1)
    {
        return Mathf.Abs(startTile.transform.position.z - endTile.transform.position.z) > height || endTile.IsBlocked;
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
    
    private static List<Tile> GetNeighbourTiles(Tile currentTile)
    {
        List<Tile> neighbours = new List<Tile>();

        // Right
        Vector2Int locationToCheck = new Vector2Int(currentTile.GridLocation.x + 1, currentTile.GridLocation.y);
        if (_searchableTiles.TryGetValue(locationToCheck, out var tile))
            neighbours.Add(tile);
        
        // Left
        locationToCheck = new Vector2Int(currentTile.GridLocation.x - 1, currentTile.GridLocation.y);
        if (_searchableTiles.TryGetValue(locationToCheck, out tile))
            neighbours.Add(tile);
        
        // Top
        locationToCheck = new Vector2Int(currentTile.GridLocation.x, currentTile.GridLocation.y + 1);
        if (_searchableTiles.TryGetValue(locationToCheck, out tile))
            neighbours.Add(tile);
        
        // Bottom
        locationToCheck = new Vector2Int(currentTile.GridLocation.x, currentTile.GridLocation.y - 1);
        if (_searchableTiles.TryGetValue(locationToCheck, out tile))
            neighbours.Add(tile);
        
        return neighbours;
    }

    private static int GetManhattanDistance(Tile start, Tile tile)
    {
        return Mathf.Abs(start.GridLocation.x - tile.GridLocation.x) +
               Mathf.Abs(start.GridLocation.y - tile.GridLocation.y);
    }
}