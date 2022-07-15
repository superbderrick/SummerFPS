using UnityEngine;

namespace Script.Game
{
    public class SummerFPSGame
    {
        public const string CHECK_LOADING = "로딩 체크중";
        public const string PREPARE_GAME = "게임준비 3초";
        public const string START_GAME = "게임 시작";
        public const string FINISH_GAME = "게임 종료";
        
        public static Color GetColor(int colorChoice)
        {
            switch (colorChoice)
            {
                case 0: return Color.red;
                case 1: return Color.green;
                case 2: return Color.blue;
                case 3: return Color.yellow;
                case 4: return Color.cyan;
                case 5: return Color.grey;
                case 6: return Color.magenta;
                case 7: return Color.white;
            }

            return Color.black;
        }
    }
}