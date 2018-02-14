using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleTile : MonoBehaviour {
    [SerializeField] private GameObject shotSign;
    [SerializeField] private GameObject selectedSign;

    //0 - empty
    //1 - my ship
    //2 - shoted
    //3 - dead
    [SerializeField] private Material[] materials;

    private int x, y;
    private bool mine;
    private bool hasShip;
    private bool selected;
    private bool wasShot;
    private bool dead;

    public void Initiate(int _x, int _y, bool _hasShip, bool _mine) {
        x = _x;
        y = _y;

        mine = _mine;
        hasShip = _hasShip;
        selected = false;
        wasShot = false;
        dead = false;
        
        UpdateView();
    }

    //for loading
    public void Initiate(string _text) {
        string[] fields = _text.Split(':');

        x = Int32.Parse(fields[0]);
        y = Int32.Parse(fields[1]);
        mine = Boolean.Parse(fields[2]);
        hasShip = Boolean.Parse(fields[3]);
        wasShot = Boolean.Parse(fields[4]);
        dead = Boolean.Parse(fields[5]);
        selected = Boolean.Parse(fields[6]);

        UpdateView();
    }

    private void OnMouseDown() {
        if (mine || wasShot || dead || EventSystem.current.IsPointerOverGameObject()){
            return;
        }
        BattleHelper.GameManager.BattleManager.OnTileClick(this);
    }

    public bool Shoot() {
        wasShot = true;
        UpdateView();

        return hasShip;
    }

    public void Kill() {
        dead = true;
        UpdateView();
    }

    //updates material
    private void UpdateView() {
        MeshRenderer _renderer = GetComponent<MeshRenderer>();
        shotSign.SetActive(wasShot);
        selectedSign.SetActive(Selected);
        if (!hasShip){
            _renderer.material = materials[0];
        } else{
            if (mine){
                if (!WasShot){
                    _renderer.material = materials[1];
                } else{
                    _renderer.material = dead ? materials[3] : materials[2];
                }
            } else{
                if (!wasShot){
                    _renderer.material = materials[0];
                } else{
                    _renderer.material = dead ? materials[3] : materials[2];
                }
            }
        }
    }

    //saves all the informtion needed about this tile
    public string Serialize() {
        return x + ":" + y + ":" + mine + ":" + hasShip + ":" + wasShot + ":" + dead + ":" + selected;
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
    }
    public bool WasShot {
        get {
            return wasShot;
        }
        set {
            wasShot = value;
        }
    }
    public bool Selected {
        get {
            return selected;
        }
        set {
            selected = value;
            UpdateView();
        }
    }
}