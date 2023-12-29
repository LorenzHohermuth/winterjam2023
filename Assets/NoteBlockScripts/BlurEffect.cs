using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BlurEffect : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    private DepthOfField depthOfField;

    void Start()
    {
        if (postProcessVolume)
            postProcessVolume.profile.TryGetSettings(out depthOfField);
    }

    public void SetBlur(bool enabled)
    {
        if (depthOfField != null)
            depthOfField.enabled.value = enabled;
    }
}
