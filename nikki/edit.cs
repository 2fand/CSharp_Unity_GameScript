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
    public Material delMaterial;
    public GameObject wallPrefab;
    private GameObject wall;
    public float wallHigh = 0;
    private bool isEdit = false;
    private GameObject[,] editMap;
    private List<string> result;
    private bool isEnd = true;
    private int draw = 0;
    private Material defaultMaterial;
    private bool wait = false;
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
        string debugOut = "";
        if (!isEdit && Input.GetKeyDown("e"))
        {
            isEdit = true;
            gameCamera.GetComponent<Camera>().enabled = false;
            editCamera.transform.position = new Vector3((m.minX + m.maxX) / 2, editCamera.transform.position.y, (m.minY + m.maxY) / 2);
            editCamera.GetComponent<Camera>().enabled = true;
            editCamera.tag = "MainCamera";
            GameObject canEditMap = GameObject.CreatePrimitive(PrimitiveType.Cube);
            canEditMap.GetComponent<MeshRenderer>().material = editMaterial;
            canEditMap.transform.position = new Vector3((m.minX + m.maxX) / 2, m.transform.position.y + 20, (m.minY + m.maxY) / 2);
            canEditMap.transform.localScale = new Vector3(m.heightX, 0.01f, m.widthY);
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
            you.notOver = true;
            Debug.Log("edit");
        }
        if (isEdit)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.point.x >= m.minX && hit.point.x <= m.maxX && hit.point.z >= m.minY && hit.point.z <= m.maxY)
            {
                wall.transform.position = new Vector3(getGameX(getMapX(hit.point.x)), wallHigh, getGameY(getMapY(hit.point.z)));
                if (Input.GetMouseButton(0) && !wait)
                {
                    wait = true;
                    if (-1 != draw && null == editMap[getMapX(wall.transform.position.x), getMapY(wall.transform.position.z)])
                    {
                        result.Add(getMapX(wall.transform.position.x).ToString() + map.xyDelimiter + getMapY(wall.transform.position.z).ToString());
                        draw = 1;
                        editMap[getMapX(wall.transform.position.x), getMapY(wall.transform.position.z)] = wall;
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
                    else if(1 != draw)
                    {
                        draw = -1;
                        result.Remove(getMapX(wall.transform.position.x).ToString() + "," + getMapY(wall.transform.position.z).ToString());
                        wall.GetComponent<MeshRenderer>().material = delMaterial;
                        wall.transform.position = new Vector3(wall.transform.position.x, wallHigh + wall.transform.localScale.y, wall.transform.position.z);
                        for (int i = 0; i < wall.transform.GetComponentsInChildren<MeshRenderer>().Length; i++)
                        {
                            wall.transform.GetComponentsInChildren<MeshRenderer>()[i].material = delMaterial;
                        }
                        //Debug.Log(null == editMap[getMapX(wall.transform.position.x), getMapY(wall.transform.position.z)]);
                        Destroy(editMap[getMapX(wall.transform.position.x), getMapY(wall.transform.position.z)]);
                        editMap[getMapX(wall.transform.position.x), getMapY(wall.transform.position.z)] = null;
                    }
                    wait = false;
                }
                else
                {
                    draw = 0;
                    wall.transform.position = new Vector3(wall.transform.position.x, wallHigh, wall.transform.position.z);
                    wall.GetComponent<MeshRenderer>().material = defaultMaterial;
                    for (int i = 0; i < wall.transform.GetComponentsInChildren<MeshRenderer>().Length; i++)
                    {
                        wall.transform.GetComponentsInChildren<MeshRenderer>()[i].material = defaultMaterial;
                    }
                }
            }
            if (Input.GetKeyDown("i"))
            {
                result.Clear();
                for (int i = 0; i < m.x; i++)
                {
                    for (int j = 0; j < m.y; j++)
                    {
                        if (null != editMap[i, j])
                        {
                            Destroy(editMap[i, j]);
                            editMap[i, j] = null;
                        }
                        else
                        {
                            editMap[i, j] = Instantiate(wallPrefab, new Vector3(getGameX(i), wallHigh, getGameY(j)), wallPrefab.transform.rotation);
                            result.Add(i.ToString() + map.xyDelimiter + j.ToString());
                        }
                    }
                }
            }
            if (Input.GetKeyDown("c"))
            {
                result.Clear();
                for (int i = 0; i < m.x; i++)
                {
                    for (int j = 0; j < m.y; j++)
                    {
                        if (null != editMap[i, j])
                        {
                            Destroy(editMap[i, j]);
                            editMap[i, j] = null;
                        }
                    }
                }
            }
            if (Input.GetKeyDown("r"))
            {
                foreach (string s in result)
                {
                    debugOut += s + map.itemDelimiter;
                }
                Debug.Log(debugOut);
            }
            if (Input.GetKeyDown("n"))
            {
                npcMove.npcCanMove = !npcMove.npcCanMove;
            }
            if (Input.GetKeyDown("y"))
            {
                you.notOver = !you.notOver;
                Debug.Log("you.notOver = " + you.notOver.ToString());
            }
        }
        isEnd = true;
        yield return null;
    }

    void Start()
    {
        editMap = new GameObject[m.x, m.y];
        result = new List<string>();
        defaultMaterial = wallPrefab.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void Update()
    {
        if (isEnd)
        {
            StartCoroutine(editMode());
        }
    }
}
