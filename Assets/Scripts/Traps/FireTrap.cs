using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header ("FireTrap Timers")]
    [SerializeField] private float activationDelay; //Через сколько активируется ловушка
    [SerializeField] private float activeTime; //Сколько ловушка остается активной
    private Animator anim;
    private SpriteRenderer spriteRend;


    private bool triggered; //Ловушка активирована
    private bool active; //Активна ли ловушка

    private void Awake() 
    {
        anim = GetComponent<Animator>(); //Ссылка на аниматор
        spriteRend = GetComponent<SpriteRenderer>(); //Ссылка на спрайт ренедер
    }

    
    private void OnTriggerStay2D(Collider2D collision) 
    {
        if(collision.tag == "Player") 
        {
            if(!triggered)
            {
                StartCoroutine(ActivateFiretrap()); //Запуск ловушки
            }
            if(active)
            {
                collision.GetComponent<Health>().TakeDamage(damage); //Метод приченения урона игроку
            }
        }   
    }
    

    //Активация ловушки
    private IEnumerator ActivateFiretrap()
    {
        //Срабатывает тригер ловушки
        triggered = true;  //Активируем срабатывание
        spriteRend.color = Color.red; //При срабатывание тригера меняем цвет спрайта для предупреждения игрока

        //Ловушка активируется
        yield return new WaitForSeconds(activationDelay); //Пауза в работе корутины
        spriteRend.color = Color.white; //Возвращаем цвет ловушки в исходное состояние после срабатывания
        active = true; //Активируем ловушку
        anim.SetBool("activated", true);


        //Ловушка деактивируется через несколько секунд
        yield return new WaitForSeconds(activeTime); //Пауза в работе корутины
        active = false; //Возвращаем триггеры в состояние выключена
        triggered = false; //Выключаем тригер срабатывания
        anim.SetBool("activated", false);
    }
}
