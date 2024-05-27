using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MovePlayer : MonoBehaviour
{
    private CustomInput _input = null;
    public GameObject weapon;
    private Vector2 _moveVector = Vector2.zero;
    private Rigidbody2D _rb = null;
    private SpriteRenderer _sprite = null;
    
    [SerializeField]
    private Inventory _inventory;
    
    private bool _isSprinting = false;
    private bool _mouseActive = true;
    public float moveSpeed = 7f;

    Vector2 direction = new Vector2(0,0);
    Vector2 lastAimDirection;
    Vector3 mousePosition;
    Vector3 lastMousePosition;


    //If the player press the button assigned for run, change the bool _isRunning. 
    //If the player release the button , change the bool again.
    public void sprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isSprinting =true;
        }
        else if (context.canceled)
        {
            _isSprinting =false;
        }
    }



    //When the game is play, it's the first thing who is done.
    //Instantiate CustomInput, Rigidbody2D, SpriteRenderer
    private void Awake() {
        _input = new CustomInput();
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }


    //This function is called when the object becomes enabled and active. Enable move _input
    private void OnEnable() {
        _input.Player.Enable();
        _input.Player.Enable();
    }


    //This function is called when the object becomes disabled. Disable move _input
    private void OnDisable() {
        _input.Player.Disable();
        _input.Player.Disable();
    }


    //Get a direction with the _input for the move,  and set the speed move (on that direction) depending on if the player sprint.
    private void Move() 
    {
        _moveVector = _input.Player.Movement.ReadValue<Vector2>();
        if(_isSprinting == true)
        {
            _rb.velocity = _moveVector * moveSpeed * 1.5f;
        }
        else
        {
            _rb.velocity = _moveVector * moveSpeed;
        }
    }



    private void FlipPlayer()
    {
        if (direction.x < 0)
        {
            _sprite.flipX = true;
        }
        else
        {
            _sprite.flipX = false;
        }
    }

    //Look where the mouse is, and determine the position of the weapon
    //The weapon is rotate of 90° for follow the mouse correctly
    //if the mouse is to the left of the player, flip the sprite of the player
    private void WeaponAim()
    {
        mousePosition = Input.mousePosition;
        float distance = Vector3.Distance(lastMousePosition, mousePosition);
        if (distance > 5)
        {
            _mouseActive=true;
            Cursor.visible = true;
        }

        Vector2 aimNotActive = new Vector2(0,0);

        if (_input.Player.Aim.ReadValue<Vector2>() != aimNotActive)
        {
            direction = _input.Player.Aim.ReadValue<Vector2>();
            lastAimDirection = direction;
            
            Cursor.visible = false;
            _mouseActive=false;
        }
        else 
        {
            if(_mouseActive)
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                direction = new Vector2(
                mousePosition.x - weapon.transform.position.x,
                mousePosition.y - weapon.transform.position.y
                );
                lastMousePosition = Input.mousePosition;
            }

        }

        if (direction == aimNotActive){
            
            direction = lastAimDirection;
            
        }

        weapon.transform.up = direction;
        Vector3 angle = new Vector3(0,0,90);
        weapon.transform.Rotate(-angle);
    }



       
    private void FixedUpdate() 
    {
        if (!_inventory.isInventoryOpen)
        {
            Move();
        }
        else
        {
            _rb.velocity = Vector3.zero;
        }
    }

    void Update()
    {
        if (!_inventory.isInventoryOpen)
        {
            WeaponAim();

            FlipPlayer();
        }
    }
}
