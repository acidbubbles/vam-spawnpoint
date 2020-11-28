using System;

public class SpawnPoint : MVRScript
{
    public override void Init()
    {
        try
        {
            SuperController.LogMessage($"{nameof(SpawnPoint)} initialized");
        }
        catch (Exception e)
        {
            SuperController.LogError($"{nameof(SpawnPoint)}.{nameof(Init)}: {e}");
        }
    }

    public void OnEnable()
    {
        try
        {
            SuperController.LogMessage($"{nameof(SpawnPoint)} enabled");
        }
        catch (Exception e)
        {
            SuperController.LogError($"{nameof(SpawnPoint)}.{nameof(OnEnable)}: {e}");
        }
    }

    public void OnDisable()
    {
        try
        {
            SuperController.LogMessage($"{nameof(SpawnPoint)} disabled");
        }
        catch (Exception e)
        {
            SuperController.LogError($"{nameof(SpawnPoint)}.{nameof(OnDisable)}: {e}");
        }
    }

    public void OnDestroy()
    {
        try
        {
            SuperController.LogMessage($"{nameof(SpawnPoint)} destroyed");
        }
        catch (Exception e)
        {
            SuperController.LogError($"{nameof(SpawnPoint)}.{nameof(OnDestroy)}: {e}");
        }
    }
}
