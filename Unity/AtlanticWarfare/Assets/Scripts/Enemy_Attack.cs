using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var city = collision.gameObject;
        if (!city.CompareTag(targetTag))
        {
            return;
        }

        StartCoroutine(DoDamage(city));

        // Destroy(gameObject, 0.5f);
    }

    private IEnumerator DoDamage(GameObject city)
    {
        while (city.activeSelf)
        {
            yield return new WaitForSeconds(cooldown);

            if (city == null || !city.activeSelf)
            {
                yield break;
            }

            city.GetComponent<Health>()?.DoDamage(power);
        }
    }

    public int power = 4;

    public float cooldown = 1.5f;

    public string targetTag;
}
