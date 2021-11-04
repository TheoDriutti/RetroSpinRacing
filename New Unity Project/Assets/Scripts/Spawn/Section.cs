using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour {

    public RoadObject left, center, right;
    public int id = 0;

    private void Update() {
        transform.position -= new Vector3(0, 0, Gino.instance.spawnManager.objectSpeed * Time.deltaTime);
    }
    public void Initialize(RoadObject left, RoadObject center, RoadObject right) {
        Vector3 leftPosition = new Vector3(Gino.instance.spawnManager.car.lanes[1].trans.position.x, transform.position.y, transform.position.z);
        Vector3 centerPosition = new Vector3(Gino.instance.spawnManager.car.lanes[2].trans.position.x, transform.position.y, transform.position.z);
        Vector3 rightPosition = new Vector3(Gino.instance.spawnManager.car.lanes[3].trans.position.x, transform.position.y, transform.position.z);
        if (Gino.instance.spawnManager.recycleObject[left.id].Count == 0) {
            this.left = Instantiate(left.gameObject, leftPosition, Quaternion.identity).GetComponent<RoadObject>();
        } else {
            this.left = Gino.instance.spawnManager.recycleObject[left.id][0];
            Gino.instance.spawnManager.recycleObject[left.id].RemoveAt(0);
            this.left.transform.position = leftPosition;
            this.left.gameObject.SetActive(true);
        }
        this.left.transform.SetParent(transform);
        if (Gino.instance.spawnManager.recycleObject[center.id].Count == 0) {
            this.center = Instantiate(center.gameObject, centerPosition, Quaternion.identity).GetComponent<RoadObject>();
        } else {
            this.center = Gino.instance.spawnManager.recycleObject[center.id][0];
            Gino.instance.spawnManager.recycleObject[center.id].RemoveAt(0);
            this.center.transform.position = centerPosition;
            this.center.gameObject.SetActive(true);
        }
        this.center.transform.SetParent(transform);
        if (Gino.instance.spawnManager.recycleObject[right.id].Count == 0) {
            this.right = Instantiate(right.gameObject, rightPosition, Quaternion.identity).GetComponent<RoadObject>();
        } else {
            this.right = Gino.instance.spawnManager.recycleObject[right.id][0];
            Gino.instance.spawnManager.recycleObject[right.id].RemoveAt(0);
            this.right.transform.position = rightPosition;
            this.right.gameObject.SetActive(true);
        }
        this.right.transform.SetParent(transform);
        this.left.transform.eulerAngles = new Vector3(0, 0, 0);
        this.center.transform.eulerAngles = new Vector3(0, 0, 0);
        this.right.transform.eulerAngles = new Vector3(0, 0, 0);
        if (this.left.GetComponent<Rigidbody>()) {
        this.left.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        if (this.center.GetComponent<Rigidbody>()) {
            this.center.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        if (this.right.GetComponent<Rigidbody>()) {
            this.right.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        id = 0;
        if (left.type != SpawnManager.RoadObjectIdentity.VEHICULE && left.type != SpawnManager.RoadObjectIdentity.SLOW) {
            id++;
        }
        if (center.type != SpawnManager.RoadObjectIdentity.VEHICULE && center.type != SpawnManager.RoadObjectIdentity.SLOW) {
            id += 2;
        }
        if (right.type != SpawnManager.RoadObjectIdentity.VEHICULE && right.type != SpawnManager.RoadObjectIdentity.SLOW) {
            id += 4;
        }
    }
}
