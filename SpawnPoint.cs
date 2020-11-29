using System.Collections;
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
        var root = containingAtom.mainController;
        var cameraTransform = SuperController.singleton.centerCameraTarget.transform;
        var navigationRigTransform = SuperController.singleton.navigationRig.transform;

        var targetRotation = root.control.eulerAngles;
        var cameraRotation = cameraTransform.rotation;
        var rigRotation = navigationRigTransform.eulerAngles;
        navigationRigTransform.eulerAngles = new Vector3(rigRotation.x, targetRotation.y + (cameraRotation.x - rigRotation.x), rigRotation.z);

        var targetPosition = root.control.position;
        var cameraPosition = cameraTransform.position;
        var rigPosition = navigationRigTransform.position;
        navigationRigTransform.position = new Vector3(targetPosition.x - (cameraPosition.x - rigPosition.x), rigPosition.y, targetPosition.z - (cameraPosition.z - rigPosition.z));
    }
}
