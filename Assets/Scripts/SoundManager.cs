using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static AudioClip[] FireSoundsPlayer = new AudioClip[2];
    public static AudioClip DamagesoundPlayer, Collectablesound, DamageSoundsEnemy, ElasticSound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        ElasticSound = Resources.Load<AudioClip>("ElasticPlayer");
        FireSoundsPlayer[0] = Resources.Load<AudioClip>("FirePlayer01");
        FireSoundsPlayer[1] = Resources.Load<AudioClip>("FirePlayer02");
        DamagesoundPlayer = Resources.Load<AudioClip>("PlayerHit");
        DamageSoundsEnemy = Resources.Load<AudioClip>("EnemyHit");
        Collectablesound = Resources.Load<AudioClip>("CaillouCollect");

        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "Fire":
                 int variationFP = UnityEngine.Random.Range(0, 2);
                   audioSrc.PlayOneShot(FireSoundsPlayer[variationFP]);
                break;
            case "PlayerHit":
                audioSrc.PlayOneShot(DamagesoundPlayer);
                break;
            case "EnemyHit":
                audioSrc.PlayOneShot(DamageSoundsEnemy);
                break;
            case "PickStone":
                audioSrc.PlayOneShot(Collectablesound);
                break;
        }
    }
}