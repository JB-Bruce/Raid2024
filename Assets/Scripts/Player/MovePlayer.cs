using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class MovePlayer : MonoBehaviour
{
    [SerializeField]
    private WeaponAttack _weaponAttack;

    private Item _lastWeaponEquiped;

    [SerializeField]
    private GameObject _weaponGameObject;

    [SerializeField]
    private PlayerInput _input;
    private InputActionMap _inGameActionMap;
    private Vector2 _moveVector = Vector2.zero;
    private Rigidbody2D _rb = null;
    private SpriteRenderer _sprite = null;
    
    [SerializeField]
    private Inventory _inventory;

    [SerializeField]
    private TextMeshProUGUI _numberAmmoWeapon;

    [SerializeField]
    private MeleeWeapon _handAttack;

    [SerializeField] private UnitSoundPlayer _unitSoundPlayer;

    [SerializeField]
    private GameObject _body;

    [SerializeField]
    private GameObject _hip;

    [SerializeField]
    private Sprite _WomanBody;

    [SerializeField]
    private Sprite _WomanHip;

    private SoundManager _soundManager;

    public static MovePlayer instance;
    private Animator _animator;
    
    private bool _isSprinting = false;
    private bool _isAiming = false;
    private bool _isParrying = false;
    private bool _tryToParrying = false;
    private bool _mouseActive = true;
    private bool _tryToHit = false;
    private bool _isReloading = false;
    private bool _cancelReload = false;
    public float moveSpeed = 7f;
    private int _selectedWeapon = 0;
    private int oldAmmo = 0;
    private float _parryingReduceDamage = 15f;
    string maxBullet;
    int oldSelectedWeapon = 0;
    int ammoRemoved;

    Vector2 direction = new Vector2(0,0);
    Vector2 lastAimDirection;
    Vector3 mousePosition;
    Vector3 lastMousePosition;

    [SerializeField]
    LineRenderer _lineRenderer;

    StatsManager stats;
    Inventory inventory;

    [SerializeField]
    AimLaser Laser;

    private Weapon _equipedWeapon;

    private float _timer = 0;

    private static readonly Quaternion _normalRotation = new Quaternion(0,0,0,0);
    private static readonly Quaternion _flipRotation = new Quaternion(0, 180, 0, 0);
    private void Start()
    {
        if (PlayerPrefs.GetString("Gender") == "Woman")
        {
            _hip.GetComponent<SpriteRenderer>().sprite = _WomanHip;
            _body.GetComponent<SpriteRenderer>().sprite = _WomanBody;
        }

        _soundManager = SoundManager.instance;

        _unitSoundPlayer = GetComponent<UnitSoundPlayer>();

        _inGameActionMap = _input.actions.FindActionMap("InGame");
        _weaponAttack.Init();
        _animator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        WeaponSelected();
    }

    //If the player press the button assigned for run, change the bool _isRunning. 
    //If the player release the button , change the bool again.
    public void sprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isSprinting = true;
        }
        else if (context.canceled)
        {
            _isSprinting = false;
        }
    }



    //When the game is play, it's the first thing who is done.
    //Instantiate CustomInput, Rigidbody2D, SpriteRenderer
    private void Awake() {
        if (instance == null) 
        { 
            instance = this;
        }
        _rb = GetComponent<Rigidbody2D>();
        _sprite = transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>();
        stats = GetComponent<StatsManager>();
        inventory = Inventory.Instance;
    }

    //Get a direction with the _input for the move,  and set the speed move (on that direction) depending on if the player sprint.
    private void Move()
    {
        float weightDebuff = WeightDebuff();

        _moveVector = UserInput.instance.MoveInput;
        bool isMoving = _moveVector != new Vector2(0,0);
        if (isMoving)
        {
            if (_isSprinting == true && stats.GetStamina() > 0 && weightDebuff > 0.10f)
            {
                stats.ChangeIsSprinting(true);
                _rb.velocity = _moveVector * moveSpeed * (weightDebuff - 0.05f) * 1.5f;
            }
            else
            {
                stats.ChangeIsSprinting(false);
                _rb.velocity = _moveVector * moveSpeed * weightDebuff;
            }
        }
        else
        {
            stats.ChangeIsSprinting(false);
            _rb.velocity = Vector2.zero;
        }
        
        //Debug.Log(_rb.velocity);
    }

    public float WeightDebuff()
    {
        float weightDebuff = 1f;

        switch (inventory.mass)
        {
            case < 45f:
                weightDebuff = 1f;
                break;
            case < 60f:
                weightDebuff = 0.90f;
                break;
            case < 80f:
                weightDebuff = 0.80f;
                break;
            case >= 80f:
                weightDebuff = 0.10f;
                break;
        }

        return weightDebuff;
    }


    //Flip the sprite of the player when the direction is highter or lower of 0
    private void FlipPlayer()
    {
        if (direction.x < 0)
        {
            _sprite.transform.rotation = _flipRotation;
        }
        else
        {
            _sprite.transform.rotation = _normalRotation;
        }
    }

    public void SetVisibleTextAmmo(bool isVisible)
    {
        _numberAmmoWeapon.gameObject.SetActive(isVisible);
    }

    //The player can shoot only if he is aiming  
    public void WeaponHit(InputAction.CallbackContext context)
    {
        if(context.started && !_tryToHit)
        {
            
            if(_isAiming || !_weaponAttack.IsRangedWeapon)
            {
                _tryToHit = true;
                if (_mouseActive == false)
                {
                    Vector2 verifDirectionManette = direction*10;
                    if(verifDirectionManette.x > 0.2 || verifDirectionManette.x < -0.2 || verifDirectionManette.y > 0.2 || verifDirectionManette.y < -0.2)
                    {
                        //Shoot with controller

                    }
                }
                else
                {
                    if(direction.x > 1.3 || direction.x < -1.3 || direction.y > 1.3 || direction.y < -1.3)
                    {
                        //Shoot with mouse and keyboard
                        
                    }
                }
                
            }
        }
        else if (context.canceled)
        {
            _tryToHit = false;
        }
        
    }

    //Look where the mouse is, and determine the position of the rangedWeapon
    //The rangedWeapon is rotate of 90° for follow the mouse correctly
    //if the mouse is to the left of the player, flip the sprite of the player
    private void WeaponAimDirection()
    {
        mousePosition = Input.mousePosition;
        float distance = Vector3.Distance(lastMousePosition, mousePosition);
        if (distance > 5)
        {
            _mouseActive=true;
            Cursor.visible = true;
        }

        Vector2 aimNotActive = new Vector2(0,0);

        if (_input.actions.FindAction("Aim").ReadValue<Vector2>() != aimNotActive)
        {
            direction = _input.actions.FindAction("Aim").ReadValue<Vector2>();
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
                mousePosition.x - _weaponGameObject.transform.position.x,
                mousePosition.y - _weaponGameObject.transform.position.y
                );

                
                
                lastMousePosition = Input.mousePosition;
            }

        }

        if (direction == aimNotActive){
            
            direction = lastAimDirection;
            
        }

        

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _weaponGameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
        
        
    }

    //Select the previous weapon equipped. If the index of the actual weapon is 1, then become 0. If the index of the actual weapon is 0, then become 2. 
    public void SelectWeaponLeft(InputAction.CallbackContext context)
    {
        
        if(context.started)
        {
            if(inventory.equipementSlots.Last().Item != null)
            {
                oldSelectedWeapon = _selectedWeapon;
                inventory.weaponSlots[_selectedWeapon].GetSelected(false);

                if (_selectedWeapon ==  0)
                {
                    if(inventory.equipementSlots.Last().Item.ToString() == "HolsterTier3 (Holster)") 
                    {
                        
                        _selectedWeapon = 2;
                    }

                    if(inventory.equipementSlots.Last().Item.ToString() == "HolsterTier2 (Holster)")
                    {
                        _selectedWeapon = 1;
                    }
                }
                else
                {
                    _selectedWeapon = _selectedWeapon - 1;
                    
                }
                
                inventory.weaponSlots[_selectedWeapon].GetSelected(true);

                if(oldSelectedWeapon != _selectedWeapon)
                {
                    if (inventory.weaponSlots[_selectedWeapon].Item is RangedWeapon rangedWeapon)
                    {
                        UpdateAmmoNumber(rangedWeapon);
                    }
                }
            }
            AmmoWhenChangeWeapon();
        }
    }

    //Select the next weapon equipped. If the index of the actual weapon is 1, then become 2. If the index of the actual weapon is 2, then become 0. 
    public void SelectWeaponRight(InputAction.CallbackContext context)
    {
        if(context.started)
        {  
            if(inventory.equipementSlots.Last().Item != null)
            {
                oldSelectedWeapon = _selectedWeapon;
                inventory.weaponSlots[_selectedWeapon].GetSelected(false);

                if(inventory.equipementSlots.Last().Item.ToString() == "HolsterTier3 (Holster)") 
                {
                    if(_selectedWeapon == 2)
                    {
                        _selectedWeapon = 0;
                    }
                    else
                    {
                        _selectedWeapon = _selectedWeapon + 1;
                    }
                }
                if(inventory.equipementSlots.Last().Item.ToString() == "HolsterTier2 (Holster)")
                {
                    if(_selectedWeapon == 1)
                    {
                        _selectedWeapon = 0;
                    }
                    else
                    {
                        _selectedWeapon = _selectedWeapon + 1;
                    }
                }

                inventory.weaponSlots[_selectedWeapon].GetSelected(true);

                if(oldSelectedWeapon != _selectedWeapon)
                {
                    if (inventory.weaponSlots[_selectedWeapon].Item is RangedWeapon rangedWeapon)
                    {  
                        UpdateAmmoNumber(rangedWeapon);
                    }
                }
            }
            AmmoWhenChangeWeapon();
        }
    }

    //Select the first weapon equipped
    public void SelectDefaultWeapon()
    {
        oldSelectedWeapon = _selectedWeapon;
        if(oldSelectedWeapon != _selectedWeapon)
        {
            if (inventory.weaponSlots[_selectedWeapon].Item is RangedWeapon rangedWeapon)
            {   
                UpdateAmmoNumber(rangedWeapon);
            }
        }
        inventory.weaponSlots[_selectedWeapon].GetSelected(false);
        _selectedWeapon = 0;
        inventory.weaponSlots[_selectedWeapon].GetSelected(true);

        AmmoWhenChangeWeapon();
    }

    //With a player input, select the first weapon 
    public void SelectFirstWeapon(InputAction.CallbackContext context)
    {
    
        if(context.started)
        {
            SelectDefaultWeapon();
        }

    }

    //With a player input, select the second weapon equipped. If the player don't have a holster2 equipped select the first weapon
    public void SelectSecondWeapon(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(inventory.equipementSlots.Last().Item != null)
            {
                oldSelectedWeapon = _selectedWeapon;
                inventory.weaponSlots[_selectedWeapon].GetSelected(false);
                if(inventory.equipementSlots.Last().Item.ToString() != "HolsterTier2 (Holster)" 
                && inventory.equipementSlots.Last().Item.ToString() != "HolsterTier3 (Holster)")
                {
                    _selectedWeapon = 0;
                }
                else
                {
                    _selectedWeapon = 1;
                }
                inventory.weaponSlots[_selectedWeapon].GetSelected(true);


                if(oldSelectedWeapon != _selectedWeapon)
                {
                    if (inventory.weaponSlots[_selectedWeapon].Item is RangedWeapon rangedWeapon)
                    {    
                        UpdateAmmoNumber(rangedWeapon);
                    }
                }
            }
            AmmoWhenChangeWeapon();
        }
    }

    //With a player input, select the third weapon equipped. If the player don't have a holster3 equipped, select the 2nd one if it's a holster2, otherwise select the first weapon
    public void SelectThirdWeapon(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(inventory.equipementSlots.Last().Item != null)
            {
                inventory.weaponSlots[_selectedWeapon].GetSelected(false);
                oldSelectedWeapon = _selectedWeapon;
                if(inventory.equipementSlots.Last().Item.ToString() != "HolsterTier3 (Holster)")
                {
                    if(inventory.equipementSlots.Last().Item.ToString() != "HolsterTier2 (Holster)")
                    {
                        _selectedWeapon = 0;
                    }
                    else
                    {
                        _selectedWeapon = 1;
                    }
                }
                else
                {
                    _selectedWeapon = 2;
                }
                inventory.weaponSlots[_selectedWeapon].GetSelected(true);

                if(oldSelectedWeapon != _selectedWeapon)
                {
                    if (inventory.weaponSlots[_selectedWeapon].Item is RangedWeapon rangedWeapon)
                    {      
                        UpdateAmmoNumber(rangedWeapon);
                    }
                }
            }
            AmmoWhenChangeWeapon();
        }
    }

    //Getter for the index of the selected weapon
    public int GetSelectedWeapon()
    {
        return _selectedWeapon;
    }

    //With a player input, change the weapon selected between the three weapon equipped. Check the holster equipped for verify if the player can switch weapon.
    public void ChangeWeaponWithScroll(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            
            var scrollValue = context.ReadValue<float>();
            oldSelectedWeapon = _selectedWeapon;
            if (scrollValue < 0)
            {
                if(inventory.equipementSlots.Last().Item != null)
                {
                    inventory.weaponSlots[_selectedWeapon].GetSelected(false);

                    if(inventory.equipementSlots.Last().Item.ToString() == "HolsterTier3 (Holster)") 
                    {
                        if(_selectedWeapon == 2)
                        {
                            _selectedWeapon = 0;
                        }
                        else
                        {
                            _selectedWeapon = _selectedWeapon + 1;
                        }
                    }

                    if(inventory.equipementSlots.Last().Item.ToString() == "HolsterTier2 (Holster)")
                    {
                        if(_selectedWeapon == 1)
                        {
                            _selectedWeapon = 0;
                        }
                        else
                        {
                            _selectedWeapon = _selectedWeapon + 1;
                        }
                    }

                    inventory.weaponSlots[_selectedWeapon].GetSelected(true);
                }
            
            }
            else if (scrollValue > 0)
            {
                if(inventory.equipementSlots.Last().Item != null)
                {
                    inventory.weaponSlots[_selectedWeapon].GetSelected(false);

                    if (_selectedWeapon ==  0)
                    {
                        if(inventory.equipementSlots.Last().Item.ToString() == "HolsterTier3 (Holster)") 
                        {
                            
                            _selectedWeapon = 2;
                        }

                        if(inventory.equipementSlots.Last().Item.ToString() == "HolsterTier2 (Holster)")
                        {
                            _selectedWeapon = 1;
                        }
                    }
                    else
                    {
                        _selectedWeapon = _selectedWeapon - 1;
                        
                    }
                    
                    inventory.weaponSlots[_selectedWeapon].GetSelected(true);
                }
            }
            AmmoWhenChangeWeapon();
        }
    }

    private void AmmoWhenChangeWeapon()
    {
        if(oldSelectedWeapon != _selectedWeapon)
        {
            
            //_cancelReload = true;

            if(_isReloading)
            {
                StopAllCoroutines();

                if (inventory.weaponSlots[_selectedWeapon].Item is RangedWeapon rangedWeapon)
                {
                    UpdateAmmoNumber(rangedWeapon);

                    _animator.Play(rangedWeapon.animIdle, 0, 0);    
                }
                else
                {
                    _animator.Play(_handAttack.animIdle, 0, 0);
                }

                if (inventory.weaponSlots[oldSelectedWeapon].Item is RangedWeapon oldRangedWeapon)
                {
                    //StopCoroutine(CouroutineReload(oldRangedWeapon));
                    for(int i = 0; i < ammoRemoved; i++)
                    {
                        inventory.AddItem(oldRangedWeapon.BulletType[0]);
                    }
                    PopUpManager.Instance.AddPopUp(oldRangedWeapon.BulletType[0], ammoRemoved);
                }
                inventory.weaponSlots[oldSelectedWeapon].ammoQuantity = oldAmmo;
                
            }
            else
            {
                if (inventory.weaponSlots[_selectedWeapon].Item is RangedWeapon rangedWeapon)
                {
                    UpdateAmmoNumber(rangedWeapon);   
                }
            }
            _isReloading = false;          
        }
    }


    //Change the boolean _isAiming if the player is aiming
    public void WeaponAimLaser(InputAction.CallbackContext context)
    {
        if(context.started && !_isAiming)
        {
            _isAiming = true;
        }
        else if(context.canceled)
        {
            _isAiming = false;
        }

    }

    // Change bool _isParrying if the player try to parrying attack
    public void Parrying(InputAction.CallbackContext context)
    {
        if(context.started && !_weaponAttack.IsRangedWeapon) 
        {
            _tryToParrying = true;
        }
        else if(context.canceled)
        {
            _tryToParrying= false;
        }
    }


    //If the player have a rangedWeapon equipped, get the weapon direction (with mouse or gamepad) and shoot a laser
    //The laser don't go throught box collider
    private void LaserShoot()
    {
        if (inventory.weaponSlots[_selectedWeapon].Item != null && inventory.weaponSlots[_selectedWeapon].Item.GetType().Name == "RangedWeapon")
        {
            Vector2 lastDirection = direction;
            if(_isAiming  && !_isReloading)
            {
                direction = direction *10;
                if(_mouseActive == false)
                {
                    if(direction.x > 0.2 || direction.x < -0.2 || direction.y > 0.2 || direction.y < -0.2)
                    {
                       _lineRenderer.enabled =true;
                    }
                    else
                    {
                        _lineRenderer.enabled =false;
                    }
                }
                else
                {
                    if(direction.x > 1.3 || direction.x < -1.3 || direction.y > 1.3 || direction.y < -1.3)
                    {
                        _lineRenderer.enabled =true;
                    }
                    else
                    {
                        _lineRenderer.enabled =false;
                    }
                }

            }
            else
            {
                _lineRenderer.enabled =false;
            }
            Laser.ShootLaser(direction);
            direction = lastDirection;
        }
    }


    //Active the actual weapon of the player have equipped (rangeWeapon or meleeWeapon) and desactive other weapons
    private void WeaponSelected()
    {
        
        _lineRenderer.enabled =false;

        if(_lastWeaponEquiped == inventory.weaponSlots[_selectedWeapon].Item && _lastWeaponEquiped != null)
        { 
            return; 
        }

        _lastWeaponEquiped = inventory.weaponSlots[_selectedWeapon].Item;
        if (inventory.weaponSlots[_selectedWeapon].Item == null)
        {
            _numberAmmoWeapon.enabled = false;
            _weaponAttack.EquipWeapon(_handAttack);
            //_weaponAttack.EquipWeapon(Fist);
        }
        else
        {
            if(inventory.weaponSlots[_selectedWeapon].Item is RangedWeapon rangedWeapon)
            {
                UpdateAmmoNumber(rangedWeapon);
                
                _weaponAttack.EquipWeapon(rangedWeapon);
                

            }
            else if(inventory.weaponSlots[_selectedWeapon].Item is MeleeWeapon meleeWeapon)
            {
                _numberAmmoWeapon.enabled = false;
                _weaponAttack.EquipWeapon(meleeWeapon);
            }
        }
    }

    //Change the text of the current ammo, next to the weapons
    public void UpdateAmmoNumber(RangedWeapon rangedWeapon)
    {
        maxBullet = rangedWeapon.MaxBullet.ToString();

        _numberAmmoWeapon.text = inventory.weaponSlots[_selectedWeapon].ammoQuantity + "/" + maxBullet;

        if(inventory.weaponSlots[_selectedWeapon].Item is RangedWeapon)
        {
            _numberAmmoWeapon.enabled = true;
        }
    }
    
    //Detect the input for reload
    public void ReloadAmmoInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            ReloadAmmo();
        }
    }

    //Change the actual ammo of the weapon, and call a couroutine for display the current ammo
    public void ReloadAmmo()
    {
        if(!_isReloading)
        {
            oldAmmo = inventory.weaponSlots[_selectedWeapon].ammoQuantity;
            if(inventory.weaponSlots[_selectedWeapon].Item is RangedWeapon rangedWeapon)
            {
                int numberOfAmmoInInventory = inventory.CountItemInInventory(rangedWeapon.BulletType[0]);
                
               
                if(numberOfAmmoInInventory > 0 && inventory.weaponSlots[_selectedWeapon].ammoQuantity != rangedWeapon.MaxBullet)
                {
                    ammoRemoved = 0;
                    _animator.Play(rangedWeapon.animReload, 0, 0);
                    _isReloading = true;
                    _soundManager.PlaySFX(_weaponAttack._equipedWeapon.reloadSFX, _unitSoundPlayer.unitAudioSource);

                    if(numberOfAmmoInInventory + inventory.weaponSlots[_selectedWeapon].ammoQuantity >= rangedWeapon.MaxBullet)
                    {
                        ammoRemoved = rangedWeapon.MaxBullet-inventory.weaponSlots[_selectedWeapon].ammoQuantity;
                        inventory.weaponSlots[_selectedWeapon].ammoQuantity = rangedWeapon.MaxBullet;
                    }
                    else
                    {
                        ammoRemoved = numberOfAmmoInInventory;
                        inventory.weaponSlots[_selectedWeapon].ammoQuantity = inventory.weaponSlots[_selectedWeapon].ammoQuantity + numberOfAmmoInInventory;
                    }
                    
                    inventory.RemoveItems(rangedWeapon.BulletType[0], ammoRemoved);

                    StartCoroutine(CouroutineReload(rangedWeapon));
                }
            }
        }
    }

    //Wait two second before displaying current ammo
    IEnumerator CouroutineReload(RangedWeapon rangedWeapon)
    {
        _lineRenderer.enabled =false;

        yield return new WaitForSeconds(2f);

        _isReloading = false;
        UpdateAmmoNumber(rangedWeapon);
    }

    

//Check on the 3 equipement slots what protection is eqquiped and if the player is parrying blow. Get for the three the amount of reduce damage and return it.
    public float CheckArmor()
    {
        float reduceDamage = 0;
        for (int i = 0; i < 3; i ++)
        {
            if(inventory.equipementSlots[i].Item != null)
            {
                Armor armor = (Armor)inventory.equipementSlots[i].Item;
                reduceDamage += armor.Protection;
            }
        }

        if (_isParrying && !_weaponAttack.IsRangedWeapon)
        {
            reduceDamage += _parryingReduceDamage;
        }
        return reduceDamage;
    }

    void Update()
    {
        if (_inGameActionMap.enabled)
        {
            _rb.velocity = Vector3.Normalize(_rb.velocity);
            Move();
            WeaponAimDirection();
            FlipPlayer();
            WeaponSelected();
            LaserShoot();

        }
        else
        {
            _moveVector = Vector2.zero;
            _rb.velocity = Vector3.zero;
        }

        if (_timer < Time.time) 
        {
            _isParrying = _tryToParrying;
            _weaponAttack.Animator.SetBool("IsParrying", _isParrying);

            if (_tryToHit && (_weaponAttack.IsRangedWeapon || !_tryToParrying) && !_isReloading)
            {


                if (inventory.weaponSlots[_selectedWeapon].Item is RangedWeapon rangedweapon)
                {
                    _equipedWeapon = rangedweapon;
                }
                else if(inventory.weaponSlots[_selectedWeapon].Item is MeleeWeapon meleeweapon)
                {
                    _equipedWeapon = meleeweapon;
                }
                else
                {
                    _equipedWeapon = _handAttack;
                }

                if (_equipedWeapon.IsSemiAuto)
                {
                    _tryToHit = false;
                }

                _timer = Time.time + _equipedWeapon.AttackSpeed;
                if(inventory.weaponSlots[_selectedWeapon].Item is RangedWeapon rangedWeapon)
                {
                    if(inventory.weaponSlots[_selectedWeapon].ammoQuantity != 0)
                    {
                        inventory.weaponSlots[_selectedWeapon].ammoQuantity -= 1;
                        UpdateAmmoNumber(rangedWeapon);
                        _weaponAttack.UseWeapon(direction, Faction.Player);
                    }
                    else
                    {
                        ReloadAmmo();
                    }
                    
                }
                else
                {
                    _cancelReload= false;
                    _weaponAttack.UseWeapon(direction, Faction.Player);
                }
            }
        }
    }
}
