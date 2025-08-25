using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Highlights 3D object by changing material color
/// Use on any interactable object (visuals for item pickup)
/// </summary>
public class HighlightObject : MonoBehaviour
{
    public Color highlightedColor = Color.white;
    static float intensity = 0.25f;

    [SerializeField] GameObject rendererParent;
    [SerializeField] private List<Renderer> renderers;
    List<Material> materials;

    private void Awake()
    {
        if (rendererParent != null)
        {
            foreach (Renderer r in rendererParent.GetComponentsInChildren<Renderer>())
            {
                if (!renderers.Contains(r))
                    renderers.Add(r);
            }
        }
    }

    void Start()
    {
        materials = new List<Material>();
        foreach (var renderer in renderers)
        {
            //A single child-object might have mutliple materials on it
            //that is why we need to all materials with "s"
            materials.AddRange(new List<Material>(renderer.materials));
        }
    }

    public void Highlight()
    {
        foreach (var material in materials)
        {
            //We need to enable the EMISSION
            material.EnableKeyword("_EMISSION");
            //before we can set the color
            material.SetColor("_EmissionColor", highlightedColor * intensity);
        }
    }

    public void DeHighlight()
    {
        foreach(var material in materials)
        {
            material.DisableKeyword("_EMISSION");
        }
    }
}
