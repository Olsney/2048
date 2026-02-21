namespace UI.Elements
{
    public interface IScoreHudView
    {
        event System.Action RestartRequested;
        event System.Action Destroyed;
        void SetScore(int score);
    }
}
