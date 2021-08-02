using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MVRScript
{
    private Atom _containingAtom;
    private JSONStorableBool _spawnOnEnable;
    private JSONStorableBool _isSpawnPointHost;

    public override void Init()
    {
        _containingAtom = containingAtom;

        var spawnNow = new JSONStorableAction("Spawn Now", SpawnNow);
        RegisterAction(spawnNow);
        var btn = CreateButton("Spawn Now");
        spawnNow.dynamicButton = btn;

        _spawnOnEnable = new JSONStorableBool("Spawn On Enable", false);
        RegisterBool(_spawnOnEnable);
        CreateToggle(_spawnOnEnable, true);

        SuperController.singleton.BroadcastMessage("OnActionsProviderAvailable", this, SendMessageOptions.DontRequireReceiver);

        StartCoroutine(InitDeferred());

        if (enabled)
            OnEnable();
    }

    private IEnumerator InitDeferred()
    {
        yield return new WaitForEndOfFrame();
        if (!enabled) yield break;
        if(_spawnOnEnable.val)
            SpawnNow();
        yield return 0;
        if (!enabled) yield break;
        if(_spawnOnEnable.val)
            SpawnNow();
    }

    private void OnEnable()
    {
        if (_containingAtom == null) return;
        if (_containingAtom.IsBoolJSONParam("IsSpawnPointHost")) return;
        _isSpawnPointHost = new JSONStorableBool("IsSpawnPointHost", true);
        _containingAtom.RegisterBool(_isSpawnPointHost);
    }

    private void OnDisable()
    {
        if (_isSpawnPointHost == null) return;
        _containingAtom.DeregisterBool(_isSpawnPointHost);
        _isSpawnPointHost = null;
    }

    private void SpawnNow()
    {
        var sc = SuperController.singleton;
        sc.ResetNavigationRigPositionRotation();

        var navigationRigTransform = sc.navigationRig.transform;

        var targetTransform = containingAtom.mainController.control;

        var targetRotation = targetTransform.eulerAngles;
        navigationRigTransform.eulerAngles = new Vector3(0, targetRotation.y, 0);

        var targetPosition = targetTransform.position;
        var centerCameraPosition = sc.centerCameraTarget.transform.position;
        var teleportPosition = targetPosition + (navigationRigTransform.position - centerCameraPosition);

        navigationRigTransform.position = new Vector3(teleportPosition.x, 0, teleportPosition.z);
        sc.playerHeightAdjust += (targetPosition.y - centerCameraPosition.y);

        var monitorCenterCameraTransform = sc.MonitorCenterCamera.transform;
        monitorCenterCameraTransform.eulerAngles = targetRotation;
        monitorCenterCameraTransform.localEulerAngles = new Vector3(monitorCenterCameraTransform.localEulerAngles.x, 0, 0);
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
