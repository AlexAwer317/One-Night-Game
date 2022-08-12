using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField] private float attackCooldown; //Задержка между атаками
    [SerializeField] private Transform firePoint; // Задаем позицию активации/создания огненного шара
    [SerializeField] private GameObject[] fireballs; //Массив объектов огненных шаров
    private float cooldownTimer = Mathf.Infinity; // таймер перезарядки выстрела с точкой бесконечности, для возможности стрелять после запуска игры

    private Animator anim; //ссылка на аниматор
    private PlayerMovement playerMovement; // ссылка скрипт движения игрока




 private void Awake() {
    anim = GetComponent<Animator>(); //получаем данные аниматора
    playerMovement = GetComponent<PlayerMovement>(); // получаем данные движение игрока
}


 private void Update() {
    
    if(Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack()) //Проверяем нажата ли левая кнопка мыши и прошло ли достаточно времени с последнего выстрела, если да вызываем метод атаки
        Attack();


    cooldownTimer += Time.deltaTime; //запуск кулдауна после стрельбы
}

//метод атаки
private void Attack()
{
    anim.SetTrigger("attack"); //устанавливаем анимацию атаки
    cooldownTimer = 0; //Обнуляем кулдаун атаки

    //pool fireball создание объекта фаербола без пересоздание копии объекта
    fireballs[FindFireball()].transform.position = firePoint.position; //Позиция фаербола = позиции фаерпоинта
    fireballs[FindFireball()].GetComponent<Fireball>().SetDirection(Mathf.Sign(transform.localScale.x)); //получаем компонент фаербола и используем заданное направление
}


//Перебор массива фаерболов и поиск не активного шара
private int FindFireball()
{
    for (int i = 0; i < fireballs.Length; i++)
    {
        if(!fireballs[i].activeInHierarchy)
            return i;   
    }
    return 0;
}

}
