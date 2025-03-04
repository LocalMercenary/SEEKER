using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Saving : MonoBehaviour
{
    string password = "casdrASJAFj23";

    CharacterController characterController;
    RaycastScript raycastScript;
    EnemyAi enemy;
    Puzzle2 puzzle2;

    public GameObject object1, object2, object3, object4; // Reference the 4 objects you need to save
    public Animator animator1, animator2, animator3, animator4, animator5;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        raycastScript = FindObjectOfType<RaycastScript>(); // Ensure this is assigned
        puzzle2 = FindObjectOfType<Puzzle2>();
        enemy = FindEnemyEvenIfDisabled(); // Find enemy even if it's disabled

        if (raycastScript == null)
        {
            Debug.LogError("RaycastScript not found in the scene!");
        }
        if (puzzle2 == null)
        {
            Debug.LogError("Puzzle2 not found in the scene!");
        }
        if (enemy == null)
        {
            Debug.LogError("EnemyAI script not found!");
        }
    }

    private EnemyAi FindEnemyEvenIfDisabled()
    {
        EnemyAi[] allEnemies = Resources.FindObjectsOfTypeAll<EnemyAi>();
        return allEnemies.Length > 0 ? allEnemies[0] : null;
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            save();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            load();
        }*/
    }

    public void save()
    {
        SaveData myData = new SaveData();

        // Save player position
        myData.playerX = transform.position.x;
        myData.playerY = transform.position.y;
        myData.playerZ = transform.position.z;

        // Save enemy position and state
        if (enemy != null)
        {
            myData.enemyX = enemy.transform.position.x;
            myData.enemyY = enemy.transform.position.y;
            myData.enemyZ = enemy.transform.position.z;
            myData.enemyActive = enemy.gameObject.activeInHierarchy;
            myData.enemyChasing = enemy.canSeePlayer;
            myData.enemyWandering = enemy.wander;
            myData.enemyReturningHome = enemy.sendHome;
        }

        // Save collectibles and enemy spawn state
        if (raycastScript != null)
        {
            myData.hasCollectedItem1 = raycastScript.hasCollectedItem1;
            myData.hasCollectedItem2 = raycastScript.hasCollectedItem2;
            myData.hasCollectedItem3 = raycastScript.hasCollectedItem3;
            myData.hasCollectedItem4 = raycastScript.hasCollectedItem4;
            myData.hasSpawned = raycastScript.hasSpawned;
        }

        // Save Puzzle2 states
        if (puzzle2 != null)
        {
            myData.Rotated1 = puzzle2.Rotated1;
            myData.Rotated2 = puzzle2.Rotated2;
            myData.Rotated3 = puzzle2.Rotated3;
            myData.Rotated4 = puzzle2.Rotated4;
            myData.FullyRotated = puzzle2.FullyRotated;
            myData.EnemyDead = puzzle2.EnemyDead;
        }

        // Save object active states
        myData.obj1Active = object1.activeInHierarchy;
        myData.obj2Active = object2.activeInHierarchy;
        myData.obj3Active = object3.activeInHierarchy;
        myData.obj4Active = object4.activeInHierarchy;

        // Save animator states
        myData.animator1State = GetCurrentAnimationName(animator1);
        myData.animator2State = GetCurrentAnimationName(animator2);
        myData.animator3State = GetCurrentAnimationName(animator3);
        myData.animator4State = GetCurrentAnimationName(animator4);
        myData.animator5State = GetCurrentAnimationName(animator5);

        myData.animator1Finished = AnimatorHasEnded(animator1);
        myData.animator2Finished = AnimatorHasEnded(animator2);
        myData.animator3Finished = AnimatorHasEnded(animator3);
        myData.animator4Finished = AnimatorHasEnded(animator4);
        myData.animator5Finished = AnimatorHasEnded(animator5);

        // Encrypt and save
        string myDataString = JsonUtility.ToJson(myData);
        myDataString = EncryptDecryptData(myDataString);
        string file = Application.persistentDataPath + "/" + gameObject.name + ".json";
        File.WriteAllText(file, myDataString);
    }

    public void load()
    {
        string file = Application.persistentDataPath + "/" + gameObject.name + ".json";
        if (File.Exists(file))
        {
            string jsonData = File.ReadAllText(file);
            jsonData = EncryptDecryptData(jsonData);
            SaveData myData = JsonUtility.FromJson<SaveData>(jsonData);

            // Load player position
            characterController.enabled = false;
            transform.position = new Vector3(myData.playerX, myData.playerY, myData.playerZ);
            characterController.enabled = true;

            // Load enemy position and state
            if (enemy != null)
            {
                enemy.transform.position = new Vector3(myData.enemyX, myData.enemyY, myData.enemyZ);
                enemy.gameObject.SetActive(myData.enemyActive);
                enemy.canSeePlayer = myData.enemyChasing;
                enemy.wander = myData.enemyWandering;
                enemy.sendHome = myData.enemyReturningHome;
            }

            // Load collectibles
            if (raycastScript != null)
            {
                raycastScript.hasCollectedItem1 = myData.hasCollectedItem1;
                raycastScript.hasCollectedItem2 = myData.hasCollectedItem2;
                raycastScript.hasCollectedItem3 = myData.hasCollectedItem3;
                raycastScript.hasCollectedItem4 = myData.hasCollectedItem4;
                raycastScript.hasSpawned = myData.hasSpawned;
            }

            // Load Puzzle2 states
            if (puzzle2 != null)
            {
                puzzle2.Rotated1 = myData.Rotated1;
                puzzle2.Rotated2 = myData.Rotated2;
                puzzle2.Rotated3 = myData.Rotated3;
                puzzle2.Rotated4 = myData.Rotated4;
                puzzle2.FullyRotated = myData.FullyRotated;
                puzzle2.EnemyDead = myData.EnemyDead;
            }

            // Load object active states
            if (object1 != null) object1.SetActive(myData.obj1Active);
            if (object2 != null) object2.SetActive(myData.obj2Active);
            if (object3 != null) object3.SetActive(myData.obj3Active);
            if (object4 != null) object4.SetActive(myData.obj4Active);

            // Restore animator states
            RestoreAnimationState(animator1, myData.animator1State, myData.animator1Finished);
            RestoreAnimationState(animator2, myData.animator2State, myData.animator2Finished);
            RestoreAnimationState(animator3, myData.animator3State, myData.animator3Finished);
            RestoreAnimationState(animator4, myData.animator4State, myData.animator4Finished);
            RestoreAnimationState(animator5, myData.animator5State, myData.animator5Finished);
        }
    }

    private string GetCurrentAnimationName(Animator animator)
    {
        if (animator == null) return "";
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return animator.GetCurrentAnimatorClipInfo(0).Length > 0 ? animator.GetCurrentAnimatorClipInfo(0)[0].clip.name : "";
    }

    private bool AnimatorHasEnded(Animator animator)
    {
        if (animator == null) return false;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.normalizedTime >= 1f && !animator.IsInTransition(0);
    }

    private void RestoreAnimationState(Animator animator, string animationName, bool finished)
    {
        if (animator == null || string.IsNullOrEmpty(animationName)) return;
        animator.Play(animationName);
        if (finished)
        {
            animator.Update(1f);
        }
    }

    public string EncryptDecryptData(string data)
    {
        string result = "";
        for (int i = 0; i < data.Length; i++)
        {
            result += (char)(data[i] ^ password[i % password.Length]);
        }
        return result;
    }
}

[System.Serializable]
public class SaveData
{
    public float playerX, playerY, playerZ;
    public float enemyX, enemyY, enemyZ;
    public bool enemyActive;
    public bool enemyChasing;
    public bool enemyWandering;
    public bool enemyReturningHome;

    // Collectibles
    public bool hasCollectedItem1;
    public bool hasCollectedItem2;
    public bool hasCollectedItem3;
    public bool hasCollectedItem4;
    public bool hasSpawned;

    // Object active states
    public bool obj1Active;
    public bool obj2Active;
    public bool obj3Active;
    public bool obj4Active;

    // Puzzle2 Bools
    public bool Rotated1;
    public bool Rotated2;
    public bool Rotated3;
    public bool Rotated4;
    public bool FullyRotated;
    public bool EnemyDead;

    // Animator States
    public string animator1State;
    public string animator2State;
    public string animator3State;
    public string animator4State;
    public string animator5State;

    public bool animator1Finished;
    public bool animator2Finished;
    public bool animator3Finished;
    public bool animator4Finished;
    public bool animator5Finished;
}
