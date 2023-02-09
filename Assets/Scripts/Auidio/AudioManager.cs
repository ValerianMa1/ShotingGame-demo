using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    [SerializeField] AudioSource SFXPlayer;



    const float MIN_PITCH = 0.9F;
    const float MAX_PITCH = 1.1F;





    //适合播放不需要经常变化的，如背景音乐
    public void PlaySFX(AudioData audioData)
    {
        SFXPlayer.PlayOneShot(audioData.audioClip,audioData.volume);
    }



    //适合播放需要快速且重复的音效
    public void PlayRandomSFX(AudioData audioData)
    {
        SFXPlayer.pitch = Random.Range(MIN_PITCH,MAX_PITCH);
        PlaySFX(audioData);
    }


    //上面播放音效函数的重载，使得播放的音效更加丰富，因为是随机从多个音效中抽取一个来播放
    public void PlayRandomSFX(AudioData[] audioData)
    {
        PlayRandomSFX(audioData[Random.Range(0,audioData.Length)]);
    }
}






[System.Serializable] public class AudioData
{
    public AudioClip audioClip;
    public float volume;
}
