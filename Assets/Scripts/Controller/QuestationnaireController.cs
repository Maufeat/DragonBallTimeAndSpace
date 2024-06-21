using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Models;
using UnityEngine;

public class QuestationnaireController : ControllerBase
{
    public UI_Questationnaire UI_Questationnaire
    {
        get
        {
            return UIManager.GetUIObject<UI_Questationnaire>();
        }
    }

    public int GetQuesCount()
    {
        return this.mQuesIdList.Count;
    }

    public QuestationnaireController.SurveyTalbeItem GetCurShowQuesItem()
    {
        if (this.mQuesIdList.Count > 0)
        {
            return this.mTableDic[this.mQuesIdList[0]];
        }
        return null;
    }

    public override void Awake()
    {
        this.mNetwork = new QuestationnaireNetworker();
        this.mNetwork.Initialize();
        this.SetupTableConfig();
        base.Awake();
    }

    private void SetupTableConfig()
    {
        this.mTableDic = new Dictionary<uint, QuestationnaireController.SurveyTalbeItem>();
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("survey_config");
        for (int i = 0; i < configTableList.Count; i++)
        {
            QuestationnaireController.SurveyTalbeItem surveyTalbeItem = new QuestationnaireController.SurveyTalbeItem();
            surveyTalbeItem.id = configTableList[i].GetField_Uint("id");
            surveyTalbeItem.url = configTableList[i].GetField_String("url");
            surveyTalbeItem.level = configTableList[i].GetField_Uint("level");
            this.mTableDic.Add(surveyTalbeItem.id, surveyTalbeItem);
        }
    }

    public void ReqQuesList()
    {
        this.mNetwork.ReqQuesList();
    }

    public void ReqQuesListCb(List<uint> idlist)
    {
        this.mQuesIdList = new List<uint>();
        for (int i = 0; i < idlist.Count; i++)
        {
            uint num = idlist[i];
            if (this.mTableDic.ContainsKey(num) && this.mTableDic[num].level <= MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.level)
            {
                this.mQuesIdList.Add(num);
            }
        }
        UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
        if (mainView != null)
        {
            mainView.SetupQuestionnaireBtn(this.mQuesIdList.Count > 0);
        }
    }

    public void ShowUI()
    {
        UIManager.Instance.ShowUI<UI_Questationnaire>("UI_Questionnaire", delegate ()
        {
            QuestationnaireController.SurveyTalbeItem curShowQuesItem = this.GetCurShowQuesItem();
            if (curShowQuesItem == null)
            {
                Debug.LogError("GetCurShowQuesItem == null!!!");
                return;
            }
            string urlWithParam = this.GetUrlWithParam(curShowQuesItem.url);
            this.UI_Questationnaire.SetupPanel(urlWithParam);
        }, UIManager.ParentType.CommonUI, false);
    }

    public void DeleteUI()
    {
        UIManager.Instance.DeleteUI<UI_Questationnaire>();
    }

    private string GetUrlWithParam(string url)
    {
        string text = "lzo";
        string uid = UserInfoStorage.StorageInfo.Uid;
        string name = MainPlayer.Self.OtherPlayerData.MapUserData.name;
        string token = this.GetToken(text, uid, name, "6v5MwI43hfrApukIcQJk8YIPu4AU5k9q");
        string text2 = url;
        url = string.Concat(new string[]
        {
            text2,
            "?project=",
            text,
            "&account=",
            uid,
            "&nickname=",
            name,
            "&token=",
            token
        });
        return url;
    }

    private string GetToken(string project, string account, string nickname, string key)
    {
        string data = project + account + nickname + key;
        return MD5Help.GetMD5ByString(data);
    }

    public void WebCb(string message)
    {
        Debug.Log("webcb======================" + message);
        if (message == "surveyfinish")
        {
            Debug.Log(string.Concat(new object[]
            {
                "webcb************",
                message,
                "*************true",
                this.mNetwork
            }));
            QuestationnaireController.SurveyTalbeItem curShowQuesItem = this.GetCurShowQuesItem();
            if (curShowQuesItem != null)
            {
                this.mNetwork.ReqSurveyReward(curShowQuesItem.id);
            }
        }
    }

    public void ReqSurveyRewardCb(uint quesid, uint retcode)
    {
        if (retcode == 0U)
        {
            this.ReqQuesList();
        }
        else
        {
            string content = "点击过快，流程处理中";
            switch (retcode)
            {
                case 2U:
                    content = "不满足问卷调查触发条件";
                    break;
                case 3U:
                    content = "已领过";
                    break;
                case 4U:
                    content = "未定义错误";
                    break;
            }
            TipsWindow.ShowNotice(content);
        }
        UIManager.Instance.DeleteUI<UI_Questationnaire>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override string ControllerName
    {
        get
        {
            return "questationnaire_controller";
        }
    }

    private const string QUEST_LIST_URL = "http://www.xxx.com";

    private QuestationnaireNetworker mNetwork;

    private Dictionary<uint, QuestationnaireController.SurveyTalbeItem> mTableDic = new Dictionary<uint, QuestationnaireController.SurveyTalbeItem>();

    private List<uint> mQuesIdList = new List<uint>();

    public class SurveyTalbeItem
    {
        public uint id;

        public string url;

        public uint level;
    }
}
