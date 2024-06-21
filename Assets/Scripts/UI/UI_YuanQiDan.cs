using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class UI_YuanQiDan : UIPanelBase
{
    private YuanQiDanController _controller
    {
        get
        {
            return ControllerManager.Instance.GetController<YuanQiDanController>();
        }
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.GAME_START_TIP = CommonTools.GetTextById(871UL);
        this.GAME_FAIL = CommonTools.GetTextById(874UL);
        this.GAME_SUCCESS = CommonTools.GetTextById(873UL);
        this.transTipText = root.Find("Offset/Text");
        this.transTipPanel = root.Find("Offset/tips");
        this.transCirclePanel = root.Find("Offset/Progress");
        this.imgCircle = this.transCirclePanel.Find("img_circle/Image").GetComponent<Image>();
        this.txtCircle = this.transCirclePanel.Find("Image/Text").GetComponent<Text>();
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public override void OnDispose()
    {
        this._controller.SetMoveState(true);
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.HideTipText));
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.CheckNpcInScene));
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
        base.OnDispose();
    }

    public void Start(int taskId)
    {
        this._configData = new UI_YuanQiDan.YuanQiDanData(taskId);
        this.ShowTipText(this.GAME_START_TIP);
        this.transTipPanel.gameObject.SetActive(false);
        this.transCirclePanel.gameObject.SetActive(false);
        Scheduler.Instance.AddTimer(5f, false, delegate
        {
            if (this._configData.npcId != 0)
            {
                Scheduler.Instance.AddTimer(1f, true, new Scheduler.OnScheduler(this.CheckNpcInScene));
            }
            this.ResetValue();
            this.transTipPanel.gameObject.SetActive(true);
            this.transCirclePanel.gameObject.SetActive(true);
            this.SetValue(0);
        });
    }

    private void SetValue(int index)
    {
        this._curStepIndex = index;
        if (index >= this._configData.round)
        {
            this.ChangeState(UI_YuanQiDan.GameState.Succeed);
        }
        else
        {
            this._startValue = this._configData.stateValue[this._curStepIndex * 2].ToFloat();
            this._endValue = this._configData.stateValue[this._curStepIndex * 2 + 1].ToFloat();
            this.ChangeState(UI_YuanQiDan.GameState.PlayReady);
        }
    }

    private void ResetValue()
    {
        this._curStepIndex = 0;
        this.imgCircle.fillAmount = 0f;
        this.txtCircle.text = "0%";
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.SpaceKeyDown();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            this.SpaceKeyOn();
        }
        if (this._curState == UI_YuanQiDan.GameState.Playing)
        {
            this.imgCircle.fillAmount += Time.deltaTime * this.GetSpeed(this.imgCircle.fillAmount);
            this.txtCircle.text = (int)(this.imgCircle.fillAmount * 100f) + "%";
        }
    }

    private void SpaceKeyDown()
    {
        if (this._curState == UI_YuanQiDan.GameState.PlayReady)
        {
            this.ChangeState(UI_YuanQiDan.GameState.Playing);
        }
    }

    private void SpaceKeyOn()
    {
        if (this._curState == UI_YuanQiDan.GameState.Playing)
        {
            this.ChangeState(UI_YuanQiDan.GameState.PlayEnd);
        }
    }

    private float GetSpeed(float rate)
    {
        this._speed = this._configData.speedValue[(int)rate].ToFloat();
        return this._speed * 5f;
    }

    private void ChangeState(UI_YuanQiDan.GameState state)
    {
        this._curState = state;
        switch (this._curState)
        {
            case UI_YuanQiDan.GameState.None:
                this.ResetValue();
                this.SetValue(0);
                break;
            case UI_YuanQiDan.GameState.Playing:
                this.PlayingAction();
                break;
            case UI_YuanQiDan.GameState.PlayEnd:
                this.CheckSuccess(this.imgCircle.fillAmount);
                break;
            case UI_YuanQiDan.GameState.Succeed:
                Scheduler.Instance.AddTimer(3f, false, delegate
                {
                    this._controller.CloseUI();
                    this.GameExit();
                });
                break;
            case UI_YuanQiDan.GameState.Failed:
                this.GameRetry();
                this.ShowTipText(this.GAME_FAIL);
                this.ChangeState(UI_YuanQiDan.GameState.None);
                break;
        }
    }

    private void PlayingAction()
    {
        FFBehaviourState_CopyAction @object = ClassPool.GetObject<FFBehaviourState_CopyAction>();
        @object.Init(this._configData.actionStartId, this._configData.actionId);
        FFBehaviourState_CopyAction object2 = ClassPool.GetObject<FFBehaviourState_CopyAction>();
        object2.Init(this._configData.npcActionStart, this._configData.npcActionId);
        MainPlayer.Self.Fbc.ChangeState(@object);
        if (this.npc != null)
        {
            this.npc.Fbc.ChangeState(object2);
        }
    }

    private void CheckSuccess(float fillAmount)
    {
        bool succ;
        if (fillAmount * 100f > this._startValue && fillAmount * 100f <= this._endValue)
        {
            this.ShowTipText(this.GAME_SUCCESS);
            this.SetValue(this._curStepIndex + 1);
            succ = true;
        }
        else
        {
            this.ChangeState(UI_YuanQiDan.GameState.Failed);
            succ = false;
        }
        this.OperateResult(succ);
        this.ResetAction();
    }

    private void ResetAction()
    {
        MainPlayer.Self.Fbc.ChangeState(ClassPool.GetObject<FFBehaviourState_Idle>());
        if (this.npc != null)
        {
            this.npc.Fbc.ChangeState(ClassPool.GetObject<FFBehaviourState_Idle>());
        }
    }

    private void OperateResult(bool succ)
    {
        this._controller.CommitYQDData_CS(this._curStepIndex, succ, this._configData.id);
    }

    private void GameRetry()
    {
        this._controller.Req_PlayYQDRetry_CS(this._configData.id);
    }

    private void GameExit()
    {
        this._controller.ReqGameExit();
    }

    private void CheckNpcInScene()
    {
        if (this._configData.npcId == 0)
        {
            return;
        }
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        if (manager != null)
        {
            manager.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
            {
                this.npc = pair.Value;
                if ((long)this._configData.npcId == (long)((ulong)this.npc.NpcData.MapNpcData.baseid))
                {
                    Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.CheckNpcInScene));
                    return;
                }
            });
        }
    }

    private Transform transTipText;

    private Transform transTipPanel;

    private Transform transCirclePanel;

    private Image imgCircle;

    private Text txtCircle;

    private string GAME_START_TIP = string.Empty;

    private string GAME_FAIL = string.Empty;

    private string GAME_SUCCESS = string.Empty;

    private UI_YuanQiDan.GameState _curState;

    private UI_YuanQiDan.YuanQiDanData _configData;

    private Npc npc;

    private int _curStepIndex;

    private float _startValue;

    private float _endValue;

    private float _speed = 0.01f;

    private enum GameState
    {
        None,
        PlayReady,
        Playing,
        PlayEnd,
        Succeed,
        Failed
    }

    public class YuanQiDanData
    {
        public YuanQiDanData(int _id)
        {
            this.id = _id;
            LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("other").GetCacheField_Table("yuanqidan");
            IEnumerator enumerator = cacheField_Table.Values.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                object obj = enumerator.Current;
                LuaTable luaTable = obj as LuaTable;
                if (luaTable.GetCacheField_Int("id") == this.id)
                {
                    this.stateValue = luaTable.GetCacheField_String("stage").Split(new char[]
                    {
                        ','
                    });
                    this.speedValue = luaTable.GetCacheField_String("speed").Split(new char[]
                    {
                        ','
                    });
                    this.round = this.stateValue.Length / 2;
                    this.actionStartId = luaTable.GetCacheField_Int("actionstartid");
                    this.actionId = luaTable.GetCacheField_Int("actionid");
                    this.npcId = luaTable.GetCacheField_Int("npcid");
                    this.npcActionStart = luaTable.GetCacheField_Int("npcactionstartid");
                    this.npcActionId = luaTable.GetCacheField_Int("npcactionid");
                    break;
                }
            }
        }

        public int id;

        public string[] stateValue;

        public string[] speedValue;

        public int round;

        public int actionStartId;

        public int actionId;

        public int npcId;

        public int npcActionStart;

        public int npcActionId;
    }
}
