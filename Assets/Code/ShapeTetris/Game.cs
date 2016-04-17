using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using ShapeTetris.Enums;
using ShapeTetris.Features;
using ShapeTetris.Features.Game;
using ShapeTetris.Features.Game.Scene;
using ShapeTetris.Models;
using UnityEditor;

namespace ShapeTetris
{
    public class Game : MonoBehaviour
    {
        #region Singleton

        public static Game Instance;

        void Awake()
        {
            Instance = this;
        }

        #endregion

        private Systems _registeredSystems;


        IEnumerator Start()
        {
            yield return new WaitForSeconds(1);
            _registeredSystems = CreateSystems();
            _registeredSystems.Initialize();
//            var generator = new LevelGenerator();
//            Observable.Start(() => generator.Generate()).ObserveOnMainThread().Subscribe(Generated);

        }

        private Systems CreateSystems()
        {
            return new Feature("BaseGame")
                .Add<StartGameSystem>()
                .Add(Pools.game.CreateSystem<RotationSystem>())
                .Add(Pools.game.CreateSystem<PositionSystem>())
                .Add(Pools.game.CreateSystem<LevelDataSystem>())
                .Add(Pools.game.CreateSystem<PieceBoundsCalculationSystem>())
                .Add(Pools.game.CreateSystem<PieceBottomCalculationSystem>())
                .Add(Pools.game.CreateSystem<PieceGenSystem>())
                .Add(Pools.game.CreateSystem<PieceRotationSystem>())
                .Add(Pools.game.CreateSystem<PieceFallingSystem>())
                .Add(Pools.game.CreateSystem<PieceApplyRotationSystem>())
//                .Add()
            ;
        }

        void Update()
        {
            if(_registeredSystems != null)
                _registeredSystems.Execute();


            if (Pools.game.hasLevel && Input.GetKeyUp(KeyCode.Space))
            {
                var piece = Pools.game.CreateEntity();
                piece.AddPiece(new[]
                {
                    new PieceDot(new Vector3i( 0, -1,   0)),
                    new PieceDot(new Vector3i( 0,  0,   0)),
                    new PieceDot(new Vector3i( 0,  1,   0)),
                    new PieceDot(new Vector3i( 0,  2,   0)),
                    new PieceDot(new Vector3i(-1,  0,   0)),
                    new PieceDot(new Vector3i( 1,  0,   0)),
                    new PieceDot(new Vector3i( 0,  1,   1)),
                    new PieceDot(new Vector3i( 0,  1,   2))
                });

                piece.AddPosition(new Vector3(5, 15, 0));
            }

            var pieces = Pools.game.GetEntities(GameMatcher.Piece);
            if (pieces.Length > 0)
            {
                if (Input.GetAxis("Vertical") != 0)
                {
                    // Todo: add rotation
                    ExecuteInput(pieces[0], Input.GetAxis("Vertical") > 0
                        ? RotateDirection.VerticalPositive
                        : RotateDirection.VerticalNegative);
                }
                else if (Input.GetAxis("Horizontal") != 0)
                {
                    ExecuteInput(pieces[0], Input.GetAxis("Horizontal") > 0
                        ? RotateDirection.HorizontalPositive
                        : RotateDirection.HorizontalNegative);
                }
            }
        }

        private PieceDot[] RecursiveGenerate(int i)
        {
            throw new System.NotImplementedException();
        }

        void ExecuteInput(Entity piece, RotateDirection direction)
        {
            if (piece.hasPieceRotation) return;
            piece.AddPieceRotation(direction, 0);
        }

        void OnGUI()
        {
            GUI.Box(new Rect(0, 0, Screen.width, 30),
                string.Format("H: {0}, V: {1}", Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        }

    }

}