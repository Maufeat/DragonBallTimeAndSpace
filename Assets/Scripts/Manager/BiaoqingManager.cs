using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Framework.Base;
using UnityEngine;
using UnityEngine.UI;

public class BiaoqingManager : IManager
{
    public void Awake()
    {
        this.LoadImg();
        this.LoadImgXml();
        this.LoadAniXml();
        this.LoadXml();
    }

    private void LoadImg()
    {
        this.DoLoadImg(string.Empty);
        this.DoLoadImg("2");
        this.DoLoadImg("3");
    }

    private void DoLoadImg(string index)
    {
        Texture2D value = Resources.Load("Biaoqing\\img\\biaoqing" + index) as Texture2D;
        this.imgCache.Add(index, value);
    }

    private void LoadImgXml()
    {
        this.DoLoadImgXml(string.Empty);
        this.DoLoadImgXml("2");
        this.DoLoadImgXml("3");
    }

    private void DoLoadImgXml(string index)
    {
        TextAsset textAsset = Resources.Load("Biaoqing\\imageset\\biaoqing" + index) as TextAsset;
        byte[] bytes = textAsset.bytes;
        MemoryStream inStream = new MemoryStream(bytes);
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(inStream);
        XmlElement documentElement = xmlDocument.DocumentElement;
        XmlNodeList elementsByTagName = documentElement.GetElementsByTagName("Image");
        foreach (object obj in elementsByTagName)
        {
            XmlElement xmlElement = (XmlElement)obj;
            BiaoqingManager.ImgData value = default(BiaoqingManager.ImgData);
            string attribute = xmlElement.GetAttribute("Name");
            value.XPos = int.Parse(xmlElement.GetAttribute("XPos"));
            value.YPos = int.Parse(xmlElement.GetAttribute("YPos"));
            value.Width = int.Parse(xmlElement.GetAttribute("Width"));
            value.Height = int.Parse(xmlElement.GetAttribute("Height"));
            this.imgDataDic.Add(attribute, value);
        }
    }

    private void LoadAniXml()
    {
        TextAsset textAsset = Resources.Load("Biaoqing\\xml\\imageanimation") as TextAsset;
        byte[] bytes = textAsset.bytes;
        MemoryStream inStream = new MemoryStream(bytes);
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(inStream);
        XmlElement documentElement = xmlDocument.DocumentElement;
        XmlNodeList elementsByTagName = documentElement.GetElementsByTagName("ImageAnimation");
        foreach (object obj in elementsByTagName)
        {
            XmlElement xmlElement = (XmlElement)obj;
            string attribute = xmlElement.GetAttribute("Name");
            if (attribute.StartsWith("bq_"))
            {
                List<BiaoqingManager.AnimData> list = new List<BiaoqingManager.AnimData>();
                XmlNodeList elementsByTagName2 = xmlElement.GetElementsByTagName("Image");
                foreach (object obj2 in elementsByTagName2)
                {
                    XmlElement xmlElement2 = (XmlElement)obj2;
                    string text = xmlElement2.GetAttribute("Name");
                    text = text.Replace("set:biaoqing image:", string.Empty).Replace("set:biaoqing2 image:", string.Empty).Replace("set:biaoqing3 image:", string.Empty);
                    list.Add(new BiaoqingManager.AnimData
                    {
                        name = text,
                        time = int.Parse(xmlElement2.GetAttribute("Time"))
                    });
                }
                this.animDic.Add(attribute, list);
            }
        }
    }

    public string ManagerName
    {
        get
        {
            return base.GetType().ToString();
        }
    }

    private void LoadXml()
    {
        this.DoLoadXml(string.Empty);
        this.DoLoadXml("2");
        this.DoLoadXml("3");
    }

    private void DoLoadXml(string index)
    {
        List<BiaoqingManager.ImageData> list = new List<BiaoqingManager.ImageData>();
        TextAsset textAsset = Resources.Load("Biaoqing\\xml\\biaoqing" + index) as TextAsset;
        byte[] bytes = textAsset.bytes;
        MemoryStream inStream = new MemoryStream(bytes);
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(inStream);
        XmlElement documentElement = xmlDocument.DocumentElement;
        XmlNodeList elementsByTagName = documentElement.GetElementsByTagName("bq");
        foreach (object obj in elementsByTagName)
        {
            XmlNode xmlNode = (XmlNode)obj;
            BiaoqingManager.ImageData imageData = new BiaoqingManager.ImageData();
            string attribute = ((XmlElement)xmlNode).GetAttribute("name");
            imageData.name = attribute;
            imageData.imgs = new List<Sprite>();
            if (this.animDic.ContainsKey(attribute))
            {
                List<BiaoqingManager.AnimData> list2 = this.animDic[attribute];
                for (int i = 0; i < list2.Count; i++)
                {
                    string name = list2[i].name;
                    imageData.Time = list2[i].time;
                    if (this.imgDataDic.ContainsKey(name))
                    {
                        BiaoqingManager.ImgData imgData = this.imgDataDic[name];
                        Texture2D texture2D = this.imgCache[index];
                        int num = imgData.XPos;
                        int num2 = texture2D.height - imgData.YPos - imgData.Height;
                        if (num > texture2D.width)
                        {
                            num = texture2D.width;
                        }
                        if (num2 > texture2D.height)
                        {
                            num2 = texture2D.height;
                        }
                        int num3 = imgData.Width;
                        if (num + num3 > texture2D.width)
                        {
                            num3 = texture2D.width - num;
                        }
                        int num4 = imgData.Height;
                        if (num2 + num4 > texture2D.height)
                        {
                            num4 = texture2D.height - num2;
                        }
                        Sprite sprite = Sprite.Create(texture2D, new Rect((float)num, (float)num2, (float)num3, (float)num4), new Vector2(0f, 0f));
                        sprite.name = i.ToString();
                        imageData.imgs.Add(sprite);
                    }
                }
            }
            list.Add(imageData);
            if (!this.oneDataDic.ContainsKey(attribute))
            {
                this.oneDataDic.Add(attribute, imageData);
            }
            else
            {
                Debug.LogError(index + ",same name:" + attribute);
            }
        }
        this.allDataDic.Add((!(index == string.Empty)) ? int.Parse(index) : 1, list);
    }

    public List<BiaoqingManager.ImageData> GetImages(int index)
    {
        return this.allDataDic[index];
    }

    public BiaoqingManager.ImageData GetImage(string name)
    {
        if (this.oneDataDic.ContainsKey(name))
        {
            return this.oneDataDic[name];
        }
        return null;
    }

    public void RegisterLoopImg(BiaoqingManager.ImageData data, Image img)
    {
        data.useTime = (float)data.Time / 1000f;
        this.cacheImageDataDic[data.name] = data;
        this.cacheImageDic[data.name] = img;
    }

    public void OnUpdate()
    {
    }

    public void OnReSet()
    {
    }

    private Dictionary<string, Texture2D> imgCache = new Dictionary<string, Texture2D>();

    private Dictionary<string, BiaoqingManager.ImgData> imgDataDic = new Dictionary<string, BiaoqingManager.ImgData>();

    private Dictionary<string, List<BiaoqingManager.AnimData>> animDic = new Dictionary<string, List<BiaoqingManager.AnimData>>();

    private Dictionary<int, List<BiaoqingManager.ImageData>> allDataDic = new Dictionary<int, List<BiaoqingManager.ImageData>>();

    private Dictionary<string, BiaoqingManager.ImageData> oneDataDic = new Dictionary<string, BiaoqingManager.ImageData>();

    private Dictionary<string, BiaoqingManager.ImageData> cacheImageDataDic = new Dictionary<string, BiaoqingManager.ImageData>();

    private Dictionary<string, Image> cacheImageDic = new Dictionary<string, Image>();

    public class ImageData
    {
        public string name;

        public List<Sprite> imgs;

        public int Time;

        public float useTime;
    }

    private struct AnimData
    {
        public int time;

        public string name;
    }

    private struct ImgData
    {
        public int XPos;

        public int YPos;

        public int Width;

        public int Height;
    }
}
