using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Humanoid : MonoBehaviour
{
    public bool isPlayer = false;
    [SerializeField] private CircleCollider2D _detectionZone;
    [SerializeField] private CircleCollider2D _lootZone;
    public bool MoveFeet = true;
    public bool CanRespawn = true;

    public RectTransform _slider;

    public float maxLife = 100;
    public float life = 100;
    public Faction faction;
    public bool isDead = false;
    public float removeDeathReputation = -0.5f;
    public float removeHitReputation = -1;

    public ParticleSystem pSystem;

    protected Transform _transform;

    [SerializeField] private Animator _feetAnimator;
    protected Rigidbody2D _rb;
    [SerializeField] protected NavMeshAgent _agent;

    private MovePlayer _player;
    private float _reduceDamage = 0;

    [SerializeField]
    private string _questType;

    private bool _canTakeDamageByPlayer = true;

    protected FactionManager _factionManager;

    protected virtual void Start()
    {
        _transform = transform;

        if(MoveFeet) 
        {
            _player = MovePlayer.instance;
            _factionManager = FactionManager.Instance;
            _rb = GetComponent<Rigidbody2D>();
        }
    }

    protected virtual void Update()
    {
        if(MoveFeet)
        {
            if (isPlayer)
            {
                _feetAnimator.SetFloat("Speed", _rb.velocity.sqrMagnitude);
            }
            else
            {
                _feetAnimator.SetFloat("Speed", _agent.velocity.sqrMagnitude);
            }
        }
    }

    // remove life to him self and return true if he is dead
    public bool TakeDamage(float damage, Faction _faction, Vector2 fwd)
    {
        if (!_canTakeDamageByPlayer && _faction != Faction.Player)
        {
            return false;
        }

        life -= damage;

        if (faction != Faction.Player && MoveFeet)
        {
            _factionManager.AddReputation(faction, _faction, removeHitReputation);
        }
        if(MoveFeet)
        {
            pSystem.Play();
            GetComponent<Rigidbody2D>().AddForce(fwd * 10, ForceMode2D.Impulse);
        }


        if (isPlayer) 
        {
            //if is this the player, get the amount of reduceDamage depending on the armors, and change the damage inflicted.
            _reduceDamage = _player.CheckArmor();
            if(_reduceDamage != 0)
            {
                _reduceDamage = _reduceDamage * damage / 100;
                life += _reduceDamage;
            }
            
            if(life <= 0)
            {
                life=0;
            }
            
            StatsManager.instance.ChangeLifeColor();
        }

        if (life <= 0 && !isDead && MoveFeet)
        {
            life = 0;
            isDead = true;
            Death(_faction);
        }
        else if (life <= 0 && !MoveFeet && !isDead) 
        {
            life = 0;
            Death(faction);
        }

        SetSlider();

        return isDead;
    }

    //modify the slider value
    public void SetSlider()
    {
        if (_slider != null)
        {
            _slider.localScale = new Vector3(life / maxLife, 1, 1);
        }
    }

    // When a unit Die
    protected virtual void Death(Faction _faction)
    {
        if (!isPlayer)
        {
            _factionManager.RemoveUnitFromFaction(faction, this.gameObject);
            _factionManager.AddReputation(faction, _faction, removeDeathReputation);
            _factionManager.ChangeAllReputation(_faction, faction);
            Animator _anim = GetComponent<Animator>();
            _anim.enabled = true;
            if (Random.Range(0, 2) == 0)
            {
                _anim.Play("DeathL");
            }
            else
            {
                _anim.Play("DeathR");
            }
            RemoveUnitComponent();

            _detectionZone.enabled = false;
            _lootZone.enabled = true;

            GetComponent<Container>().enabled = true;
        }

        if(_faction == Faction.Player)
        {
            QuestManager.instance.CheckQuestKill(faction, _questType);
        }
    }

    //Destroy All the composent of a unit when he die
    public void RemoveUnitComponent()
    {
        Destroy(GetComponent<UnitBT>());
        Destroy(_agent);
        Destroy(GetComponent<UnitCombat>());
        Destroy(GetComponent<UnitMovement>());
    }


    // Set the animation to run or walk
    protected void MakeRun(bool isRunning)
    {
        _feetAnimator.SetBool("isRunning", isRunning);
    }

    //modify the quest type
    public void SetQuestType(string questType)
    {
        _questType = questType;
    }

    public void SetCanTakeDamageByPlayer(bool canTakeDamageByPlayer)
    {
        _canTakeDamageByPlayer = canTakeDamageByPlayer;
    }
}
