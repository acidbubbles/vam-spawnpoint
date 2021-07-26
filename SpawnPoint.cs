using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MVRScript
{
    private JSONStorableBool _spawnOnEnable;

    public override void Init()
    {
        var spawnNow = new JSONStorableAction("Spawn Now", SpawnNow);
        RegisterAction(spawnNow);
        var btn = CreateButton("Spawn Now");
        spawnNow.dynamicButton = btn;

        _spawnOnEnable = new JSONStorableBool("Spawn On Enable", false);
        RegisterBool(_spawnOnEnable);
        CreateToggle(_spawnOnEnable, true);

        SuperController.singleton.BroadcastMessage("OnActionsProviderAvailable", this, SendMessageOptions.DontRequireReceiver);

        StartCoroutine(InitDeferred());
    }

    private IEnumerator InitDeferred()
    {
        yield return new WaitForEndOfFrame();
        if(_spawnOnEnable.val)
            SpawnNow();
    }

    private void SpawnNow()
    {
        var sc = SuperController.singleton;
        sc.ResetNavigationRigPositionRotation();

        var targetTransform = containingAtom.mainController.control;
        var navigationRigTransform = sc.navigationRig.transform;
        var targetRotation = targetTransform.eulerAngles;
        var targetPosition = targetTransform.position;
        var monitorCenterCameraTransform = sc.MonitorCenterCamera.transform;

        navigationRigTransform.eulerAngles = new Vector3(0, targetRotation.y, 0);
        monitorCenterCameraTransform.eulerAngles = targetRotation;
        monitorCenterCameraTransform.localEulerAngles = new Vector3(monitorCenterCameraTransform.localEulerAngles.x, 0, 0);
        sc.playerHeightAdjust += (targetPosition.y - sc.centerCameraTarget.transform.position.y);
        navigationRigTransform.position = new Vector3(targetPosition.x, 0, targetPosition.z);
    }

    public void OnBindingsListRequested(List<object> bindings)
    {
        bindings.Add(new JSONStorableAction("Spawn", SpawnNow));
    }

    public void OnDestroy()
    {
        SuperController.singleton.BroadcastMessage("OnActionsProviderDestroyed", this, SendMessageOptions.DontRequireReceiver);
    }
}
