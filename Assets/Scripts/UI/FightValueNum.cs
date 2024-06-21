using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.UI;

public class FightValueNum : MonoBehaviour
{
    public void SetNum(uint num)
    {
        GameObject gameObject = base.transform.GetChild(0).gameObject;
        UIManager.Instance.ClearListChildrens(base.transform, 1);
        string text = num.ToString();
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject.gameObject);
            Image img = gameObject2.GetComponent<Image>();
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("other", "ce" + c, delegate (Sprite sprite)
            {
                if (sprite != null && img != null)
                {
                    img.sprite = sprite;
                    img.overrideSprite = sprite;
                }
            });
            gameObject2.transform.SetParent(gameObject.transform.parent);
            gameObject2.transform.localScale = Vector3.one;
            gameObject2.SetActive(true);
        }
    }
}
