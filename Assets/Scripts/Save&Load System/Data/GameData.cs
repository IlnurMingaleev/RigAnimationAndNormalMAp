using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    // PlayerController fields
     public float maxSpeed;
     public float maxAcceleration;
     public float maxSlowDown;
     public float maxTurnSpeed;
     public float maxAirAcceleration;
     public float maxAirSlowDown;
     public float maxAirTurnSpeed;

    //CharacterJump fields
    public float timeToJumpApex;
    public float upwardMovementMultiplyer;
    public float downwardMovementMultiplyer;
    public int maxAirJumps;
    public bool variableJumpHeight;
    public float jumpCutoff;
    public float speedLimit;
    public float coyoteTime;
    public float jumpBuffer;
    public float jumpHeight;
    public float jumpHeightMultiplyer;



    public GameData() 
    {
        //PlayerController Initial Data
        this.maxSpeed = 5f;
        this.maxAcceleration = 10f;
        this.maxSlowDown = 10f;
        this.maxTurnSpeed = 50f;
        this.maxAirAcceleration = 0f;
        this.maxAirSlowDown = 0f;
        this.maxAirTurnSpeed = 10f;

        //CharacterJump Initial Data
        this.timeToJumpApex = 1f;
        this.upwardMovementMultiplyer = 1f;
        this.downwardMovementMultiplyer = 6.17f;
        this.maxAirJumps = 0;
        this.variableJumpHeight = false;
        this.jumpCutoff = 1f;
        this.speedLimit = 10f;
        this.coyoteTime = 0.15f;
        this.jumpBuffer = 0.15f;
        this.jumpHeight = 2f;
        this.jumpHeightMultiplyer = 0.13f;

}

}
