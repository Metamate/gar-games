using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Flappy12;

// A Singleton: exactly one Audio object exists, reached through Audio.Instance.
// Compare with the static Art class: both give global access, but Audio is an object,
// so it could implement an interface or be passed to the code that needs it.
public class Audio
{
    public static Audio Instance { get; } = new Audio();

    private SoundEffect _flap;
    private SoundEffect _hurt;
    private SoundEffect _explosion;
    private SoundEffect _score;
    private Song _music;

    // Private, so no other code can create a second instance.
    private Audio() { }

    public void LoadContent(ContentManager content)
    {
        _flap = content.Load<SoundEffect>("audio/flap");
        _hurt = content.Load<SoundEffect>("audio/hurt");
        _explosion = content.Load<SoundEffect>("audio/explosion");
        _score = content.Load<SoundEffect>("audio/score");
        _music = content.Load<Song>("audio/marios_way");
    }

    public void PlayMusic()
    {
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Play(_music);
    }

    public void PlayFlap() => _flap.Play();

    public void PlayScore() => _score.Play();

    public void PlayCrash()
    {
        _hurt.Play();
        _explosion.Play();
    }
}