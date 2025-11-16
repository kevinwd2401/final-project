using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
    private Rigidbody rb;

    private int damage;
    private float lifespan, totalLife;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        lifespan -= Time.deltaTime;
        if (lifespan < 0) Destruction();
    }

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.tag == "Water" || totalLife - lifespan < 0.5f) return;
        if (c.gameObject.TryGetComponent<IDamagable>(out IDamagable d)) {
            d.InflictDamage(damage, this.transform);
        }

        Destruction();
    }

    private void Destruction() {
        Destroy(gameObject);
    }

    public void SetValues(int dmg, float force, float lifespan) {
        rb = GetComponent<Rigidbody>();
        rb.velocity = force * transform.forward;
        damage = dmg;
        this.lifespan = lifespan + Random.value;
        totalLife = this.lifespan;
    }
}
