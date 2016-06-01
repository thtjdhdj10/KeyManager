using UnityEngine;
using System.Collections.Generic;

public class ControlableUnit : MonoBehaviour {

    public void ReceiveCommand(KeyManager.KeyNumber command, KeyManager.KeyPressType type)
    {
        // 눌린 키와 대응되는 명령 command 와 눌리는 방식 type 에 대응되는 행동을 정의할 것.

        if (type == KeyManager.KeyPressType.PRESS)
            return;

        Debug.Log(gameObject.name + "->" + command + " " + type);
    }



}
