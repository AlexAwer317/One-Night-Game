using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attcakCooldown; //Перезарядка до следующего выстрела
    [SerializeField] private Transform firePoint; //Откуда стреляем(огневая точка)
    [SerializeField] private GameObject[] arrows; //Массив снарядов
    private float coolDownTimer; //Таймер перезарядки

    private void Attack()
    {
        coolDownTimer = 0; //Обнуляем таймер перезарядки
        arrows[FindArrow()].transform.position = firePoint.position; //Приравниваем позицию снаряда к позиции огневой точки
        arrows[FindArrow()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }


        //Ищем снаряд для повторного выстрела
    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if(!arrows[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
    

    private void Update() 
    {
        coolDownTimer += Time.deltaTime; //Считаем таймер пперезарядки

        //Стреляем
       if(coolDownTimer >= attcakCooldown)
            Attack(); 
    }
}
