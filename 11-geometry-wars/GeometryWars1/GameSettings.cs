namespace GeometryWars1;

public static class GameSettings
{
    public static class Window
    {
        public const int Width = 1280;
        public const int Height = 720;
    }

    // The play field. It is larger than the window, and drawn scaled down into it.
    public static class Arena
    {
        public const int Width = 1920;
        public const int Height = 1080;
    }

    public static class Performance
    {
        public const int MaxParticles = 1024 * 20;
        public const int MaxGridPoints = 1024 * 2;
        public const int MaxEntities = 200;
    }
}
