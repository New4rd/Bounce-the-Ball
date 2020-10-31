using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

/// <summary>
/// A class to handle the interaction with the UI (buttons).
/// </summary>

public class UIInteraction : MonoBehaviour
{
    /// <summary>
    /// If the player touches the restart button... we restart the game!
    /// </summary>
    public void RestartButton ()
    {
        GameManager.Inst.RestartGame();
    }

    /// <summary>
    /// If the player touches the boost button, the game goes into the high-speed
    /// phase. The boost button also disappear.
    /// </summary>
    public void BoostButton ()
    {
        SceneManager.UnloadSceneAsync("Boost Button Scene", UnloadSceneOptions.None);
        GameManager.Inst.ResetBoostJaudge();
        StartCoroutine(GameManager.Inst.HighSpeedPhase());
    }


    public void RewindButton ()
    {
        StartCoroutine(_RewindButton());
    }


    /// <summary>
    /// If the player touches the rewind button, we launches the rewind phase.
    /// </summary>
    /// <returns></returns>
    public IEnumerator _RewindButton ()
    {
        if (RewindPlayer.Inst.rewindable)
        {
            GameManager.Inst.ResumeGame();
            RewindPlayer.Inst.Rewind();

            // playing the rewind sound
            AudioManager.Inst.PlaySound(AudioManager.SoundType.Rewind);

            // activating the rewind video effect
            Camera.main.GetComponent<VideoPlayer>().enabled = true;
            Camera.main.GetComponent<VHSPostProcessEffect>().enabled = true;

            yield return new WaitUntil(() => !RewindPlayer.Inst.rewind);

            // when the rewind is done, the game starts again and the video effect disappear
            GameManager.Inst.gameOver = false;
            Camera.main.GetComponent<VideoPlayer>().enabled = false;
            Camera.main.GetComponent<VHSPostProcessEffect>().enabled = false;

            AudioManager.Inst.PlayMusic();
            Automove.Inst.move = true;
        }
    }
}
