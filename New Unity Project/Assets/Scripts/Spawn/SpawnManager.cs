using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [Header("GameObjects")]
    public RoadObject[] roadObjects = new RoadObject[0];

    [Header("Variables")]
    public float objectSpeed;
    public float spawnDelay = 0;
    int oneWayPercent = 70;
    int twoWayPercent = 25;

    [Header("Variables Pour Programmeurs")]
    public List<Section> sectionRecycled = new List<Section>();
    public List<Section> sectionsOnTheRoad = new List<Section>();
    public List<RoadObject>[] recycleObject;
    public Transform recycler;
    public GameObject section;
    public CarController car;
    public Transform spawnSectionPoint;

    private float spawnDelayTimer = 0;
    private bool isOneMoreSectionNeeded = true;
    private int wayToTake =1;
    void Start() {
        recycleObject = new List<RoadObject>[roadObjects.Length];
        for (int i = 0; i < recycleObject.Length; i++) {
            recycleObject[i] = new List<RoadObject>();
        }
    }

    void Update() {
        if (spawnDelayTimer <= 0) {
            spawnDelayTimer = spawnDelay;
            MakeTheWay();
        } else {
            spawnDelayTimer -= Time.deltaTime;
        }
    }
    public void MakeTheWay() {
        RoadObject leftObjectSpawned = roadObjects[0];
        RoadObject centerObjectSpawned = roadObjects[0];
        RoadObject rightObjectSpawned = roadObjects[0];
        ////int possibleway = Random.Range(1, 99);
        if (isOneMoreSectionNeeded) {
            isOneMoreSectionNeeded = false;
            if (wayToTake == 0) {
                centerObjectSpawned = roadObjects[1];
                rightObjectSpawned = roadObjects[1];
            } else if (wayToTake == 1) {
                leftObjectSpawned = roadObjects[1];
                rightObjectSpawned = roadObjects[1];
            } else {
                leftObjectSpawned = roadObjects[1];
                centerObjectSpawned = roadObjects[1];
            }
        } else {
            int whichIsTaken = Random.Range(1, 99);
            if (whichIsTaken <= 33) {
                wayToTake = 0;
                if (sectionsOnTheRoad[sectionsOnTheRoad.Count - 1].left.id == 0) {
                    centerObjectSpawned = roadObjects[1];
                    rightObjectSpawned = roadObjects[1];
                } else if (sectionsOnTheRoad[sectionsOnTheRoad.Count - 1].center.id == 0) {
                    rightObjectSpawned = roadObjects[1];
                    isOneMoreSectionNeeded = true;
                } else {
                    isOneMoreSectionNeeded = true;
                }
            } else if (whichIsTaken <= 66) {
                wayToTake = 1;
                if (sectionsOnTheRoad[sectionsOnTheRoad.Count - 1].center.id == 0) {
                    leftObjectSpawned = roadObjects[1];
                    rightObjectSpawned = roadObjects[1];
                } else if (sectionsOnTheRoad[sectionsOnTheRoad.Count - 1].right.id == 0) {
                    leftObjectSpawned = roadObjects[1];
                    isOneMoreSectionNeeded = true;
                } else {
                    rightObjectSpawned = roadObjects[1];
                    isOneMoreSectionNeeded = true;
                }
            } else {
                wayToTake = 2;
                if (sectionsOnTheRoad[sectionsOnTheRoad.Count - 1].right.id == 0) {
                    centerObjectSpawned = roadObjects[1];
                    leftObjectSpawned = roadObjects[1];
                } else if (sectionsOnTheRoad[sectionsOnTheRoad.Count - 1].center.id == 0) {
                    leftObjectSpawned = roadObjects[1];
                    isOneMoreSectionNeeded = true;
                } else {
                    isOneMoreSectionNeeded = true;
                }
            }
        }
        sectionsOnTheRoad.Add(MakeNewSection(leftObjectSpawned, centerObjectSpawned, rightObjectSpawned));

        //if (possibleway <= oneWayPercent) {
        //    if (whichIsTaken <= 33) {
        //        if (sectionsOnTheRoad[sectionsOnTheRoad.Count - 1].left.id == 0) {
        //            centerObjectSpawned = roadObjects[1];
        //            rightObjectSpawned = roadObjects[1];
        //        }
        //    } else if (whichIsTaken <= 66) {

        //    } else {

        //    }
        //} else if (possibleway <= oneWayPercent + twoWayPercent) {
        //    if (whichIsTaken <= 33) {

        //    } else if (whichIsTaken <= 66) {

        //    } else {

        //    }
        //}
    }
    public Section MakeNewSection(RoadObject left, RoadObject center, RoadObject right) {
        Section currentSection;
        if (sectionRecycled.Count != 0) {
            currentSection = sectionRecycled[0];
            sectionRecycled.RemoveAt(0);
            currentSection.transform.position = spawnSectionPoint.position;
            currentSection.transform.SetParent(null);
        } else {
            currentSection = Instantiate(section, spawnSectionPoint.position, Quaternion.identity).GetComponent<Section>();
        }
        currentSection.Initialize(left, center, right);
        return currentSection;
    }


    public void AddToRecycleList(RoadObject toRecycle) {
        if (toRecycle.id + 1 <= roadObjects.Length) {
            recycleObject[toRecycle.id].Add(toRecycle);
            toRecycle.gameObject.SetActive(false);
            if (toRecycle.gameObject.transform.parent.childCount == 1) {
                sectionsOnTheRoad.RemoveAt(0);
                sectionRecycled.Add(toRecycle.gameObject.transform.parent.GetComponent<Section>());
                toRecycle.gameObject.transform.parent.transform.SetParent(recycler.GetChild(0));
            }
            toRecycle.gameObject.transform.SetParent(recycler.GetChild(1));

        } else {
            Destroy(toRecycle);
            Debug.Log("Destroy");
        }
    }


}
