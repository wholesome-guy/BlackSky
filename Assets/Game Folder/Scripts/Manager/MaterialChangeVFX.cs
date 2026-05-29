using System;
using System.Collections;
using UnityEngine;

public class MaterialChangeVFX : MonoBehaviour
{
    public static Action<MeshRenderer[], Material, int, float> MaterialFlashEvent;

    private Material[] _originalMaterials = new Material[8];
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
        if (meshRenderers.Length > _originalMaterials.Length)
            _originalMaterials = new Material[meshRenderers.Length];

        for (int i = 0; i < meshRenderers.Length; i++)
            _originalMaterials[i] = meshRenderers[i].sharedMaterial;

        StartCoroutine(MaterialChangeRoutine(meshRenderers, change, iterations, duration));
    }
    private IEnumerator MaterialChangeRoutine(MeshRenderer[] meshRenderers,Material change, int iterations, float duration)
    {

        int count = meshRenderers.Length;

        for (int i = 0; i < iterations; i++)
        {
            for (int k = 0; k < count; k++)
            {
                meshRenderers[k].sharedMaterial = change;
            }

            float timer = 0f;
            while (timer < duration) 
            {
                timer += Time.deltaTime; 
                yield return null; 
            }

            for (int k = 0; k < count; k++)
            {
                meshRenderers[k].sharedMaterial = _originalMaterials[k];
            }

            timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            duration = Mathf.Max(0.05f, duration * 0.9f);
        }

    }

}

