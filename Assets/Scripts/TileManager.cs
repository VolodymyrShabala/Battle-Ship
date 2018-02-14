using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour {
    [SerializeField] private Tile tileToSpawn;
    private Tile[,] tiles = new Tile[10, 10];

    [SerializeField] private LayerMask layer;
    private RectTransform draggableImage;
    private Image image;
    private Tile tile;

    private ShipTile shipTile;
    private int length;
    public bool vertical = true;

    public int start;

    public void Initiate() {
        for (int x = 0; x < 10; x++){
            for (int y = 0; y < 10; y++){
                tiles[x, y] = CreateTile(new Vector2(x, y));
                tiles[x, y].Initiate(x, y);
            }
        }
        layer = ~55;
    }

    public void DragThis(RectTransform t) {
        if (t != draggableImage || draggableImage == null){
            draggableImage = t;
            vertical = true;
            image = t.GetComponentInChildren<Image>();
        }
        if (shipTile == null) {
            shipTile = draggableImage.GetComponentInParent<ShipTile>();
            length = shipTile.shipsLength;
        }
        t.transform.position = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10, layer)){
            if (hit.transform.gameObject != tile){
                tile = hit.transform.GetComponent<Tile>();
                if (tile.HasShip || !tile.Available || !(length > 0 && CanShipBePlaced(length))){
                    image.color = Color.red;
                } else{
                    image.color = Color.white;
                }
            }
        } else{
            if (tile == null){
                return;
            }
            tile = null;
            shipTile = null;
            image.color = Color.white;
        }
    }

    public void OnDragStop() {
        if (tile == null){
            return;
        }

        if (CanShipBePlaced(length)){
            PlaceShip(length);

            shipTile = draggableImage.transform.parent.GetComponentInParent<ShipTile>();
            shipTile.length--;
            draggableImage.gameObject.SetActive(false);

            if (shipTile.length == 0){
                start++;
            }
            shipTile.UpdateText();
        } else {
            draggableImage.anchoredPosition = Vector2.zero;
            image.color = Color.white;
        }
        draggableImage = null;
        if (start == 4){
            StartGame();
        }
        length = 0;
    }

    private void StartGame() {
        bool[,] map = new bool[10, 10];
        for (int x = 0; x < 10; x++){
            for (int y = 0; y < 10; y++){
                if (tiles[x, y].HasShip){
                    map[x, y] = true;
                }
            }
        }

        BattleHelper.GameManager.GameIsReady(map);
    }

    private void Update() {
        if (draggableImage == null){
            return;
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0){
            vertical = !vertical;
            draggableImage.rotation = Quaternion.Euler(0, 0, !vertical ? 90 : 0);
        }      
    }

    private void SetTileUnAvailable(Tile t) {
        if (!t.HasShip){
            return;
        }
        for (int x = -1; x < 2; x++){
            for (int y = -1; y < 2; y++){
                if (x == 0 && y == 0){
                    continue;
                }
                if (TileAvailable(t.X - x, t.Y - y)){
                    tiles[t.X - x, t.Y - y].Available = false;
                }
            }
        }
    }

    private bool CanShipBePlaced(int _length) {
        bool canBePlaced = false;
        if (vertical){
            if (_length == 4){
                if (TileAvailable(tile.X, tile.Y) && TileAvailable(tile.X, tile.Y - 1) &&
                    TileAvailable(tile.X, tile.Y - 2) && TileAvailable(tile.X, tile.Y - 3)){
                    canBePlaced = true;
                } else{
                    canBePlaced = false;
                }
            }
            if (_length == 3){
                if (TileAvailable(tile.X, tile.Y) && TileAvailable(tile.X, tile.Y - 1) &&
                    TileAvailable(tile.X, tile.Y - 2)){
                    canBePlaced = true;
                } else{
                    canBePlaced = false;
                }
            }
            if (_length == 2){
                if (TileAvailable(tile.X, tile.Y) && TileAvailable(tile.X, tile.Y - 1)){
                    canBePlaced = true;
                } else{
                    canBePlaced = false;
                }
            }
        } else{
            if (_length == 4){
                if (TileAvailable(tile.X, tile.Y) && TileAvailable(tile.X + 1, tile.Y) &&
                    TileAvailable(tile.X + 2, tile.Y) && TileAvailable(tile.X + 3, tile.Y)){
                    canBePlaced = true;
                } else{
                    canBePlaced = false;
                }
            }
            if (_length == 3){
                if (TileAvailable(tile.X, tile.Y) && TileAvailable(tile.X + 1, tile.Y) &&
                    TileAvailable(tile.X + 2, tile.Y)){
                    canBePlaced = true;
                } else{
                    canBePlaced = false;
                }
            }
            if (_length == 2){
                if (TileAvailable(tile.X, tile.Y) && TileAvailable(tile.X + 1, tile.Y)){
                    canBePlaced = true;
                } else{
                    canBePlaced = false;
                }
            }
        }
        if (_length == 1) {
            canBePlaced = TileAvailable(tile.X, tile.Y);
        }
        return canBePlaced;
    }

    private void PlaceShip(int _length) {
        if (vertical) {
            if (_length == 4) {
                tiles[tile.X, tile.Y].HasShip = true;
                SetTileUnAvailable(tiles[tile.X, tile.Y]);
                tiles[tile.X, tile.Y - 1].HasShip = true;
                SetTileUnAvailable(tiles[tile.X, tile.Y - 1]);
                tiles[tile.X, tile.Y - 2].HasShip = true;
                SetTileUnAvailable(tiles[tile.X, tile.Y - 2]);
                tiles[tile.X, tile.Y - 3].HasShip = true;
                SetTileUnAvailable(tiles[tile.X, tile.Y - 3]);
            }
            if (_length == 3) {
                tiles[tile.X, tile.Y].HasShip = true;
                SetTileUnAvailable(tiles[tile.X, tile.Y]);
                tiles[tile.X, tile.Y - 1].HasShip = true;
                SetTileUnAvailable(tiles[tile.X, tile.Y - 1]);
                tiles[tile.X, tile.Y - 2].HasShip = true;
                SetTileUnAvailable(tiles[tile.X, tile.Y - 2]);
            }
            if (_length == 2) {
                tiles[tile.X, tile.Y].HasShip = true;
                tiles[tile.X, tile.Y - 1].HasShip = true;
                SetTileUnAvailable(tiles[tile.X, tile.Y]);
                SetTileUnAvailable(tiles[tile.X, tile.Y - 1]);
            }
        } else {
            if (_length == 4) {
                tiles[tile.X, tile.Y].HasShip = true;
                SetTileUnAvailable(tiles[tile.X, tile.Y]);
                tiles[tile.X + 1, tile.Y].HasShip = true;
                SetTileUnAvailable(tiles[tile.X + 1, tile.Y]);
                tiles[tile.X + 2, tile.Y].HasShip = true;
                SetTileUnAvailable(tiles[tile.X + 2, tile.Y]);
                tiles[tile.X + 3, tile.Y].HasShip = true;
                SetTileUnAvailable(tiles[tile.X + 3, tile.Y]);
            }
            if (_length == 3) {
                tiles[tile.X, tile.Y].HasShip = true;
                SetTileUnAvailable(tiles[tile.X, tile.Y]);
                tiles[tile.X + 1, tile.Y].HasShip = true;
                SetTileUnAvailable(tiles[tile.X + 1, tile.Y]);
                tiles[tile.X + 2, tile.Y].HasShip = true;
                SetTileUnAvailable(tiles[tile.X + 2, tile.Y]);
            }
            if (_length == 2) {
                tiles[tile.X, tile.Y].HasShip = true;
                SetTileUnAvailable(tiles[tile.X, tile.Y]);
                tiles[tile.X + 1, tile.Y].HasShip = true;
                SetTileUnAvailable(tiles[tile.X + 1, tile.Y]);
            }
        }
        if (_length == 1) {
            tiles[tile.X, tile.Y].HasShip = true;
            SetTileUnAvailable(tiles[tile.X, tile.Y]);
        }
    }

    private Tile CreateTile(Vector2 pos) {
        return Instantiate(tileToSpawn, pos, Quaternion.identity, transform);
    }

    private bool TileAvailable(int x, int y) {
        return TileExists(x, y) && !tiles[x, y].HasShip && tiles[x, y].Available;
    }

    private bool TileExists(int x, int y) {
        return x >= 0 && x <= 9 && y >= 0 && y <= 9;
    }
}