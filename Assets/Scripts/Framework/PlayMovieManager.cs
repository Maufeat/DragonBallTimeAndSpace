using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayMovieManager : SingletonForMono<PlayMovieManager>
{
    [DllImport("__Internal")]
    private static extern void PlayMovie(string path);

    [DllImport("__Internal")]
    private static extern void StopMovie();

    public void Init()
    {
        this._curPlayTime = 0f;
        GameObject gameObject = GameObject.Find("UIRoot").gameObject;
        this._img_mask = gameObject.transform.Find("Img_Mask").gameObject.GetComponent<Image>();
        this._img_mask.color = Color.black;
    }

    public void PlayVideo(string videoName)
    {
        PlayMovieManager._strMovieName = videoName;
        this._isPlaying = true;
        this._curPlayTime = 0f;
        AVPlayOP.Instance.rawImage.transform.localRotation = Quaternion.Euler(0f, 180f, 180f);
        AVPlayOP.Instance.PlayMoveToImage(videoName, false, AVPlayOP.Instance.rawImage, 0f, this.OnPlayMovieComplete);
    }

    private void PlayMovieComplete(string str)
    {
        this._isPlaying = false;
        this.SetImageMask(false);
        if (this.OnPlayMovieComplete != null)
        {
            this.OnPlayMovieComplete();
        }
    }

    private void Update()
    {
        if (!this._isPlaying)
        {
            return;
        }
        if (this._curPlayTime < 60f)
        {
            this._curPlayTime += Time.deltaTime;
        }
        else
        {
            this._curPlayTime = 60f;
            this._isPlaying = false;
            this.SetImageMask(false);
        }
    }

    private void SetImageMask(bool isShow)
    {
        if (this._img_mask == null)
        {
            return;
        }
        if (this._img_mask.gameObject.activeSelf == isShow)
        {
            return;
        }
        this._img_mask.gameObject.SetActive(isShow);
    }

    private void OnApplicationPause(bool pauseState)
    {
        if (this.OnPlayMovieComplete != null)
        {
            this.OnPlayMovieComplete();
        }
    }

    private float _curPlayTime;

    private bool _isPlaying;

    [SerializeField]
    private Image _img_mask;

    private static string _strMovieName = string.Empty;

    private static string _strMovieDir = "Data/Raw/moviesamples/";

    public Action OnPlayMovieComplete;
}
