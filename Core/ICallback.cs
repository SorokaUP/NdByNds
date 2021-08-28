namespace Core.Model
{
    public interface ICallback
    {
        void OnSuccess(string message);
        void OnFailed(string message);
        void OnMessage(string message, bool isRewriteLine = false);
        void OnProgress(int value, int max);
    }
}
