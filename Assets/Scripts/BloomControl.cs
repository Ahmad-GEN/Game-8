using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BloomControl : MonoBehaviour
{
    public Volume volume;
    private Bloom thisBloom;
    public PotaTween bloomTween;
    public bool pauseBloom = false;
    public static BloomControl Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        VolumeProfile proflile = volume.sharedProfile;
        volume.profile.TryGet(out thisBloom);
        
    }

    private void Update()
    {
        if (!pauseBloom)
        {
            thisBloom.intensity.value = bloomTween.Float.Value;
        }
    }

    public void OnHoverEnter()
    {
        pauseBloom = true;
        thisBloom.intensity.value = 1.5f;
    }
    public void OnHoverExit()
    {
        pauseBloom = false;
    }
}
