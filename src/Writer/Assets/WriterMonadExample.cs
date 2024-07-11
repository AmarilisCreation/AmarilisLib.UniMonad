using AmarilisLib.Monad;
using UnityEngine;

public class WriterMonadExample : MonoBehaviour
{
    // WriterMonad usage example 1 : Perform calculations while keeping the log in WriterMonad, and display the log
    public async void Example1()
    {
        var writer = from num1 in Writer.Tell(2, "num1 = 2")
                     from num2 in Writer.Tell(5, "num2 = 5")
                     from num3 in Writer.Tell(10, "num3 = 10")
                     from result in Writer.Return<string, int>(num1 * num2 + num3)
                     from resultLog in Writer.Tell($"num1 * num2 + num3 = {result}")
                     select result;
        await writer.ExecuteAsync(
            value => Debug.Log(value),
            outPut => {
                foreach(var log in outPut)
                {
                    Debug.Log($"Log[{log}]");
                }
            });
    }

    private void OnGUI()
    {
        if(GUILayout.Button("WriterMonad usage example 1"))
        {
            Example1();
        }
    }
}