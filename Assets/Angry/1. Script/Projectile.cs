using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Camera _followCam;  // 발사체의 자식 카메라
    public Camera _mainCam;    // 메인 카메라
    
    //스킬 이펙트
    [SerializeField] private GameObject _bombEffect;



  
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.PlaySound(7); 
            _bombEffect.SetActive(true);
            _followCam.gameObject.SetActive(false); 
            _mainCam.enabled = true;               
            Destroy(gameObject, 0.2f);
        }
        
    }

    public void Launch()
    {
     
        if (_followCam != null && _mainCam != null)
        {
            _mainCam.enabled = false;          
            _followCam.gameObject.SetActive(true); 
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_followCam != null && _mainCam != null)
        {
            _followCam.gameObject.SetActive(false); 
            _mainCam.enabled = true;               
        }
    }
}