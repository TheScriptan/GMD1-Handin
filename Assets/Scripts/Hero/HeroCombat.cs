using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HeroCombat : MonoBehaviour
{
    public enum HeroAttackType
    {
        Melee,
        Ranged
    }

    public HeroAttackType heroAttackType;

    public GameObject targetedEnemy;
    public float attackRange;
    public float rotateSpeedForAttack;

    private NavMeshAgent _agent;
    private HeroPlayerMovement _movement;
    private Stats _stats;
    private Animator _anim;
    private AudioSource _audioSource;

    public bool basicAtkIdle = false;
    public bool isHeroAlive;
    public bool performMeleeAttack = true;

    [Header("Ranged Variables")] public bool performRangedAttack = true;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _movement = GetComponent<HeroPlayerMovement>();
        _stats = GetComponent<Stats>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (targetedEnemy != null)
        {
            var dist = Vector3.Distance(transform.position, targetedEnemy.transform.position);
            if (dist > attackRange)
            {
                _agent.SetDestination(targetedEnemy.transform.position);
                _agent.stoppingDistance = attackRange;
            }
            else
            {
                //MELEE ATTACK
                if (heroAttackType == HeroAttackType.Melee)
                {
                    Quaternion rotationToLookAt = Quaternion.LookRotation(
                        targetedEnemy.transform.position - transform.position);
                    float rotationY = Mathf.SmoothDampAngle(
                        transform.eulerAngles.y,
                        rotationToLookAt.eulerAngles.y,
                        ref _movement.rotateVelocity,
                        rotateSpeedForAttack * (Time.deltaTime * 5));
                    transform.eulerAngles = new Vector3(0, rotationY, 0);

                    _agent.SetDestination(transform.position);
                    if (performMeleeAttack)
                    {
                        StartCoroutine(MeleeAttackInterval());
                    }
                }
                
                //RANGED ATTACK
                if (heroAttackType == HeroAttackType.Ranged)
                {
                    Quaternion rotationToLookAt = Quaternion.LookRotation(
                        targetedEnemy.transform.position - transform.position);
                    float rotationY = Mathf.SmoothDampAngle(
                        transform.eulerAngles.y,
                        rotationToLookAt.eulerAngles.y,
                        ref _movement.rotateVelocity,
                        rotateSpeedForAttack * (Time.deltaTime * 5));
                    transform.eulerAngles = new Vector3(0, rotationY, 0);

                    _agent.SetDestination(transform.position);
                    if (performRangedAttack)
                    {
                        StartCoroutine(RangedAttackInterval());
                    }
                }
            }
        }
    }

    private IEnumerator MeleeAttackInterval()
    {
        performMeleeAttack = false;
        _anim.SetBool("Basic Attack", true);

        yield return new WaitForSeconds(_stats.attackTime / ((100 + _stats.attackTime) * 0.01f));
        if (targetedEnemy == null)
        {
            _anim.SetBool("Basic Attack", false);
            performMeleeAttack = true;
        }
    }
    
    private IEnumerator RangedAttackInterval()
    {
        performRangedAttack = false;
        _anim.SetBool("Basic Attack", true);

        yield return new WaitForSeconds(_stats.attackTime / ((100 + _stats.attackTime) * 0.01f));
        if (targetedEnemy == null)
        {
            _anim.SetBool("Basic Attack", false);
            performRangedAttack = true;
        }
    }

    //Used by Animation events in Goblin Attack 5
    public void MeleeAttack()
    {
        if (targetedEnemy != null)
        {
            var targetable = targetedEnemy.GetComponent<Targetable>();
            if (targetable.enemyType == Targetable.EnemyType.Minion)
            {
                targetedEnemy.GetComponent<Health>().health -= _stats.attackDmg;
            }

            if (targetable.enemyType == Targetable.EnemyType.Hero)
            {
                targetedEnemy.GetComponent<Health>().health -= _stats.attackDmg;
            }
        }

        performMeleeAttack = true;
    }
    
    public void RangedAttack()
    {
        if (targetedEnemy != null)
        {
            var targetable = targetedEnemy.GetComponent<Targetable>();
            if (targetable.enemyType == Targetable.EnemyType.Minion)
            {
                _audioSource.Play();
                SpawnRangedProjectile("Minion", targetedEnemy);
            }

            if (targetable.enemyType == Targetable.EnemyType.Hero)
            {
                _audioSource.Play();
                SpawnRangedProjectile("Hero", targetedEnemy);
            }
        }

        performRangedAttack = true;
    }

    private void SpawnRangedProjectile(string typeOfEnemy, GameObject targetEnemyObj)
    {
        float dmg = _stats.attackDmg;

        var projectile = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, Quaternion.identity).GetComponent<RangedProjectile>();
        
        if (typeOfEnemy == "Minion" || typeOfEnemy == "Hero")
        {
            projectile.targetType = typeOfEnemy;
            projectile.target = targetEnemyObj;
            projectile.damage = dmg;
            projectile.targetSet = true;
        }
    }
}