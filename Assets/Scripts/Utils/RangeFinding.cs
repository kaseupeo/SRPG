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
                surroundingTiles.AddRange(Managers.Map.GetSurroundingTiles(new Vector2Int(tile.GridLocation.x, tile.GridLocation.y)));

            inRangeTile.AddRange(surroundingTiles);
            tilesForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        }

        return inRangeTile.Distinct().ToList();
    } 
}