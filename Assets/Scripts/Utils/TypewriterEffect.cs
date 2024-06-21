using System;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Typewriter Effect")]
[RequireComponent(typeof(Text))]
public class TypewriterEffect : MonoBehaviour
{
    public bool IsActive
    {
        get
        {
            return this.isActive;
        }
        set
        {
            this.isActive = value;
        }
    }

    public void StartTypeWrite(string content, Action onfinish = null)
    {
        if (this.mText == null)
        {
            this.mText = base.GetComponent<Text>();
        }
        this.timer = 0f;
        this.words = content;
        this.mText.text = string.Empty;
        this.charsPerSecond = Mathf.Max(1, this.charsPerSecond);
        this.IsActive = true;
        this.callback = onfinish;
    }

    public void BreakTypeWrite()
    {
        this.IsActive = false;
        base.GetComponent<Text>().text = this.words;
    }

    private void OnWriter()
    {
        if (this.IsActive)
        {
            int num = (int)((float)this.charsPerSecond * this.timer);
            if (num > this.words.Length)
            {
                this.OnFinish();
                return;
            }
            this.mText.text = this.words.Substring(0, num);
            this.timer += Time.deltaTime;
        }
    }

    public void OnFinish()
    {
        this.IsActive = false;
        this.timer = 0f;
        base.GetComponent<Text>().text = this.words;
        try
        {
            if (this.callback != null)
            {
                this.callback();
                this.callback = null;
            }
        }
        catch (Exception arg)
        {
            FFDebug.LogError(this, "TypeWrite Exception: " + arg);
        }
    }

    private void Update()
    {
        this.OnWriter();
    }

    public Action callback;

    public int charsPerSecond;

    private bool isActive;

    private float timer;

    private string words;

    private Text mText;

    private bool state;
}
