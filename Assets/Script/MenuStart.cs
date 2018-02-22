using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStart : MonoBehaviour {
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene("main", LoadSceneMode.Single);
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
    }
}
