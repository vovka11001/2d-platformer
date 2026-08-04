using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private string _gameSceneName = "GameScene";

    public void PlayGame()
    {
        SceneManager.LoadScene(_gameSceneName);
    }

    public void ShowAuthor()
    {
        Debug.Log("Так называемый автор");
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}