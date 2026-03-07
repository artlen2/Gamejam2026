using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 1f;

    private float skinWidth = 0.1f;

    void Start()
    {
        // projectiles rates detruits apres 3 secondes
        Destroy(gameObject, 3f);

        // verifie si le projectile est dans un objet
        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, 0.1f, collisionMask);

        if (initialCollisions.Length > 0)
        {
            OnHitObject(initialCollisions[0], transform.position);
        }
    }

    // modifier la vitesse du projectile dans un autre script (playercontroller)
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {
        // calcule la distance / frame
        float moveDistance = speed * Time.deltaTime;

        // verifie s'il va y avoir une collision
        if (CheckCollisions(moveDistance))
            return;

        // projectile lance en avant
        transform.Translate(Vector3.forward * moveDistance);
    }

    bool CheckCollisions(float moveDistance)
    {
        // rayon qui part du firepoint vers la cible
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // verifie si le projectile touche un objet dans le rayon
        if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
        {
            // place le projectile sur la surface
            transform.position = hit.point;

            // si touche, impact
            OnHitObject(hit.collider, hit.point);

            return true;
        }

        return false;
    }

    void OnHitObject(Collider collider, Vector3 hitPoint)
    {
        Debug.Log("Projectile hit: " + collider.name);

        IDamageable damageableObject = collider.GetComponent<IDamageable>();

        if (damageableObject == null)
        {
            damageableObject = collider.GetComponentInParent<IDamageable>();
        }

        if (damageableObject != null)
        {
            Debug.Log("Applying damage: " + damage);  // Vérifie le montant des dégâts appliqués
            damageableObject.TakeDamage(damage);
        }

        Destroy(gameObject);  // Détruire le projectile après l'impact
    }
}