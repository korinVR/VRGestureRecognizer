using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ScriptExample
{
    public class ScriptEngine : MonoBehaviour
    {
        public bool IsWaitingYesNo => state == State.WaitingYesNo;

        public event Action<string> ShowMessageHandler;

        int cursor;
        string[] lines;

        readonly string[] commands = {
            "gesture",
            "delay",
            "goto"
        };

        public enum State
        {
            Normal,
            WaitingYesNo,
        }

        State state;

        string yesLabel;
        string noLabel;

        readonly Dictionary<string, int> labels = new Dictionary<string, int>();

        void Start()
        {
            var textAsset = Resources.Load("script") as TextAsset;
            lines = textAsset.text.Split('\n');

            for (var i = 0; i < lines.Length; i++)
            {
                // Trim
                var line = lines[i].Trim();

                // Comment
                if (line.StartsWith(";"))
                {
                    line = "";
                }

                // Label
                if (line.StartsWith("*"))
                {
                    labels.Add(line.Substring(1), i);
                    line = "";
                }
                lines[i] = line;
            }

            state = State.Normal;

            NextCommand();
        }

        string GetCommand(string line)
        {
            foreach (var command in commands)
            {
                if (line.StartsWith(command))
                {
                    return command;
                }
            }
            return "";
        }

        public void NextCommand()
        {
            var command = GetCommand(lines[cursor]);
            switch (command)
            {
                case "gesture":
                    var r = new Regex(@"^gesture\s+\*(\w+)\s*,\s*\*(\w+)$");
                    var m = r.Match(lines[cursor]);

                    if (m.Groups.Count == 3)
                    {
                        yesLabel = m.Groups[1].ToString();
                        noLabel = m.Groups[2].ToString();
                        state = State.WaitingYesNo;
                        return;
                    }
                    throw new Exception("Syntax error");

                case "delay":
                    r = new Regex(@"^delay\s+(\d+)$");
                    m = r.Match(lines[cursor]);

                    if (m.Groups.Count == 2)
                    {
                        cursor++;
                        var time = int.Parse(m.Groups[1].ToString());
                        Invoke(nameof(NextCommand), time / 1000f);
                        return;
                    }
                    throw new Exception("Syntax error");

                case "goto":
                    r = new Regex(@"^goto\s+\*(\w+)$");
                    m = r.Match(lines[cursor]);

                    if (m.Groups.Count == 2)
                    {
                        var label = m.Groups[1].ToString();
                        GoTo(label);
                        return;
                    }
                    throw new Exception("Syntax error");
            }

            var message = "";

            while (GetCommand(lines[cursor]) == "")
            {
                string line = lines[cursor];
                if (line != "")
                {
                    message += line + "\n";
                }
                cursor++;
            }

            ShowMessageHandler?.Invoke(message);
        }

        public void GoTo(string label)
        {
            cursor = labels[label];
            NextCommand();
        }

        public void AnswerYes()
        {
            if (state == State.WaitingYesNo)
            {
                state = State.Normal;
                GoTo(yesLabel);
            }
        }

        public void AnswerNo()
        {
            if (state == State.WaitingYesNo)
            {
                state = State.Normal;
                GoTo(noLabel);
            }
        }
    }
}
