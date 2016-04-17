using System.Collections.Generic;
using Entitas;
using ShapeTetris.Behaviours.Rendering;
using ShapeTetris.Components;
using ShapeTetris.Enums;
using UniRx;
using UnityEngine;
using UnityEngine.VR;

namespace ShapeTetris.Features.Game
{
    public class LevelDataSystem : IReactiveSystem, IEnsureComponents
    {
        private VoxelVolume _levelVolume;
        public LevelDataSystem()
        {
            var levelGo = new GameObject("Level Volume", typeof(VoxelVolume));
            levelGo.name = "Level Volume";
            _levelVolume = levelGo.GetComponent<VoxelVolume>();
        }

        public void Execute(List<Entity> entities)
        {
            Debug.Log("Update Level data");
            var level = entities[0];
            Observable.Start(() => UpdateLevel(level.level, level.levelData)).Subscribe();
        }

        private void UpdateLevel(Level level, LevelData levelData)
        {
            var newData = new VoxelData(level.Width, level.Height, 1);
            var fill = new Color32(233,233,233,255);

            for (int y = 0; y < level.Height; y++)
            {
                for (int x = 0; x < level.Width; x++)
                {
                    var dot = levelData.Dots[y*level.Width + x];
                    if(dot != null && dot.Type == Dots.Border)
                        newData.SetVoxel(x, y, 0, fill);
                }
            }

            _levelVolume.Data = newData;
            _levelVolume.Data.CommitChanges();
        }

        public TriggerOnEvent trigger
        {
            get
            {
                return new TriggerOnEvent(Matcher.AllOf(GameMatcher.LevelData), GroupEventType.OnEntityAdded);
            }
        }

        public IMatcher ensureComponents { get; private set; }
    }
}