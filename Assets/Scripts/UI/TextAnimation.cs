using System;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimation : MonoBehaviour
{
    public void SetAnimationText(string content)
    {
        if (this.mText == null)
        {
            this.mText = base.GetComponent<Text>();
        }
        this.timer = 0f;
        this.speed = 1f / this.duaration;
        this.words = content;
        this.mText.text = string.Empty;
        this.charsPerSecond = Mathf.Max(1, this.charsPerSecond);
    }

    private void Update()
    {
        int num = (int)((float)this.charsPerSecond * this.timer * this.speed);
        if (num > this.words.Length)
        {
            this.timer = 0f;
            return;
        }
        this.mText.text = this.words.Substring(0, num);
        this.timer += Time.deltaTime;
    }

    public int charsPerSecond;

    public float duaration = 0.3f;

    private float timer;

    private float speed;

    private string words;

    private Text mText;
}
