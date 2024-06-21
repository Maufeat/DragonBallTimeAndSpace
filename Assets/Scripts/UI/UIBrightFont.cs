using System;
using UnityEngine;
using UnityEngine.UI;

public class UIBrightFont : MonoBehaviour
{
    private void Start()
    {
        if (base.gameObject.GetComponent<Text>())
        {
            Text component = base.gameObject.GetComponent<Text>();
            component.material = new Material(Shader.Find("UI/UITextBright"));
            component.material.SetFloat("_Bright", this.Brightness);
            component.material.SetColor("_Color", this.FontColor);
        }
        else if (base.gameObject.GetComponent<Image>())
        {
            Image component2 = base.gameObject.GetComponent<Image>();
            component2.material = new Material(Shader.Find("UI/UITextBright"));
            component2.material.SetFloat("_Bright", this.Brightness);
            component2.material.SetColor("_Color", this.FontColor);
        }
    }

    public Color FontColor;

    public float Brightness;
}
