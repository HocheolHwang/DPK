# Unity

### Version

- **게임 엔진**: Unity `2022.3.20f1`
- **실시간 통신**: PUN `2.45 - FREE`

### 실행 방법

- 유니티 Editor에서 빌드
    - Login, Opening, Lobby, StarShardPlain, ForgottenTemple, SeaOfAbyss Scene 선택해서 빌드

# 인프라

- **웹 서버**: Nginx
- **가상화** : Docker
- **CI/CD** : Jenkins

# Back-end

### Version

- **런타임 환경:** Node.js `20.11.0`
- **언어**: Java 17
- **패키지 매니저**: gradle
- **프레임워크**: Spring boot
- **DB 모델링**
    - **ORM 라이브러리**: Spring Data JPA

### 실행 방법

```sh
gradlew build
sudo nohup java -jar -Dprofile=prod /home/ubuntu/back/back-0.0.1-SNAPSHOT.jar > /home/ubuntu/back/log/back.log 2>&1 &
```

```java
//application.yml
spring:
    application:
        name: back
    profiles:
        active: ${profile}

```
```java
//application-local.yml
secret-key: ####
spring:
    datasource:
        url: jdbc:mysql://###/e207?serverTimezone=Asia/Seoul
        username: ####
        password: ####
    jpa:
        show-sql: true
        hibernate:
            ddl-auto: update

```

# DB

- **게임 데이터 DB**: MySQL

# Front-end

### Version

- **런타임 환경:** Node.js `20.11.0`
- **프레임워크**: React.js `18.2.0`

### 실행 방법

```csharp
npm install
npm run build
```

# Photon Server Setting

1. https://www.photonengine.com/
2. 로그인 후 Create New Application
3. Multiplayer Game을 선택하고 Select Photon SDK 를 Pun으로 설정
4. 프로젝트 생성 AppId 복사
5. 유니티 -> 포톤 유니티 네트워킹 -> PUN Wizard -> Setup Project -> 복사한 AppId 입력 후 Setup Project

# 시연 시나리오

1. 게임 접속


![image.png](./GameImg/LogInGIF.gif)

2. 로비 입장


![image.png](./GameImg/LobbyGIF.gif)

3. 캐릭터 창


![image.png](./GameImg/ClassGIF.gif)

4. 스킬 설명


![image.png](./GameImg/SkillInfoGIF.gif)

5. 스킬 슬롯 설명


![image.png](./GameImg/SkillGIF.gif)

6. 던전 입장


![image.png](./GameImg/DungeonSelectGIF.gif)

7. 1스테이지 ~ 3스테이지 진행


![image.png](./GameImg/MonsterGIF.gif)

8. 랭킹 설명


![image.png](./GameImg/RankingGIF.gif)

9. 시연 종료
