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
    float hitdamage;  //���� ������


    float randomx, randomy; //����Ʈ ���� ��ġ

    bool isShieldCrack = false;

    public GameObject LastTarget; // Ÿ��
    GameObject LastTarget_before=null; // ���� Ÿ�� ���� ī�� ������ �����
    [SerializeField]GameObject[] Enemies;
    int random;

    //ī�� ����
    bool card84 = false;
    GameObject card84_target = null;
    int card84_dmg=0;

    bool isUsed = false;

    int insint; //��Ʈ�� �ӽ� ���� ĳ�̿�
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

    public void previousCard() //x��ư�� ���� �� ī�� ���� ���Կ� ���� ī�尡 �����ִٸ� ī���ȯ�ϴ� �Լ�
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
                    //����ִ� ���� ī��� ����������
                    mycardpanel.GetComponent<mycard>().myhandcard.Add(tr.transform.GetChild(1).gameObject);
                    tr.transform.GetChild(1).SetParent(mycardpanel.transform);

                    DebugX.Log(a.ToString());
                    a.transform.localScale = new Vector2(1, 1);
                    //����
                    mycardpanel.GetComponent<mycard>().align();
                }
            }
        }

        // ����Ϸ��� ����ī�� ��ȯ
        //previouscard = combinationpanel.GetComponent<combinationpanel>().thiscombinationcard;
        combinationpanel.GetComponent<combinationpanel>().combinationcarddeck.Add(previouscard);
        previouscard.SetActive(true);
        previouscard.transform.SetParent(combinationpanel.transform);

        //����
        combinationpanel.GetComponent<combinationpanel>().align();

        if (cardslot[0].GetComponent<CardDrop>().rule==dropcardrule.rule.accumulate)    //���� �� ���Կ� ī�带 �� �ִ� �Ŷ��
        {
            //����� ī����� �ٽ� �����ش�.
            foreach (var card in cardslot[0].GetComponent<CardDrop>().UseCardList)
            {
                mycard.instance.othercarddraw(card);
            }
            mycard.instance.align();
        }

        Destroy(transform.gameObject);

        DebugX.Log(previouscard.name);
    }

    public void ReturnCard() //ī��ȿ���� �ߵ��ϰ� �ٽ� ���п� ������ī��
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

        // ����Ϸ��� ����ī�� ��ȯ
        //previouscard = combinationpanel.GetComponent<combinationpanel>().thiscombinationcard;
        combinationpanel.GetComponent<combinationpanel>().combinationcarddeck.Add(previouscard);
        previouscard.SetActive(true);
        previouscard.transform.SetParent(combinationpanel.transform);

        //����
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

        #region ī�� ȿ��
        if (check == cardslotcount)
        {
            if (cardslot[0].GetComponent<CardDrop>().rule == dropcardrule.rule.equal)   //���� ���ڳִ� ������ ��� - ��Ŀó��
            {
                DebugX.Log("���� ���� ��Ģ ��Ŀó��");

                jokercount = 0;
                equlnum = 0;

                for (int Cd=0; Cd<cardnum.Length; Cd++)
                {
                    if (cardnum[Cd]==14)    //��Ŀ �� ���
                    {
                        jokercount++;
                    }
                    else
                    {
                        equlnum = cardnum[Cd];  //�� ���� ����
                    }
                }

                DebugX.Log("���� ���� ��Ģ ��Ŀ����"+ jokercount);
                if (cardnum.Length == 3)        //������ 3���� ���
                {
                    if (jokercount == 3)     //���� ��Ŀ�� ���
                    {
                        cardnum[0] = 10;
                        cardnum[1] = 10;
                        cardnum[2] = 10;
                    }
                    else  //���� ��Ŀ�� �ƴ� ���
                    {
                        cardnum[0] = equlnum;
                        cardnum[1] = equlnum;
                        cardnum[2] = equlnum;
                    }
                }
                else if(cardnum.Length==2)      //2���� ���
                {
                    if (jokercount == 2)     //���� ��Ŀ�� ���
                    {
                        cardnum[0] = 10;
                        cardnum[1] = 10;
                    }
                    else  //���� ��Ŀ�� �ƴ� ���
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
                //�� ���ʺ��� 0 , 1 , 2 ������� ��� 
                //���� 1ĭ�� ��� cardnum[0]�� ���
                //���� 2ĭ�� ��� cardnum[0], cardnum[1] ���
                //���� 3ĭ�� ��� cardnum[0], cardnum[1], cardnum[2] ���

                #region �Ϲ� 0~
                case 0:
                    hitdamage += damages;
                    break;
                case 1:
                    hitdamage += cardnum[0];             //ù��° ���Կ� ī�� ���� ��

                    break;
                case 2:
                    foreach (var a in cardnum)
                    {
                        hitdamage += a;                 //�� ��� ī�� �ջ��� ������
                    }


                    break;
                case 3:
                    hitdamage += cardnum[0] * 2;          //ù��° ���Կ� ī�� ���� �� 2��
                    break;

                case 4:
                    hitdamage += cardnum[0] * cardnum[0];  //ù��° ���Կ� ī�� ���� �� ����

                    break;
                case 5:
                    hitdamage += cardnum[0] + damages;   //ī�� + �߰�������
                    break;
                case 6:
                    hitdamage += cardnum[0] * cardnum[1]; // �� ���� ��
                    break;

                case 8:
                    hitdamage += cardnum[0] / 2.0f;        //���� ���� ����
                    break;
                case 9:
                    hitdamage += cardnum[0] * 1.5f;     //���� ���� 1.5��
                    break;
                case 10:
                    hitdamage += cardnum[0] * cardnum[1] / 2.0f; // �� ���� �� / 2
                    break;
                case 11:
                    hitdamage += Mathf.Ceil((cardnum[0] * cardnum[1] * cardnum[2])/4f);
                    break;

                case 12:
                    hitdamage += cardnum[0] + cardnum[1];  // �μ��� ��
                    break;

                case 13:    //n���� ���� ���ڸ� ����ŭ ���� �����Ѵ�.
                    hitdamage = damages - cardnum[0];
                    break;

                case 14:    //���ڸ� �־� 10���� ���ڸ� �� ���� n�踸ŭ �� ��ο��� ���ظ� ������.
                    hitdamage = (10 - cardnum[0]) * damages;
                    break;

                case 20:
                    hitdamage += companel.previousDamage / 2.0f; //������ ���� ������
                    DebugX.Log("hitdamage : " + hitdamage);
                    break;
                case 21:
                    hitdamage += companel.previousDamage; //������  ������
                    DebugX.Log("hitdamage : " + hitdamage);
                    break;

                case 22:    //���ڸ� �־� n���� ���ڸ� ������ŭ
                    hitdamage = damages / cardnum[0];
                    break;
                #endregion

                #region Ư������ 100~
                case 101:
                    hitdamage += damages;
                    ReturnCard(); //�ٽ� ���п� ������
                    break;
                case 102:
                    if (Player.instance.PlayerGold > 0) hitdamage += Player.instance.PlayerGold / 5.0f;
                    else hitdamage = 0;
                    break;
                case 103:
                    if (Player.instance.PlayerGold > 0) hitdamage += Player.instance.PlayerGold / 10.0f;
                    else hitdamage = 0;
                    break;

                case 104: //ü��5 �Ҹ� �� 100������
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
                    isShieldCrack = true; //���� �μ�����
                    break;

                case 106: //���������� + ���� Ȧ�� ī�� ��
                    hitdamage += damages;
                    int randomOdd = Random.Range(1, 6) * 2 - 1; //Ȧ�� ����
                    cardpanel.othercarddraw(randomOdd);
                    break;

                case 107:
                    hitdamage += damages;
                    int randomEven = Random.Range(1, 6) * 2; //¦�� ����
                    cardpanel.othercarddraw(randomEven);
                    break;
                case 108:   //���ڸ� �־� ������ �������� ����ŭ n���� ȹ���Ѵ�. (������, �� ����)
                    DebugX.Log("������ ������ �� �нú�: " + GameManager.instance.PlayerItemList.Count);
                    hitdamage += GameManager.instance.PlayerItemList.Count;
                    break;
                case 109:   //���� ���ڸ�ŭ ���� �����Ѵ�. �ڿ� ���� ���� ��� ���� ������ �ݸ�ŭ ���� ���� �����Ѵ�.
                    hitdamage += cardnum[0];
                    card84 = true;
                    card84_dmg = cardnum[0] / 2;
                    break;
                case 110:   //���� 1�� �־� 10%Ȯ���� n��ŭ ������ ���ظ� ������.
                    random = Random.Range(0, 10);
                    DebugX.Log("0�� ������ ī�� ȿ��" + random);
                    if (random == 0)
                    {
                        hitdamage += damages;
                    }
                    break;
                case 111:   //���ڸ� �־� ������ �������� ���� n�踸ŭ ������ ���ظ� ������.
                    hitdamage += GameManager.instance.PlayerItemList.Count * damages;
                    break;
                case 112:   //���� ���ڸ�ŭ ������ ���ظ� ������. ����� ���� ī��� ��ȯ�ȴ�.
                    hitdamage += cardnum[0];

                    if (cardslot[0].GetComponent<CardDrop>().joker != 0)
                    {
                        cardnum[0] = 14;
                    }

                    mycard.instance.othercarddraw(cardnum[0]);
                    mycard.instance.align();
                    break;
                case 113:   //�� ���ڸ� �־� ���� ū ���� ���� ���� ����ŭ ������ ���ظ� ������.
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
                case 114:   //����

                    break;
                case 115:   //������ ����� 1/n��ŭ ��ȣ���� ȹ���Ѵ�. ���� ��尡 n���� ���� �� 1�� ��ȣ���� ȹ���Ѵ�.
                    if (Player.instance.PlayerGold < damages)
                    {
                        hitdamage += 1;
                    }
                    else
                    {
                        hitdamage += (int)Player.instance.PlayerGold / damages;
                    }
                    break;
                case 116:   //���ڸ� �־� �տ� ������ ���� ī�� ����ŭ ��ȣ���� ȹ���Ѵ�.
                    hitdamage += combinationpanel.GetComponent<combinationpanel>().MyhandCount();
                    break;
                case 117:   //���ڸ� �־� �տ� ������ ���� ī�� ���� n�踸ŭ ��ȣ���� ȹ���Ѵ�.
                    hitdamage += combinationpanel.GetComponent<combinationpanel>().MyhandCount() * damages;
                    break;
                case 118:   //���ڸ� �־� �տ� ������ ����ī�� ����ŭ ���ط� ���������� 1�� ȹ���Ѵ�.
                    Player.instance.Card107 = combinationpanel.GetComponent<combinationpanel>().Myhand_AttackCardCount();
                    DebugX.Log("������" + Player.instance.Card107);
                    Player.instance.PlayerStateAdd("���ط� ����", Player.instance.Card107);
                    break;
                case 119:   //7�̻��� �� ���ڸ� �־� ���� ��ȣ���� ���Ҵ´�.
                    break;
                #endregion

                #region ��ü���� 300~
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
                    //150�� ���������� ���� ��ü����
                    GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    hitdamage = 150 / Enemies.Length;
                    break;

                case 305:   //���� ����ŭ �� ��ο��� ���ظ� ������.
                    hitdamage = cardnum[0];
                    break;

                case 306:   //���ڸ� �־� 10���� ���ڸ� �� ���� n�踸ŭ �� ��ο��� ���ظ� ������.
                    hitdamage = (10 - cardnum[0]) * damages;
                    break;

                case 307:   //�� ���ڸ� �־� �� ���� ����ŭ �� ��ü�� �����Ѵ�.
                    hitdamage = cardnum[0] * cardnum[1];
                    break;

                case 308:   //n��ŭ �� ��ο��� ���ظ� ������.
                    hitdamage = damages;
                    break;

                case 603:
                    hitdamage += cardnum[0] * 2;
                    ReturnCard();
                    break;
                #endregion

                #region �����̻� 800~
                case 800: //�����̻� �Ŵ� �κ�
                          //cardnum[0]�� ���� ī���� ������
                    TargetEnemy();
                    LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("��", damages);
                    if (!LastTarget.GetComponent<EnemyBase>().poison)      //���� ���� �ƴ� ��
                    {
                        FindObjectOfType<StateEffect_e>().StateEffectOn(0, true, LastTarget);
                    }
                    //LastTarget.GetComponent<EnemyBase>().Hit();
                    EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Poison, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                    //FindObjectOfType<StateEffect_e>().StateEffectOn(0, true, LastTarget);
                    break;
                case 801: //�����̻� �Ŵ� �κ�
                          //cardnum[0]�� ���� ī���� ������
                    TargetEnemy();
                    LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("ȭ��", damages);
                    FindObjectOfType<StateEffect_e>().StateEffectOn(1, true, LastTarget);
                    break;
                case 802: //�����̻� �Ŵ� �κ�
                          //cardnum[0]�� ���� ī���� ������
                    TargetEnemy();
                    LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("��ó", damages);
                    FindObjectOfType<StateEffect_e>().StateEffectOn(2, true, LastTarget);
                    EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Bleed, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                    break;
                case 803: //�����̻� �Ŵ� �κ�
                          //cardnum[0]�� ���� ī���� ������
                    TargetEnemy();
                    LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("����", damages);
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

                        Player.instance.PlayerStateAdd("�ǰݹ���", Player.instance.BlockDamage);
                    }

                    break;

                case 805:
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject target in enemies)
                    {
                        target.GetComponent<EnemyBase>().EnemyStateAdd("����", 1);
                        FindObjectOfType<StateEffect_e>().StateEffectOn(3, true, target);
                        EffectManager.instance.EnemyCommoneEffectOn(target.name, CommonEffectName.Paralysis, target.GetComponent<EnemyBase>().enemyposion.position);
                    }
                    break;
                case 806: //������ ����� 1/n��ŭ ������ �����̻� �ο��Ѵ�. ���� ��尡 n���� ���� �� 50%�� Ȯ���� 1�� �ο��Ѵ�.

                    TargetEnemy();
                    if (Player.instance.PlayerGold < damages)
                    {
                        random = Random.Range(0, 2);
                        DebugX.Log("Ȯ�� 0 ������ ī��ȿ�� �ߵ� " + random);
                        if (random == 0)
                        {
                            LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("����", 1);
                            FindObjectOfType<StateEffect_e>().StateEffectOn(3, true, LastTarget);
                            EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Paralysis, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                        }
                    }
                    else
                    {
                        LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("����", (int)Player.instance.PlayerGold / damages);
                        EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Paralysis, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                        FindObjectOfType<StateEffect_e>().StateEffectOn(3, true, LastTarget);
                    }
                    break;
                case 807: //������ ����� 1/n��ŭ ������ �����̻� �ο��Ѵ�. ���� ��尡 n���� ���� �� 50%�� Ȯ���� 1�� �ο��Ѵ�.

                    TargetEnemy();
                    if (Player.instance.PlayerGold < damages)
                    {
                        random = Random.Range(0, 2);
                        DebugX.Log("Ȯ�� 0 ������ ī��ȿ�� �ߵ� " + random);
                        if (random == 0)
                        {
                            LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("ȭ��", 1);
                            FindObjectOfType<StateEffect_e>().StateEffectOn(1, true, LastTarget);
                        }
                    }
                    else
                    {
                        LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("ȭ��", (int)Player.instance.PlayerGold / damages);
                        FindObjectOfType<StateEffect_e>().StateEffectOn(1, true, LastTarget);
                    }
                    break;
                case 808: //������ ����� 1/n��ŭ ������ �����̻� �ο��Ѵ�. ���� ��尡 n���� ���� �� 50%�� Ȯ���� 1�� �ο��Ѵ�.

                    TargetEnemy();
                    if (Player.instance.PlayerGold < damages)
                    {
                        random = Random.Range(0, 2);
                        DebugX.Log("Ȯ�� 0 ������ ī��ȿ�� �ߵ� " + random);
                        if (random == 0)
                        {
                            LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("��", 1);
                            if (!LastTarget.GetComponent<EnemyBase>().poison)      //���� ���� �ƴ� ��
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
                        LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("��", (int)Player.instance.PlayerGold / damages);
                        if (!LastTarget.GetComponent<EnemyBase>().poison)      //���� ���� �ƴ� ��
                        {
                            FindObjectOfType<StateEffect_e>().StateEffectOn(0, true, LastTarget);
                        }
                        //LastTarget.GetComponent<EnemyBase>().Hit();
                        EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Poison, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                        //FindObjectOfType<StateEffect_e>().StateEffectOn(0, true, LastTarget);
                    }
                    break;
                case 809: //������ ����� 1/n��ŭ ������ �����̻� �ο��Ѵ�. ���� ��尡 n���� ���� �� 50%�� Ȯ���� 1�� �ο��Ѵ�.

                    TargetEnemy();
                    if (Player.instance.PlayerGold < damages)
                    {
                        random = Random.Range(0, 2);
                        DebugX.Log("Ȯ�� 0 ������ ī��ȿ�� �ߵ� " + random);
                        if (random == 0)
                        {
                            LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("��ó", 1);
                            FindObjectOfType<StateEffect_e>().StateEffectOn(2, true, LastTarget);
                            EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Bleed, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                        }
                    }
                    else
                    {
                        LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("��ó", (int)Player.instance.PlayerGold / damages);
                        EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Bleed, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                        FindObjectOfType<StateEffect_e>().StateEffectOn(2, true, LastTarget);
                    }
                    break;

                case 810:   //���ڸ� �־� n���� ���� ���ڸ� ����ŭ ������ ȭ���� �ο��Ѵ�.
                    TargetEnemy();                    
                    LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("ȭ��", (damages-cardnum[0]));
                    LastTarget.GetComponent<EnemyBase>().Hit();
                    FindObjectOfType<StateEffect_e>().StateEffectOn(1, true, LastTarget);
                    EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Burn, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                    break;

                case 811:   //���ڸ� �־� n���� ���� ���ڸ� ����ŭ ������ ���� �ο��Ѵ�.
                    TargetEnemy();
                    LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("��", (damages - cardnum[0]));
                    if (!LastTarget.GetComponent<EnemyBase>().poison)      //���� ���� �ƴ� ��
                    {
                        FindObjectOfType<StateEffect_e>().StateEffectOn(0, true, LastTarget);
                    }
                    //LastTarget.GetComponent<EnemyBase>().Hit();
                    EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Poison, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                    break;

                case 812:   //���ڸ� �־� n���� ���� ���ڸ� ����ŭ ������ ������ �ο��Ѵ�.
                    TargetEnemy();
                    LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("����", (damages - cardnum[0]));
                    LastTarget.GetComponent<EnemyBase>().Hit();
                    FindObjectOfType<StateEffect_e>().StateEffectOn(3, true, LastTarget);
                    EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Paralysis, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                    break;

                case 813:   //���ڸ� �־� n���� ���� ���ڸ� ����ŭ ������ ������ �ο��Ѵ�.
                    TargetEnemy();
                    LastTarget.GetComponent<EnemyBase>().EnemyStateAdd("��ó", (damages - cardnum[0]));
                    LastTarget.GetComponent<EnemyBase>().Hit();
                    FindObjectOfType<StateEffect_e>().StateEffectOn(2, true, LastTarget);
                    EffectManager.instance.EnemyCommoneEffectOn(LastTarget.name, CommonEffectName.Bleed, LastTarget.GetComponent<EnemyBase>().enemyposion.position);
                    break;
                case 815:   //���ڸ� �־� ������ �� �������� 2��� �Ѵ�.                   
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject target in enemies)
                    {
                        if (target.GetComponent<EnemyBase>().poison)      //���� ���� ��
                        {
                            target.GetComponent<EnemyBase>().poison_dmg *= 2;   //2��
                            target.GetComponent<EnemyBase>().EnemyStateAdd("��", 0, target.GetComponent<EnemyBase>().poison_dmg);
                            EffectManager.instance.EnemyCommoneEffectOn(target.name, CommonEffectName.Poison, target.GetComponent<EnemyBase>().enemyposion.position);
                        }
                    }
                    break;
                case 816:   //���ڸ� �־� ������ ���� �ϼ��� 2��� �Ѵ�.
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject target in enemies)
                    {
                        if (target.GetComponent<EnemyBase>().paralysis)
                        {
                            target.GetComponent<EnemyBase>().EnemyStateAdd("����", damages, 0, true, false, true);
                            EffectManager.instance.EnemyCommoneEffectOn(target.name, CommonEffectName.Paralysis, target.GetComponent<EnemyBase>().enemyposion.position);                            
                        }
                    }
                    break;
                case 817:   //���ڸ� �־� ȭ���� ���ط��� 20���� ���� ��Ų��.
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject target in enemies)
                    {
                        if (target.GetComponent<EnemyBase>().burn)      //���� ȭ���� ��
                        {
                            Player.instance.PlusStateUp(State.Burn, 1);
                            target.GetComponent<EnemyBase>().burn_dmg = damages;
                            target.GetComponent<EnemyBase>().EnemyStateAdd("ȭ��", 0, damages);
                            EffectManager.instance.EnemyCommoneEffectOn(target.name, CommonEffectName.Burn, target.GetComponent<EnemyBase>().enemyposion.position);
                            GamePlayBtn.instance.TSF += target.GetComponent<EnemyBase>().RemovePlusBurnDmg;
                        }
                    }
                    break;
                case 818:   //���ڸ� �־� 1�� ���� ��ó�� Ȯ���� 100%�� �Ѵ�.
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject target in enemies)
                    {
                        if (target.GetComponent<EnemyBase>().blood)      //���� ��ó�� ��
                        {
                            Player.instance.PlusStateUp(State.Blood, 1);
                            Player.instance.PlusBleedPercent = damages;
                            target.GetComponent<EnemyBase>().EnemyStateAdd("��ó", 0, target.GetComponent<EnemyBase>().blood_dmg);
                            EffectManager.instance.EnemyCommoneEffectOn(target.name, CommonEffectName.Bleed, target.GetComponent<EnemyBase>().enemyposion.position);
                            GamePlayBtn.instance.TSF += target.GetComponent<EnemyBase>().RemovePlusBleedPercent;
                        }
                    }
                    break;
                case 819:   //���ڸ� �־� ��ó�� ���ط��� 50���� ������Ų��.
                    enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject target in enemies)
                    {
                        if (target.GetComponent<EnemyBase>().blood)      //���� ��ó�� ��
                        {
                            Player.instance.PlusStateUp(State.Blood, 1);
                            target.GetComponent<EnemyBase>().blood_dmg = damages;
                            target.GetComponent<EnemyBase>().EnemyStateAdd("��ó", 0, damages);
                            EffectManager.instance.EnemyCommoneEffectOn(target.name, CommonEffectName.Bleed, target.GetComponent<EnemyBase>().enemyposion.position);
                            GamePlayBtn.instance.TSF += target.GetComponent<EnemyBase>().RemovePlusBleedDmg;
                        }
                    }
                    break;
                #endregion

                #region Ư�� ���� 900~
                case 903:
                    //�����̻� ��ü ����

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
                    // �����̻� ���� 1�� ���� 
                    Player.instance.PlayerStateClean();

                    break;
                case 905: //�ǰ�1ȸ���
                    Player.instance.BlockDamage += 1;

                    EffectManager.instance.PlayerEffectOn("Invincible", Player.instance.transform.position);

                    Player.instance.PlayerStateAdd("�ǰݹ���", Player.instance.BlockDamage);
                    break;
                case 906: //�ݹ�Ȯ���� �� �Ǵ� �ǵ�
                    hitdamage += cardnum[0];
                    int r = Random.Range(1, 3);
                    if (r == 1) Player.instance.PlayerGetArmor(hitdamage);
                    else Player.instance.HealHP(hitdamage);
                    break;

                case 907:
                    //companel.isMonsterTime = false;

                    break;

                case 910://�߰������� ����
                    companel.plusdamage += cardnum[0];
                    Player.instance.PlayerStateAdd("�޾Ƹ�", companel.plusdamage);
                    break;
                case 911:
                    for (int i = 0; i < 5; i++)
                    {
                        GameObject buff = Player.instance.PlayerState_buff[0].transform.GetChild(i).gameObject;
                        if (buff.activeSelf)
                        {
                            StateBox box = buff.GetComponent<StateBox>();
                            if (box.stateName == "���ط� ����")
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
                        Player.instance.PlayerStateAdd("���ط� ����", companel.damagemulltiple);
                    }
                    else
                    {
                        companel.damagemulltiple += (cardnum[0] / 2.0f);
                        Player.instance.PlayerStateAdd("���ط� ����", companel.damagemulltiple);
                    }
                    break;

                case 912: //5������ �ݻ� ������ ����
                    Player.instance.boolreturnDamage = true;
                    Player.instance.returndamage_buff = Player.instance.stateDB.state_p[20].damage;
                    Player.instance.PlayerStateAdd("�ݻ�", 1);
                    break;

                case 913:   //1�ϵ��� Ÿ���� ����
                    Player.instance.gameObject.GetComponent<SelectTarget>().enabled = true;
                    GamePlayBtn.instance.TSF += Player.instance.gameObject.GetComponent<SelectTarget>().RemoveSelectTarget;
                    break;

                case 914:   //���ڸ� �־� 5~20 ������ ��带 ȹ���Ѵ�.                    
                    Player.instance.PlayerGetGold(Random.Range(5,20));
                    break;  

                case 915:   //�� ���ڸ� �־� 10~50 ������ ��带 ȹ���Ѵ�.
                    Player.instance.PlayerGetGold(Random.Range(10, 50));
                    break;

                case 916:   //�̹� ���� Ÿ���� ����
                    Player.instance.gameObject.GetComponent<SelectTarget>().enabled = true;
                    break;
                case 917:   //5������ ���ڸ� �־� 1�� ���� ���� ���ڸ�ŭ ���ط��� ������Ų��.
                    Player.instance.Card107 = cardnum[0];
                    DebugX.Log("������" + Player.instance.Card107);
                    Player.instance.PlayerStateAdd("���ط� ����", Player.instance.Card107);
                    break;
                case 918    :   //���ڸ� �־� 1�� ���� �׸��� ������ ȹ���Ѵ�.
                    Player.instance.Shadow = true;
                    Player.instance.PlayerStateAdd("�׸���", damages);
                    break;
                #endregion

                #region ��ο� 950~
                case 950:
                    //����
                    //split����
                    hitdamage += cardnum[0];
                    int split1, split2;

                    if (cardnum[0] == 1)
                    {
                        split1 = 1; //����
                        split2 = 1; //�ݿø�                                  
                    }
                    else
                    {
                        split1 = Mathf.FloorToInt(hitdamage / 2.0f); //����
                        split2 = Mathf.CeilToInt(hitdamage / 2.0f); //�ݿø�
                        DebugX.Log(split1 + " " + split2);
                    }

                    mycardpanel.GetComponent<mycard>().othercarddraw(split1);

                    mycardpanel.GetComponent<mycard>().othercarddraw(split2);
                    break;
                case 951: //ù��°ī�� 2���� ����
                    if (cardslot[0].GetComponent<CardDrop>().joker >= 10)   //��Ŀ �� ���
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
                case 952: //ù��°ī�� 3���� ����
                    if (cardslot[0].GetComponent<CardDrop>().joker >= 10)   //��Ŀ �� ���
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
                    { // 10 ������ 10�̶� �����ڸ����� �����ϳ��� ��ο�
                        mycardpanel.GetComponent<mycard>().othercarddraw(num / 10 * 10);
                        mycardpanel.GetComponent<mycard>().othercarddraw(num % 10);
                    }
                    else mycardpanel.GetComponent<mycard>().othercarddraw(num);  //10�����̸� �� ���� �ϳ��� ��ο�
                    break;
                case 954:
                    int num1 = cardnum[0] + cardnum[1];
                    if (num1 > 10)
                    { // 10 ������ 10�̶� �����ڸ����� �����ϳ��� ��ο�
                        mycardpanel.GetComponent<mycard>().othercarddraw(10);
                        mycardpanel.GetComponent<mycard>().othercarddraw(num1 % 10);
                    }
                    else mycardpanel.GetComponent<mycard>().othercarddraw(num1);  //10�����̸� �� ���� �ϳ��� ��ο�
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

                case 961: // ������ ����� ����ī�� ����
                    if (companel.previousCardSerialNum != 0)
                        companel.drawserialone(companel.previousCardSerialNum);
                    break;

                case 962: //���ڸ� -1�ϰ� HP�� 1 ����
                    mycardpanel.GetComponent<mycard>().othercarddraw(cardnum[0] - 1);
                    break;

                case 963: //���ڸ� +1�ϰ� HP�� 1����
                    mycardpanel.GetComponent<mycard>().othercarddraw(cardnum[0] + 1);
                    break;
                case 964:   //���ڸ� �־� 5��ŭ ������ ���ظ� ������. ���� ���� ī�忡�� �������� n���� �̴´�. ���� ���� ī�尡 ���� ��� ���� �ʴ´�.
                    hitdamage += 5;
                    mycard.instance.mydeck = mycard.instance.ShuffleList(mycard.instance.mydeck);
                    mycard.instance.normaldrawone(damages);
                    break;
                case 965:   //���ڸ� �־� 10��ŭ ������ ���ظ� ������. ���� ���� ī�忡�� �������� n���� �̴´�. ���� ���� ī�尡 ���� ��� ���� �ʴ´�.
                    hitdamage += 10;
                    combinationpanel.GetComponent<combinationpanel>().combinationcarddeck = combinationpanel.GetComponent<combinationpanel>().ShuffleList(combinationpanel.GetComponent<combinationpanel>().combinationcarddeck);
                    combinationpanel.GetComponent<combinationpanel>().notusecarddraw(damages);
                    break;
                case 966:   //���ڸ� �־� ���� ���� ī�忡�� ������ n�� �� 1���� �����Ͽ� �̴´�. ���� ���� ī�尡 ���� ��� ���� �ʴ´�.
                    GameObject.Find("CardCanvas").GetComponent<GamePlayBtn>().discoverpanel.GetComponent<discoverPanel>().discover_num(damages);
                    break;
                case 967:   //���ڸ� �־� ���� ���ڸ�ŭ ���ڸ� �ٽ� �̴´�. ���� ���� ī�尡 ���� ��� ���� �ʴ´�.
                    num = mycard.instance.MyhandCount();
                    DebugX.Log("�ڵ� ����ī��" + num);
                    mycard.instance.MyhandReSet();
                    mycard.instance.ShuffleList(mycard.instance.mydeck);
                    mycard.instance.normaldrawone(num);
                    mycard.instance.align();
                    break;
                case 968:   //���ڸ� �־� ���� ���� ī�忡�� �������� n���� �̴´�. ���� ���� ī�尡 ���� ��� ���� �ʴ´�.
                    combinationpanel.GetComponent<combinationpanel>().combinationcarddeck = combinationpanel.GetComponent<combinationpanel>().ShuffleList(combinationpanel.GetComponent<combinationpanel>().combinationcarddeck);
                    combinationpanel.GetComponent<combinationpanel>().notusecarddraw(damages);
                    break;
                case 969:   //���ڸ� �־� ���� �Ͽ� ���� ���ڸ� �߰��� ȹ���Ѵ�.
                    GamePlayBtn.instance.DrawNumber.Add(cardnum[0]);
                    GamePlayBtn.instance.TSF += GamePlayBtn.instance.CardDraw_number;
                    break;
                case 970:   //����ī�尡 n���� �ɶ����� �̴´�. ���� ���� ī�尡 ���� ��� ���� �ʴ´�.
                    int handnum=combinationpanel.GetComponent<combinationpanel>().MyhandCount()-1;  //����� ī�� ī���� ����
                    if(handnum<5)
                    {
                        combinationpanel.GetComponent<combinationpanel>().combinationcarddeck = combinationpanel.GetComponent<combinationpanel>().ShuffleList(combinationpanel.GetComponent<combinationpanel>().combinationcarddeck);
                        combinationpanel.GetComponent<combinationpanel>().notusecarddraw(5- handnum);
                    }
                    break;
                case 971:   //���ڸ� �־� 1��ŭ ������ ���ظ� ������. ���� ���� ī�忡�� �������� n���� �̴´�. ���� ���� ī�尡 ���� ��� ���� �ʴ´�.
                    hitdamage += 1;
                    mycard.instance.mydeck = mycard.instance.ShuffleList(mycard.instance.mydeck);
                    mycard.instance.normaldrawone(damages);
                    break;
                    #endregion
            }
            #endregion
            companel.UpdateUseComCardcount_Front(type);     //�ܼ� ī�� ������ �� / ī�� Ÿ�� ȿ�� �������� / ���� ���� �� ���� �Լ�

            if (isSuccess)
            {
                if (type == "Attack")
                {
                    Player.instance.AddCardCount(serialnumber, Player.CardType.Attack);

                    //������ ���� ������ ���
                    if (damagerule == 119)  //�� ����
                    {
                        hitdamage = 0;
                        TargetEnemy();
                        Player.instance.PlayerGetArmor(LastTarget.GetComponent<EnemyBase>().EnemyArmor);
                        LastTarget.GetComponent<EnemyBase>().EnemyArmor = 0;
                        LastTarget.GetComponent<EnemyBase>().EnemyHpBarShow();
                    }
                    // ��Ÿ���ݺκ�       �׸��� ���� ���� ó�� ���ֱ�
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
                        StartCoroutine(DamageEnemy("��ü����"));
                        if (Player.instance.Shadow) //�׸��� ������ ���� �ִٸ�
                        {
                            StartCoroutine(DamageEnemy("��ü����", 1, true, true, Player.instance.ShadowDelay));
                        }
                    }
                    else
                    {
                        StartCoroutine(DamageEnemy("���ϰ���"));

                        if (Player.instance.Shadow) //�׸��� ������ ���� �ִٸ�
                        {
                            StartCoroutine(DamageEnemy("���ϰ���", 1, true, true, Player.instance.ShadowDelay));
                        }
                    }
                    Player.instance.PlayerStateCheck("�޾Ƹ�");
                    Player.instance.PlayerStateCheck("���ط� ����");
                }
                else if (type == "Deffend")
                {
                    Player.instance.AddCardCount(serialnumber, Player.CardType.Defend);
                    hitdamage = (int)(hitdamage + 0.5f); //�ݿø�
                    Player.instance.PlayerGetArmor(hitdamage);
                }
                else if (type == "Heal")
                {
                    Player.instance.AddCardCount(serialnumber, Player.CardType.Heal);
                    hitdamage = (int)(hitdamage + 0.5f); //�ݿø�
                    Player.instance.HealHP(hitdamage);
                }
                else
                {
                    Player.instance.AddCardCount(serialnumber, Player.CardType.Special);

                }

                companel.UpdateUseComCardcount(1); //����ī�� ���Ƚ�� 1����+
                companel.previousCardSerialNum = serialnumber; // ���� ����� �ø��� �ѹ� ���
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
            //�Ҹ�ī�忩��
            if (extinction == false)
            {
                //panel.usemyComcard.Add(panel.myComcard[mydecknumber]);
            }
            else
            {
                //comSerial���� ��ī��� �ѹ��� �´� ī���� �ø���ѹ��� 0����
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

        //���� ���õ� ������ ���� ���
        combinationpanel.GetComponent<combinationpanel>().attacktime += 1; //����Ƚ�� üũ


        //���� ���� ã��
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
        // Ÿ���� �����Ǿ����� �� Ÿ���ϰ� �ƴϸ� ������ �����Ȱ��� ����
        if (Player.instance.targetEnemy == null) LastTarget = Enemies[target];
        else LastTarget = Player.instance.targetEnemy;
    }

    //���Ϳ��� �������� �ִ� �Լ�
    public IEnumerator DamageEnemy(string targets, int ChoiceDamage = 0, bool NotChageTarget = false, bool NotAtkCount=false,float AtkDelay = 0)
    {
        if(AtkDelay != 0)   //�����̰� ���� ���
        {
            yield return new WaitForSeconds(AtkDelay);
        }

        if (ChoiceDamage != 0)   //�������� ���� ���
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

        ////////////////////////////////////////////////������ ���/////////////////////////////////////////////////////////////////////////

        if (Player.instance.Item10 == true && !NotAtkCount) //������  3��° ���� ������1.5�� �ϴ°�
        {
            TopUI.instance.FindItemNum(10, true);
        }


        //�߰� ������
        float finaldamage = TotalDmgCal(hitdamage);
        //������ ����
        finaldamage *= combinationpanel.GetComponent<combinationpanel>().damagemulltiple;

        if(Player.instance.Item52)
        {
            if (Random.value >= 0.9f)
            {
                finaldamage *= 2;
            }
        }

        if (Player.instance.paralysis) //���� �����̸� ����
        {
            finaldamage /= 2;
        }
        //�ݿø�
        finaldamage = (int)(finaldamage + 0.5f);

        //���� ���� ������
        combinationpanel.GetComponent<combinationpanel>().previousDamage = (int)finaldamage;

        finaldamage -= Player.instance.fear; //������ŭ ������ ����
                                             /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        DebugX.Log("���� ����" + finaldamage);

        if (LastTarget == null)     //Ÿ���� ���� ��� ������ ����
        {
            DebugX.Log("��ƮŸ�� ����");
        }
        else
        {
            if (targets == "���ϰ���")
            {
                DebugX.Log("���ϰ���");

                DebugX.Log(isShieldCrack.ToString());
                if (isShieldCrack)
                {
                    DebugX.Log("�����");
                    LastTarget.GetComponent<EnemyBase>().EnemyArmor = 0;
                    LastTarget.GetComponent<EnemyBase>().EnemyHpBarShow();
                    yield break;
                }
                LastTarget.GetComponent<EnemyBase>().EnemyDamaged(finaldamage);
                //����Ʈ
                if (finaldamage <= 30)
                {
                    DebugX.Log("����� ����" + finaldamage);
                    EffectManager.instance.PlayerEffectOn("Slash", Player.instance.transform.position);
                }
                else if (finaldamage >= 31)
                {
                    DebugX.Log("������ ����" + finaldamage);
                    EffectManager.instance.PlayerEffectOn("SlashStrong", Player.instance.transform.position);
                }

            }

            else if (targets == "��ü����")
            {
                DebugX.Log("��ü����");

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

            //Ÿ�ٵ� ��밡 �׾������
            if (LastTarget != null && LastTarget.GetComponent<EnemyBase>().EnemyHp <= 0 && Player.instance.gameObject.GetComponent<SelectTarget>().enabled == true)
            {
                DebugX.Log("�׾���?)");
                LastTarget.tag = "Untagged";
                Player.instance.GetComponent<SelectTarget>().firstsearchtarget();
            }
        }
        // ������ �ʱ�ȭ

        combinationpanel.GetComponent<combinationpanel>().plusdamage = 0;
        combinationpanel.GetComponent<combinationpanel>().damagemulltiple = 1.0f;
    }
    public IEnumerator DamageEnemy(float damage, int Attackcount, bool NotChageTarget = false, bool NotAtkCount = false, float AtkDelay = 0)       //��Ÿ�� ������ �Լ�
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

        if (card84) //���� 84�� ī�带 ����ߴٸ�
        {
            //x�� �������� �� ����
            Array.Sort(Enemies, (e1, e2) => e1.transform.position.x.CompareTo(e2.transform.position.x));
        }

        int target = Random.Range(0, Enemies.Length);

        DebugX.Log("target : " + target);
        //���� ���õ� ������ ���� ���
        //GameObject LastTarget;

        if (!NotChageTarget)    //Ÿ�ٰ����� �ƴҶ�
        {
            //���� ���� ã��
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
            // Ÿ���� �����Ǿ����� �� Ÿ���ϰ� �ƴϸ� ������ �����Ȱ��� ����
            if (Player.instance.targetEnemy == null)
            {
                LastTarget = Enemies[target];
            }
            else
            {
                LastTarget = Player.instance.targetEnemy;

                if (card84) //���� 84�� ī�带 ����ߴٸ�
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

            if (card84) //���� 84�� ī�带 ����ߴٸ�
            {
                LastTarget_before = LastTarget;
                Card84_Function(target, Enemies);       //Ÿ�� ����
            }
        }
        else    //Ÿ�� �����ε�
        {
            if(LastTarget_before !=null)        //���� ������ ���� ��
            {
                LastTarget = LastTarget_before; //������ ���� Ÿ������ ����
            }
        }

        Vector3 Position = LastTarget.GetComponent<EnemyBase>().enemyposion.position;
        for (int j = 0; j < Attackcount; j++)
        {


            combinationpanel.GetComponent<combinationpanel>().attacktime += 1; //����Ƚ�� üũ



            ////////////////////////////////////////////////������ ���/////////////////////////////////////////////////////////////////////////

            if (Player.instance.Item10 == true && !NotAtkCount) //������  3��° ���� ������1.5�� �ϴ°�
            {
                TopUI.instance.FindItemNum(10, true);
            }


            //�߰� ������
            float finaldamage = TotalDmgCal(damage);
            //������ ����
            finaldamage *= combinationpanel.GetComponent<combinationpanel>().damagemulltiple;
            
            if (Player.instance.paralysis)  //���� �����̸� ����
            {
                finaldamage /= 2;
            }
            
            //�ݿø�
            finaldamage = (int)(finaldamage + 0.5f);

            finaldamage -= Player.instance.fear; //������ŭ ������ ����
            ////////////////////////////////////////////////������ ���/////////////////////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////////////���Ӷ����ٰ� �׾������쿡 ó��
            if (LastTarget == null) DebugX.Log("��ƮŸ�� ����");
            else
            {
                LastTarget.GetComponent<EnemyBase>().EnemyDamaged(finaldamage);
            }
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            DebugX.Log(damage + " " + Player.instance.power + " " + finaldamage);
            //���� ���� ������
            combinationpanel.GetComponent<combinationpanel>().previousDamage = (int)finaldamage;
            DebugX.Log(combinationpanel.GetComponent<combinationpanel>().previousDamage.ToString());

            //Ÿ�ٵ� ��밡 �׾������
            if (LastTarget != null && LastTarget.GetComponent<EnemyBase>().EnemyHp <= 0 && Player.instance.gameObject.GetComponent<SelectTarget>().enabled == true)
            {
                DebugX.Log("�׾���?)");
                LastTarget.tag = "Untagged";
                Player.instance.GetComponent<SelectTarget>().firstsearchtarget();
            }



            // ������ �ʱ�ȭ
            combinationpanel.GetComponent<combinationpanel>().plusdamage = 0;
            combinationpanel.GetComponent<combinationpanel>().damagemulltiple = 1.0f;

            //84�� ī�� ȿ��
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
                else    //������ ����� ������
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
            DebugX.Log("�� �̸�" + e.name);
        }

        if (Enemies.Length == 3)
        {
            if (target == 0)   //ó�� �ڸ�
            {
                card84_target = Enemies[1];
            }
            else if(target ==1) //�� ��° �ڸ�
            {
                card84_target = Enemies[2];
            }
        }
        else if (Enemies.Length == 2)
        {
            if (target == 0)   //ó���ڸ�
            {
                card84_target = Enemies[1];
            }
        }
    }
}
