using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int health;
    public float speed;
    public int edgeCount;
    public int points;

    public Vector3 playerPosition;
    public float polygonScalar;
    public float maxSpeed;

    public PlayerData() : this("A Square", 100, 0.0f, 4, 0, ApplicationConstants.homeTownCenter, 0.7f, 15f) { }

    public PlayerData(string playerName, int health, float speed, int edgeCount, int points, Vector3 playerPosition, float polygonScalar, float maxSpeed)
    {
        this.playerName = playerName;
        this.health = health;
        this.speed = speed;
        this.edgeCount = edgeCount;
        this.points = points;
        this.playerPosition = playerPosition;
        this.polygonScalar = polygonScalar;
        this.maxSpeed = maxSpeed;
    }
}
