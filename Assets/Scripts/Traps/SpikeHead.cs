using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHead : EnemyDamage
{
    [Header("SpikeHead Attributes")]
    [SerializeField] private float speed; //Скорость
    [SerializeField] private float range; //Как далеко видит головка шипа
    [SerializeField] private float checkDelay; //Задержка между атаками
    [SerializeField] private LayerMask playerLayer;
    private Vector3 destination; //При обнаружение игрока сохраняем его позицию
    private Vector3[] directions = new Vector3[4]; // Массив направлений поиска игрока
    private bool attacking; //Враг атакует
    private float checkTimer; //Таймер до следующей атаки

    

    private void OnEnable() 
    {
        Stop();
    }

    private void Update() 
    {
        //Перемещение головки шипа к игроку при атаке
        if(attacking)
            transform.Translate(destination * Time.deltaTime * speed);
        else
        {
            checkTimer += Time.deltaTime;
            if(checkTimer > checkDelay)
                CheckForPlayer();
        }
    }

    //Ищем игрока вошел ли он в зону видимости
    private void CheckForPlayer()
    {
        CalculateDirection();

        //Головка шипа проверяет видит ли игрока во всех 4 направлениях
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if(hit.collider != null && !attacking)
            {
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
            }
        }
    }

    //Просчитываем 4 направления для поиска игрока
    private void CalculateDirection()
    {
        directions[0] = transform.right * range; //Направление поиска игрока справа
        directions[1] = -transform.right * range; //Направление поиска игрока слева
        directions[2] = transform.up * range; // Направление поиска игрока сверху
        directions[3] = -transform.up * range; //Направление поиска игрока внизу
    }

    private void Stop()
    {
        destination = transform.position; //Преобразуем пункт назначения в текущее положение
        attacking = false; //Можно опять атаковать
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        base.OnTriggerEnter2D(collision);
        
        //Остановка атаки после попадания
        Stop();
    }
}
