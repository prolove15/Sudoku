using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SudokuManager : MonoBehaviour
{
    [Header("Level Detail")]
    public int totalEasyLevel;
    public int totalMediumLevel;
    public int totalHardLevel;
    public int totalExpertLevel;
    [Space(10)]
    public CanvasGroup gameCompletedDailog;
    public Text timeLbl, bestTimeLbl, modeLbl;

    [Space(10)]
    public List<Transform> boxPrents;
    public Image pencilImage;
    public Text hintCount;
    public GameObject hintCountObj, hintAdIconObj;
    public RewardedVideoButton rewardedVideoButton;
    public GridLayoutGroup gridLayout1;
    public GridLayoutGroup[] gridLayout2;
    public RectTransform rt;

    public Color activeColor, deactiveColor;
    public bool PencilOn;
    public Block blockPrefab;
    public static SudokuManager intance;
    List<Block> blocks;
    List<State> stateList = new List<State>();

    [Header("Audio Clip")]
    public AudioClip editNoteClip;
    public AudioClip editValueClip;
    public AudioClip pencilOnClip;
    public AudioClip pencilOffClip;
    public AudioClip gameWinClip;

    public Block CurrentSelected
    {
        get
        {
            if (blocks.Exists((obj) => obj.selected))
            {
                return blocks.Find((obj) => obj.selected);
            }
            return null;
        }
    }
    public int CurrentSelectedIndex
    {
        get
        {
            if (CurrentSelected != null)
            {
                return blocks.IndexOf(CurrentSelected);
            }
            return 0;
        }
    }

    void Awake()
    {
        intance = this;
    }

    IEnumerator Start()
    {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying) yield break;
#endif

        SetupSudokuObjects();
        UpdateUI();
        yield return new WaitForSeconds(0.1f);
        if (GameManager.LastLoaded == -1)
        {
            LoadNewGame();
        }
        else
        {
            LoadLastSave();
        }
    }

    public void LoadNewGame()
    {
        int[] totalLevels = { totalEasyLevel, totalMediumLevel, totalHardLevel, totalExpertLevel };
        int mode = (int)GameManager.GameMode;
        int level = (GameManager.LastLoaded + 1) % totalLevels[mode];

        LoadGame(mode, level);
    }

    private void LoadGame(int mode, int level)
    {
        GameManager.GameTime = 0;
        GameManager.IsGamePause = false;
        MainMenu.intance.UpdateModeUI();
        MainMenu.intance.UpdateTimeUI();

        string[] paths = { "Easy/Level_", "Medium/Level_", "Hard/Level_", "Expert/Level_" };
        string path = paths[mode];

        print(GameManager.GameMode + " Level  " + level);
        LoadGame(JsonUtility.FromJson<DataGame>(Resources.Load<TextAsset>(path + level).text), level);
    }

    public void RestartGame()
    {
        LoadGame((int)GameManager.GameMode, GameManager.LastLoaded);
    }

    IEnumerator PlayNewGameAnimation()
    {
        GameManager.IsGamePause = true;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                BlockAnimUpperBg(i * 9 + j);
            }
            yield return new WaitForSeconds(.05f);
        }
        GameManager.IsGamePause = false;

    }

    void BlockAnimUpperBg(int index)
    {
        if (index < 0 || index >= blocks.Count)
            return;
        RectTransform temp = blocks[index].upperBg.rectTransform;
        if (LeanTween.isTweening(temp))
            LeanTween.cancel(temp);
        temp.gameObject.SetActive(true);
        blocks[index].upperBg.color = ThemeManager.intance.CurrentTheme.darkColor;
        LeanTween.color(temp, ThemeManager.intance.CurrentTheme.boxNormalColor, 0.5f).setOnComplete(() =>
        {
            temp.gameObject.SetActive(false);
        });
    }

    IEnumerator ShowGameOverPopup()
    {
        GameManager.IsGamePause = true;
        int c = CurrentSelectedIndex % 9;
        int r = CurrentSelectedIndex / 9;
        DeSelectAll();
        for (int i = 0; i < 9; i++)
        {
            for (int a = r - i, b = c - i; b <= c + i; b++)
            {
                if (a >= 0 && b >= 0 && a < 9 && b < 9)
                    BlockAnimUpperBg(a * 9 + b);
            }
            for (int a = r + i, b = c - i; b <= c + i; b++)
            {
                if (a >= 0 && b >= 0 && a < 9 && b < 9)
                    BlockAnimUpperBg(a * 9 + b);
            }
            for (int a = r - i, b = c - i; a <= r + i; a++)
            {
                if (a >= 0 && b >= 0 && a < 9 && b < 9)
                    BlockAnimUpperBg(a * 9 + b);
            }
            for (int a = r - i, b = c + i; a <= r + i; a++)
            {
                if (a >= 0 && b >= 0 && a < 9 && b < 9)
                    BlockAnimUpperBg(a * 9 + b);
            }
            yield return new WaitForSeconds(.05f);
        }
        if (GameManager.GameTime <= GameManager.HighScoreTime)
        {
            GameManager.HighScoreTime = GameManager.GameTime;
        }

        yield return new WaitForSeconds(0.7f);

        LeanTween.alphaCanvas(gameCompletedDailog, 1, 0.1f);
        gameCompletedDailog.GetComponent<Animator>().Play("GameOver");
        GameManager.PlaySound(gameWinClip);
        gameCompletedDailog.blocksRaycasts = true;
        timeLbl.text = GameManager.GetTimeString(GameManager.GameTime);
        bestTimeLbl.text = GameManager.GetTimeString(GameManager.HighScoreTime);
        modeLbl.text = GameManager.GameMode.ToString();

        Timer.Schedule(this, 0.6f, () =>
        {
            CUtils.ShowInterstitialAd();
        });
    }

    [ContextMenu("Complete Game")]
    void DebugGame()
    {
        GameManager.Hints = 100;
        for (int i = 0; i < 75; i++)
        {
            OnBlockClick(blocks[i]);
            GetHint();
        }
    }

    void LoadLastSave()
    {
        if (GameManager.IsGamePause)
            MainMenu.intance.PauseGame();
        LoadGame(GameManager.SavedGame, GameManager.LastLoaded);
    }

    void LoadGame(DataGame game, int index)
    {
        for (int i = 0; i < game.blocks.Count; i++)
        {
            blocks[i].DataBlock = game.blocks[i];
        }
        UpdateUI();
        UpdateAllBlockUI();
        GameManager.SavedGame = game;
        GameManager.LastLoaded = index;
        stateList.Clear();

        UpdateAllBlockUI();
        if (!GameManager.IsGamePause)
        {
            StartCoroutine(PlayNewGameAnimation());
        }

        Timer.Schedule(this, 1f, () =>
        {
            CUtils.ShowInterstitialAd();
        });
    }

    void SetupSudokuObjects()
    {
        foreach (var item in boxPrents)
        {
            for (int i = item.childCount - 1; i >= 0; i--)
            {
                Destroy(item.GetChild(i).gameObject);
            }
        }

        blocks = new List<Block>();
        for (int i = 0; i < 81; i++)
        {
            Transform p = boxPrents[(i / 27) * 3 + (i % 9) / 3];
            Block temp = Instantiate<Block>(blockPrefab, p);
            temp.name = i.ToString("00");
            blocks.Add(temp);
        }

        for (int i = 0; i < 81; i++)
        {
            Block temp = blocks[i];
            // Row Setup
            temp.rowList = new List<Block>();
            for (int j = (i / 9) * 9; j < (i / 9) * 9 + 9; j++)
            {
                temp.rowList.Add(blocks[j]);
            }
            temp.rowList.Remove(temp);

            // Col Setup
            temp.colList = new List<Block>();
            for (int j = i % 9; j < 81; j += 9)
            {
                temp.colList.Add(blocks[j]);
            }
            temp.colList.Remove(temp);

            // Col Setup
            temp.boxList = new List<Block>();
            for (int j = (i / 27) * 27 + ((i % 9) / 3) * 3; j < (i / 27) * 27 + ((i % 9) / 3) * 3 + 27; j += 9)
            {
                temp.boxList.Add(blocks[j]);
                temp.boxList.Add(blocks[j + 1]);
                temp.boxList.Add(blocks[j + 2]);
            }
            temp.boxList.Remove(temp);


            temp.btn.onClick.RemoveAllListeners();
            temp.btn.onClick.AddListener(() => OnBlockClick(temp));
        }
    }

    public void UpdateUI()
    {
        pencilImage.color = PencilOn ? activeColor : deactiveColor;

        bool outofHint = GameManager.Hints == 0;
        hintCountObj.SetActive(!outofHint);
        hintAdIconObj.SetActive(outofHint);
        hintCount.text = GameManager.Hints + "";
    }

    public void PencilClick()
    {
        PencilOn = !PencilOn;
        UpdateUI();
        UpdateAllBlockUI();
        GameManager.PlaySound(PencilOn ? pencilOnClip : pencilOffClip);
    }

    public void EraserClick()
    {
        if (CurrentSelected != null && CurrentSelected.canEdit)
        {
            AddState(CurrentSelectedIndex, CurrentSelected.num, CurrentSelected.hintNum);
            CurrentSelected.num = 0;
            CurrentSelected.hintNum = new bool[9];
            UpdateAllBlockUI();

            SaveSudoku();
        }
    }

    public void OnBlockClick(Block b)
    {
        DeSelectAll();
        b.selected = true;
        UpdateAllBlockUI();

    }

    public void DeSelectAll()
    {
        blocks.ForEach((obj) => { obj.selected = false; });
    }

    public void UpdateAllBlockUI()
    {
        blocks.ForEach((obj) => { obj.UpdateUI(); });
        if (IsGameCompleted())
        {
            StartCoroutine(ShowGameOverPopup());
        }
    }

    public bool IsGameCompleted()
    {
        foreach (var item in blocks)
        {
            if (!item.IsRigthInput())
            {
                return false;
            }
        }
        return true;
    }

    public void SetNumber(int i)
    {
        if (CurrentSelected != null && CurrentSelected.canEdit)
        {
            if (PencilOn)
            {
                GameManager.PlaySound(editNoteClip);
                AddState(CurrentSelectedIndex, CurrentSelected.num, CurrentSelected.hintNum);
                CurrentSelected.num = 0;
                CurrentSelected.hintNum[i - 1] = !CurrentSelected.hintNum[i - 1];
            }
            else if (CurrentSelected.num != i)
            {
                GameManager.PlaySound(editValueClip);
                var state = AddState(CurrentSelectedIndex, CurrentSelected.num, CurrentSelected.hintNum);

                CurrentSelected.num = i;
                CurrentSelected.hintNum = new bool[9];
                if (GameManager.AutoRemoveNotes)
                {
                    List<Block> blockList = new List<Block>();
                    blockList.AddRange(CurrentSelected.rowList);
                    blockList.AddRange(CurrentSelected.colList);
                    blockList.AddRange(CurrentSelected.boxList);

                    foreach (var block in blockList)
                    {
                        if (block.hintNum[i - 1])
                        {
                            state.AddBlockState(blocks.IndexOf(block), block.num, block.hintNum);
                            block.hintNum[i - 1] = false;
                        }
                    }
                }
            }

            SaveSudoku();
            UpdateAllBlockUI();
        }
    }

    private State AddState(int index, int value, bool[] hintValue)
    {
        State state = new State();
        state.AddBlockState(index, value, hintValue);
        stateList.Add(state);
        return state;
    }

    public void OnUndoAction()
    {
        if (stateList.Count > 0)
        {
            State lastState = stateList[stateList.Count - 1];
            Block mainBlock = blocks[lastState.blockStates[0].index];
            foreach(var bState in lastState.blockStates)
            {
                blocks[bState.index].num = bState.value;
                blocks[bState.index].hintNum = bState.hintValue;
            }
            stateList.RemoveAt(stateList.Count - 1);
            DeSelectAll();
            mainBlock.selected = true;
            UpdateAllBlockUI();

            SaveSudoku();
        }
    }

    public void GetHint()
    {
        if (GameManager.Hints == 0)
        {
            rewardedVideoButton.OnClick();
            return;
        }

        if (GameManager.Hints > 0 && CurrentSelected != null && CurrentSelected.canEdit && CurrentSelected.andNum != CurrentSelected.num)
        {
            GameManager.Hints--;
            UpdateUI();

            var state = AddState(CurrentSelectedIndex, CurrentSelected.num, CurrentSelected.hintNum);
            CurrentSelected.num = CurrentSelected.andNum;
            CurrentSelected.hintNum = new bool[9];
            if (GameManager.AutoRemoveNotes)
            {
                List<Block> blockList = new List<Block>();
                blockList.AddRange(CurrentSelected.rowList);
                blockList.AddRange(CurrentSelected.colList);
                blockList.AddRange(CurrentSelected.boxList);

                foreach(var block in blockList)
                {
                    if (block.hintNum[CurrentSelected.andNum - 1])
                    {
                        state.AddBlockState(blocks.IndexOf(block), block.num, block.hintNum);
                        block.hintNum[CurrentSelected.andNum - 1] = false;
                    }
                }
            }

            SaveSudoku();
            UpdateAllBlockUI();
        }
    }

    void SaveSudoku()
    {
        DataGame game = new DataGame();
        game.blocks = new List<DataBlock>();
        blocks.ForEach((obj) => game.blocks.Add(obj.DataBlock));
        GameManager.SavedGame = game;
    }

    private void LateUpdate()
    {
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.x);
        float cellSize = (rt.sizeDelta.x - 20) / 3f;
        gridLayout1.cellSize = new Vector2(cellSize, cellSize);

        float cellSize2 = (cellSize - 4) / 3f;
        foreach(var grid in gridLayout2)
        {
            grid.cellSize = new Vector2(cellSize2, cellSize2);
        }
    }
}

public class State
{
    public List<BlockState> blockStates = new List<BlockState>();

    public void AddBlockState(int index, int value, bool[] hintValue)
    {
        bool[] hValue = new bool[9];
        for (int m = 0; m < 9; m++)
        {
            hValue[m] = hintValue[m];
        }

        BlockState bState = new BlockState
        {
            index = index,
            value = value,
            hintValue = hValue
        };

        blockStates.Add(bState);
    }
}

public class BlockState
{
    public int index;
    public int value;
    public bool[] hintValue;
}

[System.Serializable]
public class DataGame
{
    public List<DataBlock> blocks = new List<DataBlock>();
}

[System.Serializable]
public class DataBlock
{
    public int num = 0;
    public bool canEdit = true;
    public int andNum = 1;
    public bool[] hintNum = new bool[9];
}