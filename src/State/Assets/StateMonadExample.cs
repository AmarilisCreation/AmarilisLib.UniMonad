using AmarilisLib.Monad;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class StateMonadExample : MonoBehaviour
{
    // ReadOnly class that holds the number of calls
    public class SampleState
    {
        public int Count { private set; get; }
        public SampleState(int count)
        {
            Count = count;
        }
    }

    // Prepare a state that has never been called
    SampleState defaultState = new SampleState(0);

    // StateMonad usage example 1 : Generate StateMonad that changes the operation according to the state of the number of calls
    public async void Example1()
    {
        var checkCount = from currentState in State.Get<SampleState>()
                         from greeting in State.Return<SampleState, string>(currentState.Count == 0 ? "Nice to meet you" : "Hello")
                         from comment in State.Return<SampleState, string>($"{greeting}, Monad world!\nCount:{currentState.Count}")
                         from putAddCount in State.Put(new SampleState(currentState.Count + 1))
                         select comment;
        Debug.Log("Start");
        var resultState1 = await checkCount.ExecuteAsync(defaultState, value => Debug.Log(value));
        var resultState2 = await checkCount.ExecuteAsync(resultState1, value => Debug.Log(value));
        var resultState3 = await checkCount.ExecuteAsync(resultState2, value => Debug.Log(value));
        Debug.Log($"It has been called {resultState3.Count} times");
    }

    private void OnGUI()
    {
        if(GUILayout.Button("StateMonad usage example 1"))
        {
            Example1();
        }
    }
}