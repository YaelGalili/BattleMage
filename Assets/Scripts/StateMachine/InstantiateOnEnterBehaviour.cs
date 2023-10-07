using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateOnEnterBehaviour : StateMachineBehaviour {
    [SerializeField] GameObject prefab;
    [SerializeField] Vector3 prefabPosition;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Instantiate(prefab, prefabPosition, prefab.transform.rotation);
    }

}
