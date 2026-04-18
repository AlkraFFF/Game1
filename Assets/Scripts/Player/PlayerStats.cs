using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] private PlayerStatsSO stats;
    [SerializeField] private Image hpBar;
    [SerializeField] private float invincibilityDur;

    private float invincibilityTimer;
    private bool isInvincible;

    private NetworkVariable<float> curMaxHealth = new NetworkVariable<float>();
    private NetworkVariable<float> curHealth = new NetworkVariable<float>();
    private float curRecovery;

    public float CurSpeed => curSpeed;
    private float curSpeed;

    public float CurMagnetRadius => curMagnetRadius;
    private float curMagnetRadius;

    private void Start()
    {
        curSpeed = stats.MaxSpeed;
        curMagnetRadius = stats.MagnetRadius;

        if (!IsServer)
            return;

        curMaxHealth.Value = stats.MaxHealth;
        curHealth.Value = curMaxHealth.Value;
        curRecovery = stats.Recovery;
    }

    public override void OnNetworkSpawn()
    {
        curHealth.OnValueChanged += OnHealthValueChanged;
        curMaxHealth.OnValueChanged += OnHealthValueChanged;
    }
    private void OnHealthValueChanged(float previousValue, float newValue)
    {
        hpBar.fillAmount = curHealth.Value / curMaxHealth.Value;
    }

    private void Update()
    {
       if(!IsServer)
            return;

        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }

        RecoverHealth();
    }

    private void RecoverHealth()
    {
        if (curHealth.Value < curMaxHealth.Value)
        {
            Heal(curRecovery * Time.deltaTime);
        }
    }

    private void Heal(float healthToRestore)
    {
        if(curHealth.Value == curMaxHealth.Value)
        {
            return;
        }

        curHealth.Value += healthToRestore;
        if (curHealth.Value > curMaxHealth.Value)
        {
            curHealth.Value = curMaxHealth.Value;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(!IsServer)
        {
            return;
        }

        if (collision.gameObject.TryGetComponent(out EnemyStats enemyStats))
        {
            TakeDamage(enemyStats.CurDamage);
        }
    }

    private void TakeDamage(float dmg)
    {
        if (isInvincible)
        {
            return;
        }

        invincibilityTimer = invincibilityDur;
        isInvincible = true;

        curHealth.Value -= dmg;
        if (curHealth.Value <= 0)
            {
                curHealth.Value = 0;
                Die();
        }
    }

    private void Die()
    {
        PlayerCamera.Instance.SetToNull();
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [ServerRpc]
    public void HealServerRpc(float healthToRestore)
    {
        Heal(healthToRestore);
    }
}
