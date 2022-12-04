using UnityEngine;

[System.Serializable] public class PlayerSound
{
    [SerializeField] private AudioSource source;

    public void PlayWalkSound()
    {
        if (AudioManager.Instance && !source.isPlaying) AudioManager.Instance.PlayPlayerWalk(source);
    }
    public void StopWalkSounds()
    {
        source.loop = false;
    }
}