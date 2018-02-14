using UnityEngine;
using UnityEngine.UI;

public class ShipTile : MonoBehaviour {
    [HideInInspector] public int length;
    public int shipsLength = 1;
    [SerializeField] private Text text;

    private void Start() {
        length = transform.childCount - 1;
        UpdateText();
    }    

    public void UpdateText() {
        text.text = "x " + length;
    }
}