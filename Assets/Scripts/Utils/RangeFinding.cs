using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeFinding
{
    public static List<Tile> GetTilesInRange(Vector2Int location, int range)
    {
        Tile startTile = Managers.Map.MapTiles[location];
        List<Tile> inRangeTile = new List<Tile>();
        List<Tile> tilesForPreviousStep = new List<Tile>();
        int stepCount = 0;

        inRangeTile.Add(startTile);
        tilesForPreviousStep.Add(startTile);

        while (stepCount < range)
        {
            List<Tile> surroundingTiles = new List<Tile>();
            
            foreach (Tile tile in tilesForPreviousStep)
                surroundingTiles.AddRange(GetSurroundingTiles(new Vector2Int(tile.GridLocation.x, tile.GridLocation.y)));

            inRangeTile.AddRange(surroundingTiles);
            tilesForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        }

        return inRangeTile.Distinct().ToList();
    }

    public static List<Tile> GetSurroundingTiles(Vector2Int originTile)
    {
        List<Tile> surroundingTiles = new List<Tile>();
        Dictionary<Vector2Int, Tile> mapTiles = Managers.Map.MapTiles;
        
        Vector2Int tileToCheck = new Vector2Int(originTile.x + 1, originTile.y);
        if (mapTiles.ContainsKey(tileToCheck) && !PathFinding.IsCheckToPassTile(mapTiles[originTile], mapTiles[tileToCheck], 1))
            surroundingTiles.Add(mapTiles[tileToCheck]);

        tileToCheck = new Vector2Int(originTile.x - 1, originTile.y);
        if (mapTiles.ContainsKey(tileToCheck) && !PathFinding.IsCheckToPassTile(mapTiles[originTile], mapTiles[tileToCheck], 1))
            surroundingTiles.Add(mapTiles[tileToCheck]);

        tileToCheck = new Vector2Int(originTile.x, originTile.y + 1);
        if (mapTiles.ContainsKey(tileToCheck) && !PathFinding.IsCheckToPassTile(mapTiles[originTile], mapTiles[tileToCheck], 1))
            surroundingTiles.Add(mapTiles[tileToCheck]);

        tileToCheck = new Vector2Int(originTile.x, originTile.y - 1);
        if (mapTiles.ContainsKey(tileToCheck) && !PathFinding.IsCheckToPassTile(mapTiles[originTile], mapTiles[tileToCheck], 1))
            surroundingTiles.Add(mapTiles[tileToCheck]);

        ;
        return surroundingTiles;
    }

    
    
    public static List<Tile> GetSurroundingTiles(Vector2Int originTile, int range, int height = 1)
    {
        List<Tile> surroundingTiles = new List<Tile>();
        Dictionary<Vector2Int, Tile> mapTiles = Managers.Map.MapTiles;

        for (int y = -range; y <= range; y++)
        {
            for (int x = -range; x <= range; x++)
            {
                Vector2Int tileToCheck = originTile + new Vector2Int(x, y);

                if (mapTiles.ContainsKey(tileToCheck) && Mathf.Abs(mapTiles[tileToCheck].transform.position.z - mapTiles[originTile].transform.position.z) <= height)
                {
                    surroundingTiles.Add(mapTiles[tileToCheck]);
                }
            }
        }

        return surroundingTiles;
    }
}