using UnityEngine;

public class Tile : MonoBehaviour {
    private int x, y;
    private bool hasShip;
    private bool available;
    [SerializeField] private Material hasShipMaterial;
    [SerializeField] private Material notAvailableMaterial;
    private Material empty;
    private MeshRenderer mesh;

    public void Initiate(int _x, int _y) {
        x = _x;
        y = _y;
        mesh = GetComponent<MeshRenderer>();
        empty = mesh.material;

        hasShip = false;
        available = true;
    }

    private void UpdateColor() {
        if (hasShip){
            mesh.material = hasShipMaterial;
            return;
        }
        if (!available){
            mesh.material = notAvailableMaterial;
            return;
        }
        mesh.material = empty;
    }

    public int X {
        get {
            return x;
        }
    }
    public int Y {
        get {
            return y;
        }
    }
    public bool HasShip {
        get {
            return hasShip;
        }
        set {
            hasShip = value;
            UpdateColor();
        }
    }
    public bool Available {
        get {
            return available;
        }
        set {
            available = value;
            UpdateColor();
        }
    }
}