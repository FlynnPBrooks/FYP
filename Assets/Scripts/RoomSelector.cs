using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSelector : MonoBehaviour
{
	public Sprite 	RoomT, RoomB, RoomR, RoomL,
			RoomTB, RoomRL, RoomTR, RoomTL, RoomBR, RoomBL,
			RoomTLB, RoomRTL, RoomBRT, RoomLBR, RoomTBRL;

	public bool Up, Down, Left, Right;

	public int type;

	public Color NormalColor, StartColor, ExitColor;

	Color MainColor;

	SpriteRenderer rend;

	void Start ()
	{
		rend = GetComponent<SpriteRenderer>();
		MainColor = NormalColor;
		PickSprite();
		PickColor();
	}


	void PickSprite()
	{
		if (Up)
		{
			if (Down)
			{
				if (Right)
				{
					if (Left)
					{
						rend.sprite = RoomTBRL;
					}else{
						rend.sprite = RoomBRT;
					}
				}else if (Left)
				{
					rend.sprite = RoomTLB;
				}else{
					rend.sprite = RoomTB;
				}
			}else{
				if (Right)
				{
					if (Left)
					{
						rend.sprite = RoomRTL;
					}else{
						rend.sprite = RoomTR;
					}
				}else if (Left)
				{
					rend.sprite = RoomTL;
				}else{
					rend.sprite = RoomT;
				}
			}
			return;
		}
		if (Down)
		{
			if (Right)
			{
				if(Left)
				{
					rend.sprite = RoomLBR;
				}else{
					rend.sprite = RoomBR;
				}
			}else if (Left)
			{
				rend.sprite = RoomBL;
			}else{
				rend.sprite = RoomB;
			}
			return;
		}
		if (Right)
		{
			if (Left)
			{
				rend.sprite = RoomRL;
			}else{
				rend.sprite = RoomR;
			}
		}else{
			rend.sprite = RoomL;
		}
	}


	void PickColor()
	{
		if (type == 0)
		{
			MainColor = NormalColor;
		}
		else if (type == 1)
		{
			MainColor = StartColor;
		}
		else if (type == 2)
		{
			MainColor = ExitColor;
		}
		rend.color = MainColor;
	}
}