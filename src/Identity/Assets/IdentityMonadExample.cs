using AmarilisLib.Monad;
using System.IO;
using UnityEngine;

public class IdentityMonadExample : MonoBehaviour
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
    // IdentityMonad usage example 1 : IdentityMonad is simple and does nothing on its own
    public async void Example1()
    {
        var identity = from value1 in Identity.Return(1)
                       from value2 in Identity.Return(2)
                       from value3 in Identity.Return(3)
                       select value1 + value2 + value3;
        await identity.ExecuteAsync(value => Debug.Log(value));
    }
    // IdentityMonad usage example 2 : Converting from IOManad to OptionMonad via IdentityMonad
    public async void Example2(IIOMonad<string> ioFilePath)
    {
        var identityFilePath = await ioFilePath
            .ToIdentityAsync();
        var optionLoadText = await identityFilePath
            .Select(OptionalLoadText)
            .ToOptionAsync();
        await optionLoadText
            .ExecuteAsync(
                value => Debug.Log(value),
                () => Debug.Log($"file not found"));
    }

    string _example2TextFieldInput = "Assets/Resources/TestText.txt";
    private void OnGUI()
    {
        if(GUILayout.Button("IdentityMonad usage example 1"))
        {
            Example1();
        }
        GUILayout.BeginHorizontal();
        _example2TextFieldInput = GUILayout.TextField(_example2TextFieldInput, GUILayout.Width(300));
        if(GUILayout.Button("IdentityMonad usage example 2"))
        {
            Example2(IO.Return(_example2TextFieldInput));
        }
        GUILayout.EndHorizontal();
    }
}