using AmarilisLib.Monad;
using System;
using UnityEngine;

public class ReaderMonadExample : MonoBehaviour
{
    // ReaderMonad usage example 1 : Generate ReaderMonad to convert DateTime generated from different environment to the same format string
    public async void Example1()
    {
        var getDateString = from date in Reader.Ask<DateTime>()
                            select date.ToString("yyyy/MM/dd HH:mm:ss");

        await getDateString.ExecuteAsync(
            new DateTime(2000, 1, 1, 10, 20, 30),
            value => Debug.Log(value));
        await getDateString.ExecuteAsync(
            DateTime.UtcNow,
            value => Debug.Log(value));
    }

    private void OnGUI()
    {
        if(GUILayout.Button("ReaderMonad usage example 1"))
        {
            Example1();
        }
    }
}