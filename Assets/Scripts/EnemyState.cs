using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyIdleState:IState
{
    private EnemyController enemyFSM;
    private EnemyParameter parameter;
    public EnemyIdleState(EnemyController enemyController)
    {
        enemyFSM = enemyController;
        parameter = enemyController.enemyParameter;
    }
    public void OnEnter()
    {
        parameter.enemyAnimator.Play("idle");
        parameter.enemyRigidbody.velocity = new Vector2(0, 0);
    }

    public void OnUpdate()
    {
        if (parameter.beAttacked)
        {
            enemyFSM.Transition(enemyStateType.beAttacked);
        }
    }

    public void OnExit()
    {
    }
}

public class EnemyDrop:IState
{
    private EnemyController enemyFSM;
    private EnemyParameter parameter;
    private float timer;
    public EnemyDrop(EnemyController enemyController)
    {
        enemyFSM = enemyController;
        parameter = enemyController.enemyParameter;
    }
    public void OnEnter()
    {
        parameter.enemyAnimator.Play("drop");
        parameter.enemyRigidbody.velocity = new Vector2(0, -5);
        timer = 0;
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;

        if (timer >= 0.2)
        {
            parameter.enemyRigidbody.velocity = new Vector2(0, -15);
        }

        if (parameter.beAttacked)
        {
            enemyFSM.Transition(enemyStateType.beAttacked);
        }
        else
        {
            if (parameter.isGroud)
            {
                enemyFSM.Transition(enemyStateType.idle);
            }
        }
    }

    public void OnExit()
    { }
}

public class EnemyAirUnable:IState
{
    private EnemyController enemyFSM;
    private EnemyParameter parameter;
    public EnemyAirUnable(EnemyController enemyController)
    {
        enemyFSM = enemyController;
        parameter = enemyController.enemyParameter;
    }
    public void OnEnter()
    {
        parameter.enemyAnimator.Play("be_attacked_air");
        parameter.enemyRigidbody.velocity = new Vector2(-10f*enemyFSM.transform.localScale.x,-5);
        parameter.currentAnimEnd = false;
    }

    public void OnUpdate()
    {
        if (parameter.beAttacked)
        {
            enemyFSM.Transition(enemyStateType.beAttacked);
        }
        else
        {
            if (parameter.currentAnimEnd)
            {
                enemyFSM.Transition(enemyStateType.drop);
            }
        }
    }

    public void OnExit()
    { }
}

public class EnemyBeAttackedState : IState
{
    private EnemyController enemyFSM;
    private EnemyParameter parameter;
    //private float attackedDamage;
    private GameObject player;
    private PlayerController playerController;
    private float timer;
    private int attackedNumber=0;

    public EnemyBeAttackedState(EnemyController enemyController)
    {
        enemyFSM = enemyController;
        parameter = enemyController.enemyParameter;
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }
    public void OnEnter()
    {
        timer = 0;
        parameter.beAttacked = false;
        parameter.currentAnimEnd = false;
        //AttackedDamage(parameter.attackedPrefabName.Replace("(Clone)", ""));
        AttackedAnim(parameter.attackedPrefabName.Replace("(Clone)", ""));
        attackedNumber++;
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;

        AttackedEffect(parameter.attackedPrefabName.Replace("(Clone)", ""));

        if (parameter.beAttacked)
        {
            enemyFSM.Transition(enemyStateType.beAttacked);
        }
        else
        {
            if (parameter.currentAnimEnd)
            {
                if (parameter.isGroud)
                {
                    enemyFSM.Transition(enemyStateType.idle);
                }
                else
                {
                    enemyFSM.Transition(enemyStateType.drop);
                }
            }
        }
    }

    public void OnExit()
    {
       
    }

    //private void AttackedDamage(string prefabName)
    //{
    //    switch (prefabName)
    //    {
    //        case "asuna_attack_1":
    //            attackedDamage = 10;
    //            break;
    //        case "asuna_attack_2":
    //            attackedDamage = 20;
    //            break;
    //        case "asuna_attack_3":
    //            attackedDamage = 30;
    //            break;
    //        case "asuna_uppercut_attack":
    //            attackedDamage = 40;
    //            break;
    //        case "asuna_air_attack_1":
    //            attackedDamage = 10;
    //            break;
    //        case "asuna_air_attack_2":
    //            attackedDamage = 20;
    //            break;
    //        case "asuna_air_attack_3":
    //            attackedDamage = 30;
    //            break;
    //    }
    //}

    private void AttackedAnim(string prefabName)
    {
        if (player.transform.localScale.x < 0)
        {
            enemyFSM.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            enemyFSM.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        switch (prefabName)
        {
            case "asuna_attack_1":
                if (attackedNumber % 2 == 1)
                    parameter.enemyAnimator.Play("be_attacked_ground");
                else
                    parameter.enemyAnimator.Play("be_attacked_ground_2");
                break;
            case "asuna_attack_2":
                if (attackedNumber % 2 == 1)
                    parameter.enemyAnimator.Play("be_attacked_ground");
                else
                    parameter.enemyAnimator.Play("be_attacked_ground_2");
                break;
            case "asuna_attack_3":
                if (attackedNumber % 2 == 1)
                    parameter.enemyAnimator.Play("be_attacked_ground");
                else
                    parameter.enemyAnimator.Play("be_attacked_ground_2");
                break;
            case "asuna_uppercut_attack":
                parameter.enemyAnimator.Play("be_attacked_toAir");
                break;
            case "asuna_air_attack_1":
                if (attackedNumber % 2 == 1)
                    parameter.enemyAnimator.Play("be_attacked_ground");
                else
                    parameter.enemyAnimator.Play("be_attacked_ground_2");
                break;
            case "asuna_air_attack_2":
                if (attackedNumber % 2 == 1)
                    parameter.enemyAnimator.Play("be_attacked_ground");
                else
                    parameter.enemyAnimator.Play("be_attacked_ground_2");
                break;
            case "asuna_air_attack_3":
                if (attackedNumber % 2 == 1)
                    parameter.enemyAnimator.Play("be_attacked_ground");
                else
                    parameter.enemyAnimator.Play("be_attacked_ground_2");
                break;
        }
    }

    private void AttackedEffect(string prefabName)
    {
        switch (prefabName)
        {
            case "asuna_attack_1":
                AttackedMove(new Vector2(20, 0), 0.02f);
                AttackedSlow(parameter.enemyAnimator, parameter.enemyRigidbody, 0.1f, 0.1f);
                AttackedSlow(playerController.playerParameter.playerAnimator, playerController.playerParameter.playerRigidbody, 0.1f, 0.1f);
                //CameraShake(0.01f, 0.1f);
                break;
            case "asuna_attack_2":
                AttackedMove(new Vector2(40, 0), 0.02f);
                AttackedSlow(parameter.enemyAnimator, parameter.enemyRigidbody, 0.1f, 0.1f);
                AttackedSlow(playerController.playerParameter.playerAnimator, playerController.playerParameter.playerRigidbody, 0.1f, 0.1f);
                //CameraShake(0.01f, 0.1f);
                break;
            case "asuna_attack_3":
                AttackedMove(new Vector2(40, 0), 0.02f);
                AttackedSlow(parameter.enemyAnimator, parameter.enemyRigidbody, 0.1f, 0.2f);
                AttackedSlow(playerController.playerParameter.playerAnimator, playerController.playerParameter.playerRigidbody, 0.1f, 0.2f);
                //CameraShake(0.01f, 0.1f);
                break;
            case "asuna_uppercut_attack":
                AttackedMove(new Vector2(3, 20), 0.3f);
                AttackedSlow(parameter.enemyAnimator, parameter.enemyRigidbody, 0.1f, 0.2f);
                AttackedSlow(playerController.playerParameter.playerAnimator, playerController.playerParameter.playerRigidbody, 0.1f, 0.2f);
                CameraShake(0.01f, 0.1f);
                break;
            case "asuna_air_attack_1":
                AttackedMove(new Vector2(4, 0), 0.15f);
                //AttackedSlow(parameter.enemyAnimator, parameter.enemyRigidbody, 0.1f, 0.1f);
                //AttackedSlow(playerController.playerParameter.playerAnimator, playerController.playerParameter.playerRigidbody, 0.1f, 0.2f);
                //CameraShake(0.01f, 0.1f);
                break;
            case "asuna_air_attack_2":
                AttackedMove(new Vector2(4, 0), 0.15f);
                //AttackedSlow(parameter.enemyAnimator, parameter.enemyRigidbody, 0.1f, 0.1f);
                //AttackedSlow(playerController.playerParameter.playerAnimator, playerController.playerParameter.playerRigidbody, 0.1f, 0.2f);
                //CameraShake(0.01f, 0.1f);
                break;
            case "asuna_air_attack_3":
                AttackedMove(new Vector2(30, 0), 0.1f);
                //AttackedSlow(parameter.enemyAnimator, parameter.enemyRigidbody, 0.1f, 0.1f);
                //AttackedSlow(playerController.playerParameter.playerAnimator, playerController.playerParameter.playerRigidbody, 0.1f, 0.2f);
                //CameraShake(0.01f, 0.1f);
                break;
        }
    }

    private void AttackedMove(Vector2 speed, float moveTime)
    {
        if (timer<=moveTime)
        {
            parameter.enemyRigidbody.velocity= new Vector2(speed.x * player.transform.localScale.x, speed.y);
        }
        else
        {
            parameter.enemyRigidbody.velocity = new Vector2(0, 0);
        }
    }

    private void AttackedSlow(Animator animator, Rigidbody2D rigidbody2D, float slowRatio, float slowTime)
    {
        if (timer <= slowTime)
        {
            animator.speed = slowRatio;
        }
        else
        {
            animator.speed = 1;
        }
    }

    private void CameraShake(float strength, float shakeTime)
    {
        if (!parameter.isShake)
        {
            parameter.isShake = true;
            Transform camera = Camera.main.transform;
            Vector3 startPosition = camera.position;

            if (shakeTime>0)
            {
                camera.position = Random.insideUnitSphere * strength + startPosition;
                shakeTime -= Time.deltaTime;
            }
            else
            {
                parameter.isShake = false;
            }
        }
    }
}

public class EnemyPatrolState : IState
{
    private EnemyController enemyFSM;
    private EnemyParameter parameter;
    private Vector2 playerPos;
    private int direciton;
    private float actTime;
    public EnemyPatrolState(EnemyController enemyController)
    {
        enemyFSM = enemyController;
        parameter = enemyController.enemyParameter;
        playerPos = parameter.player.transform.position;
    }
    public void OnEnter()
    {
        parameter.enemyAnimator.Play("walk");
        parameter.actTimer = 0;

        if (parameter.arriveBorder)
        {
            direciton= enemyFSM.transform.position.x - parameter.contactBorder.transform.position.x>0?1:-1;
        }
        else
        {
            direciton = Random.Range(0, 2)<0.5?-1:1;
        }
        parameter.arriveBorder = false;
        actTime = Random.Range(2f, 5f);
    }

    public void OnUpdate()
    {
        parameter.actTimer += Time.deltaTime;

        if (Mathf.Abs(playerPos.x - enemyFSM.transform.position.x) < parameter.enemyAlertRange.x && Mathf.Abs(playerPos.y - enemyFSM.transform.position.y) < parameter.enemyAlertRange.y)
        {
            enemyFSM.Transition(enemyStateType.chase);
        }
        else
        {
            if (Mathf.Abs(enemyFSM.transform.position.x-enemyFSM.bornPoint.transform.position.x)<30)
            {
                if (parameter.arriveBorder || parameter.actTimer > actTime)
                {
                    enemyFSM.Transition(enemyStateType.idle);
                }
                else
                {
                    enemyFSM.transform.localScale = new Vector3(direciton, 1, 1);
                    parameter.enemyRigidbody.velocity = new Vector2(parameter.enemyMoveSpeed * direciton, 0);
                }
            }
            else
            {
                enemyFSM.Transition(enemyStateType.backToBornPoint);
            }
        }
            
    }

    public void OnExit()
    {
        parameter.actTimer = 0;
    }
}

public class EnemyChaseState : IState
{
    private EnemyController enemyFSM;
    private EnemyParameter parameter;
    public EnemyChaseState(EnemyController enemyController)
    {
        enemyFSM = enemyController;
        parameter = enemyController.enemyParameter;
    }
    public void OnEnter()
    {
        
    }

    public void OnUpdate()
    {
        if (true)
        {

        }
    }

    public void OnExit()
    { }
}

public class EnemyWalkBackState : IState
{
    public EnemyWalkBackState(EnemyController enemyController)
    {
        
    }
    public void OnEnter()
    { }

    public void OnUpdate()
    { }

    public void OnExit()
    { }
}

public class EnemyAttack_1_State : IState
{
    public EnemyAttack_1_State(EnemyController enemyController)
    {

    }
    public void OnEnter()
    { }

    public void OnUpdate()
    { }

    public void OnExit()
    { }
}

public class EnemyAttack_2_State : IState
{
    public EnemyAttack_2_State(EnemyController enemyController)
    {

    }
    public void OnEnter()
    { }

    public void OnUpdate()
    { }

    public void OnExit()
    { }
}

public class EnemyAttack_3_State : IState
{
    public EnemyAttack_3_State(EnemyController enemyController)
    {

    }
    public void OnEnter()
    { }

    public void OnUpdate()
    { }

    public void OnExit()
    { }
}

public class BackToBornPointState : IState
{
    private EnemyController enemyFSM;
    private EnemyParameter parameter;
    private int direciton;
    public BackToBornPointState(EnemyController enemyController)
    {
        enemyFSM = enemyController;
        parameter = enemyController.enemyParameter;
    }
    public void OnEnter()
    {
        direciton = enemyFSM.transform.position.x - enemyFSM.bornPoint.transform.position.x > 0 ? -1 : 1;
        parameter.enemyAnimator.Play("walk");
    }

    public void OnUpdate()
    {
        if (Mathf.Abs(enemyFSM.transform.position.x - enemyFSM.bornPoint.transform.position.x)>3)
        {
            enemyFSM.transform.localScale = new Vector3(direciton, 1, 1);
            parameter.enemyRigidbody.velocity = new Vector2(parameter.enemyMoveSpeed * direciton, 0);
        }
        else
        {
            enemyFSM.Transition(enemyStateType.idle);
        }
    }

    public void OnExit()
    { }
}

