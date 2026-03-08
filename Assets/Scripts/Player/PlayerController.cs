using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

// assure qu'il y a un CharacterController et envoie une notification si on le supprime
[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;

    // gravite par defaut
    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;

    [SerializeField] private float smoothTime = 0.05f;
    private float _currentVelocity;

    [SerializeField] private float speed;

    [SerializeField] private float jumpPower;

    // dash
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;

    // attaque
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 10f;

    // animation
    [SerializeField] private Animator animator;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        // nettoie le trail en debut de game
        if (tr != null)
        {
            tr.Clear();
            tr.emitting = false;
        }
    }

    private void Update()
    {
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();

        // le personnage regarde dans la bonne direction
        if (_input.sqrMagnitude == 0) return;

        // le personnage effectue un dash
        if (isDashing) return;

        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

        _characterController.Move(_direction * speed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        // verifie si le personnage touche le sol
        if (IsGrounded() && _velocity < 0.0f)
        {
            _velocity = -1.0f;
        }
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }

         _direction.y = _velocity;
    }

    private void ApplyRotation()
    {
        if (_input.sqrMagnitude == 0) return;

        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    private void ApplyMovement()
    {
        _characterController.Move(_direction * speed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();

        // au lieu de bouger sur l'axe Y quand on veut aller en avant ou en arriere, on bouge sur l'axe Z
        _direction = new Vector3(_input.x, 0.0f, _input.y);

        // declenche l'animation si bouton appuye
        if (context.performed)
            animator.SetBool("Walk", true);
        Debug.Log("Marche effectue");

        // arrete l'animation si bouton relache
        if (context.canceled)
            animator.SetBool("Walk", false);
        Debug.Log("Marche stop");
    }

    public void Jump(InputAction.CallbackContext context)
    {
        // verifie si on tient la barre espace
        if (!context.started) return;

        // verifie si on n'est pas deja en l'air
        if (!IsGrounded()) return;

        // déclenche l'animation Jump via Trigger
        animator.SetTrigger("Jump");
        Debug.Log("Saut effectue");

        // vitesse verticale pour le saut
        _velocity = jumpPower;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        // on verifie que le bouton vient d'�tre press�
        if (!context.started) return;

        // empeche de dash si on est deja en train de dash ou si le cooldown n'est pas fini
        if (isDashing || !canDash) return;

        // lance la coroutine du dash
        StartCoroutine(DashCoroutine());
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // animation
        animator.SetTrigger("Attack");
        Debug.Log("Attack effectue");

        // direction de tir : toujours devant le joueur
        Vector3 shootDirection = transform.forward;

        // instanciation du projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(shootDirection));

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

            // on utilise velocity pour que le projectile parte instantanement
            rb.AddForce(shootDirection * projectileSpeed, ForceMode.VelocityChange);
        }
    }

    private bool IsGrounded() => _characterController.isGrounded;

    // coroutine dash
    private IEnumerator DashCoroutine()
    {
        // empeche de relancer un dash
        canDash = false;
        isDashing = true;

        // declenche l’animation des le depart
        if (animator != null)
            animator.SetTrigger("Dash");

        // active le trail
        tr.emitting = true;

        // direction du dash
        Vector3 dashDirection = transform.forward;

        float startTime = Time.time;

        // deplacement pendant la duree du dash
        while (Time.time < startTime + dashingTime)
        {
            _characterController.Move(dashDirection * dashingPower * Time.deltaTime);
            yield return null;
        }

        // desactive le trail quand le dash est fini ainsi que l'animation
        tr.emitting = false;
        isDashing = false;

        // cooldown avant de pouvoir redash
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

}
