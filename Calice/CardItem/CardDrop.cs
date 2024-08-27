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
				Debug.Log("Ȥ���̰�");



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
	{//���� Ŭ���� �� ��
		DebugX.Log("onDrop" + this.gameObject.name);
		GameObject mycardpanel = GameObject.Find("mycardPanel");
		if (eventData.pointerDrag != null)
		{

			if (Cardrule(eventData))
			{

				eventData.pointerDrag.transform.SetParent(transform);
				eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;


				mycardpanel.GetComponent<mycard>().myhandcard.Remove(this.transform.GetChild(1).gameObject); //�� ���� list���� ������

			}
			else
			{
				mycardpanel.GetComponent<mycard>().myhandcard.Add(eventData.pointerDrag.transform.gameObject);

			}

		}
		else
		{
			DebugX.Log("ī����鶧");
		}

		if (this.transform.childCount > 2)
		{
			if (Cardrule(eventData))
			{
				DebugX.Log("��");
				//����ī�� �׾ȿ��� �ٲ� ��
				Transform changeCard = transform.GetChild(1);  //���� ��� ������ ī��
				Transform precard = transform.GetChild(2).GetComponent<Card>().previousRect; //�����ִ� ī���� ���� ���



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
				DebugX.Log("�Ʒ�");
				Transform changeCard = transform.GetChild(0);
				Transform precard = transform.GetChild(1).GetComponent<Card>().previousRect;


				changeCard.SetParent(precard);
				changeCard.localPosition = Vector3.zero;
				if (changeCard.parent.name == "mycardPanel")
				{
					DebugX.Log("�ٲٱ�");
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
		//ī�彽�� �θ��� ������ ���� ������ ���� �Ǿ� �������� ���� �� �ִ����� ���� Ȯ��



		mycardpanel.GetComponent<mycard>().align();//����


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

				Destroy(eventData.pointerDrag.gameObject);//����ī�����
				if (accumulatenum > 0)
				{
					rulecheck = false;
					//������ 1���̱� ������ rulecheck�� true�Ǹ� �ٷ� ������ �����Ǿ �����־����
				}
				else
				{

					rulecheck = true;
				}
				slottext();
				//ī����ڴ� ����Ǿ��� �����ϴϱ� ������ true
				return true;

			case rule.equal:
				if (rulenumber == 0)    //�� �ѹ��� ������
				{
					if (EqualNumberCheck((int)eventData.pointerDrag.transform.GetComponent<Card>().number))      //���� ���ڶ��
					{
						rulecheck = true;
						return true;
					}
					else    //������ �ȸ��� ��ȯ
					{
						FindObjectOfType<SetVolume>().PlaySE("warning_sound");
						if (!rulecheck)
						{
							rulecheck = false;
						}
						return false;
					}
				}
				else    //���� ��
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

			case rule.odd: //Ȧ��
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
				if (NotEqualNumberCheck((int)eventData.pointerDrag.transform.GetComponent<Card>().number))      //���� ���ڰ� ���ٸ�
				{
					rulecheck = true;
					return true;
				}
				else    //������ �ȸ��� ��ȯ
				{
					FindObjectOfType<SetVolume>().PlaySE("warning_sound");
					if (rulecheck != true)
					{
						rulecheck = false;
					}
					return false;
				}
			case rule.plus:      //����ī���� ���� n�̻� �ǵ��� �ֱ� (��������)
				if (!rulecheck)     //�� ���Կ� ī�带 ���� ���
				{
					check = 0;
					//������ �������� üũ
					foreach (var slot in combin.cardslot)
					{
						if (slot.GetComponent<CardDrop>().rulecheck)
						{
							check++;
						}
					}
					DebugX.Log("������ ���� üũ" + check);

					//�ִ� ī�尡 ������ ������ ��
					if (combin.cardslotcount - 1 == check)
					{
						DebugX.Log("������ ����");
						sum = 0;
						joker = 0;

						//���
						foreach (var slot in combin.cardnum)
						{
							if (slot != 14)
							{
								sum += slot;
							}
							else
							{
								DebugX.Log("��Ŀ ����");
								joker += 10;
							}
						}

						//�������� �ִ� ī�嵵 ���
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
							DebugX.Log("�� ����" + sum);
						}
						else
						{
							DebugX.Log("�� ����" + sum);
							FindObjectOfType<SetVolume>().PlaySE("warning_sound");
							rulecheck = false;
							return false;
						}

					}
				}
				rulecheck = true;
				return true;

			case rule.plusset:      //����ī���� ���� n�� �ǵ��� �ֱ� (��������)

				if (!rulecheck)     //�� ���Կ� ī�带 ���� ���
				{
					check = 0;
					//������ �������� üũ
					foreach (var slot in combin.cardslot)
					{
						if (slot.GetComponent<CardDrop>().rulecheck)
						{
							check++;
						}
					}
					DebugX.Log("������ ���� üũ" + check);

					//�ִ� ī�尡 ������ ������ ��
					if (combin.cardslotcount - 1 == check)
					{
						DebugX.Log("������ ����");
						sum = 0;
						joker = 0;

						//���
						foreach (var slot in combin.cardnum)
						{
							if (slot != 14)
							{
								sum += slot;
							}
							else
							{
								DebugX.Log("��Ŀ ����");
								joker += 10;
							}
						}

						//�������� �ִ� ī�嵵 ���
						if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)
						{
							joker += 10;
						}
						else
						{
							sum += (int)eventData.pointerDrag.transform.GetComponent<Card>().number;

						}


						if (joker == 0)
						{   //��Ŀ�� ���� ���
							if (sum == accumulatenum)
							{
								DebugX.Log("�� ����" + sum);
							}
							else
							{
								DebugX.Log("�� ����" + sum);
								FindObjectOfType<SetVolume>().PlaySE("warning_sound");
								rulecheck = false;
								return false;
							}
						}
						else
						{   //��Ŀ�� �������
							if (sum < accumulatenum)    //���� ���Ǻ��� ũ�� �ȵ�. ��Ŀ�� ���̳ʽ��� �� �� �����ϱ�
							{
								if (sum >= accumulatenum - joker)
								{
									DebugX.Log("�� ����" + sum);
								}
								else
								{
									DebugX.Log("�� ����" + sum);
									FindObjectOfType<SetVolume>().PlaySE("warning_sound");
									rulecheck = false;
									return false;
								}
							}
							else
							{
								DebugX.Log("�� ����" + sum);
								FindObjectOfType<SetVolume>().PlaySE("warning_sound");
								rulecheck = false;
								return false;
							}
						}

					}
				}
				rulecheck = true;
				return true;
			case rule.minus_2:      //����ī���� ���� n���� �ǵ��� �ֱ� (2����)
				if (!rulecheck)     //�� ���Կ� ī�带 ���� ���
				{
					check = 0;

					//������ �������� üũ
					foreach (var slot in combin.cardslot)
					{
						if (slot.GetComponent<CardDrop>().rulecheck)
						{
							check++;
						}
					}
					DebugX.Log("������ ���� üũ" + check);

					//�ִ� ī�尡 ������ ������ ��
					if (combin.cardslotcount - 1 == check)
					{
						DebugX.Log("������ ����");
						joker = 0;
						sum = 0;

						//���
						foreach (var slot in combin.cardnum)
						{
							if (slot != 14)
							{
								sum += slot;
							}
							else
							{
								DebugX.Log("��Ŀ ����");
								joker += 10;
							}
						}

						//�������� �ִ� ī�嵵 ���
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
							DebugX.Log("�� ����" + sum);
						}
						else
						{
							DebugX.Log("�� ����" + sum);
							FindObjectOfType<SetVolume>().PlaySE("warning_sound");
							rulecheck = false;
							return false;
						}

					}
				}
				rulecheck = true;
				return true;

			case rule.minusset_2:      //����ī���� ���� n�� �ǵ��� �ֱ� (2����)
				check = 0;

				if (!rulecheck)     //�� ���Կ� ī�带 ���� ���
				{
					//������ �������� üũ
					foreach (var slot in combin.cardslot)
					{
						if (slot.GetComponent<CardDrop>().rulecheck)
						{
							check++;
						}
					}
					DebugX.Log("������ ���� üũ" + check);

					//�ִ� ī�尡 ������ ������ ��
					if (combin.cardslotcount - 1 == check)
					{
						DebugX.Log("������ ����");
						sum = 0;
						joker = 0;

						//���
						foreach (var slot in combin.cardnum)
						{
							if (slot != 14)
							{
								sum += slot;
							}
							else
							{
								DebugX.Log("��Ŀ ����");
								joker += 10;
							}
						}

						//�������� �ִ� ī�嵵 ���
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
						{   //��Ŀ�� ���� ���
							if (sum == accumulatenum)
							{
								DebugX.Log("�� ����" + sum);
							}
							else
							{
								DebugX.Log("�� ����" + sum);
								FindObjectOfType<SetVolume>().PlaySE("warning_sound");
								rulecheck = false;
								return false;
							}
						}
						else
						{   //��Ŀ�� �������

							if (sum < accumulatenum + joker)
							{
								DebugX.Log("�� ����" + sum);
							}
							else
							{
								DebugX.Log("�� ����" + sum);
								FindObjectOfType<SetVolume>().PlaySE("warning_sound");
								rulecheck = false;
								return false;
							}

						}

					}
				}
				rulecheck = true;
				return true;

			case rule.downodd: //���� Ȧ��
				if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)     //��Ŀ�� ���
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


			case rule.downeven: //���� ¦��
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
			case rule.upodd: //�̻� Ȧ��
				if (eventData.pointerDrag.transform.GetComponent<Card>().shape == (Cardinfo.shape)Cardinfo.shape.JOKER)     //��Ŀ�� ���
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


			case rule.upeven:   //�̻� ¦��
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
		//������ ������ �ٰɸ�
		//norule�� ���⼭ Ʈ�糪��
		rulecheck = true;
		return true;
	}

	public bool NotEqualNumberCheck(int num)
	{
		NotEqualNumberRefresh();

		if (NotEqualrulenumbers.Count == 0) //������ �� ���� ��
		{
			return true;
		}

		for (int i = 0; i < NotEqualrulenumbers.Count; i++)
		{
			//�꿡 �ɸ��ٸ�
			if (NotEqualrulenumbers[i] == num)
			{
				DebugX.Log("�������� �Դϴ�.");
				return false;
			}
		}
		DebugX.Log("�������ڰ� �ƴմϴ�.");
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
		else if (Equalrulenumbers.Count == 1)   //������ �ϳ� ������
		{
			if (Equalrulenumbers[0] == 14)  //��Ŀ�� ���
			{
				return true;
			}
		}
		else if (Equalrulenumbers.Count == 2)   //���� �ΰ��� ������
		{
			if (Equalrulenumbers[0] == 14 && Equalrulenumbers[1] == 14)  //�� �� ��Ŀ�� ���
			{
				return true;
			}
		}

		for (int i = 0; i < Equalrulenumbers.Count; i++)
		{
			//�꿡 �ɸ��ٸ�
			if (Equalrulenumbers[i] == num || num == 14)
			{
				DebugX.Log("�������� �Դϴ�.");
				return true;
			}
		}
		DebugX.Log("�������ڰ� �ƴմϴ�.");
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
					text.text = rulenumber.ToString() + "�̻�\n" + "����";
					break;
				case rule.down:
					text.text = rulenumber.ToString() + "����\n" + "����";
					break;
				case rule.only:
					if (rulenumber == 0)
						text.text = "��Ŀ";
					else
						text.text = "���� ����" + rulenumber.ToString();

					break;
				case rule.between:
					text.text = rulenumbers[0] + "�̻�\n" + rulenumbers[1] + "���� ����";
					break;
				case rule.accumulate:
					text.text = "���� ����\n" + accumulatenum.ToString();
					break;
				case rule.equal:
					break;
				case rule.bothside:
					text.text = rulenumbers[0] + "����\n" + rulenumbers[1] + "�̻� ����";
					break;
				case rule.plusonly:
					text.text = "���� " + rulenumbers[0] + " �Ǵ� ���� " + rulenumbers[1];
					break;
				case rule.odd:
					if (rulenumber != 0)
						text.text = rulenumber + "\nȦ��";
					else
						text.text = "Ȧ��";
					break;
				case rule.even:
					text.text = "¦��";
					break;
				case rule.downodd:
					text.text = rulenumber + "����\nȦ��";
					break;
				case rule.downeven:
					text.text = rulenumber + "����\n¦��";
					break;
				case rule.upodd:
					text.text = rulenumber + "�̻�\nȦ��";
					break;
				case rule.upeven:
					text.text = rulenumber + "�̻�\n¦��";
					break;
				case rule.down_one:
					text.text = rulenumber.ToString() + "����\n" + "����";
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

