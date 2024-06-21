using System;
using System.Collections.Generic;
using UnityEngine;

public class ProFlareAtlas : MonoBehaviour
{
    public void UpdateElementNameList()
    {
        if (this.elementsList != null)
        {
            this.elementNameList = new string[this.elementsList.Count];
            for (int i = 0; i < this.elementNameList.Length; i++)
            {
                this.elementNameList[i] = this.elementsList[i].name;
            }
        }
    }

    public Texture2D texture;

    public int elementNumber;

    public bool editElements;

    [SerializeField]
    public List<ProFlareAtlas.Element> elementsList = new List<ProFlareAtlas.Element>();

    public string[] elementNameList;

    [Serializable]
    public class Element
    {
        public string name = "Flare Element";

        public Rect UV = new Rect(0f, 0f, 1f, 1f);

        public bool Imported;
    }
}
