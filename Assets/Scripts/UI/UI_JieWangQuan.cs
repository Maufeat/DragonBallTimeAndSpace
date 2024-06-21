using System;
using System.Collections;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class UI_JieWangQuan : UIPanelBase
{
    private JieWangQuanController mController
    {
        get
        {
            return ControllerManager.Instance.GetController<JieWangQuanController>();
        }
    }

    public override void AfterInit()
    {
        base.AfterInit();
    }

    public override void OnInit(Transform root)
    {
        this.GAME_START_TIP = CommonTools.GetTextById(865UL);
        this.GAME_STEP2_START_TIP = CommonTools.GetTextById(866UL);
        this.GAME_STEP3_START_TIP = CommonTools.GetTextById(867UL);
        this.GAME_FAIL = CommonTools.GetTextById(835UL);
        this.GAME_SUCCESS = CommonTools.GetTextById(868UL);
        this.transTipText = root.Find("Offset/Text");
        this.transTipPanel = root.Find("Offset/tips");
        this.transSliderPanel = root.Find("Offset/Slider");
        this.mSlider = this.transSliderPanel.GetComponent<Slider>();
        base.OnInit(root);
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public override void OnDispose()
    {
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
        base.OnDispose();
    }

    public void Start(int taskId)
    {
        this.mConfigData = new UI_JieWangQuan.JiewangquanData(taskId);
        this.ShowTipText(this.GAME_START_TIP);
        this.transTipPanel.gameObject.SetActive(false);
        this.transSliderPanel.gameObject.SetActive(false);
        Scheduler.Instance.AddTimer(2f, false, delegate
        {
            Transform transform = this.transSliderPanel.Find("IconPanel/icon");
            float num = (float)(this.mConfigData.rangestart + this.mConfigData.rangeend) / 2f;
            float num2 = num / 100f;
            float x = (transform.parent as RectTransform).sizeDelta.x;
            float x2 = x * (num2 - 0.5f);
            transform.transform.localPosition = new Vector3(x2, 0f, 0f);
            Text component = transform.Find("text_2").GetComponent<Text>();
            component.text = this.mConfigData.rangetext;
            this.transTipPanel.gameObject.SetActive(true);
            this.transSliderPanel.gameObject.SetActive(true);
        });
    }

    private void ShowTipText(string txt)
    {
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.HideTipText));
        this.transTipText.Find("txt").GetComponent<Text>().text = txt;
        this.transTipText.gameObject.SetActive(true);
        Scheduler.Instance.AddTimer(2f, false, new Scheduler.OnScheduler(this.HideTipText));
    }

    private void HideTipText()
    {
        this.transTipText.gameObject.SetActive(false);
    }

    private void Update()
    {
        switch (this.mCurState)
        {
            case UI_JieWangQuan.GameState.SliderPlayReady:
                this.SliderPlay();
                break;
            case UI_JieWangQuan.GameState.SliderPlaying:
                this.SliderPlaying();
                break;
            case UI_JieWangQuan.GameState.SliderWaitReady:
                this.SlierWait();
                break;
            case UI_JieWangQuan.GameState.SliderWaiting:
                this.SliderWaiting();
                break;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.SpaceKeyClick(this.mSlider.value);
        }
    }

    public void StartGame()
    {
        this.mCurStepIndex = 0U;
        this.mCurState = UI_JieWangQuan.GameState.SliderPlayReady;
    }

    private void SliderPlay()
    {
        this.mCurStepIndex += 1U;
        this.mSlider.value = 0f;
        this.mAleadyPlayTime = 0f;
        this.mClickYet = false;
        this.mClickSucc = false;
        this.mCurState = UI_JieWangQuan.GameState.SliderPlaying;
        FFBehaviourControl component = MainPlayer.Self.GetComponent<FFBehaviourControl>();
        component.PlayNormalAction(10689U, false, 0.1f);
    }

    private void SliderPlaying()
    {
        this.mAleadyPlayTime += Time.deltaTime;
        this.mSlider.value = this.mAleadyPlayTime / this.mConfigData.time * this.mSlider.maxValue;
        if (this.mSlider.value >= this.mSlider.maxValue)
        {
            if (this.mClickSucc)
            {
                this.mCurState = UI_JieWangQuan.GameState.SliderWaitReady;
            }
            else
            {
                this.mCurState = UI_JieWangQuan.GameState.None;
            }
            if (!this.mClickYet)
            {
                this.OperateResult(false);
                FFBehaviourControl component = MainPlayer.Self.GetComponent<FFBehaviourControl>();
                component.PlayNormalAction(1U, false, 0.1f);
            }
        }
    }

    private void SlierWait()
    {
        this.mAleadyWaitTime = 0f;
        this.mCurState = UI_JieWangQuan.GameState.SliderWaiting;
    }

    private void SliderWaiting()
    {
        this.mAleadyWaitTime += Time.deltaTime;
        if (this.mAleadyWaitTime >= (float)this.mConfigData.interval)
        {
            this.mCurState = UI_JieWangQuan.GameState.SliderPlayReady;
        }
    }

    private void SpaceKeyClick(float value)
    {
        if (this.mCurState == UI_JieWangQuan.GameState.SliderPlaying && !this.mClickYet)
        {
            this.mClickSucc = (value >= (float)this.mConfigData.rangestart && value <= (float)this.mConfigData.rangeend);
            this.OperateResult(this.mClickSucc);
            this.mClickYet = true;
            FFBehaviourControl behaveiorCtrl = MainPlayer.Self.GetComponent<FFBehaviourControl>();
            float time = behaveiorCtrl.PlayNormalAction(10690U, false, 0.1f);
            Scheduler.Instance.AddTimer(time, false, delegate
            {
                behaveiorCtrl.PlayNormalAction(1U, false, 0.1f);
            });
        }
    }

    private void OperateResult(bool succ)
    {
        this.mController.ReqOperateData(this.mCurStepIndex, succ, this.mConfigData.id);
        this.ProgressOperateResult(succ);
    }

    public void ProgressOperateResult(bool succ)
    {
        if (succ)
        {
            string txt = string.Empty;
            switch (this.mCurStepIndex)
            {
                case 1U:
                    txt = this.GAME_STEP2_START_TIP;
                    break;
                case 2U:
                    txt = this.GAME_STEP3_START_TIP;
                    break;
                case 3U:
                    txt = this.GAME_SUCCESS;
                    break;
            }
            this.ShowTipText(txt);
            if (this.mCurStepIndex >= 3U)
            {
                this.transTipPanel.gameObject.SetActive(false);
                this.transSliderPanel.gameObject.SetActive(false);
                this.mController.SetMoveState(true);
                this.mCurState = UI_JieWangQuan.GameState.None;
            }
        }
        else
        {
            this.ShowTipText(this.GAME_FAIL);
            ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox("提示", "是否重新开始界王拳挑战？", "重新挑战", "退出副本", UIManager.ParentType.Tips, new Action(this.GameRetry), new Action(this.GameExit), null);
        }
    }

    private void GameRetry()
    {
        this.mController.ReqGameRetry(this.mConfigData.id);
    }

    private void GameExit()
    {
        this.mController.ReqGameExit();
    }

    private Transform transTipText;

    private Transform transTipPanel;

    private Transform transSliderPanel;

    private Slider mSlider;

    private string GAME_START_TIP = string.Empty;

    private string GAME_STEP2_START_TIP = string.Empty;

    private string GAME_STEP3_START_TIP = string.Empty;

    private string GAME_FAIL = string.Empty;

    private string GAME_SUCCESS = string.Empty;

    private uint mCurStepIndex;

    private UI_JieWangQuan.GameState mCurState;

    private UI_JieWangQuan.JiewangquanData mConfigData;

    private float mAleadyPlayTime;

    private bool mClickYet;

    private bool mClickSucc;

    private float mAleadyWaitTime;

    private enum GameState
    {
        None,
        SliderPlayReady,
        SliderPlaying,
        SliderWaitReady,
        SliderWaiting
    }

    public class JiewangquanData
    {
        public JiewangquanData(int _id)
        {
            this.id = _id;
            LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("other").GetCacheField_Table("jiewanguan");
            IEnumerator enumerator = cacheField_Table.Values.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                object obj = enumerator.Current;
                LuaTable luaTable = obj as LuaTable;
                if (luaTable.GetCacheField_Int("id") == this.id)
                {
                    this.time = luaTable.GetCacheField_Float("time");
                    this.rangestart = luaTable.GetCacheField_Int("rangstart");
                    this.rangeend = luaTable.GetCacheField_Int("rangeend");
                    this.rangetext = luaTable.GetCacheField_String("rangetext");
                    this.interval = luaTable.GetCacheField_Int("interval");
                    break;
                }
            }
        }

        public int id;

        public float time;

        public int rangestart;

        public int rangeend;

        public string rangetext;

        public int interval;
    }
}
