using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startHealth; //Сколько здоровья у игрока на старте
    public float currentHealth {get; private set; } //Текущие здоровье игрока
    private Animator anim;
    private bool dead;


    [Header ("iFrames")]
    [SerializeField] private float iFramesDuration; //Как долго игрок не уязвим
    [SerializeField] private int numberOfflashes; //Сколько вспышек до возвращения в норм состояние
    private SpriteRenderer spriteRend; //ссылка на спрайт рендерер

    [Header ("Components")]
    [SerializeField] private Behaviour[] components; //Массив компонентов

     private void Awake() 
    {
        currentHealth = startHealth; //Приравниваем на старте игры хп к стартовому значению
        anim = GetComponent<Animator>(); //Ссылка на анимтор
        spriteRend = GetComponent<SpriteRenderer>(); //Ссылка на спрайт рендерер
    }    

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startHealth); //Просчет изменения здоровья, чтобы оно не падало ниже нуля или больше максимума
        

        if(currentHealth > 0)
        {
            anim.SetTrigger("hurt"); //Запуск анимации получения урона
            StartCoroutine(Invunerability()); //Запуск корутины с периодом неуязвимости после урона
        }
        else
        {
            if(!dead)
            {
                anim.SetTrigger("die"); //Включаем анимацию смерти

                //Игрок
              /*  if(GetComponent<PlayerMovement>() != null)
                    GetComponent<PlayerMovement>().enabled = false; //Выключаем скрипт движения игрока после смерти

                //Враг
                if(GetComponentInParent<EnemyPatrol>() != null)
                    GetComponentInParent<EnemyPatrol>().enabled = false;

                if(GetComponent<MeleEnemy>() !=null)
                    GetComponent<MeleEnemy>().enabled = false;*/

                //Отключение всех компонентов объекта при смерти у всех классов
                foreach (Behaviour component in components)
                    component.enabled = false;
                


                dead = true; //Устанавливаем переменную в состояние игрок умер
            }
        }
    }
    
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startHealth); //Просчет хила, оно не может быть меньше нуля и больше максимума
    }

    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(8, 9, true); //Игрок начинает игнорировать столкновения
        for (int i = 0; i < numberOfflashes; i++)
        {
            spriteRend.color = new Color(1,0,0, 0.5f); //Смена цвета спрайта на красный в момент неуязвипости
            yield return new WaitForSeconds(iFramesDuration / (numberOfflashes * 2)); //Приостановка цикла
            spriteRend.color = Color.white; // Возвращение цвета в белый цвет
            yield return new WaitForSeconds(iFramesDuration / (numberOfflashes * 2)); //Приостановка цикла
        }
        Physics2D.IgnoreLayerCollision(8, 9, false); //Игрок прекратил игнорировать столкновения
    }
    private void Diactivate()
    {
        gameObject.SetActive(false);
    }
}

