using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlipSpawner : MonoBehaviour
{
    [SerializeField] GameObject blipPrefab;
    [SerializeField] float offsetRadius;

    public void SpawnBlip() {
        GameObject blip = Instantiate(blipPrefab, transform.position + offsetRadius * Random.insideUnitSphere, Quaternion.Euler(90, 0, 0));
        StartCoroutine(FadeOut(blip));
    }

    IEnumerator FadeOut(GameObject blip) {
        Material m = blip.GetComponent<MeshRenderer>().material;
        float timer = 0;
        while (timer < 3) {
            m.SetFloat("_Alpha", 1 - timer / 3.0f);

            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(blip);
    }
}
