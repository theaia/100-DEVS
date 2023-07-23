using UnityEngine;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour {

    [SerializeField] private float speed = 15f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float maxLifetime = 5f;

    [SerializeField] private ParticleSystem trailFX;
    [SerializeField] private GameObject ImpactFX;

    private Rigidbody rb;

    private float lifetime;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        AudioSource source = GetComponent<AudioSource>();
        if(source != null) source.pitch = Random.Range(.9f, 1.1f);
    }

    private void OnTriggerEnter(Collider other) {
        //Debug.Log($"{gameObject.name} hit {other.gameObject.name}");
        if (ImpactFX != null) {
            Instantiate(ImpactFX, transform.position, transform.rotation);
        }
        
        IDamageable damagedObject = other.GetComponent<IDamageable>();
        if (damagedObject != null) {
            damagedObject.Damage(damage);
        }
        
        Destroy(gameObject); //This would be better with a pooling system
    }

    private void OnTriggerExit(Collider other) {
        OnTriggerEnter(other);
    }

    void FixedUpdate(){
        if(rb) rb.velocity = transform.forward * speed;
        lifetime += Time.deltaTime;
        if(lifetime >= maxLifetime){
            Destroy(gameObject);
        }
    }
}