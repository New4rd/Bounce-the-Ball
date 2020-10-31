using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A class to handle the rewind functionnality.
/// </summary>

public class RewindPlayer : MonoBehaviour
{
    static private RewindPlayer _inst;
    static public RewindPlayer Inst
    {
        get { return _inst; }
    }

    /// <summary>
    /// A boolean to indicate if the game is actually rewinding or not.
    /// </summary>
    public bool rewind = false;

    /// <summary>
    /// A boolean to indicate if the player already used the rewind functionnality.
    /// If he uses it once, he won't be able to use it anymore.
    /// </summary>
    public bool rewindable = true;

    /// <summary>
    /// Reference to the player's component Automove
    /// </summary>
    Automove automove;

    /// <summary>
    /// Reference to the player's position recorder
    /// </summary>
    PlayerPositionRecorder positionsRecorder;
    GameObject player;

    int rewindPos = 0;
    List<Vector3> reversedPositions;

    private void Awake()
    {
        _inst = this;
        player = GameObject.FindWithTag("Player");
        automove = player.GetComponent<Automove>();
        positionsRecorder = GameObject.FindWithTag("Position Recorder").GetComponent<PlayerPositionRecorder>();
    }


    private void Update()
    {
        // if we're in the rewind phase ...
        if (rewind && rewindable)
        {
            // ... we're moving the player along the last reversed positions
            if (rewindPos < reversedPositions.Count)
            {
                player.transform.position = reversedPositions[rewindPos];
                Debug.Log(player.transform.position);
                rewindPos++;
            }

            // if the last rewinded position is reached, we leave the rewind phase
            if (rewindPos == reversedPositions.Count)
            {
                player.GetComponent<Rigidbody>().velocity = positionsRecorder.GetVelocities()[0];
                player.GetComponent<SphereCollider>().enabled = true;
                rewind = false;
                rewindable = false;
                rewindPos = 0;
                SceneManager.UnloadSceneAsync("Gameover Scene", UnloadSceneOptions.None);
            }
        }
    }


    /// <summary>
    /// Function the start the rewind functionnality.
    /// </summary>
    public void Rewind ()
    {
        Debug.Log("REWINDING!");

        // We're desactivating the player's sphere collider to disable any possible
        // with a platform and disallow its possible effects
        player.GetComponent<SphereCollider>().enabled = false;

        // We're getting the list of the last player positions, and reverting it
        reversedPositions = positionsRecorder.GetPositions();
        reversedPositions.Reverse();
        rewind = true;
    }
}
