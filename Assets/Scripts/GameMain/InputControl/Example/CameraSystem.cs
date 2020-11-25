using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour {

    public Transform target;
    protected Vector3 offset;
    protected Camera cam;
    public float speed;
    public FollowMode mode;

    private void Start() {
        cam = GetComponent<Camera>();
        offset = cam.transform.position - target.position;

    }

    private void Update() {
        if (mode == FollowMode.Absolute) {
            transform.position = target.position + offset;
        }

        if (mode == FollowMode.Lerp) {
            transform.position = Vector3.Lerp(
                transform.position,
                target.position + offset,
                Time.deltaTime * speed
                );
        }
    }

    public enum FollowMode {
        Absolute,
        Lerp
    }
}
