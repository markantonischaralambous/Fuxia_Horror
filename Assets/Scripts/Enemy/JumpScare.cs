// JumpScare.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JumpScare : MonoBehaviour
{
    public CameraShake cameraShake;
    public RawImage flashImage;

    void Update()
    {
        // Press J to test the jumpscare
        if (Input.GetKeyDown(KeyCode.J))
        {
            PlayJumpScare();
        }
    }

    public void PlayJumpScare()
    {
        StartCoroutine(JumpScareEffect());
    }

    IEnumerator JumpScareEffect()
    {
        // Show flash image
        flashImage.enabled = true;

        // Shake for 2 seconds
        yield return StartCoroutine(cameraShake.Shake(2f, 0.05f));

        // Hide flash image
        flashImage.enabled = false;
    }
}