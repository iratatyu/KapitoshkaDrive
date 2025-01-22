using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    [Header("Cycle Settings")]
    [Tooltip("Тривалість повного циклу (в хвилинах)")]
    public float dayLengthInMinutes = 25f;

    [Header("References")]
    [Tooltip("Джерело світла, яке виконує роль сонця")]
    public Light sunLight;

    private float _dayLengthInSeconds;
    private float _rotationSpeed;

    void Start()
    {
        if (sunLight == null)
        {
            Debug.LogError("Sun Light не прив'язане до скрипта!");
            return;
        }

        // Конвертуємо тривалість дня в секунди
        _dayLengthInSeconds = dayLengthInMinutes * 60f;

        // Розрахунок швидкості обертання (360 градусів за цикл)
        _rotationSpeed = 360f / _dayLengthInSeconds;
    }

    void Update()
    {
        // Обертання джерела світла навколо своєї осі
        sunLight.transform.Rotate(Vector3.right, _rotationSpeed * Time.deltaTime);

        // Налаштування інтенсивності світла (день і ніч)
        float sunAngle = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        float baseIntensity = Mathf.Clamp01(sunAngle + 0.5f); // Від 0 (ніч) до 1 (день)

        // Задаємо мінімальну інтенсивність для ночі
        float minNightIntensity = 0.2f; // Нічна яскравість
        sunLight.intensity = Mathf.Max(baseIntensity, minNightIntensity);
    }
}
