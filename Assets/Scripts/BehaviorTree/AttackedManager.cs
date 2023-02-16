using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedManager : MonoBehaviour
{
    public string triggerTag= "PlayerAttack";
    public bool beAttacked;
    public GameObject attackedPrefab;
    public GameObject player;
    public float attackedDamage;
    private Animator animator;
    private Animator playerAnimator;
    private AnimatorStateInfo currentAnimInfo;
    public bool currentAnimEnd = true;
    private new Rigidbody2D rigidbody;
    private Rigidbody2D playerRigidbody;
    private PlayerController playerController;
    private bool isShake = false;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();
        playerController = player.GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        playerRigidbody = player.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        currentAnimInfo = animator.GetCurrentAnimatorStateInfo(0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(triggerTag))
        {
            beAttacked = true;
            attackedPrefab = collision.gameObject;
            attackedPrefab.name = collision.gameObject.name.Replace("(Clone)", "");
            AttackedDamage(attackedPrefab.name);
            AttackedAnim(attackedPrefab.name);
            AttackedEffect(attackedPrefab.name);
        }
    }

    private void AttackedDamage(string prefabName)
    {
        switch (prefabName)
        {
            case "asuna_attack_1":
                attackedDamage = 10;
                break;
            case "asuna_attack_2":
                attackedDamage = 20;
                break;
            case "asuna_attack_3":
                attackedDamage = 30;
                break;
            case "asuna_uppercut_attack":
                attackedDamage = 40;
                break;
            case "asuna_air_attack_1":
                attackedDamage = 10;
                break;
            case "asuna_air_attack_2":
                attackedDamage = 20;
                break;
            case "asuna_air_attack_3":
                attackedDamage = 30;
                break;
        }
    }

    private void AttackedAnim(string prefabName)
    {
        if (player.transform.localScale.x<0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        switch (prefabName)
        {
            case "asuna_attack_1":
                animator.Play("be_attacked_ground");
                break;
            case "asuna_attack_2":
                animator.Play("be_attacked_ground");
                break;
            case "asuna_attack_3":
                animator.Play("be_attacked_ground");
                break;
            case "asuna_uppercut_attack":
                animator.Play("be_attacked_toAir");
                break;
            case "asuna_air_attack_1":
                animator.Play("be_attacked_air");
                break;
            case "asuna_air_attack_2":
                animator.Play("be_attacked_air");
                break;
            case "asuna_air_attack_3":
                animator.Play("be_attacked_air");
                break;
        }
    }

    private void AttackedEffect(string prefabName)
    {
        switch (prefabName)
        {
            case "asuna_attack_1":
                AttackedMove(new Vector2(20,0),0.02f);
                AttackedSlow(animator,rigidbody, 0.1f,0.2f);
                AttackedSlow(playerAnimator,playerRigidbody, 0.1f, 0.2f);
                CameraShake(0.01f,0.1f);
                break;
            case "asuna_attack_2":
                AttackedMove(new Vector2(20, 0), 0.02f);
                AttackedSlow(animator, rigidbody, 0.1f, 0.2f);
                AttackedSlow(playerAnimator, playerRigidbody, 0.1f, 0.2f);
                CameraShake(0.01f, 0.1f);
                break;
            case "asuna_attack_3":
                AttackedMove(new Vector2(40, 0), 0.03f);
                AttackedSlow(animator, rigidbody, 0.1f, 0.2f);
                AttackedSlow(playerAnimator, playerRigidbody, 0.1f, 0.2f);
                CameraShake(0.01f, 0.1f);
                break;
            case "asuna_uppercut_attack":
                AttackedMove(new Vector2(0, 20), 0.2f);
                AttackedSlow(animator, rigidbody, 0.1f, 0.2f);
                AttackedSlow(playerAnimator, playerRigidbody, 0.1f, 0.2f);
                CameraShake(0.01f, 0.1f);
                break;
            case "asuna_air_attack_1":
                AttackedMove(new Vector2(3, 0), 0.02f);
                AttackedSlow(animator, rigidbody, 0.1f, 0.2f);
                AttackedSlow(playerAnimator, playerRigidbody, 0.1f, 0.2f);
                CameraShake(0.01f, 0.1f);
                break;
            case "asuna_air_attack_2":
                AttackedMove(new Vector2(3, 0), 0.02f);
                AttackedSlow(animator, rigidbody, 0.1f, 0.2f);
                AttackedSlow(playerAnimator, playerRigidbody, 0.1f, 0.2f);
                CameraShake(0.01f, 0.1f);
                break;
            case "asuna_air_attack_3":
                AttackedMove(new Vector2(3, 0), 0.02f);
                AttackedSlow(animator, rigidbody, 0.1f, 0.2f);
                AttackedSlow(playerAnimator, playerRigidbody, 0.1f, 0.2f);
                CameraShake(0.01f, 0.1f);
                break;
        }
    }

    private void AttackedMove(Vector2 speed, float moveTime)
    {
            StartCoroutine(BeAttackedMove(speed, moveTime));
    }
    IEnumerator BeAttackedMove(Vector2 speed,float moveTime)
    {
        rigidbody.velocity = new Vector2(speed.x*player.transform.localScale.x,speed.y);
        yield return new WaitForSecondsRealtime(moveTime);
        rigidbody.velocity = new Vector2(0,0);
    }

    private void AttackedSlow(Animator animator, Rigidbody2D rigidbody2D, float slowRatio, float slowTime)
    {
            StartCoroutine(BeAttackedSlow(animator, rigidbody2D, slowRatio, slowTime));
    }
    IEnumerator BeAttackedSlow(Animator animator,Rigidbody2D rigidbody2D, float slowRatio, float slowTime)
    {
          
        animator.speed = slowRatio;
        yield return new WaitForSecondsRealtime(slowTime);
        animator.speed = 1;
    }

    private void CameraShake(float strength, float shakeTime)
    {
        if (!isShake)
        {
            StartCoroutine(BeAttackedShake(strength,shakeTime));
        }
    }
    IEnumerator BeAttackedShake(float strength,float shakeTime)
    {
        isShake = true;
        Transform camera = Camera.main.transform;
        Vector3 startPosition = camera.position;
        while (shakeTime>0)
        {
            camera.position = Random.insideUnitSphere * strength + startPosition;
            shakeTime -= Time.deltaTime;
            yield return null;
        }
        isShake = false;
    }
}
