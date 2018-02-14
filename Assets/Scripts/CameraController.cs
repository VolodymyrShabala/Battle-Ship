using UnityEngine;

public class CameraController : MonoBehaviour {
    private Camera _camera;
    public bool canScroll;
    private void Start() {
        _camera = Camera.main;
    }
    private void Update() {
        if (!canScroll){
            return;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0){
            _camera.orthographicSize += Time.deltaTime * 7;
            if (_camera.orthographicSize > 10){
                _camera.orthographicSize = 10;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0){
            _camera.orthographicSize -= Time.deltaTime * 7;
            if (_camera.orthographicSize < 3){
                _camera.orthographicSize = 3;
            }
        }
    }
}