﻿//Copyright 2022, Infima Games. All Rights Reserved.

using FishNet.Object;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Game Mode Service.
    /// </summary>
    public class GameModeService : NetworkBehaviour, IGameModeService
    {
        #region FIELDS
        
        /// <summary>
        /// The Player Character.
        /// </summary>
        private CharacterBehaviour playerCharacter;
        
        #endregion
        
        #region FUNCTIONS
        
        public CharacterBehaviour GetPlayerCharacter()
        {
            //Make sure we have a player character that is good to go!
            if (playerCharacter == null)
                playerCharacter = UnityEngine.Object.FindObjectOfType<CharacterBehaviour>();
            
            //Return.
            return playerCharacter;
        }
        
        #endregion
    }
}