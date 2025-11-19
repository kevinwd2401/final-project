using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemy : Enemy
{
    public float ShootingOffset;
    [SerializeField] int EnemyType;
    [HideInInspector] public int seed3, seed2;

    [Header("States")]
    [SerializeField] bool DuckFocus;
    [SerializeField] bool PlayerFocus;
    [SerializeField] bool TorpBoat;
    [SerializeField] bool Solo;

    private float torpSpeed, shellSpeed;
    private float avoidRange, engageRange;

    void Awake() {
        seed3 = Random.Range(0, 3);
        seed2 = Random.Range(0, 2);
        shellSpeed = 20;
        torpSpeed = 20;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        StartCoroutine(ChangeOffset(ShootingOffset));

        player = EnemyManager.Instance.playerTransform.gameObject;
        playerRB = player.GetComponent<Rigidbody>();
        ship = GetComponent<Ship>();

        StartCoroutine(DelayedStart());
    }

    protected IEnumerator DelayedStart() {
        yield return new WaitForSeconds(0.1f);
        foreach (EnemyTurret t in TurretArray) {
            //t.Ready = true;
            shellSpeed = t.bulletSpeed;
        }
        foreach (EnemyLauncher l in LauncherArray) {
            //l.Ready = true;
            torpSpeed = l.bulletSpeed;
        }
    }

    protected IEnumerator StateUpdater() {
        while (true) {
            //update boid variables, state, Destination
            
            yield return new WaitForSeconds(2f + 2 * Random.value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        //moveTargetDuck(shellSpeed);
        moveTarget(playerRB, shellSpeed);
        if (torpTargetTransform != null) {
            moveTorpTarget(torpSpeed);
        }
    }

    public virtual void InitializeEnemy(bool solo, bool playerFocus, bool duckFocus) {
        seekWeight = solo ? 0.1f : 0.8f;
        cohesionWeight = 0.8f;
        alignmentWeight = 2f;
        separationWeight = 1.4f;

        neighborRadius = 30f;
        separationRadius = 7f;
        avoidRange = 16f;

        if (EnemyType == 0) { Health = 2400; engageRange = 120;}
        else if (EnemyType == 1) { Health = 1600 + Random.Range(0, 601); engageRange = 80; TorpBoat = true;}
        else if (EnemyType == 2) { Health = 2000 + Random.Range(0, 601); engageRange = 160;}
        else if (EnemyType == 3) { Health = 2800 + Random.Range(0, 801); engageRange = 100;}
        else if (EnemyType == 4) { Health = 4200 + Random.Range(0, 1801); engageRange = 120;}



    }
    protected override void Destruction2() {
        if (EnemyType == 0) {
            EnemyManager.Instance.FlagShipDestroyed = true;
        }
    }
}
