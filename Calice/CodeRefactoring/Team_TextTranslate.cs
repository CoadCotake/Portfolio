using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TextTranslate : MonoBehaviour
{
    public int textNum;

    public Text targetText;
    public TMP_Text targetTextTMP;

    private TextDB textDB;
    private ItemDB itemDB;

    public bool AwakeCheck = false;    //기초값 세팅이 끝났는지 확인

    [Header("[툴팁은 체크하기]")]
    public bool ToolTip = false;
    public string SpaceText="";

    [Header("[글꼴 설정 (따로 건들기 X)]")]
    [SerializeField] LanguageFont font;
    /// <summary>
    /// -1 = 일반 폰트 , 0 = tmp 일반 폰트, 1 = tmp 아웃라인 약함, 2 = tmp 아웃라인 쌤
    /// </summary>
    [SerializeField] int font_num;
   

    private void Awake()
    {
        PlusTexts plustext = new PlusTexts(plusTexts.plusFrontTextKr, plusTexts.plusFrontTextEn, new List<string>(), new List<string>(), plusTexts.plusLastTextKr, plusTexts.plusLastTextEn);
        plusTexts = plustext;

        if (GetComponent<Text>() != null)
        {
            targetText = GetComponent<Text>();
        }
        else if (GetComponent<TMP_Text>() != null)
        {
            targetTextTMP = GetComponent<TMP_Text>();
        }

        FontStartSetting();

        AwakeCheck = true;

        if(ToolTip)
        {
            SpaceText = "\n\n";
        }
        else
        {
            SpaceText = "";
        }
    }

    private void Start()
    {        
        TextTranslateManager.instance.allText.Add(this);

        textDB = TextTranslateManager.instance.textDB;        

        switch (textType)
        {
            case TextType.UI:
            case TextType.PatName:
            case TextType.PatEx:
                TranslateText();
                break;
        }
    }

    public void FontStartSetting()
    {
        if (targetText != null)
        {
            font_num = -1;
            font.NomalFont = targetText.font;
        }
        else if (targetTextTMP != null)
        {
            font_num = font.FontFindNumber(targetTextTMP.font.name);
            font.TMP_List[font_num] = targetTextTMP.font;
        }
        else
        {
            DebugX.LogError("폰트를 찾을 수 없음"+this.gameObject.name);
        }
    }

    public void FontRefresh()
    {
        switch (Collection.instance.textLanguage)
        {
            case 0:                
            case 1:
                if (targetText != null)
                {
                    targetText.font= font.NomalFont;
                }
                else if (targetTextTMP != null)
                {
                    targetTextTMP.font = font.TMP_List[font_num];
                }
                else
                {
                    DebugX.LogError("폰트를 넣을 수 없음" + this.gameObject.name);
                }
                break;
            default:
                if (targetText != null)
                {
                    targetText.font = Collection.instance.Fonts[Collection.instance.textLanguage].NomalFont;
                }
                else if (targetTextTMP != null)
                {
                    targetTextTMP.font = Collection.instance.Fonts[Collection.instance.textLanguage].TMP_List[font_num];
                }
                else
                {
                    DebugX.LogError("추가 폰트를 넣을 수 없음" + this.gameObject.name);
                }
                break;
        }
    }

    public string ReturnText()
    {
        if (GetComponent<Text>() != null)
        {
            return targetText.text;
        }
        else if (GetComponent<TMP_Text>() != null)
        {
            return targetTextTMP.text;
        }
        else
        {
            return "";
        }
    }

    public void ChangeText(string changeText)
    {
        if (targetText != null)
        {
            targetText.text = changeText;
        }
        else if (targetTextTMP != null)
        {
            targetTextTMP.text = changeText;
        }
    }

    public void TranslateText()
    {
        FontRefresh();

        if (textDB == null)
        {
            textDB = TextTranslateManager.instance.textDB;
        }

        switch (textType)
        {
            case TextType.UI:
                TranslateTextUI();
                break;

            case TextType.PasName:
            case TextType.PasEx:
                TranslateTextPas();
                break;

            case TextType.ActName:
            case TextType.ActEx:
                TranslateTextAct();
                break;

            case TextType.CardName:
            case TextType.CardEx:
                TranslateTextCard();
                break;

            case TextType.PatName:
            case TextType.PatEx:
                TranslateTextPattern();
                break;

            case TextType.Event:
                TranslateTextEvent();
                break;

            case TextType.DialogName:
            case TextType.Dialog:
                TranslateTextDialog();
                break;

            case TextType.DebuffName:
            case TextType.DebuffEx:
                TranslateTextEndlessDebuff();
                break;
        }
        /*
        if (plusTexts.plusMiddleTextKr == null || plusTexts.plusMiddleTextEn == null)
        {
            PlusTexts plustext = new PlusTexts(plusTexts.plusFrontTextKr, plusTexts.plusFrontTextEn, new List<string>(), new List<string>(), plusTexts.plusLastTextKr, plusTexts.plusLastTextEn);
            plusTexts = plustext;
        }
        */
        if (plusTexts.plusFrontTextKr != "")
        {
            PlusFrontText();
        }

        if (plusTexts.plusMiddleTextKr.Count > 0)
        {
            PlusMiddleText();
        }

        if (plusTexts.plusLastTextKr != "")
        {
            PlusLastText();
        }
    }

    private void TranslateTextUI()
    {
        if (textNum == -1 || textDB.UI[textNum] == null)
            return;

        string ui_text;
        TextDBUI ui_db = textDB.UI[textNum];

        switch (Collection.instance.textLanguage)
        {
            case 0:
                ui_text = ui_db.Kr;
                break;
            case 1:
                ui_text = ui_db.En;
                break;
            case 2:
                ui_text = ui_db.Jp;
                break;
            case 3:
                ui_text = ui_db.Cn;
                break;
            default:
                ui_text = "Error_ui_text";
                break;
        }

        if (targetText != null)
        {
            targetText.text = ui_text;
        }
        else if (targetTextTMP != null)
        {
            targetTextTMP.text = ui_text;
        }        
    }    

    private void TranslateTextPas()
    {
        if (textNum == -1 || textDB.Pas[textNum] == null)
            return;

        string pas_name;
        string pas_explain;
        TextDBPas pas_db= textDB.Pas[textNum];

        switch (Collection.instance.textLanguage)
        {
            case 0:
                pas_name = pas_db.KrName;
                pas_explain = pas_db.KrExplain;
                break;
            case 1:
                pas_name = pas_db.EnName;
                pas_explain = pas_db.EnExplain;
                break;
            case 2:
                pas_name = pas_db.JpName;
                pas_explain = pas_db.JpExplain;
                break;
            default:
                pas_name = "Error_pas_name";
                pas_explain = "Error_pas_explain";
                break;
        }

        if (targetText != null)
        {
            if (textType == TextType.PasName)
                targetText.text = pas_name + TierTextSet();
            else if (textType == TextType.PasEx)
                targetText.text = pas_explain + SpaceText;
        }
        else if (targetTextTMP != null)
        {
            if (textType == TextType.PasName)
                targetTextTMP.text = pas_name + TierTextSet();
            else if (textType == TextType.PasEx)
                targetTextTMP.text = pas_explain + SpaceText;
        }
       
    }

    private void TranslateTextAct()
    {
        if (textNum == -1 || textDB.Act[textNum] == null)
            return;

        string act_name;
        string act_explain;
        TextDBAct act_db = textDB.Act[textNum];

        switch (Collection.instance.textLanguage)
        {
            case 0:
                act_name = act_db.KrName;
                act_explain = act_db.KrExplain;
                break;
            case 1:
                act_name = act_db.EnName;
                act_explain = act_db.EnExplain;
                break;
            case 2:
                act_name = act_db.JpName;
                act_explain = act_db.JpExplain;
                break;
            default:
                act_name = "Error_act_name";
                act_explain = "Error_act_explain";
                break;
        }

        if (targetText != null)
        {
            if (textType == TextType.ActName)
                targetText.text = act_name + TierTextSet();
            else if (textType == TextType.ActEx)
                targetText.text = act_explain + SpaceText;
        }
        else if (targetTextTMP != null)
        {
            if (textType == TextType.ActName)
                targetTextTMP.text = act_name + TierTextSet();
            else if (textType == TextType.ActEx)
                targetTextTMP.text = act_explain + SpaceText;
        }
       
    }
    private void TranslateTextCard()
    {
        if (textNum == -1 || textDB.Card[textNum] == null)
            return;

        string card_name;
        string card_explain;
        TextDBCard card_db = textDB.Card[textNum];

        switch (Collection.instance.textLanguage)
        {
            case 0:
                card_name = card_db.KrName;
                card_explain = card_db.KrExplain;
                break;
            case 1:
                card_name = card_db.EnName;
                card_explain = card_db.EnExplain;
                break;
            case 2:
                card_name = card_db.JpName;
                card_explain = card_db.JpExplain;
                break;
            default:
                card_name = "Error_Card_name";
                card_explain = "Error_Card_explain";
                break;
        }

        if (targetText != null)
        {
            if (textType == TextType.CardName)
                targetText.text = card_name;
            else if (textType == TextType.CardEx)
                targetText.text = card_explain;
        }
        else if (targetTextTMP != null)
        {
            if (textType == TextType.CardName)
                targetTextTMP.text = card_name;
            else if (textType == TextType.CardEx)
                targetTextTMP.text = card_explain;
        }
       
    }
    private void TranslateTextPattern()
    {
        if (textNum == -1 || textDB.Pattern[textNum] == null)
            return;

        string parttern_name;
        string parttern_explain;
        TextDBPattern parttern_db= textDB.Pattern[textNum];

        switch (Collection.instance.textLanguage)
        {
            case 0:
                parttern_name = parttern_db.KrName;
                parttern_explain = parttern_db.KrExplain;
                break;
            case 1:
                parttern_name = parttern_db.EnName;
                parttern_explain = parttern_db.EnExplain;
                break;
            case 2:
                parttern_name = parttern_db.JpName;
                parttern_explain = parttern_db.JpExplain;
                break;
            default:
                parttern_name = "Error_parttern_name";
                parttern_explain = "Error_parttern_explain";
                break;
        }

        if (targetText != null)
        {
            if (textType == TextType.PatName)
                targetText.text = parttern_name;
            else if (textType == TextType.PatEx)
                targetText.text = parttern_explain + SpaceText;
        }
        else if (targetTextTMP != null)
        {
            if (textType == TextType.PatName)
                targetTextTMP.text = parttern_name;
            else if (textType == TextType.PatEx)
                targetTextTMP.text = parttern_explain + SpaceText;
        }

    }

    private void TranslateTextEvent()
    {
        if (textNum == -1 || textDB.Event[textNum] == null)
            return;

        string event_name;
        string event_dialog;
        string event_btntext1;
        string event_btntext2;
        string event_btntext3;

        TextDBEvent event_db = textDB.Event[textNum];

        switch (Collection.instance.textLanguage)
        {
            case 0:
                event_name = event_db.name;
                event_dialog = event_db.dialog;
                event_btntext1 = event_db.btnText1;
                event_btntext2 = event_db.btnText2;
                event_btntext3 = event_db.btnText3;
                break;
            case 1:
                event_name = event_db.name_en;
                event_dialog = event_db.dialog_en;
                event_btntext1 = event_db.btnText1_en;
                event_btntext2 = event_db.btnText2_en;
                event_btntext3 = event_db.btnText3_en;
                break;
            case 2:
                event_name = event_db.name_jp;
                event_dialog = event_db.dialog_jp;
                event_btntext1 = event_db.btnText1_jp;
                event_btntext2 = event_db.btnText2_jp;
                event_btntext3 = event_db.btnText3_jp;
                break;
            default:
                event_name = "Error_event_name";
                event_dialog = "Error_event_dialog";
                event_btntext1 = "Error_event_btntext1";
                event_btntext2 = "Error_event_btntext2";
                event_btntext3 = "Error_event_btntext3";
                break;
        }

        if (targetText != null)
        {
            if (eventTextType == EventTextType.name)
                targetText.text = event_name;
            else if (eventTextType == EventTextType.dialog)
                targetText.text = event_dialog;
            else if (eventTextType == EventTextType.btnText1)
                targetText.text = event_btntext1;
            else if (eventTextType == EventTextType.btnText2)
                targetText.text = event_btntext2;
            else if (eventTextType == EventTextType.btnText3)
                targetText.text = event_btntext3;
        }
        else if (targetTextTMP != null)
        {
            if (eventTextType == EventTextType.name)
                targetTextTMP.text = event_name;
            else if (eventTextType == EventTextType.dialog)
                targetTextTMP.text = event_dialog;
            else if (eventTextType == EventTextType.btnText1)
                targetTextTMP.text = event_btntext1;
            else if (eventTextType == EventTextType.btnText2)
                targetTextTMP.text = event_btntext2;
            else if (eventTextType == EventTextType.btnText3)
                targetTextTMP.text = event_btntext3;
        }
       
    }
    private void TranslateTextDialog()
    {
        if (textNum == -1 || textDB.Dialog[textNum] == null)
            return;

        string dialog_name;
        string dialog_explain;
        TextDBDialog dialog_db = textDB.Dialog[textNum];

        switch (Collection.instance.textLanguage)
        {
            case 0:
                dialog_name = dialog_db.name;
                dialog_explain = dialog_db.dialog;
                break;
            case 1:
                dialog_name = dialog_db.name_en;
                dialog_explain = dialog_db.dialog_en;
                break;
            case 2:
                dialog_name = dialog_db.name_jp;
                dialog_explain = dialog_db.dialog;
                break;
            default:
                dialog_name = "Error_dialog_name";
                dialog_explain = "Error_dialog_explain";
                break;
        }

        if (targetText != null)
        {
            if (textType == TextType.DialogName)
                targetText.text = dialog_name;
            else if (textType == TextType.Dialog)
                targetText.text = dialog_explain;
        }
        else if (targetTextTMP != null)
        {
            if (textType == TextType.DialogName)
                targetTextTMP.text = dialog_name;
            else if (textType == TextType.Dialog)
                targetTextTMP.text = dialog_explain;
        }
        
    }

    private void TranslateTextEndlessDebuff()
    {
        if (textNum == -1 || textDB.UI[textNum] == null)
            return;

        EndlessSelectDebuff debuff_db = EndlessManager.instance.endlessSelectDB.Debuff[textNum];

        string debuff_name;
        string debuff_explain;

        switch (Collection.instance.textLanguage)
        {
            case 0:
                debuff_name = debuff_db.name;
                debuff_explain = debuff_db.explain;
                break;
            case 1:
                debuff_name = debuff_db.name_en;
                debuff_explain = debuff_db.explain;
                break;
            case 2:
                debuff_name = debuff_db.name_jp;
                debuff_explain = debuff_db.explain;
                break;
            default:
                debuff_name = "Error_debuff_name";
                debuff_explain = "Error_debuff_explain";
                break;
        }

        if (targetText != null)
        {
            if (textType == TextType.DebuffName)
                targetText.text = debuff_name;
            else if (textType == TextType.DebuffEx)
                targetText.text = debuff_explain;
        }
        else if (targetTextTMP != null)
        {
            if (textType == TextType.DebuffName)
                targetTextTMP.text = debuff_name;
            else if (textType == TextType.DebuffEx)
                targetTextTMP.text = debuff_explain;
        }

    }

    public string TierTextSet()
    {
        if (itemDB == null)
        {
            itemDB = Collection.instance.ItemDB;
        }

        int tier = 0;
        switch (textType)
        {
            case TextType.PasName:
            case TextType.PasEx:
                tier = itemDB.Entities[textNum].tier;
                break;

            case TextType.ActName:
            case TextType.ActEx:
                tier = itemDB.Active[textNum].tier;
                break;
        }

        string tierText = "";
        switch (Collection.instance.textLanguage)
        {
            case 0:
                if (tier == 1)
                    tierText = " " + TextTranslateManager.instance.textDB.UI[12].Kr;
                else if (tier == 2)
                    tierText = " <color=#5de37e>" + TextTranslateManager.instance.textDB.UI[13].Kr + "</color>";
                else if (tier == 3)
                    tierText = " <color=#1e88e5>" + TextTranslateManager.instance.textDB.UI[14].Kr + "</color>";
                break;
            case 1:
                if (tier == 1)
                    tierText = " " + TextTranslateManager.instance.textDB.UI[12].En;
                else if (tier == 2)
                    tierText = " <color=#5de37e>" + TextTranslateManager.instance.textDB.UI[13].En + "</color>";
                else if (tier == 3)
                    tierText = " <color=#1e88e5>" + TextTranslateManager.instance.textDB.UI[14].En + "</color>";
                break;
        }

        return tierText;
    }

    public void PlusFrontText()
    {
        switch (Collection.instance.textLanguage)
        {
            case 0:
                if (targetText != null)
                {
                    targetText.text = plusTexts.plusFrontTextKr + " " + targetText.text;
                }
                else if (targetTextTMP != null)
                {
                    targetTextTMP.text = plusTexts.plusFrontTextKr + " " + targetTextTMP.text;
                }
                break;
            case 1:
                if (targetText != null)
                {
                    targetText.text = plusTexts.plusFrontTextEn + " " + targetText.text;
                }
                else if (targetTextTMP != null)
                {
                    targetTextTMP.text = plusTexts.plusFrontTextEn + " " + targetTextTMP.text;
                }
                break;
        }
    }

    public void PlusMiddleText()
    {
        string[] splitList;

        int startSplit;
        string splitFront;
        string splitBack;

        switch (Collection.instance.textLanguage)
        {
            case 0:
                for (int num = 0; num < plusTexts.plusMiddleTextKr.Count; num++)
                {
                    if (targetText != null)
                    {
                        startSplit = targetText.text.IndexOf("X");
                        splitFront = targetText.text.Substring(0, startSplit);
                        splitBack = targetText.text.Substring(startSplit + 1);

                        targetText.text = splitFront + " " + plusTexts.plusMiddleTextKr[num] + " " + splitBack;
                    }
                    else if (targetTextTMP != null)
                    {
                        startSplit = targetTextTMP.text.IndexOf("X");
                        splitFront = targetTextTMP.text.Substring(0, startSplit);
                        splitBack = targetTextTMP.text.Substring(startSplit + 1);

                        targetTextTMP.text = splitFront + " " + plusTexts.plusMiddleTextKr[num] + " " + splitBack;

                    }
                }
                break;
            case 1:
                for (int num = 0; num < plusTexts.plusMiddleTextEn.Count; num++)
                {
                    if (targetText != null)
                    {
                        startSplit = targetText.text.IndexOf("X");
                        splitFront = targetText.text.Substring(0, startSplit);
                        splitBack = targetText.text.Substring(startSplit + 1);

                        targetText.text = splitFront + " " + plusTexts.plusMiddleTextEn[num] + " " + splitBack;
                    }
                    else if (targetTextTMP != null)
                    {
                        startSplit = targetTextTMP.text.IndexOf("X");
                        splitFront = targetTextTMP.text.Substring(0, startSplit);
                        splitBack = targetTextTMP.text.Substring(startSplit + 1);

                        targetTextTMP.text = splitFront + " " + plusTexts.plusMiddleTextEn[num] + " " + splitBack;
                    }
                }
                break;
        }
    }

    public void PlusLastText()
    {
        switch (Collection.instance.textLanguage)
        {
            case 0:
                if (targetText != null)
                {
                    targetText.text = targetText.text + " " + plusTexts.plusLastTextKr;
                }
                else if (targetTextTMP != null)
                {
                    targetTextTMP.text = targetTextTMP.text + " " + plusTexts.plusLastTextKr;
                }
                break;
            case 1:
                if (targetText != null)
                {
                    targetText.text = targetText.text + " " + plusTexts.plusLastTextEn;
                }
                else if (targetTextTMP != null)
                {
                    targetTextTMP.text = targetTextTMP.text + " " + plusTexts.plusLastTextEn;
                }
                break;
        }
    }

    [Header("[텍스트 세팅]")]
    public PlusTexts plusTexts;
    [System.Serializable]
    public struct PlusTexts
    {
        public PlusTexts(string Fk, string Fe, List<string> Mk, List<string> Me, string Lk, string Le)
        {
            plusFrontTextKr = Fk;
            plusFrontTextEn = Fe;
            plusMiddleTextKr = Mk;
            plusMiddleTextEn = Me;
            plusLastTextKr = Lk;
            plusLastTextEn = Le;
        }

        public string plusFrontTextKr;
        public string plusFrontTextEn;

        public List<string> plusMiddleTextKr;
        public List<string> plusMiddleTextEn;

        public string plusLastTextKr;
        public string plusLastTextEn;


    }

    public TextType textType;
    public enum TextType
    {
        UI,
        PasName,
        PasEx,
        ActName,
        ActEx,
        CardName,
        CardEx,
        PatName,
        PatEx,
        Event,
        DialogName,
        Dialog,
        DebuffName,
        DebuffEx
    }

    [Header("[TextType.Event 전용]")]
    public EventTextType eventTextType;
    public enum EventTextType
    {
        name,
        dialog,
        btnText1,
        btnText2,
        btnText3,
    }
}
