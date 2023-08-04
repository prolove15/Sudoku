using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public int num;
    public bool selected;
    public bool canEdit = true;
    public int andNum = 1;
    public bool[] hintNum;

    [Space(10)]
    public Image bg;
    public Image upperBg;
    public Text lbl;
    public GameObject hintParent;
    public Text[] hintLbl;
    public Button btn;
    public List<Block> boxList, rowList, colList;

    void Start()
    {
        hintNum = new bool[9];
        UpdateUI();
    }

    [ContextMenu("Refresh")]
    public void UpdateUI()
    {
        if (GameManager.IsGamePause)
        {
            upperBg.gameObject.SetActive(true);
            upperBg.color = ThemeManager.intance.CurrentTheme.boxNormalColor;
            return;
        }
        else
        {
            upperBg.gameObject.SetActive(false);
        }
        if (num == 0)
        {
            hintParent.SetActive(true);
            UpdateHint();
            lbl.text = "";
        }
        else
        {
            hintParent.SetActive(false);
            lbl.text = num.ToString();
            if (!canEdit)
                lbl.color = ThemeManager.intance.CurrentTheme.defultNumColor;
            else if (IsWoungInput() && GameManager.HighlightDuplicate)
                lbl.color = ThemeManager.intance.CurrentTheme.wroungNumColor;
            else
                lbl.color = ThemeManager.intance.CurrentTheme.rightNumColor;
        }


        if (IsWoungInput() && GameManager.HighlightDuplicate)
            bg.color = ThemeManager.intance.CurrentTheme.boxWithWorngNum;
        else if (selected)
        {
            bg.color = ThemeManager.intance.CurrentTheme.boxSelectedColor;
        }
        else if ((GameManager.HighlightIdenticalNumbers && num != 0 && SudokuManager.intance.CurrentSelected != null && SudokuManager.intance.CurrentSelected.num == num) ||
          (boxList.Exists((obj) => obj.selected) || rowList.Exists((obj) => obj.selected) || colList.Exists((obj) => obj.selected)))
        {
            bg.color = ThemeManager.intance.CurrentTheme.boxHighlight;
        }
        else
        {
            bg.color = ThemeManager.intance.CurrentTheme.boxNormalColor;
        }
    }

    public bool IsWoungInput()
    {
        if (num == 0)
            return false;
        return boxList.Exists((obj) => obj.num == num) || rowList.Exists((obj) => obj.num == num) || colList.Exists((obj) => obj.num == num);
    }

    public bool IsRigthInput()
    {
        if (num == 0)
            return false;
        return !boxList.Exists((obj) => obj.num == num) && !rowList.Exists((obj) => obj.num == num) && !colList.Exists((obj) => obj.num == num);

    }

    public void UpdateHint()
    {
        for (int i = 0; i < 9; i++)
        {
            hintLbl[i].text = hintNum[i] ? (i + 1).ToString() : "";
            hintLbl[i].color = ThemeManager.intance.CurrentTheme.hintNumColor;
        }
    }

    public DataBlock DataBlock
    {
        get
        {
            DataBlock d = new DataBlock();
            d.andNum = andNum;
            d.canEdit = canEdit;
            d.num = num;
            d.hintNum = new bool[9];
            for (int m = 0; m < 9; m++)
            {
                d.hintNum[m] = hintNum[m];
            }
            return d;
        }
        set
        {
            andNum = value.andNum;
            canEdit = value.canEdit;
            num = value.num;
            hintNum = new bool[9];
            for (int m = 0; m < 9; m++)
            {
                hintNum[m] = value.hintNum[m];
            }
        }
    }
}
