namespace Tests.CSharp.Homework3;

public class SingleInitializationSingleton
{
    public const int DefaultDelay = 3_000;
    
    private static readonly object Locker = new();

    private static volatile bool _isInitialized = false;

    private static SingleInitializationSingleton? _instance = null;

    private SingleInitializationSingleton(int delay = DefaultDelay)
    {
        Delay = delay;
        // imitation of complex initialization logic
        Thread.Sleep(delay);
    }

    public int Delay { get; }

    public static SingleInitializationSingleton Instance
    {
        get
        {
            lock (Locker)
            {
                if (!_isInitialized)
                {
                    _instance = new SingleInitializationSingleton();
                    _isInitialized = true;
                }
                return _instance!;
            }
        }
    }

    internal static void Reset()
    {
        lock (Locker)
        {
            _isInitialized = false;
            _instance = null;
        }
    }

    public static void Initialize(int delay)
    {
        if (!_isInitialized)
        {
            lock (Locker)
            {
                if (_isInitialized)
                    throw new InvalidOperationException("Instance has already been initialized in another thread");
                
                _isInitialized = true;
                _instance = new SingleInitializationSingleton(delay);
            }
        }
        else
            throw new InvalidOperationException("Double initialization is prohibited");
    }
}