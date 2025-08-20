using UnityEngine;

public enum SoundType
{
    Button_Click,
    Match,
    No_Match,
    Won,
    Background,
    Card_Flip,
    Game_Start,
}
public class SoundService
{
    private SoundSO soundSO;
    private AudioSource bgAudio;
    private AudioSource sfxAudio;
    public SoundService(SoundSO so, AudioSource bg, AudioSource sfx)
    {
        this.soundSO = so;
        this.bgAudio = bg;
        this.sfxAudio = sfx;

        PlayBackground();
    }
    private void PlayBackground()
    {
        AudioClip clip = GetAudioClip(SoundType.Background);
        bgAudio.loop = true;
        bgAudio.clip = clip;
        bgAudio.Play();
    }
    public void PlaySFX(SoundType type)
    {
        AudioClip clip = GetAudioClip(type);
        sfxAudio.PlayOneShot(clip);
    }
    private AudioClip GetAudioClip(SoundType type)
    {
        Sounds sound = soundSO.SoundList.Find(clp =>  clp.type == type);
        return sound.clip;
    }
}
