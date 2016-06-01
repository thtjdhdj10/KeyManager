using UnityEngine;
using System.Collections.Generic;
using System.Linq;

using DicKeyNumber = System.Collections.Generic.Dictionary<int,
System.Collections.Generic.Dictionary<UnityEngine.KeyCode,
    KeyManager.Command>>;

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

                    Command command = keySettings[keySetNumber][keyCode];

                    controler.ReceiveCommand(command, pressType);
                }
            }
        }

    }

    int CreateKeySettings(Dictionary<KeyCode, Command> keySet)
    {
        keySettings[keySetCount] = keySet;

        keySetCount++;

        return keySetCount - 1;
    }

    void EditKeySettings(Dictionary<KeyCode, Command> keySet, int idx)
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

    public enum Command
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

    Dictionary<KeyCode, Command> GetDefaultKeySetting(KeySetName targetKeySet)
    {
        var ret = new Dictionary<KeyCode, Command>();

        switch (targetKeySet)
        {
        case KeySetName.DNF:
        {
            ret[KeyCode.LeftArrow] = Command.MOVE_LEFT;
            ret[KeyCode.RightArrow] = Command.MOVE_RIGHT;
            ret[KeyCode.UpArrow] = Command.MOVE_UP;
            ret[KeyCode.DownArrow] = Command.MOVE_DOWN;

            ret[KeyCode.A] = Command.SKILL_01;
            ret[KeyCode.S] = Command.SKILL_02;
            ret[KeyCode.D] = Command.SKILL_03;
            ret[KeyCode.F] = Command.SKILL_04;
            ret[KeyCode.G] = Command.SKILL_05;
            ret[KeyCode.H] = Command.SKILL_06;
            ret[KeyCode.Q] = Command.SKILL_07;
            ret[KeyCode.W] = Command.SKILL_08;
            ret[KeyCode.E] = Command.SKILL_09;
            ret[KeyCode.R] = Command.SKILL_10;
            ret[KeyCode.T] = Command.SKILL_11;
            ret[KeyCode.Y] = Command.SKILL_12;

            ret[KeyCode.Alpha1] = Command.ITEM_1;
            ret[KeyCode.Alpha2] = Command.ITEM_2;
            ret[KeyCode.Alpha3] = Command.ITEM_3;
            ret[KeyCode.Alpha4] = Command.ITEM_4;
            ret[KeyCode.Alpha5] = Command.ITEM_5;
            ret[KeyCode.Alpha6] = Command.ITEM_6;

            ret[KeyCode.Z] = Command.COMMAND_SKILL;
            ret[KeyCode.X] = Command.COMMAND_ATTACK;
            ret[KeyCode.C] = Command.COMMAND_JUMP;
            ret[KeyCode.Space] = Command.COMMAND_SPECIAL;
        }
        break;
        case KeySetName.FPS:
        {
            ret[KeyCode.W] = Command.MOVE_LEFT;
            ret[KeyCode.A] = Command.MOVE_RIGHT;
            ret[KeyCode.S] = Command.MOVE_UP;
            ret[KeyCode.D] = Command.MOVE_DOWN;

            ret[KeyCode.Space] = Command.COMMAND_JUMP;

            ret[KeyCode.R] = Command.COMMAND_RELOAD;
            ret[KeyCode.Q] = Command.COMMAND_SWAP;
            ret[KeyCode.Mouse0] = Command.COMMAND_ATTACK;
            ret[KeyCode.Mouse1] = Command.COMMAND_ZOOM;
            ret[KeyCode.LeftShift] = Command.COMMAND_SIT;

            ret[KeyCode.Alpha1] = Command.ITEM_1;
            ret[KeyCode.Alpha2] = Command.ITEM_2;
            ret[KeyCode.Alpha3] = Command.ITEM_3;
            ret[KeyCode.Alpha4] = Command.ITEM_4;
            ret[KeyCode.Alpha5] = Command.ITEM_5;
            ret[KeyCode.Alpha6] = Command.ITEM_6;
        }
        break;
        case KeySetName.LOL:
        {
            ret[KeyCode.Space] = Command.COMMAND_VIEW_ME;

            ret[KeyCode.Q] = Command.SKILL_01;
            ret[KeyCode.W] = Command.SKILL_02;
            ret[KeyCode.E] = Command.SKILL_03;
            ret[KeyCode.R] = Command.SKILL_04;

            ret[KeyCode.Alpha1] = Command.ITEM_1;
            ret[KeyCode.Alpha2] = Command.ITEM_2;
            ret[KeyCode.Alpha3] = Command.ITEM_3;
            ret[KeyCode.Alpha4] = Command.ITEM_4;
            ret[KeyCode.Alpha5] = Command.ITEM_5;
            ret[KeyCode.Alpha6] = Command.ITEM_6;

            ret[KeyCode.A] = Command.COMMAND_ATTACK;
            ret[KeyCode.M] = Command.COMMAND_MOVE;
            ret[KeyCode.S] = Command.COMMAND_STOP;

            ret[KeyCode.Mouse0] = Command.COMMAND_APPLY;
            ret[KeyCode.Mouse1] = Command.COMMAND_MOVE_APPLY;
        }
        break;
        case KeySetName.STARCRAFT:
        {
            ret[KeyCode.Space] = Command.COMMAND_VIEW_ME;

            ret[KeyCode.R] = Command.COMMAND_RELOAD;
            ret[KeyCode.A] = Command.COMMAND_ATTACK;
            ret[KeyCode.M] = Command.COMMAND_MOVE;
            ret[KeyCode.H] = Command.COMMAND_HOLD;
            ret[KeyCode.S] = Command.COMMAND_STOP;

            ret[KeyCode.Mouse0] = Command.COMMAND_APPLY;
            ret[KeyCode.Mouse1] = Command.COMMAND_MOVE_APPLY;

            ret[KeyCode.Alpha1] = Command.ITEM_1;
            ret[KeyCode.Alpha2] = Command.ITEM_2;
            ret[KeyCode.Alpha3] = Command.ITEM_3;
            ret[KeyCode.Alpha4] = Command.ITEM_4;
            ret[KeyCode.Alpha5] = Command.ITEM_5;
            ret[KeyCode.Alpha6] = Command.ITEM_6;
            ret[KeyCode.Alpha7] = Command.ITEM_7;
            ret[KeyCode.Alpha8] = Command.ITEM_8;
            ret[KeyCode.Alpha9] = Command.ITEM_9;
        }
        break;
        case KeySetName.V_CODE:
        {
            ret[KeyCode.W] = Command.MOVE_LEFT;
            ret[KeyCode.A] = Command.MOVE_RIGHT;
            ret[KeyCode.S] = Command.MOVE_UP;
            ret[KeyCode.D] = Command.MOVE_DOWN;

            ret[KeyCode.Mouse0] = Command.COMMAND_ATTACK;
            ret[KeyCode.Mouse1] = Command.COMMAND_SKILL;
            ret[KeyCode.Space] = Command.COMMAND_SPECIAL;

            ret[KeyCode.Alpha1] = Command.ITEM_1;
            ret[KeyCode.Alpha2] = Command.ITEM_2;
            ret[KeyCode.Alpha3] = Command.ITEM_3;
            ret[KeyCode.Alpha4] = Command.ITEM_4;
            ret[KeyCode.Alpha5] = Command.ITEM_5;
            ret[KeyCode.Alpha6] = Command.ITEM_6;
        }
        break;
        }

        return ret;
    }
}
