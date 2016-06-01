## 개요

* 유니티에서 사용할 수 있는, C# 으로 개발한 키 입력 API 입니다.

---

## 기능

* 제공하는 KeySetting 을 사용하거나, 임의의 새 KeySetting 을 만들어 사용할 수 있습니다.
 기본으로 제공하는 형식은 타 게임의 키 입력 방식( DNF/FPS/Starcraft/LOL/ETC ) 이 해당됩니다.

* KeySetting 은 유니티에서 제공하는 keyCode 를 key 로 하고, 임의로 지정한 명령어들( 이동, 공격, 아이템 사용 등 )인 Command 를 value 로 하는 Dictionary<KeyCode, Command> 로 구성됩니다.

* 구체적으로 예를 들자면, 기본 제공 방식 중 하나인 LOL 을 선택했다고 가정했을 때.
 KeyCode.Q~R 이 각각 SKILL\_0~3 에 대응되고, KeyCode.Mouse0 은 COMMAND\_APPLY, KeyCode.Mouse1 은 COMMAND\_MOVE 와 대응됩니다.

* 물론 이 설정들은 실시간으로 수정하거나, 추가/제거 할 수도 있습니다.

* 구체적인 동작 방식은 다음과 같습니다.  
매 Update() 에서 임의의 KeySetting 의 모든 KeyCode 에 대해 다음의 작업을 수행.  
어떤식으로 눌리고 있는지 검사하여, 눌리고 있는 KeyCode 에 대응되는 임의의 명령을, 조작 가능한 모든 컴포넌트에게 전달.
