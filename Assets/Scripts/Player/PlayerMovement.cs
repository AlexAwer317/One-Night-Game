using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float PlayerSpeed; // Скорость игрока
    [SerializeField] private float jumpSpeed;
    private float wallJumpCooldown;
    private float horizontalInput;

    //private bool grounded;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    private BoxCollider2D boxCollider;


    private Rigidbody2D playerRB;  // Ссылка на компонент Rigidbody2D у игрока
    private Animator anim; //Ссылка на компонент Animator игрока

    private void Awake() 
    {
        playerRB = GetComponent<Rigidbody2D>(); // Получение данных компонента Rigidbody игрока
        anim = GetComponent<Animator>(); //Получение данных компонента Animator игрока
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update() 
    {
        horizontalInput = Input.GetAxis("Horizontal"); //Механика движения через проверку нажатия клавиш направления

        playerRB.velocity = new Vector2(horizontalInput * PlayerSpeed, playerRB.velocity.y); // Направление передвижения игрока по кардинате X. Остальные кардинаты залочены
    

        //Проверка на горизонатльный ввод игрока, для поворота спрайта при движение вправо влево
        if(horizontalInput > 0.01f)
            transform.localScale = Vector3.one; //Поворот спрайта вправо
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1,1,1); // Поворот спрайта игрока влево

        

    
        anim.SetBool("run", horizontalInput != 0); //Анимация бега
        anim.SetBool("grounded", isGrounded()); //Анимация прыжка
        

        //Прыжок от стены
        if(wallJumpCooldown > 0.2f)
        {
   
        
        
        playerRB.velocity = new Vector2(horizontalInput * PlayerSpeed, playerRB.velocity.y); // Направление передвижения игрока по кардинате X. Остальные кардинаты залочены
        
                //проверка находится ли игрок у стены
        if(onWall() && !isGrounded())
        {
            playerRB.gravityScale = 0;
            playerRB.velocity = Vector2.zero;
        }
        else
            playerRB.gravityScale = 5; //Возвращение гравитации в норму после отхода от стены

        //Механика прыжка
            if(Input.GetKey(KeyCode.Space)) 
                Jump();

        }
        else
            wallJumpCooldown += Time.deltaTime;


    }


    //Метод прыжок
    private void Jump() 
    {
        if(isGrounded())
        {
        playerRB.velocity = new Vector2(playerRB.velocity.x, jumpSpeed); 
        anim.SetTrigger("jump"); //Активация тригера
        } 
        else if(onWall() && !isGrounded()) //повторный прыжок от стены
        {   
            if(horizontalInput == 0) 
            {
                playerRB.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 16, 0); //Отскок от стены
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z); //разворот направления при отталкивание
            }
            else
                playerRB.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 9, 12); //механика прыжка от стены

            wallJumpCooldown = 0; //скидываем откат
            
        }
    }


    //Проверка стоит ли игрок на земле
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer); //Создаем луч чуть-чуть вниз равный размеру колайдера игрока
        return raycastHit.collider != null; //Возвращаем значение
    }


    //Прыжок по стене
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider !=null;
    }


    // Когда игрок может атаковать
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall(); 
    }
}
