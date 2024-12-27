using TMPro;
using UnityEngine;

public class GameOverCheck : MonoBehaviour
{
    public TextMeshProUGUI _projectileUI; 
    public int totalProjectiles = 5;    

    private int currentProjectiles;      

    public GameObject gameOverUI;
    private void Start()
    {
        currentProjectiles = totalProjectiles;
        UpdateProjectileUI();
    }

 
    public void FireProjectile()
    {
        if (currentProjectiles > 0)
        {
            currentProjectiles--; 
            UpdateProjectileUI(); 
        }
        else
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0f; 
        }
    }
    

    // UI 업데이트 메서드
    private void UpdateProjectileUI()
    {
        _projectileUI.text = $"Bullet: {currentProjectiles}";
    }
}