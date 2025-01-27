using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public int damage; 
    public Gun gun; 
    public Vector3 originPosition; 

    public int minimumDamage; 
    public int maximumDamage; 
    public float maximumRange; 

    private Rigidbody rb;
    private float flightTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        originPosition = gun.firePoint.position; 
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        flightTime += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            float distanceTraveled = flightTime * speed;
            float normalizedDistance = Mathf.Clamp01(distanceTraveled / maximumRange);
            damage = Mathf.RoundToInt(Mathf.Lerp(maximumDamage, minimumDamage, normalizedDistance));
            
            damageable.DealDamage(damage, originPosition);
            Destroy(gameObject);
        }

        Destroy(gameObject);
    }
}
