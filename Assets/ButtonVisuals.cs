using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonVisuals : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AudioClip hoverSound;
    private AudioSource audioSource;

    private Vector3 originalScale;
    private Vector3 enlargedScale;
    private bool isHovering = false;
    private float animationSpeed = 5.0f;

    private void Start()
    {
        originalScale = transform.localScale;
        enlargedScale = originalScale * 1.1f; 

        if (GetComponent<AudioSource>() != null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (isHovering)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, enlargedScale, Time.deltaTime * animationSpeed);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * animationSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        isHovering = false;
    }
}
