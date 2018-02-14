using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    public void StartNewGame() {
        BattleHelper.GameState = null;
        SceneManager.LoadScene(1);
    }

    public void LoadGame() {
        string save = PlayerPrefs.GetString("Save");
        if (!string.IsNullOrEmpty(save)){
            BattleHelper.GameState = save;
        }

        SceneManager.LoadScene(1);
    }

    public void Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}