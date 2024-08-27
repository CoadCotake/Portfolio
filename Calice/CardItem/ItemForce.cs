using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemForce : MonoBehaviour
{
    public static ItemForce instance = null;
    
    public TopUI topUi;
    public GameObject drawBtn;
    public GameObject mycardpanel;
    public combinationpanel combinationpanel;
    public bool openX = true;
    public bool RandomPlusone = false;

    public GameObject RewardGold;
    float insnum;//�ӽ� ������

    public void Awake_itemForce()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        if (SceneManager.GetActiveScene().name.Contains("Battle"))
        {
            Player.instance.playerPlusHP = 0;
        }
        else if (SceneManager.GetActiveScene().name == "Shop")
        {
            Player.instance.minimumGold = 0;
        }
        else if(SceneManager.GetActiveScene().name.Contains("Event"))
        {
            Player.instance.playerPlusHP = 0;
        }

        topUi = FindObjectOfType<TopUI>();
    }

    public void ItemForceOn(int itemNum)
    {
        if(!topUi)      //����ó��
            topUi = FindObjectOfType<TopUI>();

        DebugX.Log("Itemforce ON : " + itemNum);
        int idx = Collection.instance.ItemDB.Entities.FindIndex(a => a.num == itemNum);
        int type = Collection.instance.ItemDB.Entities[idx].type;

        switch (type)
        {
            case 0:
                break;
            case 1:
                BuffItem(Collection.instance.ItemDB.Entities[idx].power, Collection.instance.ItemDB.Entities[idx].shield, Collection.instance.ItemDB.Entities[idx].buff);
                break;
            case 2:
                SystemItem(Collection.instance.ItemDB.Entities[idx].num);
                break;
            case 3:
                BuffItem(Collection.instance.ItemDB.Entities[idx].power, Collection.instance.ItemDB.Entities[idx].shield, Collection.instance.ItemDB.Entities[idx].buff);
                SystemItem(Collection.instance.ItemDB.Entities[idx].num);
                break;
        }
    }

    public void ItemForceOff(int itemNum, bool isLock = false)
    {
        DebugX.Log("Itemforce OFF : " + itemNum);
        int idx = Collection.instance.ItemDB.Entities.FindIndex(a => a.num == itemNum);
        int type = Collection.instance.ItemDB.Entities[idx].type;

        switch (type)
        {
            case 0:
                break;
            case 1:
                BuffItemOff(Collection.instance.ItemDB.Entities[idx].power, Collection.instance.ItemDB.Entities[idx].shield, Collection.instance.ItemDB.Entities[idx].buff, isLock);
                break;
            case 2:
                SystemItemOff(Collection.instance.ItemDB.Entities[idx].num, isLock);
                break;
            case 3:
                BuffItemOff(Collection.instance.ItemDB.Entities[idx].power, Collection.instance.ItemDB.Entities[idx].shield, Collection.instance.ItemDB.Entities[idx].buff, isLock);
                SystemItemOff(Collection.instance.ItemDB.Entities[idx].num, isLock);
                break;
        }
    }

    public void BuffItem(int power, int shield, int buff)
    {
        if(GameManager.instance.PlayerItemList.FindIndex(item => item == 32) <= 0)//ü�°���������
        {
            if (buff != 0)
            {
                Player.instance.PlayerMaxHp += buff;
                Player.instance.PlayerHp += buff;
                Player.instance.playerPlusHP += buff;
            }
        }
        else
        {
            if (buff != 0)
            {
                Player.instance.PlayerMaxHp = 30;
                if (GameManager.instance.PlayerHp >= 30)
                {
                    if (Player.instance.PlayerHp >= 30) 
                    {
                        Player.instance.PlayerHp = 30;
                    }
                }
                else
                {
                    Player.instance.PlayerHp = GameManager.instance.PlayerHp;
                }
                Player.instance.playerPlusHP = 0;
            }
            
        }

        if (SceneManager.GetActiveScene().name.Contains("Battle") || SceneManager.GetActiveScene().name.Contains("Map"))
        // ��ȣ - �� ó�� ������ �� ���� ������ ����Ǿߴ뼭 �� �߰��ص�
        {
            if (power != 0)
            {
                Player.instance.power += power;
            }

            if (shield != 0)
            {
                Player.instance.PlayerArmor += shield;
            }
        }

        Player.instance.topUI.UpdateTopUI();
    }

    public void BuffItemOff(int power, int shield, int buff, bool isLock)
    {
        Player.instance.power -= power;

        Player.instance.PlayerMaxHp -= buff;
        Player.instance.PlayerHp -= buff;

        if (Player.instance.PlayerHp< buff)
            Player.instance.PlayerHp = 1;

        Player.instance.playerPlusHP -= buff;

        topUi.UpdateTopUI();
    }

    int dieCount = 0;
    public void SystemItem(int itemNum)
    {
        //�������� ���Ǿ���ϴ� ������
        switch (itemNum)
        {
            case 34:    //10���� ���� óġ �� ������ ���ط��� 1 �����Ѵ�.(�ִ� ������ 10)
                GameManager.instance.Item34 = true;

                if (dieCount == 0)
                {
                    dieCount = GameManager.instance.Item34_K / Collection.instance.ItemDB.Entities[itemNum].buff;
                    GameManager.instance.Item34_N = dieCount;
                    if (GameManager.instance.Item34_N > Collection.instance.ItemDB.Entities[itemNum].system)
                    {
                        GameManager.instance.Item34_N = Collection.instance.ItemDB.Entities[itemNum].system;
                    }
                    GameManager.instance.Item34_N *= Collection.instance.ItemDB.Entities[itemNum].power;    //�Ŀ��� ���� ��ġ = ���ط�
                    Player.instance.power += GameManager.instance.Item34_N;
                }
                else
                {
                    int newDieCount = GameManager.instance.Item34_K / Collection.instance.ItemDB.Entities[itemNum].buff;
                    if (newDieCount > dieCount)
                    {
                        Player.instance.power -= GameManager.instance.Item34_N;

                        dieCount = GameManager.instance.Item34_K / Collection.instance.ItemDB.Entities[itemNum].buff;
                        GameManager.instance.Item34_N = dieCount;
                        if (GameManager.instance.Item34_N > Collection.instance.ItemDB.Entities[itemNum].system)
                        {
                            GameManager.instance.Item34_N = Collection.instance.ItemDB.Entities[itemNum].system;
                        }
                        GameManager.instance.Item34_N *= Collection.instance.ItemDB.Entities[itemNum].power;    //�Ŀ��� ���� ��ġ = ���ط�
                        Player.instance.power += GameManager.instance.Item34_N;
                    }
                }
                DebugX.Log("óġ��" + GameManager.instance.Item34_K + "����" + GameManager.instance.Item34_N + "�����Ŀ�"+ Player.instance.power);
                break;
            case 36:    //������ ����ī���� 10���� 1��ŭ ���ط��� �����Ѵ�.
                Player.instance.Item36 = (int)GameManager.instance.MyCombiCard.Count / Collection.instance.ItemDB.Entities[itemNum].system;   //���ط� ���, �ý��� ������ġ = x���� 1 (x�κ�)
                Player.instance.power += Player.instance.Item36;    //���ط� ����
                DebugX.Log("���ط�" + Player.instance.Item36 + "�����Ŀ�" + Player.instance.power);
                break;
            case 37:    //������ ����� 100���� 1��ŭ ���ط��� �����Ѵ�.(�ִ� ������ 3)
                
                if(Player.instance.Item37!=-1)
                {
                    Player.instance.power -= Player.instance.Item37;     //���ط� ���� ����
                }


                Player.instance.Item37 = (int)Player.instance.PlayerGold / Collection.instance.ItemDB.Entities[itemNum].system;    //������ ȿ�� ���ط� ����, �ý��ۿ� ���� ��ġ = x���� 1 (x�κ�)

                if (Player.instance.Item37 > Collection.instance.ItemDB.Entities[itemNum].power)       //�ִ������� Ȯ��, �Ŀ��� ���� ��ġ =�ִ� ������
                {
                    Player.instance.Item37 = Collection.instance.ItemDB.Entities[itemNum].power;
                }

                Player.instance.power += Player.instance.Item37;     //���ط� ���� ����
                break;
            case 48:    //������ ȹ�� �� n��带 ȹ���Ѵ�.
                Player.instance.Item48 = true;
                Player.instance.Item48_ = Collection.instance.ItemDB.Entities[itemNum].system;
                break;
            case 49:    //�������� ��ü �� Ƚ����ŭ ���ݷ��� �����Ѵ�. �ִ� 3
                Player.instance.Item49 = true;
                if (GameManager.instance.ItemChangeCount < 3)
                {
                    Player.instance.power += GameManager.instance.ItemChangeCount;
                }
                else
                {
                    Player.instance.power += Collection.instance.ItemDB.Entities[itemNum].system; //�ý��� ��ġ = �ִ� ������
                }
                DebugX.Log("���ط�" + Player.instance.Item49 + "�����Ŀ�" + Player.instance.power);
                break;
        }

        //Ư�� �������� ���Ǵ� ������
        if ((SceneManager.GetActiveScene().name.Contains("Battle") && Player.instance.isBattle==true))
        {
            switch (itemNum)
            {
                case 4:
                    Player.instance.Item_PlusNumCardDrop += Collection.instance.ItemDB.Entities[itemNum].system; //����ī�� ��ο� 1 ����
                    break;
                case 5:
                    mycardpanel.GetComponent<mycard>().NormalAttackzone.SetActive(true);
                    break;
                case 6:
                    RandomPlusone = true;
                    break;
                case 7:
                    StartCoroutine(damage(Collection.instance.ItemDB.Entities[itemNum].system, false));
                    break;
                case 8:
                    RewardGold.GetComponent<Reward>().plusgold += 10;
                    break;
                case 9:
                    int a = Random.Range(0, 101);
                    if (a < 50)
                    {
                        Player.instance.PlayerHp += (int)(Player.instance.PlayerMaxHp * 0.01f * Collection.instance.ItemDB.Entities[itemNum].buff);
                        Player.instance.FirstHeal = true;
                        if (Player.instance.PlayerHp > Player.instance.PlayerMaxHp)
                            Player.instance.PlayerHp = Player.instance.PlayerMaxHp;
                    }
                    else
                    {
                        if (Player.instance.PlayerHp > 10)
                            Player.instance.PlayerHp -= Collection.instance.ItemDB.Entities[itemNum].system;
                        else
                            Player.instance.PlayerHp = 1;
                    }

                    GameManager.instance.PlayerHp = Player.instance.PlayerHp;
                    break;
                case 10:
                    //3��° ����ī�� ���ݸ��� ������ 1.5��
                    Player.instance.Item10 = true;
                    break;
                case 11:
                    Player.instance.boolreturnDamage = true;
                    Player.instance.returndamage += Collection.instance.ItemDB.Entities[itemNum].system;
                    break;
                case 12:
                    Player.instance.boolreturnDamage = true;
                    Player.instance.returnpercentdamage += Collection.instance.ItemDB.Entities[itemNum].system;
                    break;
                case 13:
                    Player.instance.gameObject.GetComponent<SelectTarget>().enabled = true;
                    break;
                case 14:
                    Player.instance.minimumGold -= 10;
                    break;
                case 16:
                    Player.instance.Item_PlusComCardDrop += Collection.instance.ItemDB.Entities[itemNum].system; // ����ī�� ��ο� 1����
                    break;
                case 18:
                    Player.instance.Item_PlusNumCardDrop += Collection.instance.ItemDB.Entities[itemNum].system; //����ī�� ��ο� 1 ����
                    break;
                case 19:
                    Player.instance.PlayerGetArmor(Collection.instance.ItemDB.Entities[itemNum].system, false);
                    Player.instance.Item19 = true;
                    break;
                case 21:
                    if (EnemyChecker.instance == null || EnemyChecker.instance.IsReward == false)
                    {
                        Player.instance.PlayerGetArmor(5, false,false);
                        Player.instance.HealHP(5, false,true, false);
                        Player.instance.FirstHeal = true;
                    }
                    break;
                case 22:
                    GamePlayBtn.instance.Item22 = true;
                    break;
                case 23://�ǰ� ���� ��ŭ ���� �Ͽ� ����ī�带 �߰��� �̴´�.
                    GamePlayBtn.instance.Item23 = true;
                    break;
                case 24:
                    Player.instance.Item24 = true;
                    break;                
                case 27:
                    Player.instance.Item27 = Collection.instance.ItemDB.Entities[itemNum].shield;
                    break;
                case 29:
                    Player.instance.Item29 = true;
                    break;
                case 30: //����ī�带 5�� ��� �� ������ ����ī�带 1�� �̴´�.
                    Player.instance.Item30 = Collection.instance.ItemDB.Entities[itemNum].system;
                    break;
                case 31:
                    Player.instance.isStatePlusDmg = true;
                    Player.instance.statePlusDmg += Collection.instance.ItemDB.Entities[itemNum].power;
                    break;
                case 32:
                    Player.instance.PlayerMaxHp = 30;
                    if (Player.instance.PlayerHp >= 30)
                    {
                        Player.instance.PlayerHp = 30;
                    }

                    Player.instance.playerPlusHP = 0;
                    Player.instance.PlayerHpBarShow();
                    Player.instance.PlayerGetArmor(Collection.instance.ItemDB.Entities[itemNum].shield, false,false);
                    Player.instance.everlastingArmor = true;
                    Player.instance.halfArmor = true;
                    break;
                case 33:
                    GameObject.Find("CardCanvas").GetComponent<GamePlayBtn>().hpdrawbutton.SetActive(true);
                    break;
                case 35:    //���� ���� �� ������ �� �ϳ����� ������¸� 1�� �ο��Ѵ�.
                    StartCoroutine(State("����", Collection.instance.ItemDB.Entities[itemNum].system));
                    break;
                case 38:    //���� ���� �� 5�� ���ظ� �԰� �������� ���ط��� 3�����Ѵ�.
                    /*if (Player.instance.PlayerHp >= Collection.instance.ItemDB.Entities[itemNum].system)
                    {
                        Player.instance.PlayerDamaged(Collection.instance.ItemDB.Entities[itemNum].system);      //���۽� ������ �Ա�, �ý��� ��ġ = �������� ��ġ
                    }
                    else
                    {
                        Player.instance.PlayerDamaged(Player.instance.PlayerHp-1);  //�� 1�� ��Ƴ�����
                    }*/
                    Player.instance.PlayerDamaged(Collection.instance.ItemDB.Entities[itemNum].system);      //���۽� ������ �Ա�, �ý��� ��ġ = �������� ��ġ (Hp���� �������� ũ�� �����)
                    Player.instance.power += Collection.instance.ItemDB.Entities[itemNum].power;    //���ط� ����, �Ŀ���ġ = ������ų ���ط�
                    break;
                case 39:    //�ǰ� ���� Ƚ���� �ݸ�ŭ ���� �Ͽ� ���ڸ� �߰��� �̴´�.
                    Player.instance.Item39 = true;
                    break;
                case 40:    //���� �� ó�� ���� �����̻��� ���� �ʴ´�.
                    Player.instance.Item40 = true;
                    break;
                case 41:    //���� óġ �� ������ ü�� 3 ȸ���Ѵ�.
                    Player.instance.Item41= Collection.instance.ItemDB.Entities[itemNum].system;   //�ý��� ��ġ = ȸ�� ��ġ
                    break;
                case 42:    //Ȧ�� �Ͽ��� ����ī�带 1�� �߰��� �̴´�.
                    Player.instance.Item42 = Collection.instance.ItemDB.Entities[itemNum].system;   //�ý��� ��ġ = ī�� ����
                    break;
                case 43:    //¦�� �Ͽ��� ���ڸ� 1�� �߰��� �̴´�.
                    Player.instance.Item43 = Collection.instance.ItemDB.Entities[itemNum].system;   //�ý��� ��ġ = ī�� ����
                    break;
                case 45:    //ü���� �ִ� ü���� 50%������ ��� ���ݷ��� 5�����մϴ�.
                    Player.instance.Item45 = true;   //
                    Player.instance.Item45_1 = Collection.instance.ItemDB.Entities[itemNum].power;   //���ݷ� ���� ��ġ
                    Player.instance.Item45_2 = Collection.instance.ItemDB.Entities[itemNum].system;   //
                    break;
                case 46:    //���� óġ�ϸ� 1~10������ ���ڸ� 1�� �߰��� �̴´�.
                    Player.instance.Item46= Collection.instance.ItemDB.Entities[itemNum].system;   //�ý��� ��ġ = �߰��� �̴� ����
                    break;
                case 47:    //7��° ����ϴ� ����ī�尡 ����ī���̸� ���ط��� 2�谡 �ȴ�. ���� �� ���ط��� ������� ���ƿ´�.
                    Player.instance.Item47 = Collection.instance.ItemDB.Entities[itemNum].power;   //�Ŀ� ��ġ = ���ط� ����
                    Player.instance.Item47_ = Collection.instance.ItemDB.Entities[itemNum].system;   //�ý��� ��ġ = n��° ���
                    break;
                case 50:    //10�ϸ��� HP�� 3 ȸ���Ѵ�. �� ���� ���� ���۽� �ʱ�ȭ�ȴ�.
                    Player.instance.Item50 = Collection.instance.ItemDB.Entities[itemNum].buff;   //���� ��ġ = ȸ�� ��ġ
                    Player.instance.Item50_ = Collection.instance.ItemDB.Entities[itemNum].system;   //�ý��� ��ġ = �� ��ġ
                    break;
                case 51: //���� �� Ȧ�� �� ���ط� 3����, ¦�� �� ���� 10ȹ��
                    Player.instance.Item51_power = Collection.instance.ItemDB.Entities[itemNum].power;
                    Player.instance.Item51_shield = Collection.instance.ItemDB.Entities[itemNum].shield;
                    break;
                case 52: //���� �� 10%Ȯ���� ���ط��� 2�谡 �ȴ�.
                    Player.instance.Item52 = true;
                    break;
                case 53: //5ȸ �ǰ� �� ������ ������ ���� 2�� �ο��Ѵ�.
                    break;
                case 54: //5ȸ �ǰ� �� ������ ������ ���� 2�� �ο��Ѵ�.
                    break;
                case 55: //5ȸ �ǰ� �� ������ ������ ȭ���� 2�� �ο��Ѵ�.
                    break;
                case 56: //5ȸ �ǰ� �� ������ ������ ������ 2�� �ο��Ѵ�.
                    break;
                case 58: //���� ���� ���� �� ü���� 3 ȸ���Ѵ�.
                    Player.instance.HealHP(Collection.instance.ItemDB.Entities[itemNum].buff,false,false,false);
                    Player.instance.FirstHeal = true;
                    break;
                case 60: //���� ���� ���� �� �� ����� �ִ� ü���� 5%���ҵ� ���·� �����Ѵ�.
                    StartCoroutine(ReduceHp(60));
                    break;
                case 61: //���� ���� ���� �� �� �� �ϳ��� ü���� 10%���ҵ� ���·� �����Ѵ�.
                    StartCoroutine(ReduceHp(61));
                    break;
            }
        }
        else if (SceneManager.GetActiveScene().name.Contains("Shop")) // �������������
        {
            switch (itemNum)
            {
                case 14:
                    Player.instance.minimumGold -= 10;
                    break;
                case 28:
                    Player.instance.Item28 = true;
                    break;
                case 32:
                    Player.instance.PlayerMaxHp = 30;
                    if (GameManager.instance.PlayerHp >= 30)
                    {
                        if (Player.instance.PlayerHp >= 30)
                        {
                            Player.instance.PlayerHp = 30;
                        }
                    }
                    else
                    {
                        Player.instance.PlayerHp = GameManager.instance.PlayerHp;
                    }
                    Player.instance.playerPlusHP = 0;
                    topUi.UpdateTopUI();
                    break;
            }
        }
        else if (SceneManager.GetActiveScene().name.Contains("Rest")) // �޽����������
        {
            switch (itemNum)
            {
                case 44:    //�޽Ŀ��� ȸ������ 5 �����մϴ�.
                    Player.instance.Item44 = Collection.instance.ItemDB.Entities[itemNum].system;
                    break;
                case 59: //�޽� ���� ���� �� ��带 10 �Ҹ��ϰ� ü���� 10 ȸ���Ѵ�. (��尡 10 �̸��� ��� ȸ������ �ʴ´�.)
                    if (Player.instance.PlayerGold >= Collection.instance.ItemDB.Entities[itemNum].system)
                    {
                        Player.instance.PlayerPayGold(Collection.instance.ItemDB.Entities[itemNum].system);
                        Player.instance.HealHP(Collection.instance.ItemDB.Entities[itemNum].buff);
                    }
                    break;
            }
        }
        else
        {
            switch (itemNum)
            {
                case 25: //���� ���� ���� �� ü���� 5 ȸ���Ѵ�.
                    Player.instance.Item25 = true;
                    break;
                case 26: //���� �������� ������ �߻����� �ʴ´�.
                    Player.instance.Item26 = true;
                    break;
                case 32:
                    Player.instance.PlayerMaxHp = 30;
                    if (GameManager.instance.PlayerHp >= 30)
                    {
                        if (Player.instance.PlayerHp >= 30)
                        {
                            Player.instance.PlayerHp = 30;
                        }
                    }
                    else
                    {
                        Player.instance.PlayerHp = GameManager.instance.PlayerHp;
                    }
                    Player.instance.playerPlusHP = 0;
                    topUi.UpdateTopUI();
                    break;
                case 57: //���� ���� ���� �� ��带 20 ȹ���Ѵ�.
                    Player.instance.Item57 = true;
                    break;
            }
        }
    }

    public void SystemItemOff(int itemNum, bool isLock = false)
    {
        if (SceneManager.GetActiveScene().name.Contains("Battle"))
        {
            switch (itemNum)
            {
                case 4:
                    Player.instance.Item_PlusNumCardDrop -= Collection.instance.ItemDB.Entities[itemNum].system; //����ī�� ��ο� ����
                    break;
                case 5:
                    mycardpanel.GetComponent<mycard>().NormalAttackzone.SetActive(false);
                    break;
                case 13:
                    Player.instance.gameObject.GetComponent<SelectTarget>().RemoveTargetItem();
                    break;
                case 16:
                    Player.instance.Item_PlusComCardDrop -= Collection.instance.ItemDB.Entities[itemNum].system;
                    break;
                case 18:
                    Player.instance.Item_PlusNumCardDrop -= Collection.instance.ItemDB.Entities[itemNum].system; //����ī�� ��ο� ����
                    break;
                case 19:
                    Player.instance.Item19 = false;
                    break;
                case 21:
                    break;
                case 22:
                    GamePlayBtn.instance.Item22 = false;
                    break;
                case 23://�ǰ� ���� ��ŭ ���� �Ͽ� ����ī�带 �߰��� �̴´�.
                    GamePlayBtn.instance.Item23 = false;
                    break;
                case 24:
                    Player.instance.Item24 = false;
                    break;
                case 27:
                    Player.instance.Item27 = 0;
                    break;
                case 29:
                    Player.instance.Item29 = false;
                    break;
                case 30: //����ī�带 5�� ��� �� ������ ����ī�带 1�� �̴´�.
                    Player.instance.Item30 = 0;
                    break;
                case 31:
                    //Player.instance.isStatePlusDmg = true;
                    Player.instance.statePlusDmg -= Collection.instance.ItemDB.Entities[itemNum].power;
                    break;
                case 34:    //10���� ���� óġ �� ������ ���ط��� 1 �����Ѵ�.(�ִ� ������ 10)
                    if (isLock)
                    {
                        GameManager.instance.Item34 = false;
                        Player.instance.power -= GameManager.instance.Item34_N;
                    }
                    else
                    {
                        Player.instance.power -= GameManager.instance.Item34_N;
                        GameManager.instance.Item34_N = -1;
                        GameManager.instance.Item34 = false;
                    }
                    break;
                case 36:    //������ ����ī���� 10���� 1��ŭ ���ط��� �����Ѵ�.

                    Player.instance.power -= Player.instance.Item36;
                    Player.instance.Item36 = -1;

                    break;
                case 37:    //������ ����� 100���� 1��ŭ ���ط��� �����Ѵ�.(�ִ� ������ 3)
                    Player.instance.power -= Player.instance.Item37;
                    Player.instance.Item37 = 0;
                    break;
                case 38:    //���� ���� �� 5�� ���ظ� �԰� �������� ���ط��� 3�����Ѵ�.
                    Player.instance.power -= Collection.instance.ItemDB.Entities[itemNum].power;    //���ط� ����, �Ŀ���ġ = ������ų ���ط�
                    break;
                case 39:    //�ǰ� ���� Ƚ���� �ݸ�ŭ ���� �Ͽ� ���ڸ� �߰��� �̴´�.
                    Player.instance.Item39 = false;
                    break;
                case 40:    //���� �� ó�� ���� �����̻��� ���� �ʴ´�.
                    Player.instance.Item40 = false;
                    break;
                case 41:    //���� óġ �� ������ ü�� 3 ȸ���Ѵ�.
                    Player.instance.Item41 = 0;
                    break;
                case 42:    //Ȧ�� �Ͽ��� ����ī�带 1�� �߰��� �̴´�.
                    Player.instance.Item42 = 0;
                    break;
                case 43:    //¦�� �Ͽ��� ���ڸ� 1�� �߰��� �̴´�.
                    Player.instance.Item43 = 0;
                    break;
                case 45:    //ü���� �ִ� ü���� 50%������ ��� ���ݷ��� 5�����մϴ�.
                    if (Player.instance.PlayerHp <= Player.instance.PlayerMaxHp / (100 / Player.instance.Item45_2)) //���� ü�� < ������ ü�� ���
                    {
                        Player.instance.power -= Player.instance.Item45_1;
                    }
                    Player.instance.Item45 = false;
                    Player.instance.Item45_1 = 0;
                    Player.instance.Item45_2 = 0;
                    break;
                case 46:    //���� óġ�ϸ� 1~10������ ���ڸ� 1�� �߰��� �̴´�.
                    Player.instance.Item46 = 0;   //�ý��� ��ġ = �߰��� �̴� ����
                    break;
                case 47:    //7��° ����ϴ� ����ī�尡 ����ī���̸� ���ط��� 2�谡 �ȴ�. ���� �� ���ط��� ������� ���ƿ´�.
                    Player.instance.Item47 = 0;   //�Ŀ� ��ġ = ���ط� ����
                    Player.instance.Item47_ = 0;   //�ý��� ��ġ = n��° ���
                    break;
                case 50:    //10�ϸ��� HP�� 3 ȸ���Ѵ�. �� ���� ���� ���۽� �ʱ�ȭ�ȴ�.
                    Player.instance.Item50 = 0;   //���� ��ġ = ȸ�� ��ġ
                    Player.instance.Item50_ = 0;   //�ý��� ��ġ = �� ��ġ
                    break;
                case 51: //���� �� Ȧ�� �� ���ط� 3����, ¦�� �� ���� 10ȹ��
                    Player.instance.Item51_power = 0;
                    Player.instance.Item51_shield = 0;
                    break;
                case 52: //���� �� 10%Ȯ���� ���ط��� 2�谡 �ȴ�.
                    Player.instance.Item52 = false;
                    break;
                case 53: //5ȸ �ǰ� �� ������ ������ ���� 2�� �ο��Ѵ�.
                    break;
                case 54: //5ȸ �ǰ� �� ������ ������ ���� 2�� �ο��Ѵ�.
                    break;
                case 55: //5ȸ �ǰ� �� ������ ������ ȭ���� 2�� �ο��Ѵ�.
                    break;
                case 56: //5ȸ �ǰ� �� ������ ������ ������ 2�� �ο��Ѵ�.
                    break;
                case 58: //���� ���� ���� �� ü���� 3 ȸ���Ѵ�.
                    break;
                case 60: //���� ���� ���� �� �� ����� �ִ� ü���� 5%���ҵ� ���·� �����Ѵ�.
                    break;
                case 61: //���� ���� ���� �� �� �� �ϳ��� ü���� 10%���ҵ� ���·� �����Ѵ�.
                    break;
            }
        }

        switch (itemNum)
        {
            case 6:
                RandomPlusone = false;
                break;
            case 7:
                break;
            case 8:
                if(RewardGold)
                    RewardGold.GetComponent<Reward>().plusgold -= 5;
                break;
            case 9:
                break;
            case 10:
                //3��° ����ī�� ���ݸ��� ������ 1.5��
                Player.instance.Item10 = true;
                break;
            case 11:                
                Player.instance.returndamage -= Collection.instance.ItemDB.Entities[itemNum].system;

                if (Player.instance.returndamage == 0 && Player.instance.returnpercentdamage == 0 && Player.instance.returndamage_buff == 0)   //�ݻ� ���� ������ �׸��� ������ ���ٸ�
                {
                    Player.instance.boolreturnDamage = false;   //�ݻ� ����
                }
                break;
            case 12:                
                Player.instance.returnpercentdamage -= Collection.instance.ItemDB.Entities[itemNum].system;

                if (Player.instance.returndamage == 0 && Player.instance.returnpercentdamage == 0 && Player.instance.returndamage_buff == 0)   //�ݻ� ���� ������ �׸��� ������ ���ٸ�
                {
                    Player.instance.boolreturnDamage = false;   //�ݻ� ����
                }
                break;
            case 14:
                Player.instance.minimumGold += 10;
                break;
            case 25: //���� ���� ���� �� ü���� 5 ȸ���Ѵ�.
                break;
            case 26: //���� �������� ������ �߻����� �ʴ´�.
                break;
            case 28:
                Player.instance.Item28 = false;
                break;
            case 32:              
                insnum = Player.instance.PlayerMaxHp + 70;
                Player.instance.PlayerHp = Mathf.Ceil(insnum * (Player.instance.PlayerHp / Player.instance.PlayerMaxHp));
                Player.instance.PlayerMaxHp += 70;
                Player.instance.everlastingArmor = false;
                Player.instance.halfArmor = false;
                break;
            case 33:
                GameObject.Find("CardCanvas").GetComponent<GamePlayBtn>().hpdrawbutton.SetActive(false);
                break;
            case 48:    //������ ȹ�� �� 50��带 ȹ���Ѵ�.
                Player.instance.Item48 = false;
                Player.instance.Item48_ = 0;
                break;
            case 49:    //�������� ��ü �� Ƚ����ŭ ���ݷ��� �����Ѵ�. �ִ� 3
                Player.instance.Item49 = false;
                if (GameManager.instance.ItemChangeCount < 3)
                {
                    Player.instance.power -= GameManager.instance.ItemChangeCount;
                }
                else
                {
                    Player.instance.power -= Collection.instance.ItemDB.Entities[itemNum].system; //�ý��� ��ġ = �ִ� ������
                }
                GameManager.instance.ItemResetSave(49);
                break;
        }
        topUi.UpdateTopUI();
    }

    IEnumerator damage(int itemDmg, bool effect = true)
    {
        yield return new WaitForSeconds(0.2f);
        DamageEnemy(itemDmg,effect);
    }

    public void DamageEnemy(int itemDmg,bool effect)
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (Enemies.Length > 0)
        {

            int rand = Random.Range(0, Enemies.Length);

            if(effect)
                EffectManager.instance.PlayerEffectOn("Slash", Enemies[rand].transform.position);

            Enemies[rand].GetComponent<EnemyBase>().EnemyDamaged(itemDmg, false);
        }
    }

    IEnumerator State(string name, int turn)
    {
        yield return new WaitForSeconds(0.2f);
        StateEnemy(name, turn);
    }
    public void StateEnemy(string name, int turn)
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (Enemies.Length > 0)
        {
            int rand = Random.Range(0, Enemies.Length);

            //EffectManager.instance.PlayerEffectOn("Slash", Enemies[rand].transform.position);

            switch (name)
            {
                case "����":
                    Enemies[rand].GetComponent<EnemyBase>().EnemyStateAdd("����", turn);
                    FindObjectOfType<StateEffect_e>().StateEffectOn(3, true, Enemies[rand]);
                    EffectManager.instance.EnemyCommoneEffectOn(Enemies[rand].name, CommonEffectName.Paralysis, Enemies[rand].GetComponent<EnemyBase>().enemyposion.position);
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator ReduceHp(int itemNum)
    {
        yield return new WaitForSeconds(0.4f);
        ReduceHpCor(itemNum);
    }
    public void ReduceHpCor(int itemNum)
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (Enemies.Length > 0)
        {
            switch(itemNum)
            {
                case 60: //���� ���� ���� �� �� ����� �ִ� ü���� 5%���ҵ� ���·� �����Ѵ�.
                    for(int num = 0; num < Enemies.Length; num++)
                    {
                        EnemyBase enemy = Enemies[num].GetComponent<EnemyBase>();
                        int enemyNewHp = (int)enemy.EnemyHp - (int)(enemy.EnemyMaxHp * 0.05f);
                        enemy.EnemyHp = enemyNewHp;
                        enemy.EnemyHpBarShow();
                    }
                    break;
                case 61: //���� ���� ���� �� �� �� �ϳ��� ü���� 10%���ҵ� ���·� �����Ѵ�.
                    int rand = Random.Range(0, Enemies.Length);
                    EnemyBase target = Enemies[rand].GetComponent<EnemyBase>();
                    int targetNewHp = (int)target.EnemyHp - (int)(target.EnemyMaxHp * 0.1f);
                    target.EnemyHp = targetNewHp;
                    target.EnemyHpBarShow();
                    break;
            }    
        }
    }
}
