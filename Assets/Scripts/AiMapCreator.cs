using Random = System.Random;

public static class AiMapCreator {
    private static bool[,] tiles;
    private static Random random = new Random();

    public static bool[,] CreateMap() {
        tiles = new bool[10, 10];

        bool shipPlaced = false;
        while (!shipPlaced){
            shipPlaced = PlaceShip4();
        }

        for (int i = 0; i < 2; i++){
            shipPlaced = false;
            while (!shipPlaced){
                shipPlaced = PlaceShip3();
            }
        }

        for (int i = 0; i < 3; i++){
            shipPlaced = false;
            while (!shipPlaced){
                shipPlaced = PlaceShip2();
            }
        }

        for (int i = 0; i < 4; i++){
            shipPlaced = false;
            while (!shipPlaced){
                shipPlaced = PlaceShip1();
            }
        }
        return tiles;
    }

    private static bool PlaceShip1() {
        int x = random.Next(10);
        int y = random.Next(10);

        if (TileAvailable(x, y)){
            tiles[x, y] = true;
            return true;
        }
        return false;
    }

    private static bool PlaceShip2() {
        int x = random.Next(10);
        int y = random.Next(10);

        if (TileAvailable(x, y) && TileAvailable(x - 1, y)){
            tiles[x, y] = true;
            tiles[x - 1, y] = true;
            return true;
        }
        if (TileAvailable(x, y) && TileAvailable(x, y - 1)){
            tiles[x, y] = true;
            tiles[x, y - 1] = true;
            return true;
        }
        if (TileAvailable(x, y) && TileAvailable(x + 1, y)){
            tiles[x, y] = true;
            tiles[x + 1, y] = true;
            return true;
        }
        if (TileAvailable(x, y) && TileAvailable(x, y + 1)){
            tiles[x, y] = true;
            tiles[x, y + 1] = true;
            return true;
        }
        return false;
    }

    private static bool PlaceShip3() {
        int x = random.Next(10);
        int y = random.Next(10);

        if (TileAvailable(x, y) && TileAvailable(x - 1, y) && TileAvailable(x - 2, y)){
            tiles[x, y] = true;
            tiles[x - 1, y] = true;
            tiles[x - 2, y] = true;
            return true;
        }
        if (TileAvailable(x, y) && TileAvailable(x, y - 1) && TileAvailable(x, y - 2)){
            tiles[x, y] = true;
            tiles[x, y - 1] = true;
            tiles[x, y - 2] = true;
            return true;
        }
        if (TileAvailable(x, y) && TileAvailable(x + 1, y) && TileAvailable(x + 2, y)){
            tiles[x, y] = true;
            tiles[x + 1, y] = true;
            tiles[x + 2, y] = true;
            return true;
        }
        if (TileAvailable(x, y) && TileAvailable(x, y + 1) && TileAvailable(x, y + 2)){
            tiles[x, y] = true;
            tiles[x, y + 1] = true;
            tiles[x, y + 2] = true;
            return true;
        }
        return false;
    }

    private static bool PlaceShip4() {
        int x = random.Next(10);
        int y = random.Next(10);

        if (TileAvailable(x, y) && TileAvailable(x - 1, y) && TileAvailable(x - 2, y) && TileAvailable(x - 3, y)){
            tiles[x, y] = true;
            tiles[x - 1, y] = true;
            tiles[x - 2, y] = true;
            tiles[x - 3, y] = true;
            return true;
        }
        if (TileAvailable(x, y) && TileAvailable(x, y - 1) && TileAvailable(x, y - 2) && TileAvailable(x, y - 3)){
            tiles[x, y] = true;
            tiles[x, y - 1] = true;
            tiles[x, y - 2] = true;
            tiles[x, y - 3] = true;
            return true;
        }
        if (TileAvailable(x, y) && TileAvailable(x + 1, y) && TileAvailable(x + 2, y) && TileAvailable(x + 3, y)){
            tiles[x, y] = true;
            tiles[x + 1, y] = true;
            tiles[x + 2, y] = true;
            tiles[x + 3, y] = true;
            return true;
        }
        if (TileAvailable(x, y) && TileAvailable(x, y + 1) && TileAvailable(x, y + 2) && TileAvailable(x, y + 3)){
            tiles[x, y] = true;
            tiles[x, y + 1] = true;
            tiles[x, y + 2] = true;
            tiles[x, y + 3] = true;
            return true;
        }
        return false;
    }

    private static bool TileAvailable(int x, int y) {
        if (!TileExists(x, y) || !TileEmpty(x, y)){
            return false;
        }

        for (int _x = -1; _x < 2; _x++){
            for (int _y = -1; _y < 2; _y++){
                if (_x == 0 && _y == 0){
                    continue;
                }
                if (TileExists(x - _x, y - _y) && !TileEmpty(x - _x, y - _y)){
                    return false;
                }
            }
        }

        return true;
    }

    private static bool TileEmpty(int x, int y) {
        return !tiles[x, y];
    }

    private static bool TileExists(int x, int y) {
        return x >= 0 && x <= 9 && y >= 0 && y <= 9;
    }
}