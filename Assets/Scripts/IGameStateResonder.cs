using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameStateResonder 
{
    void RespondToGameState(GameStateManager.GameState gameState);
}
