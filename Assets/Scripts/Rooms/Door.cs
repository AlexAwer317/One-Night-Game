using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //RoomCamera
    [SerializeField] private Transform previousRoom; //Комната в которой находится игрок
    [SerializeField] private Transform nextRoom; //Комната в которую входит игрок
    [SerializeField] private CameraController cam; //Ссылка на контролер камеры, для вызова методов


//Проверка столкновения игрока с тригером двери Room Camera
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.tag == "Player") //Есть ли столкновение с игроком
        {
            if(collision.transform.position.x < transform.position.x) //Позиция с какой стороны есть столкновение с игроком
            {
                cam.MoveToNewRoom(nextRoom); //Если игрок идет с лево даем команду камере переместится в следующую комнату
                nextRoom.GetComponent<Rooms>().ActivateRoom(true);
                previousRoom.GetComponent<Rooms>().ActivateRoom(false);
            }
            else
            {
                cam.MoveToNewRoom(previousRoom); //Игрок идет с права возвращаемся в предыдущую комнату
                previousRoom.GetComponent<Rooms>().ActivateRoom(true);
                nextRoom.GetComponent<Rooms>().ActivateRoom(false);
            }
        }
    }
}
