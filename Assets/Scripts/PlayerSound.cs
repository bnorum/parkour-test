using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
   
    public PlayerMovement playerMovement;
    [Header("Walking")]
    public AudioSource walkSource;
    public AudioClip[] walkSources;
    public AudioSource jumpSource;
    public AudioClip[] jumpSources;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
       if ((playerMovement.getVelX() != 0f || playerMovement.getVelZ() != 0f) && playerMovement.getIsGrounded()) {
            if (!walkSource.isPlaying) {
                walkSource.clip = walkSources[Random.Range(0, walkSources.Length)];
                if (playerMovement.getIsDashing()) {
                    walkSource.Stop();
                    walkSource.pitch = 3.5f;
                    walkSource.Play();
                } else {
                    walkSource.pitch = 1.5f;
                }
                walkSource.Play();
            }
        } 

    }
    public void PlayJumpSound(bool isDouble) {
        if (!isDouble) jumpSource.clip = jumpSources[0];
        else jumpSource.clip = jumpSources[1];
        jumpSource.Play();
    }
}
