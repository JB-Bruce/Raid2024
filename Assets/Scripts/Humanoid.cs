using UnityEngine;
using UnityEngine.AI;

public class Humanoid : MonoBehaviour
{
    public bool isPlayer = false;

    public float life = 100;
    public Faction faction;
    public bool isDead = false;
    public float removeDeathReputation = -0.5f;
    public float removeHitReputation = -1;

    public ParticleSystem pSystem;

    private Transform _transform;

    [SerializeField] private Animator _feetAnimator;
    protected Rigidbody2D _rb;
    private NavMeshAgent _agent;

    private MovePlayer _player;
    private float _reduceDamage = 0;

    private FactionManager _factionManager;

    protected virtual void Start()
    {
        _player = MovePlayer.instance;
        _factionManager = FactionManager.Instance;
        _rb = GetComponent<Rigidbody2D>();
        _transform = transform;

        if (!isPlayer)
        {
            _agent = GetComponent<NavMeshAgent>();
        }
    }

    protected virtual void Update()
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

    // remove life to him self and return true if he is dead
    public bool TakeDamage(float damage, Faction _faction, Vector2 fwd)
    {
        
        life -= damage;

        if (faction != Faction.Player)
        {
            _factionManager.AddReputation(faction, _faction, removeHitReputation);
        }

        pSystem.Play();

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
        GetComponent<Rigidbody2D>().AddForce(fwd * 10, ForceMode2D.Impulse);

        if (life <= 0 && !isDead)
        {
            isDead = true;
            Death(_faction);
        }
        return isDead;
    }

    // When a unit Die
    private void Death(Faction _faction)
    {
        if (!isPlayer)
        {
            _factionManager.RemoveUnitFromFaction(faction, this.gameObject);
            _factionManager.AddReputation(faction, _faction, removeDeathReputation);
            _factionManager.ChangeAllReputation(_faction, faction);
            Animator _anim = GetComponent<Animator>();
            //_anim.enabled = true;
            if(Random.Range(0,2)  == 0)
            {
                _anim.Play("DeathL");
            }
            else
            {
                _anim.Play("DeathR");
            }
            RemoveUnitComponent();
        }

        if(_faction == Faction.Player)
        {
            QuestManager.instance.CheckQuestKill(faction);
        }
    }

    //Destroy All the composent of a unit when he die
    public void RemoveUnitComponent()
    {
        Destroy(GetComponent<UnitBT>());
        Destroy(_agent);
        Transform _bodyAnim = _transform.GetChild(0);
        _bodyAnim.GetChild(0).gameObject.SetActive(false);
        _bodyAnim.GetChild(1).GetComponentInChildren<Animator>().enabled = false;
        _bodyAnim.GetChild(2).gameObject.SetActive(false);

        Destroy(GetComponent<UnitCombat>());
        Destroy(GetComponent<UnitMovement>());
    }


    // Set the animation to run or walk
    protected void MakeRun(bool isRunning)
    {
        _feetAnimator.SetBool("isRunning", isRunning);
    }
}
