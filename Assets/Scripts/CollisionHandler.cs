using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    public float reLoadTime = 1;
    public AudioClip audioCrash;
    public AudioClip audioSuccess;
    public ParticleSystem particleCrash;
    public ParticleSystem particleSuccess;

    AudioSource audioSource;

    public bool isTransitioning = false;
    public bool collisionDisabled = false;


    private void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    private void Update(){
        DebugKeys();
    }

    private void DebugKeys(){
        if(Input.GetKeyDown(KeyCode.L)){
            LoadNewLevel();
        }else if(Input.GetKeyDown(KeyCode.C)){
            collisionDisabled = !collisionDisabled; //This will toggle colision
        }
    }

    private void OnCollisionEnter(Collision other) {

        if( isTransitioning || collisionDisabled){
            return;
        }

        switch(other.gameObject.tag){
             case "Friendly":
                Debug.Log("You have hit an friendly object");
             break;
             case "Finish":
                StartSuccessSequence();
             break;
             default:
                StartCrashSequence();
             break;
        }
    }

    private void StartSuccessSequence()
    {
        // todo add SFX on success
        // todo add Particle effect on success
        particleSuccess.Play();
        isTransitioning = true;
        if(audioSource.isPlaying){
            audioSource.Stop();
            audioSource.PlayOneShot(audioSuccess);
        }else{
            audioSource.PlayOneShot(audioSuccess);
        }
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNewLevel",reLoadTime);
    }

    void StartCrashSequence()
    {
        // todo add SFX on crash
        // todo add Particle effect on crash
        particleCrash.Play();
        isTransitioning = true;

        if(audioSource.isPlaying){
            audioSource.Stop();
            audioSource.PlayOneShot(audioCrash);
        }else{
            audioSource.PlayOneShot(audioCrash);
        }

        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel",reLoadTime);
    }

    void ReloadLevel(){
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNewLevel(){
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings){
            SceneManager.LoadScene(0);
        }else{
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
