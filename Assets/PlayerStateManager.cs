using System;
using System.Security.Cryptography;
using UnityEngine;
public class PlayerStateManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody2D rb;
    public enum PlayerState
    {
        Land,
        Water
    }
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        rb.gravityScale = currentPlayerState == PlayerState.Water ? 0.1f : 1f;
    }
    public static PlayerState currentPlayerState = PlayerState.Land;
}
