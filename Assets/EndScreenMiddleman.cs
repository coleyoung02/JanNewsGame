using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenMiddleman : MonoBehaviour
{
    public void Restart()
    {
        PersistantManager.Instance.StartGame();
    }

    public void MainMenu()
    {
        PersistantManager.Instance.MainMenu();
    }

    public void Quit()
    {
        PersistantManager.Instance.Quit();
    }
}
