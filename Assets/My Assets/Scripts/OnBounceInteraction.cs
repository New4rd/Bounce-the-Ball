using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A classe handling the game's behavior when the player bounces on an object.
/// That script has to be placed on the object.
/// </summary>

public class OnBounceInteraction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            // if it's a spike, game over!
            if (transform.name == "Spike")
            {
                GameManager.Inst.GameOverPhase();
                return;
            }

            // if it's a prisma, bonus points! (+5)
            if (transform.name == "Prisma")
            {
                ScoreManager.Inst.UpdateScore(5);
                AudioManager.Inst.PlaySound(AudioManager.SoundType.Bonus);
                gameObject.SetActive(false);
                return;
            }

            // if it's a growable, all of the currently spawned platforms grow
            if (transform.name == "Growable zone")
            {
                PlatformGenerator.Inst.GrowPlaforms(new Vector3(.1f, 0, .1f));
            }
        }
    }
}
