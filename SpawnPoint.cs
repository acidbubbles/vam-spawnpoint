using UnityEngine;

public class SpawnPoint : MVRScript
{
    private JSONStorableBool _spawnOnEnable;

    public override void Init()
    {
        var spawnNow = new JSONStorableAction("SpawnNow", SpawnNow);
        RegisterAction(spawnNow);

        _spawnOnEnable = new JSONStorableBool("SpawnOnEnable", false);
        RegisterBool(_spawnOnEnable);
    }

    public void OnEnable()
    {
        if(_spawnOnEnable.val)
            SpawnNow();
    }

    private void SpawnNow()
    {
        var root = containingAtom.mainController;
        var navigationRigTransform = SuperController.singleton.navigationRig.transform;
        var targetPosition = root.control.position;
        var rigPosition = navigationRigTransform.position;
        var targetRotation = root.control.eulerAngles;
        var rigRotation = navigationRigTransform.eulerAngles;

        navigationRigTransform.position = new Vector3(targetPosition.x, rigPosition.y, targetPosition.z);
        navigationRigTransform.eulerAngles = new Vector3(rigRotation.x, targetRotation.y, rigRotation.z);
    }
}
