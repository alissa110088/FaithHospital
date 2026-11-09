using UnityEngine;

public class CandleFlicker : MonoBehaviour
{
    [Header("Configuration de la Lumière")]
    [SerializeField] private Light candleLight;

    [Header("Paramètres du Vacillement")]
    [Tooltip("Intensité minimale de la flamme")]
    [SerializeField] private float minIntensity = 0.75f;

    [Tooltip("Intensité maximale de la flamme")]
    [SerializeField] private float maxIntensity = 1.25f;

    [Tooltip("Plus la valeur est grande, plus le vacillement est rapide")]
    [SerializeField] private float flickerSpeed = 0.1f;

    private float targetIntensity;
    private float lastIntensity;
    private float timer;

    void Start()
    {
        // Si le composant Light n'est pas assigné, on essaie de le récupérer sur l'objet
        if (candleLight == null)
        {
            candleLight = GetComponent<Light>();
        }

        if (candleLight != null)
        {
            lastIntensity = candleLight.intensity;
            targetIntensity = candleLight.intensity;
        }
        else
        {
            Debug.LogError("Aucun composant Light trouvé sur l'objet ou assigné au script !", this);
        }
    }

    void Update()
    {
        if (candleLight == null) return;

        // Avancer le chronomètre
        timer += Time.deltaTime;

        // Si le chronomètre dépasse la vitesse définie, on choisit une nouvelle intensité cible
        if (timer >= flickerSpeed)
        {
            lastIntensity = candleLight.intensity;
            targetIntensity = Random.Range(minIntensity, maxIntensity);
            timer = 0f;
        }

        // Transition fluide (interpolation linéaire) entre l'ancienne et la nouvelle intensité
        candleLight.intensity = Mathf.Lerp(lastIntensity, targetIntensity, timer / flickerSpeed);
    }
}
