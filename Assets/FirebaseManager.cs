using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System;
using UnityEngine.Advertisements;
using Object = System.Object;

namespace DK
{
    public class FirebaseManager : MonoBehaviour, IUnityAdsInitializationListener
    {
        [Header("Ads Initializer")]
        [SerializeField] string androidId;
        [SerializeField] string IOSId;
        [SerializeField] bool test = true;
        [SerializeField] RewardedAdsButton rewardedAdsButton;
        public bool isInitialized = false;
        private string gameId;
        [Header("Fire Base Aauthorization Variables")]
        public DependencyStatus dependencyStatus;
        public FirebaseAuth auth;
        public FirebaseUser User;

        [Header("Login Text Fields")]
        public TMP_InputField emailLoginField;
        public TMP_InputField passwordLoginField;


        [Header("Register User TextFields")]
        public TMP_InputField userNameRegisterField;
        public TMP_InputField emailRegisterField;
        public TMP_InputField passwordRegisterField;

        [Header("Popups")]
        public GameObject warningPopup;
        public GameObject successPopup;
        public GameObject successPopupRegistration;
        public GameObject LeaderBoardSaveLoading;
        public GameObject LeaderBoardSaveDataPopup;
        public GameObject leaderBoardWarningPopup;
        public GameObject SignOutLoadingPopup;
        //public GameObject networkErrorPopup;
        public GameObject registerPopup;
        public GameObject loginPopup;
        [Header("Buttons")]
        public GameObject buttonRegisterOnTitleScreen;
        public GameObject buttonLoginOnTitleScreen;
        [Header("Scenes Panels")]
        public GameObject homeScene;
        public GameObject titleLoginScene;
        public GameObject titleTaptoStart;
        [Header("Firebase Database")]
        public CharacterSaveData userData = new CharacterSaveData();
        public ItemsSaveData itemData = new ItemsSaveData();
        public List<LevelProgress> levelProgresses = new List<LevelProgress>();
        public DatabaseReference reference;

        [SerializeField] GameObject loadingPopup;
        [SerializeField] TextMeshProUGUI textForLoadingTitle;
        [SerializeField] TextMeshProUGUI usernameTextinHomeScene;

        public static FirebaseManager instance;

        Dictionary<string, Object> levels = new Dictionary<string, Object>();
        Dictionary<string, Object> equipment = new Dictionary<string, Object>();
        Dictionary<string, Object> gold = new Dictionary<string, Object>();
        Dictionary<string, Object> souls = new Dictionary<string, Object>();
        [Header("Leaderboard UI")]
        public LeaderBoard myLeaderBoardData = new LeaderBoard();
        public LeaderBoard userLeaderBoard = new LeaderBoard();

        public List<LeaderBoard> leaderBoardList = new();
        public TextMeshProUGUI[] nameInLeaderboard= new TextMeshProUGUI[10];
        public TextMeshProUGUI[] deathsInLeaderboard= new TextMeshProUGUI[10];

        public TextMeshProUGUI userNameinLeaderboard;
        public TextMeshProUGUI userDeathsinLeaderboard;
        public TextMeshProUGUI userRankinLeaderboard;
        [Header("Daily reward")]
        public DailyRewardSave userDailyRewardsClaimed;
        public GameObject timeErrorPopup;
        

        public long timeMilliseconds;
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(instance);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }

            
            
        }
        private void Start()
        {
            InitializeAds();
            StartCoroutine(FirebaseDependancyCoroutineCaller());
            
        }

        private IEnumerator FirebaseDependancyCheck()
        {
            var task = FirebaseApp.CheckAndFixDependenciesAsync();
            yield return new WaitUntil(predicate: () => task.IsCompleted);
            dependencyStatus = task.Result;
            if(dependencyStatus == DependencyStatus.Available)
            {
                InitializeFireBase();
                Debug.Log("Dependancy OK");
            }
            else
            {
                Debug.LogError("Could not resolve all FireBase Dependencies" + dependencyStatus);
            }
        }

        public IEnumerator FirebaseDependancyCoroutineCaller()
        {
            yield return StartCoroutine(FirebaseDependancyCheck());
        }
        public IEnumerator requestTime()
        {
            var response = UnityWebRequest.Get("http://worldtimeapi.org/api/timezone/Europe/London");

            

                yield return response.SendWebRequest();
            if (response.error != null)
            {
                timeErrorPopup.SetActive(true);
                Debug.LogWarning("Something went wrong");
                timeMilliseconds = 0;
            }
            else
            {
                string json = response.downloadHandler.text;
                WorldTimeResponse timeResponse = JsonUtility.FromJson<WorldTimeResponse>(json);
                DateTime internetTime = DateTime.Parse(timeResponse.utc_datetime);
                timeMilliseconds = new DateTimeOffset(internetTime).ToUnixTimeMilliseconds();
                
            }
        }

        public void RequestTimeCoroutineCaller()
        {
            StartCoroutine(requestTime());
        }
        private void InitializeFireBase()
        {
            auth = FirebaseAuth.DefaultInstance;

            StartCoroutine(CheckAutoLogin());
            auth.StateChanged += AuthStateChanged;
            AuthStateChanged(this, null);
            


            reference = FirebaseDatabase.DefaultInstance.RootReference;

        }
        private void AuthStateChanged(Object sender, System.EventArgs eventArgs)
        {
            if(auth.CurrentUser != User)
            {
                bool signedIn = User != auth.CurrentUser && auth.CurrentUser != null;

                if(!signedIn && User != null)
                {
                    Debug.Log("Signed Out");
                }

                User = auth.CurrentUser;

                if (signedIn)
                {
                    Debug.Log("Signed in");
                }
            }
        }

        private IEnumerator CheckAutoLogin()
        {
            yield return new WaitForEndOfFrame();
            Debug.Log("Auto login Check");
            if (User != null)
            {
                Debug.Log("We auto logging");
                var reloadTask = User.ReloadAsync();

                yield return new WaitUntil(predicate: () => reloadTask.IsCompleted);


                
                AutoLogin();
            }
            else
            {
                Debug.Log("We not auto logging");
                titleLoginScene.SetActive(true);
            }
        }

        public void onTapStart()
        {
            titleTaptoStart.SetActive(false);
            homeScene.SetActive(true);
        }
        private void AutoLogin()
        {
            if (User != null)
            {
                GetDataFromDatabase();
                GetItemDataCoroutineCaller();
                GetRewardsCoroutineCaller();
                GetLevelProgressDataCoroutineCaller();
                titleLoginScene.SetActive(false);
                titleTaptoStart.SetActive(true);
                usernameTextinHomeScene.text = User.DisplayName;
            }
            else
            {
                homeScene.SetActive(false);
                titleLoginScene.SetActive(true);
            }
        }

        public void LoginButton()
        {
            StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
        }
        public void RegisterButton()
        {
            StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, userNameRegisterField.text));
        }
        public void GuestLogin()
        {
            StartCoroutine(Anonymous());
        }
        private IEnumerator Anonymous()
        {
            var loginTask = auth.SignInAnonymouslyAsync();
            yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

            if (loginTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed To Register tassk with {loginTask.Exception}");
                warningPopup.SetActive(true);
                warningPopup.GetComponentInChildren<TextMeshProUGUI>().text = "Cannot Create Guest At the Moment Pleasase Register";
            }
            else
            {
                successPopup.SetActive(true);
                successPopup.GetComponentInChildren<TextMeshProUGUI>().text = "Guest Created Succesfully";
            }
            User = auth.CurrentUser;
        }
        private IEnumerator Login(string email, string password)
        {
            var LoginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
            loadingPopup.SetActive(true);
            textForLoadingTitle.text = "Logging In";
            yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
            loadingPopup.SetActive(false);
            if (LoginTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed To Register tassk with {LoginTask.Exception}");
                FirebaseException firebaseException = LoginTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseException.ErrorCode;

                string message = "Login failed";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WrongPassword:
                        message = "Passwrod/email is incorrect";
                        break;
                    case AuthError.InvalidEmail:
                        message = "Passwrod/email is incorrect";
                        break;
                    case AuthError.UserNotFound:
                        message = "User does not exxist Please Register";
                        break;
                    case AuthError.NetworkRequestFailed:
                        //networkErrorPopup.SetActive(true);
                        break;
                }
                warningPopup.SetActive(true);
                warningPopup.GetComponentInChildren<TextMeshProUGUI>().text = message;
            }
            else
            {
                User = auth.CurrentUser;
                Debug.LogFormat("User signed in succesfully:{0} ({1})", User.DisplayName, User.Email, User.UserId);
                successPopup.SetActive(true);
                successPopup.GetComponentInChildren<TextMeshProUGUI>().text = "Login Succesfully";
                emailLoginField.text = "";
                passwordLoginField.text = "";
                loginPopup.SetActive(false);
                GetDataFromDatabase();
                GetItemDataCoroutineCaller();
                GetRewardsCoroutineCaller();
                GetLevelProgressDataCoroutineCaller();

            }
        }
        public void onSucessClick()
        {
            titleLoginScene.SetActive(false);
            homeScene.SetActive(true);
            usernameTextinHomeScene.text = User.DisplayName;
            successPopup.SetActive(false);
        }

        private IEnumerator Register(string email, string password, string username)
        {
            if (username == "")
            {
                warningPopup.SetActive(true);
                warningPopup.GetComponentInChildren<TextMeshProUGUI>().text = "Missing Username";
            }
            if (email == "")
            {
                warningPopup.SetActive(true);
                warningPopup.GetComponentInChildren<TextMeshProUGUI>().text = "Missing Email";
            }
            else
            {
                var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
                loadingPopup.SetActive(true);
                textForLoadingTitle.text = "Registering User";
                yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);
                loadingPopup.SetActive(false);
                if (RegisterTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to registertask with{RegisterTask.Exception}");
                    FirebaseException firebaseException = RegisterTask.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseException.ErrorCode;

                    string message = "Register Failed";
                    switch (errorCode)
                    {
                        case AuthError.MissingEmail:
                            message = "Missing Email";
                            break;
                        case AuthError.MissingPassword:
                            message = "Missing Password";
                            break;
                        case AuthError.WeakPassword:
                            message = "Weak Password";
                            break;
                        case AuthError.EmailAlreadyInUse:
                            message = "Email Already In Use";
                            break;
                    }
                    warningPopup.SetActive(true);
                    warningPopup.GetComponentInChildren<TextMeshProUGUI>().text = message;
                }
                else
                {
                    User = auth.CurrentUser;
                    if (User != null)
                    {
                        UserProfile profile = new UserProfile { DisplayName = username };
                        var profileTask = User.UpdateUserProfileAsync(profile);

                        yield return new WaitUntil(predicate: () => profileTask.IsCompleted);

                        if (profileTask.Exception != null)
                        {
                            Debug.LogWarning(message: $"Failed to register task with {profileTask.Exception}");
                            FirebaseException firebaseEx = profileTask.Exception.GetBaseException() as FirebaseException;
                            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                            warningPopup.SetActive(true);
                            warningPopup.GetComponentInChildren<TextMeshProUGUI>().text = "Username Set Failed!";
                        }
                        else
                        {
                            successPopupRegistration.SetActive(true);
                            successPopupRegistration.GetComponentInChildren<TextMeshProUGUI>().text = "Registered Succesfully";
                            loginPopup.SetActive(false);
                            registerPopup.SetActive(false);
                            buttonLoginOnTitleScreen.SetActive(true);
                            buttonRegisterOnTitleScreen.SetActive(true);
                            emailRegisterField.text = "";
                            userNameRegisterField.text = "";
                            passwordRegisterField.text = "";
                            registerPopup.SetActive(false);

                        }
                    }
                }
            }
        }
        private IEnumerator SaveDataToFirebase()
        {
            userData.characterName = User.DisplayName;
            userData.Email = User.Email;
            string json = JsonUtility.ToJson(userData);
            User = auth.CurrentUser;
            var saveDataTask = reference.Child("Users").Child(User.UserId).SetRawJsonValueAsync(json);

            yield return new WaitUntil(predicate: () => saveDataTask.IsCompleted);

            if (saveDataTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to Save Data Task with{saveDataTask.Exception}");
            }
            else
            {
                Debug.Log("Login Succesfully");
            }
        }

        private IEnumerator SaveLeaderboardData()
        {
            if (userData.deathCount <= 0)
            {
                leaderBoardWarningPopup.SetActive(true);

            }
            else
            {
                User = auth.CurrentUser;
                myLeaderBoardData.uid = User.UserId;
                myLeaderBoardData.name = User.DisplayName;
                myLeaderBoardData.deaths = userData.deathCount;
                myLeaderBoardData.timestamp = ServerValue.Timestamp;
                string json = JsonUtility.ToJson(myLeaderBoardData);
                var saveDataTask = reference.Child("Leaderboard").Child(User.UserId).SetRawJsonValueAsync(json);
                LeaderBoardSaveLoading.SetActive(true);
                yield return new WaitUntil(predicate: () => saveDataTask.IsCompleted);
                LeaderBoardSaveLoading.SetActive(false);
                if (saveDataTask.Exception != null)
                {
                    leaderBoardWarningPopup.SetActive(true);
                    Debug.LogWarning(message: $"Failed to save leaderboard with {saveDataTask.Exception}");
                }
                else
                {
                    GetLeaderBoardDataCoroutineCaller();
                }
            }

        }

        private IEnumerator GetLeaderBoardData()
        {
            User = auth.CurrentUser;
            var getDataTask = reference.Child("Leaderboard").OrderByChild("deaths").GetValueAsync();
            LeaderBoardSaveLoading.SetActive(true);
            yield return new WaitUntil(predicate: () => getDataTask.IsCompleted);
            LeaderBoardSaveLoading.SetActive(false);
            if (getDataTask.Exception != null)
            {
                leaderBoardWarningPopup.SetActive(true);
                Debug.LogWarning(message: $"Failed retreiving leaderboard data with{getDataTask.Exception}");
            }
            else
            {
                LeaderBoardSaveLoading.SetActive(true);
                int i = 0;
                DataSnapshot snapshot = getDataTask.Result;
                LeaderBoard temp = new LeaderBoard();
                foreach (DataSnapshot snap in snapshot.Children)
                {
                    temp = JsonUtility.FromJson<LeaderBoard>(snap.GetRawJsonValue());
                    temp.rank = i + 1;
                    leaderBoardList.Add(temp);
                    i++;
                }
                LeaderBoardSaveLoading.SetActive(false);
                SetLeaderboardData();
            }
        }

        public void GetLeaderBoardDataCoroutineCaller()
        {
            StartCoroutine(GetLeaderBoardData());
        }

        public void SaveleaderBoardCoroutineCaller()
        {
            StartCoroutine(SaveLeaderboardData());
        }

        private IEnumerator SaveItemDataToFirebase()
        {
            string json = JsonUtility.ToJson(itemData);
            User = auth.CurrentUser;
            var saveItemDataTask = reference.Child("Items").Child(User.UserId).SetRawJsonValueAsync(json);
            yield return new WaitUntil(predicate: () => saveItemDataTask.IsCompleted);
            if (saveItemDataTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed To add item data tast with{saveItemDataTask.Exception}");
            }
            else
            {
                Debug.Log("Item data saved Succesfully");
            }
        }

        public void SaveItemDataCoroutineCaller()
        {
            StartCoroutine(SaveItemDataToFirebase());
        }

        public void StartSaveDataCoroutine()
        {
            StartCoroutine(SaveDataToFirebase());
        }

        private IEnumerator UpdatePlayerLevelsToFireBase(int healthLevel, int staminaLevel, int focusLevel, int strengthLevel,
            int dexterityLevel, int poiseLevel, int intelligenceLevel, int faithLevel, int soulPlayersPosseses, int characterLevel)
        {
            levels["soulPlayersPosseses"] = soulPlayersPosseses;
            levels["characterLevel"] = characterLevel;
            levels["healthLevel"] = healthLevel;
            levels["staminaLevel"] = staminaLevel;
            levels["focusLevel"] = focusLevel;
            levels["strengthLevel"] = strengthLevel;
            levels["dexterityLevel"] = dexterityLevel;
            levels["poiseLevel"] = poiseLevel;
            levels["intelligenceLevel"] = intelligenceLevel;
            levels["faithLevel"] = faithLevel;
            User = auth.CurrentUser;

            var updateTask = reference.Child("Users").Child(User.UserId).UpdateChildrenAsync(levels);

            yield return new WaitUntil(predicate: () => updateTask.IsCompleted);


            if (updateTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to update Task with{updateTask.Exception}");
            }
            else
            {
                Debug.Log("Updated values Succesfully");
            }


        }

        public void UpdateLevels(int healthLevel, int staminaLevel, int focusLevel, int strengthLevel,
            int dexterityLevel, int poiseLevel, int intelligenceLevel, int faithLevel, int soulPlayersPosseses, int characterLevel)
        {
            StartCoroutine(UpdatePlayerLevelsToFireBase(healthLevel, staminaLevel, focusLevel, strengthLevel,
            dexterityLevel, poiseLevel, intelligenceLevel, faithLevel, soulPlayersPosseses, characterLevel));
        }

        private IEnumerator UpdatePlayerEquipmentToFireBase()
        {
            equipment["leftArmWeapon"] = userData.leftArmWeapon;
            equipment["rightArmWeapon"] = userData.rightArmWeapon;
            equipment["helmetIndex"] = userData.helmetIndex;
            equipment["torsoIndex"] = userData.torsoIndex;
            equipment["armIndex"] = userData.armIndex;
            equipment["hipIndex"] = userData.hipIndex;
            User = auth.CurrentUser;

            var EquipmentTask = reference.Child("Users").Child(User.UserId).UpdateChildrenAsync(equipment);

            yield return new WaitUntil(predicate: () => EquipmentTask.IsCompleted);

            if (EquipmentTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed Equipment Loading with {EquipmentTask.Exception}");
            }
            else
            {
                Debug.Log("Task Completed Successfully");
            }

        }

        public void UpdatePlayerEquipment()
        {
            StartCoroutine(UpdatePlayerEquipmentToFireBase());
        }

        private IEnumerator GetDataFromFireBase()
        {
            User = auth.CurrentUser;
            var dbTask = reference.Child("Users").Child(User.UserId).GetValueAsync();

            yield return new WaitUntil(predicate: () => dbTask.IsCompleted);

            if (dbTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed Task {dbTask.Exception}");
            }
            else if (dbTask.Result.Value == null)
            {
                StartSaveDataCoroutine();
            }
            else
            {
                DataSnapshot snapshot = dbTask.Result;
                userData.characterName = snapshot.Child("characterName").Value.ToString();
                userData.Email = snapshot.Child("Email").Value.ToString();
                userData.characterLevel = Convert.ToInt32(snapshot.Child("characterLevel").Value);
                userData.helmetIndex = Convert.ToInt32(snapshot.Child("helmetIndex").Value);
                userData.torsoIndex = Convert.ToInt32(snapshot.Child("torsoIndex").Value);
                userData.armIndex = Convert.ToInt32(snapshot.Child("armIndex").Value);
                userData.hipIndex = Convert.ToInt32(snapshot.Child("hipIndex").Value);
                userData.consumableItemIndex = Convert.ToInt32(snapshot.Child("consumableItemIndex").Value);
                userData.leftArmWeapon = Convert.ToInt32(snapshot.Child("leftArmWeapon").Value);
                userData.rightArmWeapon = Convert.ToInt32(snapshot.Child("rightArmWeapon").Value);
                userData.healthLevel = Convert.ToInt32(snapshot.Child("healthLevel").Value);
                userData.staminaLevel = Convert.ToInt32(snapshot.Child("staminaLevel").Value);
                userData.focusLevel = Convert.ToInt32(snapshot.Child("focusLevel").Value);
                userData.strengthLevel = Convert.ToInt32(snapshot.Child("strengthLevel").Value);
                userData.dexterityLevel = Convert.ToInt32(snapshot.Child("dexterityLevel").Value);
                userData.poiseLevel = Convert.ToInt32(snapshot.Child("poiseLevel").Value);
                userData.intelligenceLevel = Convert.ToInt32(snapshot.Child("intelligenceLevel").Value);
                userData.faithLevel = Convert.ToInt32(snapshot.Child("faithLevel").Value);
                userData.soulPlayersPosseses = Convert.ToInt32(snapshot.Child("soulPlayersPosseses").Value);
                userData.goldAmount = Convert.ToInt32(snapshot.Child("goldAmount").Value);
            }
        }

        public void GetDataFromDatabase()
        {
            StartCoroutine(GetDataFromFireBase());

        }

        private IEnumerator GetItemDataFromFirebase()
        {
            User = auth.CurrentUser;
            var dbItemTask = reference.Child("Items").Child(User.UserId).GetValueAsync();

            yield return new WaitUntil(predicate: () => dbItemTask.IsCompleted);

            if (dbItemTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to complete get task with{dbItemTask.Exception}");
            }
            else if (dbItemTask.Result.Value == null)
            {
                itemData.helmetPurchased.Add(0);
                itemData.armsPurchased.Add(0);
                itemData.torsoPurchased.Add(0);
                itemData.legsPurchased.Add(0);
                itemData.leftWeaponsPurchased.Add(0);
                itemData.rightWeaponsPurchased.Add(0);
                itemData.rightWeaponsPurchased.Add(1);
                itemData.leftWeaponsPurchased.Add(1);
                SaveItemDataCoroutineCaller();
            }
            else
            {

                Debug.Log("Start Getting Data");
                DataSnapshot snapshot = dbItemTask.Result;

                string json = snapshot.GetRawJsonValue();
                itemData = JsonUtility.FromJson<ItemsSaveData>(json);
                Debug.Log("Got DATA");




            }
        }

        public void GetItemDataCoroutineCaller()
        {
            StartCoroutine(GetItemDataFromFirebase());
        }

        public void SetLeaderboardData()
        {
            bool found = false;
            for (int i = 0; i < leaderBoardList.Count; i++)
            {
                if (leaderBoardList[i].uid == User.UserId)
                {
                    userLeaderBoard.rank = leaderBoardList[i].rank;
                    userLeaderBoard.name = leaderBoardList[i].name;
                    userLeaderBoard.deaths = leaderBoardList[i].deaths;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                LeaderBoardSaveDataPopup.SetActive(true);
            }
            else
            {
                userRankinLeaderboard.text = userLeaderBoard.rank.ToString();
                userNameinLeaderboard.text = userLeaderBoard.name;
                userDeathsinLeaderboard.text = userLeaderBoard.deaths.ToString();
            }
            if (leaderBoardList.Count >= 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    nameInLeaderboard[i].text = leaderBoardList[i].name;
                    deathsInLeaderboard[i].text = leaderBoardList[i].deaths.ToString();
                }
            }
            else
            {
                for (int i = 0; i < leaderBoardList.Count; i++)
                {
                    nameInLeaderboard[i].text = leaderBoardList[i].name;
                    deathsInLeaderboard[i].text = leaderBoardList[i].deaths.ToString();
                }
            }


        }

        private IEnumerator SaveDailyRewardsBoolFirstEntry()
        {
            for(int i = 0; i < 7; i++)
            {
                if(i == 0)
                {
                    userDailyRewardsClaimed.rewardsCollected.Add(true);
                }
                userDailyRewardsClaimed.rewardsCollected.Add(false);
            }

            User = auth.CurrentUser;

            string json = JsonUtility.ToJson(userDailyRewardsClaimed);

            var task = reference.Child("DailyRewards").Child(User.UserId).SetRawJsonValueAsync(json);

            yield return new WaitUntil(predicate: () => task.IsCompleted);

            if (task.Exception != null)
            {
                Debug.LogWarning(message: $"Task Failed with{task.Exception}");
            }
            else
            {
                Debug.Log("Task Completed Successfully");
            }
            
        }

        public void  SaveRewardsCoroutineCaller()
        {
            StartCoroutine(SaveDailyRewardsBoolFirstEntry());
        }

        private IEnumerator SaveDailyRewardsBool()
        {
            User = auth.CurrentUser;
            string json = JsonUtility.ToJson(userDailyRewardsClaimed);
            var task = reference.Child("DailyRewards").Child(User.UserId).SetRawJsonValueAsync(json);

            yield return new WaitUntil(predicate: () => task.IsCompleted);

            if (task.Exception != null)
            {
                Debug.LogWarning(message: $"Task failed with{task.Exception}");
            }
            else
            {
                Debug.Log("Tasked completed successfully");
            }
        }

        public void SaveRewardsCoroutineCallerOverride()
        {
            StartCoroutine(SaveDailyRewardsBool());
        }
        private IEnumerator GetRewardsFromFireBase()
        {
            var task = reference.Child("DailyRewards").Child(User.UserId).GetValueAsync();

            yield return new WaitUntil(predicate: () => task.IsCompleted);

            if (task.Exception != null)
            {
                Debug.LogWarning(message: $"Task failed with{task.Exception}");
            }
            else if(task.Result.Value == null)
            {
                SaveRewardsCoroutineCaller();
            }
            else
            {
                DataSnapshot snapshot = task.Result;

                string json = snapshot.GetRawJsonValue();

                userDailyRewardsClaimed = JsonUtility.FromJson<DailyRewardSave>(json);

            }
        }

        public void GetRewardsCoroutineCaller()
        {
            StartCoroutine(GetRewardsFromFireBase());
        }

        private IEnumerator UpdateGoldCurrency(int goldAmount)
        {
            goldAmount += userData.goldAmount;
            gold["goldAmount"] = goldAmount;
            User = auth.CurrentUser;

            var task = reference.Child("Users").Child(User.UserId).UpdateChildrenAsync(gold);

            yield return new WaitUntil(predicate: () => task.IsCompleted);
            if (task.Exception != null)
            {
                Debug.LogWarning(message: $"Task failed with{task.Exception}");
            }
            else
            {
                Debug.Log("Task Success");
            }
        }

        public void UpdateGold(int goldAmount)
        {
            StartCoroutine(UpdateGoldCurrency(goldAmount));
        }

        private IEnumerator UpdateSoulsCurrency(int soulsPlayerPosseses)
        {
            soulsPlayerPosseses += userData.soulPlayersPosseses;
            souls["soulPlayersPosseses"] = soulsPlayerPosseses;
            User = auth.CurrentUser;

            var task = reference.Child("Users").Child(User.UserId).UpdateChildrenAsync(souls);

            yield return new WaitUntil(predicate: () => task.IsCompleted);
            if (task.Exception != null)
            {
                Debug.LogWarning(message: $"Task failed with{task.Exception}");
            }
            else
            {
                Debug.Log("Task Success");
            }
        }

        public void UpdateSouls(int soulPlayersPosseses)
        {
            StartCoroutine(UpdateSoulsCurrency(soulPlayersPosseses));
        }

        private IEnumerator SetLevelDataAtFirstOccurence()
        {

            levelProgresses.Add(new LevelProgress(-1,0));
            string json = JsonUtility.ToJson(levelProgresses);
            var taskSetLevelData = reference.Child("LevelProgress").Child(User.UserId).SetRawJsonValueAsync(json);
            yield return new WaitUntil(predicate: () => taskSetLevelData.IsCompleted);

            if (taskSetLevelData.Exception != null)
            {
                Debug.LogWarning(message: $"Tak failed with{taskSetLevelData.Exception}");
            }
            else
            {
                Debug.Log("LEvelProgress added");
            }
        }

        public void LeveldataSetterCaller()
        {
            StartCoroutine(SetLevelDataAtFirstOccurence());
        }


        private IEnumerator SetLevelProgressData(int level,int stars)
        {
            levelProgresses.Add(new LevelProgress(level, stars));
            string json = JsonUtility.ToJson(levelProgresses);
            var taskSetLevelData = reference.Child("LevelProgress").Child(User.UserId).SetRawJsonValueAsync(json);
            yield return new WaitUntil(predicate: () => taskSetLevelData.IsCompleted);

            if (taskSetLevelData.Exception != null)
            {
                Debug.LogWarning(message: $"Task failed with{taskSetLevelData.Exception}");
            }
            else
            {
                Debug.Log("LevelProgress added");
            }
        }

        public void SetLevelProgressCoroutineCaller(int level,int stars)
        {
            StartCoroutine(SetLevelProgressData(level, stars));
        }
        private IEnumerator GetLevelProgressData()
        {
            var taskGetData = reference.Child("LevelProgress").Child(User.UserId).GetValueAsync();

            yield return new WaitUntil(predicate: () => taskGetData.IsCompleted);
            if (taskGetData.Exception != null)
            {
                Debug.LogWarning(message: $"Task failed With {taskGetData.Exception}");
            }
            else if (taskGetData.Result.Value == null)
            {
                LeveldataSetterCaller();
            }
            else
            {
                DataSnapshot snapshot = taskGetData.Result;

                LevelProgress temp = new LevelProgress();
                foreach (DataSnapshot snap in snapshot.Children)
                {
                    temp =JsonUtility.FromJson<LevelProgress>(snap.GetRawJsonValue());
                    levelProgresses.Add(temp);
                }
                
            }
        }

        public void GetLevelProgressDataCoroutineCaller()
        {
            StartCoroutine(GetLevelProgressData());
        }

        public float randomNumber()
        {
            float num = UnityEngine.Random.Range(0, 5) + 1;
            Debug.Log(num);
            return num;
        }

        private IEnumerator RandomAmountWaitTime()
        {
            SignOutLoadingPopup.SetActive(true);
            yield return new WaitForSeconds(randomNumber());
            SignOutLoadingPopup.SetActive(false);
        }

        private IEnumerator SignOut()
        {
            yield return StartCoroutine(RandomAmountWaitTime());
            homeScene.SetActive(false);
            titleLoginScene.SetActive(true);
            buttonLoginOnTitleScreen.SetActive(true);
            buttonRegisterOnTitleScreen.SetActive(true);
        }
        public void SignOutButtonClick()
        {
            StartCoroutine(SignOut());
            auth.SignOut();
        }

        public void InitializeAds()
        {
            #if UNITY_IOS
                gameId = IOSId;
            #elif UNITY_ANDROID
                gameId = androidId;
            #elif UNITY_EDITOR
                gameId = androidId;
            #endif

            if(!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Advertisement.Initialize(gameId, test, this);
            }
            
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads Initialization Complete");
            isInitialized = true;
            rewardedAdsButton.LoadAd();
        }
        public void OnInitializationFailed(UnityAdsInitializationError error,string message)
        {
            Debug.LogWarning($"Ads Initialization Failed with{error.ToString()} - {message}");
            isInitialized = false;
        }
    }

   
}
