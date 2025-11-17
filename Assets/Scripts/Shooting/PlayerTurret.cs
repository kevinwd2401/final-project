using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurret : Turret
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        reloadTimer -= Time.deltaTime;

        TurnTurret();

        turret.localEulerAngles += cTurretTurn * Time.deltaTime * Vector3.up;
    }
    
    public bool FireGuns() {
        return Fire(true, 0.05f);
    }
}
