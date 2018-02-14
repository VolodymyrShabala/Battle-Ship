using System.Collections.Generic;
using UnityEngine;

public class MapCreatorBattle : MonoBehaviour {
    [SerializeField]
    private GameObject battleTilePrefab;

    private BattleTile[,] tiles;

    public void Initiate(bool[,] shipPlacement, bool mine) {
        tiles = new BattleTile[10, 10];

        for (int x = 0; x < shipPlacement.GetLength(0); x++) {
            for (int y = 0; y < shipPlacement.GetLength(0); y++) {
                tiles[x, y] = CreateTile(new Vector2(x, y));
                tiles[x, y].Initiate(x, y, shipPlacement[x, y], mine);
            }
        }
    }

    //for loading
    public void Initiate(string shipPlacement, bool mine) {
        string[] tileStrings = shipPlacement.Split(';');

        tiles = new BattleTile[10, 10];
        int index = 0;
        for (int x = 0; x < 10; x++) {
            for (int y = 0; y < 10; y++) {
                tiles[x, y] = CreateTile(new Vector2(x, y));
                tiles[x, y].Initiate(tileStrings[index++]);
            }
        }
    }

    //for shooting
    public void ShootAt(int x, int y) {
        if (tiles[x, y].WasShot) {
            return;
        }
        if (tiles[x, y].Shoot()) {
            List<BattleTile> ship = MakeShip(x, y);
            bool shipIsDead = true;
            foreach (BattleTile tile in ship) {
                if (!tile.WasShot) {
                    shipIsDead = false;
                }
            }
            if (shipIsDead) {
                ShipDeadRoutine(ship);
            }
        }
    }

    //is called when a ship is dead. It sets all tiles around ship to shooted
    private void ShipDeadRoutine(List<BattleTile> ship) {
        foreach (BattleTile tile in ship) {
            tile.Kill();
            for (int x = -1; x != 2; x++) {
                for (int y = -1; y != 2; y++) {
                    if (!(tile.X + x < 0 || tile.X + x > 9 || tile.Y + y < 0 || tile.Y + y > 9)) {
                        tiles[tile.X + x, tile.Y + y].Shoot();
                    }
                }
            }
        }
    }

    //looks if all ships are dead
    public bool AllShipsAreDead() {
        foreach (BattleTile tile in tiles) {
            if (tile.HasShip && !tile.WasShot) {
                return false;
            }
        }
        return true;
    }

    //looks if a tile is a part of a bigger ship
    private List<BattleTile> MakeShip(int x, int y) {
        List<BattleTile> ship = new List<BattleTile>();
        if (!tiles[x, y].HasShip) {
            return null;
        }
        ship.Add(tiles[x, y]);
        int c = x - 1;
        while (true) {
            if (!HasTileShip(c, y)) {
                break;
            }
            ship.Add(tiles[c, y]);
            c--;
        }
        c = x + 1;
        while (true) {
            if (!HasTileShip(c, y)) {
                break;
            }
            ship.Add(tiles[c, y]);
            c++;
        }
        c = y - 1;
        while (true) {
            if (!HasTileShip(x, c)) {
                break;
            }
            ship.Add(tiles[x, c]);
            c--;
        }
        c = y + 1;
        while (true) {
            if (!HasTileShip(x, c)) {
                break;
            }
            ship.Add(tiles[x, c]);
            c++;
        }
        return ship;
    }

    public bool TileAvailableForShooting(int x, int y) {
        return !tiles[x, y].WasShot;
    }

    private bool HasTileShip(int x, int y) {
        if (x < 0 || x > 9) {
            return false;
        }
        if (y < 0 || y > 9) {
            return false;
        }
        return tiles[x, y].HasShip;
    }

    //creates new tiles for both player and enemy
    private BattleTile CreateTile(Vector2 pos) {
        BattleTile tile = Instantiate(battleTilePrefab, pos, Quaternion.identity).GetComponent<BattleTile>();
        tile.transform.parent = transform;
        tile.transform.localPosition = pos;
        return tile;
    }

    //Calls save function in every tile
    public string Serialize() {
        string map = "";
        for (int x = 0; x < 10; x++) {
            for (int y = 0; y < 10; y++) {
                string tile = tiles[x, y].Serialize();
                map += tile + ";";
            }
        }
        map = map.Substring(0, map.Length - 1);
        return map;
    }

    //for loading selected tiles
    public List<BattleTile> SelectedTiles() {
        List<BattleTile> selectedTiles = new List<BattleTile>();
        for (int x = 0; x < 10; x++) {
            for (int y = 0; y < 10; y++) {
                if (tiles[x, y].Selected) {
                    selectedTiles.Add(tiles[x, y]);
                }
            }
        }
        return selectedTiles;
    }
}