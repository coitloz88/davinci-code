using System;
using UnityEngine;
using UnityEngine.UI;

/*
 컴파일 에러가 나는 부분이 있어서
//컴파일 에러 방지용
주석 붙이고 추가한 구문있음(추후 삭제바람)
 */
public class Code
{
    public Code()
    {
        Color = "";
        Num = -1;
    }
    public Code(string _Color, int _Num)
    {
        Color = _Color;
        Num = _Num;
    }
    public bool ConfigHas(string _Color, int _Num)
    {
        if (Color == _Color && Num == _Num) return true;
        return false;
    }
    public int returnNum()
    {
        return Num;
    }

    public string Color = "";
    public int Num = -1;
}

public class Answer_Storage
{
    public void StartCode()
    {
        int i, j;
        int R1, R2;
        int num;
        string color;
        System.Random r = new System.Random();
        for (i = 0; i < 30; i++)
        {
            R1 = r.Next(0, 26 - top);
            R2 = r.Next(1, 3);
            if (box[R1] != -1)
            {
                color = colorbox[R2];
                num = box[R1];
                box[R1] = -1;
                Code c = new Code(color, num);
                if (!checkCode(c))
                {
                    if (R2 == 1)
                    {
                        c.Color = "black";
                    }
                    else
                        c.Color = "white";
                }
                InsertCode(c.Color, c.Num);
            }
            if (top == 4)
            {
                break;
            }

        }
    }
    public void player(Code code)
    {
        for (int i = 0; i < top; i++)
        {
            if (Storage_Code[i] == code)
            {
                DeleteCode(i);
            }
        }
    }

    public bool checkCode(Code code)
    {
        for (int i = 0; i < top; i++)
        {
            if (Storage_Code[i] == code)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        return true; //컴파일 에러 방지용
    }

    public Code selectCode() //카드 뽑기
    {
        int i, j;
        int R1, R2;
        int num;
        string color;
 
        Code c; 

        System.Random r = new System.Random();
        for (i = 0; i < 30; i++)
        {
            R1 = r.Next(0, 26 - top);
            R2 = r.Next(1, 3);
            if (box[R1] != -1)
            {
                color = colorbox[R2];
                num = box[R1];
                box[R1] = -1;
                c = new Code(color, num);
                if (!checkCode(c))
                {
                    if (R2 == 1)
                    {
                        c.Color = "black";
                    }
                    else
                        c.Color = "white";
                }
                InsertCode(c.Color, c.Num);
            } 


        }
        c = new Code("", -1); //컴파일 에러 방지용

        return c; //컴파일 에러 방지용
    }
    public void InsertCode(string Color, int Num,int pos=0)
    {
        int i, j;
        if (Num == 12)
        {
            // Storage_Code[pos] = new Code(Color, Num); 한칸씩 밀려나게 그리고 조커는 -1로 하기

        }
        else
        {
            for (i = 0; i < top; i++)
            {
                if (Storage_Code[i].returnNum() > Num)
                {
                    break;
                }
            }
            if (Storage_Code[i].returnNum() == Num)
            {
                if (Color == "white")
                {
                    for (j = top - 1; j > i; j--)
                    {
                        Storage_Code[j + 1] = Storage_Code[j];
                    }
                    Storage_Code[i + 1] = new Code(Color, Num);
                }
                else
                {
                    for (j = top - 1; j >= i; j--)
                    {
                        Storage_Code[j + 1] = Storage_Code[j];
                    }
                    Storage_Code[i] = new Code(Color, Num);
                }
            }
            else
            {
                for (j = top - 1; j >= i; j--)
                {
                    Storage_Code[j + 1] = Storage_Code[j];
                }
                Storage_Code[i] = new Code(Color, Num);
            }
        }
        top++;
    }
    public void DeleteCode(int Index)
    {
        int i;
        for (i = Index; i < top; i++)
        {
            Storage_Code[i] = Storage_Code[i + 1];
        }
        top--;
    }
    public int Count = 0;
    public string[] colorbox = { "white", "black" };
    public int[] box = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12 };
    public int top = -1; //카드의 개수
    public Code[] Storage_Code = new Code[30];
} // 값이 들어갈 떄마다 top 증가해야 함. Arr 길이를 알아야 해서


public class Game_Manager : MonoBehaviour
{
    public Tile_Manager CloneTile;

    public Tile_Manager[] TileArray = null;

    #region 변수 참조
    bool _isGamePlay = false; // 게임 시작 판단
    bool _isGamePause = false; // 게임 중지 판단

    bool _PlayerTurn = false; // 나의 턴인지 판단 // false이면 컴퓨터 턴임.
    bool _ChooseCode = false; // 추가 코드 뽑기
    bool _GuessCode = false; // 상대 코드 추측
    bool _isCorrect = false; // 답이 맞는지 체크

    string GuessColor = ""; // 블랙 화이트 추측
    int GuessNum; // 숫자 추측

    string AnswerColor = ""; // 상대 컬러
    int AnswerNum; // 상대 숫자

    float EachTurnTime = 15f; // 턴 제한 시간
    float EachTurnCurrTime = 0; // 현제 턴 지난 시간

    public Answer_Storage NotSelectedCode; // 고를 수 있는, 남은 Code 들
    public Answer_Storage PlayerCode; // Computer의 Code는 숨기기 위해서 ComputerAI_Manater에 생성.

    // 등등 참조해서 쓰기
    #endregion

    public void Awake()
    {
        TileArray = new Tile_Manager[4];
    }

    Tile_Manager CloneTileFunc(int num)
    {
        GameObject copyobj = GameObject.Instantiate(CloneTile.gameObject);
        copyobj.transform.SetParent(this.transform);

        Vector3 temppos = new Vector3(num, -100, 0);
        copyobj.transform.localPosition = temppos;
        copyobj.name = "Tile_" + num.ToString();

        return copyobj.GetComponent<Tile_Manager>();
    }

    void TileGenerator()
    {
        for(int i = 0; i < 4; i++)
        {
            TileArray[i] = CloneTileFunc(i);
        }
    }
}
