using System.Collections;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");
        var city = collision.gameObject;
        if (!city.CompareTag(targetTag))
        {
        Debug.Log("Collided with city");
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

            if (city.GetComponent<Health>().DoDamage(power)) Game_manager.KillCity(city.GetComponent<Stadt>());
        }
    }

    public int power = 4;

    public float cooldown = 1.5f;

    public string targetTag;
}
