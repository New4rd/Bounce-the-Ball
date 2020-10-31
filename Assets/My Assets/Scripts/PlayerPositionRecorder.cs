using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to record the last player's movements. That class will be used
/// for the feature to rewind the movements.
/// </summary>

public class PlayerPositionRecorder : MonoBehaviour
{
    static private PlayerPositionRecorder _inst;
    static public PlayerPositionRecorder Inst
    {
        get { return _inst; }
    }

    [SerializeField] GameObject player;

    /// <summary>
    /// The size of the cache for the player's movements.
    /// </summary>
    [SerializeField] int recordLimit;

    /// <summary>
    /// The cache, where all of the positions are stored as Vector3 variables.
    /// </summary>
    public List<Vector3> positions;

    /// <summary>
    /// The velocities of the player.
    /// </summary>
    List<Vector3> velocities;

    private void Awake()
    {
        _inst = this;
        positions = new List<Vector3>();
        velocities = new List<Vector3>();
    }


    private void Update()
    {
        // if the game is running (no game-over) ...
        if (!GameManager.Inst.gameOver)
        {
            // We add the player's position the cache
            if (positions.Count < recordLimit)
            {
                positions.Add(player.transform.position);
                velocities.Add(player.GetComponent<Rigidbody>().velocity);
            }

            // if the cache size is larger than its virtual limit, we're
            // removing the oldest added element
            else
            {
                positions.RemoveAt(0);
                velocities.RemoveAt(0);
            }
        }
    }

    public List<Vector3> GetPositions ()
    {
        return positions;
    }

    public List<Vector3> GetVelocities ()
    {
        return velocities;
    }
}
