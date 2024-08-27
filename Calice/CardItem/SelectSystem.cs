using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions.Must;

// ���� ������ ��� �� �Ǵ� �ٸ� �̺�Ʈ���� ������ �߻� Ŭ������ ����������
public class SelectSystem : MonoBehaviour
{
    [SerializeField] private int branch;                              //������ ����� ��ȭ�� �б� ���
    [SerializeField] private SelectDB dialogDB;                          //�����ε� ��ȭ�� ��ũ��Ʈ ���
    [SerializeField] private SelectSpeaker speaker;                      //��ȭ�� �����ϴ� ĳ���͵��� UI �迭
    [SerializeField] ItemBtn_Event itembtn_event;                       // ������ ���� �̺�Ʈ ���� ��ũ��Ʈ
    //���Ǵ� ������Ʈ�� popupPanel, getDamagePanel, getGoldPanel, gameOverPanel ��??
    public GameObject popupPanel, getDamagePanel, getItemPanel, getGoldPanel, rewardPanel, takeawayItemPanel, havenoItemPanel, takeawayComcardPanel, magicItemPanel, gameOverPanel;
    
    public RewardItem rewardItem;                                       //��������� ��ũ��Ʈ
    public Button[] btn;                                             //��ư ��� �迭
    public SelectData dialogs;                                           //���� �б��� ��� ��� �迭
    public bool IsSelected;                                            //��Ʈ�� �����߳� ���߳�
    public string lostItemname;                                     // �P�� or �ҽ��� ������ �̸�
    public string changeItemname; 
    public int lostItemNum;                                     // �P�� or �ҽ��� ������ �̸�
    public int changeItemNum;
    public List<int> btnVal;                                        // ������ �������� ������ ��� ���� -> ����Ʈ ������ �̿��Ͽ� ���� ����

    public  void StartSelectSystem()
    {
        rewardItem = FindObjectOfType<RewardItem>();

        if (branch == 37)
        {
            ShuffleList<int>(btnVal);
        }

    }

    // �������� ������ ���� ��������
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

    // ���� ������ ����    
    public void SetDialog()
    {
        //���� ȭ���� �̹��� ����
        speaker.image.sprite = speaker.imageNum[dialogs.imageNum];
        //���� ȭ���� �̸� �ؽ�Ʈ Ȯ��
        //speaker.textName.text = dialogs.name;
        speaker.textName.textNum = branch - 1;
        speaker.textDialog.textNum = branch - 1;

        //���� ȭ���� ��� �ؽ�Ʈ ����
        if (branch == 11)       //������ �ؽ�Ʈ - ī�� ���� �̺�Ʈ
        {
            //speaker.textDialog.text = "ī�� ���翡��" + "<b> <size=35> '" + GameManager.instance.comcardDB.comcard[EventManager.instance.LostCard].name + "</size></b>" + "'  ī�带 ���Ұ��.";
            //\n*+GameManager.instance.comcardDB.comcard[].explain
            speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Card[EventManager.instance.LostCard].KrName);
            speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Card[EventManager.instance.LostCard].EnName);
        }
        else if (branch == 14)
        {
            if (!GameManager.instance.IsItemNull)
            {
                //lostItemname = ES3.Load<string>("LostItemName", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
                //speaker.textDialog.text = "�ٸ����� ��� " + "<b> <size=35>" + lostItemname + "</size></b>" + "�� ����Ʈ�ȴ�.\n������ ������ ���峵��.";
                speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);
                speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
            }
            else
            {
                //speaker.textDialog.text = "<b> <size=35>" + "�ٸ����� �����. ��¦�̾�!" + "</size></b>" + "\n";
                speaker.textDialog.textNum = 46;

                GameManager.instance.IsItemNull = false;
            }
        }
        else if (branch == 27)
        {
            //lostItemname = ES3.Load<string>("LostItemName", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
            //changeItemname = ES3.Load<string>("ChangeItemName", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
            //speaker.textDialog.text = "\"��!\"" + "\n" + "\" " + lostItemname + "��(��) " + changeItemname + "(��)�� �ٲ����.\"" + "\n" + "�䳢�� " + changeItemname + "��(��) ���о���.";
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
            //speaker.textDialog.text = "\"��!\"" + "\n" + "\"������... �����ؼ� " + lostItemname + "��(��) �ν�����...\"" + "\n" + lostItemname + "��(��) �ν�����.";            
            speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);
            speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);

            speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
            speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Pas[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
        }
        else if (branch == 30)
        {
            //lostItemname = ES3.Load<string>("LostItemName", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
            //changeItemname = ES3.Load<string>("ChangeItemName", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
            //speaker.textDialog.text = "\"��!\"" + "\n" + "\" " + lostItemname + "��(��) " + changeItemname + "(��)�� �ٲ����.\"" + "\n" + "�䳢�� " + changeItemname + "��(��) ���о���.";

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
            //speaker.textDialog.text = "\"��!\"" + "\n" + "\"������... �����ؼ� " + lostItemname + "��(��) �ν�����...\"" + "\n" + lostItemname + "��(��) ��������.";

            speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Card[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);
            speaker.textDialog.plusTexts.plusMiddleTextKr.Add(TextTranslateManager.instance.textDB.Card[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].KrName);

            speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Card[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
            speaker.textDialog.plusTexts.plusMiddleTextEn.Add(TextTranslateManager.instance.textDB.Card[ES3.Load<int>("LostItemNum", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3")].EnName);
        }
        else if (branch == 35)   //������ �ؽ�Ʈ - ���� ����
        {
            //speaker.textDialog.text = "�����մϴ�. ����\"" + "\n" + "���ۿ��Լ�" + "<b> <size=35>" + EventManager.instance.ItemName + "</size></b>" + "��(��) ���.";
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
        else if (branch == 36)   //������ �ؽ�Ʈ - ���ۿ��� ����� �� �и� �̺�Ʈ (50��� ������ ���)
        {
            //speaker.textDialog.text = "\"��! �Ȼ�ٰ�? �׷� ������ ���� ��!\"" + "\n" + "������ ������ �θ���" + "<b> <size=35> '" + EventManager.instance.LostGold + "</size></b>" + "'  ��带 ��������.";
            speaker.textDialog.plusTexts.plusMiddleTextKr.Add(EventManager.instance.LostGold.ToString());
            speaker.textDialog.plusTexts.plusMiddleTextEn.Add(EventManager.instance.LostGold.ToString());
        }
        else if (branch == 46)   //������ �ؽ�Ʈ - ���ο��� ���𰡸� �Ȱ� ��带 ����
        {
            //lostItemname = ES3.Load<string>("LostItemName", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
            //speaker.textDialog.text = "<b> <size=35>" + lostItemname + "</size></b>" + "\"�� �Ȱڳ�? �� ��� ���� ����.\"" + "\n" + "���̾� ������" + "<b> <size=35>" + lostItemname + "</size></b>" + "�� �ް�" + "<b> <size=35> '" + EventManager.instance.LostGold + "</size></b>" + "'  ��带 �־���.";

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

        //��ư ���� ���� & ��ư �ؽ�Ʈ ������Ʈ
        if (dialogs.btnCount == 1)
        {
            btn[0].gameObject.SetActive(true);
            btn[1].gameObject.SetActive(false);
            btn[2].gameObject.SetActive(false);
            //��ư�� �ڽ� �ؽ�Ʈ ������Ʈ
            //btn[0].GetComponentInChildren<Text>().text = dialogs.buttonText[0];
            btn[0].GetComponentInChildren<TextTranslate>().textNum = branch - 1;
            btn[0].GetComponentInChildren<TextTranslate>().TranslateText();
        }
        if (dialogs.btnCount == 2)
        {
            btn[0].gameObject.SetActive(true);
            btn[1].gameObject.SetActive(true);
            btn[2].gameObject.SetActive(false);
            //��ư�� �ڽ� �ؽ�Ʈ ������Ʈ
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
            //��ư�� �ڽ� �ؽ�Ʈ ������Ʈ
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

    // ����Ʈ ����
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

    // ��ư ���� �̺�Ʈ -> �̺�Ʈ���� ���� �Լ��� �߰��� ������ ����ٰ� case�� �߰��ؾ���
    public void ClickDataUpdate(int btnNum, int branch, string val)  //��ư, ���� �귣ġ, �̺�Ʈ �̸�
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
            case ("Back")://�����ΰ���
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

        // ī����� �������� ����ó��
        if (branch != 37)
        {
            num = Random.Range(50, 101); 
        }
        else
        {
            num = 100;
        }

        //getGoldPanel.transform.GetChild(1).GetComponent<Text>().text = num + "��带 ȹ���Ͽ���.";
        TextTranslate goldText = getGoldPanel.transform.GetChild(1).GetComponent<TextTranslate>();
        goldText.textNum = 59;
        goldText.plusTexts.plusFrontTextKr = num.ToString();
        goldText.plusTexts.plusFrontTextEn = num.ToString();

        getGoldPanel.SetActive(true);

        Player.instance.PlayerGetGold(num);

        IsSelected = true;
        FindObjectOfType<TopUI>().UpdateTopUI();
        //��ư ������ ����
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
        //getGoldPanel.transform.GetChild(1).GetComponent<Text>().text = 100 + "��带 �Ҿ���.";
        int num = 100;
        TextTranslate goldText = getGoldPanel.transform.GetChild(1).GetComponent<TextTranslate>();
        goldText.textNum = 60;
        goldText.plusTexts.plusFrontTextKr = num.ToString();
        goldText.plusTexts.plusFrontTextEn = num.ToString();

        getGoldPanel.SetActive(true);

        nextBranch = 12;
        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������

        IsSelected = true;
        FindObjectOfType<TopUI>().UpdateTopUI();
        //��ư ������ ����
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
        //��ư ������ ����
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        //GameManager.instance.SavePlayerData();
    }

    public void GetDamage(int val)
    {
        val = 5; // ��� �̺�Ʈ �Դ� ������ 5�� ����

        if (SetVolume.instance)
        {
            SetVolume.instance.PlaySE("btn_click");
        }

        Player.instance.PlayerHp -= val;
        TopUI.instance.UpdateTopUI();

        popupPanel.SetActive(true);
        //getDamagePanel.SetActive(true);
        //getDamagePanel.transform.GetChild(1).GetComponent<Text>().text = val+" �������� �Ծ���!";
        //GameManager.instance.SavePlayerData();

        if (Player.instance.PlayerHp <= 0)
        {
            gameOverPanel.SetActive(true);
            //gameOverPanel.transform.GetChild(1).GetComponent<Text>().text = val + " �������� �Ծ���! \n �ٸ����� ������ �Ҿ���...";
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
            //getDamagePanel.transform.GetChild(1).GetComponent<Text>().text = val + " �������� �Ծ���!";
            TextTranslate getDamageText = getDamagePanel.transform.GetChild(1).GetComponent<TextTranslate>();
            getDamageText.plusTexts.plusFrontTextKr = val.ToString();
            getDamageText.plusTexts.plusFrontTextEn = val.ToString();
            getDamageText.TranslateText();
        }

        //��ư ������ ����
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

        //��ư ������ ����
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

        //��ư ������ ����
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
        //������ ����
        GameManager.instance.SavePlayerData();
        GameManager.instance.SavePlayerItemData();
        GameManager.instance.SaveLastItemData();


        IsSelected = true;


        //��ư ������ ����
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

        //��ư ������ ����
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

        //��ư ������ ����
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

    ///////////////////////////////////////////�̺�Ʈ///////////////////////////////////////////
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
            // ������ �Դ� �б�
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

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������

        IsSelected = true;

        //��ư ������ ����
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
            else if (50 < rand && rand < 100)   // ������ �Դ� �б�
            {
                int damgeRange = Random.Range(7, 13);
                GetDamage(damgeRange);
                FindObjectOfType<TopUI>().UpdateTopUI();
                nextBranch = 5;
            }
        }

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������

        IsSelected = true;

        //��ư ������ ����
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
        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������

        IsSelected = true;

        //��ư ������ ����
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
        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������

        IsSelected = true;

        //��ư ������ ����
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
        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������


        IsSelected = true;
        //��ư ������ ����
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

        if (GameManager.instance.PlayerItemList.Count != 0)     //�������� �������
        {
            int rand = Random.Range(0, GameManager.instance.PlayerItemList.Count);
            //lostItemname = GameObject.Find("TopUI").transform.Find("Items_pas").transform.Find("item" + rand).GetComponent<TopUI_Item>().itemName;
            lostItemNum = GameObject.Find("TopUI").transform.Find("Items_pas").transform.Find("item" + rand).GetComponent<TopUI_Item>().itemNum;
            // �ҽ��� ������ �̸� ����
            //ES3.Save<string>("LostItemName", "'" + lostItemname + "'", "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");
            Debug.LogError("�ҽǵ�!" + lostItemname);
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
            ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������
        }
        if (branch == 13)
        {
            nextBranch = 14;
            ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������
        }

        IsSelected = true;
        //��ư ������ ����
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

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������

        IsSelected = true;

        //��ư ������ ����
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

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������

        IsSelected = true;

        //��ư ������ ����
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
            // ����ũ �԰� �ִ� ü�� ���
            if (!TopUI.instance.FindItemNum(32)) // �� ������ ������ ���� ó��
            {
                Player.instance.PlayerMaxHP(5);
                //popupPanel.SetActive(true);
                //getGoldPanel.SetActive(true);
                //getGoldPanel.transform.GetChild(1).GetComponent<Text>().text = "�ִ�ü���� 5 �����ߴ�!";
            }
            else
            {
                //Player.instance.HealHP(5);
                //popupPanel.SetActive(true);
                //getGoldPanel.SetActive(true);
                //getGoldPanel.transform.GetChild(1).GetComponent<Text>().text = "�ִ�ü���� 5 �����ߴ�!";
            }

            TextTranslate goldText = getGoldPanel.transform.GetChild(1).GetComponent<TextTranslate>();
            goldText.textNum = 61;
            getGoldPanel.SetActive(true);
        }
        else
        {
            nextBranch = 23;
            // ����ũ �԰� ������ ����
            GetDamage(5);
            FindObjectOfType<TopUI>().UpdateTopUI();
        }

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������
        IsSelected = true;

        //��ư ������ ����
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

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������
        IsSelected = true;

        //��ư ������ ����
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

        // ������ �ֱ�
        StartCoroutine(TopUI.instance.DiscardItemPanel());
        // ���� ���� ���
        yield return new WaitUntil(() => FindObjectOfType<ItemBtn_Event>().isGive);

        // ����� ���� �귣ġ ����
        if (FindObjectOfType<ItemBtn_Event>().isSuccess)
        {
            nextBranch = 27;
        }
        else
        {
            nextBranch = 28;
        }
        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������
        IsSelected = true;

        //��ư ������ ����
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
        // ����ī�� �г� ����
        if (!EndlessManager.instance.isEndless)
        {
            GameObject.Find("RewardCanvas").transform.Find("MagicGiveCombiePanel").gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("RewardCanvas_endless").transform.Find("MagicGiveCombiePanel").gameObject.SetActive(true);
        }

        // ���� ���� ���
        yield return new WaitUntil(() => FindObjectOfType<RestBtn>().isGive);

        // ����� ���� �귣ġ ����
        if (FindObjectOfType<ItemBtn_Event>().isSuccess)
        {
            nextBranch = 30;
        }
        else
        {
            nextBranch = 31;
        }
        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������
        IsSelected = true;

        //��ư ������ ����
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

        int num = Random.Range(50, 201);        //-100 ��� +150~300��� = 50~200
        popupPanel.SetActive(true);
        //getGoldPanel.SetActive(true);
        //getGoldPanel.transform.GetChild(1).GetComponent<Text>().text = num+100 + "��带 ŉ���Ͽ���."; 
        
        TextTranslate goldText = getGoldPanel.transform.GetChild(1).GetComponent<TextTranslate>();
        goldText.textNum = 59;
        goldText.plusTexts.plusFrontTextKr = (num + 100).ToString();
        goldText.plusTexts.plusFrontTextEn = (num + 100).ToString();
        getGoldPanel.SetActive(true);

        Player.instance.PlayerGetGold(num);

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������
        IsSelected = true;

        //��ư ������ ����
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

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������
        IsSelected = true;

        //��ư ������ ����
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

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������
        IsSelected = true;

        //��ư ������ ����
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

        //�˾� ���Ÿ� ���
        /*     popupPanel.SetActive(true);
             getGoldPanel.SetActive(true);
         getGoldPanel.transform.GetChild(1).GetComponent<Text>().text = EventManager.instance.LostGold + "��带 ��������.";*/

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������
        IsSelected = true;

        //��ư ������ ����
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

        if (val == 1)       //���
        {
            GetGold();
            nextBranch = 38;
        }
        else if (val == 2)  //����
        {
            GetItem_Act();
            nextBranch = 39;
        }
        else if (val == 3)  //��Ŀ
        {
            GetJokerCard();
            nextBranch = 40;
        }

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������
        IsSelected = true;

        //��ư ������ ����
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
        //getGoldPanel.transform.GetChild(1).GetComponent<Text>().text = "20 ü���� ȸ���ߴ�!";
        int num = 20;

        TextTranslate goldText = getGoldPanel.transform.GetChild(1).GetComponent<TextTranslate>();
        goldText.textNum = 62;
        goldText.plusTexts.plusFrontTextKr = "20";
        goldText.plusTexts.plusFrontTextEn = "20";        
        getGoldPanel.SetActive(true);

        FindObjectOfType<TopUI>().UpdateTopUI();

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������
        IsSelected = true;

        //��ư ������ ����
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

        //getDamagePanel.transform.GetChild(1).GetComponent<Text>().text ="5 �������� �Ծ���.";

        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������
        IsSelected = true;

        //��ư ������ ����
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

        // ����ī�� �г� ����
        if (!EndlessManager.instance.isEndless)
        {
            GameObject.Find("RewardCanvas").transform.Find("MagicGiveCombiePanel").gameObject.SetActive(true);
        }
        else
        {
            GameObject.Find("RewardCanvas_endless").transform.Find("MagicGiveCombiePanel").gameObject.SetActive(true);
        }

        // ���� ���� ���
        yield return new WaitUntil(() => FindObjectOfType<RestBtn>().isGive);
        nextBranch = 46;
        EventManager.instance.Trade_item = false;
        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������
        IsSelected = true;

        //��ư ������ ����
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
        //TopUI.instance.itemDiscardPanel.transform.GetChild(2).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "������ �Ǹ�";
        StartCoroutine(TopUI.instance.DiscardItemPanel(63));        

        yield return new WaitUntil(() => FindObjectOfType<ItemBtn_Event>().isGive);

        nextBranch = 46;
        EventManager.instance.Trade_item = true;
        ES3.Save<int>("NextBrach", nextBranch, "./SaveData/SaveFile" + GameManager.instance.nowSlot + ".es3");  // ���� -�ڿ��� �귣ġ ���� -> ContinuitySelectTest���� ��������
        IsSelected = true;

        //��ư ������ ����
        btn[0].onClick.RemoveAllListeners();
        btn[1].onClick.RemoveAllListeners();
        btn[2].onClick.RemoveAllListeners();

        yield return null;
    }
}
//ȭ�� ����
[System.Serializable]
public struct SelectSpeaker
{
    public Image image;                             //ĳ���� �̹���
    public Sprite[] imageNum;                   //ĳ���� �̹��� ��������Ʈ ���� �迭
    public Image imageDialog;                   //��ȭâ Iamge UI
    //public TextMeshProUGUI textName;
    //public TextMeshProUGUI textDialog;
    public TextTranslate textName;            //���� ������� ĳ���� �̸� ��� Text UI
    public TextTranslate textDialog;          //���� ��� ��� Text UI
    public GameObject Panel;
}
//��ȭ����
[System.Serializable]
public struct SelectData
{
    public int imageNum;            //ĳ���� �̹��� ��������Ʈ ��ȣ
    public string name;             //ĳ���� �̸�
    [TextArea(3, 5)]
    public string dialogue;         //���
    public int btnCount;            //��ư �� ��
    public string[] buttonText;     //��ư �ؽ�Ʈ 
}

