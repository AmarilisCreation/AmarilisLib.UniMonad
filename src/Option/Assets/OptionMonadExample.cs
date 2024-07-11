using AmarilisLib.Monad;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class OptionMonadExample : MonoBehaviour
{
    // OptionMonad usage example 1 : Get the value of the key from Dictionary
    public async void Example1()
    {
        Dictionary<string, string> param = new Dictionary<string, string>() {
            {"param1", "value1"},
            {"param2", "value2"},
            {"param3", "value3"},
        };
        //success
        await param.OptionalGetValue("param1")
            .ExecuteAsync(
                value => Debug.Log(value),
                () => Debug.Log("param1 key does not exist in dictionary"));
        // If you want to execute only when successful, write like this
        //OptionValue(param, "param1").Execute(value => Debug.Log(value));

        //failed
        await param.OptionalGetValue("param4")
            .ExecuteAsync(
                value => Debug.Log(value),
                () => Debug.Log("param4 key does not exist in dictionary"));
    }

    // OptionMonad usage example 2 : Works only when multiple OptionMonad are all successful
    public async void Example2()
    {
        Dictionary<string, string> param = new Dictionary<string, string>() {
            {"param1", "value1"},
            {"param2", "value2"},
            {"param3", "value3"},
        };

        // if not use OptionMonad
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
        var composedOption1 = from p1 in param.OptionalGetValue("param1")
                              from p2 in param.OptionalGetValue("param2")
                              from p3 in param.OptionalGetValue("param3")
                              select p1 + " " + p2 + " " + p3;

        await composedOption1
            .ExecuteAsync(
                value => Debug.Log(value),
                () => Debug.Log("composedOption1 is None"));

        //failed
        var composedOption2 = from p1 in param.OptionalGetValue("param1")
                              from p2 in param.OptionalGetValue("param2")
                              from p4 in param.OptionalGetValue("param4")   //param4 key does not exist in dictionary
                              select p1 + " " + p2 + " " + p4;
        await composedOption2
            .ExecuteAsync(
                value => Debug.Log(value),
                () => Debug.Log("composedOption2 is None"));
    }

    // OptionMonad usage example 3 : example 2 and Try ParseInt
    public async void Example3()
    {
        Dictionary<string, string> param = new Dictionary<string, string>() {
            {"param1", "1"},
            {"param2", "2"},
            {"param3", "3"},
            {"param4", "value4"},
        };
        //success
        var composedOption1 = from p1 in param.OptionalGetValue("param1")
                              from i1 in p1.OptionalParseInt()
                              from p2 in param.OptionalGetValue("param2")
                              from i2 in p2.OptionalParseInt()
                              from p3 in param.OptionalGetValue("param3")
                              from i3 in p3.OptionalParseInt()
                              select i1 + i2 + i3;
        await composedOption1
            .ExecuteAsync(
                value => Debug.Log(value),
                () => Debug.Log("composedOption1 is None"));

        //failed
        var composedOption2 = from p1 in param.OptionalGetValue("param1")
                              from i1 in p1.OptionalParseInt()
                              from p2 in param.OptionalGetValue("param2")
                              from i2 in p2.OptionalParseInt()
                              from p4 in param.OptionalGetValue("param4")
                              from i4 in p4.OptionalParseInt()    //param4 key can not parse to int
                              select i1 + i2 + i4;
        await composedOption2
            .ExecuteAsync(
                value => Debug.Log(value),
                () => Debug.Log("composedOption2 is None"));
    }

    // OptionMonad usage example 4 : example 3 and Async
    public async void Example4()
    {
        Dictionary<string, string> param = new Dictionary<string, string>() {
            {"param1", "1"},
            {"param2", "2"},
            {"param3", "3"},
            {"param4", "value4"},
        };
        //success
        var composedOption1 = from p1 in param.OptionalGetValue("param1")
                              from i1 in p1.OptionalParseInt()
                              from p2 in param.OptionalGetValue("param2")
                              from i2 in p2.OptionalParseInt()
                              from p3 in param.OptionalGetValue("param3")
                              from i3 in p3.OptionalParseInt()
                              select i1 + i2 + i3;
        await composedOption1
            .Select(async v =>
            {
                await Task.Delay(1000);
                return v * 2;
            })
            .ExecuteAsync(
                value => Debug.Log(value),
                () => Debug.Log("composedOption1 is None"));

        //failed
        var composedOption2 = from p1 in param.OptionalGetValue("param1")
                              from i1 in p1.OptionalParseInt()
                              from p2 in param.OptionalGetValue("param2")
                              from i2 in p2.OptionalParseInt()
                              from p4 in param.OptionalGetValue("param4")
                              from i4 in p4.OptionalParseInt()    //param4 key can not parse to int
                              select i1 + i2 + i4;
        await composedOption2
            .Select(async v =>
            {
                await Task.Delay(1000);
                return v * 2;
            })
            .ExecuteAsync(
                value => Debug.Log(value),
                () => Debug.Log("composedOption2 is None"));
    }

    // OptionMonad usage example 5 : Use SerializableOptionProperty
    [SerializeField] SerializableOptionProperty<int> _serializableOptionProperty;
    public async void Example5()
    {
        await _serializableOptionProperty
            .ExecuteAsync(
                value => Debug.Log(value),
                () => Debug.Log("_serializableOptionProperty is None"));
    }

    private void OnGUI()
    {
        if(GUILayout.Button("OptionMonad usage example 1"))
        {
            Example1();
        }
        if(GUILayout.Button("OptionMonad usage example 2"))
        {
            Example2();
        }
        if(GUILayout.Button("OptionMonad usage example 3"))
        {
            Example3();
        }
        if(GUILayout.Button("OptionMonad usage example 4"))
        {
            Example4();
        }
        if(GUILayout.Button("OptionMonad usage example 5"))
        {
            Example5();
        }
    }
}
