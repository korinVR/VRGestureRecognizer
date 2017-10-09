using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FrameSynthesis.VR;

namespace ScriptExample
{
    public class ScriptEngine : MonoBehaviour
    {
        public bool IsYesNoWaiting
        {
            get
            {
                return state == State.YesNoWaiting;
            }
        }

        public event Action<string> ShowMessageHandler;

        int cursor = 0;
        string[] lines;

        string[] commands = {
            "gesture",
            "delay",
            "goto"
        };

        public enum State
        {
            Normal,
            YesNoWaiting,
        }

        State state;

        string yesLabel;
        string noLabel;

        Dictionary<string, int> labels = new Dictionary<string, int>();

        void Start()
        {
            var textAsset = Resources.Load("script") as TextAsset;
            lines = textAsset.text.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                // Trim
                string line = lines[i].Trim();

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
                        state = State.YesNoWaiting;
                        return;
                    }
                    // Syntax error
                    throw new Exception();

                case "delay":
                    r = new Regex(@"^delay\s+(\d+)$");
                    m = r.Match(lines[cursor]);

                    if (m.Groups.Count == 2)
                    {
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

                    if (m.Groups.Count == 2)
                    {
                        string label = m.Groups[1].ToString();
                        GoTo(label);
                        return;
                    }
                    // Syntax error
                    throw new Exception();
            }

            string message = "";

            while (GetCommand(lines[cursor]) == "")
            {
                string line = lines[cursor];
                if (line != "")
                {
                    message += line + "\n";
                }
                cursor++;
            }

            if (ShowMessageHandler != null) { ShowMessageHandler.Invoke(message); }
        }

        public void GoTo(string label)
        {
            cursor = labels[label];
            NextCommand();
        }

        public void AnswerYes()
        {
            if (state == State.YesNoWaiting)
            {
                state = State.Normal;
                GoTo(yesLabel);
            }
        }

        public void AnswerNo()
        {
            if (state == State.YesNoWaiting)
            {
                state = State.Normal;
                GoTo(noLabel);
            }
        }
    }
}
