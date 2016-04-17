using System.Collections.Generic;
using Entitas;

namespace ShapeTetris.Features.Game.Scene
{
    public class PositionSystem : IReactiveSystem
    {
        public void Execute(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                if(entity.inScene.Object)
                    entity.inScene.Object.transform.position = entity.position.Point;
            }
        }

        public TriggerOnEvent trigger
        {
            get
            {
                return new TriggerOnEvent(Matcher.AllOf(GameMatcher.Position, GameMatcher.InScene), GroupEventType.OnEntityAdded);
            }
        }
    }
}