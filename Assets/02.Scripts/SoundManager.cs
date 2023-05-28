using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioMixer mixer;
    public AudioSource Bgm;
    public AudioClip[] BgmList;
    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for(int i = 0; i < BgmList.Length; i++)
        {
            if (arg0.name == BgmList[i].name)
                BgmSoundPlay(BgmList[i]);
        }
    }


    public void SoundPlay(string SoundName,AudioClip clip)
    {
        GameObject go = new GameObject(SoundName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = clip;
        audioSource.volume = 0.2f;
        audioSource.Play();
        Debug.Log("효과음 재생중!");

        Destroy(go, clip.length);
    }

    public void BgmSoundPlay(AudioClip clip)
    {
        Bgm.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];
        Bgm.clip = clip;
        Bgm.loop = true;
        Bgm.volume = 0.1f;
        Bgm.Play();
    }
}
