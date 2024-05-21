using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    private CustomInput input = null;
    private Vector2 moveVector = Vector2.zero;
    private Rigidbody2D _rb = null;
    private SpriteRenderer _sprite = null;
    private bool _isRunning = false;
    public GameObject weapon;
    public float moveSpeed = 7f;


    //If the player press the button assigned for run, change the bool _isRunning. 
    //If the player release the button , change the bool again.
    public void sprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isRunning =true;
        }
        else if (context.canceled)
        {
            _isRunning =false;
        }
    }


    //When the game is play, it's the first thing who is done.
    //Instantiate CustomInput, Rigidbody2D, SpriteRenderer
    private void Awake() {
        input = new CustomInput();
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }


    //This function is called when the object becomes enabled and active. Enable move input
    private void OnEnable() {
        input.Move.Enable();
        
    }


    //This function is called when the object becomes disabled. Disable move input
    private void OnDisable() {
        input.Move.Disable();
    }


    //Get a direction with the input for the move,  and set the speed move (on that direction) depending on if the player sprint.   
    private void FixedUpdate() 
    {
        moveVector = input.Move.Movement.ReadValue<Vector2>();
        if(_isRunning == true)
        {
            _rb.velocity = moveVector * moveSpeed * 1.5f;
        }
        else
        {
            _rb.velocity = moveVector * moveSpeed;
        }
    }


    //Look where the mouse is, and determine the position of the weapon
    //The weapon is rotate of 90° for follow the mouse correctly
    //if the mouse is to the left of the player, flip the sprite of the player
    void Update()
    {
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(
            mousePosition.x - weapon.transform.position.x,
            mousePosition.y - weapon.transform.position.y
        );

        weapon.transform.up = direction;
        Vector3 angle = new Vector3(0,0,90);
        weapon.transform.Rotate(-angle);



        if (transform.position.x > mousePosition.x)
        {
            _sprite.flipX = true;
        }
        else
        {
            _sprite.flipX = false;
        }
        


    }
}
