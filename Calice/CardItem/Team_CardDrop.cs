using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDrop : dropcardrule, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
	private Image image;
	private RectTransform rect;

	public int rulenumber = 3;
	public int[] rulenumbers = new int[2];
	public List<int> NotEqualrulenumbers = new List<int>();
	public List<int> Equalrulenumbers = new List<int>();


	[SerializeField]
	public new rule rule;

	public int accumulatenum;
	[SerializeField]
	Text text;

	public bool rulecheck = false;

	public int joker = 0;
	public int check = 0;
	public combinationskill combin;
	public int sum = 0;

	public List<int> UseCardList = new List<int>();

	private void Awake()
	{
		image = GetComponent<Image>();
		rect = GetComponent<RectTransform>();
		combin = transform.parent.GetComponent<combinationskill>();
	}

	private void Start()
	{

		slottext();

	}

	private void Update()
	{
		if (this.transform.childCount < 2)
		{
			rulecheck = false;
		}
		/*
		else if (this.transform.childCount == 2)
		{
            if (this.transform.GetChild(1).GetComponent<Card>().shape == Cardinfo.shape.JOKER)
            {

            }
			else if (this.transform.GetChild(1).GetComponent<Card>().number < (Cardinfo.number)rulenumber)
			{
				GameObject mycardpanel = GameObject.Find("mycardPanel");
				this.transform.GetChild(1).transform.SetParent(mycardpanel.transform);
				//mycardpanel.GetComponent<mycard>().align();
				Debug.Log("혹시이거");



			}


		}
		else if (this.transform.childCount == 3)
		{
			if (this.transform.GetChild(2).GetComponent<Card>().number < (Cardinfo.number)rulenumber)
			{
				GameObject mycardpanel = GameObject.Find("gamepanel");
				this.transform.GetChild(2).transform.SetParent(mycardpanel.transform);
				mycardpanel.GetComponent<mycard>().align();

			}
		}
		*/
	}
	public void OnPointerEnter(PointerEventData eventData)
	{

		//image.color = Color.grey;
	}

	public void OnPointerExit(PointerEventData eventData)
	{

		image.color = Color.white;
	}

	public void OnDrop(PointerEventData eventData)
	{//내가 클릭을 뗀 곳
		DebugX.Log("onDrop" + this.gameObject.name);
		GameObject mycardpanel = GameObject.Find("mycardPanel");
		if (eventData.pointerDrag != null)
		{

			if (Cardrule(eventData))
			{

				eventData.pointerDrag.transform.SetParent(transform);
				eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;


				mycardpanel.GetComponent<mycard>().myhandcard.Remove(this.transform.GetChild(1).gameObject); //내 덱의 list에서 빠지기

			}
			else
			{
				mycardpanel.GetComponent<mycard>().myhandcard.Add(eventData.pointerDrag.transform.gameObject);

			}

		}
		else
		{
			DebugX.Log("카드없들때");
		}

		if (this.transform.childCount > 2)
		{
			if (Cardrule(eventData))
			{
				DebugX.Log("위");
				//조합카드 그안에서 바꿀 때
				Transform changeCard = transform.GetChild(1);  //내가 잡고 움직인 카드
				Transform precard = transform.GetChild(2).GetComponent<Card>().previousRect; //원래있던 카드의 이전 장소



				changeCard.SetParent(precard);
				changeCard.localPosition = Vector3.zero;

				if (precard.name != "mycardPanel")
				{
					if (precard.GetComponent<CardDrop>().rule == rule.equal)
					{
						precard.GetComponent<CardDrop>().rulecheck = true;
					}
				}

				else
				{
					DebugX.Log(changeCard.name);

					changeCard.parent.gameObject.GetComponent<mycard>().myhandcard.Add(changeCard.gameObject);
					changeCard.GetComponent<Card>().siblingindex = mycardpanel.transform.childCount;
					mycardpanel.GetComponent<mycard>().myhandcard.Remove(this.transform.GetChild(1).gameObject);
				}

			}
			else
			{
				DebugX.Log("아래");
				Transform changeCard = transform.GetChild(0);
				Transform precard = transform.GetChild(1).GetComponent<Card>().previousRect;


				changeCard.SetParent(precard);
				changeCard.localPosition = Vector3.zero;
				if (changeCard.parent.name == "mycardPanel")
				{
					DebugX.Log("바꾸기");
					changeCard.parent.gameObject.GetComponent<mycard>().myhandcard.Add(changeCard.gameObject);
					mycardpanel.GetComponent<mycard>().myhandcard.Remove(this.transform.GetChild(1).gameObject);
				}
			}

		}
		/*
		if (Cardrule())
        {
			this.transform.GetChild(1).SetParent(mycardpanel.transform);
			Debug.Log("ok)");

		}
		else
        {
			this.transform.GetChild(2).SetParent(mycardpanel.transform);
			Debug.Log("no");
		}
		*/
		//카드슬롯 부모의 쪽으로 가서 조건이 충족 되어 데미지를 넣을 수 있는지에 대해 확인



		mycardpanel.GetComponent<mycard>().align();//정렬


		/*
		if (eventData.pointerDrag.transform.parent.childCount > 1)
		{
			Transform changeCard = eventData.pointerDrag.transform.parent.GetChild(0);

			Debug.Log("child(0) : " + changeCard.name);
		}
		*/
		this.gameObject.transform.parent.gameObject.GetComponent<combinationskill>().damage();
	}

	public bool Cardrule()
	{

		if (this.transform.GetChild(1).GetComponent<Card>().number >= (Cardinfo.number)rulenumber)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	public bool Cardrule(PointerEventData eventData)
	{
		if (this.gameObject.transform.parent.GetComponent<combinationskill>().damagerule >= 950 && GameObject.Find("mycardPanel").GetComponent<mycard>().myhandcard.Count > 18)
		{
			rulecheck = false;
			return false;
		}
		switch ((rule))
		{
			case rule.norule_up:
				if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
				{
					if (eventData.pointerDrag.transform.GetComponent<Card>().number != (Cardinfo.number)10)
					{
						eventData.pointerDrag.transform.GetComponent<Card>().number = (Cardinfo.number)10;
						joker += 10;
						DebugX.Log(eventData.pointerDrag.transform.GetComponent<Card>().number.ToString());
					}
					rulecheck = true;
					return true;

				}
				else
				{
					joker = 0;
				}
				break;
			case rule.norule_down:
				if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
				{
					if (eventData.pointerDrag.transform.GetComponent<Card>().number != (Cardinfo.number)1)
					{
						eventData.pointerDrag.transform.GetComponent<Card>().number = (Cardinfo.number)1;
						joker += 10;
						DebugX.Log(eventData.pointerDrag.transform.GetComponent<Card>().number.ToString());
					}
					rulecheck = true;
					return true;

				}
				else
				{
					joker = 0;
				}
				break;
			case rule.up:
				if (eventData.pointerDrag.transform.GetComponent<Card>().number >= (Cardinfo.number)rulenumber)
				{
					if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
					{
						if (eventData.pointerDrag.transform.GetComponent<Card>().number != (Cardinfo.number)10)
						{
							eventData.pointerDrag.transform.GetComponent<Card>().number = (Cardinfo.number)10;
							DebugX.Log(eventData.pointerDrag.transform.GetComponent<Card>().number.ToString());
						}
					}
					rulecheck = true;
					return true;
				}
				else
				{
					if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
					{
						if (eventData.pointerDrag.transform.GetComponent<Card>().number != (Cardinfo.number)10)
						{
							eventData.pointerDrag.transform.GetComponent<Card>().number = (Cardinfo.number)10;
							DebugX.Log(eventData.pointerDrag.transform.GetComponent<Card>().number.ToString());
						}
						rulecheck = true;
						return true;

					}
					FindObjectOfType<SetVolume>().PlaySE("warning_sound");

					if (!rulecheck)
					{
						rulecheck = false;
					}

					return false;
				}
			case rule.down:
				if (eventData.pointerDrag.transform.GetComponent<Card>().number <= (Cardinfo.number)rulenumber)
				{
					rulecheck = true;
					return true;
				}
				else
				{
					if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
					{

						eventData.pointerDrag.transform.GetComponent<Card>().number = (Cardinfo.number)rulenumber;
						DebugX.Log(eventData.pointerDrag.transform.GetComponent<Card>().number.ToString());

						rulecheck = true;
						return true;

					}
					FindObjectOfType<SetVolume>().PlaySE("warning_sound");

					if (!rulecheck)
					{
						rulecheck = false;
					}

					return false;

				}
			case rule.only:
				if (eventData.pointerDrag.transform.GetComponent<Card>().number != (Cardinfo.number)rulenumber)
				{
					if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
					{
						rulecheck = true;
						return true;
					}
					FindObjectOfType<SetVolume>().PlaySE("warning_sound");

					if (!rulecheck)
					{
						rulecheck = false;
					}

					return false;
				}
				else
				{
					if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
					{
						rulecheck = true;
						return true;
					}
					rulecheck = true;
					return true;
				}
			case rule.between:
				if ((int)eventData.pointerDrag.transform.GetComponent<Card>().number >= rulenumbers[0] &&
					(int)eventData.pointerDrag.transform.GetComponent<Card>().number <= rulenumbers[1])
				{
					rulecheck = true;
					return true;
				}
				else
				{
					if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
					{
						eventData.pointerDrag.transform.GetComponent<Card>().number = (Cardinfo.number)rulenumbers[1];
						rulecheck = true;
						return true;
					}
					FindObjectOfType<SetVolume>().PlaySE("warning_sound");
					if (!rulecheck)
					{
						rulecheck = false;
					}
					return false;
				}

			case rule.accumulate:
				UseCardList.Add((int)eventData.pointerDrag.transform.GetComponent<Card>().number);

				if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
				{
					eventData.pointerDrag.transform.GetComponent<Card>().number = (Cardinfo.number)10;
				}
				accumulatenum -= (int)eventData.pointerDrag.transform.GetComponent<Card>().number;

				Destroy(eventData.pointerDrag.gameObject);//숫자카드삭제
				if (accumulatenum > 0)
				{
					rulecheck = false;
					//슬롯이 1개이기 때문에 rulecheck가 true되면 바로 조건이 성립되어서 나눠주어야함
				}
				else
				{

					rulecheck = true;
				}
				slottext();
				//카드숫자는 어찌되었든 들어가야하니까 무조건 true
				return true;

			case rule.equal:
				if (rulenumber == 0)    //룰 넘버가 없을때
				{
					if (EqualNumberCheck((int)eventData.pointerDrag.transform.GetComponent<Card>().number))      //같은 숫자라면
					{
						rulecheck = true;
						return true;
					}
					else    //조건이 안맞음 반환
					{
						FindObjectOfType<SetVolume>().PlaySE("warning_sound");
						if (!rulecheck)
						{
							rulecheck = false;
						}
						return false;
					}
				}
				else    //있을 때
				{
					if ((Cardinfo.number)rulenumber == eventData.pointerDrag.transform.GetComponent<Card>().number || eventData.pointerDrag.transform.GetComponent<Card>().shape == Cardinfo.shape.JOKER)
					{
						rulecheck = true;
						return true;
					}
					else
					{
						FindObjectOfType<SetVolume>().PlaySE("warning_sound");
						if (!rulecheck)
						{
							rulecheck = false;
						}
						return false;
					}
				}


			case rule.bothside:
				if ((int)eventData.pointerDrag.transform.GetComponent<Card>().number <= rulenumbers[0] ||
					(int)eventData.pointerDrag.transform.GetComponent<Card>().number >= rulenumbers[1])
				{
					rulecheck = true;
					return true;
				}
				else
				{
					if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
					{
						eventData.pointerDrag.transform.GetComponent<Card>().number = (Cardinfo.number)10;
						rulecheck = true;
						return true;
					}
					FindObjectOfType<SetVolume>().PlaySE("warning_sound");
					if (!rulecheck)
					{
						rulecheck = false;
					}
					return false;
				}
			case rule.plusonly:
				if ((int)eventData.pointerDrag.transform.GetComponent<Card>().number == rulenumbers[0] ||
					(int)eventData.pointerDrag.transform.GetComponent<Card>().number == rulenumbers[1])
				{
					rulecheck = true;
					return true;
				}
				else
				{
					if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
					{
						rulecheck = true;
						return true;
					}
					FindObjectOfType<SetVolume>().PlaySE("warning_sound");
					if (!rulecheck)
					{
						rulecheck = false;
					}
					return false;
				}

			case rule.odd: //홀수
				if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
				{
					eventData.pointerDrag.transform.GetComponent<Card>().number = (Cardinfo.number)9;
					rulecheck = true;
					return true;
				}
				else if ((int)eventData.pointerDrag.transform.GetComponent<Card>().number % 2 == 1)
				{
					rulecheck = true;
					return true;
				}
				else
				{
					FindObjectOfType<SetVolume>().PlaySE("warning_sound");
					if (!rulecheck)
					{
						rulecheck = false;
					}
					return false;
				}

			case rule.even:
				if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
				{
					eventData.pointerDrag.transform.GetComponent<Card>().number = (Cardinfo.number)10;
					rulecheck = true;
					return true;
				}
				else if ((int)eventData.pointerDrag.transform.GetComponent<Card>().number % 2 == 0)
				{
					rulecheck = true;
					return true;
				}
				else
				{
					FindObjectOfType<SetVolume>().PlaySE("warning_sound");
					if (!rulecheck)
					{
						rulecheck = false;
					}
					return false;
				}

			case rule.notequal:
				if (NotEqualNumberCheck((int)eventData.pointerDrag.transform.GetComponent<Card>().number))      //같은 숫자가 없다면
				{
					rulecheck = true;
					return true;
				}
				else    //조건이 안맞음 반환
				{
					FindObjectOfType<SetVolume>().PlaySE("warning_sound");
					if (rulecheck != true)
					{
						rulecheck = false;
					}
					return false;
				}
			case rule.plus:      //숫자카드의 합이 n이상 되도록 넣기 (여러슬롯)
				if (!rulecheck)     //빈 슬롯에 카드를 넣을 경우
				{
					check = 0;
					//마지막 슬롯인지 체크
					foreach (var slot in combin.cardslot)
					{
						if (slot.GetComponent<CardDrop>().rulecheck)
						{
							check++;
						}
					}
					DebugX.Log("마지막 슬롯 체크" + check);

					//넣는 카드가 마지막 슬롯일 때
					if (combin.cardslotcount - 1 == check)
					{
						DebugX.Log("마지막 슬롯");
						sum = 0;
						joker = 0;

						//계산
						foreach (var slot in combin.cardnum)
						{
							if (slot != 14)
							{
								sum += slot;
							}
							else
							{
								DebugX.Log("조커 있음");
								joker += 10;
							}
						}

						//마지막에 넣는 카드도 계산
						if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
						{
							joker += 10;
						}
						else
						{
							sum += (int)eventData.pointerDrag.transform.GetComponent<Card>().number;

						}


						if (sum >= accumulatenum - joker)
						{
							DebugX.Log("룰 성공" + sum);
						}
						else
						{
							DebugX.Log("룰 실패" + sum);
							FindObjectOfType<SetVolume>().PlaySE("warning_sound");
							rulecheck = false;
							return false;
						}

					}
				}
				rulecheck = true;
				return true;

			case rule.plusset:      //숫자카드의 합이 n이 되도록 넣기 (여러슬롯)

				if (!rulecheck)     //빈 슬롯에 카드를 넣을 경우
				{
					check = 0;
					//마지막 슬롯인지 체크
					foreach (var slot in combin.cardslot)
					{
						if (slot.GetComponent<CardDrop>().rulecheck)
						{
							check++;
						}
					}
					DebugX.Log("마지막 슬롯 체크" + check);

					//넣는 카드가 마지막 슬롯일 때
					if (combin.cardslotcount - 1 == check)
					{
						DebugX.Log("마지막 슬롯");
						sum = 0;
						joker = 0;

						//계산
						foreach (var slot in combin.cardnum)
						{
							if (slot != 14)
							{
								sum += slot;
							}
							else
							{
								DebugX.Log("조커 있음");
								joker += 10;
							}
						}

						//마지막에 넣는 카드도 계산
						if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
						{
							joker += 10;
						}
						else
						{
							sum += (int)eventData.pointerDrag.transform.GetComponent<Card>().number;

						}


						if (joker == 0)
						{   //조커가 없을 경우
							if (sum == accumulatenum)
							{
								DebugX.Log("룰 성공" + sum);
							}
							else
							{
								DebugX.Log("룰 실패" + sum);
								FindObjectOfType<SetVolume>().PlaySE("warning_sound");
								rulecheck = false;
								return false;
							}
						}
						else
						{   //조커가 있을경우
							if (sum < accumulatenum)    //합이 조건보다 크면 안됨. 조커가 마이너스가 될 순 없으니까
							{
								if (sum >= accumulatenum - joker)
								{
									DebugX.Log("룰 성공" + sum);
								}
								else
								{
									DebugX.Log("룰 실패" + sum);
									FindObjectOfType<SetVolume>().PlaySE("warning_sound");
									rulecheck = false;
									return false;
								}
							}
							else
							{
								DebugX.Log("룰 실패" + sum);
								FindObjectOfType<SetVolume>().PlaySE("warning_sound");
								rulecheck = false;
								return false;
							}
						}

					}
				}
				rulecheck = true;
				return true;
			case rule.minus_2:      //숫자카드의 차가 n이하 되도록 넣기 (2슬롯)
				if (!rulecheck)     //빈 슬롯에 카드를 넣을 경우
				{
					check = 0;

					//마지막 슬롯인지 체크
					foreach (var slot in combin.cardslot)
					{
						if (slot.GetComponent<CardDrop>().rulecheck)
						{
							check++;
						}
					}
					DebugX.Log("마지막 슬롯 체크" + check);

					//넣는 카드가 마지막 슬롯일 때
					if (combin.cardslotcount - 1 == check)
					{
						DebugX.Log("마지막 슬롯");
						joker = 0;
						sum = 0;

						//계산
						foreach (var slot in combin.cardnum)
						{
							if (slot != 14)
							{
								sum += slot;
							}
							else
							{
								DebugX.Log("조커 있음");
								joker += 10;
							}
						}

						//마지막에 넣는 카드도 계산
						if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
						{
							joker += 10;
						}
						else
						{
							sum -= (int)eventData.pointerDrag.transform.GetComponent<Card>().number;
							sum = math.abs(sum);
						}


						if (sum <= accumulatenum + joker)
						{
							DebugX.Log("룰 성공" + sum);
						}
						else
						{
							DebugX.Log("룰 실패" + sum);
							FindObjectOfType<SetVolume>().PlaySE("warning_sound");
							rulecheck = false;
							return false;
						}

					}
				}
				rulecheck = true;
				return true;

			case rule.minusset_2:      //숫자카드의 합이 n이 되도록 넣기 (2슬롯)
				check = 0;

				if (!rulecheck)     //빈 슬롯에 카드를 넣을 경우
				{
					//마지막 슬롯인지 체크
					foreach (var slot in combin.cardslot)
					{
						if (slot.GetComponent<CardDrop>().rulecheck)
						{
							check++;
						}
					}
					DebugX.Log("마지막 슬롯 체크" + check);

					//넣는 카드가 마지막 슬롯일 때
					if (combin.cardslotcount - 1 == check)
					{
						DebugX.Log("마지막 슬롯");
						sum = 0;
						joker = 0;

						//계산
						foreach (var slot in combin.cardnum)
						{
							if (slot != 14)
							{
								sum += slot;
							}
							else
							{
								DebugX.Log("조커 있음");
								joker += 10;
							}
						}

						//마지막에 넣는 카드도 계산
						if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
						{
							joker += 10;
						}
						else
						{
							sum -= (int)eventData.pointerDrag.transform.GetComponent<Card>().number;
							sum = math.abs(sum);
						}


						if (joker == 0)
						{   //조커가 없을 경우
							if (sum == accumulatenum)
							{
								DebugX.Log("룰 성공" + sum);
							}
							else
							{
								DebugX.Log("룰 실패" + sum);
								FindObjectOfType<SetVolume>().PlaySE("warning_sound");
								rulecheck = false;
								return false;
							}
						}
						else
						{   //조커가 있을경우

							if (sum < accumulatenum + joker)
							{
								DebugX.Log("룰 성공" + sum);
							}
							else
							{
								DebugX.Log("룰 실패" + sum);
								FindObjectOfType<SetVolume>().PlaySE("warning_sound");
								rulecheck = false;
								return false;
							}

						}

					}
				}
				rulecheck = true;
				return true;

			case rule.downodd: //이하 홀수
				if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)     //조커일 경우
				{
					eventData.pointerDrag.transform.GetComponent<Card>().number = (Cardinfo.number)rulenumber;
					rulecheck = true;
					return true;
				}
				else if (eventData.pointerDrag.transform.GetComponent<Card>().number <= (Cardinfo.number)rulenumber)
				{


					if ((int)eventData.pointerDrag.transform.GetComponent<Card>().number % 2 == 1)
					{
						rulecheck = true;
						return true;
					}
					else
					{
						FindObjectOfType<SetVolume>().PlaySE("warning_sound");
						rulecheck = false;
						return false;
					}
				}
				else
				{
					FindObjectOfType<SetVolume>().PlaySE("warning_sound");
					rulecheck = false;
					return false;
				}


			case rule.downeven: //이하 짝수
				if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
				{
					eventData.pointerDrag.transform.GetComponent<Card>().number = (Cardinfo.number)rulenumber;
					rulecheck = true;
					return true;
				}
				else if (eventData.pointerDrag.transform.GetComponent<Card>().number <= (Cardinfo.number)rulenumber)
				{

					if ((int)eventData.pointerDrag.transform.GetComponent<Card>().number % 2 == 0)
					{
						rulecheck = true;
						return true;
					}
					else
					{
						FindObjectOfType<SetVolume>().PlaySE("warning_sound");
						rulecheck = false;
						return false;
					}
				}
				else
				{
					FindObjectOfType<SetVolume>().PlaySE("warning_sound");
					rulecheck = false;
					return false;
				}
			case rule.upodd: //이상 홀수
				if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)     //조커일 경우
				{
					eventData.pointerDrag.transform.GetComponent<Card>().number = (Cardinfo.number)rulenumber;
					rulecheck = true;
					return true;
				}
				else if (eventData.pointerDrag.transform.GetComponent<Card>().number >= (Cardinfo.number)rulenumber)
				{
					if ((int)eventData.pointerDrag.transform.GetComponent<Card>().number % 2 == 1)
					{
						rulecheck = true;
						return true;
					}
					else
					{
						FindObjectOfType<SetVolume>().PlaySE("warning_sound");
						rulecheck = false;
						return false;
					}
				}
				else
				{
					FindObjectOfType<SetVolume>().PlaySE("warning_sound");
					rulecheck = false;
					return false;
				}


			case rule.upeven:   //이상 짝수
				if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
				{
					eventData.pointerDrag.transform.GetComponent<Card>().number = (Cardinfo.number)rulenumber;
					rulecheck = true;
					return true;
				}
				else if (eventData.pointerDrag.transform.GetComponent<Card>().number >= (Cardinfo.number)rulenumber)
				{

					if ((int)eventData.pointerDrag.transform.GetComponent<Card>().number % 2 == 0)
					{
						rulecheck = true;
						return true;
					}
					else
					{
						FindObjectOfType<SetVolume>().PlaySE("warning_sound");
						rulecheck = false;
						return false;
					}
				}
				else
				{
					FindObjectOfType<SetVolume>().PlaySE("warning_sound");
					rulecheck = false;
					return false;
				}
			case rule.down_one:
				if (eventData.pointerDrag.transform.GetComponent<Card>().number <= (Cardinfo.number)rulenumber)
				{
					rulecheck = true;
					return true;
				}
				else
				{
					if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
					{

						eventData.pointerDrag.transform.GetComponent<Card>().number = Cardinfo.number.one;
						DebugX.Log(eventData.pointerDrag.transform.GetComponent<Card>().number.ToString());

						rulecheck = true;
						return true;

					}
					FindObjectOfType<SetVolume>().PlaySE("warning_sound");

					if (!rulecheck)
					{
						rulecheck = false;
					}

					return false;

				}
			case rule.norule_not:
				DebugX.Log(eventData.pointerDrag.transform.GetComponent<Card>().number.ToString());
				rulecheck = true;
				return true;
		}
		//어차피 위에서 다걸림
		//norule만 여기서 트루나옴
		rulecheck = true;
		return true;
	}

	public bool NotEqualNumberCheck(int num)
	{
		NotEqualNumberRefresh();

		if (NotEqualrulenumbers.Count == 0) //금지된 수 없을 때
		{
			return true;
		}

		for (int i = 0; i < NotEqualrulenumbers.Count; i++)
		{
			//룰에 걸린다면
			if (NotEqualrulenumbers[i] == num)
			{
				DebugX.Log("같은숫자 입니다.");
				return false;
			}
		}
		DebugX.Log("같은숫자가 아닙니다.");
		return true;
	}

	public void NotEqualNumberRefresh()
	{
		NotEqualrulenumbers.Clear();

		foreach (var Slot in combin.cardnum)
		{
			if (Slot != 0)
			{
				NotEqualrulenumbers.Add(Slot);
			}
		}
	}
	public bool EqualNumberCheck(int num)
	{
		EqualNumberRefresh();

		if (Equalrulenumbers.Count == 0)
		{
			return true;
		}
		else if (Equalrulenumbers.Count == 1)   //슬롯을 하나 썼을때
		{
			if (Equalrulenumbers[0] == 14)  //조커일 경우
			{
				return true;
			}
		}
		else if (Equalrulenumbers.Count == 2)   //슬롯 두개를 썼을때
		{
			if (Equalrulenumbers[0] == 14 && Equalrulenumbers[1] == 14)  //둘 다 조커일 경우
			{
				return true;
			}
		}

		for (int i = 0; i < Equalrulenumbers.Count; i++)
		{
			//룰에 걸린다면
			if (Equalrulenumbers[i] == num || num == 14)
			{
				DebugX.Log("같은숫자 입니다.");
				return true;
			}
		}
		DebugX.Log("같은숫자가 아닙니다.");
		return false;

	}

	public void EqualNumberRefresh()
	{
		Equalrulenumbers.Clear();

		foreach (var Slot in this.transform.parent.gameObject.GetComponent<combinationskill>().cardnum)
		{
			if (Slot != 0)
			{
				Equalrulenumbers.Add(Slot);
			}
		}
	}

	public void slottext()
	{
		if (Collection.instance.textLanguage == 0)
		{
			switch (this.rule)
			{
				case rule.norule_up:
					break;
				case rule.up:
					text.text = rulenumber.ToString() + "이상\n" + "숫자";
					break;
				case rule.down:
					text.text = rulenumber.ToString() + "이하\n" + "숫자";
					break;
				case rule.only:
					if (rulenumber == 0)
						text.text = "조커";
					else
						text.text = "오직 숫자" + rulenumber.ToString();

					break;
				case rule.between:
					text.text = rulenumbers[0] + "이상\n" + rulenumbers[1] + "이하 숫자";
					break;
				case rule.accumulate:
					text.text = "남은 숫자\n" + accumulatenum.ToString();
					break;
				case rule.equal:
					break;
				case rule.bothside:
					text.text = rulenumbers[0] + "이하\n" + rulenumbers[1] + "이상 숫자";
					break;
				case rule.plusonly:
					text.text = "숫자 " + rulenumbers[0] + " 또는 숫자 " + rulenumbers[1];
					break;
				case rule.odd:
					if (rulenumber != 0)
						text.text = rulenumber + "\n홀수";
					else
						text.text = "홀수";
					break;
				case rule.even:
					text.text = "짝수";
					break;
				case rule.downodd:
					text.text = rulenumber + "이하\n홀수";
					break;
				case rule.downeven:
					text.text = rulenumber + "이하\n짝수";
					break;
				case rule.upodd:
					text.text = rulenumber + "이상\n홀수";
					break;
				case rule.upeven:
					text.text = rulenumber + "이상\n짝수";
					break;
				case rule.down_one:
					text.text = rulenumber.ToString() + "이하\n" + "숫자";
					break;
			}
		}
		else if (Collection.instance.textLanguage == 1)
		{
			switch (this.rule)
			{
				case rule.norule_up:
					break;
				case rule.up:
					text.text = rulenumber.ToString() + "\nor More\n" + "Number";
					break;
				case rule.down:
					text.text = rulenumber.ToString() + "\nor Less\n" + "Number";
					break;
				case rule.only:
					if (rulenumber == 0)
						text.text = "JOKER";
					else
						text.text = "Only Number\n" + rulenumber.ToString();

					break;
				case rule.between:
					text.text = rulenumbers[0] + "\nor More AND\n" + rulenumbers[1] + "\nor Less\nNumber";
					break;
				case rule.accumulate:
					text.text = "Remain Number\n" + accumulatenum.ToString();
					break;
				case rule.equal:
					break;
				case rule.bothside:
					text.text = rulenumbers[0] + "\nor Less\n" + rulenumbers[1] + "\nor More \nNumber";
					break;
				case rule.plusonly:
					text.text = "Number " + rulenumbers[0] + "\nOR\nNumber" + rulenumbers[1];
					break;
				case rule.odd:
					if (rulenumber != 0)
						text.text = rulenumber + "\nOdd Number";
					else
						text.text = "Odd Number";
					break;
				case rule.even:
					text.text = "Even Number";
					break;
				case rule.downodd:
					text.text = rulenumber + "\nor Less\nOdd Number";
					break;
				case rule.downeven:
					text.text = rulenumber + "\nor Less\nEven Number";
					break;
				case rule.upodd:
					text.text = rulenumber + "\nor More\nOdd Number";
					break;
				case rule.upeven:
					text.text = rulenumber + "\nor More\nEven Number";
					break;
				case rule.down_one:
					text.text = rulenumber.ToString() + "\nor Less\n" + "Number";
					break;
			}
		}
		else if (Collection.instance.textLanguage == 2)
		{
			switch (this.rule)
			{
				case rule.norule_up:
					break;
				case rule.up:
					text.text = rulenumber.ToString() + "\nor More\n" + "Number";
					break;
				case rule.down:
					text.text = rulenumber.ToString() + "\nor Less\n" + "Number";
					break;
				case rule.only:
					if (rulenumber == 0)
						text.text = "JOKER";
					else
						text.text = "Only Number\n" + rulenumber.ToString();

					break;
				case rule.between:
					text.text = rulenumbers[0] + "\nor More AND\n" + rulenumbers[1] + "\nor Less\nNumber";
					break;
				case rule.accumulate:
					text.text = "Remain Number\n" + accumulatenum.ToString();
					break;
				case rule.equal:
					break;
				case rule.bothside:
					text.text = rulenumbers[0] + "\nor Less\n" + rulenumbers[1] + "\nor More \nNumber";
					break;
				case rule.plusonly:
					text.text = "Number " + rulenumbers[0] + "\nOR\nNumber" + rulenumbers[1];
					break;
				case rule.odd:
					if (rulenumber != 0)
						text.text = rulenumber + "\nOdd Number";
					else
						text.text = "Odd Number";
					break;
				case rule.even:
					text.text = "Even Number";
					break;
				case rule.downodd:
					text.text = rulenumber + "\nor Less\nOdd Number";
					break;
				case rule.downeven:
					text.text = rulenumber + "\nor Less\nEven Number";
					break;
				case rule.upodd:
					text.text = rulenumber + "\nor More\nOdd Number";
					break;
				case rule.upeven:
					text.text = rulenumber + "\nor More\nEven Number";
					break;
				case rule.down_one:
					text.text = rulenumber.ToString() + "\nor Less\n" + "Number";
					break;
			}
		}
	}

}

