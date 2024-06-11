using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : Humanoid
{
    public static StatsManager instance;

    private MovePlayer _movePlayer;

    [SerializeField]
    private Transform _respawnPosition;

    public Image healthImage;

    private int hunger = 100;
    public Image waterBar;

    private int water = 100;
    public Image hungerBar;

    private float stamina = 50;
    public Image staminaBar;
    public Image staminaBorder;

    public Image DeathFade;
    public GameObject RespawnButtonFaction;
    public GameObject DeathScreen;
    public Transform MainCamera;

    [SerializeField] private float staminaDrainAmount = 10f;
    [SerializeField] private float staminaGainAmount = 5f;

    public bool _isSprinting;
    private bool _verifSprint;
    private bool _recupStamina;
    private FactionManager _factionManager;
    
    

    [SerializeField]
    private ERespawnFaction _respawnFaction = ERespawnFaction.Null;

    /*
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
    */

    //Change the life image color (is called on TakeDamage())
    public void ChangeLifeColor()
    {
        if (life <= 100 && life >= 76)
        {
            healthImage.color = Color.white;
        }

        if (life <= 75 && life >= 51)
        {
            healthImage.color = Color.yellow;
        }

        if (life <= 50 && life >= 26)
        {
            healthImage.color = new Color(1.0f,0.5f,0.0f);
        }

        if (life <= 25 && life >= 1)
        {
            healthImage.color = Color.red;
        }

        if (life <= 0)
        {

            //Death
            //Fade in/out for death screen
            if(_respawnFaction == ERespawnFaction.Survivalist)
            {
                if(_factionManager.GetReputation(Faction.Player, Faction.Survivalist) < -1)
                {
                    RespawnButtonFaction.SetActive(false);
                }
                else
                {
                    RespawnButtonFaction.SetActive(true);
                }
            }

            if(_respawnFaction == ERespawnFaction.Utopist)
            {
                if(_factionManager.GetReputation(Faction.Player, Faction.Utopist) < -1)
                {
                    RespawnButtonFaction.SetActive(false);
                }
                else
                {
                    RespawnButtonFaction.SetActive(true);
                }
            }

            if(_respawnFaction == ERespawnFaction.Scientist)
            {
                if(_factionManager.GetReputation(Faction.Player, Faction.Scientist) < -1)
                {
                    RespawnButtonFaction.SetActive(false);
                }
                else
                {
                    RespawnButtonFaction.SetActive(true);
                }
            }

            if(_respawnFaction == ERespawnFaction.Military)
            {
                if(_factionManager.GetReputation(Faction.Player, Faction.Military) < -1)
                {
                    RespawnButtonFaction.SetActive(false);
                }
                else
                {
                    RespawnButtonFaction.SetActive(true);
                }
            }


            Time.timeScale = 0.0f;
            DeathFade.CrossFadeAlpha(0,0.01f,true);
            DeathFade.enabled = true;
            DeathFade.CrossFadeAlpha(1,1f,true);
            

            StartCoroutine(CouroutineDeath());
        }
    }

    //Call this function when you want to heal the player. Change the color of the life image (depending on the life amount).
    public void AddHealth(int healthAdd)
    {
        if (life + healthAdd >= 100)
        {
            life = 100;
        }
        else
        {
            life += healthAdd;
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

    //return the amount of stamina
    public float GetStamina()
    {
        return stamina;
    }

    //Call when the player click on the  button on death screen 
    //set the player stats and respawn the player
    private void RespawnPlayer()
    {
        AddWater(100);
        AddFood(100);
        AddHealth(100);
        stamina = 50;
        staminaBar.fillAmount = stamina / 50f;
        transform.position = _respawnPosition.position;
        MainCamera.position = transform.position + new Vector3(0,0,-10);
        ChangeLifeColor();
        
        DeathFade.CrossFadeAlpha(0,0.01f,true);
        DeathFade.enabled = true;
        DeathFade.CrossFadeAlpha(1,1f,true);

        StartCoroutine(CouroutineRespawn());
    }

    public void RespawnDefault()
    {
        var factionActual = _respawnFaction;
        _respawnFaction = ERespawnFaction.Null;
        ChangeRespawnPoint();
        RespawnPlayer();
        _respawnFaction = factionActual;
    }

    public void RespawnFaction()
    {
        ChangeRespawnPoint();
        RespawnPlayer();
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

    //Desactive the fade and show the death screen
    IEnumerator CouroutineDeath()
    {
        yield return new WaitForSecondsRealtime(2f);

        
        DeathScreen.SetActive(true);
        DeathFade.CrossFadeAlpha(0,1f,true);
        
        //Time.timeScale = 1.0f;
        yield return new WaitForSecondsRealtime(1f);
        DeathFade.enabled = false;
    }

    //Desactive the death sreen and desactive the fade
    IEnumerator CouroutineRespawn()
    {
        yield return new WaitForSecondsRealtime(2f);
        
        DeathScreen.SetActive(false);
        
        DeathFade.CrossFadeAlpha(0,1f,true);
        
        yield return new WaitForSecondsRealtime(1f);
        DeathFade.enabled = false;
        Time.timeScale = 1.0f;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    protected override void Start() {
        base.Start();
        _movePlayer = MovePlayer.instance;
        _factionManager = FactionManager.Instance;
        RemoveFood();
        RemoveWater();
        ChangeRespawnPoint();
    }

    //Set _recupStamina to true, when 
    private void GainStamina()
    {
        _recupStamina = true;
    }

    protected override void Update()
    {
        base.Update();
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
                float weightDebuff = _movePlayer.WeightDebuff();

                stamina -= (staminaDrainAmount / weightDebuff) * Time.deltaTime;
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
        MakeRun(_isSprinting);
    }

    //Change the position of the respawn with the faction chosen by the player
    public void ChangeRespawnPoint()
    {
        if(FactionManager.Instance.factionRespawns.Count <=0)
            return;

        switch(_respawnFaction) 
        {
            case ERespawnFaction.Military:
                _respawnPosition = FactionManager.Instance.factionRespawns[0].RespawnTransform;
                break;

            case ERespawnFaction.Scientist:
                _respawnPosition = FactionManager.Instance.factionRespawns[3].RespawnTransform;
                break;

            case ERespawnFaction.Utopist:
                _respawnPosition = FactionManager.Instance.factionRespawns[1].RespawnTransform;
                break;

            case ERespawnFaction.Survivalist:
                _respawnPosition = FactionManager.Instance.factionRespawns[2].RespawnTransform;
                break;

            default:
                _respawnPosition = FactionManager.Instance.factionRespawns[4].RespawnTransform;
                break;
        }
    }




    //enum for the all the different faction respawn
    public enum ERespawnFaction 
    { 
        Military, 
        Utopist, 
        Survivalist, 
        Scientist,
        Null
    }
}
