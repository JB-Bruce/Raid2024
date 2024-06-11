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

        if(!isPlayer)
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

        if (life <= 0 && !isDead)
        {
            isDead = true;
            Death(_faction);
        }
        GetComponent<Rigidbody2D>().AddForce(fwd * 10, ForceMode2D.Impulse);
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
            Destroy(this.gameObject);
        }

        if(_faction == Faction.Player)
        {
            QuestManager.instance.CheckQuestKill(faction);
        }
    }

    // Set the animation to run or walk
    protected void MakeRun(bool isRunning)
    {
        _feetAnimator.SetBool("isRunning", isRunning);
    }
}
