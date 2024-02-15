using UnityEngine;

public class Atom : MonoBehaviour
{
    public GameObject electronPrefab;
    public int numElectrons = 4; // Number of electrons
    public float[] electronSpeeds; // Speeds for each electron

    private GameObject[] electrons; // Array to hold electron objects
    private float[] electronAngles; // Angles for each electron

    public float nucleusRadius = 1f; // Radius of the nucleus
    public float[] electronOrbits; // Radii for each electron orbit

    void Start()
    {
        electrons = new GameObject[numElectrons];
        electronAngles = new float[numElectrons];

        // Create electrons and position them on their orbits
        for (int i = 0; i < numElectrons; i++)
        {
            float angle = Random.Range(0f, 360f);
            electronAngles[i] = angle;
            Vector3 electronPos = CalculateOrbitPosition(i, angle);
            electrons[i] = Instantiate(electronPrefab, electronPos, Quaternion.identity);
            electrons[i].transform.parent = transform;
        }
    }

    void Update()
    {
        // Move electrons around their orbits
        for (int i = 0; i < numElectrons; i++)
        {
            electronAngles[i] += electronSpeeds[i] * Time.deltaTime;
            if (electronAngles[i] >= 360f)
                electronAngles[i] -= 360f;
            Vector3 electronPos = CalculateOrbitPosition(i, electronAngles[i]);
            Vector3 randomDirection = Random.insideUnitSphere; // Get random direction
            electrons[i].transform.position = electronPos + randomDirection * nucleusRadius; // Move in random direction
        }
    }

    // Calculate position of electron on its orbit
    Vector3 CalculateOrbitPosition(int electronIndex, float angle)
    {
        float radius = electronOrbits[electronIndex];
        float radAngle = angle * Mathf.Deg2Rad;
        float x = transform.position.x + radius * Mathf.Cos(radAngle);
        float y = transform.position.y + radius * Mathf.Sin(radAngle);
        return new Vector3(x, y, transform.position.z);
    }

    // Draw nucleus and orbits for visualization in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, nucleusRadius);

        for (int i = 0; i < electronOrbits.Length; i++)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, electronOrbits[i]);
        }
    }
}
