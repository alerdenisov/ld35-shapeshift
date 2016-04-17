//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGenerator.ComponentExtensionsGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Entitas;

namespace Entitas {
    public partial class Entity {
        public ShapeTetris.Components.PieceRotation pieceRotation { get { return (ShapeTetris.Components.PieceRotation)GetComponent(GameComponentIds.PieceRotation); } }

        public bool hasPieceRotation { get { return HasComponent(GameComponentIds.PieceRotation); } }

        public Entity AddPieceRotation(ShapeTetris.Enums.RotateDirection newDirection, float newTime) {
            var component = CreateComponent<ShapeTetris.Components.PieceRotation>(GameComponentIds.PieceRotation);
            component.Direction = newDirection;
            component.Time = newTime;
            return AddComponent(GameComponentIds.PieceRotation, component);
        }

        public Entity ReplacePieceRotation(ShapeTetris.Enums.RotateDirection newDirection, float newTime) {
            var component = CreateComponent<ShapeTetris.Components.PieceRotation>(GameComponentIds.PieceRotation);
            component.Direction = newDirection;
            component.Time = newTime;
            ReplaceComponent(GameComponentIds.PieceRotation, component);
            return this;
        }

        public Entity RemovePieceRotation() {
            return RemoveComponent(GameComponentIds.PieceRotation);
        }
    }
}

    public partial class GameMatcher {
        static IMatcher _matcherPieceRotation;

        public static IMatcher PieceRotation {
            get {
                if (_matcherPieceRotation == null) {
                    var matcher = (Matcher)Matcher.AllOf(GameComponentIds.PieceRotation);
                    matcher.componentNames = GameComponentIds.componentNames;
                    _matcherPieceRotation = matcher;
                }

                return _matcherPieceRotation;
            }
        }
    }