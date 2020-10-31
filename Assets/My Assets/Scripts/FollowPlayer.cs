using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to allow any object to follow the player along the Z-axis.
/// Note: that script could be replaced with the use of Unity's Cinemachine,
/// but I don't use it yet.
/// </summary>

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject player;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z - 4f);
    }
}
