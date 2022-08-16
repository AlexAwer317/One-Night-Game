using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown; //Перезарядка атаки
    [SerializeField] private float range; //Далность атаки
    [SerializeField] private float damage; //Урон врага


    [Header ("Ranged Attack")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;

    [Header ("Collider Parameters")]
    [SerializeField] private float colliderDistance; //Дистанция от колайдера игрока
    [SerializeField] private BoxCollider2D boxCollider; //Ссылка на бокс колайдер врага



    [Header ("Player Layer")]
    [SerializeField] private LayerMask playerLayer;  //Маска слоя игрока
    private float cooldownTimer = Mathf.Infinity; //Устанавливаем значение таймера перезарядки в значение точки бесконечности, чтобы враг мог атаковать сразу
    private Animator anim; //Ссылка на аниматор


    //References
    private EnemyPatrol enemyPatrol;


    private void Awake() 
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update() 
    {
        cooldownTimer += Time.deltaTime;

        //Атака врага
        if(PlayerInSight())
        {
            if(cooldownTimer >=attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("rangeAttack");
            }
        }

        //Если враг не видит игрока продолжить патруль, в противном случае атака
        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }


    private void RangedAttack()
    {
        cooldownTimer = 0;
        fireballs[FindFareball()].transform.position = firePoint.position;
        fireballs[FindFareball()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindFareball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if(!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }



    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 
            0, Vector2.left, 0, playerLayer); //Создаем бокс для поиска игрока


        return hit.collider != null;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)); //Визуализация в редакторе метода поиска игрока
    }



}
