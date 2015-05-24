using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public class Cell : MonoBehaviour 
{
	public int id;
	public RectTransform rTrasfrom;
	public Text caption;
	public List<RectTransform> linesTransform;
	public List<Cell> linkingCells;
	public List<Cell> connectedCells;
	public bool visible;
	public EventTrigger trigger;
	public GraphMaster master;

	public void RefreshLines()
	{
		for (int i = 0; i < linkingCells.Count; i++) 
		{
			Vector3 differenceVector = linkingCells[i].rTrasfrom.position - rTrasfrom.position;
			
			linesTransform[i].sizeDelta = new Vector2( differenceVector.magnitude, 1);
			linesTransform[i].pivot = new Vector2(0, 0.5f);
			linesTransform[i].position = rTrasfrom.position + new Vector3(0,0,-20);
			float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
			linesTransform[i].rotation = Quaternion.Euler(0,0, angle);

		}

		foreach (Cell item in connectedCells) 
		{
			item.RefreshLines();	
		}
		
	}

	public void Show()
	{
		if (visible == false) {
			visible = true;
			rTrasfrom.localScale = new Vector3 (0.3F, 0.3F, 0.3F);
			gameObject.SetActive (true);
			foreach (Cell item in connectedCells) {
				item.Show ();	
			}

			foreach (RectTransform item in linesTransform) {
				item.gameObject.SetActive (true);
			}
		}
	}

	public void Hide()
	{
		foreach (RectTransform item in linesTransform) 
		{
			item.gameObject.SetActive(false);
		}
		foreach (Cell item in connectedCells) 
		{
			item.Hide();	
		}

		gameObject.SetActive (false);
		visible = false;
	}

	public void Minimize()
	{
		visible = false;
		foreach (Cell item in connectedCells) 
		{
			item.Hide();	
		}
		rTrasfrom.localScale = new Vector3 (0.05F, 0.05F, 0.05F);
	}

	public void OnPointer()
	{
		master.NewSelectedCell (this);
	}

}
