using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;


/// <summary>
/// 動画が終了したときコールバックイベントを発生させる
/// 2017/6/9 Fantom (Unity 5.6.1p2)
/// http://fantom1x.blog130.fc2.com/blog-entry-244.html
///（使い方）
///・VideoPlayer を含む GameObject にアタッチして、インスペクタから OnVideoFinished にコールバックしたいメソッド（引数なしメソッド）を登録する。
///（仕様説明）
///・再生開始時に VideoPlayer.isLooping が true（ループ再生）のとき、VideoPlayer.loopPointReached（ループポイント到達のコールバック）で終了判定をする。
///・再生開始時に VideoPlayer.isLooping が false（ループなし）のとき、CheckFrame() でフレーム数による終了判定をする。
///・再生途中で VideoPlayer.isLooping や frameCheckInterval を変更することには未対応（停止後、再開すれば正しく動作する）。
/// </summary>
public class VideoFinishedCallback : MonoBehaviour
{
    public VideoPlayer videoPlayer;             //アタッチした VideoPlayer をインスペクタでセットする

    public float frameCheckInterval = 0.5f;     //ループなしのときに現在のフレーム番号で終端を判別する（間隔を空ければ負荷は軽くなる）

    //インスペクタ用コールバック
    public UnityEvent OnVideoFinished;          //コールバックするメソッドをインスペクタから登録する


    void Awake()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponentInChildren<VideoPlayer>();
    }

    void OnEnable()
    {
        videoPlayer.loopPointReached += EndReached; //ループポイント到達のコールバック
        videoPlayer.started += Restart;             //再生開始のコールバック
    }

    void OnDisable()
    {
        videoPlayer.loopPointReached -= EndReached;
        videoPlayer.started -= Restart;
    }

    //ループ終端の到達コールバック
    void EndReached(VideoPlayer vp)
    {
        Finish();
    }

    //フレーム数のチェックを開始する（コルーチンの再開）
    void Restart(VideoPlayer vp)
    {
        if (!videoPlayer.isLooping)      //true のときフレーム数はチェックしない
            StartCoroutine(CheckFrame());
    }

    // Use this for initialization
    void Start()
    {
        if (!videoPlayer.isLooping && videoPlayer.isPlaying)    //PlayOnAwake ※念のため
            StartCoroutine(CheckFrame());
    }

    bool checking = false;  //コルーチン２重防止

    //現在のフレームと全フレーム数を比較して終了判定をする（ループ再生時は上手く機能しない）
    IEnumerator CheckFrame()
    {
        if (checking)
            yield break;

        checking = true;
        WaitForSeconds wfs = new WaitForSeconds(frameCheckInterval);

        while (!videoPlayer.isLooping)
        {
            if ((ulong)videoPlayer.frame >= videoPlayer.frameCount)
            {
                checking = false;
                Finish();
                break;
            }
            yield return wfs;
        }
    }

    //動画が終了したときの処理
    void Finish()
    {
        if (OnVideoFinished != null)
            OnVideoFinished.Invoke();   //イベントコールバック
    }
}