using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Transform[] possibleTargetPoints;
    [SerializeField] int WaveNumber;
    [SerializeField] GameObject[] shipPrefabs;
    [SerializeField] GameObject[] blackShipPrefabs;
    public static EnemyManager Instance;
    public Transform playerTransform;

    public bool FlagShipDestroyed;
    public Enemy Flagship;
    private Vector3 chosenTargetPoint;

    private int enemyCount;
    public int EnemyCount
    {
        get => enemyCount;
        set
        {
            enemyCount = value;
            Debug.Log("Enemies: " + enemyCount);
            if (enemyCount == 0) {
                StartCoroutine(SpawnWave());
            }
        }
    }
    public int Deaths;


    void Awake() {
        Instance = this;
        WaveNumber = 0;
        Deaths = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWave());
    }


    private IEnumerator SpawnWave() {
        yield return new WaitForSeconds(3);
        WaveNumber++;
        Debug.Log("Spawning Wave " + WaveNumber);

        SpawnFlagship();

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < Mathf.Min(WaveNumber + 2, 5 + Random.Range(0, 5)); i++) {
            //could reset target point, 
            ClearTargetPoint();
            GameObject ship = Instantiate(shipPrefabs[Random.Range(1, WaveNumber + 1)]);
            ship.transform.GetChild(0).position = chosenTargetPoint;
            ship.transform.GetChild(0).gameObject.GetComponent<DefaultEnemy>().InitializeEnemy(true, false, false);
            chosenTargetPoint.x += 10;
            enemyCount++;

            yield return new WaitForSeconds(0.2f);
        }

        if (WaveNumber > 5 && Random.value > 0.7) {
            //SpawnSpecialShip()
        }

    }

    private void SelectTargetPoint() {
        int r = Random.Range(0, possibleTargetPoints.Length);
        chosenTargetPoint = playerTransform.position + 320 * (possibleTargetPoints[r].position - playerTransform.position).normalized;
        chosenTargetPoint.y = 1;
    }
    private void ClearTargetPoint() {
        Collider[] hits = Physics.OverlapSphere(chosenTargetPoint, 8f, 1 << 11);

        foreach (Collider col in hits)
        {
            Transform t = col.transform;

            Vector3 pos = t.position;
            pos.z += 20f;
            t.position = pos;
        }
    }

    private void SpawnFlagship() {
        SelectTargetPoint();
        ClearTargetPoint();

        FlagShipDestroyed = false;
        GameObject ship = Instantiate(shipPrefabs[0]);
        ship.transform.GetChild(0).position = chosenTargetPoint;
        Flagship = ship.transform.GetChild(0).gameObject.GetComponent<Enemy>();
        ((DefaultEnemy)Flagship).InitializeEnemy(true, false, false);
        chosenTargetPoint.x += 10;
        enemyCount++;
    }

    private void SpawnSpecialShip() {
        SelectTargetPoint();
        ClearTargetPoint();

        GameObject ship = Instantiate(blackShipPrefabs[Random.Range(0, blackShipPrefabs.Length)]);
        ship.transform.GetChild(0).position = chosenTargetPoint;
        ship.transform.GetChild(0).gameObject.GetComponent<DefaultEnemy>().InitializeEnemy(true, true, false);
        enemyCount++;
    }

    public void EndGame() {
        Debug.Log("Game Ended");
    }
}
