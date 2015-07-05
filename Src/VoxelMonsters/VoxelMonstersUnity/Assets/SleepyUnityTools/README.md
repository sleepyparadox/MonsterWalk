Prototyping kit for sleep deprived jammers

## Resource Loading

```c#
var myObj = new UnityObject(Assets.MyPrefabPrefab);
```
Prefab is automatically cached in the Assets.MyPrefabPrefab PrefabAsset. 
So you don't have to worry about it loading twice when using this conscuctor multiple times

Use the menu item "SleepyParadox/Generate Assets.cs"
This will generate an Assets.cs in your assets directory

```c#
///
/// This is a generated code file
/// Expect to lose any changes you make
///using Sleepy.UnityTools;
public class Assets
{
   public readonly static PrefabAsset MyPrefabPrefab = new PrefabAsset(@"MyPrefab");
   public static Asset[] GetAssets() { return new Asset []{ MyPrefabPrefab }; }
}
```

## MonoBehavour exposed as c# events

```c#
var myObj = new UnityObject(Assets.MyPrefabPrefab);
myObj.UnityUpdate += (u) => myObj.WorldPosition = Vector3.up * Mathf.Sin(Time.time);
```
## TinyCoro

```c#
public class Example : MonoBehaviour
{
    void Update()
    {
        //Step every frame
        TinyCoro.StepAllCoros();
    }

    void Awake()
    {
        var key = KeyCode.Space;

        var promptObj = new UnityObject("PressKeyPrompt");
        promptObj.UnityGUI += (u) => GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Press " + key);

        TinyCoro.SpawnNext(() => DoKillSecondAfterKeyPress(promptObj, key));
    }

    IEnumerator DoKillSecondAfterKeyPress(UnityObject obj, KeyCode key)
    {
        // Wait until key pressed 
        // (delegate will be checked every frame)
        yield return TinyCoro.WaitUntil(() => Input.GetKeyUp(key));

        obj.UnityGUI = null;
        obj.UnityGUI += (u) => GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Will die");

        // Wait 1 second
        yield return TinyCoro.Wait(1f);

        obj.Dispose();
    }
}
```
Be sure to call StepAllCoros every frame
SpawnNext will queue new TinyCoro after the current
If not TinyCoro is running, SpawnNext will add new TinyCoro to end of list