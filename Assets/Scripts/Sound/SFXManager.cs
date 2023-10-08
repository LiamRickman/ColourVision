using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    //Variables for the Sounds
    public static AudioClip playerAtkSound, playerHurtSound, playerPickupSound, playerLoseSound, 
        BossAtkSound, BossHurtSound, BossDeathSound, BossProjectile1Sound, BossProjectile2Sound, BossProjectile3Sound,
        MenemyDeathSound, MenemyHurtSound, MenemyMoveSound, RenemyDeathSound, RenemyHurtSound, RenemyAtkSound,
        FenemyDeathSound, FenemyHurtSound, FenemyAtkSound;
    static AudioSource audioScr;
    // Start is called before the first frame update
    void Start()
    {
        //loads each specific sound from the resourses folder in the engine 
        playerAtkSound = Resources.Load<AudioClip>("PlayerAttack2");
        playerHurtSound = Resources.Load<AudioClip>("PlayerHurt1");
        playerPickupSound = Resources.Load<AudioClip>("PlayerPickup");
        playerLoseSound = Resources.Load<AudioClip>("PlayerLose");

        BossAtkSound = Resources.Load<AudioClip>("BossAttack1");
        BossHurtSound = Resources.Load<AudioClip>("BossHurt1");
        BossDeathSound = Resources.Load<AudioClip>("BossDeath1");
        BossProjectile1Sound = Resources.Load<AudioClip>("BossProjectile");
        BossProjectile2Sound = Resources.Load<AudioClip>("BossProjectile2");
        BossProjectile3Sound = Resources.Load<AudioClip>("BossProjectile3");

        MenemyDeathSound = Resources.Load<AudioClip>("RollieDeath");
        MenemyHurtSound = Resources.Load<AudioClip>("RollieHurt");
        MenemyMoveSound = Resources.Load<AudioClip>("RollieMovement");
        RenemyDeathSound = Resources.Load<AudioClip>("ThrowieDeath");
        RenemyHurtSound = Resources.Load<AudioClip>("ThrowieHurt");
        RenemyAtkSound = Resources.Load<AudioClip>("ThrowieAttack");
        FenemyDeathSound = Resources.Load<AudioClip>("FlyingDeath");
        FenemyHurtSound = Resources.Load<AudioClip>("FlyingHurt");
        FenemyAtkSound = Resources.Load<AudioClip>("FlyingAttack");

        audioScr = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip)
    {
        //decides which audio clip to play when called by name in other scripts
        switch (clip)
        {
            case "PlayerAttack2":
                audioScr.PlayOneShot(playerAtkSound);
                break;
            case "PlayerHur1":
                audioScr.PlayOneShot(playerHurtSound);
                break;
            case "PlayerPickup":
                audioScr.PlayOneShot(playerPickupSound);
                break;
            case "PlayerLose":
                audioScr.PlayOneShot(playerLoseSound);
                break;

            case "BossAttack1":
                audioScr.PlayOneShot(BossAtkSound);
                break;
            case "BossHurt1":
                audioScr.PlayOneShot(BossHurtSound);
                break;
            case "BossDeath1":
                audioScr.PlayOneShot(BossDeathSound);
                break;
            case "BossProjectile":
                audioScr.PlayOneShot(BossProjectile1Sound);
                break;
            case "BossProjectile2":
                audioScr.PlayOneShot(BossProjectile2Sound);
                break;
            case "BossProjectile3":
                audioScr.PlayOneShot(BossProjectile3Sound);
                break;

            case "RollieDeath":
                audioScr.PlayOneShot(MenemyDeathSound);
                break;
            case "RollieHurt":
                audioScr.PlayOneShot(MenemyHurtSound);
                break;
            case "RollieMovement":
                audioScr.PlayOneShot(MenemyMoveSound);
                break;
            case "ThrowieDeath":
                audioScr.PlayOneShot(RenemyDeathSound);
                break;
            case "ThrowieHurt":
                audioScr.PlayOneShot(RenemyHurtSound);
                break;
            case "ThrowieAttack":
                audioScr.PlayOneShot(RenemyAtkSound);
                break;
            case "FlyingDeath":
                audioScr.PlayOneShot(FenemyDeathSound);
                break;
            case "FlyingHurt":
                audioScr.PlayOneShot(FenemyHurtSound);
                break;
            case "FlyingAttack":
                audioScr.PlayOneShot(FenemyAtkSound);
                break;
        }
    }
}
