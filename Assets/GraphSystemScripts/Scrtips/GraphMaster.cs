using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


using UnityEngine.EventSystems;

public class GraphMaster : MonoBehaviour 
{

	public List<Cell> cells;
	public Object cellPrefab;
	public Object linePrefab;

	public Transform cellsParent;
	public Transform linesParent;

	private Vector3 lastMousePosition;
	private Vector3 deltaMousePosition;

	public Cell selectedCell;
	public Cell dynamicCell;
	public Cell creatingLineCell;



	void Update () 
	{
		//Desktop controlling
	
		deltaMousePosition = Input.mousePosition - lastMousePosition;
		lastMousePosition = Input.mousePosition;

		if (Input.GetMouseButton (0))
		{
			GameObject selected = EventSystem.current.currentSelectedGameObject;
			if(selected!= null)
			{
				if(selected.tag == "CellUI")
					selected = selected.transform.parent.gameObject;

				if(selected.tag == "Cell")
				{
					Cell movedCell = selected.GetComponent<Cell>();		movedCell.RefreshLines();
					movedCell.transform.position += deltaMousePosition;
					movedCell.RefreshLines();
				}
			}

		}

		if (Input.GetMouseButtonDown (1)) 
		{
			creatingLineCell = selectedCell;
			CreateLine(creatingLineCell);
		}

		if (Input.GetMouseButton (1)) 
		{
			creatingLineCell.linkingCells[creatingLineCell.linkingCells.Count-1].rTrasfrom.position = Input.mousePosition;
			creatingLineCell.RefreshLines();
		}

		if (Input.GetMouseButtonUp (1)) 
		{
			if(selectedCell != creatingLineCell)
			{
				creatingLineCell.linkingCells[creatingLineCell.linkingCells.Count-1] = selectedCell;
				selectedCell.connectedCells.Add(creatingLineCell);
			}
			else
			{
				Destroy(creatingLineCell.linesTransform[creatingLineCell.linkingCells.Count-1].gameObject);
				creatingLineCell.linesTransform.RemoveAt(creatingLineCell.linkingCells.Count-1);
				creatingLineCell.linkingCells.RemoveAt(creatingLineCell.linkingCells.Count-1);
				creatingLineCell.RefreshLines();
			}
		}

		//to fix
		//transform.parent.localScale += new Vector3 (Input.GetAxis ("Mouse ScrollWheel"), Input.GetAxis ("Mouse ScrollWheel"), 0);


	}

	public void CreateNewCell()
	{
		Cell newCell = (Instantiate (cellPrefab, new Vector3 (Screen.width / 2, Screen.height / 2, 0), Quaternion.identity) as GameObject).GetComponent<Cell> ();
		newCell.transform.parent = cellsParent;
		newCell.master = this;
	}

	public void CreateLine(Cell parent)
	{
		GameObject newLine = Instantiate (linePrefab, Vector3.zero, Quaternion.identity) as GameObject;
		newLine.transform.parent = linesParent;
		parent.linesTransform.Add (newLine.GetComponent<RectTransform>());
		parent.linkingCells.Add(dynamicCell);
	}


	public void NewSelectedCell(Cell cell)
	{
		selectedCell = cell;
	}

}
