using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioMixer mixer;
    /*
    public AudioSource Bgm;
    public AudioClip[] BgmList;
    */
        /** 배경음악 관련 */
    [Header("---BGM---")]
    public AudioClip[] BGMClips;
    public AudioSource[] BGMPlayers;
    public int BGMChannels;
    int BGMChannelIndex;
    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            //SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }

    /*private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for(int i = 0; i < BgmList.Length; i++)
        {
            if (arg0.name == BgmList[i].name)
                BgmSoundPlay(BgmList[i]);
        }
    }*/

    
    public enum BGM
    {
        Home,
        Chapter1,
        Chapter2,
        Chapter3,
        LoadingScene0,
        LoadingScene1,
        LoadingScene2,

    }

    void Init()
    {
        /** 배경음 플레이어 초기화 */
        GameObject BGMObject = new GameObject("BGMPlayer");
        /** BGMObject의 부모클래스 이 스크립트를 가진 오브젝트로 한다. */
        BGMObject.transform.parent = transform;
        /** 채널의 개수만큼 배경음 재생기 생성 */
        BGMPlayers = new AudioSource[BGMChannels];

        for (int i = 0; i < BGMPlayers.Length; i++)
        {
            /** BGMPlayer는 BGMObject에 추가한 AudioSource를 가져온다. */
            BGMPlayers[i] = BGMObject.AddComponent<AudioSource>();
            /** 배경음 재생 무한 반복 */
            BGMPlayers[i].loop = true;
            /** 배경음 플레이 */
            BGMPlayers[i].clip = BGMClips[0];
            BGMPlayers[i].volume = 0.2f;
        }

        PlayBGM(BGM.Home);
    }

    public void PlayBGM(BGM bgm)
    {
        /** 저장된 Length값만큼 반복 */
        for (int i = 0; i < BGMPlayers.Length; i++)
        {
            int LoopIndex = (i + BGMChannelIndex) % BGMPlayers.Length;
         
            /** ChanelIndex를 LoopIndex값으로 바꿔준다. */
            BGMChannelIndex = LoopIndex;
            /** SFXPlayers의 0번째 Clip은 SFX Enum의 순서를 가져온다. */
            BGMPlayers[LoopIndex].clip = BGMClips[(int)bgm];
            /** 재생 */
            BGMPlayers[LoopIndex].Play();
            break;
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

        Destroy(go, clip.length);
    }

    /*public void BgmSoundPlay(AudioClip clip)
    {
        Bgm.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];
        Bgm.clip = clip;
        Bgm.loop = true;
        Bgm.volume = 0.1f;
        Bgm.Play();
    }*/
}
