using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCheck : MonoBehaviour
{
    public GameObject gameclearUI;
    private bool isMonster = false; 
    
    public void OnAllEnemiesCleared()
    {
        AudioManager.Instance.PlaySound(6); 
        if (gameclearUI != null)
        {
            gameclearUI.SetActive(true);
        }
        else
        {
            Debug.LogError("GameClear UI is not assigned!");
        }
    }


    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0 && !isMonster)
        {
            isMonster = true; 
            OnAllEnemiesCleared();    
        }
    }
}