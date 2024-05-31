using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance;

    [SerializeField]
    private Transform _respawnPosition;

    private int health = 100;
    public Image healthImage;

    private int hunger = 100;
    public Image waterBar;

    private int water = 100;
    public Image hungerBar;

    private float stamina = 50;
    public Image staminaBar;
    public Image staminaBorder;

    [SerializeField] private float staminaDrainAmount = 10f;
    [SerializeField] private float staminaGainAmount = 5f;

    public bool _isSprinting;
    private bool _verifSprint;
    private bool _recupStamina;

    //Change the life amount of the player, and change the color of the life image (depending on the life amount)
    public void TakeDamage(int damage)
    {

        if (health - damage < 0)
        {
            health = 0;
        }
        else
        {
            health = health - damage;
        }

        ChangeLifeColor();
        
    }

    //Change the life image color (is called on TakeDamage())
    public void ChangeLifeColor()
    {
        if (health <= 100 && health >= 76)
        {
            healthImage.color = Color.white;
        }

        if (health <= 75 && health >= 51)
        {
            healthImage.color = Color.yellow;
        }

        if (health <= 50 && health >= 26)
        {
            healthImage.color = new Color(1.0f,0.5f,0.0f);
        }

        if (health <= 25 && health >= 1)
        {
            healthImage.color = Color.red;
        }

        if (health == 0)
        {
            //Death
            AddWater(100);
            AddFood(100);
            AddHealth(100);
            stamina = 50;
            staminaBar.fillAmount = stamina / 50f;
            transform.position = _respawnPosition.position;
            ChangeLifeColor();
        }
    }

    //Call this function when you want to heal the player. Change the color of the life image (depending on the life amount).
    public void AddHealth(int healthAdd)
    {
        if (health + healthAdd >= 100)
        {
            health = 100;
        }
        else
        {
            health += healthAdd;
        }

        ChangeLifeColor();
        
    }

    //Call this function when you want to add food to the player.
    public void AddFood(int foodAdd)
    {
        if(hunger + foodAdd >= 100)
        {
            hunger = 100;
        }
        else
        {
            hunger += foodAdd;
        }
        hungerBar.fillAmount = hunger / 100f;
    }

    //Call a couroutine who decrease the food. Is called again at the end of the couroutine.
    public void RemoveFood()
    {
        StartCoroutine(CouroutineFood());
        
    }

    //Call this function when you want to add water to the player.
    public void AddWater (int waterAdd)
    {
        if(water + waterAdd >= 100)
        {
            water = 100;
        }
        else
        {
            water += waterAdd;
        }

        waterBar.fillAmount = water / 100f;

    }

    //Call a couroutine who decrease the water. Is called again at the end of the couroutine.
    public void RemoveWater()
    {
        StartCoroutine(CouroutineWater());
        
    }

    //Change the bool _isSprinting when you call the function
    public bool ChangeIsSprinting(bool _isRunning)
    {
        _isSprinting = _isRunning;
        return _isSprinting;
    }

    public float GetStamina()
    {
        return stamina;
    }
    
    //Reduce the water every 10 seconds
    IEnumerator CouroutineWater()
    {
        yield return new WaitForSeconds(10f);
        water -= 1;
        waterBar.fillAmount = water / 100f;
        RemoveWater();
    }

    //Reduce the water every 15 seconds
    IEnumerator CouroutineFood()
    {
        yield return new WaitForSeconds(15f);
        hunger -= 1;
        hungerBar.fillAmount = hunger / 100f;
        RemoveFood();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start() {
        RemoveFood();
        RemoveWater();
    }

    //Set _recupStamina to true, when 
    private void GainStamina()
    {
        _recupStamina = true;
    }

    private void Update()
    {
        //When the player is sprinting, decrease the stamina account, and change _recupStamina to false
        if  (_isSprinting == true)
        {
            if(_verifSprint == false && _recupStamina == false)
            {
                CancelInvoke("GainStamina");
            }
            
            _recupStamina = false;
            if(stamina > 0)
            {
                stamina -= staminaDrainAmount * Time.deltaTime;
            }

            staminaBar.fillAmount = stamina / 50f;
            
        }

        //if the player have just stop sprinting, the player need to wait before recup stamina. Else, increase stamina over time.
        else
        {
            if(_verifSprint)
            {
                Invoke("GainStamina", 2f);
            }
            if(_recupStamina)
            {
                if(stamina < 50)
                {
                    stamina += staminaGainAmount * Time.deltaTime;
                }

                staminaBar.fillAmount = stamina / 50f;
            }
        }

        if (stamina >=50)
        {
            staminaBar.CrossFadeAlpha(0,0.25f,false);
            staminaBorder.CrossFadeAlpha(0,0.25f,false);
        }
        else
        {
            staminaBar.CrossFadeAlpha(1,0.1f,false);
            staminaBorder.CrossFadeAlpha(1,0.1f,false);
        }

        _verifSprint = _isSprinting;
    }

}
