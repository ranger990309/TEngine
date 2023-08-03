using System.Threading;
using Cysharp.Threading.Tasks;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    /// <summary>
    /// 关卡系统，控制关卡。
    /// </summary>
    public class LevelSystem: SubSystem
    {
        public Transform LevelRoot { private set; get; }
        
        public GameObject LevelObject { private set; get; }
        
        protected override void OnInit()
        {
            Log.Debug("SubSystem LevelSystem OnInit");
            if (LevelRoot == null)
            {
                LevelRoot = new GameObject("LevelRoot").transform;
                Object.DontDestroyOnLoad(LevelRoot);
            }
        }

        public async UniTaskVoid OnStartBattle()
        {
            await UniTask.Yield();

            LevelObject = await GameModule.Resource.LoadGameObjectAsync("Level", CancellationToken.None);
            
            LevelObject.transform.SetParent(LevelRoot);
        }
    }
}