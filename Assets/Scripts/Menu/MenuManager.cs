using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private string _gameSceneName = "GameScene";

    public void PlayGame()
    {
        SceneManager.LoadScene(_gameSceneName);
    }

    public void OpenSettings()
    {
        Debug.Log("Открыть настройки");
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}