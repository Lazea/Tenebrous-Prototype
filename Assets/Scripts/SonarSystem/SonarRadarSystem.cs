using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SonarRadarSystem : MonoBehaviour
{
    [SerializeField]
    SubmarineData data;

    [Header("Radar UI")]
    public RectTransform radarScanline;
    public float radarScanlineRotationSpeed = 1f;
    public RectTransform detectedMarkersPanel;
    public float detectionMarkerUIRadius = 0.4f;
    public Color ironMarkerColor;
    public Color quartzMarkerColor;
    public Color goldMarkerColor;
    public Color diamondMarkerColor;
    public TextMeshProUGUI sonarLevelText;
    public GameObject mineralMarkerPrefab;
    public GameObject enemyMarkerPrefab;
    Image[] mineralMarkers;
    Image[] enemyMarkers;

    [Header("World Objects")]
    public List<Mineral> minerals;
    public List<BaseEnemy> enemies;

    // Start is called before the first frame update
    void Start()
    {
        minerals = new List<Mineral>();
        foreach(var m in GameObject.FindGameObjectsWithTag("Mineral"))
        {
            minerals.Add(m.GetComponent<Mineral>());
        }

        enemies = new List<BaseEnemy>();
        foreach (var e in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(e.GetComponent<BaseEnemy>());
        }

        // Setup markers
        mineralMarkers = new Image[30];
        for (int i = 0; i < mineralMarkers.Length; i++)
        {
            var newMarkers = Instantiate(
                mineralMarkerPrefab,
                detectedMarkersPanel).GetComponent<Image>();
            mineralMarkers[i] = newMarkers;
            mineralMarkers[i].gameObject.SetActive(false);
        }

        enemyMarkers = new Image[30];
        for (int i = 0; i < enemyMarkers.Length; i++)
        {
            var newMarkers = Instantiate(
                enemyMarkerPrefab,
                detectedMarkersPanel).GetComponent<Image>();
            enemyMarkers[i] = newMarkers;
            enemyMarkers[i].gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        InvokeRepeating("SonarPing", 2f, 2f);
    }

    private void OnDisable()
    {
        radarScanline.localRotation = Quaternion.identity;
        CancelInvoke("SonarPing");
    }

    // Update is called once per frame
    void Update()
    {
        radarScanline.Rotate(
            -Vector3.forward * radarScanlineRotationSpeed * Time.deltaTime,
            Space.Self);
    }

    void UpdateRadarMarkers(
        List<(Mineral.MineralType, Vector3, float)> _minerals,
        List<(Vector3, float)> _enemies)
    {
        for(int i = 0; i < mineralMarkers.Length ;i++)
        {
            if(i < _minerals.Count)
            {
                mineralMarkers[i].gameObject.SetActive(true);
                var (mType, dir, dist) = _minerals[i];
                float height = dir.y;   // TODO: Use this for indicating marker elevation
                dir.y = 0f;
                dir.Normalize();
                dir = transform.InverseTransformDirection(dir);
                dir = new Vector3(dir.x, dir.z, 0f);
                float normDist = dist / data.sonarRanges[data.sonarLevel - 1];

                Vector3 markerPosition = dir * normDist * detectionMarkerUIRadius;
                markerPosition.z = 0;
                mineralMarkers[i].rectTransform.localPosition = markerPosition;
                mineralMarkers[i].color = mType switch
                {
                    Mineral.MineralType.Iron => ironMarkerColor,
                    Mineral.MineralType.Quartz => quartzMarkerColor,
                    Mineral.MineralType.Gold => goldMarkerColor,
                    Mineral.MineralType.Diamond => diamondMarkerColor,
                };
            }
            else
            {
                mineralMarkers[i].gameObject.SetActive(false);
            }
        }

        UpdateSonarlLevelText();
    }

    public void UpdateSonarlLevelText()
    {
        sonarLevelText.text = string.Format("LVL {0}", data.sonarLevel);
    }

    [ContextMenu("Sonar Ping")]
    public void SonarPing()
    {
        List<(Mineral.MineralType, Vector3, float)> _minerals = SonarPingForMinerals();
        List<(Vector3, float)> _enemies = SonarPingForEnemies();

        UpdateRadarMarkers(_minerals, _enemies);
    }

    public List<(Mineral.MineralType, Vector3, float)> SonarPingForMinerals()
    {
        float range = data.sonarRanges[data.sonarLevel - 1];
        List<(Mineral.MineralType, Vector3, float)> _minerals = new List<(Mineral.MineralType, Vector3, float)>();
        for(int i = minerals.Count - 1; i >= 0; i--)
        {
            var m = minerals[i];
            if (m == null || m.transform == null)
            {
                minerals.RemoveAt(i);
                continue;
            }

            if (data.sonarLevel == 1)
            {
                if (m.Type != Mineral.MineralType.Iron)
                    continue;
            }
            if (data.sonarLevel == 2)
            {
                if (m.Type != Mineral.MineralType.Iron &&
                    m.Type != Mineral.MineralType.Quartz)
                    continue;
            }

            Vector3 disp = m.transform.position - transform.position;
            Vector3 dir = disp.normalized;
            float dist = disp.magnitude;
            if (dist <= range)
            {
                _minerals.Add((m.Type, dir, dist));
            }
        }

        return _minerals;
    }

    public List<(Vector3, float)> SonarPingForEnemies()
    {
        float range = data.sonarRanges[data.sonarLevel - 1];
        List<(Vector3, float)> _enemies = new List<(Vector3, float)>();
        foreach (var e in enemies)
        {
            if (data.sonarLevel == 1 || data.sonarLevel == 2)
            {
                continue;
            }

            Vector3 disp = transform.position - e.transform.position;
            Vector3 dir = disp.normalized;
            float dist = disp.magnitude;
            if (dist <= range)
            {
                _enemies.Add((dir, dist));
            }
        }

        return _enemies;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(
            transform.position,
            data.sonarRanges[data.sonarLevel-1]);
    }
}
