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
    float insnum;//임시 변수값

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
        if(!topUi)      //예외처리
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
        if(GameManager.instance.PlayerItemList.FindIndex(item => item == 32) <= 0)//체력고정아이템
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
        // 구호 - 맵 처음 시작할 때 버프 아이템 적용되야대서 맵 추가해둠
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
        //모든곳에서 사용되어야하는 아이템
        switch (itemNum)
        {
            case 34:    //10명의 적을 처치 할 때마다 피해량이 1 증가한다.(최대 증가량 10)
                GameManager.instance.Item34 = true;

                if (dieCount == 0)
                {
                    dieCount = GameManager.instance.Item34_K / Collection.instance.ItemDB.Entities[itemNum].buff;
                    GameManager.instance.Item34_N = dieCount;
                    if (GameManager.instance.Item34_N > Collection.instance.ItemDB.Entities[itemNum].system)
                    {
                        GameManager.instance.Item34_N = Collection.instance.ItemDB.Entities[itemNum].system;
                    }
                    GameManager.instance.Item34_N *= Collection.instance.ItemDB.Entities[itemNum].power;    //파워에 적힌 수치 = 피해량
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
                        GameManager.instance.Item34_N *= Collection.instance.ItemDB.Entities[itemNum].power;    //파워에 적힌 수치 = 피해량
                        Player.instance.power += GameManager.instance.Item34_N;
                    }
                }
                DebugX.Log("처치수" + GameManager.instance.Item34_K + "증가" + GameManager.instance.Item34_N + "현재파워"+ Player.instance.power);
                break;
            case 36:    //보유한 조합카드의 10분의 1만큼 피해량이 증가한다.
                Player.instance.Item36 = (int)GameManager.instance.MyCombiCard.Count / Collection.instance.ItemDB.Entities[itemNum].system;   //피해량 계산, 시스템 적힌수치 = x분의 1 (x부분)
                Player.instance.power += Player.instance.Item36;    //피해량 증가
                DebugX.Log("피해량" + Player.instance.Item36 + "현재파워" + Player.instance.power);
                break;
            case 37:    //보유한 골드의 100분의 1만큼 피해량이 증가한다.(최대 증가량 3)
                
                if(Player.instance.Item37!=-1)
                {
                    Player.instance.power -= Player.instance.Item37;     //피해량 증가 해제
                }


                Player.instance.Item37 = (int)Player.instance.PlayerGold / Collection.instance.ItemDB.Entities[itemNum].system;    //아이템 효과 피해량 적용, 시스템에 적힌 수치 = x분의 1 (x부분)

                if (Player.instance.Item37 > Collection.instance.ItemDB.Entities[itemNum].power)       //최대증가량 확인, 파워에 적힌 수치 =최대 증가량
                {
                    Player.instance.Item37 = Collection.instance.ItemDB.Entities[itemNum].power;
                }

                Player.instance.power += Player.instance.Item37;     //피해량 증가 적용
                break;
            case 48:    //아이템 획득 시 n골드를 획득한다.
                Player.instance.Item48 = true;
                Player.instance.Item48_ = Collection.instance.ItemDB.Entities[itemNum].system;
                break;
            case 49:    //아이템을 교체 한 횟수만큼 공격력이 증가한다. 최대 3
                Player.instance.Item49 = true;
                if (GameManager.instance.ItemChangeCount < 3)
                {
                    Player.instance.power += GameManager.instance.ItemChangeCount;
                }
                else
                {
                    Player.instance.power += Collection.instance.ItemDB.Entities[itemNum].system; //시스템 수치 = 최대 증가량
                }
                DebugX.Log("피해량" + Player.instance.Item49 + "현재파워" + Player.instance.power);
                break;
        }

        //특정 공간에서 사용되는 아이템
        if ((SceneManager.GetActiveScene().name.Contains("Battle") && Player.instance.isBattle==true))
        {
            switch (itemNum)
            {
                case 4:
                    Player.instance.Item_PlusNumCardDrop += Collection.instance.ItemDB.Entities[itemNum].system; //숫자카드 드로우 1 증가
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
                    //3번째 조합카드 공격마다 데미지 1.5배
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
                    Player.instance.Item_PlusComCardDrop += Collection.instance.ItemDB.Entities[itemNum].system; // 조합카드 드로우 1증가
                    break;
                case 18:
                    Player.instance.Item_PlusNumCardDrop += Collection.instance.ItemDB.Entities[itemNum].system; //숫자카드 드로우 1 증가
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
                case 23://피격 당한 만큼 다음 턴에 조합카드를 추가로 뽑는다.
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
                case 30: //조합카드를 5장 사용 할 때마다 조합카드를 1장 뽑는다.
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
                case 35:    //전투 시작 시 무작위 적 하나에게 마비상태를 1턴 부여한다.
                    StartCoroutine(State("마비", Collection.instance.ItemDB.Entities[itemNum].system));
                    break;
                case 38:    //전투 시작 시 5의 피해를 입고 전투동안 피해량이 3증가한다.
                    /*if (Player.instance.PlayerHp >= Collection.instance.ItemDB.Entities[itemNum].system)
                    {
                        Player.instance.PlayerDamaged(Collection.instance.ItemDB.Entities[itemNum].system);      //시작시 데미지 입기, 시스템 수치 = 피해입을 수치
                    }
                    else
                    {
                        Player.instance.PlayerDamaged(Player.instance.PlayerHp-1);  //피 1로 살아남도록
                    }*/
                    Player.instance.PlayerDamaged(Collection.instance.ItemDB.Entities[itemNum].system);      //시작시 데미지 입기, 시스템 수치 = 피해입을 수치 (Hp보다 데미지가 크면 사망함)
                    Player.instance.power += Collection.instance.ItemDB.Entities[itemNum].power;    //피해량 증가, 파워수치 = 증가시킬 피해량
                    break;
                case 39:    //피격 당한 횟수의 반만큼 다음 턴에 숫자를 추가로 뽑는다.
                    Player.instance.Item39 = true;
                    break;
                case 40:    //전투 중 처음 받은 상태이상을 받지 않는다.
                    Player.instance.Item40 = true;
                    break;
                case 41:    //적을 처치 할 때마다 체력 3 회복한다.
                    Player.instance.Item41= Collection.instance.ItemDB.Entities[itemNum].system;   //시스템 수치 = 회복 수치
                    break;
                case 42:    //홀수 턴에만 조합카드를 1장 추가로 뽑는다.
                    Player.instance.Item42 = Collection.instance.ItemDB.Entities[itemNum].system;   //시스템 수치 = 카드 개수
                    break;
                case 43:    //짝수 턴에만 숫자를 1장 추가로 뽑는다.
                    Player.instance.Item43 = Collection.instance.ItemDB.Entities[itemNum].system;   //시스템 수치 = 카드 개수
                    break;
                case 45:    //체력이 최대 체력의 50%이하일 경우 공격력이 5증가합니다.
                    Player.instance.Item45 = true;   //
                    Player.instance.Item45_1 = Collection.instance.ItemDB.Entities[itemNum].power;   //공격력 증가 수치
                    Player.instance.Item45_2 = Collection.instance.ItemDB.Entities[itemNum].system;   //
                    break;
                case 46:    //적을 처치하면 1~10사이의 숫자를 1장 추가로 뽑는다.
                    Player.instance.Item46= Collection.instance.ItemDB.Entities[itemNum].system;   //시스템 수치 = 추가로 뽑는 개수
                    break;
                case 47:    //7번째 사용하는 조합카드가 공격카드이면 피해량이 2배가 된다. 공격 후 피해량은 원래대로 돌아온다.
                    Player.instance.Item47 = Collection.instance.ItemDB.Entities[itemNum].power;   //파워 수치 = 피해량 개수
                    Player.instance.Item47_ = Collection.instance.ItemDB.Entities[itemNum].system;   //시스템 수치 = n번째 사용
                    break;
                case 50:    //10턴마다 HP를 3 회복한다. 턴 수는 전투 시작시 초기화된다.
                    Player.instance.Item50 = Collection.instance.ItemDB.Entities[itemNum].buff;   //버프 수치 = 회복 수치
                    Player.instance.Item50_ = Collection.instance.ItemDB.Entities[itemNum].system;   //시스템 수치 = 턴 수치
                    break;
                case 51: //전투 중 홀수 턴 피해량 3증가, 짝수 턴 쉴드 10획득
                    Player.instance.Item51_power = Collection.instance.ItemDB.Entities[itemNum].power;
                    Player.instance.Item51_shield = Collection.instance.ItemDB.Entities[itemNum].shield;
                    break;
                case 52: //공격 시 10%확률로 피해량이 2배가 된다.
                    Player.instance.Item52 = true;
                    break;
                case 53: //5회 피격 시 공격한 적에게 마비를 2턴 부여한다.
                    break;
                case 54: //5회 피격 시 공격한 적에게 독를 2턴 부여한다.
                    break;
                case 55: //5회 피격 시 공격한 적에게 화상을 2턴 부여한다.
                    break;
                case 56: //5회 피격 시 공격한 적에게 출혈을 2턴 부여한다.
                    break;
                case 58: //전투 지역 진입 시 체력을 3 회복한다.
                    Player.instance.HealHP(Collection.instance.ItemDB.Entities[itemNum].buff,false,false,false);
                    Player.instance.FirstHeal = true;
                    break;
                case 60: //전투 지역 진입 시 적 모두의 최대 체력을 5%감소된 상태로 전투한다.
                    StartCoroutine(ReduceHp(60));
                    break;
                case 61: //전투 지역 진입 시 적 중 하나의 체력을 10%감소된 상태로 전투한다.
                    StartCoroutine(ReduceHp(61));
                    break;
            }
        }
        else if (SceneManager.GetActiveScene().name.Contains("Shop")) // 상점전용아이템
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
        else if (SceneManager.GetActiveScene().name.Contains("Rest")) // 휴식전용아이템
        {
            switch (itemNum)
            {
                case 44:    //휴식에서 회복량이 5 증가합니다.
                    Player.instance.Item44 = Collection.instance.ItemDB.Entities[itemNum].system;
                    break;
                case 59: //휴식 지역 입장 시 골드를 10 소모하고 체력을 10 회복한다. (골드가 10 미만일 경우 회복하지 않는다.)
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
                case 25: //미지 지역 진입 시 체력을 5 회복한다.
                    Player.instance.Item25 = true;
                    break;
                case 26: //미지 지역에서 전투가 발생하지 않는다.
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
                case 57: //미지 지역 진입 시 골드를 20 획득한다.
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
                    Player.instance.Item_PlusNumCardDrop -= Collection.instance.ItemDB.Entities[itemNum].system; //숫자카드 드로우 증가
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
                    Player.instance.Item_PlusNumCardDrop -= Collection.instance.ItemDB.Entities[itemNum].system; //숫자카드 드로우 증가
                    break;
                case 19:
                    Player.instance.Item19 = false;
                    break;
                case 21:
                    break;
                case 22:
                    GamePlayBtn.instance.Item22 = false;
                    break;
                case 23://피격 당한 만큼 다음 턴에 조합카드를 추가로 뽑는다.
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
                case 30: //조합카드를 5장 사용 할 때마다 조합카드를 1장 뽑는다.
                    Player.instance.Item30 = 0;
                    break;
                case 31:
                    //Player.instance.isStatePlusDmg = true;
                    Player.instance.statePlusDmg -= Collection.instance.ItemDB.Entities[itemNum].power;
                    break;
                case 34:    //10명의 적을 처치 할 때마다 피해량이 1 증가한다.(최대 증가량 10)
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
                case 36:    //보유한 조합카드의 10분의 1만큼 피해량이 증가한다.

                    Player.instance.power -= Player.instance.Item36;
                    Player.instance.Item36 = -1;

                    break;
                case 37:    //보유한 골드의 100분의 1만큼 피해량이 증가한다.(최대 증가량 3)
                    Player.instance.power -= Player.instance.Item37;
                    Player.instance.Item37 = 0;
                    break;
                case 38:    //전투 시작 시 5의 피해를 입고 전투동안 피해량이 3증가한다.
                    Player.instance.power -= Collection.instance.ItemDB.Entities[itemNum].power;    //피해량 증가, 파워수치 = 증가시킬 피해량
                    break;
                case 39:    //피격 당한 횟수의 반만큼 다음 턴에 숫자를 추가로 뽑는다.
                    Player.instance.Item39 = false;
                    break;
                case 40:    //전투 중 처음 받은 상태이상을 받지 않는다.
                    Player.instance.Item40 = false;
                    break;
                case 41:    //적을 처치 할 때마다 체력 3 회복한다.
                    Player.instance.Item41 = 0;
                    break;
                case 42:    //홀수 턴에만 조합카드를 1장 추가로 뽑는다.
                    Player.instance.Item42 = 0;
                    break;
                case 43:    //짝수 턴에만 숫자를 1장 추가로 뽑는다.
                    Player.instance.Item43 = 0;
                    break;
                case 45:    //체력이 최대 체력의 50%이하일 경우 공격력이 5증가합니다.
                    if (Player.instance.PlayerHp <= Player.instance.PlayerMaxHp / (100 / Player.instance.Item45_2)) //현재 체력 < 정해진 체력 계수
                    {
                        Player.instance.power -= Player.instance.Item45_1;
                    }
                    Player.instance.Item45 = false;
                    Player.instance.Item45_1 = 0;
                    Player.instance.Item45_2 = 0;
                    break;
                case 46:    //적을 처치하면 1~10사이의 숫자를 1장 추가로 뽑는다.
                    Player.instance.Item46 = 0;   //시스템 수치 = 추가로 뽑는 개수
                    break;
                case 47:    //7번째 사용하는 조합카드가 공격카드이면 피해량이 2배가 된다. 공격 후 피해량은 원래대로 돌아온다.
                    Player.instance.Item47 = 0;   //파워 수치 = 피해량 개수
                    Player.instance.Item47_ = 0;   //시스템 수치 = n번째 사용
                    break;
                case 50:    //10턴마다 HP를 3 회복한다. 턴 수는 전투 시작시 초기화된다.
                    Player.instance.Item50 = 0;   //버프 수치 = 회복 수치
                    Player.instance.Item50_ = 0;   //시스템 수치 = 턴 수치
                    break;
                case 51: //전투 중 홀수 턴 피해량 3증가, 짝수 턴 쉴드 10획득
                    Player.instance.Item51_power = 0;
                    Player.instance.Item51_shield = 0;
                    break;
                case 52: //공격 시 10%확률로 피해량이 2배가 된다.
                    Player.instance.Item52 = false;
                    break;
                case 53: //5회 피격 시 공격한 적에게 마비를 2턴 부여한다.
                    break;
                case 54: //5회 피격 시 공격한 적에게 독를 2턴 부여한다.
                    break;
                case 55: //5회 피격 시 공격한 적에게 화상을 2턴 부여한다.
                    break;
                case 56: //5회 피격 시 공격한 적에게 출혈을 2턴 부여한다.
                    break;
                case 58: //전투 지역 진입 시 체력을 3 회복한다.
                    break;
                case 60: //전투 지역 진입 시 적 모두의 최대 체력을 5%감소된 상태로 전투한다.
                    break;
                case 61: //전투 지역 진입 시 적 중 하나의 체력을 10%감소된 상태로 전투한다.
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
                //3번째 조합카드 공격마다 데미지 1.5배
                Player.instance.Item10 = true;
                break;
            case 11:                
                Player.instance.returndamage -= Collection.instance.ItemDB.Entities[itemNum].system;

                if (Player.instance.returndamage == 0 && Player.instance.returnpercentdamage == 0 && Player.instance.returndamage_buff == 0)   //반사 관련 아이템 그리고 버프가 없다면
                {
                    Player.instance.boolreturnDamage = false;   //반사 해제
                }
                break;
            case 12:                
                Player.instance.returnpercentdamage -= Collection.instance.ItemDB.Entities[itemNum].system;

                if (Player.instance.returndamage == 0 && Player.instance.returnpercentdamage == 0 && Player.instance.returndamage_buff == 0)   //반사 관련 아이템 그리고 버프가 없다면
                {
                    Player.instance.boolreturnDamage = false;   //반사 해제
                }
                break;
            case 14:
                Player.instance.minimumGold += 10;
                break;
            case 25: //미지 지역 진입 시 체력을 5 회복한다.
                break;
            case 26: //미지 지역에서 전투가 발생하지 않는다.
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
            case 48:    //아이템 획득 시 50골드를 획득한다.
                Player.instance.Item48 = false;
                Player.instance.Item48_ = 0;
                break;
            case 49:    //아이템을 교체 한 횟수만큼 공격력이 증가한다. 최대 3
                Player.instance.Item49 = false;
                if (GameManager.instance.ItemChangeCount < 3)
                {
                    Player.instance.power -= GameManager.instance.ItemChangeCount;
                }
                else
                {
                    Player.instance.power -= Collection.instance.ItemDB.Entities[itemNum].system; //시스템 수치 = 최대 증가량
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
                case "마비":
                    Enemies[rand].GetComponent<EnemyBase>().EnemyStateAdd("마비", turn);
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
                case 60: //전투 지역 진입 시 적 모두의 최대 체력을 5%감소된 상태로 전투한다.
                    for(int num = 0; num < Enemies.Length; num++)
                    {
                        EnemyBase enemy = Enemies[num].GetComponent<EnemyBase>();
                        int enemyNewHp = (int)enemy.EnemyHp - (int)(enemy.EnemyMaxHp * 0.05f);
                        enemy.EnemyHp = enemyNewHp;
                        enemy.EnemyHpBarShow();
                    }
                    break;
                case 61: //전투 지역 진입 시 적 중 하나의 체력을 10%감소된 상태로 전투한다.
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
