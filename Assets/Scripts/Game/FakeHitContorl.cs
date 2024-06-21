using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using magic;
using msg;
using UnityEngine;

public class FakeHitContorl : IFFComponent
{
    public CompnentState State { get; set; }

    private void GetOtherComponent()
    {
        if (this.FFBC == null)
        {
            this.FFBC = this.Owmner.GetComponent<FFBehaviourControl>();
        }
        if (this.EffCtrl == null)
        {
            this.EffCtrl = this.Owmner.GetComponent<FFEffectControl>();
        }
        if (this.MatEffCtrl == null)
        {
            this.MatEffCtrl = this.Owmner.GetComponent<FFMaterialEffectControl>();
        }
    }

    public void GetHit(MSG_Ret_MagicAttack_SC mdata)
    {
        this.GetOtherComponent();
        LuaTable luaTable = ManagerCenter.Instance.GetManager<SkillManager>().Gett_skill_stage_config(mdata.skillstage);
        FFActionClip ffactionClip;
        if (luaTable != null)
        {
            CharactorBase charactorByID = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(mdata.att);
            if (charactorByID == null)
            {
                return;
            }
            string text = string.Empty;
            if (charactorByID != null)
            {
                text = charactorByID.animatorControllerName;
            }
            else if (FakeHitContorl.CareerSkillMap.ContainsKey(luaTable.GetField_Uint("skillid")))
            {
                text = FakeHitContorl.CareerSkillMap[luaTable.GetField_Uint("skillid")];
            }
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            int selectindex = (!charactorByID.IsFly) ? 0 : 1;
            ffactionClip = ManagerCenter.Instance.GetManager<FFActionClipManager>().GetFFActionClip(text, luaTable.GetField_Uint("ActionID"), selectindex);
            if (ffactionClip == null)
            {
                return;
            }
            FlyObjControl component = charactorByID.GetComponent<FlyObjControl>();
            if (component != null)
            {
                component.AddFlyObjArray(ffactionClip.GetEffectsByGroupID(FFActionClip.EffectType.Type_Fly, 1U), this.Owmner.ModelObj.transform, FlyObjConfig.LaunchType.ByHit);
            }
            FFEffectControl component2 = charactorByID.GetComponent<FFEffectControl>();
            if (component2 != null)
            {
                component2.SetTarget(ffactionClip.ClipName, this.Owmner.ModelObj.transform.position, this.Owmner.ModelObj);
            }
        }
        else
        {
            FFDebug.LogWarning(this, "mdata.skillstage:" + mdata.skillstage + " 不存在");
            ffactionClip = ManagerCenter.Instance.GetManager<FFActionClipManager>().GetFFActionClipArr(string.Empty, 0U)[0];
        }
        List<PKResult> list = this.PickMyPKResult(mdata);
        if (list.Count == 0)
        {
            return;
        }
        if (list.Count == 1)
        {
            PKResult pkr = list[0];
            this.DisplayerHit(ffactionClip, mdata, pkr);
        }
        else
        {
            this.DisplayerHit(ffactionClip, mdata, list);
        }
    }

    private List<PKResult> PickMyPKResult(MSG_Ret_MagicAttack_SC mdata)
    {
        this.PKResultTmpList.Clear();
        for (int i = 0; i < mdata.pklist.Count; i++)
        {
            PKResult pkresult = mdata.pklist[i];
            if (this.Owmner.EID.Equals(pkresult.def))
            {
                this.PKResultTmpList.Add(pkresult);
            }
        }
        return this.PKResultTmpList;
    }

    private void DisplayerHit(FFActionClip clip, MSG_Ret_MagicAttack_SC mdata, PKResult pkr)
    {
        this._isShowHitAnim = this.IsShowHitAnim(pkr.changehp);
        if (clip.FakeAttackTimeFs == null || clip.FakeAttackTimeFs.Length == 0 || pkr.attcode.Contains(ATTACKRESULT.ATTACKRESULT_MISS))
        {
            this.ShowHit(new OneSkillHitResult
            {
                Att = new EntitiesID(mdata.att),
                Hp = pkr.hp,
                HpChange = pkr.changehp,
                AttcodeList = pkr.attcode,
                Config = ManagerCenter.Instance.GetManager<SkillManager>().Gett_skill_stage_config(mdata.skillstage),
                Skillclip = clip
            }, this._isShowHitAnim);
        }
        else
        {
            int num;
            int num2;
            int[] array = this.SpritNum(pkr.changehp, clip.FakeAttackTimeFs.Length, out num, out num2);
            int num3 = 0;
            for (int i = 0; i < array.Length; i++)
            {
                uint num4 = clip.FakeAttackTimeFs[i];
                float delay = 0f;
                if (num4 > clip.AttackTimeF)
                {
                    delay = (num4 - clip.AttackTimeF) / 30f;
                }
                OneSkillHitResult oneSkillHitResult = new OneSkillHitResult();
                oneSkillHitResult.Delay = delay;
                oneSkillHitResult.Att = new EntitiesID(mdata.att);
                num3 += array[i];
                oneSkillHitResult.Hp = (uint)((ulong)pkr.hp + (ulong)((long)pkr.changehp) - (ulong)((long)num3));
                oneSkillHitResult.HpChange = array[i];
                oneSkillHitResult.Config = ManagerCenter.Instance.GetManager<SkillManager>().Gett_skill_stage_config(mdata.skillstage);
                oneSkillHitResult.Skillclip = clip;
                oneSkillHitResult.AttcodeList = new List<ATTACKRESULT>();
                if (i == num)
                {
                    if (pkr.attcode.Contains(ATTACKRESULT.ATTACKRESULT_BANG))
                    {
                        oneSkillHitResult.AttcodeList.Add(ATTACKRESULT.ATTACKRESULT_BANG);
                    }
                }
                else if (i == num2)
                {
                    if (pkr.attcode.Contains(ATTACKRESULT.ATTACKRESULT_HOLD))
                    {
                        oneSkillHitResult.AttcodeList.Add(ATTACKRESULT.ATTACKRESULT_HOLD);
                    }
                    if (pkr.attcode.Contains(ATTACKRESULT.ATTACKRESULT_BLOCK))
                    {
                        oneSkillHitResult.AttcodeList.Add(ATTACKRESULT.ATTACKRESULT_BLOCK);
                    }
                    if (pkr.attcode.Contains(ATTACKRESULT.ATTACKRESULT_DEFLECT))
                    {
                        oneSkillHitResult.AttcodeList.Add(ATTACKRESULT.ATTACKRESULT_DEFLECT);
                    }
                }
                if (oneSkillHitResult.AttcodeList.Count == 0)
                {
                    oneSkillHitResult.AttcodeList.Add(ATTACKRESULT.ATTACKRESULT_NORMAL);
                }
                this.OneSkillHitResultList.Add(oneSkillHitResult);
            }
            FFDebug.Log(this, FFLogType.Skill, string.Format("FakeAttackTimeFs: {0} pkr.changehp: {1}", clip.FakeAttackTimeFs.Length, pkr.changehp));
        }
    }

    private void DisplayerHit(FFActionClip clip, MSG_Ret_MagicAttack_SC mdata, List<PKResult> PKResultList)
    {
        this.OneSkillHitResultTmpList.Clear();
        if (clip.FakeAttackTimeFs != null && clip.FakeAttackTimeFs.Length > 0)
        {
            int num = (clip.FakeAttackTimeFs.Length > PKResultList.Count) ? PKResultList.Count : clip.FakeAttackTimeFs.Length;
            for (int i = 0; i < num; i++)
            {
                uint num2 = clip.FakeAttackTimeFs[i];
                float delay = 0f;
                if (num2 > clip.AttackTimeF)
                {
                    delay = (num2 - clip.AttackTimeF) / 30f;
                }
                OneSkillHitResult oneSkillHitResult = new OneSkillHitResult();
                oneSkillHitResult.Delay = delay;
                oneSkillHitResult.Att = new EntitiesID(mdata.att);
                oneSkillHitResult.Hp = 0U;
                oneSkillHitResult.HpChange = 0;
                oneSkillHitResult.AttcodeList = new List<ATTACKRESULT>();
                oneSkillHitResult.Config = ManagerCenter.Instance.GetManager<SkillManager>().Gett_skill_stage_config(mdata.skillstage);
                oneSkillHitResult.Skillclip = clip;
                this.OneSkillHitResultTmpList.Add(oneSkillHitResult);
            }
        }
        else
        {
            OneSkillHitResult oneSkillHitResult2 = new OneSkillHitResult();
            oneSkillHitResult2.Att = new EntitiesID(mdata.att);
            oneSkillHitResult2.Hp = 0U;
            oneSkillHitResult2.HpChange = 0;
            oneSkillHitResult2.AttcodeList = new List<ATTACKRESULT>();
            oneSkillHitResult2.Config = ManagerCenter.Instance.GetManager<SkillManager>().Gett_skill_stage_config(mdata.skillstage);
            oneSkillHitResult2.Skillclip = clip;
            this.OneSkillHitResultTmpList.Add(oneSkillHitResult2);
        }
        for (int j = 0; j < PKResultList.Count; j++)
        {
            int index = (j >= this.OneSkillHitResultTmpList.Count) ? (this.OneSkillHitResultTmpList.Count - 1) : j;
            OneSkillHitResult oneSkillHitResult3 = this.OneSkillHitResultTmpList[index];
            PKResult pkresult = PKResultList[j];
            oneSkillHitResult3.Hp = pkresult.hp;
            oneSkillHitResult3.HpChange += pkresult.changehp;
            oneSkillHitResult3.AttcodeList.AddRange(pkresult.attcode);
        }
        this.OneSkillHitResultList.AddRange(this.OneSkillHitResultTmpList);
    }

    private int[] SpritNum(int Total, int count, out int HightIndex, out int LowIndex)
    {
        this.HpChangeTmpList.Clear();
        HightIndex = 0;
        LowIndex = 0;
        int num = 0;
        int num2 = Total / count;
        int num3 = num2 / 4;
        int num4 = 0;
        for (int i = 0; i < count; i++)
        {
            int num5 = UnityEngine.Random.Range(-num3, num3);
            int num6 = num2 + num5 - num;
            num = num5;
            if (i > 0)
            {
                if (num6 > this.HpChangeTmpList[HightIndex])
                {
                    HightIndex = i;
                }
                if (num6 < this.HpChangeTmpList[LowIndex])
                {
                    LowIndex = i;
                }
            }
            num4 += num6;
            this.HpChangeTmpList.Add(num6);
        }
        int num7 = UnityEngine.Random.Range(0, count - 1);
        List<int> hpChangeTmpList;
        List<int> list = hpChangeTmpList = this.HpChangeTmpList;
        int num8;
        int index = num8 = num7;
        num8 = hpChangeTmpList[num8];
        list[index] = num8 + (Total - num4);
        if (this.HpChangeTmpList[num7] > this.HpChangeTmpList[HightIndex])
        {
            HightIndex = num7;
        }
        if (this.HpChangeTmpList[num7] < this.HpChangeTmpList[LowIndex])
        {
            LowIndex = num7;
        }
        return this.HpChangeTmpList.ToArray();
    }

    public void ResetHpByServerPush(MSG_RetHpMpToSelects_SC msgb)
    {
        if (this.Owmner.hpdata != null)
        {
            this.Owmner.hpdata.ResetHp(msgb.curhp, msgb.maxhp);
        }
        this.Owmner.BaseData.SetHp(msgb.curhp);
    }

    private void ShowHit(OneSkillHitResult HitResult, bool isShowHitAnim)
    {
        if (HitResult.HpChange < 0)
        {
            if (this.Owmner.hpdata != null)
            {
                this.Owmner.hpdata.RevertOrBleedChangeHp(HitResult.Hp, (float)HitResult.HpChange, HitResult.AttcodeList, HitResult.Att == MainPlayer.Self.EID);
            }
            this.Owmner.BaseData.SetHp(HitResult.Hp);
        }
        else if (this.Owmner.hpdata != null)
        {
            this.Owmner.hpdata.HitChangeHp(this.Owmner.EID, HitResult);
        }
        CharactorBase charactorByID = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(HitResult.Att);
        if (HitResult.HpChange == 0 && HitResult.AttcodeList.Contains(ATTACKRESULT.ATTACKRESULT_MISS))
        {
            return;
        }
        if (this.FFBC != null && charactorByID != null && isShowHitAnim)
        {
            this.FFBC.GetHit(charactorByID);
        }
        if (this.EffCtrl != null && HitResult.Skillclip != null)
        {
            this.EffCtrl.AddEffect(HitResult.Skillclip.GetEffectsByGroupID(FFActionClip.EffectType.Type_Hit, 1U), null, null, false);
        }
        if (this.MatEffCtrl != null && HitResult.Skillclip != null)
        {
            this.MatEffCtrl.AddEffect(HitResult.Skillclip.HitMaterialEffects);
        }
    }

    public void PlayAudio(Transform tran, string BindName)
    {
    }

    private bool IsShowHitAnim(int hpChange)
    {
        bool result = false;
        if (hpChange >= 0)
        {
            return result;
        }
        return this.Owmner != null && this.Owmner.IsShowHitAnim(hpChange);
    }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.Owmner = Mgr.Owner;
        this.InitCareerSkillMap();
    }

    private void InitCareerSkillMap()
    {
        if (FakeHitContorl.CareerSkillMap != null)
        {
            return;
        }
        FakeHitContorl.CareerSkillMap = new Dictionary<uint, string>();
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("careerskill").GetCacheField_Table("career_1");
        IEnumerator enumerator = cacheField_Table.Values.GetEnumerator();
        enumerator.Reset();
        while (enumerator.MoveNext())
        {
            object obj = enumerator.Current;
            LuaTable luaTable = obj as LuaTable;
            string field_String = luaTable.GetField_String("skillset");
            string[] array = field_String.Split(new char[]
            {
                '-'
            }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < array.Length; i++)
            {
                uint key = 0U;
                if (uint.TryParse(array[i], out key))
                {
                    FakeHitContorl.CareerSkillMap[key] = "sm01";
                }
            }
        }
        LuaTable cacheField_Table2 = LuaConfigManager.GetXmlConfigTable("careerskill").GetCacheField_Table("career_2");
        IEnumerator enumerator2 = cacheField_Table2.Values.GetEnumerator();
        enumerator2.Reset();
        while (enumerator2.MoveNext())
        {
            object obj2 = enumerator2.Current;
            LuaTable luaTable2 = obj2 as LuaTable;
            string field_String2 = luaTable2.GetField_String("skillset");
            string[] array2 = field_String2.Split(new char[]
            {
                '-'
            }, StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < array2.Length; j++)
            {
                uint key2 = 0U;
                if (uint.TryParse(array2[j], out key2))
                {
                    FakeHitContorl.CareerSkillMap[key2] = "as01";
                }
            }
        }
        LuaTable cacheField_Table3 = LuaConfigManager.GetXmlConfigTable("careerskill").GetCacheField_Table("career_3");
        IEnumerator enumerator3 = cacheField_Table3.Values.GetEnumerator();
        enumerator3.Reset();
        while (enumerator3.MoveNext())
        {
            object obj3 = enumerator3.Current;
            LuaTable luaTable3 = obj3 as LuaTable;
            string field_String3 = luaTable3.GetField_String("skillset");
            string[] array3 = field_String3.Split(new char[]
            {
                '-'
            }, StringSplitOptions.RemoveEmptyEntries);
            for (int k = 0; k < array3.Length; k++)
            {
                uint key3 = 0U;
                if (uint.TryParse(array3[k], out key3))
                {
                    FakeHitContorl.CareerSkillMap[key3] = "wz01";
                }
            }
        }
        LuaTable cacheField_Table4 = LuaConfigManager.GetXmlConfigTable("careerskill").GetCacheField_Table("career_4");
        IEnumerator enumerator4 = cacheField_Table4.Values.GetEnumerator();
        enumerator4.Reset();
        while (enumerator4.MoveNext())
        {
            object obj4 = enumerator4.Current;
            LuaTable luaTable4 = obj4 as LuaTable;
            string field_String4 = luaTable4.GetField_String("skillset");
            string[] array4 = field_String4.Split(new char[]
            {
                '-'
            }, StringSplitOptions.RemoveEmptyEntries);
            for (int l = 0; l < array4.Length; l++)
            {
                uint key4 = 0U;
                if (uint.TryParse(array4[l], out key4))
                {
                    FakeHitContorl.CareerSkillMap[key4] = "gm01";
                }
            }
        }
    }

    public void CompDispose()
    {
    }

    public void CompUpdate()
    {
        for (int i = 0; i < this.OneSkillHitResultList.Count; i++)
        {
            OneSkillHitResult oneSkillHitResult = this.OneSkillHitResultList[i];
            oneSkillHitResult.Delay -= Time.deltaTime;
            if (oneSkillHitResult.Delay <= 0f)
            {
                this.RemoveList.Add(oneSkillHitResult);
                this._isShowHitAnim = this.IsShowHitAnim(oneSkillHitResult.HpChange);
                this.ShowHit(oneSkillHitResult, this._isShowHitAnim);
            }
        }
        for (int j = 0; j < this.RemoveList.Count; j++)
        {
            this.OneSkillHitResultList.Remove(this.RemoveList[j]);
        }
        this.RemoveList.Clear();
    }

    public void ResetComp()
    {
    }

    private static Dictionary<uint, string> CareerSkillMap;

    private FFBehaviourControl FFBC;

    private FFEffectControl EffCtrl;

    private FFMaterialEffectControl MatEffCtrl;

    private List<PKResult> PKResultTmpList = new List<PKResult>();

    private List<OneSkillHitResult> OneSkillHitResultTmpList = new List<OneSkillHitResult>();

    private List<int> HpChangeTmpList = new List<int>();

    private List<OneSkillHitResult> OneSkillHitResultList = new List<OneSkillHitResult>();

    private bool _isShowHitAnim;

    public CharactorBase Owmner;

    private List<OneSkillHitResult> RemoveList = new List<OneSkillHitResult>();
}
