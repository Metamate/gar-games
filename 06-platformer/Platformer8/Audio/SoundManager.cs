using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Platformer8.Audio;

public static class SoundManager
{
    private static SoundEffect _jump;
    private static SoundEffect _pickup;
    private static SoundEffect _kill;
    private static SoundEffect _death;
    private static SoundEffect _powerupReveal;
    private static SoundEffect _emptyBlock;
    private static Song _music;

    public static void LoadContent(ContentManager content)
    {
        _jump = content.Load<SoundEffect>("sounds/jump");
        _pickup = content.Load<SoundEffect>("sounds/pickup");
        _kill = content.Load<SoundEffect>("sounds/kill");
        _death = content.Load<SoundEffect>("sounds/death");
        _powerupReveal = content.Load<SoundEffect>("sounds/powerup-reveal");
        _emptyBlock = content.Load<SoundEffect>("sounds/empty-block");
        _music = content.Load<Song>("sounds/music");
    }

    public static void PlayJump() => _jump?.CreateInstance().Play();
    public static void PlayPickup() => _pickup?.CreateInstance().Play();
    public static void PlayKill() => _kill?.CreateInstance().Play();
    public static void PlayDeath() => _death?.CreateInstance().Play();
    public static void PlayPowerupReveal() => _powerupReveal?.CreateInstance().Play();
    public static void PlayEmptyBlock() => _emptyBlock?.CreateInstance().Play();

    public static void PlayMusic()
    {
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Play(_music);
    }

    public static void StopMusic() => MediaPlayer.Stop();
}
