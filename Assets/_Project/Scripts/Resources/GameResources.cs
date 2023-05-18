using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using DIM.DungeonSystem;
using DIM.PlayerSystem;

namespace DIM {
    public class GameResources : MonoBehaviour {
        #region Singleton GameResources
        private static GameResources instance;
        public static GameResources Instance {
            get
            {
                if (instance == null)
                    instance = Resources.Load<GameResources>("GameResources");

                return instance;
            }
        }
        #endregion



        [Space(10)]
        [Header("AStar Tilemap")]

        public TileBase[] EnemyUnwalkableTileArray;
        public TileBase EnemyPreferredPathTile;


        [Space(10)]
        [Header("Dungeon")]

        public RoomNodeTypeListSO RoomNodeTypeList;


        [Space(10)]
        [Header("Player")]

        public List<PlayerDetailSO> PlayerDetailList;
        public CurrentPlayerSO CurrentPlayer;


        [Space(10)]
        [Header("Materials")]

        public Material DimmedMaterial;
        public Material LitMaterial;


        [Space(10)]
        [Header("Shaders")]

        public Shader VariableLitShader;
        public Shader MaterializeShader;


        [Space(10)]
        [Header("Minimap")]

        public GameObject MinimapSkullPrefab;


        [Space(10)]
        [Header("Player Selection")]

        public GameObject PlayerSelectionPrefab;

        // ===================================================================

        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckEnumerableValue(this, nameof(EnemyUnwalkableTileArray), EnemyUnwalkableTileArray);
            HelperUtilities.CheckNullValue(this, nameof(EnemyPreferredPathTile), EnemyPreferredPathTile);

            HelperUtilities.CheckNullValue(this, nameof(RoomNodeTypeList), RoomNodeTypeList);

            HelperUtilities.CheckNullValue(this, nameof(CurrentPlayer), CurrentPlayer);

            HelperUtilities.CheckNullValue(this, nameof(DimmedMaterial), DimmedMaterial);
            HelperUtilities.CheckNullValue(this, nameof(LitMaterial), LitMaterial);

            HelperUtilities.CheckNullValue(this, nameof(VariableLitShader), VariableLitShader);
        }
#endif
        #endregion
    }
}
