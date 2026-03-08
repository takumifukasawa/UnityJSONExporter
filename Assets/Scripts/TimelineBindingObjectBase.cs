using UnityEngine;
using UnityJSONExporter;

public abstract class TimelineBindingObjectBase : MonoBehaviour
{
    [HideInInspector]
    public virtual AnimationClipType AnimationClipType => AnimationClipType.Default;

    [SerializeField]
    private bool _exportComponentHierarchy = true;

    public bool ExportComponentHierarchy => _exportComponentHierarchy;
    public abstract string ResolvePropertyName(string propertyName);
}
