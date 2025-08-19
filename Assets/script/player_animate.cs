using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_animate : MonoBehaviour
{
    public float speed = 4.7f;
    public float jumpForce = 5.94f;

    public GameObject swordHitbox;

    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    private float Horizontal;
    private bool Grounded;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        Horizontal = Input.GetAxis("Horizontal");

        // Flip
        if (Horizontal < 0.0f) transform.localScale = new Vector3(-3f, 3f, 3f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(3f, 3f, 3f);

          if (Input.GetKeyDown(KeyCode.Space))
    {
        Animator.SetTrigger("attack");  // 👈 dispara la animación
    }

        // Correr
        Animator.SetBool("running", Horizontal != 0.0f);

        // Detección de suelo con Raycast
        Debug.DrawRay(transform.position, Vector3.down * 0.5f, Color.blue);
        Grounded = Physics2D.Raycast(transform.position, Vector3.down, 0.5f);

        // Saltar
if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && Grounded)
{
    Jump();
    Animator.SetBool("running", false); // 👈 forzar salir de correr
    Animator.SetBool("jumping", true);  // 👈 activar salto
}

        // Animaciones de salto y caída
        if (!Grounded)
        {
            if (Rigidbody2D.linearVelocity.y > 0) // subiendo
            {
                Animator.SetBool("jumping", true);
                Animator.SetBool("falling", false);
            }
            else if (Rigidbody2D.linearVelocity.y < 0) // bajando
            {
                float distanciaSuelo = CheckGroundDistance();

                // SOLO activa "falling" si estás cerca del suelo
                if (distanciaSuelo < 2f)  // 👈 ajusta este valor según tu escenario
                {
                    Animator.SetBool("falling", true);
                    Animator.SetBool("jumping", false);
                }
                else
                {
                    Animator.SetBool("falling", false);
                }
            }
        }
        else
        {
            Animator.SetBool("jumping", false);
            Animator.SetBool("falling", false);
        }
    }

    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        Rigidbody2D.linearVelocity = new Vector2(Horizontal * speed, Rigidbody2D.linearVelocity.y);
    }

    // Función para medir la distancia real al suelo
    private float CheckGroundDistance()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 5f);
        if (hit.collider != null)
        {
            return hit.distance;
        }
        return Mathf.Infinity;
    }

    
    // 👉 estas funciones las llamas desde la animación con Animation Events
    public void EnableHitbox()
    {
        if (swordHitbox != null) swordHitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        if (swordHitbox != null) swordHitbox.SetActive(false);
    }
}
