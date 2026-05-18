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
    public bool isDead = false;
    public JumpScare jumpScare;

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
        if (isDead) return;
        isDead = true;
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        jumpScare.PlayJumpScare();
        yield return new WaitForSeconds(2f); // wait for jumpscare to finish
        youDiedText.gameObject.SetActive(true);

        CharacterController cc = player.GetComponent<CharacterController>();
        PlayerController pc = player.GetComponent<PlayerController>();

        pc.enabled = false;
        cc.enabled = false;

        yield return new WaitForSeconds(3f);

        player.position = playerSpawnPoint;

        yield return StartCoroutine(enemy.ResetCoroutine());

        isDead = false;

        cc.enabled = true;
        pc.enabled = true;

        youDiedText.gameObject.SetActive(false);
    }
}