using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundSO", menuName = "Scriptable Objects/SoundSO")]
public class SoundSO : ScriptableObject
{
    public List<Sounds> SoundList;
}
[Serializable]
public struct Sounds
{
    public SoundType type;
    public AudioClip clip;
}
