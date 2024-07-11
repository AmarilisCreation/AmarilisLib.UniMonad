using AmarilisLib.Monad;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TryMonadExample : MonoBehaviour
{
    // TryMonad usage example 1 : Get the value of the key from Dictionary
    public async void Example1()
    {
        Dictionary<string, string> param = new Dictionary<string, string>() {
            {"param1", "value1"},
            {"param2", "value2"},
            {"param3", "value3"},
        };
        //success
        await param.TryGetValue("param1")
            .ExecuteAsync(
                value => Debug.Log(value),
                exception => Debug.LogException(exception));
        // If you want to execute only when successful, write like this
        //OptionValue(param, "param1").Execute(value => Debug.Log(value));

        //failed
        await param.TryGetValue("param4")
            .ExecuteAsync(
                value => Debug.Log(value),
                exception => Debug.LogException(exception));
    }

    // TryMonad usage example 2 : Works only when multiple TryMonad are all successful
    public async void Example2()
    {
        Dictionary<string, string> param = new Dictionary<string, string>() {
            {"param1", "value1"},
            {"param2", "value2"},
            {"param3", "value3"},
        };

        // if not use TryMonad
        if(param.ContainsKey("param1") && param.ContainsKey("param2") && param.ContainsKey("param3"))
        {
            var p1 = param["param1"];
            var p2 = param["param2"];
            var p3 = param["param3"];
            Debug.Log(p1 + " " + p2 + " " + p3);
        }
        else
        {
            Debug.Log("composedOption1 is None");
        }

        //success
        var composedOption1 = from p1 in param.TryGetValue("param1")
                              from p2 in param.TryGetValue("param2")
                              from p3 in param.TryGetValue("param3")
                              select p1 + " " + p2 + " " + p3;

        await composedOption1
            .ExecuteAsync(
                value => Debug.Log(value),
                exception => Debug.LogException(exception));

        //failed
        var composedOption2 = from p1 in param.TryGetValue("param1")
                              from p2 in param.TryGetValue("param2")
                              from p4 in param.TryGetValue("param4")   //param4 key does not exist in dictionary
                              select p1 + " " + p2 + " " + p4;
        await composedOption2
            .ExecuteAsync(
                value => Debug.Log(value),
                exception => Debug.LogException(exception));
    }

    // TryMonad usage example 3 : example 2 and Try ParseInt
    public async void Example3()
    {
        Dictionary<string, string> param = new Dictionary<string, string>() {
            {"param1", "1"},
            {"param2", "2"},
            {"param3", "3"},
            {"param4", "value4"},
        };
        //success
        var composedOption1 = from p1 in param.TryGetValue("param1")
                              from i1 in p1.TryParseInt()
                              from p2 in param.TryGetValue("param2")
                              from i2 in p2.TryParseInt()
                              from p3 in param.TryGetValue("param3")
                              from i3 in p3.TryParseInt()
                              select i1 + i2 + i3;
        await composedOption1
            .ExecuteAsync(
                value => Debug.Log(value),
                exception => Debug.LogException(exception));

        //failed
        var composedOption2 = from p1 in param.TryGetValue("param1")
                              from i1 in p1.TryParseInt()
                              from p2 in param.TryGetValue("param2")
                              from i2 in p2.TryParseInt()
                              from p4 in param.TryGetValue("param4")
                              from i4 in p4.TryParseInt()    //param4 key can not parse to int
                              select i1 + i2 + i4;
        await composedOption2
            .ExecuteAsync(
                value => Debug.Log(value),
                exception => Debug.LogException(exception));
    }

    // TryMonad usage example 4 : example 3 and Async
    public async void Example4()
    {
        Dictionary<string, string> param = new Dictionary<string, string>() {
            {"param1", "1"},
            {"param2", "2"},
            {"param3", "3"},
            {"param4", "value4"},
        };
        //success
        var composedOption1 = from p1 in param.TryGetValue("param1")
                              from i1 in p1.TryParseInt()
                              from p2 in param.TryGetValue("param2")
                              from i2 in p2.TryParseInt()
                              from p3 in param.TryGetValue("param3")
                              from i3 in p3.TryParseInt()
                              select i1 + i2 + i3;
        await composedOption1
            .Select(async v =>
            {
                await Task.Delay(1000);
                return v * 2;
            })
            .ExecuteAsync(
                value => Debug.Log(value),
                exception => Debug.LogException(exception));

        //failed
        var composedOption2 = from p1 in param.TryGetValue("param1")
                              from i1 in p1.TryParseInt()
                              from p2 in param.TryGetValue("param2")
                              from i2 in p2.TryParseInt()
                              from p4 in param.TryGetValue("param4")
                              from i4 in p4.TryParseInt()    //param4 key can not parse to int
                              select i1 + i2 + i4;
        await composedOption2
            .Select(async v =>
            {
                await Task.Delay(1000);
                return v * 2;
            })
            .ExecuteAsync(
                value => Debug.Log(value),
                exception => Debug.LogException(exception));
    }

    // TryMonad usage example 5 : Use SerializableTryProperty
    [SerializeField] SerializableTryProperty<int> _serializableTryProperty = new SerializableTryProperty<int>(10, "example", true);
    public async void Example5()
    {
        await _serializableTryProperty
            .ExecuteAsync(
                value => Debug.Log(value),
                exception => Debug.LogException(exception));
    }

    private void OnGUI()
    {
        if(GUILayout.Button("TryMonad usage example 1"))
        {
            Example1();
        }
        if(GUILayout.Button("TryMonad usage example 2"))
        {
            Example2();
        }
        if(GUILayout.Button("TryMonad usage example 3"))
        {
            Example3();
        }
        if(GUILayout.Button("TryMonad usage example 4"))
        {
            Example4();
        }
        if(GUILayout.Button("TryMonad usage example 5"))
        {
            Example5();
        }
    }
}
