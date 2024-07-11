using AmarilisLib.Monad;
using System;
using System.IO;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class IOMonadExample : MonoBehaviour
{
    private string LoadText(string filePath)
    {
        using(StreamReader streamReader = new StreamReader(filePath))
        {
            return streamReader.ReadToEnd();
        }
    }
    private IOptionMonad<string> OptionalLoadText(string filePath)
    {
        try
        {
            return Option.Return(LoadText(filePath));
        }
        catch(FileNotFoundException)
        {
            return Option.None<string>();
        }
    }

    // IOMonad usage example 1 : Generate IOMonad to convert DateTime generated from current UTC time to string.
    public async void Example1(IIOMonad<DateTime> ioDateTime)
    {
        var getDateString = ioDateTime
            .Select(v => v.ToString("yyyy/MM/dd HH:mm:ss"))
            .Do(async v => await Task.Delay(1000));
        await getDateString.ExecuteAsync(value => Debug.Log(value));
        await getDateString.ExecuteAsync(value => Debug.Log(value));
    }

    // IOMonad usage example 2 : Reads user-entered file
    public async void Example2(IIOMonad<string> ioFilePath)
    {
        var getString = ioFilePath.Select(LoadText);
        await getString.ExecuteAsync(value => Debug.Log(value));
    }

    // IOMonad usage example 3 : Along with OptionMonad
    public async void Example3(IIOMonad<string> ioFilePath)
    {
        var getOptionalString = ioFilePath.Select(OptionalLoadText);
        await getOptionalString.ExecuteAsync(option =>
        {
            option
                .ExecuteAsync(
                    value => Debug.Log(value),
                    () => Debug.Log("file not found"));
        });

    }

    string _example2TextFieldInput = "Assets/Resources/TestText.txt";
    string _example3TextFieldInput = "Assets/Resources/TestTextNotthing.txt";
    private void OnGUI()
    {
        if(GUILayout.Button("IOMonad usage example 1"))
        {
            Example1(IO.Create(() => DateTime.UtcNow));
        }

        GUILayout.BeginHorizontal();
        _example2TextFieldInput = GUILayout.TextField(_example2TextFieldInput, GUILayout.Width(300));
        if(GUILayout.Button("IOMonad usage example 2"))
        {
            Example2(IO.Return(_example2TextFieldInput));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        _example3TextFieldInput = GUILayout.TextField(_example3TextFieldInput, GUILayout.Width(300));
        if(GUILayout.Button("IOMonad usage example 3"))
        {
            Example3(IO.Return(_example3TextFieldInput));
        }
        GUILayout.EndHorizontal();
    }
}