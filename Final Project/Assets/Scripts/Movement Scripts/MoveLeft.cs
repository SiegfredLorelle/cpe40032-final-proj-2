﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    // Attached to most objects in the scene including obstacles, powerups, and background

    public GameManager gameManagerScript;
    public GameObject meatPrefab;
    public bool isThrown;

    private float moveSpeed;
    private float modifiedMoveSpeed;

    // Variables used by some but not all objects
    public ParticleSystem explosionEffects;
    //public AudioSource audioExplosion;
    //public AudioSource audioCowHurt;
    public AudioSource audioSrc;


    void Start()
    {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        moveSpeed = 20f;
        modifiedMoveSpeed = moveSpeed;
        isThrown = false;
    }

    void Update()
    {
        // Increase or decrease speed base on player dash
        if (gameManagerScript.playerIsDashing)
        {
            SpeedUp();
        }
        else
        {
            SlowDown();
        }
    }

    // Increase or decrease speed (called on update method)
    void SpeedUp()
    {
        modifiedMoveSpeed = moveSpeed * 2f;
    }
    void SlowDown()
    {
        modifiedMoveSpeed = moveSpeed;
    }


    void FixedUpdate()
    {
        // Do nothing if the game is stopped or if the object is being thrown (by strength powerup)
        if (gameManagerScript.isGameStopped || isThrown)
        {
            return;
        }

        // Do actual movement, with modifiedMoveSpeed and relative to world (so that objects will always go left with respect to the world/camera regardless of their rotation)
        transform.Translate(Vector3.left * Time.fixedDeltaTime * modifiedMoveSpeed, relativeTo: Space.World);

        //// If object is not background and is out of bounds, then destroy it
        //if (!CompareTag(GameManager.TAG_BACKGROUND) && (transform.position.x < -10 || transform.position.y < -2 || transform.position.y > 10))
        //{
        //    //Destroy(gameObject);
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag(GameManager.TAG_OBSTACLE) && other.CompareTag(GameManager.TAG_PROJECTILE))
        {
            // If this object is a cow and is by a dagger
            if (other.name == GameManager.NAME_DAGGER)
            {
                if (gameManagerScript.NAME_COWS.Contains(gameObject.name))
                {
                    DaggerHitCow(other.gameObject);
                }
                else
                { 
                    DaggerHitObstacle(other.gameObject);
                }
            }

            else if (other.name == GameManager.NAME_BOMB)
            {
                BombExplosion(other.gameObject);
            }

        }

    }

    public void BombExplosion(GameObject bomb)
    {

        audioSrc = bomb.GetComponent<AudioSource>();
        // Turn of render so it appears destroyed, then actually destroy it after playing sfx
        bomb.GetComponent<Renderer>().enabled = false;
        // Turn off collider, to prevent it from affecting other objects while the explosion sfx is still playing
        bomb.GetComponent<BoxCollider>().enabled = false;

        // Add score
        gameManagerScript.IncreaseScore(1);

        // Create an explosion effects and destroy the obstacle hit
        Instantiate(explosionEffects, transform.position, Quaternion.identity);
        Destroy(gameObject);

        audioSrc.Play();
        Destroy(bomb, audioSrc.clip.length);
    }

    private void DaggerHitCow(GameObject dagger)
    {
        audioSrc = dagger.GetComponent<AudioSource>();
        Instantiate(meatPrefab, new Vector3(transform.position.x, transform.position.y + 3.0f, transform.position.z + 1.5f), meatPrefab.transform.rotation);
        audioSrc.Play();
        Destroy(gameObject);

        // Turn of render so it appears destroyed, then actually destroy it after playing sfx
        // Turn off collider, to prevent it from affecting other objects while the explosion sfx is still playing
        dagger.GetComponent<Renderer>().enabled = false;
        dagger.GetComponent<BoxCollider>().enabled = false;
        Destroy(dagger, audioSrc.clip.length);

        Instantiate(explosionEffects, transform.position, Quaternion.identity);
        gameManagerScript.IncreaseScore(2);

    }

    public void DaggerHitObstacle(GameObject dagger)
    {
        Destroy(gameObject);
        Destroy(dagger);
        Instantiate(explosionEffects, transform.position, Quaternion.identity);
        gameManagerScript.IncreaseScore(1);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(GameManager.TAG_DESTROYSENSOR))
        {
            Destroy(gameObject);
        }

        else if (CompareTag(GameManager.TAG_OBSTACLE))
        {
            if (other.CompareTag(GameManager.TAG_POINTSENSOR))
            {
                AddScoreBaseOnDash();
            }

        }
    }

    public void AddScoreBaseOnDash()
    {
        if (gameManagerScript.playerIsDashing)
        {
            gameManagerScript.IncreaseScore(2);
        }
        else
        {
            gameManagerScript.IncreaseScore(1);
        }
    }
}
