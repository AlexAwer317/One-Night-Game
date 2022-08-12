using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
   [SerializeField] private float speed;
   private bool hit;
   private float direction;
   private float lifeTime;  // Время жизни 1 шара

   private BoxCollider2D boxCollider2D; //Ссылка на бокс колайдер пули
   private Animator anim; //Ссылка на аниматор пули

   private void Awake() {
    boxCollider2D = GetComponent<BoxCollider2D>();
    anim = GetComponent<Animator>();

   }


   private void Update() {
    if(hit) return; //Проверка попала ли во что то пуля
    
    float movementSpeed = speed * Time.deltaTime * direction; //Скорость пули и направление полета
    transform.Translate(movementSpeed, 0, 0 ); //перемещение пули

    lifeTime += Time.deltaTime; // Расчитываем время жизни шара
    if(lifeTime > 5) gameObject.SetActive(false); //Если время жизни шара привышает 5 секунд объект деактивируется

   }

   private void OnTriggerEnter2D(Collider2D collision) 
   {
        
        hit = true;
        boxCollider2D.enabled = false;
        anim.SetTrigger("explode");
   }


    //расчитываем направление выстрела
   public void SetDirection(float _direction)
   {
        lifeTime = 0;
        direction = _direction; //Задаем направление для расчета скорости и направления пули
        gameObject.SetActive(true); //Включаем объект фаербола
        hit = false; //Устанавливаем переменную попадания в значение не верно
        boxCollider2D.enabled = true; //включаем бокс колайдер фаер бола


        //расчет направления спрайта
        float localScaleX = transform.localScale.x;
        if(Mathf.Sign(localScaleX) != _direction)
            localScaleX = - localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z); //разворачиваем спрайт фаербола
   }

    //Отключение объекта через ивент анимации
   private void Diactivate()
   {
    gameObject.SetActive(false); 
   }
}
