using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooms : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies; //Массив всех врагов в комнате
    private Vector3[] initialPosition; //Начальная позиция всех врагов


    private void Awake() 
    {
        //Сохраняем начальную позицию всех врагов
        initialPosition = new Vector3[enemies.Length]; //Проверяем, что длина массива позиций равна длине массива врагов

        //Перебираем массив врагов и присваиваем их начальные позиции
        for(int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i] != null) //Проверяем что объект существует
                initialPosition[i] = enemies[i].transform.position;
        }
    }

    public void ActivateRoom(bool _status)
    {
        for(int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i] != null) 
            {
                enemies[i].SetActive(_status);
                enemies[i].transform.position = initialPosition[i];
            }
        }
    }
}
