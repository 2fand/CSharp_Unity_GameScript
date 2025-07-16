using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class edit : MonoBehaviour
{
    public map m;
    public Camera gameCamera;
    private GameObject editCamera;
    public Material editMaterial;
    public Material[] delMaterials;
    public GameObject wallPrefab;
    private GameObject wall;
    public float wallHigh = 0;
    private bool isEdit = false;
    private GameObject[,] editMap;
    private List<string> result;
    private bool isEnd = true;
    private int draw = 0;
    private Material[] defaultMaterials;
    private bool wait = false;
    private bool isMakeWall = false;
    public int extendLeft = 0;
    public int extendRight = 0;
    public int extendUp = 0;
    public int extendDown = 0;
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
            gameCamera.enabled = false;
            editCamera.GetComponent<Camera>().enabled = true;
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
                wall.GetComponent<MeshRenderer>().enabled = true;
                for (int i = 0; i < wall.GetComponentsInChildren<MeshRenderer>().Length; i++)
                {
                    wall.GetComponentsInChildren<MeshRenderer>()[i].enabled = true;
                }
                wall.transform.position = new Vector3(getGameX(getMapX(hit.point.x)), wallHigh, getGameY(getMapY(hit.point.z)));
                if (Input.GetMouseButton(0) && !wait)
                {
                    wait = true;
                    if (-1 != draw && null == editMap[getMapX(wall.transform.position.x), getMapY(wall.transform.position.z)])
                    {
                        result.Add(getMapX(wall.transform.position.x).ToString() + map.xyDelimiter + getMapY(wall.transform.position.z).ToString());
                        draw = 1;
                        if (isMakeWall)
                        {
                            for (int i = 0; i < wall.transform.childCount; i++)
                            {
                                Destroy(wall.transform.GetChild(i).gameObject);
                            }
                            wall.AddComponent<wall>();
                            wall.GetComponent<wall>().x = getMapX(wall.transform.position.x);
                            wall.GetComponent<wall>().y = getMapY(wall.transform.position.z);
                            wall.GetComponent<wall>().extendLeft = extendLeft;
                            wall.GetComponent<wall>().extendRight = extendRight;
                            wall.GetComponent<wall>().extendUp = extendUp;
                            wall.GetComponent<wall>().extendDown = extendDown;
                            wall.GetComponent<wall>().m = m;
                            wall.GetComponent<wall>().wallPrefab = wallPrefab;
                        }
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
                        wall.GetComponent<MeshRenderer>().materials = delMaterials;
                        wall.transform.position = new Vector3(wall.transform.position.x, wallHigh + wall.transform.localScale.y, wall.transform.position.z);
                        for (int i = 0; i < wall.transform.GetComponentsInChildren<MeshRenderer>().Length; i++)
                        {
                            wall.transform.GetComponentsInChildren<MeshRenderer>()[i].materials = delMaterials;
                        }
                        Destroy(editMap[getMapX(wall.transform.position.x), getMapY(wall.transform.position.z)]);
                        editMap[getMapX(wall.transform.position.x), getMapY(wall.transform.position.z)] = null;
                    }
                    wait = false;
                }
                else
                {
                    draw = 0;
                    wall.transform.position = new Vector3(wall.transform.position.x, wallHigh, wall.transform.position.z);
                    wall.GetComponent<MeshRenderer>().materials = defaultMaterials;
                    for (int i = 0; i < wall.GetComponentsInChildren<MeshRenderer>().Length; i++)
                    {
                        wall.GetComponentsInChildren<MeshRenderer>()[i].materials = defaultMaterials;
                    }
                }
            }
            else
            {
                wall.GetComponent<MeshRenderer>().enabled = false;
                for (int i = 0; i < wall.GetComponentsInChildren<MeshRenderer>().Length; i++)
                {
                    wall.GetComponentsInChildren<MeshRenderer>()[i].enabled = false;
                }
            }
            if (Input.GetKeyDown("i"))//反转
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
                            wallPrefab.name = "test";
                            editMap[i, j] = Instantiate(wallPrefab, new Vector3(getGameX(i), wallHigh, getGameY(j)), wallPrefab.transform.rotation);
                            if (isMakeWall)
                            {
                                editMap[i, j].AddComponent<wall>();
                                editMap[i, j].GetComponent<wall>().x = i;
                                editMap[i, j].GetComponent<wall>().y = j;
                                editMap[i, j].GetComponent<wall>().extendLeft = extendLeft;
                                editMap[i, j].GetComponent<wall>().extendRight = extendRight;
                                editMap[i, j].GetComponent<wall>().extendUp = extendUp;
                                editMap[i, j].GetComponent<wall>().extendDown = extendDown;
                                editMap[i, j].GetComponent<wall>().m = m;
                                editMap[i, j].GetComponent<wall>().wallPrefab = wallPrefab;
                            }
                            result.Add(i.ToString() + map.xyDelimiter + j.ToString());
                            if (!isMakeWall)
                            {
                                foreach (Vector3 v in clones)
                                {
                                    if (!m.horizontalIsCycle && v.x != 0 || !m.verticalIsCycle && v.z != 0)
                                    {
                                        continue;
                                    }
                                    wallPrefab.transform.localPosition = v + editMap[i, j].transform.position;
                                    wallPrefab.transform.rotation = editMap[i, j].transform.rotation;
                                    Instantiate(wallPrefab, editMap[i, j].transform, true);
                                }
                            }
                        }
                    }
                }
            }
            if (Input.GetKeyDown("c"))//清空
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
            if (Input.GetKeyDown("r"))//输出坐标
            {
                foreach (string s in result)
                {
                    debugOut += s + map.itemDelimiter;
                }
                GUIUtility.systemCopyBuffer = debugOut;
                Debug.Log("已完成图块坐标复制");
            }
            if (Input.GetKeyDown("n"))//npc移动
            {
                npcMove.npcCanMove = !npcMove.npcCanMove;
            }
            if (Input.GetKeyDown("y"))//玩家移动
            {
                you.notOver = !you.notOver;
                Debug.Log("you.notOver = " + you.notOver.ToString());
            }
            if (Input.GetKeyDown("q")) {//是否造墙
                isMakeWall = !isMakeWall;
                Debug.Log("isMakeWall = " + isMakeWall.ToString());
            }
        }
        isEnd = true;
        yield return null;
    }

    void Start()
    {
        editMap = new GameObject[m.x, m.y];
        result = new List<string>();
        defaultMaterials = wallPrefab.GetComponent<MeshRenderer>().sharedMaterials;
        int delMaterialIndex = -1;
        if (defaultMaterials.Length > delMaterials.Length)
        {
            Material[] tempMaterials = new Material[defaultMaterials.Length];
            delMaterials.CopyTo(tempMaterials, 0);
            delMaterials = tempMaterials;
        }
        for (int i = 0; i < delMaterials.Length; i++)
        {
            if (null != delMaterials[i])
            {
                delMaterialIndex = i;
            }
        }
        for (int i = 0; -1 != delMaterialIndex && i < delMaterials.Length; i++) {
            delMaterials[i] ??= delMaterials[delMaterialIndex];
        }
        editCamera = new GameObject("editCamera");
        editCamera.AddComponent<Camera>();
        editCamera.GetComponent<Camera>().orthographic = true;
        editCamera.GetComponent<Camera>().orthographicSize = 100;
        editCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
        editCamera.transform.position = new Vector3((m.minX + m.maxX) / 2, m.transform.position.y + 100, (m.minY + m.maxY) / 2);
        editCamera.tag = "MainCamera";
        editCamera.GetComponent<Camera>().enabled = false;
    }

    void Update()
    {
        if (isEnd)
        {
            StartCoroutine(editMode());
        }
    }
}
