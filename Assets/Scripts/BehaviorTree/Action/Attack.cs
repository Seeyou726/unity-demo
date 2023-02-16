using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Attack : Action
{
    public SharedString animName;
    public SharedInt direction;
    private GameObject character;
    private GameObject attackTriggerPrefab;
    private GameObject attackTriggerObject;
    private EnemyController enemyController;

    public override void OnStart()
    {
        character = this.gameObject;
        character.GetComponent<Animator>().Play(animName.Value);
        character.transform.localScale = new Vector3(direction.Value, 1, 1);
        enemyController = GetComponent<EnemyController>();
        enemyController.enemyParameter.currentAnimEnd = false;
        attackTriggerPrefab = Resources.Load<GameObject>("Prefab/Charactor/"+animName.Value);
    }

    public override TaskStatus OnUpdate()
    {
        if (enemyController.enemyParameter.creatAttackTrigger&& attackTriggerObject == null)
        {
            attackTriggerObject = GameObject.Instantiate(attackTriggerPrefab, this.transform);
            return TaskStatus.Running;
        }
        if (enemyController.enemyParameter.destroyAttackTrigger&& attackTriggerObject != null)
        {
            GameObject.Destroy(attackTriggerObject);
            return TaskStatus.Running;
        }
        if (enemyController.enemyParameter.currentAnimEnd)
        {
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }

    public override void OnEnd()
    {
        enemyController.enemyParameter.creatAttackTrigger = false;
        enemyController.enemyParameter.destroyAttackTrigger = false;
    }


}
