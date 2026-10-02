using UnityEngine;
using System.Collections;

namespace SpatialPartitionPattern {
    public class Soldier {
        public GameObject soldierObj;
        public MeshRenderer soldierMeshRenderer;
        public Transform soldierTrans;
        protected float walkSpeed;
        public Soldier previousSoldier;
        public Soldier nextSoldier;
        public virtual void Move(){}

        //The friendly has to move which soldier is the closest
        public virtual void Move(Soldier soldier){}
    }
}