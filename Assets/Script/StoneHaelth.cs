using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoneHealth : MonoBehaviour
{
    [Header("체력")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("점멸 기본값(미지정 시 사용)")]
    public Color defaultHitColor = Color.red;
    public float defaultFlashDuration = 0.1f;
    public int defaultFlashCount = 2;

    private Renderer rend;
    private readonly MaterialPropertyBlock mpb = new MaterialPropertyBlock();
    private bool isFlashing = false;

    private List<Color> originalColors = new List<Color>();
    private int colorId_BaseColor = Shader.PropertyToID("_BaseColor");
    private int colorId_Color = Shader.PropertyToID("_Color");

    void Awake()
    {
        currentHealth = maxHealth;
        rend = GetComponent<Renderer>();
        CacheOriginalColors();
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        if (currentHealth == 0) { Destroy(gameObject); return; }
        if (!isFlashing) StartCoroutine(FlashRoutine(defaultHitColor, defaultFlashDuration, defaultFlashCount));
    }

    public void FlashHit(Color color, float duration, int count)
    {
        if (!isFlashing) StartCoroutine(FlashRoutine(color, duration, count));
    }

    private IEnumerator FlashRoutine(Color flashColor, float dur, int count)
    {
        isFlashing = true;
        for (int i = 0; i < count; i++)
        {
            SetAllColors(flashColor); yield return new WaitForSeconds(dur);
            RestoreAllColors(); yield return new WaitForSeconds(dur);
        }
        isFlashing = false;
    }

    private void CacheOriginalColors()
    {
        originalColors.Clear();
        if (rend == null) return;
        for (int i = 0; i < rend.sharedMaterials.Length; i++)
        {
            var m = rend.sharedMaterials[i];
            if (m != null && m.HasProperty(colorId_BaseColor))
                originalColors.Add(m.GetColor(colorId_BaseColor));
            else if (m != null && m.HasProperty(colorId_Color))
                originalColors.Add(m.GetColor(colorId_Color));
            else
                originalColors.Add(Color.white);
        }
    }

    private void SetAllColors(Color c)
    {
        if (rend == null) return;
        for (int i = 0; i < rend.sharedMaterials.Length; i++)
        {
            rend.GetPropertyBlock(mpb, i);
            var m = rend.sharedMaterials[i];
            if (m != null && m.HasProperty(colorId_BaseColor)) mpb.SetColor(colorId_BaseColor, c);
            else mpb.SetColor(colorId_Color, c);
            rend.SetPropertyBlock(mpb, i);
        }
    }

    private void RestoreAllColors()
    {
        if (rend == null) return;
        for (int i = 0; i < rend.sharedMaterials.Length; i++)
        {
            Color orig = (i < originalColors.Count) ? originalColors[i] : Color.white;
            rend.GetPropertyBlock(mpb, i);
            var m = rend.sharedMaterials[i];
            if (m != null && m.HasProperty(colorId_BaseColor)) mpb.SetColor(colorId_BaseColor, orig);
            else mpb.SetColor(colorId_Color, orig);
            rend.SetPropertyBlock(mpb, i);
        }
    }
}
