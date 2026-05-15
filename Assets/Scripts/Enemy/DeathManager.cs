using UnityEngine;
using TMPro;
using System.Collections;

public class DeathManager : MonoBehaviour
{
    public static DeathManager Instance;

    [Header("References")]
    public Transform player;
    public EnemyAI enemy;
    public TMP_Text youDiedText;

    private Vector3 playerSpawnPoint;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerSpawnPoint = player.position;
        youDiedText.gameObject.SetActive(false);
    }

    public void TriggerDeath()
    {
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        youDiedText.gameObject.SetActive(true);

        CharacterController cc = player.GetComponent<CharacterController>();
        PlayerController pc = player.GetComponent<PlayerController>();

        pc.enabled = false;
        cc.enabled = false;

        yield return new WaitForSeconds(3f);

        // Teleport player
        player.position = playerSpawnPoint;

        // Reset enemy
        enemy.ResetEnemy();

        cc.enabled = true;
        pc.enabled = true;

        youDiedText.gameObject.SetActive(false);
    }
}