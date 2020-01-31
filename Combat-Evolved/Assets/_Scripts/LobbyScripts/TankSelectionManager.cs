using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSelectionManager : MonoBehaviour
{
    public static TankSelectionManager instance = null;

    public List<GameObject> players = new List<GameObject>();
    [SerializeField] public Dictionary<string, string> controllersToPlayers =
        new Dictionary<string, string>();

    public GameObject referencePromptCube;

    private void Awake() 
    {
        if (!instance)
            instance = this;
        else {
            Destroy(this);
            return;
        }

        Transform[] playerArr = this.gameObject.GetComponentsInChildren<Transform>();

        for (int i = 0; i < playerArr.Length; i++) {
            if (playerArr[i].parent != this.gameObject.transform)
                continue;
            players.Add(playerArr[i].gameObject);
        }
    }

    public void AssignControllerToPlayer(ref int index, string controllerType) 
    {
        TankPlayerSelection tps = players[index].GetComponent<TankPlayerSelection>();

        if (index >= players.Count)
            return;

        string value = string.Empty;

        if(tps.currentController == string.Empty &&
            !controllersToPlayers.TryGetValue(controllerType, out value)) 
        {
            tps.currentController = controllerType;
            tps.isReady = !tps.isReady;
            controllersToPlayers.Add(controllerType, players[index].tag);
            Destroy(tps.promptCube);
        } else if (tps.currentController != controllerType) {
            index++;
            AssignControllerToPlayer(ref index, controllerType);
        } else if (tps.currentController == controllerType) {
            tps.currentController = string.Empty;
            tps.isReady = !tps.isReady;

            tps.promptCube = Instantiate(referencePromptCube, tps.promptCubePos,
                    referencePromptCube.transform.rotation, tps.transform);

            tps.promptCube.GetComponent<CubeRotate>().cooldown = 
                referencePromptCube.GetComponent<CubeRotate>().cooldown;

            controllersToPlayers.Remove(controllerType);
        }
    }
}
