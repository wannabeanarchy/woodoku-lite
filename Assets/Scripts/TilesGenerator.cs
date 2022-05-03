using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] tilesButtons;
    [SerializeField] private List<GameObject> tilesArray;

    private List<GameObject> listTilesShapes;
      
    void Awake()
    {
        GameLogic.isNeedToSpawnShapeTiles += IsNeedToSpawnShapeTiles;
        GameLogic.respawnShapeTiles += GenerateRandomTiles;
        GameLogic.isGameOver += IsGameOver;
        GameLogic.restartGame += RestartGame;
        GameLogic.loadSaveGame += LoadFromSave;
    }

    private void OnDestroy()
    {
        GameLogic.isNeedToSpawnShapeTiles -= IsNeedToSpawnShapeTiles;
        GameLogic.respawnShapeTiles -= GenerateRandomTiles;
        GameLogic.restartGame -= RestartGame;
        GameLogic.loadSaveGame += LoadFromSave;
    }

    private void OnDisable()
    {
        if (listTilesShapes != null)
        {
            foreach (GameObject tiles in listTilesShapes)
            {
                Destroy(tiles);
            }
        }
    }

    private void GenerateRandomTiles()
    {
        listTilesShapes = new List<GameObject>();
        for  (int i = 0; i < tilesButtons.Length; i++)
        {
            int _index = UnityEngine.Random.Range(0, tilesArray.Count);
            GameObject _randomTile = tilesArray[_index];
            GameObject _tile = Instantiate(_randomTile, tilesButtons[i].transform);
            _tile.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
            _tile.GetComponent<ShapeTiles>().GenerateTilesList(null);
            _tile.GetComponent<ShapeTiles>().GenerateTilesAllRotations();
            _tile.GetComponent<ShapeTiles>().index = _index;
            _tile.GetComponent<ShapeTiles>().isActive = true;
            listTilesShapes.Add(_tile);
            DataSaveManager.SetTilesForSave(_index, i);
        } 
    }

    private void RestartGame()
    {
        if (listTilesShapes != null)
        {
            foreach (GameObject tiles in listTilesShapes)
            {
                Destroy(tiles);
            }
        }

        GenerateRandomTiles();
    }

    private void LoadFromSave()
    {
        DataSave _dataSave = DataSaveManager.GetSaveData();
        listTilesShapes = new List<GameObject>();
        if (_dataSave.dataGrids.Count != 0)
        {
            for (int i = 0; i < _dataSave.shapeTiles.Length; i++)
            {
                if (_dataSave.shapeTiles[i] != -1)
                {
                    GameObject _tile = Instantiate(tilesArray[_dataSave.shapeTiles[i]], tilesButtons[i].transform);
                    _tile.transform.localScale = new Vector3(0.6f, 0.6f, 1f);
                    _tile.GetComponent<ShapeTiles>().GenerateTilesList(null);
                    _tile.GetComponent<ShapeTiles>().GenerateTilesAllRotations();
                    _tile.GetComponent<ShapeTiles>().index = _dataSave.shapeTiles[i];
                    _tile.GetComponent<ShapeTiles>().isActive = true;
                    listTilesShapes.Add(_tile);
                }
            }
        }
        else
        {
            GenerateRandomTiles();
        }
    }

    private bool IsNeedToSpawnShapeTiles()
    {
        foreach (GameObject tiles in listTilesShapes)
        {
            int _index = (tiles.GetComponent<ShapeTiles>().isActive) ? tiles.GetComponent<ShapeTiles>().index : -1;
            DataSaveManager.SetTilesForSave(_index, listTilesShapes.IndexOf(tiles));
        }
        foreach (GameObject tiles in listTilesShapes)
        {
            if (tiles.GetComponent<ShapeTiles>().isActive)
                return false;
        }

        foreach (GameObject tiles in listTilesShapes)
        {
            Destroy(tiles);
        }
        return true;
    }

    public bool IsGameOver()
    { 
        foreach (GameObject tiles in listTilesShapes)
        {
            var _shapeTiles = tiles.GetComponent<ShapeTiles>();
            if (_shapeTiles.isActive && _shapeTiles.CheckShape())
                return false;
        }

        GameLogic.onGameOver.Invoke();
        return true;
    } 
}
