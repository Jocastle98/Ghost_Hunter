using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float hp = 18f;                
    [SerializeField] private Material normalMaterial;      
    [SerializeField] private Material badMaterial; 
    private SkinnedMeshRenderer _renderer;                  
    private Rigidbody rb;

    //이펙트 
    [SerializeField] private GameObject particleEffect;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _renderer.material = normalMaterial; 
        
    }

    void OnCollisionEnter(Collision collision)
    {
        float impactForce;
        if (collision.rigidbody != null)
        {
            impactForce = collision.relativeVelocity.magnitude * collision.rigidbody.mass;
        }
        else
        {
            impactForce = collision.relativeVelocity.magnitude * 1f; 
        }
        hp -= impactForce;

      
        UpdateMaterial();
        if (hp <= 0)
        {
           
            AudioManager.Instance.PlaySound(3); 
            _renderer.material = badMaterial; 
            particleEffect.SetActive(true);
            Destroy(gameObject,0.8f);
        
        }
    }

    private void UpdateMaterial()
    {
        if (_renderer == null) return;
        if (hp > 0 && hp <= 10)
        {
            _renderer.material = badMaterial;
        }
        else if(hp>10)
        {
            _renderer.material = normalMaterial;
        }
    }
}
