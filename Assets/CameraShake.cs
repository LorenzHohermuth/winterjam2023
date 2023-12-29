using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    float frequency = 23;

    [SerializeField]
    Vector3 maximumAngularShake = Vector3.one * 2;

    [SerializeField]
    float recoverySpeed = 1.5f;

    [SerializeField]
    Vector3 maximumTranslationShake = Vector3.one * 0.5f;

    [SerializeField]
    float traumaExponent = 2;

    [SerializeField]
    private GameObject self;

    private float trauma = 0;

    private float seed;

    private void Awake()
    {
        seed = Random.value;
    }

    public void shake(float x)
    {
        trauma = 0.3f * x;
        frequency = 23;
    }

    public void sShake(float x)
    {
        trauma = 0.3f * x;
        frequency = 10;
    }


    void Update()
    {
        if (trauma < 0.11f)
        {
            trauma = 0.1f;
            frequency = 0.15f;
        }
        float shake = Mathf.Pow(trauma, traumaExponent);
        transform.localPosition = new Vector3(
        maximumTranslationShake.x * (Mathf.PerlinNoise(seed, Time.time * frequency) * 2 - 1),
        maximumTranslationShake.y * (Mathf.PerlinNoise(seed + 1, Time.time * frequency) * 2 - 1),
        maximumTranslationShake.z * (Mathf.PerlinNoise(seed + 2, Time.time * frequency) * 2 - 1)
    ) * trauma;

        transform.localRotation = Quaternion.Euler(new Vector3(
            maximumAngularShake.x * (Mathf.PerlinNoise(seed + 3, Time.time * frequency) * 2 - 1),
            maximumAngularShake.y * (Mathf.PerlinNoise(seed + 4, Time.time * frequency) * 2 - 1),
            maximumAngularShake.z * (Mathf.PerlinNoise(seed + 5, Time.time * frequency) * 2 - 1)
        ) * trauma);

        trauma = Mathf.Clamp01(trauma - recoverySpeed * Time.deltaTime);
    }
}