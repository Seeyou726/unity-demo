using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyStateType
{
    idle, patrol, chase, walk_back, attack_1, attack_2, attack_3,backToBornPoint,beAttacked,drop,airUnable
}
public class EnemyParameter  //角色属性参数
{
    public bool isGroud = false;
    public float enemyMoveSpeed = 5;
    public float enemyJumpSpeed = 40;
    public float hp = 200;
    public Rigidbody2D enemyRigidbody;
    public Animator enemyAnimator;
    public float jumpTimer;  //跳跃计时器
    public bool currentAnimEnd;  //当前动画是否结束
    public bool creatAttackTrigger;  //攻击生成触发器
    public bool destroyAttackTrigger;  //销毁攻击触发器
    public bool goAttack_2;
    public bool goAttack_3;
    public float actTimer;  //状态计时器
    public Vector2 enemyAlertRange = new Vector2(20, 5);  //警戒范围
    public Vector2 enemyGoAttackRange = new Vector2(2, 1);  //开始攻击范围
    public bool arriveBorder;  //是否走到边缘
    public GameObject contactBorder;  //碰到的边缘
    public PlayerController player;
    public IState previousState;  //上一个状态
    public bool beAttacked;  //是否被攻击
    public bool attackSucceed;  //是否成功打到目标
    public bool isShake;
    public string attackedPrefabName;
}

public class EnemyController : MonoBehaviour
{
    public EnemyParameter enemyParameter = new EnemyParameter();  //敌人参数
    public GameObject bornPoint;

    public Dictionary<enemyStateType, IState> enemyStates = new Dictionary<enemyStateType, IState>();  //敌人状态
    public IState enemyCurrentState;  //敌人当前状态

    private void Awake()
    {
        enemyParameter.enemyRigidbody = this.GetComponent<Rigidbody2D>();
        enemyParameter.enemyAnimator = this.GetComponent<Animator>();
        enemyParameter.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();   

        enemyStates.Add(enemyStateType.idle, new EnemyIdleState(this));
        enemyStates.Add(enemyStateType.patrol, new EnemyPatrolState(this));
        enemyStates.Add(enemyStateType.chase, new EnemyChaseState(this));
        enemyStates.Add(enemyStateType.walk_back, new EnemyWalkBackState(this));
        enemyStates.Add(enemyStateType.attack_1, new EnemyAttack_1_State(this));
        enemyStates.Add(enemyStateType.attack_2, new EnemyAttack_2_State(this));
        enemyStates.Add(enemyStateType.attack_3, new EnemyAttack_3_State(this));
        enemyStates.Add(enemyStateType.backToBornPoint, new BackToBornPointState(this));
        enemyStates.Add(enemyStateType.beAttacked, new EnemyBeAttackedState(this));
        enemyStates.Add(enemyStateType.drop, new EnemyDrop(this));
        enemyStates.Add(enemyStateType.airUnable, new EnemyAirUnable(this));

        Transition(enemyStateType.idle);
    }
    // Update is called once per frame
    void Update()
    {
        enemyCurrentState.OnUpdate();
        //Debug.Log(enemyCurrentState);
    }

    public void Transition(enemyStateType enemyStateType)
    {
        if (enemyCurrentState != null)
            enemyCurrentState.OnExit();
        enemyParameter.previousState = enemyCurrentState;
        enemyCurrentState = enemyStates[enemyStateType];
        enemyCurrentState.OnEnter();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            enemyParameter.arriveBorder = true;
            enemyParameter.contactBorder = collision.gameObject;
        }
        if (collision.CompareTag("PlayerAttack"))
        {
            enemyParameter.beAttacked = true;
            enemyParameter.attackedPrefabName = collision.gameObject.name;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            enemyParameter.arriveBorder = false;
            enemyParameter.contactBorder = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            enemyParameter.isGroud = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            enemyParameter.isGroud = false;
        }
    }

    public void CurrentAnimEnd()
    {
        this.enemyParameter.currentAnimEnd = true;
    }


    public void CreatAttackTrigger()
    {
        enemyParameter.creatAttackTrigger = true;
    }

    public void DestroyAttackTrigger()
    {
        enemyParameter.destroyAttackTrigger = true;
    }
}


