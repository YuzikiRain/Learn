
```csharp
public class SoundEffect : MonoBehaviour, IAudio
{
    private AudioSource _audioSource;

    public void Init()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void Play(AudioClip audioClip)
    {
        _audioSource.Play();
    }
}

SoundEffect soundEffect = gameObject.AddComponent<SoundEffect>();
soundEffect.Init();
var clone = Object.Instantiate(gameObject);
// error, _audioSource is null
clone.Play(audioClip);
```