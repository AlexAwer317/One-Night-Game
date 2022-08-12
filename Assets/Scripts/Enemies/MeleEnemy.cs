using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown; //Перезарядка атаки
    [SerializeField] private float range; //Далность атаки
    [SerializeField] private float colliderDistance; //Дистанция от колайдера игрока
    [SerializeField] private int damage; //Урон врага
    [SerializeField] private BoxCollider2D boxCollider2D; //Ссылка на бокс колайдер врага
    [SerializeField] private LayerMask playerLayer;  //Маска слоя игрока
    private float cooldownTimer = Mathf.Infinity; //Устанавливаем значение таймера перезарядки в значение точки бесконечности, чтобы враг мог атаковать сразу
    private Animator anim; //Ссылка на аниматор
    private Health playerHealth;

    private void Awake() 
    {
        anim = GetComponent<Animator>();
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
                anim.SetTrigger("meleAttack");
            }
        }
    }

    //Проверяем видит ли враг игрока
    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
        new Vector3(boxCollider2D.bounds.size.x * range, boxCollider2D.bounds.size.y, boxCollider2D.bounds.size.z), 
            0, Vector2.left, 0, playerLayer); //Создаем бокс для поиска игрока

        return hit.collider != null;

        //Если игрок в зоне видимости получаем его компонент здоровья
        if(hit.collider !=null)
        {
            playerHealth = hit.transform.GetComponent<Health>(); //Получаем ссылку на компонент здоровья игрока
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider2D.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider2D.bounds.size.x * range, boxCollider2D.bounds.size.y, boxCollider2D.bounds.size.z)); //Визуализация в редакторе метода поиска игрока
    }

    private void DamagePlayer()
    {
        //Если плеер в зоне видимости нанести урон (Функция для ивент системы юнити)
        if (PlayerInSight())
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
