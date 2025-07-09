using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class edit : MonoBehaviour
{
    public map m;
    public Camera gameCamera;
    public Camera editCamera;
    public Material editMaterial;
    public GameObject wallPrefab;
    private GameObject wall;
    public float wallHigh = 0;
    private bool isEdit = false;
    private GameObject[,] editMap;
    private List<string> result;
    private Hashtable stringIndex;
    private bool isEnd = true;
    private int draw = 0;
    int getMapX(float ox)
    {
        return (int)((ox - m.minX) * m.x / m.heightX - 0.5f);
    }

    int getMapY(float oy)
    {
        return (int)((m.maxY - oy) * m.y / m.widthY - 0.5f);
    }

    float getGameX(int mx)
    {
        return m.minX + (mx + 0.5f) * m.heightX / m.x;
    }

    float getGameY(int my)
    {
        return m.maxY - (my + 0.5f) * m.widthY / m.y;
    }

    IEnumerator editMode()
    {
        isEnd = false;
        Vector3[] clones = { new Vector3(-m.heightX, 0, m.widthY), new Vector3(0, 0, m.widthY), new Vector3(m.heightX, 0, m.widthY), new Vector3(-m.heightX, 0, 0), new Vector3(m.heightX, 0, 0), new Vector3(-m.heightX, 0, -m.widthY), new Vector3(0, 0, -m.widthY), new Vector3(m.heightX, 0, -m.widthY) };
        if (!isEdit && Input.GetKeyDown("e"))
        {
            isEdit = true;
            gameCamera.GetComponent<Camera>().enabled = false;
            editCamera.transform.position = new Vector3((m.minX + m.maxX) / 2, editCamera.transform.position.y, (m.minY + m.maxY) / 2);
            editCamera.GetComponent<Camera>().enabled = true;
            editCamera.tag = "MainCamera";
            GameObject editMap = GameObject.CreatePrimitive(PrimitiveType.Cube);
            editMap.GetComponent<MeshRenderer>().material = editMaterial;
            editMap.transform.position = new Vector3((m.minX + m.maxX) / 2, m.transform.position.y + 20, (m.minY + m.maxY) / 2);
            editMap.transform.localScale = new Vector3(m.heightX, 0.01f, m.widthY);
            wall = Instantiate(wallPrefab);
            foreach (Vector3 v in clones)
            {
                if (!m.horizontalIsCycle && v.x != 0 || !m.verticalIsCycle && v.z != 0)
                {
                    continue;
                }
                wallPrefab.transform.localPosition = v + wall.transform.position;
                wallPrefab.transform.rotation = wall.transform.rotation;
                Instantiate(wallPrefab, wall.transform, true);
            }
            Debug.Log("edit");
        }
        if (isEdit)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int index = 0;
            string debugOut = "";
            if (Physics.Raycast(ray, out hit) && hit.point.x >= m.minX && hit.point.x <= m.maxX && hit.point.z >= m.minY && hit.point.z <= m.maxY)
            {
                wall.transform.position = new Vector3(getGameX(getMapX(hit.point.x)), wallHigh, getGameY(getMapY(hit.point.z)));
                if (Input.GetMouseButton(0))
                {
                    result.Add(getMapX(wall.transform.position.x).ToString() + map.xyDelimiter + getMapY(wall.transform.position.z).ToString());
                    if (!stringIndex.ContainsKey(result[result.Count - 1]))
                    {
                        stringIndex.Add(result[result.Count - 1], result.Count);
                    }
                    else
                    {
                        index = (int)stringIndex[result[result.Count - 1]] - 1;
                        stringIndex.Remove(result[result.Count - 1]);
                        result.RemoveAt(result.Count - 1);
                        result.RemoveAt(index);
                    }
                    foreach (string s in result)
                    {
                        debugOut += s + map.itemDelimiter;
                    }
                    Debug.Log(debugOut);
                    if (-1 != draw && (1 == draw || null == editMap[getMapX(wall.transform.position.x), getMapY(wall.transform.position.z)]))
                    {
                        draw = 1;
                        editMap[getMapX(wall.transform.position.x), getMapY(wall.transform.position.z)] = wall;
                    }
                    else
                    {
                        draw = -1;
                        Destroy(wall);
                        Destroy(editMap[getMapX(wall.transform.position.x), getMapY(wall.transform.position.z)]);
                        editMap[getMapX(wall.transform.position.x), getMapY(wall.transform.position.z)] = null;
                    }
                    wall = Instantiate(wallPrefab);
                    foreach (Vector3 v in clones)
                    {
                        if (!m.horizontalIsCycle && v.x != 0 || !m.verticalIsCycle && v.z != 0)
                        {
                            continue;
                        }
                        wallPrefab.transform.localPosition = v + wall.transform.position;
                        wallPrefab.transform.rotation = wall.transform.rotation;
                        Instantiate(wallPrefab, wall.transform, true);
                    }
                }
                else
                {
                    draw = 0;
                }
            }
        }
        isEnd = true;
        yield return null;
    }

    void Start()
    {
        editMap = new GameObject[m.x, m.y];
        result = new List<string>();
        stringIndex = new Hashtable();
    }

    void Update()
    {
        if (isEnd)
        {
            StartCoroutine(editMode());
        }
    }
}
