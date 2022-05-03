using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject partPrefab;
    [SerializeField] private Color colorTile1;
    [SerializeField] private Color colorTile2;
    [SerializeField] private List<Sprite> listImage; 

    private GameObject[,] cellGrids;

    void Awake()
    {
        DataSaveManager.Init();
        GameLogic.checkGridMatces += CheckGridMatches;
        GameLogic.checkShapeTiles += CheckGameOver;
        GameLogic.restartGame += RestartGame;
        GameLogic.loadSaveGame += LoadFromSave;
    }
    
    private void OnDestroy()
    {
        GameLogic.checkGridMatces -= CheckGridMatches;
        GameLogic.checkShapeTiles -= CheckGameOver;
        GameLogic.restartGame += RestartGame;
        GameLogic.loadSaveGame -= LoadFromSave;
    }

    private void OnEnable()
    { 
        GenerateGrid();
    }

    private void OnDisable()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
    }

    private void RestartGame()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
        GenerateGrid();
        DataSaveManager.RestartGame();
    }

    private void LoadFromSave()
    {
        DataSave _dataSave = DataSaveManager.GetSaveData();
        if (_dataSave.dataGrids != null)
        {
            foreach (var data in _dataSave.dataGrids)
            {
                if (data.part != -1)
                {
                    GameObject _part = Instantiate(partPrefab, cellGrids[data.x, data.y].transform);
                    partPrefab.GetComponent<Image>().sprite = listImage[data.part];
                    _part.GetComponent<Tile>().isActive = true;
                }
            }
        }
        DataSaveManager.SetGridForSave(cellGrids, listImage);
        CheckGridMatches(); 
    }

    private void GenerateGrid()
    {
        cellGrids = new GameObject[GameLogic.sizeGriedX, GameLogic.sizeGriedY];

        Color _currentColorTile = colorTile1;
        cellPrefab.GetComponent<Image>().color = _currentColorTile;

        for (int y = 0; y < GameLogic.sizeGriedX; y++)
        {
            if (y != 0 && y % 3 == 0)
            {
                _currentColorTile = _currentColorTile == colorTile1 ? colorTile2 : colorTile1;
                cellPrefab.GetComponent<Image>().color = _currentColorTile;
            }

            else
                cellPrefab.GetComponent<Image>().color = _currentColorTile;

            for (int x = 0; x < 9; x++)
            {
                if (x != 0 && x % 3 == 0)
                {
                    _currentColorTile = _currentColorTile == colorTile1 ? colorTile2 : colorTile1;
                    cellPrefab.GetComponent<Image>().color = _currentColorTile;
                }

                GameObject cell = Instantiate(cellPrefab, this.transform);
                cell.name = String.Format("x:{0}, y:{1}", x, y);
                cell.GetComponent<Cell>().coordinate.x = x;
                cell.GetComponent<Cell>().coordinate.y = y;
                cellGrids[x, y] = cell;
            }
        }
    }

    private bool CheckGridMatches()
    {
        List<GameObject> _tilesRemove = new List<GameObject>();
        for (int row = 0; row < GameLogic.sizeGriedX; row++)
        {
            _tilesRemove.AddRange(CheckColumns(row));
        }

        for (int column = 0; column < GameLogic.sizeGriedY; column++)
        {
            _tilesRemove.AddRange(CheckRows(column));
        }

        int x = 0; int y = 0;
        for (int index = 0; index < GameLogic.sizeGriedX; index++)
        {
            x = (x == 9) ? 0 : x;
            y = (y == 9) ? 0 : y;

            _tilesRemove.AddRange(CheckSquare(x, y)); 

            if (index % 3 == 0)
                y += 3; 

            x += 3; 
        }

        if (_tilesRemove.Count > 0)
        {
            foreach (var tile in _tilesRemove)
            {
                Tile _tile = tile.GetComponent<Tile>();
                if (_tile != null && _tile.isActive)
                    _tile.NeedDestroy();
            }

            GameLogic.isGameOver.Invoke();
            DataSaveManager.SetGridForSave(cellGrids, listImage);
            return true;
        }
        else
        {
            DataSaveManager.SetGridForSave(cellGrids, listImage);
            return false;
        }

    }
     
    private List<GameObject> CheckColumns(int row)
    {
        List<GameObject> _tiles = new List<GameObject>();

        for (int i = 0; i < GameLogic.sizeGriedY; i++)
        {
            if (cellGrids[row, i].transform.childCount > 0)
                _tiles.Add(cellGrids[row, i].transform.GetChild(0).gameObject);
        }

        if (_tiles.Count != GameLogic.sizeGriedY)
            _tiles.Clear();

        else
            GameLogic.addScore.Invoke();

        return _tiles;
    }

    private List<GameObject> CheckRows(int column)
    {
        List<GameObject> _tiles = new List<GameObject>();

        for (int i = 0; i < GameLogic.sizeGriedX; i++)
        {
            if (cellGrids[i, column].transform.childCount > 0)
                _tiles.Add(cellGrids[i, column].transform.GetChild(0).gameObject);
        }

        if (_tiles.Count != GameLogic.sizeGriedX)
            _tiles.Clear();
        else
            GameLogic.addScore.Invoke();

        return _tiles;
    }

    private List<GameObject> CheckSquare(int x, int y)
    {
        List<GameObject> _tiles = new List<GameObject>();

        for (var row = 0; row < GameLogic.sizeGriedX / 3; row++)
        {
            for (var column = 0; column < GameLogic.sizeGriedY / 3; column++)
            {
                if (cellGrids[row + x, column + y].transform.childCount > 0)
                    _tiles.Add(cellGrids[row + x, column + y].transform.GetChild(0).gameObject);
            }
        }


        if (_tiles.Count != GameLogic.sizeGriedX)
            _tiles.Clear();
        else
            GameLogic.addScore.Invoke();


        return _tiles;
    }

    public GameObject[,] GetGridCells()
    {
        return cellGrids;
    }
 
    public bool CheckGameOver(List<Coordinate> shapeTiles)
    {
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (cellGrids[x, y].transform.childCount == 0)
                {
                    if (GameLogic.CheckShapeInsert(x, y, cellGrids, shapeTiles))
                        return false;
                }
            }
        }
        return true;
    }
 }
