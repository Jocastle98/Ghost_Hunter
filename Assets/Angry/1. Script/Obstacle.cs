using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float hp = 18f;                
    [SerializeField] private Material normalMaterial;       
    [SerializeField] private Material badMaterial; 
    private MeshRenderer _renderer;                  
    private Rigidbody rb;
    
    //이펙트 설정
    [SerializeField] private GameObject particleEffect;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _renderer = GetComponent<MeshRenderer>();
        _renderer.material = normalMaterial;

        
    }

    private void OnCollisionEnter(Collision collision)
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
        
        //오디오 충돌 사운드
        AudioManager.Instance.PlaySound(2); 
        
        
      
        UpdateMaterial();

 
        if (hp <= 0)
        {
            AudioManager.Instance.PlaySound(4); 
            _renderer.material = badMaterial; 
            particleEffect.SetActive(true); 
            Destroy(gameObject, 0.5f); 
            
        }
    }

    private void UpdateMaterial()
    {
        if (_renderer == null) return;

        if (hp > 10)
        {
            _renderer.material = normalMaterial; 
        }
        else if (hp > 0 && hp <= 10)
        {
            _renderer.material = badMaterial; 
        }

    }
    
}