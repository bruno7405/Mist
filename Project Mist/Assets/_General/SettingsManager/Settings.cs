using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class Settings : ScriptableObject
{
    public float sensitivity;
    public float volume;

    public UnityEvent OnSettingsChanged;
}
