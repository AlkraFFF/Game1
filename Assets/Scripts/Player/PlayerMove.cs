using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMove : NetworkBehaviour
{
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float idleSpeed = 0f;

    private float speed;

    private Rigidbody2D rb;
    private Vector2 aimVector;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform hostPlayer;
    private PlayerStats playerStats;

    public NetworkVariable<Vector2> LastMovedVector => lastMovedVector;
    private NetworkVariable<Vector2> lastMovedVector = new NetworkVariable<Vector2>(
        new Vector2(0, -1),
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );
    
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsOwner)
        {
            return;
        }

        PlayerCamera.Instance.SetToPlayer(transform);

        if (IsHost)
        {
            return;
        }

        SetHostPlayerServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetHostPlayerServerRpc()
    {
        SetHostPlayerClientRpc(NetworkManager.Singleton.ConnectedClients[0].PlayerObject);
    }
    [ClientRpc]
    public void SetHostPlayerClientRpc(NetworkObjectReference hostNetworkObj)
    {
        if (!hostNetworkObj.TryGet(out NetworkObject player))
        {
            return;
        }

        hostPlayer = player.transform;
    }
    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }

        PlayerCamera.Instance.SetToNull();
    }
    private void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerStats = GetComponent<PlayerStats>();
    }
    
    void Update()
    {
        if (!IsOwner || !Application.isFocused)
            return;

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        aimVector = new Vector2(hor, ver).normalized;

        if (aimVector != Vector2.zero)
        {
            lastMovedVector.Value = aimVector;

            animator.SetBool("Walk", true);
            animator.SetFloat("X", hor);
            animator.SetFloat("Y", ver);
            animator.SetFloat("speed", walkSpeed);
            speed = walkSpeed;
        }
        else
        {
            animator.SetBool("Walk", false);
            animator.SetFloat("speed", idleSpeed);
            speed = 0f;
        }

        if (hor > 0)
        {
            ChangeSpriteFlipServerRpc(false);
        }
        else if (hor < 0)
        {
            ChangeSpriteFlipServerRpc(true);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
            animator.SetFloat("speed", runSpeed);
        }
    }

    [ServerRpc]
    private void ChangeSpriteFlipServerRpc(bool needToFlip)
    {
        spriteRenderer.flipX = needToFlip;
        ChangeSpriteFlipClientRpc(needToFlip);
    }

    [ClientRpc]
    private void ChangeSpriteFlipClientRpc(bool needToFlip)
    {
        spriteRenderer.flipX = needToFlip;
    }
    private void FixedUpdate()
    {
        rb.velocity = aimVector * playerStats.CurSpeed;
        ClampPos();
    }
    private void ClampPos()
    {
        if (IsHost|| !IsOwner || !hostPlayer)
        {
            return;
        }

        Vector3 directionToHost = hostPlayer.position - transform.position;

        if (directionToHost.magnitude > 5f)
        {
            transform.position = Vector3.Lerp(transform.position,
                hostPlayer.position - directionToHost.normalized * 5f, 5f);
        }
    }
}
