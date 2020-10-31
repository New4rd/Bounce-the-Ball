using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to make the player bounce on platforms.
/// To do so, we're detecting the interaction between the player's collider sphere
/// and the platform's collider box. If there is a collision, we apply a force on
/// the player along the Y-axis to give the illusion of bounce.
///
/// When a player touches a platform, it also makes a previous one disappear, since
/// it's not visible to the player anymore.
/// </summary>

public class Bounce : MonoBehaviour
{
    [SerializeField] float bounceForce;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Inst.PlaySound(AudioManager.SoundType.Bounce);
        GameManager.Inst.UpdateBoostJaudge();
        ScoreManager.Inst.UpdateScore(1);
        rb.velocity = new Vector3(0,0,0);
        rb.AddForce(new Vector3(0, bounceForce, 0), ForceMode.Impulse);
        PlatformGenerator.Inst.SpawnPlatform();

        if (PlatformGenerator.Inst.platformsAmount() > 20)
        {
            PlatformGenerator.Inst.DestroyPlatform();
        }
    }
}
