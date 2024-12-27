using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; }
    
    private AudioSource introSource; 
    private AudioSource bgmSource;   

    public AudioClip[] audioClip; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Start()
    {
        introSource = GetComponent<AudioSource>();
        bgmSource = GetComponent<AudioSource>();

        IntroBGM();
    }


    public void IntroBGM()
    {
        introSource.clip = audioClip[0]; 
        introSource.volume = 0.8f;
        introSource.loop = true; 
        introSource.Play(); 
    }
    
    public void MainBGM()
    {
        bgmSource.clip = audioClip[1]; 
        bgmSource.loop = true; 
        bgmSource.Play(); 
    }
    
    public void PlaySound(int index)
    {
        if (index >= 2 && index < audioClip.Length && audioClip[index] != null)
        {
            bgmSource.PlayOneShot(audioClip[index]); 
        }
    }
}