using UnityEngine;

// Singleton helper
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static object _lock        = new object();
    protected static T      _instance    = null; //this instance will not be destroyed on scene destroyed, only destroyed when Game destroyed
    protected static bool   _initialized = false;

    public static bool IsNotNull
    {
        get { return _instance != null; }
    }

    public static T Instance
    {
        get
        {
            if (_quitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed on application quit." +
                                  " Won't create again!");

                if (!Application.isEditor || Application.isPlaying)
                {
                    return null;
                }
                else
                {
                    //if is Unity editor and in edit mode
                    if (_instance) { DestroyImmediate(_instance.gameObject); }
                    _quitting = false;
                }
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    T[] objectsOfType = FindObjectsOfType<T>();
                    if (objectsOfType != null && objectsOfType.Length == 1) { _instance = objectsOfType[0]; }
                    if (objectsOfType != null && objectsOfType.Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                                        " - there should never be more than 1 singleton!" +
                                        " Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance      = singleton.AddComponent<T>();
                        singleton.name = typeof(T).ToString() + "(Singleton)";

                        DontDestroyOnLoad(singleton);

                        GameObject par = GameObject.Find("SingletonHolder(Singleton)") ?? GameObject.Find("GlobalManagers");
                        if (par)
                        {
                            singleton.transform.SetParent(par.transform);

                            singleton.transform.position      = Vector3.zero;
                            singleton.transform.localPosition = Vector3.zero;
                            singleton.transform.localScale    = Vector3.one;
                        }

                        Debug.Log("[Singleton] An instance of " + typeof(T) +
                                   " is needed in the scene, so '" + singleton +
                                   "' was created with DontDestroyOnLoad.");
                    }
                    else
                    {
                        DontDestroyOnLoad(_instance);
                        Debug.Log("[Singleton] Using instance already created: " + _instance.gameObject.name);
                    }
                }
                if (_instance != null)
                {
                    if (!_initialized)
                    {
                        _instance.GetComponent<Singleton<T>>().Initialize();
                    }
                    _initialized = true;
                }
                return _instance;
            }
        }
    }

    /// <summary>
    /// this call if want to reset singleton
    /// </summary>
    public static void ResetSingleton()
    {
        if (_instance)
        {
            DestroyImmediate(_instance.gameObject);
        }
        _instance    = null;
        _quitting    = false;
        _initialized = false;
    }

    /// <summary>
    /// this call if current object is the [Singleton]
    /// </summary>
    public virtual void Initialize() { }

    /// <summary>
    /// this call if current object is the [Singleton]
    /// </summary>
    public virtual void Deinitialize() { }

    protected static bool _quitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed,
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            Debug.LogWarning("[Singleton] Destroy: " + gameObject.name);
            _quitting = true;
            if (_initialized) { Deinitialize(); }
            _initialized = false;
            _instance    = null;
        }
    }
}
