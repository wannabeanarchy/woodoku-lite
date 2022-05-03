using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DataSaveManager
{
    private static string pathSaves = Application.persistentDataPath + "\\saves_woodku.json";
    private static DataSave dataSave; 

    public static void Init()
    {  
        dataSave = new DataSave();

        if (File.Exists(pathSaves))
        {
            String JSONtxt = File.ReadAllText(pathSaves);
            dataSave = Newtonsoft.Json.JsonConvert.DeserializeObject<DataSave>(JSONtxt);
        } 
    }

    public static void SetTilesForSave(int tile, int index)
    {
        dataSave.shapeTiles[index] = tile; 
        Save();
    }

    public static void SetGridForSave(GameObject[,] grid, List<Sprite> listImage)
    {
        List<DataGrid> _dataGrid = new List<DataGrid>();
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                DataGrid _dtSave = new DataGrid();
                _dtSave.x = x;
                _dtSave.y = y;
                if (!grid[x, y].GetComponent<Cell>().IsEmptyCell())
                {
                    Sprite sprite = grid[x, y].transform.GetChild(0).GetComponent<Image>().sprite;
                    listImage.IndexOf(sprite); 
                    _dtSave.part = listImage.IndexOf(sprite);
                }
                else
                {
                    _dtSave.part = -1;
                }

                _dataGrid.Add(_dtSave);
            }
        } 
        dataSave.dataGrids = _dataGrid;
        Save();
    }

    public static void Save()
    {
        using (var file = File.CreateText(pathSaves))
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(dataSave, Newtonsoft.Json.Formatting.Indented);
            file.Write(json);
        }
    }

    public static DataSave GetSaveData()
    {
        return dataSave;
    }

    public static void RestartGame()
    {
        dataSave = new DataSave(); 
        Save();
    }
}

public class DataGrid
{
    public int x { get; set; }
    public int y { get; set; }
    public int part { get; set; }
}

public class DataSave
{
    public int[] shapeTiles = new int[3]; 
    public List<DataGrid> dataGrids = new List<DataGrid>();

}