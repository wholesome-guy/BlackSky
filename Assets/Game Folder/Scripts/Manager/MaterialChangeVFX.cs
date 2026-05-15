using System;
using System.Collections;
using UnityEngine;

public class MaterialChangeVFX : MonoBehaviour
{
    public static Action<MeshRenderer[], Material, int, float> MaterialFlashEvent;
    private void OnEnable()
    {
        MaterialFlashEvent += MaterialFlash;
    }
    private void OnDisable()
    {
        MaterialFlashEvent -= MaterialFlash;
    }
    private void MaterialFlash(MeshRenderer[] meshRenderers, Material change, int iterations, float duration)
    {
        Material[] originalMaterials =  new Material[meshRenderers.Length];
        for(int i = 0; i < meshRenderers.Length; i++)
        {
            originalMaterials[i] = meshRenderers[i].sharedMaterial;
        }
        StartCoroutine(MaterialChangeRoutine(meshRenderers, originalMaterials, change, iterations, duration));
    }
    private IEnumerator MaterialChangeRoutine(MeshRenderer[] meshRenderers, Material[] originalMaterials, Material change, int iterations, float duration)
    {

        for (int i = 0; i < iterations; i++)
        {
            for (int k = 0; k < meshRenderers.Length; k++)
            {
                meshRenderers[k].sharedMaterial = originalMaterials[k];
                meshRenderers[k].sharedMaterial = change;
            }

            yield return new WaitForSeconds(duration);

            for (int k = 0; k < meshRenderers.Length; k++)
            {
                meshRenderers[k].sharedMaterial = originalMaterials[k];
            }

            yield return new WaitForSeconds(duration);

            duration = Mathf.Max(0.05f, duration * 0.9f);
        }

    }
}
