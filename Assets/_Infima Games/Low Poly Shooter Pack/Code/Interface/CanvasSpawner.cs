//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// Player Interface.
    /// </summary>
    public class CanvasSpawner : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        [Title(label: "Settings")]
        
        [Tooltip("Canvas prefab spawned at start. Displays the player's user interface.")]
        [SerializeField]
        private GameObject canvasPrefab;
        
        [Tooltip("Quality settings menu prefab spawned at start. Used for switching between different quality settings in-game.")]
        [SerializeField]
        private GameObject qualitySettingsPrefab;

        [Tooltip("Canvas prefab spawned at start. Displays minimap.")]
        [SerializeField]
        private GameObject Minimap;
        
        [SerializeField]
        private GameObject UpgradeMenu;

        #endregion

        #region UNITY

        /// <summary>
        /// Awake.
        /// </summary>
        private void Awake()
        {
            Time.timeScale = 1;
            //Spawn Interface.
            Instantiate(canvasPrefab);
            //Spawn Quality Settings Menu.
            Instantiate(qualitySettingsPrefab);
            //Spawn Minimap.
            Instantiate(Minimap);

            Instantiate(UpgradeMenu);
        }

        #endregion
    }
}