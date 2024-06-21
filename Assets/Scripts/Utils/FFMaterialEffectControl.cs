using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;
using UnityEngine.Rendering;

public class FFMaterialEffectControl : IFFComponent
{
    public CompnentState State { get; set; }

    public GameObject ModelObj
    {
        get
        {
            return this.FFCompMgr.Owner.ModelObj;
        }
    }

    public bool IsInInvisibilityBuffState { get; set; }

    public void SetRoleColor(string propertyName, Color color)
    {
        for (int i = 0; i < this._lstRoleMaterial.Count; i++)
        {
            Material material = this._lstRoleMaterial[i];
            if (material.HasProperty(propertyName))
            {
                string defaultListKey = this.GetDefaultListKey(material, propertyName);
                if (!this.DefaultRoleColorList.ContainsKey(defaultListKey))
                {
                    Color color2 = material.GetColor(propertyName);
                    this.DefaultRoleColorList.Add(defaultListKey, color2);
                }
                material.SetColor(propertyName, color);
            }
        }
    }

    public void SetRoleFloat(string propertyName, float value)
    {
        for (int i = 0; i < this._lstRoleMaterial.Count; i++)
        {
            Material material = this._lstRoleMaterial[i];
            if (material.HasProperty(propertyName))
            {
                string defaultListKey = this.GetDefaultListKey(material, propertyName);
                if (!this.DefaultRolefloatList.ContainsKey(defaultListKey))
                {
                    float @float = material.GetFloat(propertyName);
                    this.DefaultRolefloatList.Add(defaultListKey, @float);
                }
                material.SetFloat(propertyName, value);
            }
        }
    }

    public void SetRoleDefaultColor(string propertyName)
    {
        for (int i = 0; i < this._lstRoleMaterial.Count; i++)
        {
            Material material = this._lstRoleMaterial[i];
            if (material.HasProperty(propertyName))
            {
                string defaultListKey = this.GetDefaultListKey(material, propertyName);
                if (this.DefaultRoleColorList.ContainsKey(defaultListKey))
                {
                    Color color = this.DefaultRoleColorList[defaultListKey];
                    material.SetColor(propertyName, color);
                }
            }
        }
    }

    public void SetRoleDefaultFloat(string propertyName)
    {
        for (int i = 0; i < this._lstRoleMaterial.Count; i++)
        {
            Material material = this._lstRoleMaterial[i];
            if (material.HasProperty(propertyName))
            {
                string defaultListKey = this.GetDefaultListKey(material, propertyName);
                if (this.DefaultRolefloatList.ContainsKey(defaultListKey))
                {
                    float value = this.DefaultRolefloatList[defaultListKey];
                    material.SetFloat(propertyName, value);
                }
            }
        }
    }

    public void SetWeaponColor(string strPropertyName, Color color)
    {
        for (int i = 0; i < this._lstWeaponMaterial.Count; i++)
        {
            Material material = this._lstWeaponMaterial[i];
            if (material.HasProperty(strPropertyName))
            {
                string defaultListKey = this.GetDefaultListKey(material, strPropertyName);
                if (!this._lstDefaultWeaponColor.ContainsKey(defaultListKey))
                {
                    Color color2 = material.GetColor(strPropertyName);
                    this._lstDefaultWeaponColor.Add(defaultListKey, color2);
                }
                material.SetColor(strPropertyName, color);
            }
        }
    }

    public void SetWeaponFloat(string strPropertyName, float fValue)
    {
        for (int i = 0; i < this._lstWeaponMaterial.Count; i++)
        {
            Material material = this._lstWeaponMaterial[i];
            if (material.HasProperty(strPropertyName))
            {
                string defaultListKey = this.GetDefaultListKey(material, strPropertyName);
                if (!this._lstDefaultWeaponfloat.ContainsKey(defaultListKey))
                {
                    float @float = material.GetFloat(strPropertyName);
                    this._lstDefaultWeaponfloat.Add(defaultListKey, @float);
                }
                material.SetFloat(strPropertyName, fValue);
            }
        }
    }

    public void SetWeaponDefaultColor(string strPropertyName)
    {
        for (int i = 0; i < this._lstWeaponMaterial.Count; i++)
        {
            Material material = this._lstWeaponMaterial[i];
            if (material.HasProperty(strPropertyName))
            {
                string defaultListKey = this.GetDefaultListKey(material, strPropertyName);
                if (this._lstDefaultWeaponColor.ContainsKey(defaultListKey))
                {
                    Color color = this._lstDefaultWeaponColor[defaultListKey];
                    material.SetColor(strPropertyName, color);
                }
            }
        }
    }

    public void SetWeaponDefaultFloat(string strPropertyName)
    {
        for (int i = 0; i < this._lstWeaponMaterial.Count; i++)
        {
            Material material = this._lstWeaponMaterial[i];
            if (material.HasProperty(strPropertyName))
            {
                string defaultListKey = this.GetDefaultListKey(material, strPropertyName);
                if (this._lstDefaultWeaponfloat.ContainsKey(defaultListKey))
                {
                    float value = this._lstDefaultWeaponfloat[defaultListKey];
                    material.SetFloat(strPropertyName, value);
                }
            }
        }
    }

    private string GetDefaultListKey(Material mat, string propertyName)
    {
        return mat.GetInstanceID() + "*" + propertyName;
    }

    public void AddEffect(FFMateffect eff)
    {
        eff.Control = this;
        eff.mState = FFMateffect.State.Dalay;
        this.FFMateffectList.Insert(0, eff);
    }

    public void AddEffect(string[] EffectNameArray)
    {
        if (EffectNameArray == null)
        {
            return;
        }
        foreach (string text in EffectNameArray)
        {
            FFDebug.Log(this, FFLogType.Effect, "effname----->" + text);
            FFMaterialAnimClip clip = ManagerCenter.Instance.GetManager<FFMaterialEffectManager>().GetClip(text);
            if (clip != null)
            {
                this.AddEffect(new FFMateffect(clip));
            }
        }
    }

    public void RemoveEffect(FFMateffect eff)
    {
        eff.Control = this;
        eff.mState = FFMateffect.State.Over;
        this.FFMateffectList.Remove(eff);
    }

    private void SetRoleToDefault()
    {
        for (int i = 0; i < this._lstRoleRendererMatRecord.Count; i++)
        {
            this._lstRoleRendererMatRecord[i].BackToDefault();
        }
        this._lstRoleRendererMatRecord.Clear();
    }

    private void SetWeaponToDefault()
    {
        for (int i = 0; i < this._lstWeaponRendererMatRecord.Count; i++)
        {
            this._lstWeaponRendererMatRecord[i].BackToDefault();
        }
        this._lstWeaponRendererMatRecord.Clear();
    }

    private void SetAllToDefault()
    {
        this.SetRoleToDefault();
        this.SetWeaponToDefault();
    }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.FFCompMgr = Mgr;
        if (this.ModelObj == null)
        {
            FFDebug.LogError(this, "FFMaterialEffectControl error : ModelObj null");
            return;
        }
        Renderer[] componentsInChildren = this.ModelObj.GetComponentsInChildren<Renderer>(true);
        List<Material> list = new List<Material>();
        foreach (Renderer renderer in componentsInChildren)
        {
            FFMaterialEffectControl.RendererMatRecord rendererMatRecord = new FFMaterialEffectControl.RendererMatRecord(renderer);
            rendererMatRecord.RecordDefault();
            list.AddRange(renderer.materials);
            this._lstRoleRendererMatRecord.Add(rendererMatRecord);
        }
        if (list.Count != 0)
        {
            this._lstRoleMaterial.AddRange(list.ToArray());
        }
    }

    public void ResetAllMaterialData()
    {
        this.ClearRoleMaterial();
        this.ClearWeaponMaterial();
        this.ResetRoleMaterial();
        this.ResetWeaponMaterial();
    }

    public void ResetRoleMaterial()
    {
        if (null == this.ModelObj)
        {
            FFDebug.LogError(this, "FFMaterialEffectControl error : ModelObj null");
            return;
        }
        Renderer[] componentsInChildren = this.ModelObj.GetComponentsInChildren<Renderer>(true);
        List<Material> list = new List<Material>();
        foreach (Renderer renderer in componentsInChildren)
        {
            FFMaterialEffectControl.RendererMatRecord rendererMatRecord = new FFMaterialEffectControl.RendererMatRecord(renderer);
            rendererMatRecord.RecordDefault();
            List<Material> list2 = new List<Material>();
            for (int j = 0; j < renderer.materials.Length; j++)
            {
                list2.Add(UnityEngine.Object.Instantiate<Material>(renderer.materials[j]));
            }
            renderer.materials = list2.ToArray();
            list.AddRange(renderer.materials);
            this._lstRoleRendererMatRecord.Add(rendererMatRecord);
        }
        this._lstRoleMaterial.Clear();
        if (list.Count != 0)
        {
            this._lstRoleMaterial.AddRange(list.ToArray());
        }
    }

    public void ClearRoleMaterial()
    {
        this.SetRoleToDefault();
        this._lstRoleMaterial.Clear();
    }

    public void OnWeaponChange()
    {
        this.ResetWeaponMaterial();
        if (this.IsInInvisibilityBuffState)
        {
            this.SetWeaponFloat("_Alpha", 0.2f);
        }
    }

    public void ResetWeaponMaterial()
    {
        if (this.FFCompMgr == null || this.FFCompMgr.Owner == null)
        {
            FFDebug.Log(this, FFLogType.Buff, "FFCompMgr is null");
            return;
        }
        FFWeaponHold component = this.FFCompMgr.Owner.GetComponent<FFWeaponHold>();
        if (component == null)
        {
            return;
        }
        GameObject[] allWeaponObjs = component.GetAllWeaponObjs();
        if (allWeaponObjs == null)
        {
            return;
        }
        if (allWeaponObjs.Length == 0)
        {
            FFDebug.Log(this, FFLogType.Buff, "No Weapon");
            return;
        }
        this.RendererList.Clear();
        foreach (GameObject gameObject in allWeaponObjs)
        {
            if (gameObject != null)
            {
                this.RendererList.AddRange(gameObject.GetComponentsInChildren<Renderer>(true));
            }
        }
        Renderer[] array = this.RendererList.ToArray();
        List<Material> list = new List<Material>();
        foreach (Renderer renderer in array)
        {
            FFMaterialEffectControl.RendererMatRecord rendererMatRecord = new FFMaterialEffectControl.RendererMatRecord(renderer);
            rendererMatRecord.RecordDefault();
            List<Material> list2 = new List<Material>();
            for (int k = 0; k < renderer.materials.Length; k++)
            {
                list2.Add(UnityEngine.Object.Instantiate<Material>(renderer.materials[k]));
            }
            renderer.materials = list2.ToArray();
            list.AddRange(renderer.materials);
            this._lstWeaponRendererMatRecord.Add(rendererMatRecord);
        }
        this._lstWeaponMaterial.AddRange(list.ToArray());
    }

    public void ClearWeaponMaterial()
    {
        this.SetWeaponToDefault();
        this._lstWeaponMaterial.Clear();
    }

    public void CloseCastShadow()
    {
        if (null != this.ModelObj)
        {
            SkinnedMeshRenderer[] componentsInChildren = this.ModelObj.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].shadowCastingMode = ShadowCastingMode.Off;
            }
        }
        FFWeaponHold component = this.FFCompMgr.Owner.GetComponent<FFWeaponHold>();
        if (component == null)
        {
            return;
        }
        GameObject[] allWeaponObjs = component.GetAllWeaponObjs();
        for (int j = 0; j < allWeaponObjs.Length; j++)
        {
            MeshRenderer component2 = allWeaponObjs[j].GetComponent<MeshRenderer>();
            if (null != component2)
            {
                component2.shadowCastingMode = ShadowCastingMode.Off;
            }
        }
    }

    public void OpenCastShadow()
    {
        if (null != this.ModelObj)
        {
            SkinnedMeshRenderer[] componentsInChildren = this.ModelObj.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].shadowCastingMode = ShadowCastingMode.On;
            }
        }
        FFWeaponHold component = this.FFCompMgr.Owner.GetComponent<FFWeaponHold>();
        if (component == null)
        {
            return;
        }
        GameObject[] allWeaponObjs = component.GetAllWeaponObjs();
        for (int j = 0; j < allWeaponObjs.Length; j++)
        {
            MeshRenderer component2 = allWeaponObjs[j].GetComponent<MeshRenderer>();
            if (null != component2)
            {
                component2.shadowCastingMode = ShadowCastingMode.On;
            }
        }
    }

    public void CompUpdate()
    {
        if (this.ModelObj == null)
        {
            return;
        }
        this.RemoveList.Clear();
        bool flag = false;
        for (int i = 0; i < this.FFMateffectList.Count; i++)
        {
            if (this.FFMateffectList[i].mState == FFMateffect.State.Over)
            {
                this.RemoveList.Add(this.FFMateffectList[i]);
            }
            else
            {
                if (this.FFMateffectList[i].mState == FFMateffect.State.Play)
                {
                    if (!flag)
                    {
                        this.FFMateffectList[i].CanPlayer = true;
                        flag = true;
                    }
                    else
                    {
                        this.FFMateffectList[i].CanPlayer = false;
                    }
                }
                this.FFMateffectList[i].update();
            }
        }
        for (int j = 0; j < this.RemoveList.Count; j++)
        {
            this.RemoveList[j].DisPose();
            this.FFMateffectList.Remove(this.RemoveList[j]);
        }
        this.RemoveList.Clear();
    }

    public void CompDispose()
    {
        this.SetAllToDefault();
    }

    public void ResetComp()
    {
    }

    private bool isInInvisibilityBuffState;

    private List<Material> _lstRoleMaterial = new List<Material>();

    private List<Material> _lstWeaponMaterial = new List<Material>();

    private BetterDictionary<string, Color> DefaultRoleColorList = new BetterDictionary<string, Color>();

    private BetterDictionary<string, float> DefaultRolefloatList = new BetterDictionary<string, float>();

    private BetterDictionary<string, Color> _lstDefaultWeaponColor = new BetterDictionary<string, Color>();

    private BetterDictionary<string, float> _lstDefaultWeaponfloat = new BetterDictionary<string, float>();

    private List<FFMateffect> FFMateffectList = new List<FFMateffect>();

    private List<FFMaterialEffectControl.RendererMatRecord> _lstRoleRendererMatRecord = new List<FFMaterialEffectControl.RendererMatRecord>();

    private List<FFMaterialEffectControl.RendererMatRecord> _lstWeaponRendererMatRecord = new List<FFMaterialEffectControl.RendererMatRecord>();

    private FFComponentMgr FFCompMgr;

    private List<Renderer> RendererList = new List<Renderer>();

    private List<FFMateffect> RemoveList = new List<FFMateffect>();

    private class RendererMatRecord
    {
        public RendererMatRecord(Renderer render)
        {
            this.Render = render;
        }

        public void RecordDefault()
        {
            this.MaterialsRecord = this.Render.materials;
        }

        public void BackToDefault()
        {
            if (null != this.Render)
            {
                this.Render.materials = this.MaterialsRecord;
            }
        }

        private Renderer Render;

        private Material[] MaterialsRecord;
    }
}
