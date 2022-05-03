using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLogic
{ 
    public static int sizeGriedX = 9;
    public static int sizeGriedY = 9; 

    public static IsNeedToSpawnShapeTiles isNeedToSpawnShapeTiles;
    public static RespawnSpawnShapeTiles respawnShapeTiles;
    public static CheckGridMatches checkGridMatces;
    public static CheckShapeTiles checkShapeTiles;
    public static AddScore addScore;
    public static IsGameOver isGameOver;
    public static RestartGame restartGame;
    public static LoadSaveGame loadSaveGame;
    public static LoadSaveGame onGameOver;
    public static OnSoundClick onSoundClick;

    public delegate void RestartGame();
    public delegate void LoadSaveGame();
    public delegate bool IsNeedToSpawnShapeTiles();
    public delegate void RespawnSpawnShapeTiles();
    public delegate bool CheckGridMatches();
    public delegate void AddScore();
    public delegate bool CheckShapeTiles(List<Coordinate> shapeTiles); 
    public delegate bool IsGameOver();
    public delegate void OnGameOver(); 
    public delegate void OnSoundClick();

    public static bool CheckShapeInsert(int x, int y, GameObject[,] gridCells, List<GameObject> shapeTiles)
    {
        foreach (var tile in shapeTiles)
        {
            int _x = x - tile.GetComponent<Tile>().coordinate.x;
            int _y = y + tile.GetComponent<Tile>().coordinate.y;

            if (!(_x >= 0 && _x < sizeGriedX) || !(_y >= 0 && _y < sizeGriedY) || !gridCells[_x, _y].GetComponent<Cell>().IsEmptyCell())
            {
                return false;
            }
        }
        return true;
    }

    public static bool CheckShapeInsert(int x, int y, GameObject[,] gridCells, List<Coordinate> shapeTiles)
    {
        foreach (var tile in shapeTiles)
        {
            int _x = x - tile.x;
            int _y = y + tile.y;

            if (!(_x >= 0 && _x < sizeGriedX) || !(_y >= 0 && _y < sizeGriedY) || !gridCells[_x, _y].GetComponent<Cell>().IsEmptyCell())
            {
                return false;
            }
        }
        return true;
    }

    public static void TryInsertShapeIntoCells(GridGenerator gridGenerator, ShapeTiles shapeTiles, Cell cell)
    {
        int _xCell = cell.coordinate.x;
        int _yCell = cell.coordinate.y;

        if (!CheckShapeInsert(_xCell, _yCell, gridGenerator.GetGridCells(), shapeTiles.GetTiles()))
            return;

        shapeTiles.isActive = false;

        foreach (var tile in shapeTiles.GetTiles())
        {
            int _x = _xCell - tile.GetComponent<Tile>().coordinate.x;
            int _y = _yCell + tile.GetComponent<Tile>().coordinate.y;
            tile.transform.SetParent(gridGenerator.GetGridCells()[_x, _y].transform);
            tile.transform.localPosition = Vector3.zero;
        } 

        if (isNeedToSpawnShapeTiles.Invoke())
        {
            respawnShapeTiles.Invoke();
        }

        if (!checkGridMatces.Invoke())
            isGameOver.Invoke();
    }
      
}
