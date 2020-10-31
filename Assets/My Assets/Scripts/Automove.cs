using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class automatically handling the ball's movement, including moving forward
/// and sideway depending on the user's touch
/// </summary>

public class Automove : MonoBehaviour
{
    static private Automove _inst;
    static public Automove Inst
    {
        get { return _inst; }
    }


    /// <summary>
    /// The speed player's speed along the Z-axis
    /// </summary>
    [SerializeField] float speed;


    /// <summary>
    /// The main camera
    /// </summary>
    [SerializeField] Camera myCamera;

    public bool move = true;


    private void Awake()
    {
        _inst = this;
    }


    void Update()
    {

        // if the player is allowed to move by the Game Manager ...
        if (move)
        {
            // ... it's moving one step forward depending on the speed along the Z-axis
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.deltaTime);

            // if the player touches the screen ...
            if (Input.touchCount > 0)
            {
                // ... we apply a side transformation on position, depending on where the
                // player touched the screen
                transform.position = new Vector3(myCamera.ScreenToWorldPoint(
                    new Vector3(
                        Input.GetTouch(0).position.x,
                        Input.GetTouch(0).position.y,
                        5f)).x, transform.position.y, transform.position.z);
            }

            // if the player goes below the platforms, it's game over!
            if (transform.position.y < -.5f && !GameManager.Inst.gameOver)
            {
                Debug.Log("GAME OVER PHASE");
                GameManager.Inst.GameOverPhase();
            }
        }
    }
}
