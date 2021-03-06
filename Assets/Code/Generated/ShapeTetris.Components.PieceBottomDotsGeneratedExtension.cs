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
        public ShapeTetris.Components.PieceBottomDots pieceBottomDots { get { return (ShapeTetris.Components.PieceBottomDots)GetComponent(GameComponentIds.PieceBottomDots); } }

        public bool hasPieceBottomDots { get { return HasComponent(GameComponentIds.PieceBottomDots); } }

        public Entity AddPieceBottomDots(ShapeTetris.Models.PieceDot[] newBottoms) {
            var component = CreateComponent<ShapeTetris.Components.PieceBottomDots>(GameComponentIds.PieceBottomDots);
            component.Bottoms = newBottoms;
            return AddComponent(GameComponentIds.PieceBottomDots, component);
        }

        public Entity ReplacePieceBottomDots(ShapeTetris.Models.PieceDot[] newBottoms) {
            var component = CreateComponent<ShapeTetris.Components.PieceBottomDots>(GameComponentIds.PieceBottomDots);
            component.Bottoms = newBottoms;
            ReplaceComponent(GameComponentIds.PieceBottomDots, component);
            return this;
        }

        public Entity RemovePieceBottomDots() {
            return RemoveComponent(GameComponentIds.PieceBottomDots);
        }
    }
}

    public partial class GameMatcher {
        static IMatcher _matcherPieceBottomDots;

        public static IMatcher PieceBottomDots {
            get {
                if (_matcherPieceBottomDots == null) {
                    var matcher = (Matcher)Matcher.AllOf(GameComponentIds.PieceBottomDots);
                    matcher.componentNames = GameComponentIds.componentNames;
                    _matcherPieceBottomDots = matcher;
                }

                return _matcherPieceBottomDots;
            }
        }
    }
