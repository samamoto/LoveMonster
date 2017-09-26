using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static bool isInstance;

    private void Clean()
    {
        if (FindObjectsOfType<T>().Length > 1)
            DestroyImmediate(this);
    }

    //インスタンスを入手する
    public static T Instance
    {
        get
        {
            if (isInstance)
                return _instance;

            if (_instance == null)
            {
                var type = typeof(T);
                var objects = FindObjectsOfType<T>();

                //生成数チェック(既に1つインスタンスが存在する場合)
                if (objects.Length > 0)
                {
                    _instance = objects[0];
                    //インスタンスが2つ以上発生している場合
                    if (objects.Length > 1)
                    {
                        Debug.LogWarning(type + "のインスタンスが複数存在しています。" + "インスタンスを破棄します。");
                        for (int i = 1; i < objects.Length; i++)
                            //1つ目以降のインスタンス即刻破棄
                            DestroyImmediate(objects[i].gameObject);
                    }
                    return _instance;
                }
                GameObject gameObj = new GameObject(type.ToString(), type);
                isInstance = true;

                _instance = gameObj.AddComponent<T>();
                DontDestroyOnLoad(gameObj);
            }
            return _instance;
        }
    }
}