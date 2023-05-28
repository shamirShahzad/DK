using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System;
using Object = System.Object;

namespace DK
{
    public class FirebaseManager : MonoBehaviour
    {
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
        //public GameObject networkErrorPopup;
        public GameObject registerPopup;
        public GameObject loginPopup;
        [Header("Buttons")]
        public GameObject buttonRegisterOnTitleScreen;
        public GameObject buttonLoginOnTitleScreen;
        [Header("Scenes Panels")]
        public GameObject homeScene;
        public GameObject titleLoginScene;
        [Header("Firebase Database")]
        public CharacterSaveData userData = new CharacterSaveData();
        public ItemsSaveData itemData = new ItemsSaveData();
        public DatabaseReference reference;

        [SerializeField] GameObject loadingPopup;
        [SerializeField] TextMeshProUGUI textForLoadingTitle;
        [SerializeField] TextMeshProUGUI usernameTextinHomeScene;

        public static FirebaseManager instance;

        Dictionary<string, Object> levels = new Dictionary<string, Object>();
        Dictionary<string, Object> equipment = new Dictionary<string, Object>();

        [Header("Leaderboard UI")]
        public LeaderBoard myLeaderBoardData = new LeaderBoard();
        public LeaderBoard userLeaderBoard = new LeaderBoard();

        public List<LeaderBoard> leaderBoardList = new();
        public TextMeshProUGUI[] nameInLeaderboard= new TextMeshProUGUI[10];
        public TextMeshProUGUI[] deathsInLeaderboard= new TextMeshProUGUI[10];

        public TextMeshProUGUI userNameinLeaderboard;
        public TextMeshProUGUI userDeathsinLeaderboard;
        public TextMeshProUGUI userRankinLeaderboard;
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

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    InitializeFireBase();
                }
                else
                {
                    //networkErrorPopup.SetActive(true);
                    Debug.LogError("Could Not Resolve all Firebase Dependencies:" + dependencyStatus);
                }
                Debug.Log(reference);
            });
        }


        private void InitializeFireBase()
        {
            auth = FirebaseAuth.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;

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

        //public void SaveDataToFirebase(int level, int hIndex, int tIndex, int aIndex, int lIndex, int cIIndex, int lWIndex, int rWindex,
        //                               int healthLevel, int staminaLevel, int focusLevel, int strengthLevel, int dexterityLevel, int poiseLevel,
        //                               int intlligenceLevel, int faithLevel, int souls, int gold, int levelCompleted)
        //{
        //    userData.characterLevel = level;
        //    userData.helmetIndex = hIndex;
        //    userData.torsoIndex = tIndex;
        //    userData.armIndex = aIndex;
        //    userData.hipIndex = lIndex;
        //    userData.consumableItemIndex = cIIndex;
        //    userData.leftArmWeapon = lWIndex;
        //    userData.rightArmWeapon = rWindex;
        //    userData.healthLevel = healthLevel;
        //    userData.staminaLevel = staminaLevel;
        //    userData.focusLevel = focusLevel;
        //    userData.strengthLevel = strengthLevel;
        //    userData.dexterityLevel = dexterityLevel;
        //    userData.poiseLevel = poiseLevel;
        //    userData.intelligenceLevel = intlligenceLevel;
        //    userData.faithLevel = faithLevel;
        //    userData.soulPlayersPosseses = souls;
        //    userData.goldAmount = gold;
        //    userData.levelsCompleted = levelCompleted;

        //    string json = JsonUtility.ToJson(userData);

        //    reference.Child("Users").Child(User.UserId).SetRawJsonValueAsync(json);


        //}

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
                userData.levelsCompleted = Convert.ToInt32(snapshot.Child("levelsCompleted").Value);
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
                Debug.Log(snapshot.Child("helmetPurchased").Value);

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
    }
}
