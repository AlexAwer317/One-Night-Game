using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    //RoomCamera
    [SerializeField] private float speed; // Скорость перемещения камеры
    private float currentPosX; //Позиция камеры по X
    private Vector3 velocity = Vector3.zero; //Вектор направления движения камеры


    //FollowCamera
    [SerializeField] private Transform player; //Ссылка на позицию игрока
    [SerializeField] private float aheadDistance; //Растояние от игрока до угла экрана
    [SerializeField] private float cameraSpeed; //Скорость камеры
    private float lookAhead; //Просчет передвижения камеры вперед


    private void Update() {

        //RoomCamera
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed); //Изменения позиции камеры, через метод smoothDamp
       
       //FollowPlayer
        transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed); //Высчитываем lookAhead для прибавление к перемещению камеры
    }

    //Метод изменяющий позицию камеры в другую комнату привязывая камеру не к персонажу, а к комнате
    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x; //Перемещение камеры в новую комнату
    }
}
