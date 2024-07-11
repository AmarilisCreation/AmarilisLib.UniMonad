using System;
using System.Threading;
using AmarilisLib.Monad;
using UnityEngine;

public class RWSExample : MonoBehaviour
{
    // ReadOnly class that holds the number of calls
    public class SampleState {
        public int Count { private set; get; }
        public SampleState(int count) {
            Count = count;
        }
    }

    // Prepare a state that has never been called
    SampleState defaultState = new SampleState(0);

    // RWSMonad usage example 1 : Realize the contents performed in each Example 1 of WriterMonad, ReaderMonad, and StateMonad with one RWSMonad
    public async void Example1() {
        var checkCount = from currentState in RWS.Get<DateTime, string, SampleState>()
            from date in RWS.Ask<DateTime, string, SampleState>()
            from _ in RWS.Tell<DateTime, string, SampleState>(date.ToString("yyyy/MM/dd HH:mm:ss"))
            from greeting in RWS.Tell<DateTime, string, SampleState, string>(currentState.Count == 0 ? "Nice to meet you" : "Hello", $"currentState.Count = {currentState.Count}")
            from comment in RWS.Return<DateTime, string, SampleState, string>($"{greeting}, Monad world!")
            from __ in RWS.Tell<DateTime, string, SampleState>("Add count")
            from putAddCount in RWS.Put<DateTime, string, SampleState>(new SampleState(currentState.Count + 1))
            select comment;

        var result1State = await checkCount.ExecuteAsync(
            new DateTime(2000, 1, 1, 10, 20, 30),
            defaultState,
            value => Debug.Log(value),
            outPut => {
                foreach(var log in outPut) {
                    Debug.Log($"Log[{log}]");
                }
            });

        var result2State = await checkCount.ExecuteAsync(
            DateTime.UtcNow,
            result1State,
            value => Debug.Log(value),
            outPut => {
                foreach(var log in outPut) {
                    Debug.Log($"Log[{log}]");
                }
            });

        Debug.Log($"It has been called {result2State.Count} times");
    }

    private void OnGUI() {
        if(GUILayout.Button("RWSMonad usage example 1")) {
            Example1();
        }
    }
}