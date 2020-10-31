using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

/// <summary>
/// A class to handle the whole game management. It has to be included in
/// the main scene, loading and unloading scenes at will.
/// </summary>

public class GameManager : MonoBehaviour
{
    static private GameManager _inst;
    static public GameManager Inst
    {
        get { return _inst; }
    }


    /// <summary>
    /// The main camera
    /// </summary>
    [SerializeField] GameObject myCamera;

    /// <summary>
    /// Boolean describing if the game is in the game-over phase
    /// </summary>
    public bool gameOver = false;

    int boostJaudge = 0;        // counter for the boost jaudge
    int boostJaudgeLimit = 30;  // fill limit of the boost jaudge. When that value
                                // get reached, the player can use the boost

    bool boostButtonDisplayed = false;

    /// <summary>
    /// Boolean describing if the game is in the high-speed phase
    /// </summary>
    bool highSpeedPhase = false;

    private void Awake()
    {
        _inst = this;
    }


    private IEnumerator Start()
    {
        // displaying the first title screen for 5 seconds
        SceneManager.LoadSceneAsync("Title Scene", LoadSceneMode.Additive);
        yield return new WaitForSecondsRealtime(5);
        SceneManager.UnloadSceneAsync("Title Scene", UnloadSceneOptions.None);

        // displaying the second title screen for 5 seconds
        SceneManager.LoadSceneAsync("Title Scene 2", LoadSceneMode.Additive);
        yield return new WaitForSecondsRealtime(5);
        SceneManager.UnloadSceneAsync("Title Scene 2", UnloadSceneOptions.None);

        // loading the main game scene
        StartCoroutine(LoadGameScenes());
    }


    /// <summary>
    /// Function to update the boost jaudge status. If the jaudge isn't filled,
    /// it's increased by 1. If it is, it doesn't increase anymore and we display
    /// the button to activate the boost.
    /// </summary>
    public void UpdateBoostJaudge ()
    {
       if (boostJaudge < boostJaudgeLimit && !highSpeedPhase)
       {
            boostJaudge++;
       }

       if (boostJaudge == boostJaudgeLimit && !boostButtonDisplayed)
       {
            SceneManager.LoadSceneAsync("Boost Button Scene", LoadSceneMode.Additive);
            boostButtonDisplayed = true;
            ResetBoostJaudge();
       }
    }


    public void ResetBoostJaudge ()
    {
        boostJaudge = 0;
    }


    /// <summary>
    /// Function the load the main game scene and the UI scene (score)
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadGameScenes ()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("Game Scene", LoadSceneMode.Additive);
        yield return new WaitUntil(() => op.isDone);
        op = SceneManager.LoadSceneAsync("UI Scene", LoadSceneMode.Additive);
        yield return new WaitUntil(() => op.isDone);
        myCamera = GameObject.FindWithTag("MainCamera");
    }


    /// <summary>
    /// Launching the game-over phase. The player can't move anymore, we play
    /// the fail sound, and the "GAME OVER" message gets displayed.
    /// </summary>
    public void GameOverPhase ()
    {
        ModifyGameSpeed(1);
        Automove.Inst.move = false;
        gameOver = true;
        AudioManager.Inst.PauseMusic();
        AudioManager.Inst.PlaySound(AudioManager.SoundType.Fail);
        SceneManager.LoadSceneAsync("Gameover Scene", LoadSceneMode.Additive);
        PauseGame();
    }


    /// <summary>
    /// Launching the high-speed phase. To do so, we're increasing the whole game
    /// speed for a limited amount of time.
    /// </summary>
    /// <returns></returns>
    public IEnumerator HighSpeedPhase ()
    {
        Debug.Log("High speed phase");
        highSpeedPhase = true;
        myCamera.GetComponent<PostProcessVolume>().enabled = true;/*
        ModifyGameSpeed(.5f);
        AudioManager.Inst.PlaySound(AudioManager.SoundType.Speed);
        Debug.Log("HIGH");
        yield return new WaitForSecondsRealtime(1);*/
        Debug.Log("HIGH2");
        ModifyGameSpeed(1.5f);
        yield return new WaitForSecondsRealtime(1);
        myCamera.GetComponent<PostProcessVolume>().enabled = false;
        ModifyGameSpeed(1);
        highSpeedPhase = false;
    }


    /// <summary>
    /// Function the restart the whole game by unloading-reloading the scenes.
    /// </summary>
    public void RestartGame ()
    {
        SceneManager.UnloadSceneAsync("Game Scene", UnloadSceneOptions.None);
        SceneManager.UnloadSceneAsync("UI Scene", UnloadSceneOptions.None);

        if (gameOver)
        {
            SceneManager.UnloadSceneAsync("Gameover Scene", UnloadSceneOptions.None);
        }

        gameOver = false;
        StartCoroutine(LoadGameScenes());
        ResetBoostJaudge();
        ResumeGame();
    }


    public void PauseGame ()
    {
        ModifyGameSpeed(0);
    }


    public void ResumeGame ()
    {
        ModifyGameSpeed(1);
    }


    public void ModifyGameSpeed(float speed)
    {
        Time.timeScale = speed;
    }
}
