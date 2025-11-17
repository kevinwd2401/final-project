using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLauncher : Turret
{
    float delayTimer;
    public float spreadAngle;
    public bool Ready;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        reloadTimer -= Time.deltaTime;

        if (!freezeRotation) {
            TurnTurret();

            turret.localEulerAngles += cTurretTurn * Time.deltaTime * Vector3.up;
        }

        if (Ready) {
            delayTimer -= Time.deltaTime;
            if (delayTimer < 0) {
                FireTorps();
                delayTimer = 0.16f;
            }
        }
        
    }
    
    private bool FireTorps() {
        return EnemyFireTorpedo(30, spreadAngle);
    }
    public override void AddSelfToTurrets(Enemy e) {
        e.LauncherArray.Add(this);
    }
}
