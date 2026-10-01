namespace GeometryWars2;

public static class GameSettings
{
    public static class Window
    {
        public const int Width = 1280;
        public const int Height = 720;
    }

    public static class Performance
    {
        public const int MaxParticles = 1024 * 20;
        public const int MaxGridPoints = 1024;
        public const int MaxEntities = 200;
    }

    public static class Player
    {
        public const int StartingLives = 3;
        public const int RespawnFrames = 120;
        public const int GameOverFrames = 120;
        public const float MultiplierExpiry = 0.8f;
        public const int MaxMultiplier = 20;
    }

    public static class Enemy
    {
        public static class Spawning
        {
            public const float ChanceStart = 90f;
            public const float ChanceMin = 30f;
            public const float ChanceDecay = 0.005f;
            public const float MinDistance = 180f;
        }
    }
}
