using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private TileManager tileManager;

    [SerializeField] private GameObject selectShipsCanvas;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Text text;

    private bool gameStarted;
    private void Awake() {
        BattleHelper.GameManager = this;

        if (BattleHelper.GameState == null){
            OnEmptyStart();
            return;
        }
        OnLoadStart(BattleHelper.GameState);
    }

    private void Start() {
        Invoke("DeactivateText", 5);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)){
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        }
    }

    private void OnEmptyStart() {
        BattleHelper.GameState = null;

        tileManager.gameObject.SetActive(true);
        battleManager.gameObject.SetActive(false);
        tileManager.Initiate();
        selectShipsCanvas.SetActive(true);
    }

    public void OnLoadStart(string gameState) {
        selectShipsCanvas.SetActive(false);
        tileManager.gameObject.SetActive(false);
        battleManager.gameObject.SetActive(true);
        battleManager.Initiate(gameState);
        Camera.main.GetComponent<CameraController>().canScroll = true;
    }

    public void GameIsReady(bool[,] tiles) {
        tileManager.gameObject.SetActive(false);
        battleManager.gameObject.SetActive(true);
        battleManager.Initiate(tiles, AiMapCreator.CreateMap());
        selectShipsCanvas.SetActive(false);
        StartCoroutine(NextTips());
        Camera.main.GetComponent<CameraController>().canScroll = true;
    }

    public void EndGame(bool playerWon) {
        text.gameObject.SetActive(true);
        text.color = playerWon ? new Color(0.27f, 0.61f, 0.14f, 1.0f) : new Color(0.61f, 0.14f, 0.14f, 1.0f);
        text.text = playerWon ? "YOU WON" : "YOU LOST";
        text.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        StartCoroutine(EndGame());
    }

    private IEnumerator EndGame() {
        battleManager.gameEnded = true;
        float timeToEnd = 3;
        while (true){
            yield return new WaitForEndOfFrame();
            timeToEnd -= Time.deltaTime;
            Color color = text.color;
            color.a = timeToEnd / 3;
            text.color = color;
            if (timeToEnd <= 0){
                SceneManager.LoadScene(0);
            }
        }
    }

    private void DeactivateText() {
        if (gameStarted) {
            return;
        }
        text.gameObject.SetActive(false);
        text.text = " ";
    }

    private IEnumerator NextTips() {
        gameStarted = true;
        text.gameObject.SetActive(true);
        text.text = "You can scroll with mouse wheel";
        yield return new WaitForSeconds(4);
        text.text = "You must first mark 3 tiles to shoot before shooting. When you choose the third one shots will be fired automatically";
        yield return new WaitForSeconds(6);
        gameStarted = false;
        DeactivateText();
    }

    //saving with string
    public void OnGameSave() {
        string gameState = battleManager.Serialize();
        PlayerPrefs.SetString("Save", gameState);
        PlayerPrefs.Save();
    }

    public void NewGame() {
        OnEmptyStart();
    }

    public void OnLoadGame() {
        if (BattleHelper.GameState == null){
            OnEmptyStart();
            return;
        }
        OnLoadStart(BattleHelper.GameState);
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LazyMode() {
        GameIsReady(AiMapCreator.CreateMap());
    }

    public void Save() {
        OnGameSave();
    }

    public void Continue() {
        pauseMenu.SetActive(false);
    }

    public void ExitTiMainMenu() {
        BattleHelper.GameManager.OnGameSave();
        SceneManager.LoadScene(0);
    }

    public TileManager TileManager {
        get {
            return tileManager;
        }
    }

    public BattleManager BattleManager {
        get {
            return battleManager;
        }
    }
}