using AmarilisLib.Monad;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EitherMonadExample : MonoBehaviour
{
    // This time, I'm doing it this way to make the Left an Exception in the sample.
    private static Exception GetValueLeftSelector<TKey>(TKey key)
        => new Exception($"[{key.ToString()}] key does not exist in dictionary");
    private static Exception ParseIntLeftSelector(string parseTarget)
        => new Exception($"[{parseTarget}] Failed to parse to int type");

    // EitherMonad usage example 1 : Get the value of the key from Dictionary
    public async void Example1()
    {
        Dictionary<string, string> param = new Dictionary<string, string>() {
            {"param1", "value1"},
            {"param2", "value2"},
            {"param3", "value3"},
        };
        //success
        await param.GetValueOrLeft("param1", GetValueLeftSelector)
            .ExecuteAsync(
                right => Debug.Log(right),
                left => Debug.LogException(left));

        //failed
        await param.GetValueOrLeft("param4", GetValueLeftSelector)
            .ExecuteAsync(
                right => Debug.Log(right),
                left => Debug.LogException(left));
    }

    // EitherMonad usage example 2 : Works only when multiple EitherMonad are all successful
    public async void Example2()
    {
        Dictionary<string, string> param = new Dictionary<string, string>() {
            {"param1", "value1"},
            {"param2", "value2"},
            {"param3", "value3"},
        };

        // if not use EitherMonad
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
        var composedOption1 = from p1 in param.GetValueOrLeft("param1", GetValueLeftSelector)
                              from p2 in param.GetValueOrLeft("param2", GetValueLeftSelector)
                              from p3 in param.GetValueOrLeft("param3", GetValueLeftSelector)
                              select p1 + " " + p2 + " " + p3;

        await composedOption1
            .ExecuteAsync(
                right => Debug.Log(right),
                left => Debug.LogException(left));

        //failed
        var composedOption2 = from p1 in param.GetValueOrLeft("param1", GetValueLeftSelector)
                              from p2 in param.GetValueOrLeft("param2", GetValueLeftSelector)
                              from p4 in param.GetValueOrLeft("param4", GetValueLeftSelector)   //param4 key does not exist in dictionary
                              select p1 + " " + p2 + " " + p4;
        await composedOption2
            .ExecuteAsync(
                right => Debug.Log(right),
                left => Debug.LogException(left));
    }

    // EitherMonad usage example 3 : example 2 and Try ParseInt
    public async void Example3()
    {
        Dictionary<string, string> param = new Dictionary<string, string>() {
            {"param1", "1"},
            {"param2", "2"},
            {"param3", "3"},
            {"param4", "value4"},
        };
        //success
        var composedOption1 = from p1 in param.GetValueOrLeft("param1", GetValueLeftSelector)
                              from i1 in p1.ParseIntOrLeft(ParseIntLeftSelector)
                              from p2 in param.GetValueOrLeft("param2", GetValueLeftSelector)
                              from i2 in p2.ParseIntOrLeft(ParseIntLeftSelector)
                              from p3 in param.GetValueOrLeft("param3", GetValueLeftSelector)
                              from i3 in p3.ParseIntOrLeft(ParseIntLeftSelector)
                              select i1 + i2 + i3;
        await composedOption1
            .ExecuteAsync(
                right => Debug.Log(right),
                left => Debug.LogException(left));

        //failed
        var composedOption2 = from p1 in param.GetValueOrLeft("param1", GetValueLeftSelector)
                              from i1 in p1.ParseIntOrLeft(ParseIntLeftSelector)
                              from p2 in param.GetValueOrLeft("param2", GetValueLeftSelector)
                              from i2 in p2.ParseIntOrLeft(ParseIntLeftSelector)
                              from p4 in param.GetValueOrLeft("param4", GetValueLeftSelector)
                              from i4 in p4.ParseIntOrLeft(ParseIntLeftSelector)    //param4 key can not parse to int
                              select i1 + i2 + i4;
        await composedOption2
            .ExecuteAsync(
                right => Debug.Log(right),
                left => Debug.LogException(left));
    }

    // EitherMonad usage example 4 : example 3 and Async
    public async void Example4()
    {
        Dictionary<string, string> param = new Dictionary<string, string>() {
            {"param1", "1"},
            {"param2", "2"},
            {"param3", "3"},
            {"param4", "value4"},
        };
        //success
        var composedOption1 = from p1 in param.GetValueOrLeft("param1", GetValueLeftSelector)
                              from i1 in p1.ParseIntOrLeft(ParseIntLeftSelector)
                              from p2 in param.GetValueOrLeft("param2", GetValueLeftSelector)
                              from i2 in p2.ParseIntOrLeft(ParseIntLeftSelector)
                              from p3 in param.GetValueOrLeft("param3", GetValueLeftSelector)
                              from i3 in p3.ParseIntOrLeft(ParseIntLeftSelector)
                              select i1 + i2 + i3;
        await composedOption1
            .Select(async v =>
            {
                await Task.Delay(1000);
                return v * 2;
            })
            .ExecuteAsync(
                right => Debug.Log(right),
                left => Debug.LogException(left));

        //failed
        var composedOption2 = from p1 in param.GetValueOrLeft("param1", GetValueLeftSelector)
                              from i1 in p1.ParseIntOrLeft(ParseIntLeftSelector)
                              from p2 in param.GetValueOrLeft("param2", GetValueLeftSelector)
                              from i2 in p2.ParseIntOrLeft(ParseIntLeftSelector)
                              from p4 in param.GetValueOrLeft("param4", GetValueLeftSelector)
                              from i4 in p4.ParseIntOrLeft(ParseIntLeftSelector)    //param4 key can not parse to int
                              select i1 + i2 + i4;
        await composedOption2
            .Select(async v =>
            {
                await Task.Delay(1000);
                return v * 2;
            })
            .ExecuteAsync(
                right => Debug.Log(right),
                left => Debug.LogException(left));
    }

    // EitherMonad usage example 5 : Use SerializableOptionProperty
    [SerializeField]
    SerializableEitherProperty<string, Vector3> _eitherTest = new SerializableEitherProperty<string, Vector3>("this is left", new Vector3(), true);
    public void Example5()
    {
        _eitherTest
            .ExecuteAsync(
                right => Debug.Log(right),
                left => Debug.LogException(new Exception(left)));
    }

    private void OnGUI()
    {
        if(GUILayout.Button("EitherMonad usage example 1"))
        {
            Example1();
        }
        if(GUILayout.Button("EitherMonad usage example 2"))
        {
            Example2();
        }
        if(GUILayout.Button("EitherMonad usage example 3"))
        {
            Example3();
        }
        if(GUILayout.Button("EitherMonad usage example 4"))
        {
            Example4();
        }
        if(GUILayout.Button("EitherMonad usage example 5"))
        {
            Example5();
        }
    }
}
