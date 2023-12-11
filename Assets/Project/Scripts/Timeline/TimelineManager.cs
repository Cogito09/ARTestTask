using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public CinemachineBrain MainBrain => GameMaster.MainCameraBrain;
    [SerializeField] public PlayableDirector _Director;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => GameMaster.IsGameLoaded);
        SetupBaseCamera();
    }

    private void SetupBaseCamera()
    {
        foreach (var playableAssetOutput in _Director.playableAsset.outputs)
        {
            if (playableAssetOutput.streamName == "Cinemachine Track")
            {
                _Director.SetGenericBinding(playableAssetOutput.sourceObject,MainBrain);
            }
        }
    }
}
    