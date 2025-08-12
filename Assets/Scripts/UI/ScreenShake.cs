using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float defaultShakeDuration = 0.5f;
    [SerializeField] private float defaultShakeMagnitude = 0.3f;
    [SerializeField] private float dampingSpeed = 1.0f;

    private Vector3 initialPosition;
    private Coroutine shakeCoroutine;

    private void OnEnable()
    {
        initialPosition = transform.localPosition;
        GameEvents.OnCaughtPenalty += OnCaughtPenaltyHandler;
    }

    private void OnDisable()
    {
        GameEvents.OnCaughtPenalty -= OnCaughtPenaltyHandler;
    }

    private void OnCaughtPenaltyHandler()
    {
        TriggerShake(defaultShakeDuration, defaultShakeMagnitude);
    }

 
    public void TriggerShake(float duration, float magnitude)
    {
        // If there's already a shake happening, stop it first
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Calculate percentage through the shake duration
            float percentComplete = elapsed / duration;
            // Reduce magnitude over time for damping effect
            float currentMagnitude = magnitude * (1f - percentComplete);

            transform.localPosition = initialPosition + Random.insideUnitSphere * currentMagnitude;

            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Ensure we return to the exact initial position
        transform.localPosition = initialPosition;
        shakeCoroutine = null;
    }
}