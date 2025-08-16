using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonScaleAnimation : MonoBehaviour
{
    [Tooltip("The transform to scale. Defaults to this object's transform if not set.")]
    public Transform targetTransform;

    [Tooltip("How much to scale the button down on press.")]
    public float scaleDownFactor = 0.95f;

    [Tooltip("How long the scale animation takes in seconds.")]
    public float animationDuration = 0.1f;

    private Vector3 originalScale;
    private Coroutine scaleCoroutine;

    private void Awake()
    {
        if (targetTransform == null)
        {
            targetTransform = transform;
        }
        originalScale = targetTransform.localScale;
    }

    /// <summary>
    /// Starts the animation to shrink the button.
    /// </summary>
    public void StartPressAnimation()
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }
        Vector3 targetScale = originalScale * scaleDownFactor;
        scaleCoroutine = StartCoroutine(AnimateScale(targetTransform.localScale, targetScale));
    }

    /// <summary>
    /// Starts the animation to return the button to its original size.
    /// </summary>
    public void StartReleaseAnimation()
    {
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }
        scaleCoroutine = StartCoroutine(AnimateScale(targetTransform.localScale, originalScale));
    }

    private IEnumerator AnimateScale(Vector3 startScale, Vector3 endScale)
    {
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            // Use unscaled time to ensure animation plays even if Time.timeScale is 0
            elapsedTime += Time.unscaledDeltaTime;
            float progress = Mathf.Clamp01(elapsedTime / animationDuration);
            targetTransform.localScale = Vector3.Lerp(startScale, endScale, progress);
            yield return null;
        }
        targetTransform.localScale = endScale;
    }
}