using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    private CustomInput input = null;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D _rb = null;
    public float moveSpeed = 10f;

    private void Awake() {
        input = new CustomInput();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        input.Enable();
        input.Move.Movement.performed += OnMovementPerformed;
        input.Move.Movement.canceled += OnMovementCancelled;
    }

    private void OnDisable() {
        input.Disable();
        input.Move.Movement.performed -= OnMovementPerformed;
        input.Move.Movement.canceled -= OnMovementCancelled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }

    private void FixedUpdate() {
        _rb.velocity = moveVector * moveSpeed;
    }

    void Update()
    {
        /*
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        transform.up = direction;


        Vector3 movement = new Vector3(
            
            Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.Q) ? -1 : 0,
            Input.GetKey(KeyCode.Z) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0,
            0f
        );

        transform.position += movement * playerVelocity * Time.deltaTime;*/
    }
}
