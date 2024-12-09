using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{


    public NavMeshAgent agent;
    public float attackTimer = 0;
    public bool isContact = false;
    public bool isCooling = false;

    [SerializeField]
    public EnemyData enemyData;





    public float skillTimer = 0;
    public bool skilling = false;



    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void Start()
    {

    }

    public void Update()
    {
        if (Player.Instance.isDead)
        {
            return;
        }


        Move();
        if (isContact && !isCooling)
        {
            Attack();
        }


        if (isCooling)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0)
            {
                attackTimer = 0;
                isCooling = false;
            }
        }



        UpdateSkill();

    }

    public void SetElite()
    {
        enemyData.hp *= 2;
        enemyData.damage *= 2;

        GetComponent<SpriteRenderer>().color = new Color(255 / 255f, 113 / 255f, 113 / 255f);

    }

    private void UpdateSkill()
    {
        if (enemyData.skillTime < 0)
        {
            return;
        }


        if (skillTimer <= 0)
        {

            float dis = Vector2.Distance(transform.position, Player.Instance.transform.position);

            if (dis <= enemyData.range)
            {

                Vector2 dir = (Player.Instance.transform.position - transform.position).normalized;
                LaunchSkill(dir);

                skillTimer = enemyData.skillTime;
            }
        }
        else
        {
            skillTimer -= Time.deltaTime;


        }


    }

    public virtual void LaunchSkill(Vector2 dir)
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isContact = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isContact = false;
        }
    }



    public void Move()
    {
        if (skilling)
        {
            return;
        }

        Vector3 playerPos = Player.Instance.transform.position;
        Vector2 direction = (playerPos - transform.position).normalized;
        //transform.Translate(direction * enemyData.speed * Time.deltaTime);

        agent.SetDestination(playerPos);
        TurnAround();
    }


    public void TurnAround()
    {

        if (Player.Instance.transform.position.x - transform.position.x >= 0.1)
        {

            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (Player.Instance.transform.position.x - transform.position.x < 0.1)
        {

            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        }
    }


    //攻击
    public void Attack()
    {
        //如果攻击冷却, 则返回
        if (isCooling)
        {
            return;
        }

        Player.Instance.Injured(enemyData.damage);

        //攻击进入冷却
        isCooling = true;
        attackTimer = enemyData.attackTime;

    }


    //受伤
    public void Injured(float attack)
    {
        // if (isDead)
        // {
        //     return;
        // }

        //判断本次攻击是否会死亡
        if (enemyData.hp - attack <= 0)
        {
            enemyData.hp = 0;
            Dead();
        }
        else
        {
            enemyData.hp -= attack;
        }
    }

    //死亡
    public void Dead()
    {
        //增加玩家经验值
        GameManager.Instance.exp = enemyData.provideExp * GameManager.Instance.propData.expMuti;
        GamePanel.Instance.RenewExp();

        // 掉落金币
        Instantiate(GameManager.Instance.moeny_prefab, transform.position, Quaternion.identity);

        //销毁自己
        Destroy(gameObject);
    }

}