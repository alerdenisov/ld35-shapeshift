using System.Collections.Generic;
using Entitas;

namespace ShapeTetris.Features.Game.Scene
{
    public class RotationSystem : IReactiveSystem
    {
        public void Execute(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                if(entity.inScene.Object)
                    entity.inScene.Object.transform.localEulerAngles = entity.rotation.LocalEuler;
            }
        }

        public TriggerOnEvent trigger
        {
            get
            {
                return new TriggerOnEvent(Matcher.AllOf(GameMatcher.Rotation, GameMatcher.InScene), GroupEventType.OnEntityAdded);
            }
        }
    }
}