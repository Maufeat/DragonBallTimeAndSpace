using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMouseFllow : MonoBehaviour
{
    private void Start()
    {
        this.mCanvas = UIManager.FindInParents<Canvas>(base.gameObject);
    }

    private void OnEnable()
    {
        this.img = base.GetComponent<Image>();
        this.img.color = new Color(0f, 0f, 0f, 0f);
    }

    private void Update()
    {
        Vector3 vector;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.mCanvas.transform as RectTransform, Input.mousePosition, this.mCanvas.worldCamera, out vector))
        {
            base.transform.position = new Vector2(vector.x, vector.y);
            if (this.img != null)
            {
                this.img.color = Color.white;
            }
        }
    }

    private Canvas mCanvas;

    private Image img;
}
