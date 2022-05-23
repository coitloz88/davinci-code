using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using UnityEngine;

namespace AI
{
    public class ComputerGuessCode
    {
        public ComputerGuessCode() { }
        public Code mRandomCode;
        public int mRandomPos;
    }
    public class Data_PlayerCode
    {
        public Data_PlayerCode()
        { 
            Data_CanPlayerHas = new List<List<int>>();
        }

        public List<List<int>> Data_CanPlayerHas; // ForGuess

        #region Data 삽입
        // 길이가 26인 코드를 받아와야 함 , index = 저장위치 
        // 코드는 -1이 없음 0 모름 1 있음으로 표기 
        // 색 또한 볼 수 있으니 넣기 전에 다른 색 일괄 -1 결정
        public void Add_CanPlayerHas(List<int> AddCL, int index) 
        {
            Data_CanPlayerHas.Insert(index, AddCL);
            Check_CanPlayerHas();
        }

        public void Check_CanPlayerHas() // Data_CanPlayerHas 중 한 곳에 1이 있을 시 그 전의 (이상의 값)의 Code는 가질 수 없고 그 후 (이하의 값)의 Code는 가질 수 없음을 표시
        {
            for (int i = 0; i < Data_CanPlayerHas.Count; i++)
            {
                for (int j = 0; j < Data_CanPlayerHas[i].Count - 2; j++) // -2 는 조커 때문에
                {
                    if (Data_CanPlayerHas[i][j] == 1)
                    {
                        Change_LeftUpCode_NotHas(i,j);
                        Change_RightDownCode_NotHas(i,j);
                    }
                }
                for (int j = Data_CanPlayerHas[i].Count - 2; j < Data_CanPlayerHas[i].Count; j++) // 이 부분은 조커 때문에
                {
                    if (Data_CanPlayerHas[i][j] == 1)
                    {
                        Change_joker_NotHas();
                    }
                }
            }
        }
        public void Change_LeftUpCode_NotHas(int index, int value)
        {
            for (int i = 0; i < index; i++)
            {
                for (int j = value; j < Data_CanPlayerHas[i].Count - 2; j++)
                {
                    Data_CanPlayerHas[i][j] = -1;
                }
            }
        }
        public void Change_RightDownCode_NotHas(int index, int value)
        {
            for (int i = index; i < Data_CanPlayerHas.Count; i++)
            {
                for (int j = 0; j <= value; j++)
                {
                    Data_CanPlayerHas[i][j] = -1;
                }
            }
        }
        public void Change_joker_NotHas()
        {
            for (int i = 0; i < Data_CanPlayerHas.Count; i++)
            {
                for (int j = Data_CanPlayerHas[i].Count - 2; j < Data_CanPlayerHas[i].Count; j++) Data_CanPlayerHas[i][j] = -1;
            }
        }
        #endregion

        #region Data 갱신
        public void Renew_DataPlayerHas(int value) // 컴퓨터가 가진 것은 가질 수 없음(새로 뽑은 것 포함)
        {
            for (int i = 0; i < Data_CanPlayerHas.Count; i++)
            {
                Data_CanPlayerHas[i][value] = -1;
            }
        }
        #endregion
    } // 단순히 시각적으로 알 수 있는 Data // 얘 아래 단계에 섞어줘야함

    public class Create_RandomCode
    {
        public Create_RandomCode(Data_PlayerCode DPC)
        {
            List<List<int>> Data = DPC.Data_CanPlayerHas;
            Check_EachCodeZeroValue = new List<int>();
            Check_CanknowCode(); // Check_EachCodeZeroValue 세팅
        }

        public ComputerGuessCode Create()
        {
            ComputerGuessCode RandomCode = new ComputerGuessCode();

            //여기 꾸며야 함

            return RandomCode;
        }

        public void Check_CanknowCode() // Check_EachCodeZeroValue 세팅 Code별 0 개수 보는 것
        {
            for (int i = 0; i < Data.Count; i++)
            {
                int n = 0;
                for (int j = 0; j < Data[i].Count; j++)
                {
                    if (Data[i][j] == 1) {
                        n = 0;
                        break; 
                    }
                    else if (Data[i][j] == -1) continue;
                    n++;
                }
                Check_EachCodeZeroValue.Add(n);
            }
        }

        List<List<int>> Data;
        List<int> Check_EachCodeZeroValue;
    } // 컴퓨터가 제시할 RandomCode를 뽑는 과정

    public class Guess_Storage
    {
        public Guess_Storage() { 
            for (int i = 0; i < Storage_AllCode.Length; i++)
            {
                Storage_AllCode[i].Num = i / 2;
                if (i % 2 == 0) Storage_AllCode[i].Color = "black";
                else if (i % 2 == 1) Storage_AllCode[i].Color = "white";
            }
            for (int i = 0; i < Storage_AllCode_PlayerHas.Length; i++) {
                Storage_AllCode_PlayerHas[i] = 0;
            }
        }
        public Code[] Storage_AllCode = new Code[26]; // Code 정보가 다 들어 있음

        public int[] Storage_AllCode_PlayerHas = new int[26]; // -1 없음 // 0 모름 // 1 있음

        public bool CompareRecode(ComputerGuessCode GuessCode) // 추측 Array 중 모르는 Code였는지 체크 // 모르는 것이라면 false 리턴
        {
            for (int i = 0; i < Storage_AllCode_PlayerHas.Length; i++)
                if (Storage_AllCode[i] == GuessCode.mRandomCode)
                    if (Storage_AllCode_PlayerHas[i] == 0) return false;
            return true;
        }

        public void ExceptComputerCode(Code AddCode) // AI 자신이 새로 뽑은 Code를 Storage_AllCode_PlayerHas에서 제외
        {
            for (int i = 0; i < Storage_AllCode_PlayerHas.Length; i++)
                if ((Storage_AllCode[i].Color == AddCode.Color) && (Storage_AllCode[i].Num == AddCode.Num))
                    Storage_AllCode_PlayerHas[i] = -1;
        }
        public void ExceptPlayerCode(Code GuessCode) // 맞춘 플레이어 코드를 Storage_AllCode_PlayerHas에서 제외
        {
            for (int i = 0; i < Storage_AllCode_PlayerHas.Length; i++)
                if ((Storage_AllCode[i].Color == GuessCode.Color) && (Storage_AllCode[i].Num == GuessCode.Num))
                    Storage_AllCode_PlayerHas[i] = 1;
        }
    }

    public class ComputerAI_Manager : MonoBehaviour
    {
        System.Random r = new System.Random();

        public GameObject Game_Manager;

        public Answer_Storage Computer_Storage;
        public Guess_Storage Guess_Player_Storage;

        #region 컴퓨터의 카드 뽑기
        public void Computer_Storage_AddCode(Code AddCode)
        {
            int index = 0;
            while (index <= Computer_Storage.top)
            {
                if (CompareCode(Computer_Storage.Storage_Code[index], AddCode)) index++;
                else break;
            }
            for (int i = ++Computer_Storage.top; i > index; i--) Computer_Storage.Storage_Code[i] = Computer_Storage.Storage_Code[i - 1];
            Computer_Storage.Storage_Code[index] = AddCode;
            Guess_Player_Storage.ExceptComputerCode(AddCode);
        } // 컴퓨터가 뽑는 Code 정보 저장
        public bool CompareCode(Code PlayerCode,Code AddCode) // 뒷 코드가 크면 true 반환 아니면 false 반환
        {
            if (PlayerCode.Num < AddCode.Num) return true;
            if ((PlayerCode.Num == AddCode.Num) && PlayerCode.Color == "black") return true;
        
            return false;
        }
        #endregion

        public bool ConfigCode(Answer_Storage PlayerCode, ComputerGuessCode GuessCode) // 생성한 랜덤 Code 블럭이 맞는지 체크
        {
            if (PlayerCode.Storage_Code[GuessCode.mRandomPos].ConfigHas(GuessCode.mRandomCode.Color, GuessCode.mRandomCode.Num)) {
                Guess_Player_Storage.ExceptPlayerCode(GuessCode.mRandomCode);
                return true; 
            }
            else return false;
        }

        #region BeginnerAI 판단 함수
        public ComputerGuessCode BeginnerAI_GuessPlayerCode(Answer_Storage PlayerCode) // Beginner AI의 랜덤 Code 블럭 추측 함수
        {
            ComputerGuessCode GuessCode = new ComputerGuessCode();
            
            do {
                string RandomColor;
                if (r.Next(2) == 0) RandomColor = "white";
                else RandomColor = "black";
                GuessCode.mRandomCode.Color = RandomColor;
                int RandomNum = r.Next(12); GuessCode.mRandomCode.Num = RandomNum;
                int RandomPos = r.Next(PlayerCode.top + 1); GuessCode.mRandomPos = RandomPos;
            } while (Guess_Player_Storage.CompareRecode(GuessCode));

            return GuessCode;
        }
        //public ComputerGuessCode MakeRandomCode() {
        //    ComputerGuessCode GuessCode = new ComputerGuessCode();
        //}

        public void WorkingBeginnerAI()
        {
            Answer_Storage PlayerCode = Game_Manager.GetComponent<Game_Manager>().PlayerCode;
            ComputerGuessCode GuessCode = BeginnerAI_GuessPlayerCode(PlayerCode);

            bool _isAnswer = ConfigCode(PlayerCode, GuessCode);
            if (_isAnswer) Debug.Log("AI가 정답을 맞춤!");
            else Debug.Log("AI가 오답을 맞춤!");
        }
        #endregion
    }
}
