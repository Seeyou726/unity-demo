using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;
    private float attackLastTime;

    public IdleState(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }
    public void OnEnter()
    {
        parameter.playerAnimator.Play("idle");
        parameter.playerRigidbody.velocity = new Vector2(0, 0);
        attackLastTime = 0;
    }

    public void OnUpdate()
    {

        if (parameter.goAttack_2||parameter.goAttack_3|| parameter.goAttack_4)
        {
            attackLastTime += Time.deltaTime;
            if (attackLastTime > 0.5f)
            {
                parameter.goAttack_2 = false;
                parameter.goAttack_3 = false;
                parameter.goAttack_4 = false;
                attackLastTime = 0;
            }
        }
        if (parameter.isGround)
        {
            if (keyboardState.pressedRight || keyboardState.pressedLeft)
            {
                playerFSM.Transition(stateType.run);
            }
            else if (keyboardState.pressedJump)
                playerFSM.Transition(stateType.jumpUp);
            else if (!parameter.goAttack_2&&!parameter.goAttack_3&& !parameter.goAttack_4 && keyboardState.pressedAttack)
                playerFSM.Transition(stateType.attack_1);
            else if (parameter.goAttack_2 && keyboardState.pressedAttack)
                playerFSM.Transition(stateType.attack_2);
            else if (parameter.goAttack_3 && keyboardState.pressedAttack)
                playerFSM.Transition(stateType.attack_3);
            else if (parameter.goAttack_4 && keyboardState.pressedAttack)
                playerFSM.Transition(stateType.attack_4);
        }
        else
            playerFSM.Transition(stateType.jumpDown);

    }

    public void OnExit()
    {
        parameter.goAttack_2 = false;
        parameter.goAttack_3 = false;
        parameter.goAttack_4 = false;
    }
}

public class RunState : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;


    public RunState(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;

    }
    public void OnEnter()
    {
        parameter.playerAnimator.Play("run");
        parameter.playerRigidbody.velocity = new Vector2(parameter.playerRigidbody.velocity.x, 0);
    }

    public void OnUpdate()
    {
        if (parameter.isGround)
        { 
            if (keyboardState.pressedJump)
                playerFSM.Transition(stateType.jumpUp);
            else if (keyboardState.pressedAttack)           
                playerFSM.Transition(stateType.attack_1);
            else if (keyboardState.pressedRight)
                {
                    parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed, parameter.playerRigidbody.velocity.y);
                    playerFSM.transform.localScale = new Vector3(1, 1, 1);
                }
            else if (keyboardState.pressedLeft)
                {
                    parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed * -1, parameter.playerRigidbody.velocity.y);
                    playerFSM.transform.localScale = new Vector3(-1, 1, 1);
            }          
            else
            {
                playerFSM.Transition(stateType.idle);
            }
        }
        else
        {
            playerFSM.Transition(stateType.jumpDown);
        }

    }
     
    public void OnExit()
    {

    }
}

public class JumpUpState : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;
    private float attackLastTime;

    public JumpUpState(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }
    public void OnEnter()
    {
        parameter.jumpTimer = 0;
        parameter.playerAnimator.Play("jumpUp");
        parameter.playerRigidbody.velocity = new Vector2(parameter.playerRigidbody.velocity.x, parameter.playerJumpSpeed);
    }

    public void OnUpdate()
    {
        //if (parameter.goAttack_2 || parameter.goAttack_3)
        //{
        //    attackLastTime += Time.deltaTime;
        //    if (attackLastTime > 1f)
        //    {
        //        parameter.goAttack_2 = false;
        //        parameter.goAttack_3 = false;
        //        attackLastTime = 0;
        //    }
        //}

        parameter.jumpTimer += Time.deltaTime;

        if (parameter.jumpTimer<0.1f&&keyboardState.pressedJump)
        {
            parameter.playerRigidbody.velocity = new Vector2(parameter.playerRigidbody.velocity.x, parameter.playerJumpSpeed/2);
        }
        else if (parameter.jumpTimer < 0.3f && keyboardState.pressedJump)
        {
            parameter.playerRigidbody.velocity = new Vector2(parameter.playerRigidbody.velocity.x, parameter.playerJumpSpeed);
        }
        else
        {
            parameter.playerRigidbody.velocity = new Vector2(parameter.playerRigidbody.velocity.x, parameter.playerRigidbody.velocity.y-parameter.gravity);
        }
        if (parameter.playerRigidbody.velocity.y < 10)
        {
            playerFSM.Transition(stateType.jumpTop);
        }

        if (keyboardState.pressedRight)
        {
            parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed, parameter.playerRigidbody.velocity.y);
            playerFSM.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (keyboardState.pressedLeft)
        {
            parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed * -1, parameter.playerRigidbody.velocity.y);
            playerFSM.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            parameter.playerRigidbody.velocity = new Vector2(0, parameter.playerRigidbody.velocity.y);
        }

        if (!parameter.isGround)
        {
            if (!parameter.goAttack_2 && !parameter.goAttack_3 && keyboardState.pressedAttack)
                playerFSM.Transition(stateType.air_attack_1);
            else if (parameter.goAttack_2 && keyboardState.pressedAttack)
                playerFSM.Transition(stateType.air_attack_2);
            else if (parameter.goAttack_3 && keyboardState.pressedAttack)
                playerFSM.Transition(stateType.air_attack_3);

            //if (parameter.climbBorder)
            //{
            //    playerFSM.Transition(stateType.climbBorder);
            //}
        }
    }

    public void OnExit()
    {
        parameter.jumpTimer = 0;
        //parameter.goAttack_2 = false;
        //parameter.goAttack_3 = false;
    }
}

public class JumpDownState : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;
    private float airAttckCDTimer;

    public JumpDownState(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }
    public void OnEnter()
    {
        parameter.playerAnimator.Play("jumpDown");
        airAttckCDTimer = 0;
    }

    public void OnUpdate()
    {
        //if (parameter.goAttack_2 || parameter.goAttack_3)
        //{
        //    attackLastTime += Time.deltaTime;
        //    if (attackLastTime > 1f)
        //    {
        //        parameter.goAttack_2 = false;
        //        parameter.goAttack_3 = false;
        //        attackLastTime = 0;
        //    }
        //}
        airAttckCDTimer += Time.deltaTime;

        if (airAttckCDTimer>=1&&parameter.airAttackCD)
        {
            parameter.airAttackCD = false;
        }

        if (parameter.playerRigidbody.velocity.y > -5 )
        {
            parameter.playerRigidbody.velocity = new Vector2(parameter.playerRigidbody.velocity.x, parameter.playerRigidbody.velocity.y - 1f);
        }
        else if (parameter.playerRigidbody.velocity.y>-40)
        {
            parameter.playerRigidbody.velocity = new Vector2(parameter.playerRigidbody.velocity.x, parameter.playerRigidbody.velocity.y-parameter.gravity);
        }
        else
        {
            parameter.playerRigidbody.velocity = new Vector2(parameter.playerRigidbody.velocity.x, -40);
        }

        if (!parameter.isGround)
        {
            if (keyboardState.pressedRight)
            {
                parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed, parameter.playerRigidbody.velocity.y);
                playerFSM.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (keyboardState.pressedLeft)
            {
                parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed * -1, parameter.playerRigidbody.velocity.y);
                playerFSM.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                parameter.playerRigidbody.velocity = new Vector2(0, parameter.playerRigidbody.velocity.y);
            }

            if (!parameter.airAttackCD && keyboardState.pressedAttack)
                playerFSM.Transition(stateType.air_attack_1);
            
        }
        else
        {
            playerFSM.Transition(stateType.idle);
        }
            

    }

    public void OnExit()
    {
        parameter.airAttackCD = false;
        //parameter.goAttack_2 = false;
        //parameter.goAttack_3 = false;
    }
}

public class JumpTopState : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;
    private float attackLastTime;

    public JumpTopState(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }
    public void OnEnter()
    {
        //parameter.playerAnimator.Play("jumpTop");
    }

    public void OnUpdate()
    {
        //if (parameter.goAttack_2 || parameter.goAttack_3)
        //{
        //    attackLastTime += Time.deltaTime;
        //    if (attackLastTime > 1f)
        //    {
        //        parameter.goAttack_2 = false;
        //        parameter.goAttack_3 = false;
        //        attackLastTime = 0;
        //    }
        //}
      
        if (!parameter.isGround)
        {
            if (parameter.playerRigidbody.velocity.y <= 10&& parameter.playerRigidbody.velocity.y >= 5)
            {
                parameter.playerAnimator.Play("jumpTop_1");
                parameter.playerRigidbody.velocity = new Vector2(parameter.playerRigidbody.velocity.x, parameter.playerRigidbody.velocity.y - 1f);
                if (keyboardState.pressedRight)
                {
                    parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed, parameter.playerRigidbody.velocity.y);
                    playerFSM.transform.localScale = new Vector3(1, 1, 1);
                }
                else if (keyboardState.pressedLeft)
                {
                    parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed * -1, parameter.playerRigidbody.velocity.y);
                    playerFSM.transform.localScale = new Vector3(-1, 1, 1);
                }
            }
            else if (parameter.playerRigidbody.velocity.y < 5&&parameter.playerRigidbody.velocity.y > 0)
            {
                parameter.playerAnimator.Play("jumpTop_2");
                parameter.playerRigidbody.velocity = new Vector2(parameter.playerRigidbody.velocity.x, parameter.playerRigidbody.velocity.y - 0.2f);
                if (keyboardState.pressedRight)
                {
                    parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed, parameter.playerRigidbody.velocity.y);
                    playerFSM.transform.localScale = new Vector3(1, 1, 1);
                }
                else if (keyboardState.pressedLeft)
                {
                    parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed * -1, parameter.playerRigidbody.velocity.y);
                    playerFSM.transform.localScale = new Vector3(-1, 1, 1);
                }
            }
            else if (parameter.playerRigidbody.velocity.y < 0 && parameter.playerRigidbody.velocity.y >= -10)
            {
                parameter.playerAnimator.Play("jumpTop_3");
                parameter.playerRigidbody.velocity = new Vector2(parameter.playerRigidbody.velocity.x, parameter.playerRigidbody.velocity.y - 1f);
                if (keyboardState.pressedRight)
                {
                    parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed, parameter.playerRigidbody.velocity.y);
                    playerFSM.transform.localScale = new Vector3(1, 1, 1);
                }
                else if (keyboardState.pressedLeft)
                {
                    parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed * -1, parameter.playerRigidbody.velocity.y);
                    playerFSM.transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    parameter.playerRigidbody.velocity = new Vector2(0, parameter.playerRigidbody.velocity.y);
                }
            }
            else
            {
                playerFSM.Transition(stateType.jumpDown);
            }
               
            if (!parameter.goAttack_2 && !parameter.goAttack_3 && keyboardState.pressedAttack)
                playerFSM.Transition(stateType.air_attack_1);
            else if (parameter.goAttack_2 && keyboardState.pressedAttack)
                playerFSM.Transition(stateType.air_attack_2);
            else if (parameter.goAttack_3 && keyboardState.pressedAttack)
                playerFSM.Transition(stateType.air_attack_3);

            //if (parameter.climbBorder)
            //{
            //    playerFSM.Transition(stateType.climbBorder);
            //}
        }
        else
            playerFSM.Transition(stateType.idle);
    }

    public void OnExit()
    {
        //parameter.goAttack_2 = false;
        //parameter.goAttack_3 = false;
    }
}

public class Attack_1_State : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;
    private GameObject attackTriggerPrefab;
    private GameObject attackTriggerObject;
    private float moveDuration;

    public Attack_1_State(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }
    public void OnEnter()
    {
        parameter.playerAnimator.Play("attack_1");
        parameter.currentAnimEnd = false;
        parameter.playerRigidbody.velocity = new Vector2(0, 0);
        attackTriggerPrefab = Resources.Load<GameObject>("Prefab/Charactor/asuna_attack_1");
        moveDuration = 0.02f;
    }

    public void OnUpdate()
    {
        //if (parameter.hasExtraMovement)
        //{
        //    if (moveDuration>=0)
        //    {
        //        parameter.playerRigidbody.velocity = new Vector2(20f * playerFSM.transform.localScale.x, 0);
        //        moveDuration -= Time.deltaTime;
        //    }
        //    else
        //    {
        //        parameter.hasExtraMovement = false;
        //        parameter.playerRigidbody.velocity = new Vector2(0, 0);
        //    }
        //}

        if (parameter.hasExtraMovement)
        {
            if (moveDuration >= 0 && parameter.playerAnimator.speed == 1)
            {
                parameter.playerRigidbody.velocity = new Vector2(20f * playerFSM.transform.localScale.x, 0);
                moveDuration -= Time.deltaTime;
            }
            else if (moveDuration >= 0 && parameter.playerAnimator.speed == 0.1)
            {
                parameter.playerRigidbody.velocity = new Vector2(2f * playerFSM.transform.localScale.x, 0);
            }
            else
            {
                parameter.hasExtraMovement = false;
                parameter.playerRigidbody.velocity = new Vector2(0, 0);
            }
        }

        if (parameter.creatAttackTrigger&&attackTriggerObject==null)
        {
            attackTriggerObject= GameObject.Instantiate(attackTriggerPrefab, playerFSM.transform);
        }
        if (parameter.destroyAttackTrigger && attackTriggerObject != null)
        {
            GameObject.Destroy(attackTriggerObject);
        }

        if (parameter.acceptNextOrder && keyboardState.pressedAttack)
        {
            parameter.goNextAttack = true;
        }
        else if (parameter.goNextAttack&&(Input.GetKeyDown(Const.Control[2])|| Input.GetKeyDown(Const.Control[3])|| Input.GetKeyDown(Const.Control[4])))
        {
            parameter.goNextAttack = false;
        }

        if (parameter.currentAnimEnd)
        {
            if (parameter.isGround)
            {
                if (parameter.goNextAttack)
                {
                    playerFSM.Transition(stateType.attack_2);
                }
                else if (keyboardState.pressedRight || keyboardState.pressedLeft)
                    playerFSM.Transition(stateType.run);
                else
                {
                    parameter.goAttack_2 = true;
                    playerFSM.Transition(stateType.idle);
                }
            }
            else
                playerFSM.Transition(stateType.jumpDown);
        }
    }
    public void OnExit()
    {
        parameter.acceptNextOrder = false;
        parameter.goNextAttack = false;
        parameter.creatAttackTrigger = false;
        parameter.destroyAttackTrigger = false;
    }
}

public class Attack_2_State : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;
    private GameObject attackTriggerPrefab;
    private GameObject attackTriggerObject;
    private float moveDuration;

    public Attack_2_State(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;

    }
    public void OnEnter()
    {
        parameter.playerAnimator.Play("attack_2");
        parameter.currentAnimEnd = false;
        parameter.playerRigidbody.velocity = new Vector2(0, 0);
        attackTriggerPrefab = Resources.Load<GameObject>("Prefab/Charactor/asuna_attack_2");
        moveDuration = 0.02f;
    }

    public void OnUpdate()
    {
        //if (parameter.hasExtraMovement)
        //{
        //    if (moveDuration >= 0)
        //    {
        //        parameter.playerRigidbody.velocity = new Vector2(20f * playerFSM.transform.localScale.x, 0);
        //        moveDuration -= Time.deltaTime;
        //    }
        //    else
        //    {
        //        parameter.hasExtraMovement = false;
        //        parameter.playerRigidbody.velocity = new Vector2(0, 0);
        //    }
        //}

        if (parameter.hasExtraMovement)
        {
            if (moveDuration >= 0 && parameter.playerAnimator.speed == 1)
            {
                parameter.playerRigidbody.velocity = new Vector2(20f * playerFSM.transform.localScale.x, 0);
                moveDuration -= Time.deltaTime;
            }
            else if (moveDuration >= 0 && parameter.playerAnimator.speed == 0.1)
            {
                parameter.playerRigidbody.velocity = new Vector2(2f * playerFSM.transform.localScale.x, 0);
            }
            else
            {
                parameter.hasExtraMovement = false;
                parameter.playerRigidbody.velocity = new Vector2(0, 0);
            }
        }

        if (parameter.creatAttackTrigger && attackTriggerObject == null)
        {
            attackTriggerObject = GameObject.Instantiate(attackTriggerPrefab, playerFSM.transform);
        }
        if (parameter.destroyAttackTrigger)
        {
            GameObject.Destroy(attackTriggerObject);
        }

        if (parameter.acceptNextOrder && keyboardState.pressedAttack)
        {
            parameter.goNextAttack = true;
        }
        else if (parameter.goNextAttack && (Input.GetKeyDown(Const.Control[2]) || Input.GetKeyDown(Const.Control[3]) || Input.GetKeyDown(Const.Control[4])))
        {
            parameter.goNextAttack = false;
        }

        if (parameter.currentAnimEnd)
        {
            if (parameter.isGround)
            {
                if (parameter.goNextAttack)
                {
                    playerFSM.Transition(stateType.attack_3);
                }
                else if (keyboardState.pressedRight || keyboardState.pressedLeft)
                    playerFSM.Transition(stateType.run);
                else
                {
                    parameter.goAttack_3 = true;
                    playerFSM.Transition(stateType.idle);
                }
            }
            else
                playerFSM.Transition(stateType.jumpDown);
        }
    }

    public void OnExit()
    {
        parameter.acceptNextOrder = false;
        parameter.goNextAttack = false;
        parameter.creatAttackTrigger = false;
        parameter.destroyAttackTrigger = false;
    }
}

public class Attack_3_State : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;
    private GameObject attackTriggerPrefab;
    private GameObject attackTriggerObject;
    private float moveDuration;

    public Attack_3_State(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }
    public void OnEnter()
    {
        parameter.playerAnimator.Play("attack_3");
        parameter.currentAnimEnd = false;
        parameter.playerRigidbody.velocity = new Vector2(0, 0);
        attackTriggerPrefab = Resources.Load<GameObject>("Prefab/Charactor/asuna_attack_3");
        moveDuration = 0.03f;
    }

    public void OnUpdate()
    {
        //if (parameter.hasExtraMovement)
        //{
        //    if (moveDuration >= 0)
        //    {
        //        parameter.playerRigidbody.velocity = new Vector2(40f * playerFSM.transform.localScale.x, 0);
        //        moveDuration -= Time.deltaTime;
        //    }
        //    else
        //    {
        //        parameter.hasExtraMovement = false;
        //        parameter.playerRigidbody.velocity = new Vector2(0, 0);
        //    }
        //}

        if (parameter.hasExtraMovement)
        {
            if (moveDuration >= 0 && parameter.playerAnimator.speed == 1)
            {
                parameter.playerRigidbody.velocity = new Vector2(40f * playerFSM.transform.localScale.x, 0);
                moveDuration -= Time.deltaTime;
            }
            else if (moveDuration >= 0 && parameter.playerAnimator.speed == 0.1)
            {
                parameter.playerRigidbody.velocity = new Vector2(4f * playerFSM.transform.localScale.x, 0);
            }
            else
            {
                parameter.hasExtraMovement = false;
                parameter.playerRigidbody.velocity = new Vector2(0, 0);
            }
        }

        if (parameter.creatAttackTrigger && attackTriggerObject == null)
        {
            attackTriggerObject = GameObject.Instantiate(attackTriggerPrefab, playerFSM.transform);
        }
        if (parameter.destroyAttackTrigger)
        {
            GameObject.Destroy(attackTriggerObject);
        }

        if (parameter.acceptNextOrder && keyboardState.pressedAttack)
        {
            parameter.goNextAttack = true;
        }
        else if (parameter.goNextAttack && (Input.GetKeyDown(Const.Control[2]) || Input.GetKeyDown(Const.Control[3]) || Input.GetKeyDown(Const.Control[4])))
        {
            parameter.goNextAttack = false;
        }

        if (parameter.currentAnimEnd)
        {
            if (parameter.isGround)
            {
                if (parameter.goNextAttack)
                {
                    playerFSM.Transition(stateType.attack_4);
                }
                else if (keyboardState.pressedRight || keyboardState.pressedLeft)
                    playerFSM.Transition(stateType.run);
                else
                {
                    parameter.goAttack_4 = true;
                    playerFSM.Transition(stateType.idle);
                }
            }
            else
                playerFSM.Transition(stateType.jumpDown);
        }
    }

    public void OnExit()
    {
        parameter.acceptNextOrder = false;
        parameter.goNextAttack = false;
        parameter.creatAttackTrigger = false;
        parameter.destroyAttackTrigger = false;
    }
}

public class Attack_4_State : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;
    private GameObject attackTriggerPrefab;
    private GameObject attackTriggerObject;
    private float moveDuration;

    public Attack_4_State(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }
    public void OnEnter()
    {
        parameter.playerAnimator.Play("uppercut_attack");
        parameter.currentAnimEnd = false;
        parameter.playerRigidbody.velocity = new Vector2(0, 0);
        attackTriggerPrefab = Resources.Load<GameObject>("Prefab/Charactor/asuna_uppercut_attack");
        moveDuration = 0.04f;
    }

    public void OnUpdate()
    {
        //if (parameter.hasExtraMovement)
        //{
        //    parameter.playerRigidbody.velocity = new Vector2(5f * playerFSM.transform.localScale.x, 20);
        //}
        //else
        //{
        //    parameter.hasExtraMovement = false;
        //    parameter.playerRigidbody.velocity = new Vector2(0, 0);
        //}

        if (parameter.hasExtraMovement)
        {
            if (moveDuration >= 0)
            {
                if (parameter.playerAnimator.speed == 1)
                {
                    parameter.playerRigidbody.velocity = new Vector2(40f * playerFSM.transform.localScale.x, 0);
                    moveDuration -= Time.deltaTime;
                }
                else
                {
                    parameter.playerRigidbody.velocity = new Vector2(4f * playerFSM.transform.localScale.x, 0);
                }
            }
            else
            {
                if (parameter.playerAnimator.speed == 1)
                {
                    parameter.playerRigidbody.velocity = new Vector2(3, 20);
                    moveDuration -= Time.deltaTime;
                }
                else
                {
                    parameter.playerRigidbody.velocity = new Vector2(0.3f, 2);
                }
            }
        }

        if (parameter.creatAttackTrigger && attackTriggerObject == null)
        {
            attackTriggerObject = GameObject.Instantiate(attackTriggerPrefab, playerFSM.transform);
        }
        if (parameter.destroyAttackTrigger)
        {
            GameObject.Destroy(attackTriggerObject);
        }

        if (parameter.currentAnimEnd)
        {
            if (parameter.isGround)
            {
                if (keyboardState.pressedRight || keyboardState.pressedLeft)
                    playerFSM.Transition(stateType.run);
                else
                    playerFSM.Transition(stateType.idle);
            }
            else
                playerFSM.Transition(stateType.jumpDown);
        }
    }

    public void OnExit()
    {
        parameter.creatAttackTrigger = false;
        parameter.destroyAttackTrigger = false;
    }
}

public class Air_Attack_1_State : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;
    private GameObject attackTriggerPrefab;
    private GameObject attackTriggerObject;
    private float moveDuration;

    public Air_Attack_1_State(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }
    public void OnEnter()
    {
        parameter.playerAnimator.Play("asuna_air_attack_1");
        parameter.currentAnimEnd = false;
        parameter.playerRigidbody.velocity = new Vector2(0, 0);
        attackTriggerPrefab = Resources.Load<GameObject>("Prefab/Charactor/asuna_air_attack_1");
        moveDuration = 0.15f;
    }

    public void OnUpdate()
    {
        //if (parameter.hasExtraMovement)
        //{
        //    parameter.playerRigidbody.velocity = new Vector2(2f * playerFSM.transform.localScale.x, 0);
        //}

        if (parameter.hasExtraMovement)
        {
            if (moveDuration >= 0 && parameter.playerAnimator.speed == 1)
            {
                parameter.playerRigidbody.velocity = new Vector2(4f * playerFSM.transform.localScale.x, 0);
                moveDuration -= Time.deltaTime;
            }
            else if (moveDuration >= 0 && parameter.playerAnimator.speed == 0.1)
            {
                parameter.playerRigidbody.velocity = new Vector2(0.4f * playerFSM.transform.localScale.x, 0);
            }
            else
            {
                parameter.hasExtraMovement = false;
                parameter.playerRigidbody.velocity = new Vector2(0, 0);
            }
        }

        if (parameter.creatAttackTrigger && attackTriggerObject == null)
        {
            attackTriggerObject = GameObject.Instantiate(attackTriggerPrefab, playerFSM.transform);
        }
        if (parameter.destroyAttackTrigger && attackTriggerObject != null)
        {
            GameObject.Destroy(attackTriggerObject);
        }

        if (parameter.acceptNextOrder && keyboardState.pressedAttack)
        {
            parameter.goNextAttack = true;
        }
        else if (parameter.goNextAttack && (Input.GetKeyDown(Const.Control[2]) || Input.GetKeyDown(Const.Control[3])))
        {
            parameter.goNextAttack = false;
        }

        if (parameter.currentAnimEnd)
        {
            if (!parameter.isGround)
            {
                if (parameter.goNextAttack)
                {
                    playerFSM.Transition(stateType.air_attack_2);
                }
                
                else
                {
                    parameter.goAttack_2 = true;
                    playerFSM.Transition(stateType.jumpIdle);
                }
            }
        }
    }
    public void OnExit()
    {
        parameter.acceptNextOrder = false;
        parameter.goNextAttack = false;
        parameter.creatAttackTrigger = false;
        parameter.destroyAttackTrigger = false;
    }
}

public class Air_Attack_2_State : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;
    private GameObject attackTriggerPrefab;
    private GameObject attackTriggerObject;
    private float moveDuration;

    public Air_Attack_2_State(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }
    public void OnEnter()
    {
        parameter.playerAnimator.Play("asuna_air_attack_2");
        parameter.currentAnimEnd = false;
        parameter.playerRigidbody.velocity = new Vector2(0, 0);
        attackTriggerPrefab = Resources.Load<GameObject>("Prefab/Charactor/asuna_air_attack_2");
        moveDuration = 0.15f;
    }

    public void OnUpdate()
    {
        //if (parameter.hasExtraMovement)
        //{
        //    parameter.playerRigidbody.velocity = new Vector2(2f * playerFSM.transform.localScale.x, 10);
        //}

        if (parameter.hasExtraMovement)
        {
            if (moveDuration >= 0 && parameter.playerAnimator.speed == 1)
            {
                parameter.playerRigidbody.velocity = new Vector2(4f * playerFSM.transform.localScale.x, 0);
                moveDuration -= Time.deltaTime;
            }
            else if (moveDuration >= 0 && parameter.playerAnimator.speed == 0.1)
            {
                parameter.playerRigidbody.velocity = new Vector2(0.4f * playerFSM.transform.localScale.x, 0);
            }
            else
            {
                parameter.hasExtraMovement = false;
                parameter.playerRigidbody.velocity = new Vector2(0, 0);
            }
        }

        if (parameter.creatAttackTrigger && attackTriggerObject == null)
        {
            attackTriggerObject = GameObject.Instantiate(attackTriggerPrefab, playerFSM.transform);
        }
        if (parameter.destroyAttackTrigger && attackTriggerObject != null)
        {
            GameObject.Destroy(attackTriggerObject);
        }

        if (parameter.acceptNextOrder && keyboardState.pressedAttack)
        {
            parameter.goNextAttack = true;
        }
        else if (parameter.goNextAttack && (Input.GetKeyDown(Const.Control[2]) || Input.GetKeyDown(Const.Control[3])))
        {
            parameter.goNextAttack = false;
        }

        if (parameter.currentAnimEnd)
        {
            if (!parameter.isGround)
            {
                if (parameter.goNextAttack)
                {
                    playerFSM.Transition(stateType.air_attack_3);
                }

                else
                {
                    parameter.goAttack_3 = true;
                    playerFSM.Transition(stateType.jumpIdle);
                }
            }
        }
    }
    public void OnExit()
    {
        parameter.acceptNextOrder = false;
        parameter.goNextAttack = false;
        parameter.creatAttackTrigger = false;
        parameter.destroyAttackTrigger = false;
    }
}

public class Air_Attack_3_State : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;
    private GameObject attackTriggerPrefab;
    private GameObject attackTriggerObject;
    private float moveDuration;

    public Air_Attack_3_State(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }
    public void OnEnter()
    {
        parameter.playerAnimator.Play("asuna_air_attack_3");
        parameter.currentAnimEnd = false;
        parameter.playerRigidbody.velocity = new Vector2(0, 0);
        attackTriggerPrefab = Resources.Load<GameObject>("Prefab/Charactor/asuna_air_attack_3");
        moveDuration = 0.1f;
    }
    public void OnUpdate()
    {
        //if (parameter.hasExtraMovement)
        //{
        //    parameter.playerRigidbody.velocity = new Vector2(60f * playerFSM.transform.localScale.x, -8);
        //}
        //else
        //{
        //    parameter.playerRigidbody.velocity = new Vector2(0, 0);
        //}
        if (parameter.hasExtraMovement)
        {
            if (moveDuration >= 0 && parameter.playerAnimator.speed == 1)
            {
                parameter.playerRigidbody.velocity = new Vector2(60f * playerFSM.transform.localScale.x, 0);
                moveDuration -= Time.deltaTime;
            }
            else if (moveDuration >= 0 && parameter.playerAnimator.speed == 0.1)
            {
                parameter.playerRigidbody.velocity = new Vector2(6f * playerFSM.transform.localScale.x, 0);
            }
            else
            {
                parameter.hasExtraMovement = false;
                parameter.playerRigidbody.velocity = new Vector2(0, 0);
            }
        }


        if (parameter.creatAttackTrigger && attackTriggerObject == null)
        {
            attackTriggerObject = GameObject.Instantiate(attackTriggerPrefab, playerFSM.transform);
        }
        if (parameter.destroyAttackTrigger)
        {
            GameObject.Destroy(attackTriggerObject);
        }

        if (parameter.currentAnimEnd)
        {
            if (!parameter.isGround)
            {
                playerFSM.Transition(stateType.jumpDown);
            }
            else
                playerFSM.Transition(stateType.idle);
        }
    }

    public void OnExit()
    {
        parameter.creatAttackTrigger = false;
        parameter.destroyAttackTrigger = false;
    }
}

public class Jump_Idle:IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;
    private float timer;

    public Jump_Idle(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }

    public void OnEnter()
    {
        parameter.playerAnimator.Play("jumpTop_2");
        parameter.playerRigidbody.velocity = new Vector2(0, 0);
        timer = 0;
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;
        parameter.playerRigidbody.velocity = new Vector2(parameter.playerRigidbody.velocity.x, parameter.playerRigidbody.velocity.y - 0.5f);
        if (timer>=0.3)
        {
            parameter.airAttackCD = true;
            playerFSM.Transition(stateType.jumpDown);
        }

        if (keyboardState.pressedRight)
        {
            parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed, parameter.playerRigidbody.velocity.y);
            playerFSM.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (keyboardState.pressedLeft)
        {
            parameter.playerRigidbody.velocity = new Vector2(parameter.playerMoveSpeed * -1, parameter.playerRigidbody.velocity.y);
            playerFSM.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            parameter.playerRigidbody.velocity = new Vector2(0, parameter.playerRigidbody.velocity.y);
        }

        if (parameter.goAttack_2&&keyboardState.pressedAttack)
        {
            playerFSM.Transition(stateType.air_attack_2);
        }
        if (parameter.goAttack_3 && keyboardState.pressedAttack)
        {
            playerFSM.Transition(stateType.air_attack_3);
        }
    }

    public void OnExit()
    {
        parameter.goAttack_2 = false;
        parameter.goAttack_3 = false;
    }

}

public class Climb_Border : IState
{
    private PlayerController playerFSM;
    private PlayerParameter parameter;
    private KeyBoardDetection keyboardState;

    public Climb_Border(PlayerController playerController)
    {
        playerFSM = playerController;
        parameter = playerController.playerParameter;
        keyboardState = playerController.PlayerKeyboardStates;
    }

    public void OnEnter()
    {
        parameter.playerAnimator.Play("jumpTop_2");
        parameter.playerRigidbody.velocity = new Vector2(parameter.playerRigidbody.velocity.x, 20);
    }

    public void OnUpdate()
    {
        parameter.playerRigidbody.velocity = new Vector2(parameter.playerRigidbody.velocity.x, 20);
        if (!parameter.climbBorder)
        {
            playerFSM.Transition(stateType.jumpDown);
        }
    }

    public void OnExit()
    {

    }

}