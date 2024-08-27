using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions.Must;

// 만약 아이템 얻는 것 또는 다른 이벤트들이 있으면 추상 클래스로 만들어버리기
public class SelectSystem : MonoBehaviour
{
    [SerializeField] private int branch;                              //엑셀로 저장된 대화문 분기 담기
    [SerializeField] private SelectDB dialogDB;                          //엑셀로된 대화문 스크립트 담기
    [SerializeField] private SelectSpeaker speaker;                      //대화에 참여하는 캐릭터들의 UI 배열
    [SerializeField] ItemBtn_Event itembtn_event;                       // 아이템 선택 이벤트 관련 스크립트
    //사용되는 오브젝트가 popupPanel, getDamagePanel, getGoldPanel, gameOverPanel 끝??
    public GameObject popupPanel, getDamagePanel, getItemPanel, getGoldPanel, rewardPanel, takeawayItemPanel, havenoItemPanel, takeawayComcardPanel, magicItemPanel, gameOverPanel;
    
    public RewardItem rewardItem;                                       //보상아이템 스크립트
    public Button[] btn;                                             //버튼 담는 배열
    public SelectData dialogs;                                           //현재 분기의 대사 목록 배열
    public bool IsSelected;                                            //루트를 선택했냐 안했냐
    public string lostItemname;                                     // 뻇긴 or 소실한 아이템 이름
    public string changeItemname; 
    public int lostItemNum;                                     // 뻇긴 or 소실한 아이템 이름
    public int changeItemNum;
    public List<int> btnVal;                                        // 세게의 선택지의 정보를 담는 변수 -> 리스트 셔플을 이용하여 랜덤 구현

    public  void StartSelectSystem()
    {
        rewardItem = FindObjectOfType<RewardItem>();

        if (branch == 37)
        {
            ShuffleList<int>(btnVal);
        }

    }

    // 엑셀에서 선택지 정보 가져오기
    public void SetData()
    {
        for (int i = 0; i < dialogDB.Select.Count; i++)
        {
            if (dialogDB.Select[i].branch == branch)
            {
                //.name = dialogDB.Select[i].name;
                ///dialogs.dialogue = dialogDB.Select[i].dialog;
                dialogs.imageNum = dialogDB.Select[i].imageNum;
                dialogs.btnCount = dialogDB.Select[i].btnCount;
                if (dialogs.btnCount == 1)
                {
                    //dialogs.buttonText[0] = dialogDB.Select[i].btnText1;
                    ClickDataUpdate(0, i, dialogDB.Select[i].Event1);
                }
                if (dialogs.btnCount == 2)
                {
                    //dialogs.buttonText[0] = dialogDB.Select[i].btnText1;
                    //dialogs.buttonText[1] = dialogDB.Select[i].btnText2;
                    ClickDataUpdate(0, i, dialogDB.Select[i].Event1);
                    ClickDataUpdate(1, i, dialogDB.Select[i].Event2);
                }
                if (dialogs.btnCount == 3)
                {
                    //dialogs.buttonText[0] = dialogDB.Select[i].btnText1;
                    //dialogs.buttonText[1] = dialogDB.Select[i].btnText2;
                    //dialogs.buttonText[2] = dialogDB.Select[i].btnText3;
                    ClickDataUpdate(0, i, dialogDB.Select[i].Event1);
                    ClickDataUpdate(1, i, dialogDB.Select[i].Event2);
                    ClickDataUpdate(2, i, dialogDB.Select[i].Event3);
                }
            }
        }
    }

    // 다음 선택지 시작    
    public void SetDialog()
    {
        //현재 화자의 이미지 설정
        speaker.image.sprite = speaker.imageNum[dialogs.imageNum];
        //현재 화자의 이름 텍스트 확인
        //speaker.textName.text = dialogs.name;
        speaker.textName.textNum = branch - 1;
        speaker.textDialog.textNum = branch - 1;

        //현재 화자의 대사 텍스트 설정
        if (branch == 11)       //반응형 텍스트 - 카드 뺏긴 이벤트
        {
            //speaker.textDialog.text = "카드 병사에게" + "<b> <size=35> '" + GameManager.instance.comcardDB.comcard[EventManager.instance.LostCard].name + "</size></b>" + "'  카드를 빼았겼다.";
            //\n*+GameManager.instance.comcardDB.comcard[].explain
            speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Card[EventManager.instance.LostCard].KrName);
            speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Card[EventManager.instance.LostCard].EnName);
        }
        else if (branch == 14)
        {
            if (!GameManager.instance.IsItemNull)
            {
                //lostItemname = ES3.Load<string>("LostItemName", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
                //speaker.textDialog.text = "앨리스는 놀라서 " + "<b> <size=35>" + lostItemname + "</size></b>" + "을 떨어트렸다.\n떨어진 물건이 고장났다.";
                speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);
                speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
            }
            else
            {
                //speaker.textDialog.text = "<b> <size=35>" + "앨리스는 놀랐다. 깜짝이야!" + "</size></b>" + "\n";
                speaker.textDialog.textNum = 46;

                GameManager.instance.IsItemNull = false;
            }
        }
        else if (branch == 27)
        {
            //lostItemname = ES3.Load<string>("LostItemName", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
            //changeItemname = ES3.Load<string>("ChangeItemName", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
            //speaker.textDialog.text = "\"펑!\"" + "\n" + "\" " + lostItemname + "이(가) " + changeItemname + "(으)로 바뀌었어.\"" + "\n" + "토끼가 " + changeItemname + "을(를) 내밀었다.";
            speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);
            speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("ChangeItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);
            speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("ChangeItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);

            speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
            speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("ChangeItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
            speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("ChangeItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
        }
        else if (branch == 28)
        {
            //lostItemname = ES3.Load<string>("LostItemName", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
            //speaker.textDialog.text = "\"펑!\"" + "\n" + "\"에구구... 실패해서 " + lostItemname + "이(가) 부숴졌네...\"" + "\n" + lostItemname + "이(가) 부숴졌다.";            
            speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);
            speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);

            speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
            speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
        }
        else if (branch == 30)
        {
            //lostItemname = ES3.Load<string>("LostItemName", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
            //changeItemname = ES3.Load<string>("ChangeItemName", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
            //speaker.textDialog.text = "\"펑!\"" + "\n" + "\" " + lostItemname + "이(가) " + changeItemname + "(으)로 바뀌었어.\"" + "\n" + "토끼가 " + changeItemname + "을(를) 내밀었다.";

            speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Card[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);
            speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Card[ES3.Load<int>("ChangeItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);
            speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Card[ES3.Load<int>("ChangeItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);

            speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Card[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
            speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Card[ES3.Load<int>("ChangeItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
            speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Card[ES3.Load<int>("ChangeItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);

        }
        else if(branch == 31)
        {
            //lostItemname = ES3.Load<string>("LostItemName", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
            //speaker.textDialog.text = "\"펑!\"" + "\n" + "\"에구구... 실패해서 " + lostItemname + "이(가) 부숴졌네...\"" + "\n" + lostItemname + "이(가) 찢어졌다.";

            speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Card[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);
            speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Card[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);

            speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Card[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
            speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Card[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
        }
        else if (branch == 35)   //반응형 텍스트 - 포션 구매
        {
            //speaker.textDialog.text = "감사합니다. 고객님\"" + "\n" + "장사꾼에게서" + "<b> <size=35>" + EventManager.instance.ItemName + "</size></b>" + "을(를) 샀다.";
            switch (EventManager.instance.itemNum.Count)
            {
                case 2:
                    speaker.textDialog.textNum = 47;
                    break;
                case 3:
                    speaker.textDialog.textNum = 48;
                    break;
            }

            for(int count = 0; count < EventManager.instance.itemNum.Count; count++)
            {
                speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Act[EventManager.instance.itemNum[count]].KrName);
                speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Act[EventManager.instance.itemNum[count]].EnName);
            }

        }
        else if (branch == 36)   //반응형 텍스트 - 장사꾼에게 전재산 다 털린 이벤트 (50골드 이하일 경우)
        {
            //speaker.textDialog.text = "\"뭐! 안산다고? 그럼 구경한 값을 내!\"" + "\n" + "장사꾼이 억지를 부리며" + "<b> <size=35> '" + EventManager.instance.LostGold + "</size></b>" + "'  골드를 가져갔다.";
            speaker.textDialog.plusTexts.plusMiddleTextKr.Add(EventManager.instance.LostGold.ToString());
            speaker.textDialog.plusTexts.plusMiddleTextEn.Add(EventManager.instance.LostGold.ToString());
        }
        else if (branch == 46)   //반응형 텍스트 - 상인에게 무언가를 팔고 골드를 받음
        {
            //lostItemname = ES3.Load<string>("LostItemName", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
            //speaker.textDialog.text = "<b> <size=35>" + lostItemname + "</size></b>" + "\"을 팔겠나? 내 비싼 값에 사지.\"" + "\n" + "다이아 상인이" + "<b> <size=35>" + lostItemname + "</size></b>" + "을 받고" + "<b> <size=35> '" + EventManager.instance.LostGold + "</size></b>" + "'  골드를 주었다.";

            if (EventManager.instance.Trade_item == true)
            {
                speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);
                speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);
                speaker.textDialog.plusTexts.plusMiddleTextKr.Add(EventManager.instance.LostGold.ToString());

                speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
                speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
                speaker.textDialog.plusTexts.plusMiddleTextEn.Add(EventManager.instance.LostGold.ToString());
            }
            else
            {
                speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Card[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);
                speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Card[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);
                speaker.textDialog.plusTexts.plusMiddleTextKr.Add(EventManager.instance.LostGold.ToString());

                speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Card[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
                speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Card[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
                speaker.textDialog.plusTexts.plusMiddleTextEn.Add(EventManager.instance.LostGold.ToString());
            }

            
        }
        else
        {
            //speaker.textDialog.text = dialogs.dialogue;
        }
        
        speaker.textName.TranslateText();
        speaker.textDialog.TranslateText();

        foreach (var button in btn)
        {
            button.interactable = true;
        }
        foreach (var button in EventManager.instance.ExclusionButton)
        {
            btn[button].interactable = false;
        }

        //버튼 갯수 설정 & 버튼 텍스트 업데이트
        if (dialogs.btnCount == 1)
        {
            btn[0].gameObject.SetActive(true);
            btn[1].gameObject.SetActive(false);
            btn[2].gameObject.SetActive(false);
            //버튼의 자식 텍스트 업데이트
            //btn[0].GetComponentInChildren<Text>().text = dialogs.buttonText[0];
            btn[0].GetComponentInChildren<TextTranslate>().textNum = branch - 1;
            btn[0].GetComponentInChildren<TextTranslate>().TranslateText();
        }
        if (dialogs.btnCount == 2)
        {
            btn[0].gameObject.SetActive(true);
            btn[1].gameObject.SetActive(true);
            btn[2].gameObject.SetActive(false);
            //버튼의 자식 텍스트 업데이트
            //btn[0].GetComponentInChildren<Text>().text = dialogs.buttonText[0];
            //btn[1].GetComponentInChildren<Text>().text = dialogs.buttonText[1];
            btn[0].GetComponentInChildren<TextTranslate>().textNum = branch - 1;
            btn[0].GetComponentInChildren<TextTranslate>().TranslateText();
            btn[1].GetComponentInChildren<TextTranslate>().textNum = branch - 1;
            btn[1].GetComponentInChildren<TextTranslate>().TranslateText();
        }
        if (dialogs.btnCount == 3)
        {
            btn[0].gameObject.SetActive(true);
            btn[1].gameObject.SetActive(true);
            btn[2].gameObject.SetActive(true);
            //버튼의 자식 텍스트 업데이트
            //btn[0].GetComponentInChildren<Text>().text = dialogs.buttonText[0];
            //btn[1].GetComponentInChildren<Text>().text = dialogs.buttonText[1];
            //btn[2].GetComponentInChildren<Text>().text = dialogs.buttonText[2];

            btn[0].GetComponentInChildren<TextTranslate>().textNum = branch - 1;
            btn[0].GetComponentInChildren<TextTranslate>().TranslateText();
            btn[1].GetComponentInChildren<TextTranslate>().textNum = branch - 1;
            btn[1].GetComponentInChildren<TextTranslate>().TranslateText();
            btn[2].GetComponentInChildren<TextTranslate>().textNum = branch - 1;
            btn[2].GetComponentInChildren<TextTranslate>().TranslateText();

        }

        EventManager.instance.ExclusionButton.Clear();
    }

    // 리스트 셔플
    public static void ShuffleList<T>(List<T> list)
    {
        int random1; int random2;
        T tmp;
        for (int index = 0; index < list.Count; ++index)
        {
            random1 = Random.Range(0, list.Count);
            random2 = Random.Range(0, list.Count);
            tmp = list[random1];
            list[random1] = list[random2];
            list[random2] = tmp;
        }
    }

    // 버튼 선택 이벤트 -> 이벤트에서 사용될 함수가 추가될 때마다 여기다가 case로 추가해야함
    public void ClickDataUpdate(int btnNum, int branch, string val)  //버튼, 엑셀 브랜치, 이벤트 이름
    {
        switch (val)
        {
            case ("GetGold"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => GetGold());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => GetGold());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => GetGold());
                break;
            case ("LostGold"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => LostGold());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => LostGold());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => LostGold());
                break;
            case ("GetHeal"):
                btn[btnNum].onClick.AddListener(GetHeal);
                break;
            case ("GetItem"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(GetItem);
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(GetItem);
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(GetItem);
                break;
            case ("GetNormalCard"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => GetNormalCard());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => GetNormalCard());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => GetNormalCard());
                break;
            case ("GetActItem"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(GetItem_Act);
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(GetItem_Act);
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(GetItem_Act);
                break;
            case ("KickTree"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => KickTree());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => KickTree());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => KickTree());
                break;
            case ("SlashTree"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => SlashTree());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => SlashTree());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => SlashTree());
                break;
            case ("Vine"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => Vine());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => Vine());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => Vine());
                break;
            case ("TakeAwayComCard"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => TakeAwayComCard());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => TakeAwayComCard());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => TakeAwayComCard());
                break;
            case ("TakeAwayItem"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => TakeAwayItem());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => TakeAwayItem());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => TakeAwayItem());
                break;
            case ("RunningRaceRabbitWin"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => RunningRaceRabbitWin());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => RunningRaceRabbitWin());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => RunningRaceRabbitWin());
                break;
            case ("RunningRaceTurtleWin"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => RunningRaceTurtleWin());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => RunningRaceTurtleWin());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => RunningRaceTurtleWin());
                break;
            case ("GiveItem"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => GiveItem());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => GiveItem());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => GiveItem());
                break;
            case ("CheshireCat"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => CheshireCat());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => CheshireCat());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => CheshireCat());
                break;
            case ("Cake"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => Cake());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => Cake());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => Cake());
                break;
            case ("Larva"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => Larva());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => Larva());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => Larva());
                break;
            case ("MagicGiveItem"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => MagicGiveItem());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => MagicGiveItem());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => MagicGiveItem());
                break;
            case ("MagicGiveCombie"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => MagicGiveCombie());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => MagicGiveCombie());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => MagicGiveCombie());
                break;
            case ("Beggar"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => Beggar());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => Beggar());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => Beggar());
                break;
            case ("MerchantBuy1"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => MerchantBuy1());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => MerchantBuy1());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => MerchantBuy1());
                break;
            case ("MerchantBuy2"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => MerchantBuy2());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => MerchantBuy2());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => MerchantBuy2());
                break;
            case ("MerchantPass"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => MerchantPass());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => MerchantPass());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => MerchantPass());
                break;
            case ("CardGame"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => CardGame(btnVal[0]));
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => CardGame(btnVal[1]));
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => CardGame(btnVal[2]));
                break;
            case ("ClearSpring"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => ClearSpring());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => ClearSpring());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => ClearSpring());
                break;
            case ("Treasure"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => Treasure());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => Treasure());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => Treasure());
                break;
            case ("TraderCard"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => TraderCard());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => TraderCard());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => TraderCard());
                break;
            case ("TraderItem"):
                if (btnNum == 0)
                    btn[btnNum].onClick.AddListener(() => TraderItem());
                if (btnNum == 1)
                    btn[btnNum].onClick.AddListener(() => TraderItem());
                if (btnNum == 2)
                    btn[btnNum].onClick.AddListener(() => TraderItem());
                break;
            case ("Back")://맵으로가기
                btn[btnNum].onClick.AddListener(Back);
                break;
        }
    }

    public void GetGold()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        popupPanel.SetActive(true);
        //getGoldPanel.SetActive(true);

        int num = 0;

        // 카드게임 스테이지 예외처리
        if (branch != 37)
        {
            num = Random.Range(50, 101); 
        }
        else
        {
            num = 100;
        }

        //getGoldPanel.transform.GetChild(1).GetComponent<Text>().text = num + "골드를 획득하였다.";
        TextTranslate goldText = getGoldPanel.transform.GetChild(1).GetComponent<TextTranslate>();
        goldText.textNum = 59;
        goldText.plusTexts.plusFrontTextKr = num.ToString();
        goldText.plusTexts.plusFrontTextEn = num.ToString();

        getGoldPanel.SetActive(true);

        Player.instance.PlayerGetGold(num);

        IsSelected = true;
        FindObjectOfType<TopUI>().UpdateTopUI();
        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        //GameManager.instance.SavePlayerData();
    }

    public void LostGold()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);
        Player.instance.PlayerPayGold(100,true);
        popupPanel.SetActive(true);
        //getGoldPanel.SetActive(true);
        //getGoldPanel.transform.GetChild(1).GetComponent<Text>().text = 100 + "골드를 잃었다.";
        int num = 100;
        TextTranslate goldText = getGoldPanel.transform.GetChild(1).GetComponent<TextTranslate>();
        goldText.textNum = 60;
        goldText.plusTexts.plusFrontTextKr = num.ToString();
        goldText.plusTexts.plusFrontTextEn = num.ToString();

        getGoldPanel.SetActive(true);

        nextBranch = 12;
        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함

        IsSelected = true;
        FindObjectOfType<TopUI>().UpdateTopUI();
        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        //GameManager.instance.SavePlayerData();
    }

    public void GetHeal()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        GameManager.instance.PlayerHp = GameManager.instance.PlayerMaxHp;
        IsSelected = true;
        FindObjectOfType<TopUI>().UpdateTopUI();
        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        //GameManager.instance.SavePlayerData();
    }

    public void GetDamage(int val)
    {
        val = 5; // 모든 이벤트 입는 데미지 5로 고정

        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        Player.instance.PlayerHp -= val;
        TopUI.instance.UpdateTopUI();

        popupPanel.SetActive(true);
        //getDamagePanel.SetActive(true);
        //getDamagePanel.transform.GetChild(1).GetComponent<Text>().text = val+" 데미지를 입었다!";
        //GameManager.instance.SavePlayerData();

        if (Player.instance.PlayerHp <= 0)
        {
            gameOverPanel.SetActive(true);
            //gameOverPanel.transform.GetChild(1).GetComponent<Text>().text = val + " 데미지를 입었다! \n 앨리스는 정신을 잃었다...";
            TextTranslate gameOverText = gameOverPanel.transform.GetChild(1).GetComponent<TextTranslate>();
            gameOverText.plusTexts.plusFrontTextKr = val.ToString();
            gameOverText.plusTexts.plusFrontTextEn = val.ToString();
            gameOverText.TranslateText();


            if (GameManager.instance.difficulty == "normal")
            {
                ES3.Save<int>("EventDie", 1, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
            }
            else if (GameManager.instance.difficulty == "hard")
            {
                ES3.Save<int>("EventDie", 2, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
            }
            //Player.instance.PlayerGameOver();
        }
        else
        {
            getDamagePanel.SetActive(true);
            //getDamagePanel.transform.GetChild(1).GetComponent<Text>().text = val + " 데미지를 입었다!";
            TextTranslate getDamageText = getDamagePanel.transform.GetChild(1).GetComponent<TextTranslate>();
            getDamageText.plusTexts.plusFrontTextKr = val.ToString();
            getDamageText.plusTexts.plusFrontTextEn = val.ToString();
            getDamageText.TranslateText();
        }

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        //GameManager.instance.SavePlayerData();
    }

    public void GetNormalCard()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        itembtn_event.ChooseNormalCard();
        FindObjectOfType<TopUI>().UpdateTopUI();

        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        //GameManager.instance.SavePlayerData();
    }
    public void GetCombieCard()
    {
        if(SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        itembtn_event.ChooseComibeCard();
        FindObjectOfType<TopUI>().UpdateTopUI();

        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

    }
    public void GetItem()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        itembtn_event.ChooseItem();
        FindObjectOfType<TopUI>().UpdateTopUI();
        //데이터 저장
        GameManager.instance.SavePlayerData();
        GameManager.instance.SavePlayerItemData();
        GameManager.instance.SaveLastItemData();


        IsSelected = true;


        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        //GameManager.instance.SavePlayerData();
    }
    public void GetItem_Act()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        itembtn_event.ChooseItem_Act();
        btn[0].interactable = false;
        FindObjectOfType<TopUI>().UpdateTopUI();
        GameManager.instance.SavePlayerData();
        GameManager.instance.SavePlayerItemData();
        GameManager.instance.SaveLastItemData();

        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        
        //GameManager.instance.SavePlayerData();
    }
    public void GetJokerCard()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        itembtn_event.GetJokerCard();
        FindObjectOfType<TopUI>().UpdateTopUI();

        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();
    }
    public void Back()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        IsSelected = true;

        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        GameManager.instance.NextMap();
    }

    ///////////////////////////////////////////이벤트///////////////////////////////////////////
    public void GiveItem()
    {
        int nextBranch = 15;
        GetItem();
        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
    }
    public void KickTree()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        int nextBranch = 0;

        if (branch == 1)
        {
            int rand = Random.Range(0, 100);
            // 데미지 입는 분기
            if (rand < 30)
            {
                int damgeRange = Random.Range(7, 13);
                GetDamage(damgeRange);
                FindObjectOfType<TopUI>().UpdateTopUI();
                nextBranch = 4;
            }
            else
            {
                nextBranch = 2;
            }
        }

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함

        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        //GameManager.instance.SavePlayerData();
    }
    public void SlashTree()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);

        if (branch == 1)
        {
            int rand = Random.Range(0, 100);
            if (rand < 50)
            {
                nextBranch = 3;
            }
            else if (50 < rand && rand < 100)   // 데미지 입는 분기
            {
                int damgeRange = Random.Range(7, 13);
                GetDamage(damgeRange);
                FindObjectOfType<TopUI>().UpdateTopUI();
                nextBranch = 5;
            }
        }

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함

        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        //GameManager.instance.SavePlayerData();
    }
    public void Vine()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);

        if (branch == 7)
        {
            int rand = Random.Range(0, 100);
            if (rand < 50)
            {
                GetGold();

                nextBranch = 8;
            }
            else if (50 <= rand && rand < 100)
            {
                GetDamage(10);
                FindObjectOfType<TopUI>().UpdateTopUI();
                
                nextBranch = 9;
            }
        }
        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함

        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        //GameManager.instance.SavePlayerData();
    }
    public void CheshireCat()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);
        nextBranch = 15;
        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함

        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        //GameManager.instance.SavePlayerData();

    }
    public void TakeAwayComCard()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);

        int rand = Random.Range(0, GameManager.instance.MyCombiCard.Count);
        EventManager.instance.LostCard = GameManager.instance.MyCombiCard[rand] - 1;
        GameManager.instance.MyCombiCard.RemoveAt(rand);

        /*        popupPanel.SetActive(true);
                takeawayComcardPanel.SetActive(true);*/

        nextBranch = 11;
        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함


        IsSelected = true;
        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        //GameManager.instance.SavePlayerData();
    }
    public void TakeAwayItem()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);

        if (GameManager.instance.PlayerItemList.Count != 0)     //아이템이 있을경우
        {
            int rand = Random.Range(0, GameManager.instance.PlayerItemList.Count);
            //lostItemname = GameObject.Find("TopUI").transform.Find("Items_pas").transform.Find("item" + rand).GetComponent<TopUI_Item>().itemName;
            lostItemNum = GameObject.Find("TopUI").transform.Find("Items_pas").transform.Find("item" + rand).GetComponent<TopUI_Item>().itemNum;
            // 소실한 아이템 이름 저장
            //ES3.Save<string>("LostItemName", "'" + lostItemname + "'", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
            Debug.LogError("소실됨!" + lostItemname);
            ES3.Save<int>("LostItemNum", lostItemNum, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");

            GameManager.instance.LastItemList.Add(GameManager.instance.PlayerItemList[rand]);
            FindObjectOfType<ItemForce>().ItemForceOff(GameManager.instance.PlayerItemList[rand]);
            GameManager.instance.PlayerItemList.RemoveAt(rand);
            GameObject.Find("TopUI/Items_pas/item" + rand).SetActive(false);    

            FindObjectOfType<TopUI>().UpdateTopUI();
            /*        popupPanel.SetActive(true);
                    takeawayItemPanel.SetActive(true);*/
        }
        else
        {
            GameManager.instance.IsItemNull = true;
        }

        if (branch == 11)
        {
            nextBranch = 11;
            ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함
        }
        if (branch == 13)
        {
            nextBranch = 14;
            ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함
        }

        IsSelected = true;
        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        //GameManager.instance.SavePlayerData();
    }
    public void RunningRaceRabbitWin()
    {

        int nextBranch = 0;
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            nextBranch = 17;
        }
        else
        {
            nextBranch = 18;
        }

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함

        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        //GameManager.instance.SavePlayerData();
    }
    public void RunningRaceTurtleWin()
    {
        int nextBranch = 0;
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            nextBranch = 19;
        }
        else
        {
            nextBranch = 20;
        }

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함

        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        //GameManager.instance.SavePlayerData();
    }
    public void Cake()
    {
        DebugX.LogError("cake is lie");
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);
        int rand = Random.Range(0, 100);
        if (rand < 50)
        {
            nextBranch = 22;

            popupPanel.SetActive(true);
            // 케이크 먹고 최대 체력 상승
            if (!TopUI.instance.FindItemNum(32)) // 골무 아이템 소지시 따로 처리
            {
                Player.instance.PlayerMaxHP(5);
                //popupPanel.SetActive(true);
                //getGoldPanel.SetActive(true);
                //getGoldPanel.transform.GetChild(1).GetComponent<Text>().text = "최대체력이 5 증가했다!";
            }
            else
            {
                //Player.instance.HealHP(5);
                //popupPanel.SetActive(true);
                //getGoldPanel.SetActive(true);
                //getGoldPanel.transform.GetChild(1).GetComponent<Text>().text = "최대체력이 5 증가했다!";
            }

            TextTranslate goldText = getGoldPanel.transform.GetChild(1).GetComponent<TextTranslate>();
            goldText.textNum = 61;
            getGoldPanel.SetActive(true);
        }
        else
        {
            nextBranch = 23;
            // 케이크 먹고 데미지 받음
            GetDamage(5);
            FindObjectOfType<TopUI>().UpdateTopUI();
        }

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함
        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();
    }
    public void Larva()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);
        nextBranch = 25;

        GetCombieCard();

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함
        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();
    }
    public void MagicGiveItem()
    {
        StartCoroutine(MagicGiveItemProcess());
    }
    public IEnumerator MagicGiveItemProcess()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);

        // 아이템 주기
        StartCoroutine(TopUI.instance.DiscardItemPanel());
        // 선택 까지 대기
        yield return new WaitUntil(() => FindObjectOfType<ItemBtn_Event>().isGive);

        // 결과에 따라서 브랜치 갈림
        if (FindObjectOfType<ItemBtn_Event>().isSuccess)
        {
            nextBranch = 27;
        }
        else
        {
            nextBranch = 28;
        }
        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함
        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        yield return null;
    }

    public void MagicGiveCombie()
    {
        StartCoroutine(MagicGiveCombieProcess());
    }

    public IEnumerator MagicGiveCombieProcess()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);
        // 조합카드 패널 열기
        if (!EndlessManager.instance.isEndless)
        {
            GameObject.Find("RewardCanvas").transform.Find("MagicGiveCombiePanel").gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("RewardCanvas_endless").transform.Find("MagicGiveCombiePanel").gameObject.SetActive(true);
        }

        // 선택 까지 대기
        yield return new WaitUntil(() => FindObjectOfType<RestBtn>().isGive);

        // 결과에 따라서 브랜치 갈림
        if (FindObjectOfType<ItemBtn_Event>().isSuccess)
        {
            nextBranch = 30;
        }
        else
        {
            nextBranch = 31;
        }
        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함
        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        yield return null;
    }

    public void Beggar()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);

        nextBranch = 33;

        int num = Random.Range(50, 201);        //-100 골드 +150~300골드 = 50~200
        popupPanel.SetActive(true);
        //getGoldPanel.SetActive(true);
        //getGoldPanel.transform.GetChild(1).GetComponent<Text>().text = num+100 + "골드를 흭득하였다."; 
        
        TextTranslate goldText = getGoldPanel.transform.GetChild(1).GetComponent<TextTranslate>();
        goldText.textNum = 59;
        goldText.plusTexts.plusFrontTextKr = (num + 100).ToString();
        goldText.plusTexts.plusFrontTextEn = (num + 100).ToString();
        getGoldPanel.SetActive(true);

        Player.instance.PlayerGetGold(num);

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함
        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();
    }

    public void MerchantBuy1()
    {
        StartCoroutine(MerchantBuy1F());
    }
    public IEnumerator MerchantBuy1F()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);

        nextBranch = 35;

        Player.instance.PlayerPayGold(20,true);
        itembtn_event.ChooseItem_Act();
        yield return new WaitUntil(() => EventManager.instance.Act);
        FindObjectOfType<TopUI>().UpdateTopUI();

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함
        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();
    }
    public void MerchantBuy2()
    {
        StartCoroutine(MerchantBuy2F());
    }

    public IEnumerator MerchantBuy2F()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);

        nextBranch = 35;

        Player.instance.PlayerPayGold(30,true);
        itembtn_event.ChooseItem_Act();
        yield return new WaitUntil(() => EventManager.instance.Act);
        EventManager.instance.Act = false;
        itembtn_event.ChooseItem_Act();
        FindObjectOfType<EventReward>().RewardItem_act[0].SetActive(true);
        yield return new WaitUntil(() => EventManager.instance.Act);
        FindObjectOfType<TopUI>().UpdateTopUI();

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함
        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();
    }

    public void MerchantPass()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);
        nextBranch = 36;

        if (Player.instance.PlayerGold > 50)
        {
            Player.instance.PlayerPayGold(50,true);
            EventManager.instance.LostGold = 50;
        }
        else
        {
            EventManager.instance.LostGold = (int)Player.instance.PlayerGold;
            Player.instance.PlayerPayGold(EventManager.instance.LostGold,true);
        }

        FindObjectOfType<TopUI>().UpdateTopUI();

        //팝업 띄울거면 사용
        /*     popupPanel.SetActive(true);
             getGoldPanel.SetActive(true);
         getGoldPanel.transform.GetChild(1).GetComponent<Text>().text = EventManager.instance.LostGold + "골드를 가져갔다.";*/

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함
        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();
    }
    public void CardGame(int val)
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);

        EventManager.instance.CardGameNum = val;

        if (val == 1)       //골드
        {
            GetGold();
            nextBranch = 38;
        }
        else if (val == 2)  //포션
        {
            GetItem_Act();
            nextBranch = 39;
        }
        else if (val == 3)  //조커
        {
            GetJokerCard();
            nextBranch = 40;
        }

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함
        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();
    }
    public void ClearSpring()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);

        nextBranch = 42;

        Player.instance.HealHP(20); 
        popupPanel.SetActive(true);
        //getGoldPanel.SetActive(true);
        //getGoldPanel.transform.GetChild(1).GetComponent<Text>().text = "20 체력을 회복했다!";
        int num = 20;

        TextTranslate goldText = getGoldPanel.transform.GetChild(1).GetComponent<TextTranslate>();
        goldText.textNum = 62;
        goldText.plusTexts.plusFrontTextKr = "20";
        goldText.plusTexts.plusFrontTextEn = "20";        
        getGoldPanel.SetActive(true);

        FindObjectOfType<TopUI>().UpdateTopUI();

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함
        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();
    }

    public void Treasure()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);

        nextBranch = 44;

        GetDamage(5);
        FindObjectOfType<TopUI>().UpdateTopUI();

        //getDamagePanel.transform.GetChild(1).GetComponent<Text>().text ="5 데미지를 입었다.";

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함
        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();
    }

    public void TraderCard()
    {
        StartCoroutine(TraderCardGiveItemProcess());
    }
    public void TraderItem()
    {
        StartCoroutine(TraderItemGiveItemProcess());
    }
    public IEnumerator TraderCardGiveItemProcess()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }
        
        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);

        // 조합카드 패널 열기
        if (!EndlessManager.instance.isEndless)
        {
            GameObject.Find("RewardCanvas").transform.Find("MagicGiveCombiePanel").gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("RewardCanvas_endless").transform.Find("MagicGiveCombiePanel").gameObject.SetActive(true);
        }

        // 선택 까지 대기
        yield return new WaitUntil(() => FindObjectOfType<RestBtn>().isGive);
        nextBranch = 46;
        EventManager.instance.Trade_item = false;
        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함
        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        yield return null;
    }

    public IEnumerator TraderItemGiveItemProcess()
    {
        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }        
        int nextBranch = ES3.Load<int>("NextBrach", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3", 0);
        //TopUI.instance.itemDiscardPanel.transform.GetChild(2).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "아이템 판매";
        StartCoroutine(TopUI.instance.DiscardItemPanel(63));        

        yield return new WaitUntil(() => FindObjectOfType<ItemBtn_Event>().isGive);

        nextBranch = 46;
        EventManager.instance.Trade_item = true;
        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // 다음 -뒤에올 브랜치 저장 -> ContinuitySelectTest에서 쓰기위함
        IsSelected = true;

        //버튼 리스너 삭제
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        yield return null;
    }
}
//화자 정보
[System.Serializable]
public struct SelectSpeaker
{
    public Image image;                             //캐릭터 이미지
    public Sprite[] imageNum;                   //캐릭터 이미지 스프라이트 담은 배열
    public Image imageDialog;                   //대화창 Iamge UI
    //public TextMeshProUGUI textName;
    //public TextMeshProUGUI textDialog;
    public TextTranslate textName;            //현재 대사중인 캐릭터 이름 출력 Text UI
    public TextTranslate textDialog;          //현재 대사 출력 Text UI
    public GameObject Panel;
}
//대화정보
[System.Serializable]
public struct SelectData
{
    public int imageNum;            //캐릭터 이미지 스프라이트 번호
    public string name;             //캐릭터 이름
    [TextArea(3, 5)]
    public string dialogue;         //대사
    public int btnCount;            //버튼 갯 수
    public string[] buttonText;     //버튼 텍스트 
}

