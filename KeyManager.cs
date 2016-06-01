using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using DicKeyNumber = System.Collections.Generic.Dictionary<int,
System.Collections.Generic.Dictionary<UnityEngine.KeyCode,
    KeyManager.KeyNumber>>;

public class KeyManager : MonoBehaviour {
    
    private int keySetNumber = 0;
    private int keySetCount = 0;

    public static DicKeyNumber keySettings = new DicKeyNumber();

    //

    private delegate bool GetKeyEachType(KeyCode kc);
    
    private Dictionary<KeyPressType, GetKeyEachType> GetKeyFunctions = new Dictionary<KeyPressType, GetKeyEachType>();

    public enum KeySetName
    {
        DNF,
        FPS,
        V_CODE,
        STARCRAFT,
        LOL,
        COUNT,
    }

    public enum KeyPressType
    {
        DOWN = 0,
        UP,
        PRESS,
    }

    private void GetFunctionMatch()
    {
        GetKeyFunctions[KeyPressType.DOWN] = Input.GetKeyDown;
        GetKeyFunctions[KeyPressType.UP] = Input.GetKeyUp;
        GetKeyFunctions[KeyPressType.PRESS] = Input.GetKey;
    }

    //

    void Awake()
    {
        GetFunctionMatch();

    }

    void Start()
    {
        // 임시로 V_CODE 에 해당하는 KeySetting 사용

        int number = CreateKeySettings(GetDefaultKeySetting(KeySetName.V_CODE));
        SetKeySetting(number);
    }

    void Update()
    {
        GiveCommand();

    }

    // 사용중인 key 값으로 dictionary 순회.
    // KeyCode 와 매칭되는 KeyNumber 가 전달된다.
    void GiveCommand()
    {
        // ControlableUnit Component 를 가진 모든 GameObject 를 찾는다.
        List<GameObject> controlableUnitList = new List<GameObject>();

        ControlableUnit[] unitArr = FindObjectsOfType<ControlableUnit>();
        for (int i = 0; i < unitArr.Length; ++i)
        {
            controlableUnitList.Add(unitArr[i].gameObject);
        }
        // 매번 찾는건 비효율적이니, ControlableUnit 가 추가될 때 ObjectPooler 에서 관리하도록 하고,
        // 그것들을 가져와서 사용하는 것으로 수정.
//               VEasyPoolerManager.RefObjectListAtLayer(LayerManager.StringToMask("Controlable"));

        if (controlableUnitList.Count == 0)
            return;

        List<KeyCode> keyCodeList = keySettings[keySetNumber].Keys.ToList();

        // 임의의 KeySetting 의 모든 KeyCode 에 대해 
        for (int i = 0; i < keyCodeList.Count; ++i)
        {
            KeyCode keyCode = keyCodeList[i];
            
            // 어떤 KeyPressType 으로 눌리고 있는지에 대해 검사한다.
            for (int t = 0; t < GetKeyFunctions.Count; ++t)
            {
                KeyPressType pressType = (KeyPressType)t;

                if (GetKeyFunctions[pressType](keyCode) == false)
                    continue;

                // 모든 ControlableUnit 에게 <command, pressType> 을 넘겨준다.
                for (int j = 0; j < controlableUnitList.Count; ++j)
                {
                    var controler = controlableUnitList[j].GetComponent<ControlableUnit>();
                    if (controler == null)
                        continue;

                    KeyNumber command = keySettings[keySetNumber][keyCode];

                    controler.ReceiveCommand(command, pressType);
                }
            }
        }

    }

    int CreateKeySettings(Dictionary<KeyCode, KeyNumber> keySet)
    {
        keySettings[keySetCount] = keySet;

        keySetCount++;

        return keySetCount - 1;
    }

    void EditKeySettings(Dictionary<KeyCode, KeyNumber> keySet, int idx)
    {
        if (idx < 0 || idx >= keySetCount)
            return;

        keySettings[idx] = keySet;
    }

    void SetKeySetting(int number)
    {
        if (number < 0 || number >= keySetCount)
            return;

        keySetNumber = number;
    }

    public enum KeyNumber
    {
        NONE = 0,

        MOVE_LEFT,
        MOVE_RIGHT,
        MOVE_UP,
        MOVE_DOWN,

        SKILL_01,
        SKILL_02,
        SKILL_03,
        SKILL_04,
        SKILL_05,
        SKILL_06,
        SKILL_07,
        SKILL_08,
        SKILL_09,
        SKILL_10,
        SKILL_11,
        SKILL_12,

        ITEM_1,
        ITEM_2,
        ITEM_3,
        ITEM_4,
        ITEM_5,
        ITEM_6,
        ITEM_7,
        ITEM_8,
        ITEM_9,

        COMMAND_SKILL,
        COMMAND_ATTACK,
        COMMAND_JUMP,

        COMMAND_SPECIAL,
        COMMAND_RELOAD,
        COMMAND_SWAP,

        COMMAND_ZOOM,
        COMMAND_VIEW_ME,
        COMMAND_SIT,

        COMMAND_STOP,
        COMMAND_HOLD,
        COMMAND_MOVE,

        COMMAND_APPLY,
        COMMAND_MOVE_APPLY,

    }

    Dictionary<KeyCode, KeyNumber> GetDefaultKeySetting(KeySetName targetKeySet)
    {
        var ret = new Dictionary<KeyCode, KeyNumber>();

        switch (targetKeySet)
        {
        case KeySetName.DNF:
        {
            ret[KeyCode.LeftArrow] = KeyNumber.MOVE_LEFT;
            ret[KeyCode.RightArrow] = KeyNumber.MOVE_RIGHT;
            ret[KeyCode.UpArrow] = KeyNumber.MOVE_UP;
            ret[KeyCode.DownArrow] = KeyNumber.MOVE_DOWN;

            ret[KeyCode.A] = KeyNumber.SKILL_01;
            ret[KeyCode.S] = KeyNumber.SKILL_02;
            ret[KeyCode.D] = KeyNumber.SKILL_03;
            ret[KeyCode.F] = KeyNumber.SKILL_04;
            ret[KeyCode.G] = KeyNumber.SKILL_05;
            ret[KeyCode.H] = KeyNumber.SKILL_06;
            ret[KeyCode.Q] = KeyNumber.SKILL_07;
            ret[KeyCode.W] = KeyNumber.SKILL_08;
            ret[KeyCode.E] = KeyNumber.SKILL_09;
            ret[KeyCode.R] = KeyNumber.SKILL_10;
            ret[KeyCode.T] = KeyNumber.SKILL_11;
            ret[KeyCode.Y] = KeyNumber.SKILL_12;

            ret[KeyCode.Alpha1] = KeyNumber.ITEM_1;
            ret[KeyCode.Alpha2] = KeyNumber.ITEM_2;
            ret[KeyCode.Alpha3] = KeyNumber.ITEM_3;
            ret[KeyCode.Alpha4] = KeyNumber.ITEM_4;
            ret[KeyCode.Alpha5] = KeyNumber.ITEM_5;
            ret[KeyCode.Alpha6] = KeyNumber.ITEM_6;

            ret[KeyCode.Z] = KeyNumber.COMMAND_SKILL;
            ret[KeyCode.X] = KeyNumber.COMMAND_ATTACK;
            ret[KeyCode.C] = KeyNumber.COMMAND_JUMP;
            ret[KeyCode.Space] = KeyNumber.COMMAND_SPECIAL;
        }
        break;
        case KeySetName.FPS:
        {
            ret[KeyCode.W] = KeyNumber.MOVE_LEFT;
            ret[KeyCode.A] = KeyNumber.MOVE_RIGHT;
            ret[KeyCode.S] = KeyNumber.MOVE_UP;
            ret[KeyCode.D] = KeyNumber.MOVE_DOWN;

            ret[KeyCode.Space] = KeyNumber.COMMAND_JUMP;

            ret[KeyCode.R] = KeyNumber.COMMAND_RELOAD;
            ret[KeyCode.Q] = KeyNumber.COMMAND_SWAP;
            ret[KeyCode.Mouse0] = KeyNumber.COMMAND_ATTACK;
            ret[KeyCode.Mouse1] = KeyNumber.COMMAND_ZOOM;
            ret[KeyCode.LeftShift] = KeyNumber.COMMAND_SIT;

            ret[KeyCode.Alpha1] = KeyNumber.ITEM_1;
            ret[KeyCode.Alpha2] = KeyNumber.ITEM_2;
            ret[KeyCode.Alpha3] = KeyNumber.ITEM_3;
            ret[KeyCode.Alpha4] = KeyNumber.ITEM_4;
            ret[KeyCode.Alpha5] = KeyNumber.ITEM_5;
            ret[KeyCode.Alpha6] = KeyNumber.ITEM_6;
        }
        break;
        case KeySetName.LOL:
        {
            ret[KeyCode.Space] = KeyNumber.COMMAND_VIEW_ME;

            ret[KeyCode.Q] = KeyNumber.SKILL_01;
            ret[KeyCode.W] = KeyNumber.SKILL_02;
            ret[KeyCode.E] = KeyNumber.SKILL_03;
            ret[KeyCode.R] = KeyNumber.SKILL_04;

            ret[KeyCode.Alpha1] = KeyNumber.ITEM_1;
            ret[KeyCode.Alpha2] = KeyNumber.ITEM_2;
            ret[KeyCode.Alpha3] = KeyNumber.ITEM_3;
            ret[KeyCode.Alpha4] = KeyNumber.ITEM_4;
            ret[KeyCode.Alpha5] = KeyNumber.ITEM_5;
            ret[KeyCode.Alpha6] = KeyNumber.ITEM_6;

            ret[KeyCode.A] = KeyNumber.COMMAND_ATTACK;
            ret[KeyCode.M] = KeyNumber.COMMAND_MOVE;
            ret[KeyCode.S] = KeyNumber.COMMAND_STOP;

            ret[KeyCode.Mouse0] = KeyNumber.COMMAND_APPLY;
            ret[KeyCode.Mouse1] = KeyNumber.COMMAND_MOVE_APPLY;
        }
        break;
        case KeySetName.STARCRAFT:
        {
            ret[KeyCode.Space] = KeyNumber.COMMAND_VIEW_ME;

            ret[KeyCode.R] = KeyNumber.COMMAND_RELOAD;
            ret[KeyCode.A] = KeyNumber.COMMAND_ATTACK;
            ret[KeyCode.M] = KeyNumber.COMMAND_MOVE;
            ret[KeyCode.H] = KeyNumber.COMMAND_HOLD;
            ret[KeyCode.S] = KeyNumber.COMMAND_STOP;

            ret[KeyCode.Mouse0] = KeyNumber.COMMAND_APPLY;
            ret[KeyCode.Mouse1] = KeyNumber.COMMAND_MOVE_APPLY;

            ret[KeyCode.Alpha1] = KeyNumber.ITEM_1;
            ret[KeyCode.Alpha2] = KeyNumber.ITEM_2;
            ret[KeyCode.Alpha3] = KeyNumber.ITEM_3;
            ret[KeyCode.Alpha4] = KeyNumber.ITEM_4;
            ret[KeyCode.Alpha5] = KeyNumber.ITEM_5;
            ret[KeyCode.Alpha6] = KeyNumber.ITEM_6;
            ret[KeyCode.Alpha7] = KeyNumber.ITEM_7;
            ret[KeyCode.Alpha8] = KeyNumber.ITEM_8;
            ret[KeyCode.Alpha9] = KeyNumber.ITEM_9;
        }
        break;
        case KeySetName.V_CODE:
        {
            ret[KeyCode.W] = KeyNumber.MOVE_LEFT;
            ret[KeyCode.A] = KeyNumber.MOVE_RIGHT;
            ret[KeyCode.S] = KeyNumber.MOVE_UP;
            ret[KeyCode.D] = KeyNumber.MOVE_DOWN;

            ret[KeyCode.Mouse0] = KeyNumber.COMMAND_ATTACK;
            ret[KeyCode.Mouse1] = KeyNumber.COMMAND_SKILL;
            ret[KeyCode.Space] = KeyNumber.COMMAND_SPECIAL;

            ret[KeyCode.Alpha1] = KeyNumber.ITEM_1;
            ret[KeyCode.Alpha2] = KeyNumber.ITEM_2;
            ret[KeyCode.Alpha3] = KeyNumber.ITEM_3;
            ret[KeyCode.Alpha4] = KeyNumber.ITEM_4;
            ret[KeyCode.Alpha5] = KeyNumber.ITEM_5;
            ret[KeyCode.Alpha6] = KeyNumber.ITEM_6;
        }
        break;
        }

        return ret;
    }
}
