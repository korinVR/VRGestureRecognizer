using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ScriptEngine : MonoBehaviour
{
    int cursor = 0;
    string[] lines;

    string[] commands = {
        "riftgesture",
        "delay",
        "goto"
    };

    Dictionary<string, int> labels;

    void Start()
    {
        TextAsset textAsset = Resources.Load("script") as TextAsset;
        lines = textAsset.text.Split('\n');

        labels = new Dictionary<string, int>();

        for (int i = 0; i < lines.Length; i++) {
            // Trim
            string line = lines[i].Trim();

            // Comment
            if (line.StartsWith(";")) {
                line = "";
            }

            // Label
            if (line.StartsWith("*")) {
                labels.Add(line.Substring(1), i);
                line = "";
            }
            lines[i] = line;
        }

        NextCommand();
    }

    string GetCommand(string line)
    {
        foreach (string command in commands) {
            if (line.StartsWith(command)) {
                return command;
            }
        }
        return "";
    }

    void NextCommand()
    {
        string command = GetCommand(lines[cursor]);
        switch (command) {
        case "riftgesture":
            var r = new Regex(@"^riftgesture\s+\*(\w+)\s*,\s*\*(\w+)$");
            var m = r.Match(lines[cursor]);

            if (m.Groups.Count == 3) {
                string yesLabel = m.Groups[1].ToString();
                string noLabel = m.Groups[2].ToString();
                WaitForYesNo(yesLabel, noLabel);
                return;
            }
            // Syntax error
            throw new Exception();

        case "delay":
            r = new Regex(@"^delay\s+(\d+)$");
            m = r.Match(lines[cursor]);

            if (m.Groups.Count == 2) {
                cursor++;
                int time = int.Parse(m.Groups[1].ToString());
                Invoke("NextCommand", time / 1000f);
                return;
            }
            // Syntax error
            throw new Exception();

        case "goto":
            r = new Regex(@"^goto\s+\*(\w+)$");
            m = r.Match(lines[cursor]);

            if (m.Groups.Count == 2) {
                string label = m.Groups[1].ToString();
                GoTo(label);
                return;
            }
            // Syntax error
            throw new Exception();
        }

        string message = "";

        while (GetCommand(lines[cursor]) == "") {
            string line = lines[cursor];
            if (line != "") {
                message += line + "\n";
            }
            cursor++;
        }
        WaitForMessage(message);
    }

    void GoTo(string label)
    {
        cursor = labels[label];
        NextCommand();
    }

    void WaitForYesNo(string yesLabel, string noLabel)
    {
        YesNoListener listener = GameObject.Find("Rift Gesture").AddComponent<YesNoListener>() as YesNoListener;
        listener.yesLabel = yesLabel;
        listener.noLabel = noLabel;
    }

    void WaitForClick()
    {
        UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(GameObject.Find("Rift Gesture"), "Assets/Script Example/ScriptEngine.cs (126,9)", "ClickListener");
    }

    void WaitForMessage(string message)
    {
        GameObject.Find("Message").SendMessage("StartMessage", message);
    }
}

