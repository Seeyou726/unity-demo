using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum stateType
{
    idle, run, jumpUp, jumpDown, jumpTop, attack_1, attack_2, attack_3,attack_4,air_attack_1, air_attack_2, air_attack_3,jumpIdle,climbBorder
}
public class PlayerParameter  //角色属性参数
{
    public bool isGround = false;
    public float playerMoveSpeed = 20;
    public float playerJumpSpeed = 30;
    public float gravity = 2;
    public float hp = 200;
    public Rigidbody2D playerRigidbody;
    public Animator playerAnimator;
    public float jumpTimer;  //跳跃计时器
    public bool currentAnimEnd = false;  //当前动画是否结束
    public bool acceptNextOrder = false;  //是否可预输入下一个指令
    public bool goNextAttack = false;  //是否可进入下一个攻击动作
    public bool goAttack_2 = false;  //下个攻击衔接为Attack_2
    public bool goAttack_3 = false;  //下个攻击衔接为Attack_3
    public bool goAttack_4 = false;  //下个攻击衔接为Attack_4
    public bool creatAttackTrigger = false;  //攻击生成触发器
    public bool destroyAttackTrigger = false;  //销毁攻击触发器
    public bool hasExtraMovement = false;
    public bool airAttackCD = false;
    public bool hitPlatformBorder = false;
    public bool climbBorder = false;
}
public class PlayerController : MonoBehaviour
{
    public KeyBoardDetection PlayerKeyboardStates = new KeyBoardDetection();  //角色按键状态
    public PlayerParameter playerParameter = new PlayerParameter();  //角色参数
    //public Gravity PlayerGravity = new Gravity();  //重力模拟
    public AttackDamageData attackDamageData = new AttackDamageData();

    public Dictionary<stateType, IState> playerStates = new Dictionary<stateType, IState>();  //角色状态
    public IState currentState;  //角色当前状态

    private BoxCollider2D playerBoxCollider;
    private Vector2 topRayPoint;
    public List<RaycastHit2D> hitInfo = new List<RaycastHit2D>();

    private Vector2 initialPos;
    private GameObject bottomBorder;

    public LayerMask borderLayerMask;


    void Awake()
    {        
        playerParameter.playerRigidbody = this.GetComponent<Rigidbody2D>();
        playerParameter.playerAnimator = this.GetComponent<Animator>();
        playerBoxCollider = GetComponent<BoxCollider2D>();
        topRayPoint = new Vector2(transform.position.x,transform.position.y+ playerBoxCollider.size.y/2+playerBoxCollider.offset.y);

        StateRegister();

        Transition(stateType.idle);

        initialPos = transform.position;
        bottomBorder = GameObject.FindWithTag("Bottom");

        borderLayerMask = LayerMask.GetMask("PlatformBorder");
    }

    void StateRegister()
    {
        playerStates.Add(stateType.idle, new IdleState(this));
        playerStates.Add(stateType.run, new RunState(this));
        playerStates.Add(stateType.jumpUp, new JumpUpState(this));
        playerStates.Add(stateType.jumpDown, new JumpDownState(this));
        playerStates.Add(stateType.jumpTop, new JumpTopState(this));
        playerStates.Add(stateType.attack_1, new Attack_1_State(this));
        playerStates.Add(stateType.attack_2, new Attack_2_State(this));
        playerStates.Add(stateType.attack_3, new Attack_3_State(this));
        playerStates.Add(stateType.attack_4, new Attack_4_State(this));
        playerStates.Add(stateType.air_attack_1, new Air_Attack_1_State(this));
        playerStates.Add(stateType.air_attack_2, new Air_Attack_2_State(this));
        playerStates.Add(stateType.air_attack_3, new Air_Attack_3_State(this));
        playerStates.Add(stateType.jumpIdle, new Jump_Idle(this));
        playerStates.Add(stateType.climbBorder, new Climb_Border(this));
    }

    // Update is called once per frame
    void Update()
    {       
        PlayerKeyboardStates.KeyDown();

        currentState.OnUpdate();

        Dead();
    }

    private void FixedUpdate()
    {
        RayHitPlatformBorder();
        //RayHitIsGround();
    }

    public void Transition(stateType stateType)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = playerStates[stateType];
        currentState.OnEnter();
    }

    private void Dead()
    {
        if (transform.position.y<bottomBorder.transform.position.y)
        {
            transform.position = initialPos;
        }
    }

    public void BeAttacked(string attackName)
    {
        float attackDamage = attackDamageData.attackDamage[attackName];
        playerParameter.hp -= attackDamage;
    }

    public void CurrentAnimEnd()
    {
        this.playerParameter.currentAnimEnd = true;

    }

    public void CanAcceptNextOrder()
    {
        this.playerParameter.acceptNextOrder = true;
    }

    public void CreatAttackTrigger()
    {
        playerParameter.creatAttackTrigger = true;
    }

    public void DestroyAttackTrigger()
    {
        playerParameter.destroyAttackTrigger = true;
    }

    public void BeginExtraMovement()
    {
        playerParameter.hasExtraMovement = true;
    }

    public void EndExtraMovement()
    {
        playerParameter.hasExtraMovement = false;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Border")
        //{
        //    this.GetComponent<Rigidbody2D>().isKinematic = true;
        //}                   
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Border")
        //{
        //    this.GetComponent<Rigidbody2D>().isKinematic = false;
        //}
    }

    public void RayHitPlatformBorder()
    {
        topRayPoint = new Vector2(transform.position.x, transform.position.y + playerBoxCollider.size.y / 2 + playerBoxCollider.offset.y);
        //RaycastHit2D raycastHit2D;
        //BoxCollider2D hitBorderCollider;

        for (float offset = 0; offset <= playerBoxCollider.size.y ; offset = offset + 0.3f)
        {
            //if (offset <= playerBoxCollider.size.y * 3 / 4)
            //{
            //    Debug.DrawRay(new Vector2(topRayPoint.x, topRayPoint.y - offset), new Vector2(transform.localScale.x, 0) * 0.6f, Color.red);
            //}
            if (Physics2D.Raycast(new Vector2(topRayPoint.x, topRayPoint.y - offset), new Vector2(transform.localScale.x, 0), 0.6f, borderLayerMask))
            {
                playerParameter.playerRigidbody.velocity = new Vector2(0, playerParameter.playerRigidbody.velocity.y);
                if (offset <= playerBoxCollider.size.y * 3 / 4)
                {
                    playerParameter.playerRigidbody.velocity = new Vector2(0, playerParameter.playerRigidbody.velocity.y);
                }
                else
                {
                    //raycastHit2D = Physics2D.Raycast(new Vector2(topRayPoint.x, topRayPoint.y - offset), new Vector2(transform.localScale.x, 0), 0.6f, borderLayerMask);
                    //hitBorderCollider = raycastHit2D.transform.GetComponent<BoxCollider2D>();
                    //transform.position = Vector3.Lerp(transform.position, new Vector3(raycastHit2D.transform.position.x+(-transform.localScale.x)*hitBorderCollider.size.x,raycastHit2D.transform.position.y+hitBorderCollider.size.y/2),0.1f) ;
                    playerParameter.playerRigidbody.velocity = new Vector2(playerParameter.playerRigidbody.velocity.x, 10);
                }
            }        
        }
        //Debug.DrawRay(new Vector2(topRayPoint.x, topRayPoint.y - playerBoxCollider.size.y), new Vector2(transform.localScale.x, 0) * 0.6f, Color.red);
        if (Physics2D.Raycast(new Vector2(topRayPoint.x, topRayPoint.y - playerBoxCollider.size.y), new Vector2(transform.localScale.x, 0), 0.6f, borderLayerMask))
        {
            playerParameter.playerRigidbody.velocity = new Vector2(playerParameter.playerRigidbody.velocity.x, 10);
        }
        //else
        //{
        //    playerParameter.climbBorder = false;
        //}
    }

    //public void RayHitIsGround()
    //{
    //    LayerMask layerMask = LayerMask.GetMask("Ground");

    //    Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + playerBoxCollider.offset.y), Vector2.down* playerBoxCollider.size.y / 2, Color.red);

    //    if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + playerBoxCollider.offset.y),Vector2.down,playerBoxCollider.size.y/2,layerMask))
    //    {
    //        playerParameter.isGround = true;
    //    }

    //    Debug.Log(Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + playerBoxCollider.offset.y), Vector2.down, playerBoxCollider.size.y / 2, layerMask).distance);
    //}
    public void IsGround()
    {
        playerParameter.isGround = true;
    }

    public void IsNotGround()
    {
        playerParameter.isGround = false;
    }

    public void HeadHitBottom()
    {
        Transition(stateType.jumpDown);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttack"))
        {
            //playerParameter.beAttacked = true;
            //playerParameter.beAttackedTrigger = collision.gameObject;
            float attackDamage = attackDamageData.attackDamage[collision.name.Replace("(Clone)", "")];
            playerParameter.hp -= attackDamage;
            print(playerParameter.hp);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

}
public class KeyBoardDetection   //按键检测
{
    public bool pressedUp = false;  //是否按下上键
    public bool pressedDown = false;  //是否按下下键
    public bool pressedLeft = false;   //是否按下左键
    public bool pressedRight = false;  //是否按下右键
    public bool pressedJump = false;  //是否按下跳跃键
    public bool pressedAttack = false;  //是否按下攻击键
    public bool pressedRush = false;  //是否按下冲刺键
    public bool pressedTimeBack = false;  //是否按下时间回退
    public PlayerParameter playerParameter = new PlayerParameter();

    public void KeyDown()  //检测键盘按键
    {
        if (Input.GetKey(Const.Control[0]))
            pressedUp = true;
        else
            pressedUp = false;

        if (Input.GetKey(Const.Control[1]))
            pressedDown = true;
        else
            pressedDown = false;

        if (Input.GetKey(Const.Control[2]))
            pressedLeft = true;
        else
            pressedLeft = false;

        if (Input.GetKey(Const.Control[3]))
            pressedRight = true;
        else
            pressedRight = false;

        if (Input.GetKey(Const.Control[4]))       
            pressedJump = true;
        else
            pressedJump = false;

        if (Input.GetKeyDown(Const.Control[5]))
            pressedAttack = true;
        else
            pressedAttack = false;

        if (Input.GetKeyDown(Const.Control[5]))
        {
            pressedRush = true;
        }


        if (Input.GetKey(Const.Control[6]))
        {
            pressedTimeBack = true;
        }
        else
        {
            pressedTimeBack = false;
        }

    }

}



//public class Gravity  //重力模拟
//{
//    float acceleration = -1.5f;  //重力加速度  每固定帧
//    float maxDropSpeed = 20;  //最大下落速度
//    Vector2 velocity;
//    public void GravitySimulate(PlayerController playerController, bool isAvailable)
//    {
//        //    if (playerController.GetType() == typeof(JumpUpState))
//        //    {
//        //        if (playerController.playerParameter.playerRigidbody.velocity.y > -1 * maxDropSpeed && playerController.playerParameter.jumpTimer > 0.02f)
//        //            playerController.playerParameter.playerRigidbody.AddForce(new Vector2(0, -70));
//        //    }else
//        //        playerController.playerParameter.playerRigidbody.AddForce(new Vector2(0, -70));

//        if (isAvailable)
//        {
//            if (playerController.currentState.GetType() == typeof(JumpUpState))
//            {
//                if (playerController.playerParameter.playerRigidbody.velocity.y > -1 * maxDropSpeed && playerController.playerParameter.jumpTimer > 0.02f)
//                    playerController.playerParameter.playerRigidbody.velocity = new Vector2(playerController.playerParameter.playerRigidbody.velocity.x, playerController.playerParameter.playerRigidbody.velocity.y + acceleration);
//            }
//            else
//                playerController.playerParameter.playerRigidbody.velocity = new Vector2(playerController.playerParameter.playerRigidbody.velocity.x, playerController.playerParameter.playerRigidbody.velocity.y + acceleration);
//        }
//    }
//}