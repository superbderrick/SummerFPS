
using System.Collections;
using System.IO;
using Jinyoung.dev.summerfps.Utills;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using Script.Game;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
        public static GameManager Instance = null;
        public Text InfoText;
        public Text GameStatusText;
        public Text GameTimeText;
        // public Text MonsterHPText;
        public MonsterManager monster;
        
        private float currentTime;
        private bool endedTimer;
        public float GameTime = 60f;
        public float ResultOpenningTime = 3.0f;

        private static string LOSE_GAME = "Lose the game";
        private static string WIN_GAME = "Win the game";
        
        #region UNITY

        public void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (endedTimer)
                return;
            CheckTimer();
        }

        public override void OnEnable()
        {
            base.OnEnable();

            CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
            // MonsterManager.onHpChange +=
        }
        
        public void Start()
        {
            Hashtable props = new Hashtable
            {
                {SummerFPSGame.PLAYER_LOADED_LEVEL, true}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            
            ResetTimer();
        }

        public override void OnDisable()
        {
            base.OnDisable();

            CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
        }

        #endregion

        #region COROUTINES
        
        private IEnumerator EndOfGame(string winner, int remainMonsterHP)
        {
            while (ResultOpenningTime > 0.0f)
            {
                var winnerMessage = GetResultMessage(remainMonsterHP);
                InfoText.text = CommonUtils.GetResultMessage(winner, remainMonsterHP, ResultOpenningTime.ToString("n2"), winnerMessage);
                yield return new WaitForEndOfFrame();

                ResultOpenningTime -= Time.deltaTime;
            }

            GameStatusText.text = CommonUtils.GetStringMessage("게임 상태 :", SummerFPSGame.FINISH_GAME);

            PhotonNetwork.LeaveRoom();
        }

        private static string GetResultMessage(int remainMonsterHP)
        {
            string winnerMessage = LOSE_GAME;
            if (remainMonsterHP > 0)
            {
                winnerMessage = LOSE_GAME;
            }
            else
            {
                winnerMessage = WIN_GAME;
            }

            return winnerMessage;
        }

        #endregion

        #region PUN CALLBACKS

        public override void OnDisconnected(DisconnectCause cause)
        {
            CommonUtils.LoadScene("TitleScene");
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();
        }
        
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            int startTimestamp;
            bool startTimeIsSet = CountdownTimer.TryGetStartTime(out startTimestamp);

            if (changedProps.ContainsKey(AsteroidsGame.PLAYER_LOADED_LEVEL))
            {
                if (CheckAllPlayerLoadedLevel())
                {
                    if (!startTimeIsSet)
                    {
                        GameStatusText.text = CommonUtils.GetStringMessage("게임 상태", SummerFPSGame.PREPARE_GAME); 
                        CountdownTimer.SetStartTime();
                    }
                }
                else
                {
                    InfoText.text = "Waiting for other players...";
                }
            }
        
        }

        #endregion
        
        private void StartGame()
        {
            GameStatusText.text = CommonUtils.GetStringMessage("게임 상태 :", SummerFPSGame.START_GAME);
            MakePlayerManager();
        }

        private static void MakePlayerManager()
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        }
    
        private bool CheckAllPlayerLoadedLevel()
        {
            GameStatusText.text = CommonUtils.GetStringMessage("게임 상태 :", SummerFPSGame.CHECK_LOADING);
            
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object playerLoadedLevel;

                if (p.CustomProperties.TryGetValue(SummerFPSGame.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
                {
                    if ((bool) playerLoadedLevel)
                    {
                        continue;
                    }
                }

                return false;
            }

            return true;
        }
        
        private void FinishGame()
        {
            
            var finalResult = GetFinalResult();
            
            if (PhotonNetwork.IsMasterClient)
            {
                StopAllCoroutines();
            }

            string winner = "";
            int monsterHP = -1;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.GetScore() > monsterHP)
                {
                    winner = p.NickName;
                    monsterHP = finalResult;
                }
            }

            StartCoroutine(EndOfGame(winner, monsterHP));
        }

        private int GetFinalResult()
        {
            string remainMonsterHP = monster.Health.ToString();
            int finalResult = int.Parse(remainMonsterHP);
            return finalResult;
        }

        private void OnCountdownTimerIsExpired()
        {
            StartGame();
        }
        
        private void End_Timer()
        {
            currentTime = 0;
            GameTimeText.text = CommonUtils.GetStringMessage("게임 시간 :" , $"{currentTime:N1}") ;
            endedTimer = true;
            FinishGame();
        }
        
        private void ResetTimer()
        {
            currentTime = GameTime;
            GameTimeText.text = GameTimeText.text = CommonUtils.GetStringMessage("게임 시간 :" , $"{currentTime:N1}") ;
            endedTimer = false;
            
        }
        private void CheckTimer()
        {
            if (0 < currentTime)
            {
                currentTime -= Time.deltaTime;
                GameTimeText.text = GameTimeText.text = CommonUtils.GetStringMessage("게임 시간 :" , $"{currentTime:N1}") ;
            }
            else if (!endedTimer)
            {
                End_Timer();
            }
        }
}