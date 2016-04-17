using Entitas;
using ShapeTetris.Levels;
using ShapeTetris.Models;
using UniRx;
using UnityEngine;

namespace ShapeTetris.Features
{
    public class StartGameSystem : IInitializeSystem
    {
        public void Initialize()
        {
            var generator = new LevelGenerator(15, 30, 1);
            Observable.Start(() => generator.Generate()).ObserveOnMainThread().Subscribe(OnGenerated);
        }

        private void OnGenerated(Level level)
        {
//            Entity levelEntity = null;
//            var levelEntities = Pools.game.GetEntities(Matcher.Level);
//            Debug.Log(levelEntities.Length);
//            if (levelEntities.Length > 0)
//            {
//                levelEntity = levelEntities[0];
//            }
//            else
//            {
//                levelEntity = Pools.game.CreateEntity();
//            }
            Pools.game.ReplaceLevel(level.Width, level.Height);
            Pools.game.levelEntity.ReplaceLevelData(level.Dots);
//            levelEntity.ReplaceLevel(level.Width, level.Height);
//            levelEntity.ReplaceLevelData(level.Dots);
        }
    }
}