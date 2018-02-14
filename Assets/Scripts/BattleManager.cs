using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class BattleManager : MonoBehaviour {
    [SerializeField] private GameObject mapPrefab;

    private MapCreatorBattle myMap, enemyMap;

    private GameState gameState;
    private List<BattleTile> selectedTile;
    private int shotsAmount = 3;

    private int tilesAvalaibleforShootingPlayer;
    private int tilesAvailableforShootingEnemy;

    public bool gameEnded;

    public void Initiate(bool[,] _myMap, bool[,] _enemyMap) {
        myMap = CreateMap(new Vector3(-6, 0, 0));
        myMap.Initiate(_myMap, true);

        enemyMap = CreateMap(new Vector3(6, 0, 0));
        enemyMap.Initiate(_enemyMap, false);

        gameState = GameState.MyTurn;

        selectedTile = new List<BattleTile>();
    }

    //for loading saved game
    public void Initiate(string _gameState) {
        string[] parts = _gameState.Split('/');
        myMap = Deserialize(new Vector3(-6, 0, 0), true, parts[0]);
        enemyMap = Deserialize(new Vector3(6, 0, 0), false, parts[1]);
        gameState = GameState.MyTurn;
        selectedTile = enemyMap.SelectedTiles();
    }

    //update is used fot Ai to know when to shoot
    private void Update() {
        switch (gameState) {
            case GameState.MyTurn:
                break;
            case GameState.EnemyTurn:
                if (gameEnded) {
                    return;
                }
                MakeShot();
                if (myMap.AllShipsAreDead()) {
                EndGame();
                }
                MakeShot();
                if (myMap.AllShipsAreDead()) {
                    EndGame();
                }
                MakeShot();
                if (myMap.AllShipsAreDead()) {
                    EndGame();
                }
                gameState = GameState.MyTurn;
                break;
        }
    }

    //Player shooting
    public void OnTileClick(BattleTile tile) {
        if (gameState == GameState.MyTurn) {
            if (gameEnded) {
                return;
            }
            if (selectedTile.Contains(tile)) {
                tile.Selected = false;
                selectedTile.Remove(tile);
            } else {
                tile.Selected = true;
                selectedTile.Add(tile);
                HowManyTileLeftEmpty();
                if (selectedTile.Count == shotsAmount) {
                    foreach (BattleTile t in selectedTile) {
                        t.Selected = false;
                        enemyMap.ShootAt(t.X, t.Y);
                    }

                    if (enemyMap.AllShipsAreDead()) {
                        EndGame();
                    }

                    selectedTile.Clear();
                    gameState = GameState.EnemyTurn;
                }
            }
        }
    }

    //Maybe change to a List and remove tiles that was shoted
    private void HowManyTileLeftEmpty() {
        int index = 0;
        for (int x = 0; x < 10; x++) {
            for (int y = 0; y < 10; y++) {
                if (enemyMap.TileAvailableForShooting(x, y)) {
                    index++;
                }
            }
        }
        if (index < 3) {
            shotsAmount = index;
        }
    }

    //AI shooting
    private void MakeShot() {
        Random r = new Random();
        while (true) {
            int x = r.Next(10);
            int y = r.Next(10);

            if (myMap.TileAvailableForShooting(x, y)) {
                myMap.ShootAt(x, y);
                return;
            }
        }
    }

    // if all ships are dead
    private void EndGame() {
        if (myMap.AllShipsAreDead()) {
            BattleHelper.GameManager.EndGame(false);
            return;
        }
        if (enemyMap.AllShipsAreDead()) {
            BattleHelper.GameManager.EndGame(true);
        }
    }

    //creates an instance of a MapCreatorBattle
    private MapCreatorBattle CreateMap(Vector3 pos) {
        MapCreatorBattle map = Instantiate(mapPrefab, pos, Quaternion.identity).GetComponent<MapCreatorBattle>();
        map.transform.parent = transform;
        map.transform.localPosition = pos;
        return map;
    }

    //calls save funktion in MapCreatorBattle twice for every map and save a gameState
    public string Serialize() {
        return myMap.Serialize() + "/" + enemyMap.Serialize() + "/" + gameState;
    }

    //same as createMap but for loading the game
    private MapCreatorBattle Deserialize(Vector3 pos, bool mine, string _text) {
        MapCreatorBattle map = CreateMap(pos);
        map.Initiate(_text, mine);
        return map;
    }
}

public enum GameState { MyTurn, EnemyTurn}