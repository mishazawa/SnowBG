using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour
{
    private float VECTOR_DEFAULT_SCALE = 100f;

    private Camera cam;
    private SkiFree controls;
    private Rigidbody rb;

    private Vector3 lookDirection = Vector3.forward;
    private Vector3 movePosition;

    public  float speed = 1f;
    private float closeDistance = 0.5f;
    private bool isMoving = false;


    void Awake(){
        controls = new SkiFree();
        controls.Player.Fire.started += Move;
    }

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() {
        controls.Player.Enable();
    }

    private void OnDisable() {
        controls.Player.Disable();
    }

    void Move(InputAction.CallbackContext ctx) {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            isMoving = true;
            movePosition = hit.point;
            var tv = (transform.position - movePosition).normalized * VECTOR_DEFAULT_SCALE;

            lookDirection.Set(tv.x, transform.position.y, tv.z);
        }
    }

    void FixedUpdate() {
        if (isMoving) {
            float sqrLen = (movePosition - rb.position).sqrMagnitude;
            if (sqrLen >= closeDistance) {
                rb.MoveRotation(Quaternion.LookRotation(lookDirection));
                rb.MovePosition(rb.position - transform.forward * speed * Time.deltaTime);
            } else {
                isMoving = false;
            }
        }
    }

    public void AttachCamera(Camera c) {
        cam = c;
        cam.GetComponent<FollowCamera>().SetTarget(gameObject);
    }
}
