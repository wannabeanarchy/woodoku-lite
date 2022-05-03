using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private GameObject sceneGame;
    [SerializeField] private GameObject sceneMenu;

    public void OnStartNewGameClick()
    {
        sceneMenu.SetActive(false);
        sceneGame.SetActive(true);
        GameLogic.restartGame.Invoke();
        GameLogic.onSoundClick.Invoke();
    }

    public void OnContinueGameClick()
    { 
        sceneMenu.SetActive(false);
        sceneGame.SetActive(true);
        GameLogic.loadSaveGame.Invoke();
        GameLogic.onSoundClick.Invoke();
    }

    public void OnQuitGameClick()
    {
        Application.Quit();
    }
}
