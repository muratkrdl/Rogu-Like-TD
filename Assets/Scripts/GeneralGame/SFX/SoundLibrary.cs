using UnityEngine;

[System.Serializable]
public struct SoundEffect
{
    public string groupID;
    public AudioClip[] clips;
}

public class SoundLibrary : MonoBehaviour
{
    [SerializeField] SoundEffect[] soundEffects;

    public AudioClip GetClipFromName(string name)
    {
        foreach (SoundEffect soundEffect in soundEffects)
        {
            if(soundEffect.groupID == name)
            {
                return soundEffect.clips[Random.Range(0, soundEffect.clips.Length)];
            }
        }
        return null;
    }
}
