using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public enum RoadObjectIdentity {
        EMPTY = 0,
        VEHICULE = 1,
        SLOW = 2,
        COIN = 3,
        BONUS = 4
    }

    [Header("GameObjects")]
    public RoadObject[] roadObjects = new RoadObject[0];

    [Header("Variables")]
    public float objectSpeed;
    public float spawnDelay = 0;
    public int oneWayPercent = 70;
    public int twoWayPercent = 28;
    public int coinPercentOnRoad = 5;
    public int bonusPercentOnCoin = 1;
    public int weightSlow = 20;

    [Header("Variables Pour Programmeurs")]
    public List<Section> sectionRecycled = new List<Section>();
    public List<Section> sectionsOnTheRoad = new List<Section>();
    public List<RoadObject>[] recycleObject;
    public Transform recycler;
    public GameObject section;
    public CarController car;
    public Transform spawnSectionPoint;

    public int isOneMoreSectionNeeded = 0;
    public int startVehiculeRange;
    public int startSlowRange;
    private float spawnDelayTimer = 0;
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
        RoadObject leftObjectSpawned = GiveRoadObject(0, RoadObjectIdentity.EMPTY);
        RoadObject centerObjectSpawned = GiveRoadObject(1, RoadObjectIdentity.EMPTY);
        RoadObject rightObjectSpawned = GiveRoadObject(2, RoadObjectIdentity.EMPTY);
        int lastSectionID = sectionsOnTheRoad.Count == 0 ? 7 : sectionsOnTheRoad[sectionsOnTheRoad.Count - 1].id;
        Debug.Log("lastsectionId : " + lastSectionID);
        int roadConfiguration = 7;
        Debug.Log("isOneMoreSectionNeeded 1 : " + isOneMoreSectionNeeded);
        if (sectionsOnTheRoad.Count > 2 && lastSectionID == 5 && sectionsOnTheRoad[sectionsOnTheRoad.Count - 1].id == 5) {
            isOneMoreSectionNeeded = RandomBetween(new int[] { 1, 3, 4, 6, 7 });
        }
        if (isOneMoreSectionNeeded != 0) {
            roadConfiguration = isOneMoreSectionNeeded;
            isOneMoreSectionNeeded = 0;
        } else {
            int possibleway = Random.Range(1, (oneWayPercent * 3 + twoWayPercent * 3 + (100 - oneWayPercent - twoWayPercent) * 3) / 3 + 1);
            Debug.Log("PossibleWay :" + possibleway);
            if (possibleway <= oneWayPercent / 3) {
                Debug.Log("Way : " + 1);
                if (lastSectionID == 1 || lastSectionID == 3 || lastSectionID == 5 || lastSectionID == 7) {
                    roadConfiguration = 1;
                } else if (lastSectionID == 2 || lastSectionID == 6) {
                    roadConfiguration = 3;
                    isOneMoreSectionNeeded = 1;
                } else {
                    roadConfiguration = 7;
                    isOneMoreSectionNeeded = 1;
                }
            } else if (possibleway <= oneWayPercent / 3 * 2) {
                Debug.Log("Way : " + 2);
                if (lastSectionID == 2 || lastSectionID == 3 || lastSectionID == 6 || lastSectionID == 7) {
                    roadConfiguration = 2;
                } else if (lastSectionID == 1) {
                    roadConfiguration = RandomBetween(new int[] { 3, 7 });
                    isOneMoreSectionNeeded = 2;
                } else if (lastSectionID == 4) {
                    roadConfiguration = RandomBetween(new int[] { 6, 7 });
                    isOneMoreSectionNeeded = 2;
                } else {
                    roadConfiguration = RandomBetween(new int[] { 3, 6, 7 });
                    isOneMoreSectionNeeded = 2;
                }
            } else if (possibleway <= oneWayPercent) {
                Debug.Log("Way : " + 4);
                if (lastSectionID == 4 || lastSectionID == 5 || lastSectionID == 6 || lastSectionID == 7) {
                    roadConfiguration = 4;
                } else if (lastSectionID == 2 || lastSectionID == 3) {
                    roadConfiguration = 6;
                    isOneMoreSectionNeeded = 4;
                } else {
                    roadConfiguration = 7;
                    isOneMoreSectionNeeded = 4;
                }
            } else if (possibleway <= oneWayPercent + twoWayPercent / 3) {
                Debug.Log("Way : " + 3);
                if (lastSectionID == 1 || lastSectionID == 2 || lastSectionID == 3 || lastSectionID == 5 || lastSectionID == 6 || lastSectionID == 7) {
                    roadConfiguration = 3;
                } else {
                    roadConfiguration = RandomBetween(new int[] { 6, 7 });
                    isOneMoreSectionNeeded = 3;
                }
            } else if (possibleway <= oneWayPercent + twoWayPercent / 3 * 2) {
                Debug.Log("Way : " + 5);
                if (lastSectionID == 5 || lastSectionID == 7) {
                    roadConfiguration = 5;
                } else {
                    roadConfiguration = 7;
                    isOneMoreSectionNeeded = 5;
                }
            } else if (possibleway <= oneWayPercent + twoWayPercent) {
                Debug.Log("Way : " + 6);
                if (lastSectionID == 2 || lastSectionID == 3 || lastSectionID == 4 || lastSectionID == 5 || lastSectionID == 6 || lastSectionID == 7) {
                    roadConfiguration = 6;
                } else {
                    roadConfiguration = RandomBetween(new int[] { 3, 7 });
                    isOneMoreSectionNeeded = 6;
                }
            } else {
                Debug.Log("Way : " + 7);
                roadConfiguration = 7;
            }
        }

        switch (roadConfiguration) {
            case 1:
                leftObjectSpawned = GiveRoadObject(0, RoadObjectIdentity.COIN, coinPercentOnRoad);
                centerObjectSpawned = GiveRoadObject(1, RoadObjectIdentity.VEHICULE);
                rightObjectSpawned = GiveRoadObject(2, RoadObjectIdentity.VEHICULE);
                break;
            case 2:
                leftObjectSpawned = GiveRoadObject(0, RoadObjectIdentity.VEHICULE);
                centerObjectSpawned = GiveRoadObject(1, RoadObjectIdentity.COIN, coinPercentOnRoad);
                rightObjectSpawned = GiveRoadObject(2, RoadObjectIdentity.VEHICULE);
                break;
            case 3:
                leftObjectSpawned = GiveRoadObject(0, RoadObjectIdentity.COIN, coinPercentOnRoad);
                centerObjectSpawned = GiveRoadObject(1, RoadObjectIdentity.COIN, coinPercentOnRoad);
                rightObjectSpawned = GiveRoadObject(2, RoadObjectIdentity.VEHICULE);
                break;
            case 4:
                leftObjectSpawned = GiveRoadObject(0, RoadObjectIdentity.VEHICULE);
                centerObjectSpawned = GiveRoadObject(1, RoadObjectIdentity.VEHICULE);
                rightObjectSpawned = GiveRoadObject(2, RoadObjectIdentity.COIN, coinPercentOnRoad);
                break;
            case 5:
                leftObjectSpawned = GiveRoadObject(0, RoadObjectIdentity.COIN, coinPercentOnRoad);
                centerObjectSpawned = GiveRoadObject(1, RoadObjectIdentity.VEHICULE);
                rightObjectSpawned = GiveRoadObject(2, RoadObjectIdentity.COIN, coinPercentOnRoad);
                break;
            case 6:
                leftObjectSpawned = GiveRoadObject(0, RoadObjectIdentity.VEHICULE);
                centerObjectSpawned = GiveRoadObject(1, RoadObjectIdentity.COIN, coinPercentOnRoad);
                rightObjectSpawned = GiveRoadObject(2, RoadObjectIdentity.COIN, coinPercentOnRoad);
                break;
            case 7:
            default:
                leftObjectSpawned = GiveRoadObject(0, RoadObjectIdentity.COIN, coinPercentOnRoad);
                centerObjectSpawned = GiveRoadObject(1, RoadObjectIdentity.COIN, coinPercentOnRoad);
                rightObjectSpawned = GiveRoadObject(2, RoadObjectIdentity.COIN, coinPercentOnRoad);
                break;
        }
        Debug.Log("roadConfiguration : " + roadConfiguration);
        Debug.Log("isOneMoreSectionNeeded 2 : " + isOneMoreSectionNeeded);

        sectionsOnTheRoad.Add(MakeNewSection(leftObjectSpawned, centerObjectSpawned, rightObjectSpawned));
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

    public RoadObject GiveRoadObject(int way, RoadObjectIdentity roadObjectidentity, float PercentGive = 0) {

        if (PercentGive != 0) {
            if (Random.Range(1, 101) > PercentGive) {
                roadObjectidentity = RoadObjectIdentity.EMPTY;
            }
        }
        if (roadObjectidentity == RoadObjectIdentity.VEHICULE) {
            if (Random.Range(1, 101) <= weightSlow) {
                roadObjectidentity = RoadObjectIdentity.SLOW;
            }
        }

        if (roadObjectidentity == RoadObjectIdentity.COIN) {
            if (Random.Range(1, 101) <= bonusPercentOnCoin) {
                roadObjectidentity = RoadObjectIdentity.BONUS;
            }
        }

        switch (roadObjectidentity) {
            case RoadObjectIdentity.VEHICULE:
                return roadObjects[Random.Range(startVehiculeRange, startSlowRange)];
            case RoadObjectIdentity.SLOW:
                return roadObjects[Random.Range(startSlowRange, roadObjects.Length)];
            case RoadObjectIdentity.COIN:
                return roadObjects[1];
            case RoadObjectIdentity.BONUS:
                return roadObjects[2];
            default:
            case RoadObjectIdentity.EMPTY:
                return roadObjects[0];
        }
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

    public int RandomBetween(int[] toChoose) {
        return toChoose[Random.Range(1, toChoose.Length + 1) - 1];
    }
}
