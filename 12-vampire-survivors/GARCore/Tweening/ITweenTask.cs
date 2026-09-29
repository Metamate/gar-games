namespace GARCore.Tweening;

internal interface ITweenTask
{
    void Update(float dt);
    bool IsComplete { get; }
}
