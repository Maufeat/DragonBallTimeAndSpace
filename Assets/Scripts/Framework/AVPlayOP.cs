using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AVPlayOP : MonoBehaviour
{
    private void Start()
    {
        AVPlayOP.Instance = this;
    }

    public void PlayMoveToImage(string vedioName, bool loop, RawImage imgToShow, float eventTimer, Action endAction = null)
    {
        this.callEventTime = eventTimer;
        this.curPlayImage = imgToShow;
        this.vedioEndCall = endAction;
        this.m_movie.UnloadMovie();
        this.m_movie._folder = Application.streamingAssetsPath + "/moviesamples/";
        this.m_movie._filename = vedioName;
        this.m_movie._loadOnStart = true;
        this.m_movie._playOnStart = true;
        bool flag = this.m_movie.LoadMovie(true);
        base.StartCoroutine(this.onLoadMediaFinish());
        this.m_movie._loop = loop;
        this.isLoop = loop;
        this.avpWm = this.m_movie.MovieInstance;
        this.curPlayImage.texture = this.m_movie.OutputTexture;
    }

    private IEnumerator onLoadMediaFinish()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.1f);
            if (this.m_movie == null || this.m_movie.MovieInstance == null)
            {
                yield break;
            }
            if (this.m_movie.MovieInstance.IsPlaying)
            {
                yield return new WaitForSeconds(0.2f);
                break;
            }
        }
        if (this.curPlayImage != null)
        {
            this.curPlayImage.gameObject.SetActive(true);
            this.curPlayImage.color = Color.white;
        }
        yield break;
    }

    public void StopPlay()
    {
        if (null == this.curPlayImage)
        {
            return;
        }
        this.curPlayImage.gameObject.SetActive(false);
        this.m_movie.UnloadMovie();
        if (this.avpWm != null)
        {
            this.avpWm.Pause();
            this.avpWm.Dispose();
        }
        this.curPlayImage.texture = null;
        this.curPlayImage = null;
        if (this.vedioEndCall != null)
        {
            this.vedioEndCall();
            this.vedioEndCall = null;
        }
    }

    private void Update()
    {
        if (this.avpWm != null)
        {
            if (this.curPlayImage != null && this.curPlayImage.texture == null)
            {
                this.curPlayImage.texture = this.m_movie.OutputTexture;
            }
            if (this.avpWm.PositionSeconds >= this.avpWm.DurationSeconds - this.callEventTime && !this.isLoop)
            {
                this.StopPlay();
            }
        }
    }

    private void rqwImageSetActiveFalse()
    {
        this.rawImage.gameObject.SetActive(false);
        this.layMaskGo.SetActive(false);
    }

    public AVProWindowsMediaMovie m_movie;

    public RawImage rawImage;

    public GameObject layMaskGo;

    private RawImage curPlayImage;

    public AVProWindowsMedia avpWm;

    private float callEventTime;

    private bool isLoop;

    private Action vedioEndCall;

    public static AVPlayOP Instance;
}
