using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PersistantAudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static PersistantAudioManager instance;

    bool fadingOut = false;
    string fadingSong, newSong;
    float timer;
    float startTimer = 2;

    bool turningDown = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        fadingOut = false;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.outputAudioMixerGroup;
        }
    }

    private void Update()
    {
        if (fadingOut)
        {
            Sound s = Array.Find(sounds, sound => sound.name == fadingSong);
            if (timer > 0)
            {
                // fade out music
                timer -= Time.deltaTime;
                s.source.volume = Mathf.Max(0, timer / startTimer);
                if (s.source.volume <= 0)
                {
                    s.source.Stop();
                    fadingOut = false;
                    if (!turningDown)
                    {
                        Sound t = Array.Find(sounds, sound => sound.name == newSong);
                        t.source.Play();
                        t.source.volume = t.volume;
                    }
                }
            }
        }
    }

    public bool isPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.source.isPlaying; 
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("could not find song " + name + "... typo?");
            return;
        }

        for (int i=0; i < sounds.Length; i++)
        {
            Sound oldSong = sounds[i];
            if (oldSong.source.isPlaying)
            {
                Debug.Log(oldSong.name + " is playing.");
            }
            if (oldSong.source.isPlaying && oldSong.name != name)
            {
                Debug.Log("changing song");
                // fadeout current song, and then play new song
                fadingSong = oldSong.name;
                newSong = name;
                FadeOut();
                return;
            }
        }

        if (!isPlaying(name) || (isPlaying(name) && s.source.volume <= 0))
        {
            s.source.volume = s.volume;
            s.source.Play();
        }
    }

    public void RandomizePitch(string name, float low, float high)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("could not find song " + name + "... typo?");
            return;
        }
        s.pitch = UnityEngine.Random.Range(low, high);
        s.source.pitch = s.pitch;
    }

    public void FadeOut()
    {
        fadingOut = true;
        turningDown = false;
        timer = startTimer;
    }

    public void TurnDown(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (!s.source.isPlaying) return;
        turningDown = true;
        fadingOut = true;
        fadingSong = name;
        timer = startTimer;
    }

    public void StopWhateverIsPlaying()
    {
        for (int i=0; i < sounds.Length; i++)
        {
            if (sounds[i].source.isPlaying)
            {
                sounds[i].source.Stop();
            }
        }
    }
}
