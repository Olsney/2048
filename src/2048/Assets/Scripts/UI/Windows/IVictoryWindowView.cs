namespace UI.Windows
{
    public interface IVictoryWindowView
    {
        event System.Action CloseRequested;
        event System.Action Destroyed;
        void SetMessage(string message);
        void Close();
    }
}
