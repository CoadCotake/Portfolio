using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System;
using Random = UnityEngine.Random;
using System.Linq;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEditor.Rendering;
using static UnityEngine.Rendering.VolumeComponent;
using static GamePlayBtn;
using UnityEngine.UIElements;

public class combinationskill : MonoBehaviour
{
    public GameObject previouscard;
    public GameObject childcombinationskill;
    GameObject combinationpanel;
    GameObject mycardpanel;
    GameObject gamepanel;
    public GameObject Xbutton;

    public string type;
    public int damagerule;
    public int damages;
    public GameObject[] cardslot;
    public int cardslotcount;
    public int serialnumber;
    public int[] cardnum;

    public bool extinction;

    public int plusdamage;

    public int mydecknumber;
    float hitdamage;  //넣을 데미지


    float randomx, randomy; //이펙트 랜덤 위치

    bool isShieldCrack = false;

    public GameObject LastTarget; // 타겟
    GameObject LastTarget_before=null; // 이전 타겟 고무공 카드 변수로 사용함
    [SerializeField]GameObject[] Enemies;
    int random;

    //카드 변수
    bool card84 = false;
    GameObject card84_target = null;
    int card84_dmg=0;

    bool isUsed = false;

    int insint; //인트형 임시 변수 캐싱용
    int jokercount;
    int equlnum;

    void Start()
    {
        Xbutton.SetActive(true);
        combinationpanel = GameObject.Find("combinationpanel");
        mycardpanel = GameObject.Find("mycardPanel");
        childcombinationskill = transform.gameObject;
        gamepanel = GameObject.Find("gamepanel");

        previouscard = combinationpanel.GetComponent<combinationpanel>().thiscombinationcard;

        cardslotcount = cardslot.Length;
        cardnum = new int[cardslotcount];

        if (GameObject.Find("GameManager").GetComponent<ItemForce>().openX == true)
        {
            Xbutton.SetActive(true);
        }

        this.gameObject.transform.localScale = new Vector2(1, 1);

    }

    public void Update()
    {
        if (Player.instance.isBattle == false && !isUsed)
            previousCard();

        if (Player.instance.isCombiLimit && !isUsed)
            previousCard();
    }

    public void previousCard() //x버튼을 누를 때 카드 넣을 슬롯에 아직 카드가 남아있다면 카드반환하는 함수
    {
        Transform T = this.transform;
        foreach (Transform tr in T)
        {
            if (tr.tag == "gameslot")
            {
                //a.Add(tr.gameObject);
                if (tr.childCount > 1)
                {
                    GameObject a = tr.transform.GetChild(1).gameObject;
                    //들어있던 숫자 카드들 돌려보내기
                    mycardpanel.GetComponent<mycard>().myhandcard.Add(tr.transform.GetChild(1).gameObject);
                    tr.transform.GetChild(1).SetParent(mycardpanel.transform);

                    DebugX.Log(a.ToString());
                    a.transform.localScale = new Vector2(1, 1);
                    //정렬
                    mycardpanel.GetComponent<mycard>().align();
                }
            }
        }

        // 사용하려던 조합카드 반환
        //previouscard = combinationpanel.GetComponent<combinationpanel>().thiscombinationcard;
        combinationpanel.GetComponent<combinationpanel>().combinationcarddeck.Add(previouscard);
        previouscard.SetActive(true);
        previouscard.transform.SetParent(combinationpanel.transform);

        //정렬
        combinationpanel.GetComponent<combinationpanel>().align();

        if (cardslot[0].GetComponent<CardDrop>().rule==dropcardrule.rule.accumulate)    //만약 한 슬롯에 카드를 다 넣는 거라면
        {
            //사라진 카드들을 다시 돌려준다.
            foreach (var card in cardslot[0].GetComponent<CardDrop>().UseCardList)
            {
                mycard.instance.othercarddraw(card);
            }
            mycard.instance.align();
        }

        Destroy(transform.gameObject);

        DebugX.Log(previouscard.name);
    }

    public void ReturnCard() //카드효과를 발동하고도 다시 손패에 들어오는카드
    {
        DebugX.Log(childcombinationskill.ToString());
        Transform T = this.transform;
        foreach (Transform tr in T)
        {
            if (tr.tag == "gameslot")
            {
                //a.Add(tr.gameObject);
                if (tr.childCount > 1)
                {

                }
            }
        }

        // 사용하려던 조합카드 반환
        //previouscard = combinationpanel.GetComponent<combinationpanel>().thiscombinationcard;
        combinationpanel.GetComponent<combinationpanel>().combinationcarddeck.Add(previouscard);
        previouscard.SetActive(true);
        previouscard.transform.SetParent(combinationpanel.transform);

        //정렬
        combinationpanel.GetComponent<combinationpanel>().align();


        Destroy(transform.gameObject);

        DebugX.Log(previouscard.name);
    }

    public void damage()
    {
        isUsed = true;
        bool isSuccess = true;

        if (Player.instance.cardFailProb != 0)
        {
            float successProb = Random.Range(0f, 100f);
            if (successProb < Player.instance.cardFailProb)
                isSuccess = false;
        }

        var companel = combinationpanel.GetComponent<combinationpanel>();
        var cardpanel = mycardpanel.GetComponent<mycard>();
        int check = 0;
        int c = 0;
        foreach (var a in cardslot)
        {
            if (a.transform.childCount > 1)
            {
                cardnum[c] = (int)a.transform.GetChild(1).GetComponent<Card>().number;
            }


            if (a.GetComponent<CardDrop>().rulecheck == true)
            {

                check++;
            }
            c++;
        }

        #region 카드 효과
        if (check == cardslotcount)
        {
            if (cardslot[0].GetComponent<CardDrop>().rule == dropcardrule.rule.equal)   //같은 숫자넣는 슬롯일 경우 - 조커처리
            {
                DebugX.Log("같은 숫자 규칙 조커처리");

                jokercount = 0;
                equlnum = 0;

                for (int Cd=0; Cd<cardnum.Length; Cd++)
                {
                    if (cardnum[Cd]==14)    //조커 인 경우
                    {
                        jokercount++;
                    }
                    else
                    {
                        equlnum = cardnum[Cd];  //룰 숫자 저장
                    }
                }

                DebugX.Log("같은 숫자 규칙 조커개수"+ jokercount);
                if (cardnum.Length == 3)        //슬롯이 3개일 경우
                {
                    if (jokercount == 3)     //전부 조커일 경우
                    {
                        cardnum[0] = 10;
                        cardnum[1] = 10;
                        cardnum[2] = 10;
                    }
                    else  //전부 조커가 아닐 경우
                    {
                        cardnum[0] = equlnum;
                        cardnum[1] = equlnum;
                        cardnum[2] = equlnum;
                    }
                }
                else if(cardnum.Length==2)      //2개일 경우
                {
                    if (jokercount == 2)     //전부 조커일 경우
                    {
                        cardnum[0] = 10;
                        cardnum[1] = 10;
                    }
                    else  //전부 조커가 아닐 경우
                    {
                        cardnum[0] = equlnum;
                        cardnum[1] = equlnum;
                    }
                }
            }

            DebugX.Log("damagerule" + damagerule);

            if (Player.instance.isCombiCoercion)
            {
                if (serialnumber == Player.instance.combiCoercion)
                    Player.instance.isCombiCoercion = false;
            }


            if (Player.instance.isNormalCoercion)
            {
                for (int i = 0; i < cardslotcount; i++)
                {
                    if (cardnum[i] == Player.instance.normalCoercion)
                    {
                        Player.instance.isNormalCoercion = false;
                        break;
                    }
                }
            }

            switch (damagerule)
            {
                //cardnum[0] cardnum[1] cardnum[2]
                //맨 왼쪽부터 0 , 1 , 2 순서대로 계산 
                //슬롯 1칸일 경우 cardnum[0]만 사용
                //슬롯 2칸일 경우 cardnum[0], cardnum[1] 사용
                //슬롯 3칸일 경우 cardnum[0], cardnum[1], cardnum[2] 사용

                #region 일반 0~
                case 0:
                    hitdamage += damages;
                    break;
                case 1:
                    hitdamage += cardnum[0];             //첫번째 슬롯에 카드 넣은 값

                    break;
                case 2:
                    foreach (var a in cardnum)
                    {
                        hitdamage += a;                 //들어간 모든 카드 합산형 데미지
                    }


                    break;
                case 3:
                    hitdamage += cardnum[0] * 2;          //첫번째 슬롯에 카드 넣은 값 2배
                    break;

                case 4:
                    hitdamage += cardnum[0] * cardnum[0];  //첫번째 슬롯에 카드 넣은 값 제곱

                    break;
                case 5:
                    hitdamage += cardnum[0] + damages;   //카드 + 추가데미지
                    break;
                case 6:
                    hitdamage += cardnum[0] * cardnum[1]; // 두 수의 곱
                    break;

                case 8:
                    hitdamage += cardnum[0] / 2.0f;        //넣은 수의 절반
                    break;
                case 9:
                    hitdamage += cardnum[0] * 1.5f;     //넣은 수의 1.5배
                    break;
                case 10:
                    hitdamage += cardnum[0] * cardnum[1] / 2.0f; // 두 수의 곱 / 2
                    break;
                case 11:
                    hitdamage += Mathf.Ceil((cardnum[0] * cardnum[1] * cardnum[2])/4f);
                    break;

                case 12:
                    hitdamage += cardnum[0] + cardnum[1];  // 두수의 합
                    break;

                case 13:    //n에서 넣은 숫자를 뺀만큼 적을 공격한다.
                    hitdamage = damages - cardnum[0];
                    break;

                case 14:    //숫자를 넣어 10에서 숫자를 뺀 수의 n배만큼 적 모두에게 피해를 입힌다.
                    hitdamage = (10 - cardnum[0]) * damages;
                    break;

                case 20:
                    hitdamage += companel.previousDamage / 2.0f; //이전턴 반절 데미지
                    DebugX.Log("hitdamage : " + hitdamage);
                    break;
                case 21:
                    hitdamage += companel.previousDamage; //이전턴  데미지
                    DebugX.Log("hitdamage : " + hitdamage);
                    break;

                case 22:    //숫자를 넣어 n에서 숫자를 나눈만큼
                    hitdamage = damages / cardnum[0];
                    break;
                #endregion

                #region 특수공격 100~
                case 101:
                    hitdamage += damages;
                    ReturnCard(); //다시 손패에 들어오기
                    break;
                case 102:
                    if (Player.instance.PlayerGold > 0) hitdamage += Player.instance.PlayerGold / 5.0f;
                    else hitdamage = 0;
                    break;
                case 103:
                    if (Player.instance.PlayerGold > 0) hitdamage += Player.instance.PlayerGold / 10.0f;
                    else hitdamage = 0;
                    break;

                case 104: //체력5 소모 및 100데미지
                    if (Player.instance.PlayerHp > 3)
                    {
                        Player.instance.PlayerHp -= 3;
                        Player.instance.PlayerHpBarShow();
                        hitdamage = 100;
                    }
                    else
                    {
                        Player.instance.PlayerHp = 0;
                        Player.instance.PlayerHpBarShow();
                        Player.instance.PlayerGameOver();
                    }

                    break;

                case 105:
                    isShieldCrack = true; //쉴드 부술건지
                    break;

                case 106: //고정데매지 + 랜덤 홀수 카드 득
                    hitdamage += damages;
                    int randomOdd = Random.Range(1, 6) * 2 - 1; //홀수 추출
                    cardpanel.othercarddraw(randomOdd);
                    break;

                case 107:
                    hitdamage += damages;
                    int randomEven = Random.Range(1, 6) * 2; //짝수 추출
                    cardpanel.othercarddraw(randomEven);
                    break;
                case 108:   //숫자를 넣어 보유한 아이템의 수만큼 n값을 획득한다. (데미지, 방어막 적용)
                    DebugX.Log("보유한 아이템 수 패시브: " + GameManager.instance.PlayerItemList.Count);
                    hitdamage += GameManager.instance.PlayerItemList.Count;
                    break;
                case 109:   //넣은 숫자만큼 적을 공격한다. 뒤에 적이 있을 경우 넣은 숫자의 반만큼 뒤의 적을 공격한다.
                    hitdamage += cardnum[0];
                    card84 = true;
                    card84_dmg = cardnum[0] / 2;
                    break;
                case 110:   //숫자 1을 넣어 10%확률로 n만큼 적에게 피해를 입힌다.
                    random = Random.Range(0, 10);
                    DebugX.Log("0이 나오면 카드 효과" + random);
                    if (random == 0)
                    {
                        hitdamage += damages;
                    }
                    break;
                case 111:   //숫자를 넣어 보유한 아이템의 수의 n배만큼 적에게 피해를 입힌다.
                    hitdamage += GameManager.instance.PlayerItemList.Count * damages;
                    break;
                case 112:   //넣은 숫자만큼 적에게 피해를 입힌다. 사용한 숫자 카드는 반환된다.
                    hitdamage += cardnum[0];

                    if (cardslot[0].GetComponent<CardDrop>().joker != 0)
                    {
                        cardnum[0] = 14;
                    }

                    mycard.instance.othercarddraw(cardnum[0]);
                    mycard.instance.align();
                    break;
                case 113:   //세 숫자를 넣어 가장 큰 수와 작은 수의 곱만큼 적에게 피해를 입힌다.
                    int max;
                    int min;
                    max = cardnum[0];
                    min = cardnum[1];
                    if (cardnum[0] < cardnum[1])
                    {
                        max = cardnum[1];
                        min = cardnum[0];
                    }
                    if (max < cardnum[2])
                    {
                        max = cardnum[2];
                    }
                    else if (cardnum[2] < min)
                    {
                        min = cardnum[2];
                    }
                    hitdamage += max * min;
                    break;
                case 114:   //공석

                    break;
                case 115:   //보유한 골드의 1/n만큼 보호막을 획득한다. 보유 골드가 n보다 적을 시 1의 보호막을 획득한다.
                    if (Player.instance.PlayerGold < damages)
                    {
                        hitdamage += 1;
                    }
                    else
                    {
                        hitdamage += (int)Player.instance.PlayerGold / damages;
                    }
                    break;
                case 116:   //숫자를 넣어 손에 보유한 조합 카드 수만큼 보호막을 획득한다.
                    hitdamage += combinationpanel.GetComponent<combinationpanel>().MyhandCount();
                    break;
                case 117:   //숫자를 넣어 손에 보유한 조합 카드 수의 n배만큼 보호막을 획득한다.
                    hitdamage += combinationpanel.GetComponent<combinationpanel>().MyhandCount() * damages;
                    break;
                case 118:   //숫자를 넣어 손에 보유한 공격카드 수만큼 피해량 증가버프를 1턴 획득한다.
                    Player.instance.Card107 = combinationpanel.GetComponent<combinationpanel>().Myhand_AttackCardCount();
                    DebugX.Log("증가값" + Player.instance.Card107);
                    Player.instance.PlayerStateAdd("피해량 증가", Player.instance.Card107);
                    break;
                case 119:   //7이상의 세 숫자를 넣어 적의 보호막을 빼았는다.
                    break;
                #endregion

                #region 전체공격 300~
                case 301:
                    if (Player.instance.PlayerGold > 10) hitdamage += Player.instance.PlayerGold / 10.0f;
                    else hitdamage = 1;
                    break;
                case 302:
                    if (Player.instance.PlayerGold > 30) hitdamage += Player.instance.PlayerGold / 30.0f;
                    else hitdamage = 1;
                    break;
                case 303:
                    if (Player.instance.PlayerGold > 20) hitdamage += Player.instance.PlayerGold / 20.0f;
                    else hitdamage = 1;
                    break;

                case 304:
                    //150을 마리당으로 나눠 전체공격
                    GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    hitdamage = 150 / Enemies.Length;
                    break;

                case 305:   //넣은 수만큼 적 모두에게 피해를 입힌다.
                    hitdamage = cardnum[0];
                    break;

                case 306:   //숫자를 넣어 10에서 숫자를 뺀 수의 n배만큼 적 모두에게 피해를 입힌다.
                    hitdamage = (10 - cardnum[0]) * damages;
                    break;

                case 307:   //두 숫자를 넣어 두 수의 곱만큼 적 전체를 공격한다.
                    hitdamage = cardnum[0] * cardnum[1];
                    break;

                case 308:   //n만큼 적 모두에게 피해를 입힌다.
                    hitdamage = damages;
                    break;

                case 603:
                    hitdamage += cardnum[0] * 2;
                    ReturnCard();
                    break;
                #endregion

                #region 상태이상 800~
                case 800: //상태이상 거는 부분
                          //cardnum[0]이 넣은 카드의 숫자임
                    TargetEnemy();
                    LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("독", damages);
                    if (!LastTarget.GetComponent<EnemyBase>().poison)      //적이 독이 아닐 때
                    {
                        FindObjectOfType<StateEffect_e>().StateEffectOn(0, true, LastTarget);
                    }
                    //LastTarget.GetComponent<EnemyBase>().Hit();
                    EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Poison, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                    //FindObjectOfType<StateEffect_e>().StateEffectOn(0, true, LastTarget);
                    break;
                case 801: //상태이상 거는 부분
                          //cardnum[0]이 넣은 카드의 숫자임
                    TargetEnemy();
                    LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("화상", damages);
                    FindObjectOfType<StateEffect_e>().StateEffectOn(1, true, LastTarget);
                    break;
                case 802: //상태이상 거는 부분
                          //cardnum[0]이 넣은 카드의 숫자임
                    TargetEnemy();
                    LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("상처", damages);
                    FindObjectOfType<StateEffect_e>().StateEffectOn(2, true, LastTarget);
                    EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Bleed, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                    break;
                case 803: //상태이상 거는 부분
                          //cardnum[0]이 넣은 카드의 숫자임
                    TargetEnemy();
                    LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("마비", damages);
                    FindObjectOfType<StateEffect_e>().StateEffectOn(3, true, LastTarget);
                    EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Paralysis, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                    break;

                case 804:
                    int randoms = Random.Range(1, 3);
                    if (randoms == 1)
                    {
                        int randomArmor = Random.Range(1, 11);
                        Player.instance.PlayerGetArmor(randomArmor);
                    }
                    else
                    {
                        Player.instance.BlockDamage += 1;

                        EffectManager.instance.PlayerEffectOn("Invincible", Player.instance.transform.position);

                        Player.instance.PlayerStateAdd("피격무시", Player.instance.BlockDamage);
                    }

                    break;

                case 805:
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject target in enemies)
                    {
                        target.GetComponent<EnemyBase>().EnemyStateAdd("마비", 1);
                        FindObjectOfType<StateEffect_e>().StateEffectOn(3, true, target);
                        EffectManager.instance.EnemyCommoneEffectOn(target.name, CommonEffectName.Paralysis, target.GetComponent<EnemyBase>().enemyposion.position);
                    }
                    break;
                case 806: //보유한 골드의 1/n만큼 적에게 상태이상를 부여한다. 보유 골드가 n보다 적을 시 50%의 확률로 1턴 부여한다.

                    TargetEnemy();
                    if (Player.instance.PlayerGold < damages)
                    {
                        random = Random.Range(0, 2);
                        DebugX.Log("확률 0 나오면 카드효과 발동 " + random);
                        if (random == 0)
                        {
                            LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("마비", 1);
                            FindObjectOfType<StateEffect_e>().StateEffectOn(3, true, LastTarget);
                            EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Paralysis, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                        }
                    }
                    else
                    {
                        LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("마비", (int)Player.instance.PlayerGold / damages);
                        EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Paralysis, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                        FindObjectOfType<StateEffect_e>().StateEffectOn(3, true, LastTarget);
                    }
                    break;
                case 807: //보유한 골드의 1/n만큼 적에게 상태이상를 부여한다. 보유 골드가 n보다 적을 시 50%의 확률로 1턴 부여한다.

                    TargetEnemy();
                    if (Player.instance.PlayerGold < damages)
                    {
                        random = Random.Range(0, 2);
                        DebugX.Log("확률 0 나오면 카드효과 발동 " + random);
                        if (random == 0)
                        {
                            LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("화상", 1);
                            FindObjectOfType<StateEffect_e>().StateEffectOn(1, true, LastTarget);
                        }
                    }
                    else
                    {
                        LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("화상", (int)Player.instance.PlayerGold / damages);
                        FindObjectOfType<StateEffect_e>().StateEffectOn(1, true, LastTarget);
                    }
                    break;
                case 808: //보유한 골드의 1/n만큼 적에게 상태이상를 부여한다. 보유 골드가 n보다 적을 시 50%의 확률로 1턴 부여한다.

                    TargetEnemy();
                    if (Player.instance.PlayerGold < damages)
                    {
                        random = Random.Range(0, 2);
                        DebugX.Log("확률 0 나오면 카드효과 발동 " + random);
                        if (random == 0)
                        {
                            LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("독", 1);
                            if (!LastTarget.GetComponent<EnemyBase>().poison)      //적이 독이 아닐 때
                            {
                                FindObjectOfType<StateEffect_e>().StateEffectOn(0, true, LastTarget);
                            }
                            //LastTarget.GetComponent<EnemyBase>().Hit();
                            EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Poison, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                            //FindObjectOfType<StateEffect_e>().StateEffectOn(0, true, LastTarget);
                        }
                    }
                    else
                    {
                        LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("독", (int)Player.instance.PlayerGold / damages);
                        if (!LastTarget.GetComponent<EnemyBase>().poison)      //적이 독이 아닐 때
                        {
                            FindObjectOfType<StateEffect_e>().StateEffectOn(0, true, LastTarget);
                        }
                        //LastTarget.GetComponent<EnemyBase>().Hit();
                        EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Poison, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                        //FindObjectOfType<StateEffect_e>().StateEffectOn(0, true, LastTarget);
                    }
                    break;
                case 809: //보유한 골드의 1/n만큼 적에게 상태이상를 부여한다. 보유 골드가 n보다 적을 시 50%의 확률로 1턴 부여한다.

                    TargetEnemy();
                    if (Player.instance.PlayerGold < damages)
                    {
                        random = Random.Range(0, 2);
                        DebugX.Log("확률 0 나오면 카드효과 발동 " + random);
                        if (random == 0)
                        {
                            LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("상처", 1);
                            FindObjectOfType<StateEffect_e>().StateEffectOn(2, true, LastTarget);
                            EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Bleed, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                        }
                    }
                    else
                    {
                        LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("상처", (int)Player.instance.PlayerGold / damages);
                        EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Bleed, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                        FindObjectOfType<StateEffect_e>().StateEffectOn(2, true, LastTarget);
                    }
                    break;

                case 810:   //숫자를 넣어 n에서 넣은 숫자를 뺀만큼 적에게 화상을 부여한다.
                    TargetEnemy();                    
                    LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("화상", (damages-cardnum[0]));
                    LastTarget.GetComponent<EnemyBase>().Hit();
                    FindObjectOfType<StateEffect_e>().StateEffectOn(1, true, LastTarget);
                    EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Burn, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                    break;

                case 811:   //숫자를 넣어 n에서 넣은 숫자를 뺀만큼 적에게 독을 부여한다.
                    TargetEnemy();
                    LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("독", (damages - cardnum[0]));
                    if (!LastTarget.GetComponent<EnemyBase>().poison)      //적이 독이 아닐 때
                    {
                        FindObjectOfType<StateEffect_e>().StateEffectOn(0, true, LastTarget);
                    }
                    //LastTarget.GetComponent<EnemyBase>().Hit();
                    EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Poison, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                    break;

                case 812:   //숫자를 넣어 n에서 넣은 숫자를 뺀만큼 적에게 마비을 부여한다.
                    TargetEnemy();
                    LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("마비", (damages - cardnum[0]));
                    LastTarget.GetComponent<EnemyBase>().Hit();
                    FindObjectOfType<StateEffect_e>().StateEffectOn(3, true, LastTarget);
                    EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Paralysis, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                    break;

                case 813:   //숫자를 넣어 n에서 넣은 숫자를 뺀만큼 적에게 출혈을 부여한다.
                    TargetEnemy();
                    LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("상처", (damages - cardnum[0]));
                    LastTarget.GetComponent<EnemyBase>().Hit();
                    FindObjectOfType<StateEffect_e>().StateEffectOn(2, true, LastTarget);
                    EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Bleed, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                    break;
                case 815:   //숫자를 넣어 누적된 독 데미지를 2배로 한다.                   
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject target in enemies)
                    {
                        if (target.GetComponent<EnemyBase>().poison)      //적이 독일 때
                        {
                            target.GetComponent<EnemyBase>().poison_dmg *= 2;   //2배
                            target.GetComponent<EnemyBase>().EnemyStateAdd("독", 0, target.GetComponent<EnemyBase>().poison_dmg);
                            EffectManager.instance.EnemyCommoneEffectOn(target.name, CommonEffectName.Poison, target.GetComponent<EnemyBase>().enemyposion.position);
                        }
                    }
                    break;
                case 816:   //숫자를 넣어 누적된 마비 턴수를 2배로 한다.
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject target in enemies)
                    {
                        if (target.GetComponent<EnemyBase>().paralysis)
                        {
                            target.GetComponent<EnemyBase>().EnemyStateAdd("마비", damages, 0, true, false, true);
                            EffectManager.instance.EnemyCommoneEffectOn(target.name, CommonEffectName.Paralysis, target.GetComponent<EnemyBase>().enemyposion.position);                            
                        }
                    }
                    break;
                case 817:   //숫자를 넣어 화상의 피해량을 20으로 증가 시킨다.
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject target in enemies)
                    {
                        if (target.GetComponent<EnemyBase>().burn)      //적이 화상일 때
                        {
                            Player.instance.PlusStateUp(State.Burn, 1);
                            target.GetComponent<EnemyBase>().burn_dmg = damages;
                            target.GetComponent<EnemyBase>().EnemyStateAdd("화상", 0, damages);
                            EffectManager.instance.EnemyCommoneEffectOn(target.name, CommonEffectName.Burn, target.GetComponent<EnemyBase>().enemyposion.position);
                            GamePlayBtn.instance.TSF += target.GetComponent<EnemyBase>().RemovePlusBurnDmg;
                        }
                    }
                    break;
                case 818:   //숫자를 넣어 1턴 동안 상처의 확률을 100%로 한다.
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject target in enemies)
                    {
                        if (target.GetComponent<EnemyBase>().blood)      //적이 상처일 때
                        {
                            Player.instance.PlusStateUp(State.Blood, 1);
                            Player.instance.PlusBleedPercent = damages;
                            target.GetComponent<EnemyBase>().EnemyStateAdd("상처", 0, target.GetComponent<EnemyBase>().blood_dmg);
                            EffectManager.instance.EnemyCommoneEffectOn(target.name, CommonEffectName.Bleed, target.GetComponent<EnemyBase>().enemyposion.position);
                            GamePlayBtn.instance.TSF += target.GetComponent<EnemyBase>().RemovePlusBleedPercent;
                        }
                    }
                    break;
                case 819:   //숫자를 넣어 상처의 피해량을 50으로 증가시킨다.
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject target in enemies)
                    {
                        if (target.GetComponent<EnemyBase>().blood)      //적이 상처일 때
                        {
                            Player.instance.PlusStateUp(State.Blood, 1);
                            target.GetComponent<EnemyBase>().blood_dmg = damages;
                            target.GetComponent<EnemyBase>().EnemyStateAdd("상처", 0, damages);
                            EffectManager.instance.EnemyCommoneEffectOn(target.name, CommonEffectName.Bleed, target.GetComponent<EnemyBase>().enemyposion.position);
                            GamePlayBtn.instance.TSF += target.GetComponent<EnemyBase>().RemovePlusBleedDmg;
                        }
                    }
                    break;
                #endregion

                #region 특수 버프 900~
                case 903:
                    //상태이상 전체 해제

                    for (int i = 0; i < 5; i++)
                    {
                        GameObject debuff = Player.instance.PlayerState_debuff[0].transform.GetChild(i).gameObject;
                        if (debuff.activeSelf)
                        {
                            StateBox box = debuff.GetComponent<StateBox>();
                            int idx = Player.instance.stateDB.state_p.FindIndex(a => a.num == box.stateNum);
                            if (Player.instance.stateDB.state_p[idx].clean)
                            {
                                Player.instance.GetComponent<StateEffect>().StateEffectOn(box.stateNum, false);
                                box.ResetBox();
                                debuff.SetActive(false);
                            }
                        }
                    }
                    break;
                case 904:
                    // 상태이상 랜덤 1개 해제 
                    Player.instance.PlayerStateClean();

                    break;
                case 905: //피격1회방어
                    Player.instance.BlockDamage += 1;

                    EffectManager.instance.PlayerEffectOn("Invincible", Player.instance.transform.position);

                    Player.instance.PlayerStateAdd("피격무시", Player.instance.BlockDamage);
                    break;
                case 906: //반반확률로 힐 또는 실드
                    hitdamage += cardnum[0];
                    int r = Random.Range(1, 3);
                    if (r == 1) Player.instance.PlayerGetArmor(hitdamage);
                    else Player.instance.HealHP(hitdamage);
                    break;

                case 907:
                    //companel.isMonsterTime = false;

                    break;

                case 910://추가데미지 저장
                    companel.plusdamage += cardnum[0];
                    Player.instance.PlayerStateAdd("메아리", companel.plusdamage);
                    break;
                case 911:
                    for (int i = 0; i < 5; i++)
                    {
                        GameObject buff = Player.instance.PlayerState_buff[0].transform.GetChild(i).gameObject;
                        if (buff.activeSelf)
                        {
                            StateBox box = buff.GetComponent<StateBox>();
                            if (box.stateName == "피해량 배율")
                            {
                                box.ResetBox();
                                if (box.StackBox.activeSelf)
                                {
                                    box.StackBox.SetActive(false);
                                }
                                box.gameObject.SetActive(false);
                                break;
                            }

                        }
                    }

                    if (companel.damagemulltiple == 1)
                    {
                        companel.damagemulltiple += (cardnum[0] / 2.0f) - 1;
                        Player.instance.PlayerStateAdd("피해량 배율", companel.damagemulltiple);
                    }
                    else
                    {
                        companel.damagemulltiple += (cardnum[0] / 2.0f);
                        Player.instance.PlayerStateAdd("피해량 배율", companel.damagemulltiple);
                    }
                    break;

                case 912: //5데미지 반사 데미지 버프
                    Player.instance.boolreturnDamage = true;
                    Player.instance.returndamage_buff = Player.instance.stateDB.state_p[20].damage;
                    Player.instance.PlayerStateAdd("반사", 1);
                    break;

                case 913:   //1턴동안 타겟팅 가능
                    Player.instance.gameObject.GetComponent<SelectTarget>().enabled = true;
                    GamePlayBtn.instance.TSF += Player.instance.gameObject.GetComponent<SelectTarget>().RemoveSelectTarget;
                    break;

                case 914:   //숫자를 넣어 5~20 사이의 골드를 획득한다.                    
                    Player.instance.PlayerGetGold(Random.Range(5,20));
                    break;  

                case 915:   //두 숫자를 넣어 10~50 사이의 골드를 획득한다.
                    Player.instance.PlayerGetGold(Random.Range(10, 50));
                    break;

                case 916:   //이번 전투 타겟팅 가능
                    Player.instance.gameObject.GetComponent<SelectTarget>().enabled = true;
                    break;
                case 917:   //5이하의 숫자를 넣어 1턴 동안 넣은 숫자만큼 피해량을 증가시킨다.
                    Player.instance.Card107 = cardnum[0];
                    DebugX.Log("증가값" + Player.instance.Card107);
                    Player.instance.PlayerStateAdd("피해량 증가", Player.instance.Card107);
                    break;
                case 918    :   //숫자를 넣어 1턴 동안 그림자 버프를 획득한다.
                    Player.instance.Shadow = true;
                    Player.instance.PlayerStateAdd("그림자", damages);
                    break;
                #endregion

                #region 드로우 950~
                case 950:
                    //복사
                    //split공간
                    hitdamage += cardnum[0];
                    int split1, split2;

                    if (cardnum[0] == 1)
                    {
                        split1 = 1; //내림
                        split2 = 1; //반올림                                  
                    }
                    else
                    {
                        split1 = Mathf.FloorToInt(hitdamage / 2.0f); //내림
                        split2 = Mathf.CeilToInt(hitdamage / 2.0f); //반올림
                        DebugX.Log(split1 + " " + split2);
                    }

                    mycardpanel.GetComponent<mycard>().othercarddraw(split1);

                    mycardpanel.GetComponent<mycard>().othercarddraw(split2);
                    break;
                case 951: //첫번째카드 2개로 복제
                    if (cardslot[0].GetComponent<CardDrop>().joker >= 10)   //조커 일 경우
                    {
                        mycardpanel.GetComponent<mycard>().othercarddraw(14);
                        mycardpanel.GetComponent<mycard>().othercarddraw(14);
                    }
                    else
                    {
                        mycardpanel.GetComponent<mycard>().othercarddraw(cardnum[0]);
                        mycardpanel.GetComponent<mycard>().othercarddraw(cardnum[0]);
                    }
                    break;
                case 952: //첫번째카드 3개로 복제
                    if (cardslot[0].GetComponent<CardDrop>().joker >= 10)   //조커 일 경우
                    {
                        mycardpanel.GetComponent<mycard>().othercarddraw(14);
                        mycardpanel.GetComponent<mycard>().othercarddraw(14);
                        mycardpanel.GetComponent<mycard>().othercarddraw(14);
                    }
                    else
                    {
                        mycardpanel.GetComponent<mycard>().othercarddraw(cardnum[0]);
                        mycardpanel.GetComponent<mycard>().othercarddraw(cardnum[0]);
                        mycardpanel.GetComponent<mycard>().othercarddraw(cardnum[0]);
                    }
                    break;
                case 953:
                    int num = cardnum[0] * cardnum[1];
                    if (num > 10)
                    { // 10 넘으면 10이랑 일의자리숫자 각각하나씩 드로우
                        mycardpanel.GetComponent<mycard>().othercarddraw(num / 10 * 10);
                        mycardpanel.GetComponent<mycard>().othercarddraw(num % 10);
                    }
                    else mycardpanel.GetComponent<mycard>().othercarddraw(num);  //10이하이면 그 숫자 하나만 드로우
                    break;
                case 954:
                    int num1 = cardnum[0] + cardnum[1];
                    if (num1 > 10)
                    { // 10 넘으면 10이랑 일의자리숫자 각각하나씩 드로우
                        mycardpanel.GetComponent<mycard>().othercarddraw(10);
                        mycardpanel.GetComponent<mycard>().othercarddraw(num1 % 10);
                    }
                    else mycardpanel.GetComponent<mycard>().othercarddraw(num1);  //10이하이면 그 숫자 하나만 드로우
                    break;
                case 955:

                    if (cardnum[0] == (int)Cardinfo.number.JOKER)
                    {
                        if (cardnum[1] >= 6)
                        {
                            cardnum[0] = 1;
                        }
                        else if (cardnum[1] < 6)
                        {
                            cardnum[0] = 10;
                        }
                    }
                    else if (cardnum[1] == (int)Cardinfo.number.JOKER)
                    {
                        if (cardnum[0] >= 6)
                        {
                            cardnum[1] = 1;
                        }
                        else if (cardnum[0] < 6)
                        {
                            cardnum[1] = 10;
                        }
                    }
                    int num2 = Mathf.Abs(cardnum[0] - cardnum[1]);
                    if (num2 == 0)
                        break;
                    else
                        mycardpanel.GetComponent<mycard>().othercarddraw(num2);
                    break;

                case 956:
                    companel.DrawPanel.SetActive(true);
                    companel.DrawpanelOpen();
                    break;

                case 957:
                    GameObject.Find("CardCanvas").GetComponent<GamePlayBtn>().discoverpanel.GetComponent<discoverPanel>().discover_com(3);
                    break;

                case 958:
                    companel.notusecarddraw(1);
                    break;

                case 959:
                    int number1, number2, number3;
                    number1 = cardnum[0] / 3;
                    number2 = cardnum[0] / 3;
                    number3 = cardnum[0] / 3;

                    switch (cardnum[0])
                    {
                        case 4:
                        case 7:
                        case 10:
                            number1++;
                            break;
                        case 5:
                        case 8:
                            number1++;
                            number2++;
                            break;
                    }

                    mycardpanel.GetComponent<mycard>().othercarddraw(number1);
                    mycardpanel.GetComponent<mycard>().othercarddraw(number2);
                    mycardpanel.GetComponent<mycard>().othercarddraw(number3);
                    break;

                case 960:
                    ReturnCard();
                    companel.notusecarddraw(1);
                    break;

                case 961: // 이전에 사용한 조합카드 복사
                    if (companel.previousCardSerialNum != 0)
                        companel.drawserialone(companel.previousCardSerialNum);
                    break;

                case 962: //숫자를 -1하고 HP가 1 감소
                    mycardpanel.GetComponent<mycard>().othercarddraw(cardnum[0] - 1);
                    break;

                case 963: //숫자를 +1하고 HP가 1감소
                    mycardpanel.GetComponent<mycard>().othercarddraw(cardnum[0] + 1);
                    break;
                case 964:   //숫자를 넣어 5만큼 적에게 피해를 입힌다. 남은 숫자 카드에서 무작위로 n장을 뽑는다. 남은 숫자 카드가 없을 경우 뽑지 않는다.
                    hitdamage += 5;
                    mycard.instance.mydeck = mycard.instance.ShuffleList(mycard.instance.mydeck);
                    mycard.instance.normaldrawone(damages);
                    break;
                case 965:   //숫자를 넣어 10만큼 적에게 피해를 입힌다. 남은 조합 카드에서 무작위로 n장을 뽑는다. 남은 조합 카드가 없을 경우 뽑지 않는다.
                    hitdamage += 10;
                    combinationpanel.GetComponent<combinationpanel>().combinationcarddeck = combinationpanel.GetComponent<combinationpanel>().ShuffleList(combinationpanel.GetComponent<combinationpanel>().combinationcarddeck);
                    combinationpanel.GetComponent<combinationpanel>().notusecarddraw(damages);
                    break;
                case 966:   //숫자를 넣어 남은 숫자 카드에서 무작위 n장 중 1장을 선택하여 뽑는다. 남은 숫자 카드가 없을 경우 뽑지 않는다.
                    GameObject.Find("CardCanvas").GetComponent<GamePlayBtn>().discoverpanel.GetComponent<discoverPanel>().discover_num(damages);
                    break;
                case 967:   //숫자를 넣어 남은 숫자만큼 숫자를 다시 뽑는다. 남은 조합 카드가 없을 경우 뽑지 않는다.
                    num = mycard.instance.MyhandCount();
                    DebugX.Log("핸드 숫자카드" + num);
                    mycard.instance.MyhandReSet();
                    mycard.instance.ShuffleList(mycard.instance.mydeck);
                    mycard.instance.normaldrawone(num);
                    mycard.instance.align();
                    break;
                case 968:   //숫자를 넣어 남은 조합 카드에서 무작위로 n장을 뽑는다. 남은 조합 카드가 없을 경우 뽑지 않는다.
                    combinationpanel.GetComponent<combinationpanel>().combinationcarddeck = combinationpanel.GetComponent<combinationpanel>().ShuffleList(combinationpanel.GetComponent<combinationpanel>().combinationcarddeck);
                    combinationpanel.GetComponent<combinationpanel>().notusecarddraw(damages);
                    break;
                case 969:   //숫자를 넣어 다음 턴에 넣은 숫자를 추가로 획득한다.
                    GamePlayBtn.instance.DrawNumber.Add(cardnum[0]);
                    GamePlayBtn.instance.TSF += GamePlayBtn.instance.CardDraw_number;
                    break;
                case 970:   //조합카드가 n장이 될때까지 뽑는다. 남은 조합 카드가 없을 경우 뽑지 않는다.
                    int handnum=combinationpanel.GetComponent<combinationpanel>().MyhandCount()-1;  //사용한 카드 카운팅 제거
                    if(handnum<5)
                    {
                        combinationpanel.GetComponent<combinationpanel>().combinationcarddeck = combinationpanel.GetComponent<combinationpanel>().ShuffleList(combinationpanel.GetComponent<combinationpanel>().combinationcarddeck);
                        combinationpanel.GetComponent<combinationpanel>().notusecarddraw(5- handnum);
                    }
                    break;
                case 971:   //숫자를 넣어 1만큼 적에게 피해를 입힌다. 남은 숫자 카드에서 무작위로 n장을 뽑는다. 남은 숫자 카드가 없을 경우 뽑지 않는다.
                    hitdamage += 1;
                    mycard.instance.mydeck = mycard.instance.ShuffleList(mycard.instance.mydeck);
                    mycard.instance.normaldrawone(damages);
                    break;
                    #endregion
            }
            #endregion
            companel.UpdateUseComCardcount_Front(type);     //단순 카드 갯수만 셈 / 카드 타입 효과 터지기전 / 스택 쌓을 때 쓰는 함수

            if (isSuccess)
            {
                if (type == "Attack")
                {
                    Player.instance.AddCardCount(serialnumber, Player.CardType.Attack);

                    //데미지 없는 공격일 경우
                    if (damagerule == 119)  //방어막 뺏기
                    {
                        hitdamage = 0;
                        TargetEnemy();
                        Player.instance.PlayerGetArmor(LastTarget.GetComponent<EnemyBase>().EnemyArmor);
                        LastTarget.GetComponent<EnemyBase>().EnemyArmor = 0;
                        LastTarget.GetComponent<EnemyBase>().EnemyHpBarShow();
                    }
                    // 연타공격부분       그림자 버프 예외 처리 해주기
                    else if (damagerule == 201)
                    {
                        StartCoroutine(DamageEnemy(cardnum[0] / 2.0f, 2)); if (Player.instance.Shadow) { StartCoroutine(DamageEnemy(1, 2, true, true, Player.instance.ShadowDelay)); }
                        EffectManager.instance.PlayerEffectOn("MultiSlash", Player.instance.transform.position, 2);
                    }
                    else if (damagerule == 202)
                    {
                        StartCoroutine(DamageEnemy(cardnum[0], 2)); if (Player.instance.Shadow) { StartCoroutine(DamageEnemy(1, 2, true, true, Player.instance.ShadowDelay)); }
                        EffectManager.instance.PlayerEffectOn("MultiSlash", Player.instance.transform.position, 2);
                    }
                    else if (damagerule == 203)
                    {
                        StartCoroutine(DamageEnemy(damages, Mathf.CeilToInt(cardnum[0] / 2f))); if (Player.instance.Shadow) { StartCoroutine(DamageEnemy(1, Mathf.CeilToInt(cardnum[0] / 2f), true, true, Player.instance.ShadowDelay)); }
                        EffectManager.instance.PlayerEffectOn("MultiSlash", Player.instance.transform.position, Mathf.CeilToInt(cardnum[0] / 2f));
                    }
                    else if (damagerule == 204)
                    {
                        StartCoroutine(DamageEnemy(damages, cardnum[0])); if (Player.instance.Shadow) { StartCoroutine(DamageEnemy(1, cardnum[0], true, true, Player.instance.ShadowDelay)); }
                        EffectManager.instance.PlayerEffectOn("MultiSlash", Player.instance.transform.position, cardnum[0]);
                    }
                    else if (damagerule == 205)
                    {
                        StartCoroutine(DamageEnemy(damages, cardnum[0] * cardnum[1])); if (Player.instance.Shadow) { StartCoroutine(DamageEnemy(1, cardnum[0] * cardnum[1], true, true, Player.instance.ShadowDelay)); }
                        EffectManager.instance.PlayerEffectOn("MultiSlash", Player.instance.transform.position, cardnum[0] * cardnum[1]);
                    }
                    else if (damagerule == 206)
                    {
                        StartCoroutine(DamageEnemy(damages, cardnum[0])); if (Player.instance.Shadow) { StartCoroutine(DamageEnemy(1, cardnum[0], true, true, Player.instance.ShadowDelay)); }
                        EffectManager.instance.PlayerEffectOn("MultiSlash", Player.instance.transform.position, cardnum[0]);
                    }
                    else if (damagerule == 207)
                    {
                        StartCoroutine(DamageEnemy(damages, 11 - cardnum[0])); if (Player.instance.Shadow) { StartCoroutine(DamageEnemy(1, 11 - cardnum[0], true, true, Player.instance.ShadowDelay)); }
                        EffectManager.instance.PlayerEffectOn("MultiSlash", Player.instance.transform.position, 11 - cardnum[0]);
                    }
                    else if (damagerule == 208)
                    {
                        StartCoroutine(DamageEnemy(damages, GameManager.instance.PlayerItemList.Count));
                        if (Player.instance.Shadow)
                        {
                            StartCoroutine(DamageEnemy(1, GameManager.instance.PlayerItemList.Count, true, true, Player.instance.ShadowDelay));
                        }
                        EffectManager.instance.PlayerEffectOn("MultiSlash", Player.instance.transform.position, GameManager.instance.PlayerItemList.Count);
                    }
                    else if (damagerule == 109)
                    {
                        StartCoroutine(DamageEnemy(hitdamage, 2)); if (Player.instance.Shadow) { StartCoroutine(DamageEnemy(1, 2, true, true, Player.instance.ShadowDelay)); }
                        EffectManager.instance.PlayerEffectOn("MultiSlash", Player.instance.transform.position, 2);
                    }

                    else if (damagerule > 300 && damagerule < 400)
                    {
                        StartCoroutine(DamageEnemy("전체공격"));
                        if (Player.instance.Shadow) //그림자 버프가 켜져 있다면
                        {
                            StartCoroutine(DamageEnemy("전체공격", 1, true, true, Player.instance.ShadowDelay));
                        }
                    }
                    else
                    {
                        StartCoroutine(DamageEnemy("단일공격"));

                        if (Player.instance.Shadow) //그림자 버프가 켜져 있다면
                        {
                            StartCoroutine(DamageEnemy("단일공격", 1, true, true, Player.instance.ShadowDelay));
                        }
                    }
                    Player.instance.PlayerStateCheck("메아리");
                    Player.instance.PlayerStateCheck("피해량 배율");
                }
                else if (type == "Deffend")
                {
                    Player.instance.AddCardCount(serialnumber, Player.CardType.Defend);
                    hitdamage = (int)(hitdamage + 0.5f); //반올림
                    Player.instance.PlayerGetArmor(hitdamage);
                }
                else if (type == "Heal")
                {
                    Player.instance.AddCardCount(serialnumber, Player.CardType.Heal);
                    hitdamage = (int)(hitdamage + 0.5f); //반올림
                    Player.instance.HealHP(hitdamage);
                }
                else
                {
                    Player.instance.AddCardCount(serialnumber, Player.CardType.Special);

                }

                companel.UpdateUseComCardcount(1); //조합카드 사용횟수 1증가+
                companel.previousCardSerialNum = serialnumber; // 이전 사용한 시리얼 넘버 기억
            }
            else
            {
                StartCoroutine(Player.instance.Shake(0));
            }

            if (damagerule != 101 && damagerule != 603 && damagerule != 960)
            {
                Destroy(previouscard);
            }
            this.gameObject.transform.SetParent(gamepanel.transform.parent);
            this.gameObject.transform.localPosition = new Vector2(-1500, 100);
            this.gameObject.transform.DOShakePosition(2, 10).SetDelay(2f).OnComplete(() => Destroy(this.gameObject));
            //this.gameObject.

            //Destroy(this.gameObject);
            //소멸카드여부
            if (extinction == false)
            {
                //panel.usemyComcard.Add(panel.myComcard[mydecknumber]);
            }
            else
            {
                //comSerial에서 이카드와 넘버가 맞는 카드의 시리얼넘버를 0으로
                for (int i = 0; i < companel.comSerialnum.Count; i++)
                {
                    if (companel.comSerialnum[i] == serialnumber)
                    {
                        companel.comSerialnum[i] = 0;
                        break;
                    }
                }
            }

        }


    }

    public void TargetEnemy()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int target = Random.Range(0, Enemies.Length);

        //최종 선택된 데미지 입힐 대상
        combinationpanel.GetComponent<combinationpanel>().attacktime += 1; //공격횟수 체크


        //도발 몬스터 찾기
        float targetHp = 0;
        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i].GetComponent<EnemyBase>().isTaunt)
            {
                if (targetHp < Enemies[i].GetComponent<EnemyBase>().EnemyHp)
                {
                    targetHp = Enemies[i].GetComponent<EnemyBase>().EnemyHp;
                    target = i;
                    DebugX.Log("taunt target : " + target);
                }
            }
        }
        // 타겟이 지정되어있을 때 타겟하고 아니면 위에서 선별된것중 랜덤
        if (Player.instance.targetEnemy == null) LastTarget = Enemies[target];
        else LastTarget = Player.instance.targetEnemy;
    }

    //몬스터에게 데미지를 주는 함수
    public IEnumerator DamageEnemy(string targets, int ChoiceDamage = 0, bool NotChageTarget = false, bool NotAtkCount=false,float AtkDelay = 0)
    {
        if(AtkDelay != 0)   //딜레이가 있을 경우
        {
            yield return new WaitForSeconds(AtkDelay);
        }

        if (ChoiceDamage != 0)   //고정값이 있을 경우
        {
            hitdamage = ChoiceDamage;
        }

        if (!NotChageTarget)
        {
            TargetEnemy();
        }

        if (Player.instance.blood)
        {
            Player.instance.PlayerDamagedBlood(Player.instance.stateEffect.BloodDmg);
        }

        ////////////////////////////////////////////////데미지 계산/////////////////////////////////////////////////////////////////////////

        if (Player.instance.Item10 == true && !NotAtkCount) //아이템  3번째 공격 데미지1.5배 하는거
        {
            TopUI.instance.FindItemNum(10, true);
        }


        //추가 데미지
        float finaldamage = TotalDmgCal(hitdamage);
        //데미지 배율
        finaldamage *= combinationpanel.GetComponent<combinationpanel>().damagemulltiple;

        if(Player.instance.Item52)
        {
            if (Random.value >= 0.9f)
            {
                finaldamage *= 2;
            }
        }

        if (Player.instance.paralysis) //마비 상태이면 실행
        {
            finaldamage /= 2;
        }
        //반올림
        finaldamage = (int)(finaldamage + 0.5f);

        //이전 공격 데미지
        combinationpanel.GetComponent<combinationpanel>().previousDamage = (int)finaldamage;

        finaldamage -= Player.instance.fear; //공포만큼 데미지 감소
                                             /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        DebugX.Log("공격 실행" + finaldamage);

        if (LastTarget == null)     //타켓이 없을 경우 공격을 멈춤
        {
            DebugX.Log("라스트타겟 없음");
        }
        else
        {
            if (targets == "단일공격")
            {
                DebugX.Log("단일공격");

                DebugX.Log(isShieldCrack.ToString());
                if (isShieldCrack)
                {
                    DebugX.Log("쉴드깎");
                    LastTarget.GetComponent<EnemyBase>().EnemyArmor = 0;
                    LastTarget.GetComponent<EnemyBase>().EnemyHpBarShow();
                    yield break;
                }
                LastTarget.GetComponent<EnemyBase>().EnemyDamaged(finaldamage);
                //이펙트
                if (finaldamage <= 30)
                {
                    DebugX.Log("약공격 실행" + finaldamage);
                    EffectManager.instance.PlayerEffectOn("Slash", Player.instance.transform.position);
                }
                else if (finaldamage >= 31)
                {
                    DebugX.Log("강공격 실행" + finaldamage);
                    EffectManager.instance.PlayerEffectOn("SlashStrong", Player.instance.transform.position);
                }

            }

            else if (targets == "전체공격")
            {
                DebugX.Log("전체공격");

                GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");

                for (int i = 0; i < Enemies.Length; i++)
                {
                    Enemies[i].GetComponent<EnemyBase>().EnemyDamaged(finaldamage);
                }

                if (finaldamage <= 30)
                {
                    EffectManager.instance.PlayerEffectOn("Slash", Player.instance.transform.position);
                }
                else if (finaldamage > 30)
                {
                    EffectManager.instance.PlayerEffectOn("SlashStrong", Player.instance.transform.position);
                }
            }

            //타겟된 상대가 죽어버리면
            if (LastTarget != null && LastTarget.GetComponent<EnemyBase>().EnemyHp <= 0 && Player.instance.gameObject.GetComponent<SelectTarget>().enabled == true)
            {
                DebugX.Log("죽었음?)");
                LastTarget.tag = "Untagged";
                Player.instance.GetComponent<SelectTarget>().firstsearchtarget();
            }
        }
        // 데미지 초기화

        combinationpanel.GetComponent<combinationpanel>().plusdamage = 0;
        combinationpanel.GetComponent<combinationpanel>().damagemulltiple = 1.0f;
    }
    public IEnumerator DamageEnemy(float damage, int Attackcount, bool NotChageTarget = false, bool NotAtkCount = false, float AtkDelay = 0)       //연타용 데미지 함수
    {
        if(AtkDelay !=0)
        {
            yield return new WaitForSeconds(AtkDelay);
        }

        if(Attackcount<1)
        {
            Attackcount = 1;
        }

        if (Player.instance.blood)
        {
            Player.instance.PlayerDamagedBlood(Player.instance.stateEffect.BloodDmg);
        }

        Enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (card84) //만약 84번 카드를 사용했다면
        {
            //x축 기준으로 적 정렬
            Array.Sort(Enemies, (e1, e2) => e1.transform.position.x.CompareTo(e2.transform.position.x));
        }

        int target = Random.Range(0, Enemies.Length);

        DebugX.Log("target : " + target);
        //최종 선택된 데미지 입힐 대상
        //GameObject LastTarget;

        if (!NotChageTarget)    //타겟고정이 아닐때
        {
            //도발 몬스터 찾기
            float targetHp = 0;
            for (int i = 0; i < Enemies.Length; i++)
            {
                if (Enemies[i].GetComponent<EnemyBase>().isTaunt)
                {
                    if (targetHp < Enemies[i].GetComponent<EnemyBase>().EnemyHp)
                    {
                        targetHp = Enemies[i].GetComponent<EnemyBase>().EnemyHp;
                        target = i;
                        DebugX.Log("taunt target : " + target);
                    }
                }
            }
            // 타겟이 지정되어있을 때 타겟하고 아니면 위에서 선별된것중 랜덤
            if (Player.instance.targetEnemy == null)
            {
                LastTarget = Enemies[target];
            }
            else
            {
                LastTarget = Player.instance.targetEnemy;

                if (card84) //만약 84번 카드를 사용했다면
                {
                    for (int i = 0; i < Enemies.Length; i++)
                    {
                        if (Enemies[i] == LastTarget)
                        {
                            target = i;
                            break;
                        }
                    }
                }
            }

            if (card84) //만약 84번 카드를 사용했다면
            {
                LastTarget_before = LastTarget;
                Card84_Function(target, Enemies);       //타켓 지정
            }
        }
        else    //타겟 고정인데
        {
            if(LastTarget_before !=null)        //고무공 변수가 있을 때
            {
                LastTarget = LastTarget_before; //고무공이 때린 타겟으로 변경
            }
        }

        Vector3 Position = LastTarget.GetComponent<EnemyBase>().enemyposion.position;
        for (int j = 0; j < Attackcount; j++)
        {


            combinationpanel.GetComponent<combinationpanel>().attacktime += 1; //공격횟수 체크



            ////////////////////////////////////////////////데미지 계산/////////////////////////////////////////////////////////////////////////

            if (Player.instance.Item10 == true && !NotAtkCount) //아이템  3번째 공격 데미지1.5배 하는거
            {
                TopUI.instance.FindItemNum(10, true);
            }


            //추가 데미지
            float finaldamage = TotalDmgCal(damage);
            //데미지 배율
            finaldamage *= combinationpanel.GetComponent<combinationpanel>().damagemulltiple;
            
            if (Player.instance.paralysis)  //마비 상태이면 실행
            {
                finaldamage /= 2;
            }
            
            //반올림
            finaldamage = (int)(finaldamage + 0.5f);

            finaldamage -= Player.instance.fear; //공포만큼 데미지 감소
            ////////////////////////////////////////////////데미지 계산/////////////////////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////////////연속때리다가 죽어버린경우에 처리
            if (LastTarget == null) DebugX.Log("라스트타겟 없음");
            else
            {
                LastTarget.GetComponent<EnemyBase>().EnemyDamaged(finaldamage);
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            DebugX.Log(damage + " " + Player.instance.power + " " + finaldamage);
            //이전 공격 데미지
            combinationpanel.GetComponent<combinationpanel>().previousDamage = (int)finaldamage;
            DebugX.Log(combinationpanel.GetComponent<combinationpanel>().previousDamage.ToString());

            //타겟된 상대가 죽어버리면
            if (LastTarget != null && LastTarget.GetComponent<EnemyBase>().EnemyHp <= 0 && Player.instance.gameObject.GetComponent<SelectTarget>().enabled == true)
            {
                DebugX.Log("죽었음?)");
                LastTarget.tag = "Untagged";
                Player.instance.GetComponent<SelectTarget>().firstsearchtarget();
            }



            // 데미지 초기화
            combinationpanel.GetComponent<combinationpanel>().plusdamage = 0;
            combinationpanel.GetComponent<combinationpanel>().damagemulltiple = 1.0f;

            //84번 카드 효과
            if(card84)
            {
                if (card84_target)
                {
                    if (card84_target.tag == "Untagged")
                    {
                        break;
                    }


                    LastTarget = card84_target;
                    Position = LastTarget.transform.position;
                    damage = card84_dmg;
                }
                else    //공격할 대상이 없으면
                {
                    break;
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
        /*yield return new WaitForSeconds(0.2f);
        if (LastTarget != null) LastTarget.transform.DOMove(Position, 0f);*/

    }

    private float TotalDmgCal(float basicDmg)
    {
        float totalDamage = basicDmg + Player.instance.power + combinationpanel.GetComponent<combinationpanel>().plusdamage + Player.instance.item_act_power+ Player.instance.Card107;
        return (int)Mathf.Ceil(totalDamage);
    }

    public void Card84_Function(int target, GameObject[] Enemies)
    { 

        foreach (var e in Enemies)
        {
            DebugX.Log("적 이름" + e.name);
        }

        if (Enemies.Length == 3)
        {
            if (target == 0)   //처음 자리
            {
                card84_target = Enemies[1];
            }
            else if(target ==1) //두 번째 자리
            {
                card84_target = Enemies[2];
            }
        }
        else if (Enemies.Length == 2)
        {
            if (target == 0)   //처음자리
            {
                card84_target = Enemies[1];
            }
        }
    }
}
